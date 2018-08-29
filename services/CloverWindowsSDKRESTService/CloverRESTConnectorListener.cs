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
using com.clover.remotepay.transport;
using RestSharp;
using System;
using System.Net;

namespace CloverWindowsSDKREST
{
    public class CloverRESTConnectorListener : ICloverConnectorListener
    {
        public RestClient RestClient { get; set; }
        public string Status { get; internal set; }
        private MerchantInfo MerchantInfo { get; set; }

        public CloverRESTConnectorListener()
        {
            Status = "Disconnected";
        }

        public void OnAuthResponse(AuthResponse response)
        {
            Send("/AuthResponse", Serialize(response));
        }

        public void OnPreAuthResponse(PreAuthResponse response)
        {
            Send("/PreAuthResponse", Serialize(response));
        }

        public void OnCapturePreAuthResponse(CapturePreAuthResponse response)
        {
            Send("/CapturePreAuthResponse", Serialize(response));
        }

        public void OnTipAdjustAuthResponse(TipAdjustAuthResponse response)
        {
            Send("/TipAdjustAuthResponse", Serialize(response));
        }

        public void OnCloseoutResponse(CloseoutResponse response)
        {
            Send("/CloseoutResponse", Serialize(response));
        }

        public void OnDeviceConnected()
        {
            Status = "Connected";
            Send("/DeviceConnected", null);
        }

        public void OnDeviceDisconnected()
        {
            Status = "Disconnected";
            Send("/DeviceDisconnected", null);
        }

        public void OnDeviceReady(MerchantInfo merchantInfo)
        {
            Status = "Ready";
            this.MerchantInfo = merchantInfo;
            Send("/DeviceReady", Serialize(merchantInfo));
        }

        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
            Send("/DeviceActivityEnd", Serialize(deviceEvent));
        }

        public void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {
            Send("/DeviceActivityStart", Serialize(deviceEvent));
        }

        public void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {
            Send("/DeviceError", Serialize(deviceErrorEvent));
        }

        public void OnManualRefundResponse(ManualRefundResponse response)
        {
            Send("/ManualRefundResponse", Serialize(response));
        }

        public void OnRefundPaymentResponse(RefundPaymentResponse response)
        {
            Send("/RefundPaymentResponse", Serialize(response));
        }

        public void OnSaleResponse(SaleResponse response)
        {
            Send("/SaleResponse", Serialize(response));
        }

        public void OnVerifySignatureRequest(VerifySignatureRequest request)
        {
            Send("/VerifySignatureRequest", Serialize(request));
        }

        public void OnConfirmPaymentRequest(ConfirmPaymentRequest request)
        {
            Send("/ConfirmPaymentRequest", Serialize(request));
        }

        public void OnVoidPaymentResponse(VoidPaymentResponse response)
        {
            Send("/VoidPaymentResponse", Serialize(response));
        }

        public void OnTipAdded(com.clover.remotepay.transport.TipAddedMessage message)
        {
            Send("/TipAdded", Serialize(message));
        }

        public void OnVaultCardResponse(VaultCardResponse message)
        {
            Send("/VaultCardResponse", Serialize(message));
        }

        public void OnRetrievePendingPaymentsResponse(RetrievePendingPaymentsResponse message)
        {
            Send("/RetrievePendingPaymentsResponse", Serialize(message));
        }

        public void OnReadCardDataResponse(ReadCardDataResponse message)
        {
            Send("/ReadCardDataResponse", Serialize(message));
        }

        public void OnCustomActivityResponse(CustomActivityResponse message)
        {
            Send("/CustomActivityResponse", Serialize(message));
        }

        public virtual void OnPrintManualRefundReceipt(PrintManualRefundReceiptMessage printManualRefundReceiptMessage)
        {
            Send("/PrintManualRefundReceipt", Serialize(printManualRefundReceiptMessage));
        }

        public virtual void OnPrintManualRefundDeclineReceipt(PrintManualRefundDeclineReceiptMessage printManualRefundDeclineReceiptMessage)
        {
            Send("/PrintManualRefundDeclineReceipt", Serialize(printManualRefundDeclineReceiptMessage));
        }

        public virtual void OnPrintPaymentReceipt(PrintPaymentReceiptMessage printPaymentReceiptMessage)
        {
            Send("/PrintPaymentReceipt", Serialize(printPaymentReceiptMessage));
        }

        public virtual void OnPrintPaymentDeclineReceipt(PrintPaymentDeclineReceiptMessage printPaymentDeclineReceiptMessage)
        {
            Send("/PrintPaymentDeclineReceipt", Serialize(printPaymentDeclineReceiptMessage));
        }

        public virtual void OnPrintPaymentMerchantCopyReceipt(PrintPaymentMerchantCopyReceiptMessage printPaymentMerchantCopyReceiptMessage)
        {
            Send("/PrintPaymentMerchantCopyReceipt", Serialize(printPaymentMerchantCopyReceiptMessage));
        }

        public virtual void OnPrintRefundPaymentReceipt(PrintRefundPaymentReceiptMessage printRefundPaymentReceiptMessage)
        {
            Send("/PrintRefundPaymentReceipt", Serialize(printRefundPaymentReceiptMessage));
        }

        public virtual void OnMessageFromActivity(MessageFromActivity message)
        {
            Send("/MessageFromActivity", Serialize(message));
        }

        public virtual void OnResetDeviceResponse(ResetDeviceResponse response)
        {
            Send("/ResetDeviceResponse", Serialize(response));
        }

        public virtual void OnRetrieveDeviceStatusResponse(RetrieveDeviceStatusResponse response)
        {
            Send("/RetrieveDeviceStatusResponse", Serialize(response));
        }

        public void OnRetrievePaymentResponse(RetrievePaymentResponse response)
        {
            Send("/RetrievePaymentResponse", Serialize(response));
        }

        public virtual void OnPrintJobStatusResponse(PrintJobStatusResponse response)
        {
            Send("/PrintJobStatusResponse", Serialize(response));
        }

        public virtual void OnRetrievePrintersResponse(RetrievePrintersResponse response)
        {
            Send("/RetrievePrintersResponse", Serialize(response));
        }

        public virtual void OnPrintJobStatusRequest(PrintJobStatusRequest request)
        {
            Send("/PrintJobStatusRequest", Serialize(request));
        }

        public void OnDisplayReceiptOptionsResponse(DisplayReceiptOptionsResponse response)
        {
            Send("/DisplayReceiptOptionsResponse", Serialize(response));
        }

        public void ResendStatus()
        {
            switch (Status)
            {
                case "Ready":
                    {
                        this.OnDeviceReady(MerchantInfo);
                        break;
                    }
                case "Disconnected" :
                    {
                        OnDeviceDisconnected();
                        break;
                    }
                case "Connected":
                    {
                        OnDeviceConnected();
                        break;
                    }
            }

        }

        private object Serialize(object obj) 
        {
            return obj;
        }

        private void Send(string endpoint, object msg)
        {
            if (RestClient != null)
            {
                RestRequest request = new RestRequest(endpoint, Method.POST);
                if (msg != null)
                {
                    string payload = JsonUtils.serialize(msg);
                    request.AddParameter("application/json", payload, ParameterType.RequestBody);
                }
                else
                {
                    request.AddJsonBody("");
                }
                request.RequestFormat = DataFormat.Json;
                
                var asyncHandle = RestClient.ExecuteAsync(request, response =>
                {
                    if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
                    {
                        Console.WriteLine("Response on callback is code: " + response.StatusCode + " for " + endpoint);
                    }
#if DEBUG
                    Console.WriteLine(response.StatusCode + " for " + endpoint + " with payload: " + request.Parameters[0].ToString());
#endif
                });

            }
        }

        
    }
}
