// Copyright (C) 2018 Clover Network, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
//
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CloverExamplePOS
{
    public class POSOrder
    {
        
        public enum OrderStatus
        {
            PENDING, OPEN, CLOSED, LOCKED, AUTHORIZED, PARTIALLY_REFUNDED, PARTIALLY_PAID
        }
        public enum OrderChangeTarget
        {
            ORDER, ITEM, PAYMENT
        }

        public delegate void OrderDataChangeHandler(POSOrder order, OrderChangeTarget target);
        public event OrderDataChangeHandler OrderChange;
        protected void onOrderChange(POSOrder order, OrderChangeTarget target)
        {
            if (order.status == OrderStatus.PENDING &&
                order.Items.Count > 0)
            {
                order.status = OrderStatus.OPEN;
            }
            if (OrderChange != null)
            {
                OrderChange(order, target);
            }
        }

        public POSOrder()
        {
            Status = OrderStatus.PENDING;
            Items = new List<POSLineItem>();
            Payments = new List<POSExchange>();
            Discount = new POSDiscount("None", 0);
            Date = new DateTime();
        }

        public List<POSLineItem> Items { get; internal set; }
        public List<POSExchange> Payments { get; internal set; }
        public POSDiscount Discount
        {
            get { return discount; }
            set
            {
                if (discount != value)
                {
                    discount = value;
                    onOrderChange(this, OrderChangeTarget.ORDER);
                }
            }
        }
        private OrderStatus status;
        private POSDiscount discount;
        public string ID { set; get; }
        public DateTime Date { set; get; }
        public OrderStatus Status {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    onOrderChange(this, OrderChangeTarget.ORDER);
                }
            }
        }

        public void AddOrderLineItem(POSLineItem item)
        {
            this.Items.Add(item);
            onOrderChange(this, OrderChangeTarget.ITEM);
        }

        public void AddOrderPayment(POSPayment payment)
        {
            this.Payments.Add(payment);
            onOrderChange(this, OrderChangeTarget.PAYMENT);
        }
        public void ModifyTipAmount(String paymentID, long? amount)
        {
            foreach(POSExchange paymentObject in Payments)
            {
                if (paymentObject is POSPayment payment && payment.PaymentID == paymentID)
                {
                    payment.TipAmount = amount ?? 0;
                    onOrderChange(this, OrderChangeTarget.PAYMENT);
                }
            }
        }
        public void ModifyPaymentStatus(string paymentID, POSPayment.Status status)
        {
            foreach (POSExchange paymentObject in Payments)
            {
                if (paymentObject is POSPayment payment && payment.PaymentID == paymentID)
                {
                    payment.PaymentStatus = status;
                    onOrderChange(this, OrderChangeTarget.PAYMENT);
                }
            }

        }

        public long PreDiscountSubTotal
        {
            get
            {
                long sub = 0;
                foreach (POSLineItem li in Items)
                {
                    sub += li.Price * li.Quantity;
                }
                return sub;
            }
        }
        public long PreTaxSubTotal {
            get
            {
                long sub = 0;
                foreach(POSLineItem li in Items)
                {
                    sub += li.Price * li.Quantity ;
                }
                if(Discount != null)
                {
                    sub = Discount.AppliedTo(sub);
                }
                return sub;
            }
        }
        public long TippableAmount
        {
            get
            {
                long tippableAmount = 0;
                foreach(POSLineItem li in Items)
                {
                    if (li.Item.Tippable)
                    {
                        tippableAmount += li.Price * li.Quantity;
                    }
                }
                if (Discount != null)
                {
                    tippableAmount = Discount.AppliedTo(tippableAmount);
                }
                return tippableAmount + TaxAmount; // should match Total if there aren't any "non-tippable" items
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
                if (Discount != null)
                {
                    sub = Discount.AppliedTo(sub);
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
                return PreTaxSubTotal + TaxAmount;
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


        /// <summary>
        /// manages adding a POSItem to an order. If the POSItem already exists, the quantity is just incremented
        /// </summary>
        /// <param name="i"></param>
        /// <param name="quantity"></param>
        /// <returns>The POSLineItem for the POSItem. Will either return a new one, or an existing with its quantity incremented</returns>
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
            onOrderChange(this, OrderChangeTarget.ITEM);
            return targetItem;
        }

        public void AddPayment(POSPayment payment)
        {
            Payments.Add(payment);
            onOrderChange(this, OrderChangeTarget.PAYMENT);
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
            onOrderChange(this, OrderChangeTarget.PAYMENT);
        }

        internal void RemoveItem(POSLineItem selectedLineItem)
        {
            Items.Remove(selectedLineItem);
            onOrderChange(this, OrderChangeTarget.ITEM);
        }

    }

    public class POSCard
    {
        public string Name { get; set; }
        public string First6 { get; set; }
        public string Last4 { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Token { get; set; }
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
        public POSRefund(string refundID, string paymentID, string orderID, string employeeID, long amount) : base(paymentID, orderID, employeeID, amount)
        {
            RefundID = refundID;
        }
        public string RefundID { get; set; }
    }

    public class POSManualRefund 
    {
        public POSManualRefund(string creditID, string orderID, long amount)
        {
            CreditID = creditID;
            OrderID = orderID;
            Amount = amount;
        }
        public string CreditID { get; set; }
        public string OrderID { get; set; }
        public long Amount { get; set; }
    }

    public class POSPayment : POSExchange
    {
        public String ExternalID { get; set; }
        public enum Status
        {
            PAID, VOIDED, REFUNDED, AUTHORIZED
        }
        public POSPayment(string paymentID, string externalID, string orderID, string employeeID, long amount, long tip = 0, long cashBack = 0) : base(paymentID, orderID, employeeID, amount)
        {
            TipAmount = tip;
            CashBackAmount = cashBack;
            OrderID = orderID;
            EmployeeID = employeeID;
            ExternalID = externalID;
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
                if(_status != value)
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
        public long CashBackAmount { get; set; }
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
        public POSItem(string id, string name, long price, bool taxable = true, bool tippable = true)
        {
            ID = id;
            Name = name;
            Price = price;
            Taxable = taxable;
            Tippable = tippable;
        }
        public bool Tippable { get; internal set; }
        public bool Taxable { get; set; }
        public string ID { get; set; }
        public long Price { get; set; }
        public String Name { get; set; }
        
    }

    public class POSDiscount
    {
        public POSDiscount()
        {
            Name = "";
        }
        public POSDiscount(string name, float percentOff) : this()
        {
            Name = name;
            PercentageOff = percentOff;
        }
        public POSDiscount(string name, long amountOff) : this()
        {
            Name = name;
            AmountOff = amountOff;
        }
        public string Name { get; set; }

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

        internal long AppliedTo(long sub)
        {
            if(AmountOff == 0)
            {
                sub = (long)Math.Round(sub - (sub * PercentageOff));
            }
            else
            {
                sub -= AmountOff;
            }
            return Math.Max(sub, 0);
        }

        public long Value(long sub)
        {
            long value = AmountOff;
            if (AmountOff == 0)
            {
                value = (long)Math.Round(sub * PercentageOff);
            }

            return value;
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
                return (int)(order.PreTaxSubTotal * PercentageOff);
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
            AvailableDiscounts = new List<POSDiscount>();
            Orders = new List<POSOrder>();
            NewDiscount = false;
            Cards = new List<POSCard>();
            PreAuths = new List<POSPayment>();
        }

        public List<POSCard> Cards { set; get; }
        public List<POSPayment> PreAuths { set; get; }
        public List<POSItem> AvailableItems { set; get; }
        public List<POSDiscount> AvailableDiscounts { set; get; }
        public List<POSOrder> Orders { set; get; }
        public POSOrder CurrentOrder { set; get; }
        public Boolean NewDiscount { set; get; }

        public enum OrderListAction
        {
            ADDED, REMOVED, UPDATED
        }

        public enum PreAuthAction
        {
            ADDED, REMOVED
        }


        public delegate void OrderListChangeHandler(Store store, OrderListAction action);
        public event OrderListChangeHandler OrderListChange;
        public delegate void PreAuthListChangeHandler(POSPayment payment, PreAuthAction action);
        public event PreAuthListChangeHandler PreAuthListChange;
        protected void onOrderListChange(Store store, OrderListAction action)
        {
            if (OrderListChange != null)
            {
                OrderListChange(store, action);
            }
        }
        public void CreateOrder()
        {
            //Get rid of any prior pending orders, before creating a new one
            DeletePendingOrder();
            POSOrder order = new POSOrder();
            order.ID = "" + (++orderNumber);
            CurrentOrder = order;
            AddOrder(order);
        }
        protected void OnPreAuthChanged(POSPayment payment, PreAuthAction action)
        {
            if (PreAuthListChange != null)
            {
                PreAuthListChange(payment, action);
            }
        }

        /*  This will remove any other PENDING order before creating a new one. */
        private void DeletePendingOrder()
        {
            POSOrder delOrder = null;
            foreach (POSOrder order in Orders)
            {
                if (order.Status == POSOrder.OrderStatus.PENDING)
                {
                    delOrder = order;
                    break;
                }
            }
            if (delOrder != null)
            {
                Orders.Remove(delOrder); //This shouldn't trigger a onOrderListChange, as PENDING orders aren't displayed
                delOrder = null;
            }
        }
        public void AddPreAuth(POSPayment payment)
        {
            PreAuths.Add(payment);
            OnPreAuthChanged(payment, PreAuthAction.ADDED);
        }
        public void RemovePreAuth(POSPayment payment)
        {
            if(PreAuths.Remove(payment))
            {
                OnPreAuthChanged(payment, PreAuthAction.REMOVED);
            }
        }
        public void AddOrder(POSOrder order)
        {
            Orders.Add(order);
            onOrderListChange(this, OrderListAction.ADDED);
        }
        public POSOrder GetOrder(String paymentID)
        {
            foreach (POSOrder order in Orders)
            {
                foreach (Object payment in order.Payments) // payments can be POSPayment or POSRefund
                {
                    if (payment is POSPayment && ((POSPayment)payment).PaymentID == paymentID)
                    {
                        return order;
                    }
                }
            }
            return null;
        }

    }
}
