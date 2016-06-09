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

using com.clover.remotepay.transport;

namespace com.clover.remotepay.sdk
{
    /// <summary>
    /// These are the methods to implement for intercepting messages that 
    /// are sent from a Clover device.
    /// </summary>
    public interface ICloverConnectorListener 
    {
        /// <summary>
        /// Called when a Clover device is activity starts.
        /// </summary>
        /// <param name="deviceEvent">The device event.</param>
        void OnDeviceActivityStart(CloverDeviceEvent deviceEvent);
        /// <summary>
        /// Called when a Clover device is activity ends.
        /// </summary>
        /// <param name="deviceEvent">The device event.</param>
        void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent);
        /// <summary>
        /// Called when a Clover device is error event is encountered.
        /// </summary>
        /// <param name="deviceErrorEvent">The device error event.</param>
        void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent);
        /// <summary>
        /// Called when a pre authorization response message is sent.
        /// </summary>
        /// <param name="response">The response.</param>
        void OnPreAuthResponse(PreAuthResponse response);
        /// <summary>
        /// Called when an authorization response is sent from the Clover device.
        /// </summary>
        /// <param name="response">The response.</param>
        void OnAuthResponse(AuthResponse response);
        /// <summary>
        /// Called when a tip adjust authorization response is sent from the Clover device.
        /// </summary>
        /// <param name="response">The response.</param>
        void OnTipAdjustAuthResponse(TipAdjustAuthResponse response);
        /// <summary>
        /// Called when a capture pre authorization response is sent from the Clover device.
        /// </summary>
        /// <param name="response">The response.</param>
        void OnCapturePreAuthResponse(CapturePreAuthResponse response);
        /// <summary>
        /// Called when a verify signature request is sent from the Clover device.
        /// </summary>
        /// <param name="request">The request.</param>
        void OnVerifySignatureRequest(VerifySignatureRequest request);
        /// <summary>
        /// Called when a closeout response is sent from the Clover device.
        /// </summary>
        /// <param name="response">The response.</param>
        void OnCloseoutResponse(CloseoutResponse response);
        /// <summary>
        /// Called when a sale response is sent from the Clover device.
        /// </summary>
        /// <param name="response">The response.</param>
        void OnSaleResponse(SaleResponse response);
        /// <summary>
        /// Called when a manual refund response is sent from the Clover device.
        /// </summary>
        /// <param name="response">The response.</param>
        void OnManualRefundResponse(ManualRefundResponse response);
        /// <summary>
        /// Called when a refund payment response is sent from the Clover device.
        /// </summary>
        /// <param name="response">The response.</param>
        void OnRefundPaymentResponse(RefundPaymentResponse response);
        /// <summary>
        /// Called when a tip is added.
        /// </summary>
        /// <param name="message">The message.</param>
        void OnTipAdded(TipAddedMessage message);
        /// <summary>
        /// Called when a void payment response is sent from the Clover device.
        /// </summary>
        /// <param name="response">The response.</param>
        void OnVoidPaymentResponse(VoidPaymentResponse response);
        /// <summary>
        /// Called when a Clover device is connected.
        /// </summary>
        void OnDeviceConnected();
        /// <summary>
        /// Called when a Clover device is ready to receive communications from the CloverConnector.
        /// </summary>
        /// <param name="merchantInfo">The merchant information.</param>
        void OnDeviceReady(MerchantInfo merchantInfo);
        /// <summary>
        /// Called when a Clover device is disconnected from the CloverConnector.
        /// </summary>
        void OnDeviceDisconnected();
        /// <summary>
        /// Called when a vault card response is sent from the Clover device.
        /// </summary>
        /// <param name="response">The response.</param>
        void OnVaultCardResponse(VaultCardResponse response);
    }

    /// <summary>
    ///  This is a default implementation of the ICloverConnectorListener
    ///  that can be used for quickly creating example applications
    ///  by simply overriding the appropriate listener method(s) needed
    ///  for testing a particular remote call.
    /// </summary>
    public class DefaultCloverConnectorListener : ICloverConnectorListener
    {
        ICloverConnector cloverConnector;
        bool _isReady = false;

        public DefaultCloverConnectorListener(ICloverConnector cloverConnector)
        {
            this.cloverConnector = cloverConnector;
        }

        public void OnVaultCardResponse(VaultCardResponse response)
        {

        }

        public void OnCapturePreAuthResponse(CapturePreAuthResponse response)
        {

        }

        public void OnAuthResponse(AuthResponse response)
        {

        }

        public void OnPreAuthResponse(PreAuthResponse response)
        {

        }

        public void OnTipAdjustAuthResponse(TipAdjustAuthResponse response)
        {

        }

        public void OnCloseoutResponse(CloseoutResponse response)
        {

        }

        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {

        }

        public void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {

        }

        public void OnDeviceConnected()
        {
            _isReady = false;
        }

        public void OnDeviceDisconnected()
        {
            _isReady = false;
        }

        public void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {

        }

        public void OnDeviceReady(MerchantInfo merchantInfo)
        {
            _isReady = true;
        }

        public void OnManualRefundResponse(ManualRefundResponse response)
        {

        }

        public void OnRefundPaymentResponse(RefundPaymentResponse response)
        {

        }

        public void OnSaleResponse(SaleResponse response)
        {

        }

        public void OnVerifySignatureRequest(VerifySignatureRequest request)
        {

        }

        public void OnVoidPaymentResponse(VoidPaymentResponse response)
        {

        }

        public void OnTipAdded(TipAddedMessage message)
        {

        }

        public bool IsReady()
        {
            return _isReady;
        }
    }
}
