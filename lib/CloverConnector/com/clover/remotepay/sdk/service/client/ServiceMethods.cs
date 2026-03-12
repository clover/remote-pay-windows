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

using com.clover.remote.order;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

//
// contains service message payloads for the REST and WebSocket services, to enable auto-serialization
//
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

    [XmlRoot(ElementName = "PrintImage")]
    public class PrintImage
    {
        public string Bitmap { get; set; }
        public string Url { get; set; }

        public Bitmap GetBitmap()
        {
            byte[] imgBytes = Convert.FromBase64String(Bitmap);

            MemoryStream ms = new MemoryStream();
            ms.Write(imgBytes, 0, imgBytes.Length);
            Bitmap bp = new Bitmap(ms);
            ms.Close();
            return bp;
        }

    }

    [XmlRoot(ElementName = "PrintRequest64")]
    public class PrintRequest64
    {
        public List<string> base64strings = new List<string>();
        public List<string> imgUrls = new List<string>();
        public List<string> textLines = new List<string>();
        public string externalPrintJobId { get; set; }
        public string printDeviceId { get; set; }

        public PrintRequest64() { }
        public void setBase64Strings(string img)
        {
            this.base64strings.Add(img);
        }
        public void setImageUrls(string url)
        {
            this.imgUrls.Add(url);
        }
        public void setText(List<string> textLine)
        {
            if(textLine.Count < 1)
            {
                return;
            }
            this.textLines = textLine;
        }
    }

    [XmlRoot(ElementName = "LineItemAddedToDisplayOrder")]
    public class LineItemAddedToDisplayOrder
    {
        public DisplayOrder DisplayOrder { get; set; }
        public DisplayLineItem DisplayLineItem { get; set; }
    }

    [XmlRoot(ElementName = "LineItemRemovedFromDisplayOrder")]
    public class LineItemRemovedFromDisplayOrder
    {
        public DisplayOrder DisplayOrder { get; set; }
        public DisplayLineItem DisplayLineItem { get; set; }
    }

    [XmlRoot(ElementName = "DiscountAddedToDisplayOrder")]
    public class DiscountAddedToDisplayOrder
    {
        public DisplayOrder DisplayOrder { get; set; }
        public DisplayDiscount DisplayDiscount { get; set; }
    }

    [XmlRoot(ElementName = "DiscountRemovedFromDisplayOrder")]
    public class DiscountRemovedFromDisplayOrder
    {
        public DisplayOrder DisplayOrder { get; set; }
        public DisplayDiscount DisplayDiscount { get; set; }
    }

    [XmlRoot(ElementName = "RejectPaymentObject")]
    public class RejectPaymentObject
    {
        public clover.sdk.v3.payments.Payment Payment { get; set; }
        public transport.Challenge Challenge { get; set; }
    }
}