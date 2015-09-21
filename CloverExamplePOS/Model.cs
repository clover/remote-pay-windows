using System;
using System.Collections.Generic;

namespace CloverExamplePOS
{
    public class POSOrder
    {
        
        public enum OrderStatus
        {
            OPEN, CLOSED, LOCKED
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
                    sub += li.Item.Price * li.Quantity ;
                }
                return sub;
            }
        }
        public long TipAmount { set; get; }
        public long TaxAmount {
            get
            {
                return (int)(SubTotal * 0.07);
            }
        }
        public long Total
        {
            get
            {
                return SubTotal + TaxAmount;
            }
        }

        public List<POSLineItem> Items { get; }
        public List<POSExchange> Payments { get; }


        public POSOrder()
        {
            Status = OrderStatus.OPEN;
            Items = new List<POSLineItem>();
            Payments = new List<POSExchange>();
            Date = new DateTime();
        }

        public void AddItem(POSItem i, int quantity)
        {
            bool exists = false;
            foreach(POSLineItem lineI in Items)
            {
                if(lineI.Item.ID == i.ID)
                {
                    exists = true;
                    lineI.Quantity += quantity;
                    break;
                }
            }
            if(!exists)
            {
                POSLineItem li = new POSLineItem();
                li.Quantity = quantity;
                li.Item = i;

                Items.Add(li);
            }
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
        public POSPayment(string paymentID, string orderID, string employeeID, long amount) : base(paymentID, orderID, employeeID, amount)
        {

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

    }

    public class POSLineItem
    {
        public POSLineItem()
        {
            Quantity = 1;
        }

        public POSItem Item
        {
            set; get;
        }
        public int Quantity
        {
            set; get;
        }


    }

    public class POSItem
    {
        public POSItem(string id, string name, long price)
        {
            ID = id;
            Name = name;
            Price = price;
        }
        public string ID { get; set; }
        public long Price { get; set; }
        public String Name { get; set; }
    }

    public class Store
    {
        private static int orderNumber = 1000;

        public Store()
        {
            AvailableItems = new List<POSItem>();
            Orders = new List<POSOrder>();
            CreateOrder();
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
