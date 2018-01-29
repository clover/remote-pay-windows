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
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using com.clover.remotepay.sdk;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections;

namespace com.clover.remote.order
{

    public class DisplayDiscount
    {
        public string id { get; set; }
        public string lineItemId { get; set; }
        public string name { get; set; }
        public string amount { get; set; }
        public string percentage { get; set; }

        public DisplayDiscount() { }
    }

    public class DisplayLineItem
    {
        public string id { get; set; }
        public string orderId { get; set; }
        public string name { get; set; }
        public string alternateName { get; set; }
        /**
       * Formatted unit quantity - such as 10 @ $1.99/oz
       */
        public string price { get; set; }
        /**
       * Formatted unit price in cases if applicable
       */
        public string unitPrice { get; set; }
        /**
       * Formatted quantity
       */
        public string quantity { get; set; }
        /**
       * Formatted unit quantity - such as 10 @ $1.99/oz
       */
        public string unitQuantity { get; set; }
        public string note { get; set; }
        public Boolean printed { get; set; }
        public string binName { get; set; }
        public string userData { get; set; }
        public ListWrapper<DisplayDiscount> discounts { get; set; }
        public string discountAmount { get; set; }
        public Boolean exchanged { get; set; }
        public string exchangedAmount { get; set; }
        public ListWrapper<DisplayModification> modifications { get; set; }
        public Boolean refunded { get; set; }
        public string refundedAmount { get; set; }
        public string percent { get; set; }

        public DisplayLineItem()
        {
            discounts = new ListWrapper<DisplayDiscount>();
            modifications = new ListWrapper<DisplayModification>();
        }

        public void addDiscount(DisplayDiscount discount)
        {
            this.discounts.Add(discount);
        }
        public void removeDiscount(DisplayDiscount discount)
        {
            this.discounts.Remove(discount);
        }
        public void addDisplayModification(DisplayModification mod)
        {
            this.modifications.Add(mod);
        }
        public void removeDisplayModification(DisplayModification mod)
        {
            this.modifications.Remove(mod);
        }

    }

    public class DisplayModification
    {
        public string id { get; set; }
        public string name { get; set; }
        public string amount { get; set; }

        public DisplayModification() { }
    }

    public class DisplayOrder
    {
        /*Unique id*/
        public string id { get; set; }
        public string currency { get; set; }
        public string employee { get; set; }
        /**
       * Formatted subtotal of the order
       */
        public string subtotal { get; set; }
        /**
       * Formatted tax of the order
       */
        public string tax { get; set; }
        /**
       * Formatted total of the order
       */
        public string total { get; set; }
        public string title { get; set; }
        public string note { get; set; }
        /**
       * Optional service charge name (gratuity) applied to this order
       */
        public string serviceChargeName { get; set; }
        /**
       * Optional service charge amount (gratuity) applied to this order
       */
        public string serviceChargeAmount { get; set; }

        public ListWrapper<DisplayDiscount> discounts { get; set; }
        public ListWrapper<DisplayLineItem> lineItems { get; set; }
        /**
       * Formatted amount remaining
       */
        public string amountRemaining { get; set; }
        public ListWrapper<DisplayPayment> payments { get; set; }

        public DisplayOrder()
        {
            discounts = new ListWrapper<DisplayDiscount>();
            lineItems = new ListWrapper<DisplayLineItem>();
            payments = new ListWrapper<DisplayPayment>();
        }

        public void addDisplayPayment(DisplayPayment payment)
        {
            this.payments.Add(payment);
        }

        public void removeDisplayPayment(DisplayPayment payment)
        {
            this.payments.Remove(payment);
        }

        public void addDisplayLineItem(DisplayLineItem lineItem)
        {
            this.lineItems.addElement(lineItem);
        }

        public void removeDisplayLineItem(DisplayLineItem lineItem)
        {
            this.lineItems.removeElement(lineItem);
        }

        public void addDisplayDiscount(DisplayDiscount discount)
        {
            this.discounts.addElement(discount);
        }
        public void removeDisplayDiscount(DisplayDiscount discount)
        {
            this.discounts.removeElement(discount);
        }

    }

    public class DisplayPayment
    {
        public string id { get; set; }
       /*
        * Formatted display string for the tender e.g. credit card, cash, etc.
        */
        public string label { get; set; }
        public string amount { get; set; }
        public string tipAmount { get; set; }
        public string taxAmount { get; set; }
        public DisplayPayment() { }
    }

    public class DisplayFactory
    {
        public static DisplayOrder createDisplayOrder()
        {
            String guid = Guid.NewGuid().ToString();
            DisplayOrder dispOrder = new DisplayOrder();
            dispOrder.id = guid;
            return dispOrder;
        }

        public static DisplayPayment createDisplayPayment()
        {
            String guid = Guid.NewGuid().ToString();
            DisplayPayment dispPayment = new DisplayPayment();
            dispPayment.id = guid;
            return dispPayment;
        }

        public static DisplayDiscount createDisplayDiscount()
        {
            String guid = Guid.NewGuid().ToString();
            DisplayDiscount dispDisc = new DisplayDiscount();
            dispDisc.id = guid;
            return dispDisc;
        }

        public static DisplayLineItem createDisplayLineItem()
        {
            String guid = Guid.NewGuid().ToString();
            DisplayLineItem dispItem = new DisplayLineItem();
            dispItem.id = guid;
            return dispItem;
        }
    }

    public class ListWrapper<T>
    {
        public List<T> elements { get; set; }

        public ListWrapper()
        {
            elements = new List<T>();
        }

        public void Add(T obj)
        {
            this.addElement(obj);
        }
        public void addElement(T obj)
        {
            this.elements.Add(obj);
        }

        public void Remove(T obj)
        {
            this.removeElement(obj);
        }
        public void removeElement(T obj)
        {
            this.elements.Remove(obj);
        }

    }

}
