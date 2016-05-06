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

using com.clover.remotepay.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace com.clover.remotepay.transport.remote
{
    [ServiceContract]
    interface ICloverCallbackService
    {
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        BodyStyle = WebMessageBodyStyle.Bare,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/AuthResponse")]
        void AuthResponse(AuthResponse authResponse);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        BodyStyle = WebMessageBodyStyle.Bare,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/PreAuthResponse")]
        void PreAuthResponse(PreAuthResponse preAuthResponse);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/SaleResponse")]
        void SaleResponse(SaleResponse saleResponse);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/CloseoutResponse")]
        void CloseoutResponse(CloseoutResponse closeoutResponse);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/RefundPaymentResponse")]
        void RefundPaymentResponse(RefundPaymentResponse response);


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/TipAdjustAuthResponse")]
        void TipAdjustAuthResponse(TipAdjustAuthResponse response);


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/CaptureAuthResponse")]
        void CaptureAuthResponse(CaptureAuthResponse response);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/ManualRefundResponse")]
        void ManualRefundResponse(ManualRefundResponse response);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/VaultCardResponse")]
        void VaultCardResponse(VaultCardResponse response);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/ConfigErrorResponse")]
        void ConfigErrorResponse(ConfigErrorResponse response);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/VoidPaymentResponse")]
        void VoidPaymentResponse(VoidPaymentResponse response);


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/SignatureVerifyRequest")]
        void SignatureVerifyRequest(SignatureVerifyRequest request);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/DeviceActivityStart")]
        void OnDeviceActivityStart(CloverDeviceEvent deviceEvent);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/DeviceActivityEnd")]
        void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/DeviceError")]
        void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/TipAdded")]
        void OnTipAdded(TipAddedEvent msg);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/DeviceConnected")]
        void OnDeviceConnected();


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/DeviceDisconnected")]
        void OnDeviceDisconnected();

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/DeviceReady")]
        void OnDeviceReady();

    }

    public class TipAddedEvent
    {
        public long tipAmount { get; set; }
    }
}
