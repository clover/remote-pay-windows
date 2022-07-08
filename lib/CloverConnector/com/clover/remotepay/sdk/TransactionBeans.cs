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

using System;
using System.Collections.Generic;
using System.Drawing;
using com.clover.remotepay.transport;
using com.clover.sdk.v3;
using com.clover.sdk.v3.merchant;
using com.clover.sdk.v3.order;
using com.clover.sdk.v3.payments;
using com.clover.sdk.v3.printer;

namespace com.clover.remotepay.sdk
{
    /// <summary>
    ///
    /// </summary>
    public abstract class BaseRequest
    {
        protected BaseRequest()
        {
        }

        protected string RequestId { get; set; }
    }

    public enum ResponseCode
    {
        SUCCESS, FAIL, UNSUPPORTED, CANCEL, ERROR
    }

    public static class ResponseCodeExtension
    {
        public static ResponseCode ToResponseCode(this ResultStatus status)
        {
            switch (status)
            {
                case ResultStatus.FAIL: return ResponseCode.FAIL;
                case ResultStatus.SUCCESS: return ResponseCode.SUCCESS;
                case ResultStatus.CANCEL: return ResponseCode.CANCEL;
            }
            return ResponseCode.FAIL;
        }

        public static ResultStatus ToResultCode(this  ResponseCode code)
        {
            switch (code)
            {
                case ResponseCode.FAIL:
                case ResponseCode.UNSUPPORTED:
                case ResponseCode.ERROR: return ResultStatus.FAIL;
                case ResponseCode.SUCCESS: return ResultStatus.SUCCESS;
                case ResponseCode.CANCEL:  return ResultStatus.CANCEL;
            }
            return ResultStatus.FAIL;
        }
    }

    public enum TipMode
    {
        TIP_PROVIDED, ON_SCREEN_BEFORE_PAYMENT, NO_TIP
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class BaseResponse
    {
        protected BaseResponse()
        {

        }
        /// <summary>
        /// If true then the requested operation succeeded
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// The result of the requested operation.
        /// </summary>
        public ResponseCode Result { get; set; }
        /// <summary>
        /// Optional information about result.
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// Detailed information about result.
        /// </summary>
        public string Message { get; set; }

    }

    /// <summary>
    /// Base Sale/Auth/Refund/PreAuth Transaction Request
    /// </summary>
    public abstract class BaseTransactionRequest : BaseRequest
    {
        public bool? DisablePrinting { get; set; }
        public bool? CardNotPresent { get; set; }
        public bool? DisableRestartTransactionOnFail { get; set; }
        public long Amount { get; set; }
        public int? CardEntryMethods { get; set; }
        public VaultedCard VaultedCard { get; set; }
        public string ExternalId { get; set; }

        public PayIntent.TransactionType Type { get; set; }
        public bool? DisableDuplicateChecking { get; set; }
        public bool? DisableReceiptSelection { get; set; }
        public bool? AutoAcceptPaymentConfirmations { get; set; }

        public Dictionary<string, string> Extras { get; } = new Dictionary<string, string>();
        public Dictionary<string, string> RegionalExtras { get; } = new Dictionary<string, string>();

        /// <summary>
        /// The external reference id if associated with the payment
        /// </summary>
        public string ExternalReferenceId { get; set; }

        /// <summary>
        /// Present only the QR Code for Payment
        /// </summary>
        public bool? PresentQrcOnly { get; set; }
    }

    /// <summary>
    /// Extended Sale/Auth Transaction Request
    /// </summary>
    public abstract class TransactionRequest : BaseTransactionRequest
    {
        public long? SignatureThreshold { get; set; }
        public DataEntryLocation? SignatureEntryLocation { get; set; }
        public bool? AutoAcceptSignature { get; set; }
        public List<TipSuggestion> TipSuggestions { get; set; } = new List<TipSuggestion>();
    }

    /// <summary>
    ///
    /// </summary>
    public class TransactionStartResponse : BaseResponse
    {
        public TransactionStartResponse()
        {
            this.ExternalId = null;
        }
        public string ExternalId { get; set; }
    }

    /// <summary>
    /// This should be used for a print call.
    /// </summary>
    public class PrintRequest : BaseRequest
    {
        public List<Bitmap> images { get; set; } = new List<Bitmap>();
        public List<string> imageURLs { get; set; } = new List<string>();
        public List<string> text { get; set; } = new List<string>();
        public string printRequestId { get; set; }
        public string printDeviceId { get; set; }

        public PrintRequest() { }

        /// Create a PrintRequest to print a given Image
        ///
        /// - Parameters:
        ///   - image: Image to print
        ///   - printRequestId: Optional identifier to give to the print job, so it can later be queried
        ///   - printDeviceId: Optional identifier to speciy which printer to use
        public PrintRequest(Bitmap image, string printRequestId, string printDeviceId)
        {
            images.Add(image);
            this.printRequestId = printRequestId;
            this.printDeviceId = printDeviceId;

        }

        /// Create a PrintRequest to print an image at a given URL
        ///
        /// - Parameters:
        ///   - imageURL: URL to the image to print
        ///   - printRequestId: Optional identifier to give to the print job, so it can later be queried
        ///   - printDeviceId: Optional identifier to speciy which printer to use
        public PrintRequest(string imageURL, string printRequestId, string printDeviceId)
        {
            imageURLs.Add(imageURL);
            this.printRequestId = printRequestId;
            this.printDeviceId = printDeviceId;
        }

        /// Create a PrintRequest to print an array of strings to print
        ///
        /// - Parameters:
        ///   - text: Array of strings to be printed
        ///   - printRequestId: Optional identifier to give to the print job, so it can later be queried
        ///   - printDeviceId: Optional identifier to speciy which printer to use
        public PrintRequest(List<string> text, string printRequestId, string printDeviceId)
        {
            if (text.Count < 1)
            {
                return;
            }

            this.text = text;
            this.printRequestId = printRequestId;
            this.printDeviceId = printDeviceId;

        }


    }


    public class OpenCashDrawerRequest : BaseRequest
    {
        public string reason { get; set; }
        public string printerId { get; set; }

        /// Create an object used to inform the Clover Connector's `openCashDrawer()` function of required/additional information when requesting the cash drawer be opened
        ///
        /// - Parameters:
        ///   - reason: string describing the reason to open the drawer
        public OpenCashDrawerRequest(string reason)
        {
            this.reason = reason;
        }
    }

    /// <summary>
    /// This request should be used for a Sale call.
    /// </summary>
    public class SaleRequest : TransactionRequest
    {

        public SaleRequest()
        {
            this.Type = PayIntent.TransactionType.PAYMENT;
            DisableCashback = false;
            DisablePrinting = false;
            DisableRestartTransactionOnFail = false;
        }
        /// <summary>
        /// If true then the cashback feature will not appear during the transaction
        /// </summary>
        public bool? DisableCashback { get; set; }
        /// <summary>
        /// Amount paid in tips
        /// </summary>
        public long? TaxAmount { get; set; }
        /// <summary>
        /// Amount against which a tip should be applied
        /// </summary>
        public long? TippableAmount { get; set; }
        /// <summary>
        /// If true then offline payments can be accepted
        /// </summary>
        public bool? AllowOfflinePayment { get; set; }
        /// <summary>
        /// If true then offline payments will be approved without a prompt.  Currently must be true.
        /// </summary>
        public bool? ApproveOfflinePaymentWithoutPrompt { get; set; }
        /// <summary>
        /// Gets or sets the tip amount.
        /// </summary>
        public long? TipAmount { get; set; }
        /// <summary>
        /// Gets or sets the tip mode, which controls when the tip is either 
        /// designated by the customer, provided by the system as part of the request,
        /// or ommitted completely from the transaction
        /// </summary>
        public TipMode? TipMode { get; set; }
        /// <summary>
        /// If true then payments will be placed directly in the offline queue for
        /// processing when a valid connection exists to the gateway.  This means that
        /// payments can be taken offline even with a healthy gateway connection, for purposes
        /// of keeping customer interaction with the payment device to the bare minimum.
        /// The merchant assumes all risk of potentially declined transactions when this 
        /// feature is enabled.
        /// </summary>
        public bool? ForceOfflinePayment { get; set; }
    }

    /// <summary>
    /// Object passed in to an OnSaleResponse
    /// </summary>
    public class SaleResponse : PaymentResponse
    {
    }

    /// <summary>
    /// Object passed in to VerifySignatureRequest. This must
    /// also be used to either accept or reject a signature as requested
    /// from the clover device.
    /// </summary>
    public class VerifySignatureRequest
    {
        public virtual void Accept() { }
        public virtual void Reject() { }
        public Signature2 Signature { get; set; }
        public Payment Payment { get; set; }
    }

    /// <summary>
    /// This request should be used for Auth only, but is backward
    /// compatible for older implementations.
    /// If you are currently using an AuthRequest with IsPreAuth = true,
    /// please change your code to use PreAuthRequest/PreAuthResponse
    /// for all PreAuth transactions.
    /// </summary>
    public class AuthRequest : TransactionRequest
    {
        public AuthRequest()
        {
            this.Type = PayIntent.TransactionType.PAYMENT;
        }
        public bool? DisableCashback { get; set; }
        /// <summary>
        /// Amount paid in tips
        /// </summary>
        public long? TaxAmount { get; set; }
        /// <summary>
        /// Amount against which a tip should be applied
        /// </summary>
        public long? TippableAmount { get; set; }
        /// <summary>
        /// If true then offline payments can be accepted
        /// </summary>
        public bool? AllowOfflinePayment { get; set; }
        /// <summary>
        /// If true then offline payments will be approved without a prompt.  Currently must be true.
        /// </summary>
        public bool? ApproveOfflinePaymentWithoutPrompt { get; set; }
        /// <summary>
        /// If true then payments will be placed directly in the offline queue for
        /// processing when a valid connection exists to the gateway.  This means that
        /// payments can be taken offline even with a healthy gateway connection, for purposes
        /// of keeping customer interaction with the payment device to the bare minimum.
        /// The merchant assumes all risk of potentially declined transactions when this 
        /// feature is enabled.
        /// </summary>
        public bool? ForceOfflinePayment { get; set; }
    }

    /// <summary>
    /// Object passed in to OnAuthResponse
    /// </summary>
    public class AuthResponse : PaymentResponse
    {

    }

    /// <summary>
    /// This request should be used for PreAuth transactions.
    /// If you are currently using an AuthRequest with IsPreAuth = true,
    /// please change your code to use PreAuthRequest/PreAuthResponse
    /// for all PreAuth transactions.
    /// </summary>
    public class PreAuthRequest : BaseTransactionRequest
    {
        public PreAuthRequest()
        {
            this.Type = PayIntent.TransactionType.AUTH;
        }
    }

    /// <summary>
    /// Object passed in to OnPreAuthResponse
    /// </summary>
    public class PreAuthResponse : PaymentResponse
    {
    }
    public class PaymentResponse : BaseResponse
    {

        /**
        * Initialize the values for this.
        * @private
        */
        public PaymentResponse()
        {
        }
        /// <summary>
        /// The payment from the sale
        /// </summary>
        public Payment Payment { get; set; }
        public bool IsSale
        {
            get
            {
                if (Payment != null && Payment.cardTransaction != null &&
                    Payment.cardTransaction.type.Equals(CardTransactionType.AUTH) &&
                    Payment.result.Equals(clover.sdk.v3.payments.Result.SUCCESS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsPreAuth
        {
            get
            {
                if (Payment != null && Payment.cardTransaction != null &&
                    Payment.cardTransaction.type.Equals(CardTransactionType.PREAUTH) &&
                    Payment.result.Equals(clover.sdk.v3.payments.Result.AUTH))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsAuth
        {
            get
            {
                if (Payment != null && Payment.cardTransaction != null &&
                    Payment.cardTransaction.type.Equals(CardTransactionType.PREAUTH) &&
                    Payment.result.Equals(clover.sdk.v3.payments.Result.SUCCESS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Signature2 Signature { get; set; } // optional
    }

    /// <summary>
    /// This request should be used for capturing payments obtained from
    /// a PreAuth response
    /// </summary>
    public class CapturePreAuthRequest : BaseRequest
    {
        public string PaymentID { get; set; }
        public long Amount { get; set; }
        public long TipAmount { get; set; }
    }

    /// <summary>
    /// Increments a previously made pre auth
    /// </summary>
    public class IncrementPreAuthRequest : BaseRequest
    {
        /// <summary>
        /// The amount by which to increment the pre-auth.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// The preauth to be incremented. This id should be pulled from the Payment.paymentId field in the PreAuthResponse.
        /// </summary>
        public string PaymentID { get; set; }
    }

    /// <summary>
    /// The result of an attempt to increment a pre auth
    /// </summary>
    public class IncrementPreAuthResponse : BaseResponse
    {
        /// <summary>
        /// The resulting Authorization
        /// </summary>
        public Authorization Authorization { get; set; }
    }

    /// <summary>
    /// Object passed in to an OnVaultCardResponse
    /// </summary>
    public class VaultCardResponse : BaseResponse
    {
        public VaultedCard Card { get; set; }
    }

    /// <summary>
    /// This request should be passed to initiate retrieval of card data
    /// </summary>
    public class ReadCardDataRequest : BaseRequest
    {
        public int? CardEntryMethods { get; set; }
        public bool? IsForceSwipePinEntry { get; set; }
    }

    /// <summary>
    /// Retrieve Card Data
    /// </summary>
    public class ReadCardDataResponse : BaseResponse
    {
        public CardData CardData { get; set; }
    }

    /// <summary>
    /// Object passed in to OnCapturePreAuthResponse
    /// </summary>
    public class CapturePreAuthResponse : BaseResponse
    {
        public string PaymentId { get; set; }
        public long Amount { get; set; }
        public long? TipAmount { get; set; }
    }

    /// <summary>
    /// This request should be passed in to make a closeout request
    /// </summary>
    public class CloseoutRequest : BaseRequest
    {
        public bool AllowOpenTabs { get; set; }
        public string BatchId { get; set; }
    }

    /// <summary>
    /// Object passed in to OnCloseoutResponse
    /// </summary>
    public class CloseoutResponse : BaseResponse
    {
        public Batch Batch { get; set; }
    }

    /// <summary>
    /// This request should be used to make a request to adjust the
    /// tip amount on a payment obtained from an Auth request or payment
    /// after a CapturePreAuth request
    /// </summary>
    public class TipAdjustAuthRequest : BaseRequest
    {
        public string PaymentID { get; set; }
        public string OrderID { get; set; }
        public long? TipAmount { get; set; }
    }

    /// <summary>
    /// Object passed in to OnTipAdjustAuthResponse
    /// </summary>
    public class TipAdjustAuthResponse : BaseResponse
    {
        public string PaymentId { get; set; }
        public long? TipAmount { get; set; }
    }


    /// <summary>
    /// Object passed in to ConfirmPaymentRequest. This must
    /// also be used to either accept or reject a payment as requested
    /// from the clover device.
    /// </summary>
    public class ConfirmPaymentRequest
    {
        public Payment Payment { get; set; }
        public List<Challenge> Challenges { get; set; }
    }

    /// <summary>
    /// Object passed in to request the voiding of a payment
    /// </summary>
    public class VoidPaymentRequest : BaseRequest
    {
        public string PaymentId { get; set; }
        /// <summary>
        /// Reason for void, must be one of the recognized categories, see VoidPaymentRequest consts for common options
        /// use VoidPaymentRequest.USER_CANCEL as default
        /// </summary>
        public string VoidReason { get; set; }

        public string EmployeeId { get; set; }
        public string OrderId { get; set; }
        /// <summary>
        /// If true, then do not print using the clover printer.  Return print information.
        /// </summary>
        public bool? DisablePrinting { get; set; }
        /// <summary>
        /// Do not show the receipt options screen
        /// </summary>
        public bool? DisableReceiptSelection { get; set; }

        public Dictionary<string, string> Extras { get; } = new Dictionary<string, string>();

        // Valid VoidReason values - use USER_CANCEL by default
        public const string USER_CANCEL = "USER_CANCEL";
        public const string NOT_APPROVED = "NOT_APPROVED";
        public const string FAILED = "FAILED";
        public const string REJECT_SIGNATURE = "REJECT_SIGNATURE";
        public const string REJECT_PARTIAL_AUTH = "REJECT_PARTIAL_AUTH";
        public const string REJECT_DUPLICATE = "REJECT_DUPLICATE";
        public const string REJECT_OFFLINE = "REJECT_OFFLINE";
        public const string AUTH_CLOSED_NEW_CARD = "AUTH_CLOSED_NEW_CARD";
        public const string DEVELOPER_PAY_PARTIAL_AUTH = "DEVELOPER_PAY_PARTIAL_AUTH";
        public const string TRANSPORT_ERROR = "TRANSPORT_ERROR";
    }

    /// <summary>
    /// Object passed in to OnVoidPaymentResopnse
    /// </summary>
    public class VoidPaymentResponse : BaseResponse
    {
        public string PaymentId { get; set; }
        public Payment Payment { get; set; }
    }

    /// <summary>
    /// Voiding a payment refund (card present flows like canada region)
    /// </summary>
    public class VoidPaymentRefundRequest : BaseRequest
    {
        public string RefundId { get; set; }
        public string EmployeeId { get; set; }
        public string OrderId { get; set; }
        public bool DisablePrinting { get; set; }
        public bool DisableReceiptSelection { get; set; }

        public Dictionary<string, string> Extras { get; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// Object passed in to OnVoidPaymentRefundResponse
    /// </summary>
    public class VoidPaymentRefundResponse : BaseResponse
    {
        public string RefundId { get; set; }
    }

    /// <summary>
    /// This should be used to request a manual refund via the ManualRefund method
    /// </summary>
    public class ManualRefundRequest : BaseTransactionRequest
    {
        public ManualRefundRequest()
        {
            this.Type = PayIntent.TransactionType.CREDIT;
        }
    }

    /// <summary>
    /// The object passed in to OnManualRefundResponse
    /// </summary>
    public class ManualRefundResponse : BaseResponse
    {
        public Credit Credit { get; set; }
    }

    public class RetrievePrintersResponse : BaseResponse
    {
        public List<Printer> printers { get; set; } = new List<Printer>();

        public RetrievePrintersResponse()
        {
        }

        public RetrievePrintersResponse(Printer printer)
        {
            printers.Add(printer);
        }

        public RetrievePrintersResponse(List<Printer> printers)
        {
            this.printers = printers;
        }
    }

    public class PrintJobStatusRequest : BaseRequest
    {
        public string printRequestId { get; set; }

        public PrintJobStatusRequest()
        {
        }

        public PrintJobStatusRequest(string printRequestId)
        {
            this.printRequestId = printRequestId;
        }
    }

    public class PrintJobStatusResponse : BaseResponse
    {
        public string printRequestId { get; set; }
        public PrintJobStatus status { get; set; }

        public PrintJobStatusResponse(string printRequestId, string status)
        {
            this.printRequestId = printRequestId;
            try
            {
                this.status = (PrintJobStatus)Enum.Parse(typeof(PrintJobStatus), status);
            }
            catch (ArgumentException)
            {
                // Console.WriteLine("{0} is not a member of the PrintJobStatus enumeration.", status);
                this.status = PrintJobStatus.UNKNOWN;
            }
        }
    }

    public enum PrintJobStatus
    {
        IN_QUEUE,
        PRINTING,
        DONE,
        ERROR,
        UNKNOWN,
        NOT_FOUND
    }

    /// <summary>
    /// This request should be used to make a payment request
    /// using the RefundPayment method
    /// </summary>
    public class RefundPaymentRequest : BaseRequest
    {
        public bool FullRefund { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public long Amount { get; set; } // optional
        public bool? DisablePrinting { get; set; }
        public bool? DisableReceiptSelection { get; set; }

        public Dictionary<string, string> Extras { get; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// Object passed in to OnRefundPaymentResponse
    /// </summary>
    public class RefundPaymentResponse : BaseResponse
    {
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public Refund Refund { get; set; }
    }

    /// <summary>
    /// Object passed in to OnAcceptPayment
    /// </summary>
    public class AcceptPayment
    {
        public Payment Payment { get; set; }
    }

    /// <summary>
    /// Object passed in to OnRejectPayment
    /// </summary>
    public class RejectPayment
    {
        public Payment Payment { get; set; }
        public Challenge Challenge { get; set; }
    }

    /// <summary>
    /// Pased in to OnTipAdded, when an on-screen tip
    /// is selected
    /// </summary>
    public class TipAdded : BaseResponse
    {

        /**
        * Initialize the values for this.
        * @private
        */
        public TipAdded()
        {
        }
        /// <summary>
        /// Tip amount
        /// </summary>
        public long? TipAmount { get; set; }
    }

    /// <summary>
    /// The request used to show the receipt options screen
    /// </summary>
    public class DisplayPaymentReceiptOptionsRequest : BaseRequest
    {
        public string OrderID { get; set; }
        public string PaymentID { get; set; }
        public bool DisablePrinting { get; set; }
    }

    /// <summary>
    /// The response object for offline payments that
    /// have not been processed by the Clover device
    /// </summary>
    public class RetrievePendingPaymentsResponse : BaseResponse
    {
        /// <summary>
        /// List of payments taken offline and not yet processed
        /// </summary>
        public List<PendingPaymentEntry> PendingPayments { get; set; }
    }

    /// <summary>
    /// Callback to request the POS print a receipt for a ManualRefund
    ///</summary>
    public class PrintManualRefundReceiptMessage : BaseResponse
    {
        public Credit Credit { get; set; }
    }

    /// <summary>
    /// Callback to request the POS print a ManualRefund declined receipt
    /// </summary>
    public class PrintManualRefundDeclineReceiptMessage : BaseResponse
    {
        public Credit Credit { get; set; }
    }

    /// <summary>
    /// Callback to the POS to request a payment receipt be printed
    /// </summary>
    public class PrintPaymentReceiptMessage : BaseResponse
    {
        public Order Order { get; set; }
        public Payment Payment { get; set; }
    }

    /// <summary>
    /// Callback to the POS to request a payment declined receipt
    /// </summary>
    public class PrintPaymentDeclineReceiptMessage : BaseResponse
    {
        public Payment Payment { get; set; }
    }

    /// <summary>
    /// Callback to the POS to request a merchant payment receipt
    /// be printed
    /// </summary>
    public class PrintPaymentMerchantCopyReceiptMessage : BaseResponse
    {
        public Payment Payment { get; set; }
    }

    /// <summary>
    /// Callback to the POS to request a payment refund receipt
    /// be printed
    /// </summary>
    public class PrintRefundPaymentReceiptMessage : BaseResponse
    {
        public Payment Payment { get; set; }
        public Refund Refund { get; set; }
        public Order Order { get; set; }
    }

    /// <summary>
    /// Request for the Clover device to start a Custom Activity
    /// </summary>
    public class CustomActivityRequest : BaseRequest
    {
        /// <summary>
        /// Action string, usually the Activity name
        /// </summary>
        public string Action { get; set; }
        public string Payload { get; set; }
        public bool NonBlocking { get; set; } = false;
    }

    /// <summary>
    /// Response when a custom activity completes on the
    /// Clover device when it is completed in a normal flow
    /// </summary>
    public class CustomActivityResponse : BaseResponse
    {
        /// <summary>
        /// Action string, usually the Activity name
        /// </summary>
        public string Action { get; set; }
        public string Payload { get; set; }
    }

    /// <summary>
    /// base class for messages flowing between POS and CustomActivity
    /// </summary>
    public class ActivityMessage
    {
        /// <summary>
        /// Action string, usually the Activity name
        /// </summary>
        public string Action { get; set; }
        public string Payload { get; set; }
    }

    /// <summary>
    /// class for sending messages to a CustomActivity from POS
    /// </summary>
    public class MessageToActivity
    {
        /// <summary>
        /// Action string, usually the Activity name
        /// </summary>
        public string Action { get; set; }
        public string Payload { get; set; }
    }

    /// <summary>
    /// class for sending message from a CustomActivity to POS
    /// </summary>
    public class MessageFromActivity
    {
        /// <summary>
        /// Action string, usually the Activity name
        /// </summary>
        public string Action { get; set; }
        public string Payload { get; set; }
    }

    /// <summary>
    /// response to RetrieveDeviceStatus call
    /// </summary>
    public class RetrieveDeviceStatusResponse : BaseResponse
    {
        public ExternalDeviceState State { get; set; }
        public ExternalDeviceStateData Data { get; set; }
    }

    /// <summary>
    /// Depending on the current device status,
    /// these fields may or may not be populated
    /// </summary>
    public class ExternalDeviceStateData
    {
        public string ExternalPaymentId { get; set; }
        public string CustomActivityId { get; set; }
    }

    /// <summary>
    /// enum of current states of the Clover device.
    /// IDLE - device can handle new requests. Sale, CustomActivity, etc.
    /// BUSY - device will not response to new requests
    /// WAITING_FOR_POS - device is waiting for a response from the POS
    /// WAITING_FOR_CUSTOMER - device is waiting for the customer to take some action
    /// </summary>
    public enum ExternalDeviceState
    {
        UNKNOWN, // a null enum in JSON will evaluate to this, the first enum
        IDLE,
        BUSY,
        WAITING_FOR_POS,
        WAITING_FOR_CUSTOMER
    }

    public class ResetDeviceResponse : BaseResponse
    {
        public ExternalDeviceState State { get; set; }
    }

    /// <summary>
    /// response to RetrievePayment call
    /// </summary>
    public class RetrievePaymentResponse : BaseResponse
    {
        public string ExternalPaymentId { get; set; }
        public QueryStatus QueryStatus { get; set; }
        public Payment Payment { get; set; }
    }

    public class DisplayReceiptOptionsRequest : BaseRequest
    {
        public string orderId { get; set; }
        public string paymentId { get; set; }
        public string refundId { get; set; }
        public string creditId { get; set; }
        public bool disablePrinting { get; set; }
    }

    public class DisplayReceiptOptionsResponse : BaseResponse
    {
        public ResultStatus status { get; set; }
    }

    public class RegisterForCustomerProvidedDataRequest : BaseRequest
    {
        public DataProviderConfig[] Configurations { get; set; }
    }

    public class CustomerProvidedDataEvent : BaseResponse
    {
        public string eventId { get; set; }
        public DataProviderConfig config { get; set; }
        public string data { get; set; }
    }

    public class InvalidStateTransitionNotification : BaseResponse
    {
        public string RequestedTransition { get; set; }
        public string State { get; set; }
        public string Substate { get; set; }
        public ExternalDeviceStateData Data { get; set; }
    }

    public class SetCustomerInfoRequest : BaseRequest
    {
        public com.clover.sdk.v3.customers.Customer customer { get; set; }
        public string displayString { get; set; }
        public string externalId { get; set; }
        public string externalSystemName { get; set; }
        public Dictionary<string, string> extras { get; set; } = new Dictionary<string, string>();
    }

    public static class CloverConnectorExtensions
    {
        public static com.clover.sdk.v3.payments.TipMode? GetAltTipMode(this com.clover.remotepay.sdk.TipMode? value)
        {
            if (value == null)
            {
                return null;
            }
            switch (value)
            {
                case TipMode.NO_TIP: return clover.sdk.v3.payments.TipMode.NO_TIP;
                case TipMode.ON_SCREEN_BEFORE_PAYMENT: return clover.sdk.v3.payments.TipMode.ON_SCREEN_BEFORE_PAYMENT;
                case TipMode.TIP_PROVIDED: return clover.sdk.v3.payments.TipMode.TIP_PROVIDED;
                default: return (com.clover.sdk.v3.payments.TipMode)Enum.Parse(typeof(TipMode), value.ToString());
            }
        }

        public static com.clover.remotepay.sdk.TipMode? GetAltTipMode(this com.clover.sdk.v3.payments.TipMode? value)
        {
            if (value == null)
            {
                return null;
            }
            switch (value)
            {
                case clover.sdk.v3.payments.TipMode.NO_TIP: return TipMode.NO_TIP;
                case clover.sdk.v3.payments.TipMode.ON_SCREEN_BEFORE_PAYMENT: return TipMode.ON_SCREEN_BEFORE_PAYMENT;
                case clover.sdk.v3.payments.TipMode.TIP_PROVIDED: return TipMode.TIP_PROVIDED;
                default: return (com.clover.remotepay.sdk.TipMode)Enum.Parse(typeof(com.clover.remotepay.sdk.TipMode), value.ToString());
            }
        }
    }
}