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
using System.Collections;

namespace com.clover.remote.order.operation
{

    public class DisplayOperation { }
    public class OrderDeletedOperation : DisplayOperation
    {
        public string id { get; set; }
        public OrderDeletedOperation() { }
        public OrderDeletedOperation(string id)
        {
            this.id = id;
        }
    }

    public class LineItemsDeletedOperation : DisplayOperation
    {
        public ListWrapper<string> ids { get; set; }
        public string orderId { get; set; }

        public LineItemsDeletedOperation() 
        {
            ids = new ListWrapper<string>();
            orderId = null;
        }

        public LineItemsDeletedOperation(string orderId)
        {
            this.orderId = orderId;
        }

        public void addId(string id)
        {
            this.ids.Add(id);
        }
        public void removeId(string id)
        {
            this.ids.Remove(id);
        }
    }

    public class LineItemsAddedOperation : DisplayOperation
    {
        public ListWrapper<string> ids { get; set; }
        public string orderId { get; set; }

        public LineItemsAddedOperation() 
        {
            ids = new ListWrapper<string>();
            orderId = null;
        }

        public LineItemsAddedOperation(string orderId)
        {
            this.orderId = orderId;
        }

        public void addId(string id)
        {
            this.ids.Add(id);
        }
        public void removeId(string id)
        {
            this.ids.Remove(id);
        }
    }

    public class DiscountsDeletedOperation : DisplayOperation
    {
        public ListWrapper<string> ids { get; set; }
        public string orderId { get; set; }

        public DiscountsDeletedOperation() 
        {
            ids = new ListWrapper<string>();
            orderId = null;
        }

        public DiscountsDeletedOperation(string orderId)
        {
            this.orderId = orderId;
        }

        public void addId(string id)
        {
            this.ids.Add(id);
        }
        public void removeId(string id)
        {
            this.ids.Remove(id);
        }
    }

    public class DiscountsAddedOperation : DisplayOperation
    {
        public ListWrapper<string> ids { get; set; }
        public string orderId { get; set; }

        public DiscountsAddedOperation() 
        {
            ids = new ListWrapper<string>();
            orderId = null;
        }

        public DiscountsAddedOperation(string orderId)
        {
            this.orderId = orderId;
        }

        public void addId(string id)
        {
            this.ids.Add(id);
        }
        public void removeId(string id)
        {
            this.ids.Remove(id);
        }
    }

}
