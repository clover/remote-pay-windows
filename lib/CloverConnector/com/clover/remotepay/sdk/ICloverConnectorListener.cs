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
        /// Called when the Clover device transitions to a new screen or activity. The 
        /// CloverDeviceEvent passed in will contain an event type, a description, and a 
        /// list of available InputOptions.
        /// </summary>
        /// <param name="deviceEvent">The CloverDeviceEvent event.</param>
        void OnDeviceActivityStart(CloverDeviceEvent deviceEvent);

        /// <summary>
        /// Called when the Clover device transitions away from a screen or activity. The 
        /// CloverDeviceEvent passed in will contain an event type and description. 
        /// Note: The start and end events are not guaranteed to process in order. The 
        /// event type should be used to make sure these events are paired.
        /// </summary>
        /// <param name="deviceEvent">The CloverDeviceEvent event.</param>
        void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent);

        /// <summary>
        /// Called when an error occurs while trying to send messages to the Clover 
        /// device.
        /// </summary>
        /// <param name="deviceErrorEvent">The CloverDeviceErrorEvent event.</param>
        void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent);

        /// <summary>
        /// Called in response to a PreAuth() request. 
        /// Note: The boolean IsPreAuth flag in the PreAuthResponse indicates whether 
        /// CapturePreAuth() can be called for the returned Payment. If the IsPreAuth flag 
        /// is false and the IsAuth flag is true, then the payment gateway coerced the 
        /// PreAuth() request to an Auth. 
        /// The payment will need to be voided or it will be automatically captured at 
        /// closeout.
        /// </summary>
        /// <param name="response">The PreAuthResponse details.</param>
        void OnPreAuthResponse(PreAuthResponse response);

        /// <summary>
        /// Called in response to an Auth() request. 
        /// Note: An Auth transaction may come back as a final Sale, depending on the 
        /// payment gateway. The AuthResponse has a boolean IsAuth flag that indicates 
        /// whether the Payment can still be tip-adjusted.
        /// </summary>
        /// <param name="response">The AuthResponse to the transaction request.</param>
        void OnAuthResponse(AuthResponse response);

        /// <summary>
        /// Called in response to a tip adjustment for an Auth transaction. Contains the 
        /// TipAmount if successful.
        /// </summary>
        /// <param name="response">The TipAdjustAuthResponse to the transaction 
        /// request.</param>
        void OnTipAdjustAuthResponse(TipAdjustAuthResponse response);

        /// <summary>
        /// Called in response to a CapturePreAuth() request. 
        /// Contains the new Amount and TipAmount if successful.
        /// </summary>
        /// <param name="response">The CapturePreAuthResponse to the transaction 
        /// request.</param>
        void OnCapturePreAuthResponse(CapturePreAuthResponse response);

        /// <summary>
        /// Called in response to an IncrementPreAuth() request.
        /// Contains the authorization if successful.
        /// </summary>
        /// <param name="response">The IncrementPreAuthResponse to the transaction</param>
        void OnIncrementPreAuthResponse(IncrementPreAuthResponse response);

        /// <summary>
        /// Called when the Clover device requests verification for a user's on-screen 
        /// signature. The Payment and Signature will be passed in.
        /// </summary>
        /// <param name="request">The VerifySignatureRequest.</param>
        void OnVerifySignatureRequest(VerifySignatureRequest request);

        /// <summary>
        /// Called when the Clover device encounters a Challenge at the payment gateway 
        /// and requires confirmation. A Challenge is triggered by a potential duplicate 
        /// Payment (DUPLICATE_CHALLENGE) or an offline Payment (OFFLINE_CHALLENGE). The 
        /// device sends an OnConfirmPaymentRequest() asking the merchant to reply by 
        /// sending either an AcceptPayment() or RejectPayment() call.
        ///
        /// Note: Duplicate Payment Challenges are raised when multiple Payments are made 
        /// with the same card type and last four digits within the same hour. For this 
        /// reason, we recommend that you do not programmatically call 
        /// CloverConnector.RejectPayment() on all instances of DUPLICATE_CHALLENGE. 
        /// For more information, see {@link 
        /// https://docs.clover.com/build/working-with-challenges/|Working with 
        /// Challenges}. 
        /// </summary>
        /// <param name="request">The ConfirmPaymentRequest for confirmation.</param>
        void OnConfirmPaymentRequest(ConfirmPaymentRequest request);

        /// <summary>
        /// Called in response to a Closeout() request.
        /// </summary>
        /// <param name="response">The CloseoutResponse details for the transaction 
        /// request.</param>
        void OnCloseoutResponse(CloseoutResponse response);

        /// <summary>
        /// Called at the completion of a Sale() request. The SaleResponse contains a 
        /// {@see com.clover.remote.client.messages.ResultCode} and a Success boolean. 
        /// A successful Sale transaction will also have the Payment object, which can be 
        /// for the full or partial amount of the Sale request. Note: A Sale transaction 
        /// my come back as a tip-adjustable Auth, depending on the payment gateway. The 
        /// SaleResponse has a boolean IsSale flag that indicates whether 
        /// the Sale is final, or will be finalized during closeout.
        /// </summary>
        /// <param name="response">The SaleResponse details for the transaction 
        /// request.</param>
        void OnSaleResponse(SaleResponse response);

        /// <summary>
        /// Called in response to a ManualRefund() request. Contains a 
        /// {@see com.clover.remote.client.messages.ResultCode} and a Success boolean. If 
        /// successful, the ManualRefundResponse will have a Credit object associated with 
        /// the relevant Payment information.
        /// </summary>
        /// <param name="response">The ManualRefundResponse details for the transaction 
        /// request.</param>
        void OnManualRefundResponse(ManualRefundResponse response);

        /// <summary>
        /// Called in response to a RefundPayment() request. Contains a 
        /// {@see com.clover.remote.client.messages.ResultCode} and a Success boolean. The 
        /// response to a successful transaction will contain the Refund, which includes 
        /// the original paymentId as a reference.
        /// </summary>
        /// <param name="response">The RefundPaymentResponse details for the transaction 
        /// request.</param>
        void OnRefundPaymentResponse(RefundPaymentResponse response);

        /// <summary>
        /// Called in response to a VoidPaymentRefund() request with results.
        /// </summary>
        /// <param name="response"></param>
        void OnVoidPaymentRefundResponse(VoidPaymentRefundResponse response);

        /// <summary>
        /// Called when a customer selects a tip amount on the Clover device's screen.
        /// </summary>
        /// <param name="message">The TipAddedMessage.</param>
        void OnTipAdded(TipAddedMessage message);

        /// <summary>
        /// Called in response to a VoidPayment() request. Contains a 
        /// {@see com.clover.remote.client.messages.ResultCode} and a Success boolean. If 
        /// successful, the response will also contain the paymentId for the voided 
        /// Payment.
        /// </summary>
        /// <param name="response">The VoidPaymentResponse details for the transaction 
        /// request.</param>
        void OnVoidPaymentResponse(VoidPaymentResponse response);

        /// <summary>
        /// Called when the Clover device is initially connected, but not ready to 
        /// communicate.
        /// </summary>
        void OnDeviceConnected();

        /// <summary>
        /// Called when the Clover device is ready to communicate and respond to requests.
        /// </summary>
        /// <param name="merchantInfo">The MerchantInfo details to associate with the 
        /// device.</param>
        void OnDeviceReady(MerchantInfo merchantInfo);

        /// <summary>
        /// Called when the Clover device is disconnected from the CloverConnector or not 
        /// responding.
        /// </summary>
        void OnDeviceDisconnected();

        /// <summary>
        /// Called in response to a VaultCard() request. Contains a 
        /// {@see com.clover.remote.client.messages.ResultCode} and a Success boolean. If 
        /// successful, the response will contain a VaultedCard object with a token value 
        /// that's unique for the card and merchant. The token can be used for future 
        /// Sale() and Auth() requests.
        /// </summary>
        /// <param name="response">The VaultCardResponse details for the request.</param>
        void OnVaultCardResponse(VaultCardResponse response);

        /// <summary>
        /// Called in response to a RetrievePendingPayment() request.
        /// </summary>
        /// <param name="response">The RetrievePendingPaymentsResponse details for the 
        /// request.</param>
        void OnRetrievePendingPaymentsResponse(RetrievePendingPaymentsResponse response);

        /// <summary>
        /// Called in response to a ReadCardData() request. Contains card information 
        /// (specifically Track 1 and Track 2 card data).
        /// </summary>
        /// <param name="response">The ReadCardDataResponse details for the 
        /// request.</param>
        void OnReadCardDataResponse(ReadCardDataResponse response);

        /// <summary>
        /// Called when a user requests a paper receipt for a Manual Refund. Will only be 
        /// called if DisablePrinting = true on the ManualRefund() request.
        /// </summary>
        /// <param name="message">A callback that asks the POS to 
        /// print a receipt for a ManualRefund. Contains a Credit object.</param>
        void OnPrintManualRefundReceipt(PrintManualRefundReceiptMessage message);

        /// <summary>
        /// Called when a user requests a paper receipt for a declined Manual Refund. Will 
        /// only be called if DisablePrinting = true on the ManualRefund() request.
        /// </summary>
        /// <param name="message">The 
        /// PrintManualRefundDeclineReceiptMessage.</param>
        void OnPrintManualRefundDeclineReceipt(PrintManualRefundDeclineReceiptMessage message);

        /// <summary>
        /// Called when a user requests a paper receipt for a Payment. Will only be called 
        /// if DisablePrinting = true on the Sale(), Auth(), or PreAuth() request.
        /// </summary>
        /// <param name="message">The PrintPaymentReceiptMessage 
        /// details.</param>
        void OnPrintPaymentReceipt(PrintPaymentReceiptMessage message);

        /// <summary>
        /// Called when a user requests a paper receipt for a declined Payment.  Will only 
        /// be called if DisablePrinting = true on the Sale(), Auth(), or PreAuth() 
        /// request.
        /// </summary>
        /// <param name="message">The 
        /// PrintPaymentDeclineReceiptMessage details.</param>
        void OnPrintPaymentDeclineReceipt(PrintPaymentDeclineReceiptMessage message);

        /// <summary>
        /// Called when a user requests a merchant copy of a Payment receipt. Will only be 
        /// called if DisablePrinting = true on the Sale(), Auth(), or PreAuth() request.
        /// </summary>
        /// <param name="message">The 
        /// PrintPaymentMerchantCopyReceiptMessage details.</param>
        void OnPrintPaymentMerchantCopyReceipt(PrintPaymentMerchantCopyReceiptMessage message);

        /// <summary>
        /// Called when a user requests a paper receipt for a Payment Refund. Will only be 
        /// called if DisablePrinting = true on the Sale(), Auth(), PreAuth() or 
        /// ManualRefund() request.
        /// </summary>
        /// <param name="message">The 
        /// PrintRefundPaymentReceiptMessage details.</param>
        void OnPrintRefundPaymentReceipt(PrintRefundPaymentReceiptMessage message);

        /// <summary>
        /// Called in response to a RetrievePrintJobStatus() request.
        /// </summary>
        /// <param name="response">The PrintJobStatusResponse details for the 
        /// request.</param>
        void OnPrintJobStatusResponse(PrintJobStatusResponse response);

        /// <summary>
        /// Called in response to a RetrievePrinters() request.
        /// </summary>
        /// <param name="response">The RetrievePrintersResponse details for the 
        /// request.</param>
        void OnRetrievePrintersResponse(RetrievePrintersResponse response);

        /// <summary>
        /// Called when a Custom Activity finishes normally.
        /// </summary>
        /// <param name="response">The CustomActivityResponse.</param>
        void OnCustomActivityResponse(CustomActivityResponse response);

        /// <summary>
        /// Called in response to a RetrieveDeviceStatus() request.
        /// </summary>
        /// <param name="response">The RetrieveDeviceStatusResponse details for the 
        /// request.</param>
        void OnRetrieveDeviceStatusResponse(RetrieveDeviceStatusResponse response);

        /// <summary>
        /// Called when a Custom Activity sends a message to the POS.
        /// </summary>
        /// <param name="response">The MessageFromActivity details.</param>
        void OnMessageFromActivity(MessageFromActivity response);

        /// <summary>
        /// Called in response to a ResetDevice() request.
        /// </summary>
        /// <param name="response">The ResetDeviceResponse details for the 
        /// request.</param>
        void OnResetDeviceResponse(ResetDeviceResponse response);

        /// <summary>
        /// Called in response to a RetrievePayment() request.
        /// </summary>
        /// <param name="response">The RetrievePaymentResponse details for the 
        /// request.</param>
        void OnRetrievePaymentResponse(RetrievePaymentResponse response);

        /// <summary>
        /// Called in response to a RetrievePrintJobStatus() request.
        /// </summary>
        /// <param name="request">The PrintJobStatusResponse details for the 
        /// request.</param>
        void OnPrintJobStatusRequest(PrintJobStatusRequest request);

        /// <summary>
        /// Called in response to a DisplayReceiptOptions request.
        /// </summary>
        /// <param name="response">The DisplayReceiptOptionsResponse details for the response.</param>
        void OnDisplayReceiptOptionsResponse(DisplayReceiptOptionsResponse response);

        /// <summary>
        /// Called to notify the Point of Sale that a call was invalid in the current context or an internal state change failed.
        /// </summary>
        /// <param name="message"></param>
        void OnInvalidStateTransitionResponse(InvalidStateTransitionNotification message);

        /// <summary>
        /// Called when Loyalty API sends customer identifying data for a loyalty data type subscribed to in CloverConnector.RegisterForCustomerProvidedData()
        /// </summary>
        /// <param name="response">The CustomerProvidedDataEvent containing type and data payload, like PHONE, 555-1212 in type-specific custom encoding</param>
        void OnCustomerProvidedData(CustomerProvidedDataEvent response);
    }

    /// <summary>
    /// This is a default implementation of the ICloverConnectorListener
    /// that can be used for quickly creating applications by 
    /// simply overriding the appropriate listener method(s) needed
    /// for testing a particular remote call.
    ///
    /// Also see the CloverEventConnector for another option to receive
    /// events instead of overriding the class and receiving method calls
    /// </summary>
    public abstract class DefaultCloverConnectorListener : ICloverConnectorListener
    {
        ICloverConnector cloverConnector;

        public abstract void OnConfirmPaymentRequest(ConfirmPaymentRequest request);

        protected DefaultCloverConnectorListener(ICloverConnector cloverConnector)
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

        public virtual void OnIncrementPreAuthResponse(IncrementPreAuthResponse response)
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

        public virtual void OnVoidPaymentResponse(VoidPaymentResponse response)
        {
        }

        public virtual void OnVoidPaymentRefundResponse(VoidPaymentRefundResponse response)
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

        public virtual void OnPrintManualRefundReceipt(PrintManualRefundReceiptMessage message)
        {
        }

        public virtual void OnPrintManualRefundDeclineReceipt(PrintManualRefundDeclineReceiptMessage message)
        {
        }

        public virtual void OnPrintPaymentReceipt(PrintPaymentReceiptMessage message)
        {
        }

        public virtual void OnPrintPaymentDeclineReceipt(PrintPaymentDeclineReceiptMessage message)
        {
        }

        public virtual void OnPrintPaymentMerchantCopyReceipt(PrintPaymentMerchantCopyReceiptMessage message)
        {
        }

        public virtual void OnPrintRefundPaymentReceipt(PrintRefundPaymentReceiptMessage message)
        {
        }

        public virtual void OnRetrieveDeviceStatusResponse(RetrieveDeviceStatusResponse response)
        {
        }

        public virtual void OnMessageFromActivity(MessageFromActivity response)
        {
        }

        public virtual void OnResetDeviceResponse(ResetDeviceResponse response)
        {
        }

        public virtual void OnRetrievePaymentResponse(RetrievePaymentResponse response)
        {
        }

        public virtual void OnPrintJobStatusResponse(PrintJobStatusResponse response)
        {
        }

        public virtual void OnPrintJobStatusRequest(PrintJobStatusRequest request)
        {
        }

        public virtual void OnRetrievePrintersResponse(RetrievePrintersResponse response)
        {
        }

        public virtual void OnDisplayReceiptOptionsResponse(DisplayReceiptOptionsResponse response)
        {
        }

        public virtual void OnCustomerProvidedData(CustomerProvidedDataEvent response)
        {
        }

        public virtual void OnInvalidStateTransitionResponse(InvalidStateTransitionNotification notification)
        {
        }
    }
}
