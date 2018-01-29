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
            get
            {
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
            try
            {
                CloverDeviceEvent deviceEvent = ParseResponse<CloverDeviceEvent>(context);
                if (deviceEvent != null)
                {
                    connectorListener.ForEach(listener => listener.OnDeviceActivityStart(deviceEvent));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }

        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/DeviceActivityEnd$")]
        public void OnDeviceActivityEnd(HttpListenerContext context)
        {
            try
            {
                CloverDeviceEvent deviceEvent = ParseResponse<CloverDeviceEvent>(context);
                if (deviceEvent != null)
                {
                    connectorListener.ForEach(listener => listener.OnDeviceActivityEnd(deviceEvent));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }

        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/DeviceError$")]
        public void OnDeviceError(HttpListenerContext context)
        {
            try
            {
                CloverDeviceErrorEvent deviceErrorEvent = ParseResponse<CloverDeviceErrorEvent>(context);
                if (deviceErrorEvent != null)
                {
                    connectorListener.ForEach(listener => listener.OnDeviceError(deviceErrorEvent));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }

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
            try
            {
                MerchantInfo merchantInfo = ParseResponse<MerchantInfo>(context);
                if (merchantInfo != null)
                {
                    connectorListener.ForEach(listener => listener.OnDeviceReady(merchantInfo));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }

        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/TipAdded$")]
        public void OnTipAdded(HttpListenerContext context)
        {
            try
            {
                TipAddedMessage tipAddedEvent = ParseResponse<TipAddedMessage>(context);
                if (tipAddedEvent != null)
                {
                    connectorListener.ForEach(listener => listener.OnTipAdded(tipAddedEvent));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }

        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/AuthResponse$")]
        public void AuthResponse(HttpListenerContext context)
        {
            try
            {
                AuthResponse response = ParseResponse<AuthResponse>(context);
                if (response != null)
                {
                    connectorListener.ForEach(listener => listener.OnAuthResponse(response));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }

        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PreAuthResponse$")]
        public void PreAuthResponse(HttpListenerContext context)
        {
            try
            {
                PreAuthResponse response = ParseResponse<PreAuthResponse>(context);
                if (response != null)
                {
                    connectorListener.ForEach(listener => listener.OnPreAuthResponse(response));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }

        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/SaleResponse$")]
        public void SaleResponse(HttpListenerContext context)
        {
            try
            {
                SaleResponse response = ParseResponse<SaleResponse>(context);
                if (response != null)
                {
                    connectorListener.ForEach(listener => listener.OnSaleResponse(response));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }


        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/VaultCardResponse$")]
        public void VaultCardResponse(HttpListenerContext context)
        {
            try
            {
                VaultCardResponse response = ParseResponse<VaultCardResponse>(context);
                if (response != null)
                {
                    connectorListener.ForEach(listener => listener.OnVaultCardResponse(response));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/ReadCardDataResponse$")]
        public void ReadCardDataResponse(HttpListenerContext context)
        {
            try
            {
                ReadCardDataResponse response = ParseResponse<ReadCardDataResponse>(context);
                if (response != null)
                {
                    connectorListener.ForEach(listener => listener.OnReadCardDataResponse(response));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }

        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/RefundPaymentResponse$")]
        public void RefundPaymentResponse(HttpListenerContext context)
        {
            try
            {
                RefundPaymentResponse response = ParseResponse<RefundPaymentResponse>(context);
                if (response != null)
                {
                    connectorListener.ForEach(listener => listener.OnRefundPaymentResponse(response));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }

        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/VoidPaymentResponse$")]
        public void VoidPaymentResponse(HttpListenerContext context)
        {

            try
            {
                VoidPaymentResponse response = ParseResponse<VoidPaymentResponse>(context);
                if (response != null)
                {
                    connectorListener.ForEach(listener => listener.OnVoidPaymentResponse(response));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }

        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/ManualRefundResponse$")]
        public void ManualRefundResponse(HttpListenerContext context)
        {
            try
            {
                ManualRefundResponse response = ParseResponse<ManualRefundResponse>(context);
                if (response != null)
                {
                    connectorListener.ForEach(listener => listener.OnManualRefundResponse(response));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }

        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/CapturePreAuthResponse$")]
        public void CapturePreAuthResponse(HttpListenerContext context)
        {
            try
            {
                CapturePreAuthResponse response = ParseResponse<CapturePreAuthResponse>(context);
                if (response != null)
                {
                    connectorListener.ForEach(listener => listener.OnCapturePreAuthResponse(response));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }

        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/TipAdjustAuthResponse")]
        public void TipAdjustAuthResponse(HttpListenerContext context)
        {
            try
            {
                TipAdjustAuthResponse response = ParseResponse<TipAdjustAuthResponse>(context);
                if (response != null)
                {
                    connectorListener.ForEach(listener => listener.OnTipAdjustAuthResponse(response));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/CloseoutResponse$")]
        public void CloseoutResponse(HttpListenerContext context)
        {
            try
            {
                CloseoutResponse response = ParseResponse<CloseoutResponse>(context);
                if (response != null)
                {
                    connectorListener.ForEach(listener => listener.OnCloseoutResponse(response));
                }
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/VerifySignatureRequest$")]
        public void VerifySignatureRequest(HttpListenerContext context)
        {
            try
            {
                VerifySignatureRequest request = ParseResponse<VerifySignatureRequest>(context);
                RemoteRESTCloverConnector.RESTSigVerRequestHandler sigVerRequest =
                    new RemoteRESTCloverConnector.RESTSigVerRequestHandler((RemoteRESTCloverConnector)cloverConnector, request);
                connectorListener.ForEach(listener => listener.OnVerifySignatureRequest(sigVerRequest));
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/ConfirmPaymentRequest$")]
        public void ConfirmPaymentRequest(HttpListenerContext context)
        {
            try
            {
                ConfirmPaymentRequest request = ParseResponse<ConfirmPaymentRequest>(context);
                connectorListener.ForEach(listener => listener.OnConfirmPaymentRequest(request));
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/RetrievePendingPaymentsResponse$")]
        public void RetrievePendingPayments(HttpListenerContext context)
        {
            try
            {
                RetrievePendingPaymentsResponse response = ParseResponse<RetrievePendingPaymentsResponse>(context);
                connectorListener.ForEach(listener => listener.OnRetrievePendingPaymentsResponse(response));
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/RetrievePaymentResponse$")]
        public void RetrievePaymentResponse(HttpListenerContext context)
        {
            try
            {
                RetrievePaymentResponse response = ParseResponse<RetrievePaymentResponse>(context);
                connectorListener.ForEach(listener => listener.OnRetrievePaymentResponse(response));
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/CustomActivityResponse$")]
        public void CustomActivityResponse(HttpListenerContext context)
        {
            try
            {
                CustomActivityResponse response = ParseResponse<CustomActivityResponse>(context);
                connectorListener.ForEach(listener => listener.OnCustomActivityResponse(response));
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PrintManualRefundReceipt$")]
        public void PrintManualRefundReceipt(HttpListenerContext context)
        {
            try
            {
                PrintManualRefundReceiptMessage printManualRefundReceiptMessage = ParseResponse<PrintManualRefundReceiptMessage>(context);
                connectorListener.ForEach(listener => listener.OnPrintManualRefundReceipt(printManualRefundReceiptMessage));
                SendTextResponse(context, "");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PrintManualRefundDeclineReceipt$")]
        public void PrintManualRefundDeclineReceipt(HttpListenerContext context)
        {
            try
            {
                PrintManualRefundDeclineReceiptMessage printManualRefundDeclineReceiptMessage = ParseResponse<PrintManualRefundDeclineReceiptMessage>(context);
                connectorListener.ForEach(listener => listener.OnPrintManualRefundDeclineReceipt(printManualRefundDeclineReceiptMessage));
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PrintPaymentReceipt$")]
        public void PrintPaymentReceipt(HttpListenerContext context)
        {
            try
            {
                PrintPaymentReceiptMessage printPaymentReceiptMessage = ParseResponse<PrintPaymentReceiptMessage>(context);
                connectorListener.ForEach(listener => listener.OnPrintPaymentReceipt(printPaymentReceiptMessage));
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PrintPaymentDeclineReceipt$")]
        public void PrintPaymentDeclineReceipt(HttpListenerContext context)
        {
            try
            {
                PrintPaymentDeclineReceiptMessage printPaymentDeclineReceiptMessage = ParseResponse<PrintPaymentDeclineReceiptMessage>(context);
                connectorListener.ForEach(listener => listener.OnPrintPaymentDeclineReceipt(printPaymentDeclineReceiptMessage));
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PrintPaymentMerchantCopyReceipt$")]
        public void PrintPaymentMerchantCopyReceipt(HttpListenerContext context)
        {
            try
            {
                PrintPaymentMerchantCopyReceiptMessage printPaymentMerchantCopyReceiptMessage = ParseResponse<PrintPaymentMerchantCopyReceiptMessage>(context);
                connectorListener.ForEach(listener => listener.OnPrintPaymentMerchantCopyReceipt(printPaymentMerchantCopyReceiptMessage));
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PrintRefundPaymentReceipt$")]
        public void PrintRefundPaymentReceipt(HttpListenerContext context)
        {
            try
            {
                PrintRefundPaymentReceiptMessage printRefundPaymentReceiptMessage = ParseResponse<PrintRefundPaymentReceiptMessage>(context);
                connectorListener.ForEach(listener => listener.OnPrintRefundPaymentReceipt(printRefundPaymentReceiptMessage));
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/RetrieveDeviceStatusResponse$")]
        public void RetrieveDeviceStatusResponse(HttpListenerContext context)
        {
            try
            {
                RetrieveDeviceStatusResponse rdsr = ParseResponse<RetrieveDeviceStatusResponse>(context);
                connectorListener.ForEach(listener => listener.OnRetrieveDeviceStatusResponse(rdsr));
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/ResetDeviceResponse$")]
        public void ResetDeviceResponse(HttpListenerContext context)
        {
            try
            {
                ResetDeviceResponse rdr = ParseResponse<ResetDeviceResponse>(context);
                connectorListener.ForEach(listener => listener.OnResetDeviceResponse(rdr));
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/RetrievePrintersResponse$")]
        public void RetrievePrintersResponse(HttpListenerContext context)
        {
            try
            {
                RetrievePrintersResponse rpr = ParseResponse<RetrievePrintersResponse>(context);
                connectorListener.ForEach(listener => listener.OnRetrievePrintersResponse(rpr));
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }

        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/PrintJobStatusResponse$")]
        public void PrintJobStatusResponse(HttpListenerContext context)
        {
            try
            {
                PrintJobStatusResponse pjsr = ParseResponse<PrintJobStatusResponse>(context);
                connectorListener.ForEach(listener => listener.OnPrintJobStatusResponse(pjsr));
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
        }


        [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/CloverCallback/MessageFromActivity")]
        public void MessageFromActivity(HttpListenerContext context)
        {
            try
            {
                MessageFromActivity mfa = ParseResponse<MessageFromActivity>(context);
                connectorListener.ForEach(listener => listener.OnMessageFromActivity(mfa));
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = e.Message;
                SendTextResponse(context, "error processing request");
            }
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
