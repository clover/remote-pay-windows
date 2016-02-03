using com.clover.remotepay.transport.remote;
using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.clover.remotepay.transport.remote
{
    class CloverCallbackService : ICloverCallbackService
    {
        public List<CloverConnectorListener> connectorListener = new List<CloverConnectorListener>();
        ICloverConnector cloverConnector;

        public CloverCallbackService(ICloverConnector cloverConnector)
        {
            this.cloverConnector = cloverConnector;
        }

        public void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {
            connectorListener.ForEach(listener => listener.OnDeviceActivityStart(deviceEvent));
        }

        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
            connectorListener.ForEach(listener => listener.OnDeviceActivityEnd(deviceEvent));
        }

        public void OnDeviceConnected()
        {
            connectorListener.ForEach(listener => listener.OnDeviceConnected());
        }

        public void OnDeviceDisconnected()
        {
            connectorListener.ForEach(listener => listener.OnDeviceDisconnected());
        }

        public void OnDeviceReady()
        {
            connectorListener.ForEach(listener => listener.OnDeviceReady());
        }

        public void OnTipAdded(TipAddedEvent tipAddedEvent)
        {
            TipAddedMessage msg = new TipAddedMessage(tipAddedEvent.tipAmount);
            connectorListener.ForEach(listener => listener.OnTipAdded(msg));
        }

        public void AuthResponse(AuthResponse response)
        {
            connectorListener.ForEach(listener => listener.OnAuthResponse(response));
        }
        public void SaleResponse(SaleResponse response)
        {
            connectorListener.ForEach(listener => listener.OnSaleResponse(response));
        }

        public void VaultCardResponse(VaultCardResponse response)
        {
            connectorListener.ForEach(listener => listener.OnVaultCardResponse(response));
        }

        public void RefundPaymentResponse(RefundPaymentResponse response)
        {
            Console.WriteLine("RefundPaymentResponse: " + response.OrderId);
            connectorListener.ForEach(listener => listener.OnRefundPaymentResponse(response));
        }

        public void VoidPaymentResponse(VoidPaymentResponse response)
        {
            connectorListener.ForEach(listener => listener.OnVoidPaymentResponse(response));
        }

        public void ManualRefundResponse(ManualRefundResponse response)
        {
            connectorListener.ForEach(listener => listener.OnManualRefundResponse(response));
        }

        public void CaptureAuthResponse(CaptureAuthResponse response)
        {
            connectorListener.ForEach(listener => listener.OnAuthCaptureResponse(response));
        }

        public void TipAdjustAuthResponse(TipAdjustAuthResponse response)
        {
            connectorListener.ForEach(listener => listener.OnAuthTipAdjustResponse(response));
        }

        public void SignatureVerifyRequest(SignatureVerifyRequest request)
        {
            RemoteRESTCloverConnector.RESTSigVerRequestHandler sigVerRequest =
                new RemoteRESTCloverConnector.RESTSigVerRequestHandler((RemoteRESTCloverConnector)cloverConnector, request);
            connectorListener.ForEach(listener => listener.OnSignatureVerifyRequest(sigVerRequest));
        }

        internal void AddListener(CloverConnectorListener connectorListener)
        {
            this.connectorListener.Add(connectorListener);
        }
    }
}
