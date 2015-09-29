using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CloverExamplePOS
{
    public class POSOrder
    {
        
        public enum OrderStatus
        {
            OPEN, CLOSED, LOCKED
        }


        public POSOrder()
        {
            Status = OrderStatus.OPEN;
            Items = new List<POSLineItem>();
            Payments = new List<POSExchange>();
            Date = new DateTime();
        }

        public string ID { set; get; }
        public DateTime Date { set; get; }
        public OrderStatus Status { set; get; }

        public long SubTotal {
            get
            {
                long sub = 0;
                foreach(POSLineItem li in Items)
                {
                    sub += li.Price * li.Quantity ;
                }
                return sub;
            }
        }
        public long TaxableSubtotal
        {
            get
            {
                long sub = 0;
                foreach (POSLineItem li in Items)
                {
                    if(li.Item.Taxable)
                    {
                        sub += li.Price * li.Quantity;
                    }
                }
                return sub;
            }
        }
        public long TaxAmount {
            get
            {
                return (int)(TaxableSubtotal * 0.07);
            }
        }
        public long Total
        {
            get
            {
                return SubTotal + TaxAmount;
            }
        }
        public long Tips()
        {
            long tips = 0;
            foreach(POSPayment posPayment in Payments)
            {
                tips += posPayment.TipAmount;
            }
            return tips;
        }

        public List<POSLineItem> Items { get; }
        public List<POSExchange> Payments { get; }

        /// <summary>
        /// manageds adding a POSItem to an order. If the POSItem already exists, the quantity is just incremented
        /// </summary>
        /// <param name="i"></param>
        /// <param name="quantity"></param>
        /// <returns>The POSLineItem for the POSItem. Will either return a new one, or an exising with its quantity incremented</returns>
        public POSLineItem AddItem(POSItem i, int quantity)
        {
            bool exists = false;
            POSLineItem targetItem = null;
            foreach(POSLineItem lineI in Items)
            {
                if(lineI.Item.ID == i.ID)
                {
                    exists = true;
                    lineI.Quantity += quantity;
                    targetItem = lineI;
                    break;
                }
            }
            if(!exists)
            {
                POSLineItem li = new POSLineItem();
                li.Quantity = quantity;
                li.Item = i;
                targetItem = li;
                Items.Add(li);
            }
            return targetItem;
        }

        public void AddPayment(POSPayment payment)
        {
            Payments.Add(payment);
        }

        public void AddRefund(POSRefund refund)
        {
            foreach(POSExchange pay in Payments)
            {
                if(pay is POSPayment)
                {
                    if (pay.PaymentID == refund.PaymentID)
                    {
                        ((POSPayment)pay).PaymentStatus = POSPayment.Status.REFUNDED;
                    }

                }
            }
            Payments.Add(refund);
        }

        internal void RemoveItem(POSLineItem selectedLineItem)
        {
            Items.Remove(selectedLineItem);
        }
    }

    public class POSExchange
    {
        public POSExchange(string paymentID, string orderID, string employeeID, long amount)
        {
            PaymentID = paymentID;
            OrderID = orderID;
            EmployeeID = employeeID;
            Amount = amount;
        }

        public string PaymentID { get; set; }
        public string OrderID { get; set; }
        public string EmployeeID { get; set; }
        public long Amount { get; set; }
    }

    public class POSRefund : POSExchange
    {
        public POSRefund(string paymentID, string orderID, string employeeID, long amount) : base(paymentID, orderID, employeeID, amount)
        {
            
        }
    }

    public class POSNakedRefund
    {
        public string EmployeeID { get; }
        public long Amount { get; }

        public POSNakedRefund(String employeeID, long amount)
        {
            EmployeeID = employeeID;
            Amount = amount;
        }
    }

    public class POSPayment : POSExchange
    {
        public enum Status
        {
            PAID, VOIDED, REFUNDED
        }
        public POSPayment(string paymentID, string orderID, string employeeID, long amount, long tip = 0) : base(paymentID, orderID, employeeID, amount)
        {
            TipAmount = tip;
        }

        private Status _status;

        public Status PaymentStatus
        {
            get
            {
                return _status;
            }
            set
            {
                if(_status == Status.PAID)
                {
                    _status = value;
                }
            }
        }
        public bool Voided {
            get
            {
                return _status == Status.VOIDED;
            }
        }
        public bool Refunded
        {
            get
            {
                return _status == Status.REFUNDED;
            }
        }

        public long TipAmount { get; set; }
    }

    public class POSLineItem
    {
        public POSLineItem()
        {
            Quantity = 1;
            ID = Guid.NewGuid().ToString();
        }

        public POSLineItemDiscount Discount { get; set; }
        public string ID { get; internal set; }
        public POSItem Item
        {
            set; get;
        }
        public long Price {
            get
            {
                if(Discount != null)
                {
                    return Item.Price - Discount.Value(Item);
                }
                else
                {
                    return Item.Price;
                }
            }
        }
        public int Quantity
        {
            set; get;
        }

        public static implicit operator POSLineItem(ListViewItem v)
        {
            throw new NotImplementedException();
        }
    }

    public class POSItem
    {
        public POSItem(string id, string name, long price, bool taxable = true)
        {
            ID = id;
            Name = name;
            Price = price;
            Taxable = taxable;
        }
        public bool Taxable { get; set; } = true;
        public string ID { get; set; }
        public long Price { get; set; }
        public String Name { get; set; }
    }

    public class POSDiscount
    {
        public string Name { get; set; } = "";

        private long _amountOff = 0;
        public long AmountOff
        {
            get
            {
                return _amountOff;
            }
            set
            {
                _percentageOff = 0.0f;
                _amountOff = value;
            }
        }
        private float _percentageOff = 0.0f;
        public float PercentageOff
        {
            get
            {
                return _percentageOff;
            }
            set
            {
                _amountOff = 0;
                _percentageOff = value;
            }
        }
    }

    public class POSOrderDiscount : POSDiscount
    {
        public POSOrderDiscount(long fixedDiscountAmount, string name)
        {
            AmountOff = fixedDiscountAmount;
            Name = name;
        }
        public POSOrderDiscount(float percentageOff, string name)
        {
            PercentageOff = percentageOff;
            Name = name;
        }

        public long Value(POSOrder order)
        {
            if (AmountOff > 0)
            {
                return AmountOff;
            }
            else
            {
                return (int)(order.SubTotal * PercentageOff);
            }
        }

    }

    public class POSLineItemDiscount : POSDiscount
    {
        public POSLineItemDiscount(long fixedDiscountAmount, string name)
        {
            AmountOff = fixedDiscountAmount;
            Name = name;
        }
        public POSLineItemDiscount(float percentageOff, string name)
        {
            PercentageOff = percentageOff;
            Name = name;
        }

        public long Value(POSItem item)
        {
            if(AmountOff > 0)
            {
                return AmountOff;
            }
            else
            {
                return (int)(item.Price * PercentageOff);
            }
        }
    }

    public class Store
    {
        private static int orderNumber = 1000;

        public Store()
        {
            AvailableItems = new List<POSItem>();
            Orders = new List<POSOrder>();
        }
        public List<POSItem> AvailableItems { set; get; }
        public List<POSOrder> Orders { set; get; }
        public POSOrder CurrentOrder { set; get; }

        public void CreateOrder()
        {
            POSOrder order = new POSOrder();
            order.ID = "" + (++orderNumber);
            CurrentOrder = order;
            Orders.Add(order);
        }
    }
}
