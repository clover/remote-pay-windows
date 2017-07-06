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
using System.IO;
using System.Collections.Generic;
using Grapevine.Server;
using System.Net;
using Newtonsoft.Json.Converters;

namespace com.clover.remotepay.transport.remote
{
    /// <summary>
    /// Is the subclass of RESTServer for adding CloverConnectorListeners
    /// </summary>
    public class CloverRESTServer : RESTServer
    {
        public List<ICloverConnectorListener> cloverConnectorListeners = new List<ICloverConnectorListener>();
        public CloverRESTServer(string host = "localhost", string port = "1234", string protocol = "http", string dirindex = "index.html", string webroot = null, int maxthreads = 1) : base(host, port, protocol, dirindex, webroot, maxthreads)
        {

        }

        public void AddCloverConnectorListener(ICloverConnectorListener listener)
        {
            cloverConnectorListeners.Add(listener);
        }

        public ICloverConnector CloverConnector { get; set; }

    }

    /// <summary>
    /// Contains the restpoint definitions for the listening
    /// service that listens for callbacks from 
    /// the Clover Conector Windows REST Service. This class should
    /// parallel ICloverConnectorListener
    /// </summary>
    sealed class CloverCallbackListenerService : RESTResource
    {
        public List<ICloverConnectorListener> connectorListener
        {
            get {
                return (Server as CloverRESTServer).cloverConnectorListeners;
            }
        }
        public ICloverConnector cloverConnector
        {
            get
            {
                return (Server as CloverRESTServer).CloverConnector;
            }
        }

        public CloverCallbackListenerService()
        {
            //
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/DeviceActivityStart$")]
        public void OnDeviceActivityStart(HttpListenerContext context)
        {
            CloverDeviceEvent deviceEvent = ParseResponse<CloverDeviceEvent>(context);
            if(deviceEvent != null)
            {
                connectorListener.ForEach(listener => listener.OnDeviceActivityStart(deviceEvent));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/DeviceActivityEnd$")]
        public void OnDeviceActivityEnd(HttpListenerContext context)
        {
            CloverDeviceEvent deviceEvent = ParseResponse<CloverDeviceEvent>(context);
            if(deviceEvent != null)
            {
                connectorListener.ForEach(listener => listener.OnDeviceActivityEnd(deviceEvent));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/DeviceError$")]
        public void OnDeviceError(HttpListenerContext context)
        {
            CloverDeviceErrorEvent deviceErrorEvent = ParseResponse<CloverDeviceErrorEvent>(context);
            if(deviceErrorEvent != null)
            {
                connectorListener.ForEach(listener => listener.OnDeviceError(deviceErrorEvent));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/DeviceConnected$")]
        public void OnDeviceConnected(HttpListenerContext context)
        {
            connectorListener.ForEach(listener => listener.OnDeviceConnected());
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/DeviceDisconnected$")]
        public void OnDeviceDisconnected(HttpListenerContext context)
        {
            connectorListener.ForEach(listener => listener.OnDeviceDisconnected());
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/DeviceReady$")]
        public void OnDeviceReady(HttpListenerContext context)
        {
            MerchantInfo merchantInfo = ParseResponse<MerchantInfo>(context);
            if(merchantInfo != null)
            {
                connectorListener.ForEach(listener => listener.OnDeviceReady(merchantInfo));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/TipAdded$")]
        public void OnTipAdded(HttpListenerContext context)
        {
            TipAddedMessage tipAddedEvent = ParseResponse<TipAddedMessage>(context);
            if(tipAddedEvent != null)
            {
                connectorListener.ForEach(listener => listener.OnTipAdded(tipAddedEvent));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/AuthResponse$")]
        public void AuthResponse(HttpListenerContext context)
        {
            AuthResponse response = ParseResponse<AuthResponse>(context);
            if(response != null)
            {
                connectorListener.ForEach(listener => listener.OnAuthResponse(response));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PreAuthResponse$")]
        public void PreAuthResponse(HttpListenerContext context)
        {
            PreAuthResponse response = ParseResponse<PreAuthResponse>(context);
            if (response != null)
            {
                connectorListener.ForEach(listener => listener.OnPreAuthResponse(response));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/SaleResponse$")]
        public void SaleResponse(HttpListenerContext context)
        {
            SaleResponse response = ParseResponse<SaleResponse>(context);
            if(response != null)
            {
                connectorListener.ForEach(listener => listener.OnSaleResponse(response));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/VaultCardResponse$")]
        public void VaultCardResponse(HttpListenerContext context)
        {
            VaultCardResponse response = ParseResponse<VaultCardResponse>(context);
            if(response != null)
            {
                connectorListener.ForEach(listener => listener.OnVaultCardResponse(response));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/ReadCardDataResponse$")]
        public void ReadCardDataResponse(HttpListenerContext context)
        {
            ReadCardDataResponse response = ParseResponse<ReadCardDataResponse>(context);
            if (response != null)
            {
                connectorListener.ForEach(listener => listener.OnReadCardDataResponse(response));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/RefundPaymentResponse$")]
        public void RefundPaymentResponse(HttpListenerContext context)
        {
            RefundPaymentResponse response = ParseResponse<RefundPaymentResponse>(context);
            if(response != null)
            {
                connectorListener.ForEach(listener => listener.OnRefundPaymentResponse(response));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/VoidPaymentResponse$")]
        public void VoidPaymentResponse(HttpListenerContext context)
        {
            VoidPaymentResponse response = ParseResponse<VoidPaymentResponse>(context);
            if(response != null)
            {
                connectorListener.ForEach(listener => listener.OnVoidPaymentResponse(response));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/ManualRefundResponse$")]
        public void ManualRefundResponse(HttpListenerContext context)
        {
            ManualRefundResponse response = ParseResponse<ManualRefundResponse>(context);
            if(response != null)
            {
                connectorListener.ForEach(listener => listener.OnManualRefundResponse(response));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/CapturePreAuthResponse$")]
        public void CapturePreAuthResponse(HttpListenerContext context)
        {
            CapturePreAuthResponse response = ParseResponse<CapturePreAuthResponse>(context);
            if(response != null)
            {
                connectorListener.ForEach(listener => listener.OnCapturePreAuthResponse(response));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/TipAdjustAuthResponse")]
        public void TipAdjustAuthResponse(HttpListenerContext context)
        {
            TipAdjustAuthResponse response = ParseResponse<TipAdjustAuthResponse>(context);
            if(response != null)
            {
                connectorListener.ForEach(listener => listener.OnTipAdjustAuthResponse(response));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/CloseoutResponse$")]
        public void CloseoutResponse(HttpListenerContext context)
        {
            CloseoutResponse response = ParseResponse<CloseoutResponse>(context);
            if(response != null)
            {
                connectorListener.ForEach(listener => listener.OnCloseoutResponse(response));
            }
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/VerifySignatureRequest$")]
        public void VerifySignatureRequest(HttpListenerContext context)
        {
            VerifySignatureRequest request = ParseResponse<VerifySignatureRequest>(context);
            RemoteRESTCloverConnector.RESTSigVerRequestHandler sigVerRequest =
                new RemoteRESTCloverConnector.RESTSigVerRequestHandler((RemoteRESTCloverConnector)cloverConnector, request);
            connectorListener.ForEach(listener => listener.OnVerifySignatureRequest(sigVerRequest));
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/ConfirmPaymentRequest$")]
        public void ConfirmPaymentRequest(HttpListenerContext context)
        {
            ConfirmPaymentRequest request = ParseResponse<ConfirmPaymentRequest>(context);
            connectorListener.ForEach(listener => listener.OnConfirmPaymentRequest(request));
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/RetrievePendingPaymentsResponse$")]
        public void RetrievePendingPayments(HttpListenerContext context)
        {
            RetrievePendingPaymentsResponse response = ParseResponse<RetrievePendingPaymentsResponse>(context);
            connectorListener.ForEach(listener => listener.OnRetrievePendingPaymentsResponse(response));
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/RetrievePaymentResponse$")]
        public void RetrievePaymentResponse(HttpListenerContext context)
        {
            RetrievePaymentResponse response = ParseResponse<RetrievePaymentResponse>(context);
            connectorListener.ForEach(listener => listener.OnRetrievePaymentResponse(response));
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/CustomActivityResponse$")]
        public void CustomActivityResponse(HttpListenerContext context)
        {
            CustomActivityResponse response = ParseResponse<CustomActivityResponse>(context);
            connectorListener.ForEach(listener => listener.OnCustomActivityResponse(response));
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PrintManualRefundReceipt$")]
        public void PrintManualRefundReceipt(HttpListenerContext context)
        {
            PrintManualRefundReceiptMessage printManualRefundReceiptMessage = ParseResponse<PrintManualRefundReceiptMessage>(context);
            connectorListener.ForEach(listener => listener.OnPrintManualRefundReceipt(printManualRefundReceiptMessage));
            SendTextResponse(context, "");
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PrintManualRefundDeclineReceipt$")]
        public void PrintManualRefundDeclineReceipt(HttpListenerContext context)
        {
            PrintManualRefundDeclineReceiptMessage printManualRefundDeclineReceiptMessage = ParseResponse<PrintManualRefundDeclineReceiptMessage>(context);
            connectorListener.ForEach(listener => listener.OnPrintManualRefundDeclineReceipt(printManualRefundDeclineReceiptMessage));
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PrintPaymentReceipt$")]
        public void PrintPaymentReceipt(HttpListenerContext context)
        {
            PrintPaymentReceiptMessage printPaymentReceiptMessage = ParseResponse<PrintPaymentReceiptMessage>(context);
            connectorListener.ForEach(listener => listener.OnPrintPaymentReceipt(printPaymentReceiptMessage));
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PrintPaymentDeclineReceipt$")]
        public void PrintPaymentDeclineReceipt(HttpListenerContext context)
        {
            PrintPaymentDeclineReceiptMessage printPaymentDeclineReceiptMessage = ParseResponse<PrintPaymentDeclineReceiptMessage>(context);
            connectorListener.ForEach(listener => listener.OnPrintPaymentDeclineReceipt(printPaymentDeclineReceiptMessage));
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PrintPaymentMerchantCopyReceipt$")]
        public void PrintPaymentMerchantCopyReceipt(HttpListenerContext context)
        {
            PrintPaymentMerchantCopyReceiptMessage printPaymentMerchantCopyReceiptMessage = ParseResponse<PrintPaymentMerchantCopyReceiptMessage>(context);
            connectorListener.ForEach(listener => listener.OnPrintPaymentMerchantCopyReceipt(printPaymentMerchantCopyReceiptMessage));
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PrintRefundPaymentReceipt$")]
        public void PrintRefundPaymentReceipt(HttpListenerContext context)
        {
            PrintRefundPaymentReceiptMessage printRefundPaymentReceiptMessage = ParseResponse<PrintRefundPaymentReceiptMessage>(context);
            connectorListener.ForEach(listener => listener.OnPrintRefundPaymentReceipt(printRefundPaymentReceiptMessage));
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/RetrieveDeviceStatusResponse$")]
        public void RetrieveDeviceStatusResponse(HttpListenerContext context)
        {
            RetrieveDeviceStatusResponse rdsr = ParseResponse<RetrieveDeviceStatusResponse>(context);
            connectorListener.ForEach(listener => listener.OnRetrieveDeviceStatusResponse(rdsr));
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/ResetDeviceResponse$")]
        public void ResetDeviceResponse(HttpListenerContext context)
        {
            ResetDeviceResponse rdr = ParseResponse<ResetDeviceResponse>(context);
            connectorListener.ForEach(listener => listener.OnResetDeviceResponse(rdr));
        }


        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/MessageFromActivity")]
        public void MessageFromActivity(HttpListenerContext context)
        {
            MessageFromActivity mfa = ParseResponse<MessageFromActivity>(context);
            connectorListener.ForEach(listener => listener.OnMessageFromActivity(mfa));
        }

        private T ParseResponse<T>(HttpListenerContext context)
        {
            if (context.Request.ContentType != "application/json")
            {
                throw new HttpListenerException(500, "Unexpected Content Type. Expecting 'application/json'");
            }


            StreamReader stream = new StreamReader(context.Request.InputStream);
            string x = stream.ReadToEnd();  // added to view content of input stream

            T message = default(T);
            try
            {
                message = JsonUtils.deserialize<T>(x, new Newtonsoft.Json.JsonConverter[] { new StringEnumConverter() });
                if (message == null && x.Trim().Length > 0)
                {
                    Console.WriteLine("Error parsing " + typeof(T) + " from: " + x);
                }
            }
            finally
            {
                // return the default...
            }
            return message;
        }
    }
}
