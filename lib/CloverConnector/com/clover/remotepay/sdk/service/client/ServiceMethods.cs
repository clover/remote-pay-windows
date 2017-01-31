// Copyright (C) 2016 Clover Network, Inc.
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

using com.clover.remote.order;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

/// <summary>
/// contains service message payloads for the REST and WebSocket services, to enable auto-serialization
/// </summary>
namespace com.clover.remotepay.sdk.service.client {

    [XmlRoot(ElementName = "ShowMessage")]
    public class ShowMessage
    {
        public string Message { get; set; }
    }

    [XmlRoot(ElementName = "VaultCard")]
    public class VaultCard
    {
        public int? CardEntryMethods { get; set; }
    }

    [XmlRoot(ElementName = "PrintText")]
    public class PrintText
    {
        public List<string> Messages { get; set; }
    }

    [XmlRoot(ElementName = "OpenCashDrawer")]
    public class OpenCashDrawer
    {
        public string Reason { get; set; }
    }

    [XmlRoot(ElementName = "PrintImage")]
    public class PrintImage
    {
        public string Bitmap { get; set; }
        public string Url { get; set; }

        public System.Drawing.Bitmap GetBitmap()
        {
            byte[] imgBytes = Convert.FromBase64String(Bitmap);

            MemoryStream ms = new MemoryStream();
            ms.Write(imgBytes, 0, imgBytes.Length);
            Bitmap bp = new Bitmap(ms);
            ms.Close();
            return bp;
        }

    }

    [XmlRoot(ElementName = "LineItemAddedToDisplayOrder")]
    public class LineItemAddedToDisplayOrder
    {
        public com.clover.remote.order.DisplayOrder DisplayOrder { get; set; }
        public DisplayLineItem DisplayLineItem { get; set; }
    }

    [XmlRoot(ElementName = "LineItemRemovedFromDisplayOrder")]
    public class LineItemRemovedFromDisplayOrder
    {
        public com.clover.remote.order.DisplayOrder DisplayOrder { get; set; }
        public DisplayLineItem DisplayLineItem { get; set; }
    }

    [XmlRoot(ElementName = "DiscountAddedToDisplayOrder")]
    public class DiscountAddedToDisplayOrder
    {
        public com.clover.remote.order.DisplayOrder DisplayOrder { get; set; }
        public DisplayDiscount DisplayDiscount { get; set; }
    }

    [XmlRoot(ElementName = "DiscountRemovedFromDisplayOrder")]
    public class DiscountRemovedFromDisplayOrder
    {
        public com.clover.remote.order.DisplayOrder DisplayOrder { get; set; }
        public DisplayDiscount DisplayDiscount { get; set; }
    }

    [XmlRoot(ElementName = "RejectPaymentObject")]
    public class RejectPaymentObject
    {
        public clover.sdk.v3.payments.Payment Payment { get; set; }
        public transport.Challenge Challenge { get; set; }
    }
}