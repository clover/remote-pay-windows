﻿// Copyright (C) 2016 Clover Network, Inc.
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
    class CloverWebSocketConnectorListener : ICloverConnectorListener
    {
        public Fleck.IWebSocketConnection WebSocket { get; set; }
        public MerchantInfo MerchantInfo { get; private set; }

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

        public void OnCapturePreAuthResponse(CapturePreAuthResponse response)
        {
            OnCapturePreAuthResponseMessage authCaptureResponse = new OnCapturePreAuthResponseMessage();
            authCaptureResponse.payload = response;
            WebSocket.Send(Serialize(authCaptureResponse));
        }

        public void OnTipAdjustAuthResponse(TipAdjustAuthResponse response)
        {
            OnTipAdjustAuthResponseMessage tipAdjustResponse = new OnTipAdjustAuthResponseMessage();
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

        public void OnDeviceReady(MerchantInfo merchantInfo)
        {
            CurrentConnectionStatus = "Ready";
            this.MerchantInfo = merchantInfo;
            OnDeviceReadyMessage message = new OnDeviceReadyMessage();
            message.payload = merchantInfo;
            WebSocket.Send(Serialize(message));
        }

        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
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

        public void OnVerifySignatureRequest(VerifySignatureRequest request)
        {
            OnVerifySignatureRequestMessage onVerifySignatureRequest = new OnVerifySignatureRequestMessage();
            onVerifySignatureRequest.payload = request;
            WebSocket.Send(Serialize(onVerifySignatureRequest));
        }

        public void OnConfirmPaymentRequest(ConfirmPaymentRequest request)
        {
            OnConfirmPaymentRequestMessage onConfirmPaymentRequest = new OnConfirmPaymentRequestMessage();
            onConfirmPaymentRequest.payload = request;
            WebSocket.Send(Serialize(onConfirmPaymentRequest));
        }

        public void OnVoidPaymentResponse(VoidPaymentResponse response)
        {
            OnVoidPaymentResponseMessage voidPaymentResponse = new OnVoidPaymentResponseMessage();
            voidPaymentResponse.payload = response;
            WebSocket.Send(Serialize(voidPaymentResponse));
        }

        public void OnTipAdded(com.clover.remotepay.transport.TipAddedMessage message)
        {
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

        public void OnRetrievePendingPaymentsResponse(RetrievePendingPaymentsResponse response)
        {
            OnRetrievePendingPaymentsResponseMessage retrievePendingPaymentsMessage = new OnRetrievePendingPaymentsResponseMessage();
            retrievePendingPaymentsMessage.payload = response;
            WebSocket.Send(Serialize(retrievePendingPaymentsMessage));
        }

        public void OnReadCardDataResponse(ReadCardDataResponse response)
        {
            OnReadCardDataResponseMessage cardDataResponseMessage = new OnReadCardDataResponseMessage();
            cardDataResponseMessage.payload = response;
            WebSocket.Send(Serialize(cardDataResponseMessage));
        }

        public void OnCustomActivityResponse(CustomActivityResponse response)
        {
            OnCustomActivityResponseMessage carMessage = new OnCustomActivityResponseMessage();
            carMessage.payload = response;
            WebSocket.Send(Serialize(carMessage));
        }

        public virtual void OnPrintManualRefundReceipt(PrintManualRefundReceiptMessage printManualRefundReceiptMessage)
        {
            OnPrintManualRefundReceiptMessage message = new OnPrintManualRefundReceiptMessage();
            message.payload = printManualRefundReceiptMessage;
            WebSocket.Send(Serialize(message));
        }

        public virtual void OnPrintManualRefundDeclineReceipt(PrintManualRefundDeclineReceiptMessage printManualRefundDeclineReceiptMessage)
        {
            OnPrintManualRefundDeclinedReceiptMessage message = new OnPrintManualRefundDeclinedReceiptMessage();
            message.payload = printManualRefundDeclineReceiptMessage;
            WebSocket.Send(Serialize(message));
        }

        public virtual void OnPrintPaymentReceipt(PrintPaymentReceiptMessage printPaymentReceiptMessage)
        {
            OnPrintPaymentReceiptMessage message = new OnPrintPaymentReceiptMessage();
            message.payload = printPaymentReceiptMessage;
            WebSocket.Send(Serialize(message));
        }

        public virtual void OnPrintPaymentDeclineReceipt(PrintPaymentDeclineReceiptMessage printPaymentDeclineReceiptMessage)
        {
            OnPrintPaymentDeclinedReceiptMessage message = new OnPrintPaymentDeclinedReceiptMessage();
            message.payload = printPaymentDeclineReceiptMessage;
            WebSocket.Send(Serialize(message));
        }

        public virtual void OnPrintPaymentMerchantCopyReceipt(PrintPaymentMerchantCopyReceiptMessage printPaymentMerchantCopyReceiptMessage)
        {
            OnPrintPaymentMerchatCopyReceiptMessage message = new OnPrintPaymentMerchatCopyReceiptMessage();
            message.payload = printPaymentMerchantCopyReceiptMessage;
            WebSocket.Send(Serialize(message));
        }

        public virtual void OnPrintRefundPaymentReceipt(PrintRefundPaymentReceiptMessage printRefundPaymentReceiptMessage)
        {
            OnPrintPaymentRefundReceiptMessage message = new OnPrintPaymentRefundReceiptMessage();
            message.payload = printRefundPaymentReceiptMessage;
            WebSocket.Send(Serialize(message));
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
                OnDeviceReady(this.MerchantInfo);
            }
        }

        private string Serialize(object obj) 
        {
            var myStr = JsonUtils.serialize(obj);
            return myStr;
        }
    }
}
