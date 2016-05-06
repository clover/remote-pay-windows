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
using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CloverWindowsSDKWebSocketService {
    /*
    [XmlRoot(ElementName = "OnTipAdded")]
    public class OnTipAdded
    {
        public TipAddedMessage TipAddedMessage { get; set; }
    }

    [XmlRoot(ElementName = "OnDeviceActivityEnd")]
    public class OnDeviceActivityEnd
    {
        public CloverDeviceEvent EndDeviceActivity { get; set; }
    }

    [XmlRoot(ElementName = "OnDeviceActivityStart")]
    public class OnDeviceActivityStart
    {
        public CloverDeviceEvent StartDeviceActivity { get; set; }
    }

    [XmlRoot(ElementName = "OnDeviceReady")]
    public class OnDeviceReadyMessage
    {
        public OnDeviceReadyMessage()
        {
            OnDeviceReady = new JObject();
        }
        public JObject OnDeviceReady { get; set; }
    }

    [XmlRoot(ElementName = "OnDeviceConnected")]
    public class OnDeviceConnectedMessage
    {
        public OnDeviceConnectedMessage()
        {
            OnDeviceConnected = new JObject();
        }
        public JObject OnDeviceConnected { get; set; }
    }

    [XmlRoot(ElementName = "OnDeviceDisconnected")]
    public class OnDeviceDisconnectedMessage
    {
        public OnDeviceDisconnectedMessage()
        {
            OnDeviceDisconnected = new JObject();
        }
        public JObject OnDeviceDisconnected { get; set; }
    }

    [XmlRoot(ElementName = "OnAuthResponse")]
    public class OnAuthResponse
    {
        public AuthResponse AuthResponse { get; set; }
    }

    [XmlRoot(ElementName = "OnAuthCaptureResponse")]
    public class OnAuthCaptureResponse
    {
        public CaptureAuthResponse CaptureAuthResponse { get; set; }
    }

    [XmlRoot(ElementName = "OnAuthTipAdjustResponse")]
    public class OnAuthTipAdjustResponse
    {
        public TipAdjustAuthResponse TipAdjustAuthResponse { get; set; }
    }

    [XmlRoot(ElementName = "OnCloverCloseoutResponse")]
    public class OnCloverCloseoutResponse
    {
        public CloseoutResponse CloseoutResponse { get; set; }
    }

    [XmlRoot(ElementName = "OnVoidPaymentResponse")]
    public class OnVoidPaymentResponse
    {
        public VoidPaymentResponse VoidPaymentResponse { get; set; }
    }

    [XmlRoot(ElementName = "OnVoidTransactionResponse")]
    public class OnVoidTransactionResponse
    {
        public VoidTransactionResponse VoidTransactionResponse { get; set; }
    }

    [XmlRoot(ElementName = "OnSaleResponse")]
    public class OnSaleResponse
    {
        public SaleResponse SaleResponse { get; set; }
    }

    [XmlRoot(ElementName = "OnSignatureVerifyRequest")]
    public class OnSignatureVerifyRequest
    {
        public SignatureVerifyRequest SignatureVerifyRequest { get; set; }
    }

    [XmlRoot(ElementName = "OnManualRefundResponse")]
    public class OnManualRefundResponse
    {
        public ManualRefundResponse ManualRefundResponse { get; set; }
    }

    [XmlRoot(ElementName = "OnRefundPaymentResponse")]
    public class OnRefundPaymentResponse
    {
        public RefundPaymentResponse RefundPaymentResponse { get; set; }
    }

    [XmlRoot(ElementName="OnDisplayReceiptOptionsResponse")]
    public class OnDisplayReceiptOptionsResponse
    {
        public DisplayReceiptOptionsResponse DisplayReceiptOptionsResponse { get; set; }
    }

    [XmlRoot(ElementName = "OnError")]
    public class OnError
    {

    }

    [XmlRoot(ElementName = "OnDeviceError")]
    public class OnDeviceError
    {
        public CloverDeviceErrorEvent DeviceErrorEvent { get; set; }
    }
    */
}