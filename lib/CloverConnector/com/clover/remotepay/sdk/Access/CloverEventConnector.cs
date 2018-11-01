using System;
using System.Collections.Generic;
using System.Drawing;
using com.clover.remote.order;
using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using com.clover.sdk.v3.payments;

namespace Clover.RemotePay
{
    /// <summary>
    /// Clover Connector and Clover Connector Listener wrapper for easy .NET event consumption of standard Clover API.
    /// This is an alternate access Ease of Use layer wrapping the standard Clover Connector API for designs that would prefer to consume an Event model API.
    /// 
    /// Standard CloverConnector API calls are made to this object
    /// The single Message event can be subscribed to for a central message handler design. All messages raise the common Message event.
    /// Individual response events can be subscribed to in order to handle specific messages. All messages raise their own custom event.
    /// </summary>
    public class CloverEventConnector : ICloverConnector, ICloverConnectorListener
    {
        private ICloverConnector connector;

        /// <summary>
        /// Create an empty CloverEventConnector
        /// Attach an existing ICloverConnector object or create a new on to use this CloverEventConnector
        /// </summary>
        public CloverEventConnector()
        {
        }

        /// <summary>
        /// Construct a CloverEventConnector from an existing CloverConnector object that has already been setup
        /// </summary>
        /// <param name="existingCloverConnector">Existing CloverConnector SDK object managed by the POS</param>
        public CloverEventConnector(ICloverConnector existingCloverConnector)
        {
            Attach(existingCloverConnector);
        }

        /// <summary>
        /// Attach an existing CloverConnector to this CloverEventConnector as the one and only CloverConnector SDK object
        /// </summary>
        /// <param name="existingCloverConnector"></param>
        public void Attach(ICloverConnector existingCloverConnector)
        {
            if (connector != null)
            {
                connector.RemoveCloverConnectorListener(this);
            }

            connector = existingCloverConnector;
            connector.AddCloverConnectorListener(this);
        }

        #region CloverConnector

        /// <inheritdoc />
        public void InitializeConnection()
        {
            connector.InitializeConnection();
        }

        /// <inheritdoc />
        public void AddCloverConnectorListener(ICloverConnectorListener connectorListener)
        {
            connector.AddCloverConnectorListener(connectorListener);
        }

        /// <inheritdoc />
        public void RemoveCloverConnectorListener(ICloverConnectorListener connectorListener)
        {
            connector.RemoveCloverConnectorListener(connectorListener);
        }

        /// <inheritdoc />
        public void Sale(SaleRequest request)
        {
            connector.Sale(request);
        }

        /// <inheritdoc />
        public void AcceptSignature(VerifySignatureRequest request)
        {
            connector.AcceptSignature(request);
        }

        /// <inheritdoc />
        public void RejectSignature(VerifySignatureRequest request)
        {
            connector.RejectSignature(request);
        }

        /// <inheritdoc />
        public void AcceptPayment(Payment payment)
        {
            connector.AcceptPayment(payment);
        }

        /// <inheritdoc />
        public void RejectPayment(Payment payment, Challenge challenge)
        {
            connector.RejectPayment(payment, challenge);
        }

        /// <inheritdoc />
        public void Auth(AuthRequest request)
        {
            connector.Auth(request);
        }

        /// <inheritdoc />
        public void PreAuth(PreAuthRequest request)
        {
            connector.PreAuth(request);
        }

        /// <inheritdoc />
        public void CapturePreAuth(CapturePreAuthRequest request)
        {
            connector.CapturePreAuth(request);
        }

        /// <inheritdoc />
        public void TipAdjustAuth(TipAdjustAuthRequest request)
        {
            connector.TipAdjustAuth(request);
        }

        /// <inheritdoc />
        public void VoidPayment(VoidPaymentRequest request)
        {
            connector.VoidPayment(request);
        }

        /// <inheritdoc />
        public void VoidPaymentRefund(VoidPaymentRefundRequest request)
        {
            connector.VoidPaymentRefund(request);
        }

        /// <inheritdoc />
        public void RefundPayment(RefundPaymentRequest request)
        {
            connector.RefundPayment(request);
        }

        /// <inheritdoc />
        public void ManualRefund(ManualRefundRequest request)
        {
            connector.ManualRefund(request);
        }

        /// <inheritdoc />
        public void VaultCard(int? CardEntryMethods)
        {
            connector.VaultCard(CardEntryMethods);
        }

        /// <inheritdoc />
        public void ReadCardData(ReadCardDataRequest request)
        {
            connector.ReadCardData(request);
        }


        /// <inheritdoc />
        public void Closeout(CloseoutRequest request)
        {
            connector.Closeout(request);
        }

        /// <inheritdoc />
        public void ResetDevice()
        {
            connector.ResetDevice();
        }

        /// <inheritdoc />
        public void ShowMessage(string message)
        {
            connector.ShowMessage(message);
        }

        /// <inheritdoc />
        public void ShowWelcomeScreen()
        {
            connector.ShowWelcomeScreen();
        }

        /// <inheritdoc />
        public void ShowThankYouScreen()
        {
            connector.ShowThankYouScreen();
        }

        /// <inheritdoc />
        public void DisplayPaymentReceiptOptions(DisplayPaymentReceiptOptionsRequest request)
        {
            connector.DisplayPaymentReceiptOptions(request);
        }

        /// <inheritdoc />
        public void OpenCashDrawer(OpenCashDrawerRequest request)
        {
            connector.OpenCashDrawer(request);
        }

        /// <inheritdoc />
        public void ShowDisplayOrder(DisplayOrder order)
        {
            connector.ShowDisplayOrder(order);
        }

        /// <inheritdoc />
        public void RemoveDisplayOrder(DisplayOrder order)
        {
            connector.RemoveDisplayOrder(order);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            connector.Dispose();
        }

        /// <inheritdoc />
        public void InvokeInputOption(InputOption io)
        {
            connector.InvokeInputOption(io);
        }

        /// <inheritdoc />
        public void Print(PrintRequest request)
        {
            connector.Print(request);
        }

        /// <inheritdoc />
        public void RetrievePrinters(RetrievePrintersRequest request)
        {
            connector.RetrievePrinters(request);
        }

        /// <inheritdoc />
        public void RetrievePrintJobStatus(PrintJobStatusRequest request)
        {
            connector.RetrievePrintJobStatus(request);
        }

        /// <inheritdoc />
        public void RetrievePendingPayments()
        {
            connector.RetrievePendingPayments();
        }

        /// <inheritdoc />
        public void StartCustomActivity(CustomActivityRequest request)
        {
            connector.StartCustomActivity(request);
        }

        /// <inheritdoc />
        public void SendMessageToActivity(MessageToActivity request)
        {
            connector.SendMessageToActivity(request);
        }

        /// <inheritdoc />
        public void RetrieveDeviceStatus(RetrieveDeviceStatusRequest request)
        {
            connector.RetrieveDeviceStatus(request);
        }

        /// <inheritdoc />
        public void RetrievePayment(RetrievePaymentRequest request)
        {
            connector.RetrievePayment(request);
        }

        /// <inheritdoc />
        public void DisplayReceiptOptions(DisplayReceiptOptionsRequest request)
        {
            connector.DisplayReceiptOptions(request);
        }

        /// <inheritdoc />
        public void RegisterForCustomerProvidedData(RegisterForCustomerProvidedDataRequest request)
        {
            connector.RegisterForCustomerProvidedData(request);
        }

        /// <inheritdoc />
        public void SetCustomerInfo(SetCustomerInfoRequest request)
        {
            connector.SetCustomerInfo(request);
        }

        #endregion

        #region ICloverConnectorListener

        #region Event definitions - Attach event handlers as necessary to receive specific ICloverConnectorListener signal messages from the Clover Device
        // The events send by this Clover Connector Listener when the Clover Device commumicates to user code via the SDK

        /// <summary>
        /// Catch-all event signal raised by every ICloverConnectorListener handler in addition to their custom event to enable a central Message Handling pattern
        /// The Message event is raised after the custom event, and you may set the CloverEventArgs.handled flag if you wish to do either/or message processing
        /// </summary>
        public event MessageHandler Message;

        /// <summary>
        /// Device Activity message - UI screen has changed and is being shown
        /// </summary>
        public event DeviceActivityStartHandler DeviceActivityStart;
        /// <summary>
        /// Device Activity message - UI screen is changing and current screen is being hidden
        /// </summary>
        public event DeviceActivityEndHandler DeviceActivityEnd;
        /// <summary>
        /// There was an error from the device or SDK layer, usually directly related to the last call
        /// </summary>
        public event DeviceErrorHandler DeviceError;
        /// <summary>
        /// PreAuth call has completed with these details (success or failure)
        /// </summary>
        public event PreAuthResponseHandler PreAuthResponse;
        /// <summary>
        /// Auth call has completed with these details (success or failure)
        /// </summary>
        public event AuthResponseHandler AuthResponse;
        /// <summary>
        /// Tip Adjust call has completed with these details (success or failure)
        /// </summary>
        public event TipAdjustAuthResponseHandler TipAdjustAuthResponse;
        /// <summary>
        /// Capture PreAuth call has completed with these details (success or failure)
        /// </summary>
        public event CapturePreAuthResponseHandler CapturePreAuthResponse;
        /// <summary>
        /// The customer has signed for the payment, the POS needs to confirm the signature so the payment can continue (eg Cashier compares signature to card as appropriate)
        /// </summary>
        public event VerifySignatureRequestHandler VerifySignatureRequest;
        /// <summary>
        /// The payment was detected as a possible duplicate or the device is offline, the POS needs to confirm the payment is acceptable to continue (eg Cashier confirms it isn't a duplicate payment and should be processed)
        /// </summary>
        public event ConfirmPaymentRequestHandler ConfirmPaymentRequest;
        /// <summary>
        /// Closeout call has completed with these details (success or failure)
        /// </summary>
        public event CloseoutResponseHandler CloseoutResponse;
        /// <summary>
        /// Sale call has completed with these details (success or failure)
        /// </summary>
        public event SaleResponseHandler SaleResponse;
        /// <summary>
        /// Manual Refund call has completed with these details (success or failure)
        /// </summary>
        public event ManualRefundResponseHandler ManualRefundResponse;
        /// <summary>
        /// Refund call has completed with these details (success or failure)
        /// </summary>
        public event RefundPaymentResponseHandler RefundPaymentResponse;
        /// <summary>
        /// Customer added a tip to the order on the device screen
        /// </summary>
        public event TipAddedHandler TipAdded;
        /// <summary>
        /// Void Payment call has completed with these details (success or failure)
        /// </summary>
        public event VoidPaymentResponseHandler VoidPaymentResponse;
        /// <summary>
        /// Void Payment Refund call has completed with these details (success or failure)
        /// </summary>
        public event VoidPaymentRefundResponseHandler VoidPaymentRefundResponse;
        /// <summary>
        /// A Clover Device was connected to the SDK and the SDK can see it
        /// </summary>
        public event DeviceConnectedHandler DeviceConnected;
        /// <summary>
        /// A Clover Device was connected to and initialized by the SDK and is ready to use
        /// </summary>
        public event DeviceReadyHandler DeviceReady;
        /// <summary>
        /// An attached Clover Device lost connection to the SDK and cannot be used (device power cycled, unplugged, network connection lost, etc.)
        /// </summary>
        public event DeviceDisconnectedHandler DeviceDisconnected;
        /// <summary>
        /// Vault Card call has completed with these details (success or failure)
        /// </summary>
        public event VaultCardResponseHandler VaultCardResponse;
        /// <summary>
        /// List of Pending Payments on the device in need of transmitting to server (ie Payments taken and queued for later processing while device was offline / in forced offline mode)
        /// </summary>
        public event RetrievePendingPaymentsResponseHandler RetrievePendingPaymentsResponse;
        /// <summary>
        /// Read Card Data call has completed with these details (success or failure)
        /// </summary>
        public event ReadCardDataResponseHandler ReadCardDataResponse;
        /// <summary>
        /// Device wants a receipt printed for Manual Refund (ie in POS Print mode)
        /// </summary>
        public event PrintManualRefundReceiptHandler PrintManualRefundReceipt;
        /// <summary>
        /// Device wants a receipt printed for Manual Refund Declined (ie in POS Print mode)
        /// </summary>
        public event PrintManualRefundDeclineReceiptHandler PrintManualRefundDeclineReceipt;
        /// <summary>
        /// Device wants a receipt printed for a Payment (ie in POS Print mode)
        /// </summary>
        public event PrintPaymentReceiptHandler PrintPaymentReceipt;
        /// <summary>
        /// Device wants a receipt printed for a Payment Declined (ie in POS Print mode)
        /// </summary>
        public event PrintPaymentDeclineReceiptHandler PrintPaymentDeclineReceipt;
        /// <summary>
        /// Device wants a receipt printed for a Payment (ie in POS Print mode)
        /// </summary>
        public event PrintPaymentMerchantCopyReceiptHandler PrintPaymentMerchantCopyReceipt;
        /// <summary>
        /// Device wants a receipt printed for a Refunded Payment (ie in POS Print mode)
        /// </summary>
        public event PrintRefundPaymentReceiptHandler PrintRefundPaymentReceipt;
        /// <summary>
        /// Device wants a receipt printed for a Payment (ie in POS Print mode)
        /// </summary>
        public event PrintJobStatusResponseHandler PrintJobStatusResponse;
        /// <summary>
        /// Print Job status details call has completed with these details (success or failure)
        /// </summary>
        public event RetrievePrintersResponseHandler RetrievePrintersResponse;
        /// <summary>
        /// Custom Activity call has completed with these details (success or failure)
        /// </summary>
        public event CustomActivityResponseHandler CustomActivityResponse;
        /// <summary>
        /// Response to a Retrieve Device Status with current status (Idle, Busy, etc.)
        /// </summary>
        public event RetrieveDeviceStatusResponseHandler RetrieveDeviceStatusResponse;
        /// <summary>
        /// Custom Activity user code on the Device sent a custom message through the SDK to the POS
        /// </summary>
        public event MessageFromActivityHandler MessageFromActivity;
        /// <summary>
        /// Reset Device call has completed with these details (success or failure)
        /// </summary>
        public event ResetDeviceResponseHandler ResetDeviceResponse;
        /// <summary>
        /// Retrieve Payment call has completed with these payment details (success or failure)
        /// </summary>
        public event RetrievePaymentResponseHandler RetrievePaymentResponse;
        /// <summary>
        /// Print Job Status call has completed with these details (success or failure)
        /// </summary>
        public event PrintJobStatusRequestHandler PrintJobStatusRequest;
        /// <summary>
        /// Display Receipt Options call has completed with these details (success or failure)
        /// </summary>
        public event DisplayReceiptOptionsResponseHandler DisplayReceiptOptionsResponse;
        /// <summary>
        /// Display Receipt Options call has completed with these details (success or failure)
        /// </summary>
        public event CustomerProvidedDataResponseHandler CustomerProvidedDataResponse;

        #endregion

        #region Internal ICloverConnectorListener interface impelementation. Receives the calls from the Clover SDK from the device and converts them to .Net event messages

        public void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {
            DeviceActivityStartEventArgs eventArgs = new DeviceActivityStartEventArgs
            {
                cloverMessage = CloverMessage.DeviceActivityStart,
                cloverConnector = this,
                deviceEvent = deviceEvent
            };
            DeviceActivityStart?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
            DeviceActivityEndEventArgs eventArgs = new DeviceActivityEndEventArgs()
            {
                cloverMessage = CloverMessage.DeviceActivityEnd,
                cloverConnector = this,
                deviceEvent = deviceEvent
            };
            DeviceActivityEnd?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {
            DeviceErrorEventArgs eventArgs = new DeviceErrorEventArgs()
            {
                cloverMessage = CloverMessage.DeviceError,
                cloverConnector = this,
                deviceErrorEvent = deviceErrorEvent
            };
            DeviceError?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnPreAuthResponse(PreAuthResponse response)
        {
            PreAuthResponseEventArgs eventArgs = new PreAuthResponseEventArgs()
            {
                cloverMessage = CloverMessage.PreAuthResponse,
                cloverConnector = this,
                response = response
            };
            PreAuthResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnAuthResponse(AuthResponse response)
        {
            AuthResponseEventArgs eventArgs = new AuthResponseEventArgs()
            {
                cloverMessage = CloverMessage.AuthResponse,
                cloverConnector = this,
                response = response
            };
            AuthResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnTipAdjustAuthResponse(TipAdjustAuthResponse response)
        {
            TipAdjustAuthResponseEventArgs eventArgs = new TipAdjustAuthResponseEventArgs()
            {
                cloverMessage = CloverMessage.TipAdjustAuthResponse,
                cloverConnector = this,
                response = response
            };
            TipAdjustAuthResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnCapturePreAuthResponse(CapturePreAuthResponse response)
        {
            CapturePreAuthResponseEventArgs eventArgs = new CapturePreAuthResponseEventArgs()
            {
                cloverMessage = CloverMessage.CapturePreAuthResponse,
                cloverConnector = this,
                response = response
            };
            CapturePreAuthResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnVerifySignatureRequest(VerifySignatureRequest request)
        {
            VerifySignatureRequestEventArgs eventArgs = new VerifySignatureRequestEventArgs()
            {
                cloverMessage = CloverMessage.VerifySignatureRequest,
                cloverConnector = this,
                request = request
            };
            VerifySignatureRequest?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnConfirmPaymentRequest(ConfirmPaymentRequest request)
        {
            ConfirmPaymentRequestEventArgs eventArgs = new ConfirmPaymentRequestEventArgs()
            {
                cloverMessage = CloverMessage.ConfirmPaymentRequest,
                cloverConnector = this,
                request = request
            };
            ConfirmPaymentRequest?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnCloseoutResponse(CloseoutResponse response)
        {
            CloseoutResponseEventArgs eventArgs = new CloseoutResponseEventArgs()
            {
                cloverMessage = CloverMessage.CloseoutResponse,
                cloverConnector = this,
                response = response
            };
            CloseoutResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnSaleResponse(SaleResponse response)
        {
            SaleResponseEventArgs eventArgs = new SaleResponseEventArgs()
            {
                cloverMessage = CloverMessage.SaleResponse,
                cloverConnector = this,
                response = response
            };
            SaleResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnManualRefundResponse(ManualRefundResponse response)
        {
            ManualRefundResponseEventArgs eventArgs = new ManualRefundResponseEventArgs()
            {
                cloverMessage = CloverMessage.ManualRefundResponse,
                cloverConnector = this,
                response = response
            };
            ManualRefundResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnRefundPaymentResponse(RefundPaymentResponse response)
        {
            RefundPaymentResponseEventArgs eventArgs = new RefundPaymentResponseEventArgs()
            {
                cloverMessage = CloverMessage.RefundPaymentResponse,
                cloverConnector = this,
                response = response
            };
            RefundPaymentResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnTipAdded(TipAddedMessage message)
        {
            TipAddedEventArgs eventArgs = new TipAddedEventArgs()
            {
                cloverMessage = CloverMessage.TipAdded,
                cloverConnector = this,
                message = message
            };
            TipAdded?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }
        public void OnVoidPaymentResponse(VoidPaymentResponse response)
        {
            VoidPaymentResponseEventArgs eventArgs = new VoidPaymentResponseEventArgs()
            {
                cloverMessage = CloverMessage.VoidPaymentResponse,
                cloverConnector = this,
                response = response
            };
            VoidPaymentResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }
        public void OnVoidPaymentRefundResponse(VoidPaymentRefundResponse response)
        {
            VoidPaymentRefundResponseEventArgs eventArgs = new VoidPaymentRefundResponseEventArgs()
            {
                cloverMessage = CloverMessage.VoidPaymentRefundResponse,
                cloverConnector = this,
                response = response
            };
            VoidPaymentRefundResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }
        public void OnDeviceConnected()
        {
            DeviceConnectedEventArgs eventArgs = new DeviceConnectedEventArgs()
            {
                cloverMessage = CloverMessage.DeviceConnected,
                cloverConnector = this
            };
            DeviceConnected?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnDeviceReady(MerchantInfo merchantInfo)
        {
            DeviceReadyEventArgs eventArgs = new DeviceReadyEventArgs()
            {
                cloverMessage = CloverMessage.DeviceReady,
                cloverConnector = this,
                merchantInfo = merchantInfo
            };
            DeviceReady?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnDeviceDisconnected()
        {
            DeviceDisconnectedEventArgs eventArgs = new DeviceDisconnectedEventArgs()
            {
                cloverMessage = CloverMessage.DeviceDisconnected,
                cloverConnector = this
            };
            DeviceDisconnected?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnVaultCardResponse(VaultCardResponse response)
        {
            VaultCardResponseEventArgs eventArgs = new VaultCardResponseEventArgs()
            {
                cloverMessage = CloverMessage.VaultCardResponse,
                cloverConnector = this,
                response = response
            };
            VaultCardResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnRetrievePendingPaymentsResponse(RetrievePendingPaymentsResponse response)
        {
            RetrievePendingPaymentsResponseEventArgs eventArgs = new RetrievePendingPaymentsResponseEventArgs()
            {
                cloverMessage = CloverMessage.RetrievePendingPaymentsResponse,
                cloverConnector = this,
                response = response
            };
            RetrievePendingPaymentsResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnReadCardDataResponse(ReadCardDataResponse response)
        {
            ReadCardDataResponseEventArgs eventArgs = new ReadCardDataResponseEventArgs()
            {
                cloverMessage = CloverMessage.ReadCardDataResponse,
                cloverConnector = this,
                response = response
            };
            ReadCardDataResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnPrintManualRefundReceipt(PrintManualRefundReceiptMessage printManualRefundReceiptMessage)
        {
            PrintManualRefundReceiptEventArgs eventArgs = new PrintManualRefundReceiptEventArgs()
            {
                cloverMessage = CloverMessage.PrintManualRefundReceipt,
                cloverConnector = this,
                message = printManualRefundReceiptMessage
            };
            PrintManualRefundReceipt?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnPrintManualRefundDeclineReceipt(PrintManualRefundDeclineReceiptMessage printManualRefundDeclineReceiptMessage)
        {
            PrintManualRefundDeclineReceiptEventArgs eventArgs = new PrintManualRefundDeclineReceiptEventArgs()
            {
                cloverMessage = CloverMessage.PrintManualRefundDeclineReceipt,
                cloverConnector = this,
                message = printManualRefundDeclineReceiptMessage
            };
            PrintManualRefundDeclineReceipt?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnPrintPaymentReceipt(PrintPaymentReceiptMessage printPaymentReceiptMessage)
        {
            PrintPaymentReceiptEventArgs eventArgs = new PrintPaymentReceiptEventArgs()
            {
                cloverMessage = CloverMessage.PrintPaymentReceipt,
                cloverConnector = this,
                message = printPaymentReceiptMessage
            };
            PrintPaymentReceipt?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnPrintPaymentDeclineReceipt(PrintPaymentDeclineReceiptMessage printPaymentDeclineReceiptMessage)
        {
            PrintPaymentDeclineReceiptEventArgs eventArgs = new PrintPaymentDeclineReceiptEventArgs()
            {
                cloverMessage = CloverMessage.PrintPaymentDeclineReceipt,
                cloverConnector = this,
                message = printPaymentDeclineReceiptMessage
            };
            PrintPaymentDeclineReceipt?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnPrintPaymentMerchantCopyReceipt(PrintPaymentMerchantCopyReceiptMessage printPaymentMerchantCopyReceiptMessage)
        {
            PrintPaymentMerchantCopyReceiptEventArgs eventArgs = new PrintPaymentMerchantCopyReceiptEventArgs()
            {
                cloverMessage = CloverMessage.PrintPaymentMerchantCopyReceipt,
                cloverConnector = this,
                message = printPaymentMerchantCopyReceiptMessage
            };
            PrintPaymentMerchantCopyReceipt?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnPrintRefundPaymentReceipt(PrintRefundPaymentReceiptMessage printRefundPaymentReceiptMessage)
        {
            PrintRefundPaymentReceiptEventArgs eventArgs = new PrintRefundPaymentReceiptEventArgs()
            {
                cloverMessage = CloverMessage.PrintRefundPaymentReceipt,
                cloverConnector = this,
                message = printRefundPaymentReceiptMessage
            };
            PrintRefundPaymentReceipt?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnPrintJobStatusResponse(PrintJobStatusResponse response)
        {
            PrintJobStatusResponseEventArgs eventArgs = new PrintJobStatusResponseEventArgs()
            {
                cloverMessage = CloverMessage.PrintJobStatusResponse,
                cloverConnector = this,
                response = response
            };
            PrintJobStatusResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnRetrievePrintersResponse(RetrievePrintersResponse response)
        {
            RetrievePrintersResponseEventArgs eventArgs = new RetrievePrintersResponseEventArgs()
            {
                cloverMessage = CloverMessage.RetrievePrintersResponse,
                cloverConnector = this,
                response = response
            };
            RetrievePrintersResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnCustomActivityResponse(CustomActivityResponse response)
        {
            CustomActivityResponseEventArgs eventArgs = new CustomActivityResponseEventArgs()
            {
                cloverMessage = CloverMessage.CustomActivityResponse,
                cloverConnector = this,
                response = response
            };
            CustomActivityResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnRetrieveDeviceStatusResponse(RetrieveDeviceStatusResponse response)
        {
            RetrieveDeviceStatusResponseEventArgs eventArgs = new RetrieveDeviceStatusResponseEventArgs()
            {
                cloverMessage = CloverMessage.RetrieveDeviceStatusResponse,
                cloverConnector = this,
                response = response
            };
            RetrieveDeviceStatusResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnMessageFromActivity(MessageFromActivity response)
        {
            MessageFromActivityEventArgs eventArgs = new MessageFromActivityEventArgs()
            {
                cloverMessage = CloverMessage.MessageFromActivity,
                cloverConnector = this,
                response = response
            };
            MessageFromActivity?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnResetDeviceResponse(ResetDeviceResponse response)
        {
            ResetDeviceResponseEventArgs eventArgs = new ResetDeviceResponseEventArgs()
            {
                cloverMessage = CloverMessage.ResetDeviceResponse,
                cloverConnector = this,
                response = response
            };
            ResetDeviceResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnRetrievePaymentResponse(RetrievePaymentResponse response)
        {
            RetrievePaymentResponseEventArgs eventArgs = new RetrievePaymentResponseEventArgs()
            {
                cloverMessage = CloverMessage.RetrievePaymentResponse,
                cloverConnector = this,
                response = response
            };
            RetrievePaymentResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnPrintJobStatusRequest(PrintJobStatusRequest request)
        {
            PrintJobStatusRequestEventArgs eventArgs = new PrintJobStatusRequestEventArgs()
            {
                cloverMessage = CloverMessage.PrintJobStatusRequest,
                cloverConnector = this,
                request = request
            };
            PrintJobStatusRequest?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnDisplayReceiptOptionsResponse(DisplayReceiptOptionsResponse response)
        {
            DisplayReceiptOptionsResponseEventArgs eventArgs = new DisplayReceiptOptionsResponseEventArgs()
            {
                cloverMessage = CloverMessage.DisplayReceiptOptionsResponse,
                cloverConnector = this,
                response = response
            };
            DisplayReceiptOptionsResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        public void OnCustomerProvidedData(CustomerProvidedDataEvent response)
        {
            CustomerProvidedDataResponseEventArgs eventArgs = new CustomerProvidedDataResponseEventArgs
            {
                response = response
            };
            CustomerProvidedDataResponse?.Invoke(this, eventArgs);
            Message?.Invoke(this, eventArgs);
        }

        #endregion

        #region Event Delegate definitions

        // Setup event delegates to get good default parameter names
        public delegate void MessageHandler(object sender, CloverEventArgs e);
        public delegate void DeviceActivityStartHandler(object sender, DeviceActivityStartEventArgs e);
        public delegate void DeviceActivityEndHandler(object sender, DeviceActivityEndEventArgs e);
        public delegate void DeviceErrorHandler(object sender, DeviceErrorEventArgs e);
        public delegate void PreAuthResponseHandler(object sender, PreAuthResponseEventArgs e);
        public delegate void AuthResponseHandler(object sender, AuthResponseEventArgs e);
        public delegate void TipAdjustAuthResponseHandler(object sender, TipAdjustAuthResponseEventArgs e);
        public delegate void CapturePreAuthResponseHandler(object sender, CapturePreAuthResponseEventArgs e);
        public delegate void VerifySignatureRequestHandler(object sender, VerifySignatureRequestEventArgs e);
        public delegate void ConfirmPaymentRequestHandler(object sender, ConfirmPaymentRequestEventArgs e);
        public delegate void CloseoutResponseHandler(object sender, CloseoutResponseEventArgs e);
        public delegate void SaleResponseHandler(object sender, SaleResponseEventArgs e);
        public delegate void ManualRefundResponseHandler(object sender, ManualRefundResponseEventArgs e);
        public delegate void RefundPaymentResponseHandler(object sender, RefundPaymentResponseEventArgs e);
        public delegate void TipAddedHandler(object sender, TipAddedEventArgs e);
        public delegate void VoidPaymentResponseHandler(object sender, VoidPaymentResponseEventArgs e);
        public delegate void VoidPaymentRefundResponseHandler(object sender, VoidPaymentRefundResponseEventArgs e);
        public delegate void DeviceConnectedHandler(object sender, DeviceConnectedEventArgs e);
        public delegate void DeviceReadyHandler(object sender, DeviceReadyEventArgs e);
        public delegate void DeviceDisconnectedHandler(object sender, DeviceDisconnectedEventArgs e);
        public delegate void VaultCardResponseHandler(object sender, VaultCardResponseEventArgs e);
        public delegate void RetrievePendingPaymentsResponseHandler(object sender, RetrievePendingPaymentsResponseEventArgs e);
        public delegate void ReadCardDataResponseHandler(object sender, ReadCardDataResponseEventArgs e);
        public delegate void PrintManualRefundReceiptHandler(object sender, PrintManualRefundReceiptEventArgs e);
        public delegate void PrintManualRefundDeclineReceiptHandler(object sender, PrintManualRefundDeclineReceiptEventArgs e);
        public delegate void PrintPaymentReceiptHandler(object sender, PrintPaymentReceiptEventArgs e);
        public delegate void PrintPaymentDeclineReceiptHandler(object sender, PrintPaymentDeclineReceiptEventArgs e);
        public delegate void PrintPaymentMerchantCopyReceiptHandler(object sender, PrintPaymentMerchantCopyReceiptEventArgs e);
        public delegate void PrintRefundPaymentReceiptHandler(object sender, PrintRefundPaymentReceiptEventArgs e);
        public delegate void PrintJobStatusResponseHandler(object sender, PrintJobStatusResponseEventArgs e);
        public delegate void RetrievePrintersResponseHandler(object sender, RetrievePrintersResponseEventArgs e);
        public delegate void CustomActivityResponseHandler(object sender, CustomActivityResponseEventArgs e);
        public delegate void RetrieveDeviceStatusResponseHandler(object sender, RetrieveDeviceStatusResponseEventArgs e);
        public delegate void MessageFromActivityHandler(object sender, MessageFromActivityEventArgs e);
        public delegate void ResetDeviceResponseHandler(object sender, ResetDeviceResponseEventArgs e);
        public delegate void RetrievePaymentResponseHandler(object sender, RetrievePaymentResponseEventArgs e);
        public delegate void PrintJobStatusRequestHandler(object sender, PrintJobStatusRequestEventArgs e);
        public delegate void DisplayReceiptOptionsResponseHandler(object sender, DisplayReceiptOptionsResponseEventArgs e);
        public delegate void CustomerProvidedDataResponseHandler(object sender, CustomerProvidedDataResponseEventArgs e);

        #endregion

        #endregion
    }

    #region ICloverConnectorListener Event Handler Args Objects 

    /// <summary>
    /// Base class for all CloverConnectorListener event messages
    /// </summary>
    public class CloverEventArgs : EventArgs
    {
        /// <summary>
        /// The actual Clover ICloverConnectorLisener callback method that raised this event. Especially usful when using the central Message Handler pattern.
        /// </summary>
        public CloverMessage cloverMessage { get; set; }

        /// <summary>
        /// The ICloverConnector object associated with this ICloverConnectorListener event message (ie the device SDK object that sent this message)
        /// </summary>
        public ICloverConnector cloverConnector { get; set; }

        /// <summary>
        /// Flag to allow user code to specify whether it handled this message already when consuming both specific API and the catch-all Message events.
        /// This value is always ignored by the SDK
        /// </summary>
        public bool Handled { get; set; }
    }

    public class DeviceActivityStartEventArgs : CloverEventArgs
    {
        public CloverDeviceEvent deviceEvent;

        public override string ToString()
        {
            return $"{cloverMessage}\n{deviceEvent.Code}\n{deviceEvent.EventState}\n{deviceEvent.InputOptions}\n{deviceEvent.Message}\n{deviceEvent.Options}";
        }
    }

    public class DeviceActivityEndEventArgs : CloverEventArgs
    {
        public CloverDeviceEvent deviceEvent;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class DeviceErrorEventArgs : CloverEventArgs
    {
        public CloverDeviceErrorEvent deviceErrorEvent;

        public override string ToString()
        {
            return $"{cloverMessage}\n{deviceErrorEvent.Message}\n{deviceErrorEvent.Cause} {deviceErrorEvent.Code} {deviceErrorEvent.ErrorType}";
        }
    }

    public class PreAuthResponseEventArgs : CloverEventArgs
    {
        public PreAuthResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.Success} {response.Result} {response.Reason}\n{response.Message}";
        }
    }

    public class AuthResponseEventArgs : CloverEventArgs
    {
        public AuthResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.Success} {response.Result} {response.Reason}\n{response.Message}";
        }
    }

    public class TipAdjustAuthResponseEventArgs : CloverEventArgs
    {
        public TipAdjustAuthResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.Success} {response.Result} {response.Reason}\n{response.Message}\n{response.TipAmount}";
        }
    }

    public class CapturePreAuthResponseEventArgs : CloverEventArgs
    {
        public CapturePreAuthResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.Success} {response.Result} {response.Reason}\n{response.Message}";
        }
    }

    public class VerifySignatureRequestEventArgs : CloverEventArgs
    {
        public VerifySignatureRequest request;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class ConfirmPaymentRequestEventArgs : CloverEventArgs
    {
        public ConfirmPaymentRequest request;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class CloseoutResponseEventArgs : CloverEventArgs
    {
        public CloseoutResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.Success} {response.Result} {response.Reason}\n{response.Message}";
        }
    }

    public class SaleResponseEventArgs : CloverEventArgs
    {
        public SaleResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.Success} {response.Result} {response.Reason}\n{response.Message}";
        }
    }

    public class ManualRefundResponseEventArgs : CloverEventArgs
    {
        public ManualRefundResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.Success} {response.Result} {response.Reason}\n{response.Message}";
        }
    }

    public class RefundPaymentResponseEventArgs : CloverEventArgs
    {
        public RefundPaymentResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.Success} {response.Result} {response.Reason}\n{response.Message}";
        }
    }

    public class TipAddedEventArgs : CloverEventArgs
    {
        public TipAddedMessage message;

        public override string ToString()
        {
            return $"{cloverMessage}\n{message.tipAmount}";
        }
    }

    public class VoidPaymentResponseEventArgs : CloverEventArgs
    {
        public VoidPaymentResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.Success} {response.Result} {response.Reason}\n{response.Message}";
        }
    }

    public class VoidPaymentRefundResponseEventArgs : CloverEventArgs
    {
        public VoidPaymentRefundResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.Success} {response.Result} {response.Reason}\n{response.Message}";
        }
    }

    public class DeviceConnectedEventArgs : CloverEventArgs
    {
        // No data parameters in ICloverConnectorListener OnDeviceConnected

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class DeviceReadyEventArgs : CloverEventArgs
    {
        public MerchantInfo merchantInfo;

        public override string ToString()
        {
            return $"{cloverMessage}\n{merchantInfo.merchantID}\n{merchantInfo.merchantMId}\n{merchantInfo.merchantName}\n{merchantInfo.Device.Serial}";
        }
    }

    public class DeviceDisconnectedEventArgs : CloverEventArgs
    {
        // No data parameters in ICloverConnectorListener OnDeviceDisconnected

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class VaultCardResponseEventArgs : CloverEventArgs
    {
        public VaultCardResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.Success} {response.Result} {response.Reason}\n{response.Message}";
        }
    }

    public class RetrievePendingPaymentsResponseEventArgs : CloverEventArgs
    {
        public RetrievePendingPaymentsResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.Success} {response.Result} {response.Reason}\n{response.Message}";
        }
    }

    public class ReadCardDataResponseEventArgs : CloverEventArgs
    {
        public ReadCardDataResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.Success} {response.Result} {response.Reason}\n{response.Message}";
        }
    }

    public class PrintManualRefundReceiptEventArgs : CloverEventArgs
    {
        public PrintManualRefundReceiptMessage message;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class PrintManualRefundDeclineReceiptEventArgs : CloverEventArgs
    {
        public PrintManualRefundDeclineReceiptMessage message;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class PrintPaymentReceiptEventArgs : CloverEventArgs
    {
        public PrintPaymentReceiptMessage message;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class PrintPaymentDeclineReceiptEventArgs : CloverEventArgs
    {
        public PrintPaymentDeclineReceiptMessage message;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class PrintPaymentMerchantCopyReceiptEventArgs : CloverEventArgs
    {
        public PrintPaymentMerchantCopyReceiptMessage message;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class PrintRefundPaymentReceiptEventArgs : CloverEventArgs
    {
        public PrintRefundPaymentReceiptMessage message;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class PrintJobStatusResponseEventArgs : CloverEventArgs
    {
        public PrintJobStatusResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class RetrievePrintersResponseEventArgs : CloverEventArgs
    {
        public RetrievePrintersResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class CustomActivityResponseEventArgs : CloverEventArgs
    {
        public CustomActivityResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.Action}\n{response.Message}";
        }
    }

    public class RetrieveDeviceStatusResponseEventArgs : CloverEventArgs
    {
        public RetrieveDeviceStatusResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class MessageFromActivityEventArgs : CloverEventArgs
    {
        public MessageFromActivity response;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class ResetDeviceResponseEventArgs : CloverEventArgs
    {
        public ResetDeviceResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class RetrievePaymentResponseEventArgs : CloverEventArgs
    {
        public RetrievePaymentResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class PrintJobStatusRequestEventArgs : CloverEventArgs
    {
        public PrintJobStatusRequest request;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class DisplayReceiptOptionsResponseEventArgs : CloverEventArgs
    {
        public DisplayReceiptOptionsResponse response;

        public override string ToString()
        {
            return $"{cloverMessage}";
        }
    }

    public class CustomerProvidedDataResponseEventArgs : CloverEventArgs
    {
        public CustomerProvidedDataEvent response;

        public override string ToString()
        {
            return $"{cloverMessage}\n{response.config.type}";
        }
    }
    #endregion

    #region ICloverConnectorListener Event Hander Enum
    /// <summary>
    /// All the possible Clover Connector Listener message names as a reference enum
    /// </summary>
    public enum CloverMessage
    {
        DeviceActivityStart,
        DeviceActivityEnd,
        DeviceError,
        PreAuthResponse,
        AuthResponse,
        TipAdjustAuthResponse,
        CapturePreAuthResponse,
        VerifySignatureRequest,
        ConfirmPaymentRequest,
        CloseoutResponse,
        SaleResponse,
        ManualRefundResponse,
        RefundPaymentResponse,
        TipAdded,
        VoidPaymentResponse,
        VoidPaymentRefundResponse,
        DeviceConnected,
        DeviceReady,
        DeviceDisconnected,
        VaultCardResponse,
        RetrievePendingPaymentsResponse,
        ReadCardDataResponse,
        PrintManualRefundReceipt,
        PrintManualRefundDeclineReceipt,
        PrintPaymentReceipt,
        PrintPaymentDeclineReceipt,
        PrintPaymentMerchantCopyReceipt,
        PrintRefundPaymentReceipt,
        PrintJobStatusResponse,
        RetrievePrintersResponse,
        CustomActivityResponse,
        RetrieveDeviceStatusResponse,
        MessageFromActivity,
        ResetDeviceResponse,
        RetrievePaymentResponse,
        PrintJobStatusRequest,
        DisplayReceiptOptionsResponse,
        CustomerProvidedData
    }
    #endregion
}
