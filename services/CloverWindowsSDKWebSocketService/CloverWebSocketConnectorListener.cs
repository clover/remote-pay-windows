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
using com.clover.remotepay.transport;
using com.clover.sdk.remote.websocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CloverWindowsSDKWebSocketService
{
    class CloverWebSocketConnectorListener : CloverConnectorListener
    {
        public Fleck.IWebSocketConnection WebSocket { get; set; }
        public string CurrentConnectionStatus = "Disconnected";

        public void OnAuthResponse(AuthResponse response)
        {
            OnAuthResponseMessage authResponse = new OnAuthResponseMessage();
            authResponse.payload = response;
            WebSocket.Send(Serialize(authResponse));
        }

        public void OnPreAuthResponse(PreAuthResponse response)
        {
            OnPreAuthResponseMessage preAuthResponse = new OnPreAuthResponseMessage();
            preAuthResponse.payload = response;
            WebSocket.Send(Serialize(preAuthResponse));
        }

        public void OnAuthCaptureResponse(CaptureAuthResponse response)
        {
            OnAuthCaptureResponseMessage authCaptureResponse = new OnAuthCaptureResponseMessage();
            authCaptureResponse.payload = response;
            WebSocket.Send(Serialize(authCaptureResponse));
        }

        public void OnAuthTipAdjustResponse(TipAdjustAuthResponse response)
        {
            OnAuthTipAdjustResponseMessage tipAdjustResponse = new OnAuthTipAdjustResponseMessage();
            tipAdjustResponse.payload = response;
            WebSocket.Send(Serialize(tipAdjustResponse));
        }

        public void OnCloseoutResponse(CloseoutResponse response)
        {
            OnCloseoutResponseMessage closeoutResponse = new OnCloseoutResponseMessage();
            closeoutResponse.payload = response;
            WebSocket.Send(Serialize(closeoutResponse));
        }

        public void OnDeviceConnected()
        {
            CurrentConnectionStatus = "Connected";
            WebSocket.Send(Serialize(new OnDeviceConnectedMessage()));
        }

        public void OnDeviceDisconnected()
        {
            CurrentConnectionStatus = "Disconnected";
            WebSocket.Send(Serialize(new OnDeviceDisconnectedMessage()));
        }

        public void OnDeviceReady()
        {
            CurrentConnectionStatus = "Ready";
            WebSocket.Send(Serialize(new OnDeviceReadyMessage()));
        }

        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(OnDeviceActivityEndMessage));
            OnDeviceActivityEndMessage method = new OnDeviceActivityEndMessage();
            method.payload = deviceEvent;
            string messageContent = Serialize(method);
            WebSocket.Send(messageContent);
        }

        public void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {
            OnDeviceActivityStartMessage method = new OnDeviceActivityStartMessage();
            method.payload = deviceEvent;
            string messageContent = Serialize(method);
            WebSocket.Send(messageContent);
        }

        public void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {
            OnDeviceErrorMessage deviceError = new OnDeviceErrorMessage();
            deviceError.payload = deviceErrorEvent;
            WebSocket.Send(Serialize(deviceError));
        }

        public void OnDisplayReceiptOptionsResponse(DisplayReceiptOptionsResponse response)
        {
            // TODO: don't think this weill ever get called
        }

        public void OnError(Exception e)
        {
            OnError onError = new OnError();
            WebSocket.Send(Serialize(onError));
        }

        public void OnManualRefundResponse(ManualRefundResponse response)
        {
            OnManualRefundResponseMessage manualRefundResponse = new OnManualRefundResponseMessage();
            manualRefundResponse.payload = response;
            WebSocket.Send(Serialize(manualRefundResponse));
        }

        public void OnRefundPaymentResponse(RefundPaymentResponse response)
        {
            OnRefundPaymentResponseMessage refundPaymentResponse = new OnRefundPaymentResponseMessage();
            refundPaymentResponse.payload = response;
            WebSocket.Send(Serialize(refundPaymentResponse));
        }

        public void OnSaleResponse(SaleResponse response)
        {
            OnSaleResponseMessage onSaleResponse = new OnSaleResponseMessage();
            onSaleResponse.payload = response;
            WebSocket.Send(Serialize(onSaleResponse));
        }

        public void OnSignatureVerifyRequest(SignatureVerifyRequest request)
        {
            OnSignatureVerifyRequestMessage onSignatureVerifyRequest = new OnSignatureVerifyRequestMessage();
            onSignatureVerifyRequest.payload = request;
            WebSocket.Send(Serialize(onSignatureVerifyRequest));
        }

        public void OnVoidPaymentResponse(VoidPaymentResponse response)
        {
            OnVoidPaymentResponseMessage voidPaymentResponse = new OnVoidPaymentResponseMessage();
            voidPaymentResponse.payload = response;
            WebSocket.Send(Serialize(voidPaymentResponse));
        }

        public void OnVoidTransactionResponse(VoidTransactionResponse response)
        {
            // not implemented
            /*
            OnVoidTransactionResponseMessage voidTransactionResponse = new OnVoidTransactionResponseMessage();
            voidTransactionResponse.payload = response;
            WebSocket.Send(Serialize(voidTransactionResponse));
            */
        }

        public void OnTipAdded(com.clover.remotepay.transport.TipAddedMessage message)
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(OnTipAdded));
            OnTipAddedMessage method = new OnTipAddedMessage();
            method.payload = message;
            string messageContent = Serialize(method);
            WebSocket.Send(messageContent);
        }

        public void OnVaultCardResponse(VaultCardResponse response)
        {
            OnVaultCardResponseMessage vaultCardResponseMessage = new OnVaultCardResponseMessage();
            vaultCardResponseMessage.payload = response;
            WebSocket.Send(Serialize(vaultCardResponseMessage));
        }

        internal void SendConnectionStatus()
        {
            if ("Disconnected".Equals(CurrentConnectionStatus))
            {
                OnDeviceDisconnected();
            }
            else if ("Connected".Equals(CurrentConnectionStatus))
            {
                OnDeviceConnected();
            }
            else if ("Ready".Equals(CurrentConnectionStatus))
            {
                OnDeviceReady();
            }
        }

        private string Serialize(object obj) 
        {
            /*
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();

            serializer.Serialize(ms, obj);
            ms.Position = 0;
            var sr = new StreamReader(ms);
            var myStr = sr.ReadToEnd();
            ms.Close();
            sr.Close();
            */

            var myStr = JsonUtils.serialize(obj);
            return myStr;
        }

        public void OnConfigError(ConfigErrorResponse ceResponse)
        {
            OnConfigErrorMessage configErrorResponse = new OnConfigErrorMessage();
            configErrorResponse.payload = ceResponse;
            WebSocket.Send(Serialize(configErrorResponse));

        }
    }
}
