using com.clover.remotepay.sdk;
using com.clover.sdk.remote.websocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket4Net;

namespace com.clover.remotepay.transport.remote
{
    class RemoteWebSocketCloverConnectorListener : CloverConnectorListener
    {
        public WebSocket WebSocket { get; internal set; }
        public String connectionStatus = "Disconnected";

        public void OnAuthCaptureResponse(CaptureAuthResponse response)
        {
            OnAuthCaptureResponseMessage message = new OnAuthCaptureResponseMessage();
            message.payload = response;
            WebSocket.Send(JsonUtils.serialize(response));
        }

        public void OnAuthResponse(AuthResponse response)
        {
            OnAuthResponseMessage message = new OnAuthResponseMessage();
            message.payload = response;
            WebSocket.Send(JsonUtils.serialize(message));
        }

        public void OnAuthTipAdjustResponse(TipAdjustAuthResponse response)
        {
            OnAuthTipAdjustResponseMessage message = new OnAuthTipAdjustResponseMessage();
            message.payload = response;
            WebSocket.Send(JsonUtils.serialize(message));
        }

        public void OnCloseoutResponse(CloseoutResponse response)
        {
            throw new NotImplementedException();
        }

        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
            OnDeviceActivityEndMessage message = new OnDeviceActivityEndMessage();
            message.payload = deviceEvent;
            WebSocket.Send(JsonUtils.serialize(message));
        }

        public void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {
            OnDeviceActivityStartMessage message = new OnDeviceActivityStartMessage();
            message.payload = deviceEvent;
            WebSocket.Send(JsonUtils.serialize(message));
        }

        public void OnDeviceConnected()
        {
            connectionStatus = "Connected";
            WebSocket.Send(JsonUtils.serialize(new OnDeviceConnectedMessage()));
        }

        public void OnDeviceDisconnected()
        {
            connectionStatus = "Disconnected";
            WebSocket.Send(JsonUtils.serialize(new OnDeviceDisconnectedMessage()));
        }

        public void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {
            throw new NotImplementedException();
        }

        public void OnDeviceReady()
        {
            connectionStatus = "Ready";
            WebSocket.Send(JsonUtils.serialize(new OnDeviceReadyMessage()));
        }

        public void OnDisplayReceiptOptionsResponse(DisplayReceiptOptionsResponse response)
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception e)
        {
            throw new NotImplementedException();
        }

        public void OnManualRefundResponse(ManualRefundResponse response)
        {
            OnManualRefundResponseMessage message = new OnManualRefundResponseMessage();
            message.payload = response;
            WebSocket.Send(JsonUtils.serialize(message));
        }

        public void OnRefundPaymentResponse(RefundPaymentResponse response)
        {
            OnRefundPaymentResponseMessage message = new OnRefundPaymentResponseMessage();
            message.payload = response;
            WebSocket.Send(JsonUtils.serialize(message));
        }

        public void OnSaleResponse(SaleResponse response)
        {
            OnSaleResponseMessage message = new OnSaleResponseMessage();
            message.payload = response;
            WebSocket.Send(JsonUtils.serialize(message));
        }

        public void OnSignatureVerifyRequest(SignatureVerifyRequest request)
        {
            OnSignatureVerifyRequestMessage message = new OnSignatureVerifyRequestMessage();
            message.payload = request;
            WebSocket.Send(JsonUtils.serialize(message));
        }

        public void OnTipAdded(TipAddedMessage taMessage)
        {
            OnTipAddedMessage message = new OnTipAddedMessage();
            message.payload = taMessage;
            WebSocket.Send(JsonUtils.serialize(message));
        }

        public void OnVoidPaymentResponse(VoidPaymentResponse response)
        {
            OnVoidPaymentResponseMessage message = new OnVoidPaymentResponseMessage();
            message.payload = response;
            WebSocket.Send(JsonUtils.serialize(message));
        }

        public void OnVoidTransactionResponse(VoidTransactionResponse response)
        {
            throw new NotImplementedException();
        }
    }
}
