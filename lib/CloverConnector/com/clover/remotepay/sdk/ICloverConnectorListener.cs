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
        /// Called when a confirm payment request is sent from the Clover device.
        /// </summary>
        /// <param name="request">The request.</param>
        void OnConfirmPaymentRequest(ConfirmPaymentRequest request);
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
        /// <summary>
        /// Called when a retrieve pending payments response is sent from the Clover device.
        /// </summary>
        /// <param name="response"></param>
        void OnRetrievePendingPaymentsResponse(RetrievePendingPaymentsResponse response);
        /// <summary>
        /// Called when a retrieve card data response is sent from the Clover device.
        /// </summary>
        /// <param name="response">The response.</param>
        void OnReadCardDataResponse(ReadCardDataResponse response);

        ///<summary>
        /// Will only be called if disablePrinting = true on the Sale, Auth, PreAuth or ManualRefund Request
        /// Called when a user requests to print a receipt for a ManualRefund
        /// </summary>
        /// <param name="printManualRefundReceiptMessage"></param>
        void OnPrintManualRefundReceipt(PrintManualRefundReceiptMessage printManualRefundReceiptMessage);

        /// <summary>
        /// Will only be called if disablePrinting = true on the Sale, Auth, PreAuth or ManualRefund Request
        /// Called when a user requests to print a receipt for a declined ManualRefund
        /// </summary>
        void OnPrintManualRefundDeclineReceipt(PrintManualRefundDeclineReceiptMessage printManualRefundDeclineReceiptMessage);

        /// <summary>
        /// Will only be called if disablePrinting = true on the Sale, Auth, PreAuth or ManualRefund Request
        /// Called when a user requests to print a receipt for a payment
        /// </summary>
        void OnPrintPaymentReceipt(PrintPaymentReceiptMessage printPaymentReceiptMessage);

        /// <summary>
        /// Will only be called if disablePrinting = true on the Sale, Auth, PreAuth or ManualRefund Request
        /// Called when a user requests to print a receipt for a declined payment
        /// </summary>
        void OnPrintPaymentDeclineReceipt(PrintPaymentDeclineReceiptMessage printPaymentDeclineReceiptMessage);

        /// <summary>
        /// Will only be called if disablePrinting = true on the Sale, Auth, PreAuth or ManualRefund Request
        /// Called when a user requests to print a merchant copy of a payment receipt
        /// </summary>
        void OnPrintPaymentMerchantCopyReceipt(PrintPaymentMerchantCopyReceiptMessage printPaymentMerchantCopyReceiptMessage);

        /// <summary>
        /// Will only be called if disablePrinting = true on the Sale, Auth, PreAuth or ManualRefund Request
        /// Called when a user requests to print a receipt for a payment refund
        /// </summary>
        void OnPrintRefundPaymentReceipt(PrintRefundPaymentReceiptMessage printRefundPaymentReceiptMessage);

        ///<summary>
        /// Called when a custom activity is terminated in a normal flow
        ///</summary>
        void OnCustomActivityResponse(CustomActivityResponse response);
    }

    /// <summary>
    ///  This is a default implementation of the ICloverConnectorListener
    ///  that can be used for quickly creating example applications
    ///  by simply overriding the appropriate listener method(s) needed
    ///  for testing a particular remote call.
    /// </summary>
    public abstract class DefaultCloverConnectorListener : ICloverConnectorListener
    {
        ICloverConnector cloverConnector;

        public DefaultCloverConnectorListener(ICloverConnector cloverConnector)
        {
            this.cloverConnector = cloverConnector;
        }

        public virtual void OnVaultCardResponse(VaultCardResponse response)
        {

        }

        public virtual void OnReadCardDataResponse(ReadCardDataResponse response)
        {

        }

        public virtual void OnCapturePreAuthResponse(CapturePreAuthResponse response)
        {

        }

        public virtual void OnAuthResponse(AuthResponse response)
        {

        }

        public virtual void OnPreAuthResponse(PreAuthResponse response)
        {

        }

        public virtual void OnTipAdjustAuthResponse(TipAdjustAuthResponse response)
        {

        }

        public virtual void OnCloseoutResponse(CloseoutResponse response)
        {

        }

        public virtual void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {

        }

        public virtual void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {

        }

        public virtual void OnDeviceConnected()
        {
            
        }

        public virtual void OnDeviceDisconnected()
        {
            
        }

        public virtual void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {

        }

        public virtual void OnDeviceReady(MerchantInfo merchantInfo)
        {
            
        }

        public virtual void OnManualRefundResponse(ManualRefundResponse response)
        {

        }

        public virtual void OnRefundPaymentResponse(RefundPaymentResponse response)
        {

        }

        public virtual void OnSaleResponse(SaleResponse response)
        {

        }

        public virtual void OnVerifySignatureRequest(VerifySignatureRequest request)
        {

        }

        public abstract void OnConfirmPaymentRequest(ConfirmPaymentRequest request);

        public virtual void OnVoidPaymentResponse(VoidPaymentResponse response)
        {

        }

        public virtual void OnTipAdded(TipAddedMessage message)
        {

        }

        public virtual void OnRetrievePendingPaymentsResponse(RetrievePendingPaymentsResponse response)
        {

        }

        public virtual void OnCustomActivityResponse(CustomActivityResponse response)
        {

        }

        public virtual void OnPrintManualRefundReceipt(PrintManualRefundReceiptMessage printManualRefundReceiptMessage)
        {

        }

        public virtual void OnPrintManualRefundDeclineReceipt(PrintManualRefundDeclineReceiptMessage printManualRefundDeclineReceiptMessage)
        {

        }

        public virtual void OnPrintPaymentReceipt(PrintPaymentReceiptMessage printPaymentReceiptMessage)
        {

        }

        public virtual void OnPrintPaymentDeclineReceipt(PrintPaymentDeclineReceiptMessage printPaymentDeclineReceiptMessage)
        {

        }

        public virtual void OnPrintPaymentMerchantCopyReceipt(PrintPaymentMerchantCopyReceiptMessage printPaymentMerchantCopyReceiptMessage)
        {

        }

        public virtual void OnPrintRefundPaymentReceipt(PrintRefundPaymentReceiptMessage printRefundPaymentReceiptMessage)
        {

        }

    }
}
