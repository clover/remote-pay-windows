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
using com.clover.remote.order;
using com.clover.remote.order.operation;
using com.clover.remotepay.data;
using com.clover.sdk.v3;
using com.clover.sdk.v3.order;
using com.clover.sdk.v3.payments;
using com.clover.sdk.v3.printer;

namespace com.clover.remotepay.transport
{
    public class Message
    {
        public readonly int version = 1;
        public Methods method { get; set; }

        public Message(Methods method)
        {
            this.method = method;
        }

        public Message(Methods method, int version)
        {
            this.method = method;
            this.version = version;
        }
    }

    public class DiscoveryRequestMessage : Message
    {
        public readonly bool supportsOrderModification;

        public DiscoveryRequestMessage(bool supportsOrderModification = false) : base(Methods.DISCOVERY_REQUEST)
        {
            this.supportsOrderModification = supportsOrderModification;
        }
    }

    public class DiscoveryResponseMessage : Message
    {
        public readonly string merchantId;
        public readonly string merchantName;
        public readonly string merchantMId;
        public readonly string name;
        public readonly string serial;
        public readonly string model;
        public readonly bool ready;
        public readonly bool supportsAcknowledgement;
        public readonly bool supportsTipAdjust;
        public readonly bool supportsManualRefund;
        public readonly bool supportsMultiPayToken;
        public readonly bool supportsRemoteConfirmation;
        public readonly bool supportsNakedCredit;
        public readonly bool supportsVoidPaymentResponse;
        public readonly bool supportsPreAuth;
        public readonly bool supportsAuth;
        public readonly bool supportsVaultCard;

        public DiscoveryResponseMessage(string merchantId,
                                        string merchantName,
                                        string merchantMId,
                                        string name,
                                        string serial,
                                        string model,
                                        bool ready,
                                        bool? supportsAcknowledgement,
                                        bool? supportsTipAdjust = null,
                                        bool? supportsManualRefund = null,
                                        bool? supportsMultiPayToken = null,
                                        bool? supportsRemoteConfirmation = null,
                                        bool? supportsNakedCredit = null,
                                        bool? supportsVoidPaymentResponse = null,
                                        bool? supportsPreAuth = null,
                                        bool? supportsAuth = null,
                                        bool? supportsVaultCard = null
            )
            : base(Methods.DISCOVERY_RESPONSE)
        {
            this.merchantId = merchantId;
            this.merchantName = merchantName ?? "";
            this.merchantMId = merchantMId ?? "";
            this.name = name;
            this.serial = serial;
            this.model = model;
            this.ready = ready;

            this.supportsAcknowledgement = supportsAcknowledgement ?? false;
            this.supportsTipAdjust = supportsTipAdjust ?? true;
            this.supportsManualRefund = supportsManualRefund ?? true;
            this.supportsMultiPayToken = supportsMultiPayToken ?? true;

            this.supportsRemoteConfirmation = supportsRemoteConfirmation ?? true;
            this.supportsNakedCredit = supportsNakedCredit ?? true;
            this.supportsVoidPaymentResponse = supportsVoidPaymentResponse ?? true;
            this.supportsPreAuth = supportsPreAuth ?? true;
            this.supportsAuth = supportsAuth ?? true;
            this.supportsVaultCard = supportsVaultCard ?? true;

        }
    }

    public class ShowPaymentReceiptOptionsMessage : Message
    {
        public string orderId { get; set; }
        public string paymentId { get; set; }
        public bool disableCloverPrinting { get; set; }

        public ShowPaymentReceiptOptionsMessage(string orderId, string paymentId, bool disableCloverPrinting)
            : base(Methods.SHOW_PAYMENT_RECEIPT_OPTIONS, 2) //automatically sets the version to the newer, preferable version
        {
            this.orderId = orderId;
            this.paymentId = paymentId;
            this.disableCloverPrinting = disableCloverPrinting;
        }

    }

    public class RefundReceiptMessage : Message
    {
        public string orderId { get; set; }
        public string refundId { get; set; }

        public RefundReceiptMessage(string orderId, string refundId)
            : base(Methods.SHOW_REFUND_RECEIPT_OPTIONS)
        {
            this.orderId = orderId;
            this.refundId = refundId;
        }
    }

    public class CreditReceiptMessage : Message
    {
        public string orderId { get; set; }
        public string creditId { get; set; }

        public CreditReceiptMessage(string orderId, string creditId)
            : base(Methods.SHOW_CREDIT_RECEIPT_OPTIONS)
        {
            this.orderId = orderId;
            this.creditId = creditId;
        }
    }

    public class TextPrintMessage : Message
    {
        public List<string> textLines { get; set; }
        public string externalPrintJobId { get; set; }

        public TextPrintMessage()
            : base(Methods.PRINT_TEXT)
        {
            textLines = new List<string>();
        }
    }

    public class ImagePrintMessage : Message
    {
        public string png { get; set; }
        public string urlString { get; set; }
        public string externalPrintJobId { get; set; }
        public Printer printer { get; set; }

        public ImagePrintMessage()
            : base(Methods.PRINT_IMAGE)
        {
        }
    }

    public class FinishCancelMessage : Message
    {
        public TxType requestInfo { get; set; }

        public FinishCancelMessage()
            : base(Methods.FINISH_CANCEL)
        {
        }
    }

    public class BreakMessage : Message
    {
        public BreakMessage()
            : base(Methods.BREAK)
        {
        }
    }

    public class FinishOkMessage : Message
    {
        public Payment payment { get; set; }
        public Credit credit { get; set; }
        public Refund refund { get; set; }
        public Signature2 signature { get; set; }
        public TxType requestInfo { get; set; }

        public FinishOkMessage()
            : base(Methods.FINISH_OK)
        {
        }
    }

    public enum TxType
    {
        NONE,
        SALE,
        AUTH,
        PREAUTH,
        CREDIT,
        REFUND,
        MANUAL_REFUND
    }

    public class TxStartRequestMessage : Message
    {
        public readonly PayIntent payIntent;
        public readonly Order order;
        public TxType requestInfo { get; set; }

        public TxStartRequestMessage(PayIntent payIntent, Order order, TxType type)
            : base(Methods.TX_START, 2)
        {
            this.payIntent = payIntent;
            this.order = order;
            requestInfo = type;
        }
    }

    public class TxStartResponseMessage : Message
    {
        public Order order { get; set; }
        public TxStartResponseResult result { get; set; }
        public string externalId { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string reason { get; set; }
        public string requestInfo { get; set; }

        public TxStartResponseMessage() : base(Methods.TX_START_RESPONSE)
        {
        }
    }

    public class VerifySignatureMessage : Message
    {
        public Payment payment { get; set; }
        public Signature2 signature { get; set; }

        public VerifySignatureMessage()
            : base(Methods.VERIFY_SIGNATURE)
        {
        }
    }

    public class TxStateMessage : Message
    {
        public readonly TxState txState;

        public TxStateMessage(TxState txState)
            : base(Methods.TX_STATE)
        {
            this.txState = txState;
        }
    }

    public class PartialAuthMessage : Message
    {
        public long partialAuthAmount { get; set; }

        public PartialAuthMessage()
            : base(Methods.PARTIAL_AUTH)
        {
        }

        public PartialAuthMessage(long partialAuth)
            : base(Methods.PARTIAL_AUTH)
        {
            this.partialAuthAmount = partialAuth;
        }
    }

    public class CashbackSelectedMessage : Message
    {
        public long cashbackAmount { get; set; }

        public CashbackSelectedMessage()
            : base(Methods.CASHBACK_SELECTED)
        {
        }

        public CashbackSelectedMessage(long cashback)
            : base(Methods.CASHBACK_SELECTED)
        {
            this.cashbackAmount = cashback;
        }
    }

    public class TipAddedMessage : Message
    {
        public long tipAmount { get; set; }

        public TipAddedMessage()
            : base(Methods.TIP_ADDED)
        {
        }

        public TipAddedMessage(long tip)
            : base(Methods.TIP_ADDED)
        {
            this.tipAmount = tip;
        }
    }

    public class RefundRequestMessage : Message
    {
        public string orderId { get; set; }
        public string paymentId { get; set; }
        public long? amount { get; set; }
        public bool? fullRefund { get; set; }
        public bool? disableCloverPrinting { get; set; }
        public bool? disableReceiptSelection { get; set; }
        public Dictionary<string, string> passThroughValues { get; set; }

        public RefundRequestMessage() : base(Methods.REFUND_REQUEST, 3)
        {
        }

        public RefundRequestMessage(string oid, string pid, long? amt, bool? fullRefund, bool? disableCloverPrinting, bool? disableReceiptSelection, Dictionary<string, string> passThroughValues)
            : base(Methods.REFUND_REQUEST, 2)
        {
            this.orderId = oid;
            this.paymentId = pid;
            this.amount = amt;
            this.fullRefund = fullRefund;
            this.disableCloverPrinting = disableCloverPrinting;
            this.disableReceiptSelection = disableReceiptSelection;
            this.passThroughValues = passThroughValues;
        }
    }

    public class TipAdjustAuthMessage : Message
    {
        public string orderId { get; set; }
        public string paymentId { get; set; }
        public long? tipAmount { get; set; }
        public Dictionary<string, string> passThroughValues { get; set; }

        public TipAdjustAuthMessage(string orderId, string paymentId, long? tipAmount, Dictionary<string, string> passThroughValues)
            : base(Methods.TIP_ADJUST)
        {
            this.orderId = orderId;
            this.paymentId = paymentId;
            this.tipAmount = tipAmount;
            this.passThroughValues = passThroughValues;
        }
    }

    public class TipAdjustResponseMessage : Message
    {
        public string orderId { get; set; }
        public string paymentId { get; set; }
        public bool success { get; set; }
        public long amount { get; set; }

        public TipAdjustResponseMessage(string orderId, string paymentId, long amount, bool success)
            : base(Methods.TIP_ADJUST_RESPONSE)
        {
            this.orderId = orderId;
            this.paymentId = paymentId;
            this.amount = amount;
            this.success = success;
        }
    }

    public class CapturePreAuthMessage : Message
    {
        public string paymentId { get; set; }
        public long amount { get; set; }
        public long tipAmount { get; set; }

        public CapturePreAuthMessage(string paymentID, long amount, long tipAmount)
            : base(Methods.CAPTURE_PREAUTH)
        {
            this.paymentId = paymentID;
            this.amount = amount;
            this.tipAmount = tipAmount;
        }
    }

    public class CapturePreAuthResponseMessage : CapturePreAuthMessage
    {
        public ResultStatus status { get; set; }
        public string reason { get; set; }

        public CapturePreAuthResponseMessage(ResultStatus status, string reason, string paymentID, long amount, long tipAmount)
            : base(paymentID, amount, tipAmount)
        {
            this.status = status;
            this.reason = reason;
        }
    }

    /// <summary>
    /// RefundResponseMessage is used when there is a refund for a payment. It is not used when doing a manual refund
    /// </summary>
    public class RefundResponseMessage : Message
    {
        /// <summary>
        /// Unique identifier for a order
        /// </summary>
        public string orderId { get; set; }

        /// <summary>
        /// Unique identifier for a payment
        /// </summary>
        public string paymentId { get; set; }

        /// <summary>
        /// The refund
        /// </summary>
        public Refund refund { get; set; }

        /// <summary>
        /// Detail code if an error is encountered
        /// </summary>
        public ResponseReasonCode reason { get; set; }

        /// <summary>
        /// Detail message
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// Transaction state (success|fail)
        /// </summary>
        public TxState code { get; set; }

        public RefundResponseMessage()
            : base(Methods.REFUND_RESPONSE)
        {
        }

        public RefundResponseMessage(string oid, string pid, Refund refund, TxState state, string msg, ResponseReasonCode reason)
            : base(Methods.REFUND_RESPONSE)
        {
            this.orderId = oid;
            this.paymentId = pid;
            this.refund = refund;
            this.code = state;
            this.message = msg;
            this.reason = reason;
        }
    }

    public class UiStateMessage : Message
    {
        public readonly UiState uiState;
        public readonly string uiText;
        public readonly UiDirection uiDirection;
        public readonly InputOption[] inputOptions;

        public UiStateMessage(UiState uiState, string uiText, UiDirection uiDirection, InputOption[] inputOptions)
            : base(Methods.UI_STATE)
        {
            this.uiState = uiState;
            this.uiText = uiText;
            this.uiDirection = uiDirection;
            this.inputOptions = inputOptions;
        }
    }

    /// <summary>
    /// Message used to indicate that a payment should be voided.
    /// </summary>
    public class VoidPaymentMessage : Message
    {
        public VoidReason voidReason { get; set; }
        public Payment payment { get; set; }
        public Dictionary<string, string> passThroughValues { get; set; }

        public VoidPaymentMessage() : base(Methods.VOID_PAYMENT, 2)
        {
        }
    }

    /// <summary>
    /// Message used to indicate that a card present refund should be voided
    /// </summary>
    public class VoidPaymentRefundMessage : Message
    {
        public string orderId { get; set; }
        public string refundId { get; set; }
        public bool disableCloverPrinting { get; set; }
        public bool disableReceiptSelection { get; set; }
        public string employeeId { get; set; }

        public VoidPaymentRefundMessage() : base(Methods.VOID_PAYMENT_REFUND, 2)
        {
        }
    }

    public class TerminalMessage : Message
    {
        public readonly string text;

        public TerminalMessage(string text)
            : base(Methods.TERMINAL_MESSAGE)
        {
            this.text = text;
        }
    }

    public class VaultCardMessage : Message
    {
        public int? cardEntryMethods { get; set; }

        public VaultCardMessage(int? CardEntryMethods) : base(Methods.VAULT_CARD)
        {
            this.cardEntryMethods = CardEntryMethods;
        }
    }

    public class VoidPaymentResponseMessage : Message
    {
        public Payment payment { get; set; }
        public VoidReason voidReason { get; set; }
        public bool success { get; set; }
        public ResultStatus status { get; set; }
        public string reason { get; set; }
        public string message { get; set; }

        public VoidPaymentResponseMessage(Payment payment, VoidReason voidReason, bool success, ResultStatus status, string reason, string message)
            : base(Methods.VOID_PAYMENT_RESPONSE)
        {
            this.payment = payment;
            this.voidReason = voidReason;
            this.success = success;
            this.status = status;
            this.reason = reason;
            this.message = message;
        }
    }

    public class VaultCardResponseMessage : Message
    {
        public VaultedCard card { get; set; }
        public string reason { get; set; }
        public ResultStatus status { get; set; }

        public VaultCardResponseMessage()
            : base(Methods.VAULT_CARD_RESPONSE)
        {
        }
    }

    public class ReadCardDataMessage : Message
    {
        public readonly PayIntent payIntent;
        public ReadCardDataMessage(PayIntent PayIntentIn)
            : base(Methods.CARD_DATA)
        {
            this.payIntent = PayIntentIn;
        }
    }

    public class ReadCardDataResponseMessage : Message
    {
        public CardData cardData { get; set; }
        public string reason { get; set; }
        public ResultStatus status { get; set; }

        public ReadCardDataResponseMessage()
            : base(Methods.CARD_DATA_RESPONSE)
        {

        }
    }

    public class OpenCashDrawerMessage : Message
    {
        public string reason { get; set; }
        public Printer printer { get; set; }

        public OpenCashDrawerMessage()
            : base(Methods.OPEN_CASH_DRAWER)
        {

        }

        public OpenCashDrawerMessage(string reason, Printer printer)
            : base(Methods.OPEN_CASH_DRAWER)
        {
            this.reason = reason;
            this.printer = printer;
        }

        public OpenCashDrawerMessage(string reason)
            : base(Methods.OPEN_CASH_DRAWER)
        {
            this.reason = reason;
        }
    }

    public class CloseoutMessage : Message
    {
        public bool allowOpenTabs { get; set; }
        public string batchId { get; set; }

        public CloseoutMessage() : base(Methods.CLOSEOUT_REQUEST)
        {
            allowOpenTabs = false;
            batchId = null;
        }

        public CloseoutMessage(bool allowOpenTabs, string batchId) : base(Methods.CLOSEOUT_REQUEST)
        {
            this.allowOpenTabs = allowOpenTabs;
            this.batchId = batchId;
        }
    }

    public class CloseoutResponseMessage : Message
    {
        public ResultStatus status { get; set; }
        public string reason { get; set; }
        public Batch batch { get; set; }

        public CloseoutResponseMessage() : base(Methods.CLOSEOUT_RESPONSE)
        {
        }
    }

    public class ConfirmPaymentMessage : Message
    {
        public Payment payment { get; set; }
        public List<Challenge> challenges { get; set; }

        public ConfirmPaymentMessage(Payment payment, List<Challenge> challenges)
            : base(Methods.CONFIRM_PAYMENT_MESSAGE)
        {
            this.payment = payment;
            this.challenges = challenges;
        }
    }

    public class PaymentConfirmedMessage : Message
    {
        public Payment payment { get; set; }

        public PaymentConfirmedMessage()
            : base(Methods.PAYMENT_CONFIRMED)
        {
        }
    }

    public class PaymentRejectedMessage : Message
    {
        public Payment payment { get; set; }
        public VoidReason reason { get; set; }

        public PaymentRejectedMessage()
            : base(Methods.PAYMENT_REJECTED)
        {
        }
    }

    public class ActivityRequest : Message
    {
        public string action { get; set; }
        public string payload { get; set; }
        public bool nonBlocking { get; set; }
        public bool forceLaunch { get; set; }

        public ActivityRequest()
            : base(Methods.ACTIVITY_REQUEST)
        {

        }
    }

    public class ActivityResponseMessage : Message
    {
        public int resultCode { get; set; }
        public string failReason { get; set; }
        public string payload { get; set; }
        public string action { get; set; }

        public ActivityResponseMessage()
            : base(Methods.ACTIVITY_RESPONSE)
        {

        }
    }

    public class KeyPressMessage : Message
    {
        public KeyPress keyPress { get; set; }

        public KeyPressMessage()
            : base(Methods.KEY_PRESS)
        {
        }

        public KeyPressMessage(KeyPress keyPre)
            : base(Methods.KEY_PRESS)
        {
            this.keyPress = keyPre;
        }
    }

    public class SignatureVerifiedMessage : Message
    {
        public Payment payment;
        public bool verified;

        public SignatureVerifiedMessage(Payment payment, bool verified)
            : base(Methods.SIGNATURE_VERIFIED)
        {
            this.payment = payment;
            this.verified = verified;
        }
    }

    public class OrderUpdateMessage : Message
    {
        public string order { get; set; }
        public string lineItemsAddedOperation { get; set; }
        public string lineItemsDeletedOperation { get; set; }
        public string discountsAddedOperation { get; set; }
        public string discountsDeletedOperation { get; set; }
        public string orderDeletedOperation { get; set; }

        public OrderUpdateMessage()
            : base(Methods.SHOW_ORDER_SCREEN)
        {

        }

        public OrderUpdateMessage(DisplayOrder displayOrder)
            : base(Methods.SHOW_ORDER_SCREEN)
        {
            order = JsonUtils.Serialize(displayOrder);
        }

        public void setOperation(DisplayOperation operation)
        {
            if (null == operation)
            {
                return;
            }

            if (operation is LineItemsAddedOperation)
            {
                this.lineItemsAddedOperation = JsonUtils.Serialize(operation);
            }
            else if (operation is LineItemsDeletedOperation)
            {
                this.lineItemsDeletedOperation = JsonUtils.Serialize(operation);
            }
            else if (operation is DiscountsDeletedOperation)
            {
                this.discountsDeletedOperation = JsonUtils.Serialize(operation);
            }
            else if (operation is DiscountsAddedOperation)
            {
                this.discountsAddedOperation = JsonUtils.Serialize(operation);
            }
            else if (operation is OrderDeletedOperation)
            {
                this.orderDeletedOperation = JsonUtils.Serialize(operation);
            }
        }
    }

    /// <summary>
    /// Message used when logging from a remote source to the Clover device.
    /// </summary
    public class LogMessage : Message
    {
        public LogLevelEnum logLevel;
        public Dictionary<string, string> messages;

        public LogMessage(LogLevelEnum logLevel, Dictionary<string, string> messages)
            : base(Methods.LOG_MESSAGE)
        {
            this.logLevel = logLevel;
            this.messages = messages;
        }
    }

    public class RetrievePendingPaymentsMessage : Message
    {
        public RetrievePendingPaymentsMessage()
            : base(Methods.RETRIEVE_PENDING_PAYMENTS)
        {
        }
    }

    public class RetrievePendingPaymentsResponseMessage : Message
    {
        public ResultStatus status { get; set; }
        public string message { get; set; }
        public List<PendingPaymentEntry> pendingPaymentEntries { get; set; }

        public RetrievePendingPaymentsResponseMessage()
            : base(Methods.RETRIEVE_PENDING_PAYMENTS_RESPONSE)
        {
        }
    }

    public class ActivityMessageToActivity : Message
    {
        public string action { get; set; }
        public string payload { get; set; }

        public ActivityMessageToActivity(string action, string payload)
            : base(Methods.ACTIVITY_MESSAGE_TO_ACTIVITY)
        {
            this.action = action;
            this.payload = payload;
        }
    }

    public class ActivityMessageFromActivity : Message
    {
        public string action { get; set; }
        public string payload { get; set; }

        public ActivityMessageFromActivity(string action, string payload)
            : base(Methods.ACTIVITY_MESSAGE_FROM_ACTIVITY)
        {
            this.action = action;
            this.payload = payload;
        }
    }

    public enum ExternalDeviceState
    {
        UNKNOWN,
        IDLE,
        BUSY,
        WAITING_FOR_POS,
        WAITING_FOR_CUSTOMER
    }

    public enum ExternalDeviceSubState
    {
        UNKNOWN,
        CUSTOM_ACTIVITY,
        STARTING_PAYMENT_FLOW,
        PROCESSING_PAYMENT,
        PROCESSING_CARD_DATA,
        PROCESSING_CREDIT,
        VERIFY_SIGNATURE,
        TIP_SCREEN,
        RECEIPT_SCREEN,
        CONFIRM_PAYMENT
    }

    public class ExternalDeviceStateData
    {
        public string externalPaymentId { get; set; }
        public string customActivityId { get; set; }
    }

    /// <summary>
    /// request for current status of the Clover device
    /// </summary>
    public class RetrieveDeviceStatusRequest
    {
        public bool sendLastMessage { get; set; }

        public RetrieveDeviceStatusRequest()
        {
            this.sendLastMessage = false;
        }
        public RetrieveDeviceStatusRequest(bool sendLastMessage)
        {
            this.sendLastMessage = sendLastMessage;
        }
    }

    public class RetrievePrintersRequest
    {
        public PrintCategory category;

        public RetrievePrintersRequest()
        {
        }

        public RetrievePrintersRequest(PrintCategory category)
        {
            this.category = category;
        }
    }

    public enum PrintCategory
    {
        NONE,
        ORDER,
        RECEIPT
    }

    public class RetrievePrintersRequestMessage : Message
    {
        public PrintCategory category { get; set; }
        public RetrievePrintersRequestMessage()
            : base(Methods.GET_PRINTERS_REQUEST)
        {
        }
    }

    public class RetrievePrintersResponseMessage : Message
    {
        public List<Printer> printers = new List<Printer>();

        public RetrievePrintersResponseMessage()
            : base(Methods.GET_PRINTERS_RESPONSE)
        {
        }

        public RetrievePrintersResponseMessage(Printer printer) : base(Methods.GET_PRINTERS_RESPONSE)
        {
            printers.Add(printer);
        }
    }

    public class PrintJobStatusResponseMessage : Message
    {
        public string externalPrintJobId;
        public string status;

        public PrintJobStatusResponseMessage()
            : base(Methods.PRINT_JOB_STATUS_RESPONSE)
        {
        }
    }

    public class ShowReceiptOptionsMessage : Message
    {
        public string orderId;
        public string paymentId;
        public string refundId;
        public string creditId;
        public bool disableCloverPrinting;
        public Payment payment;
        public Credit credit;
        public Refund refund;
        public bool isReprint;

        protected ShowReceiptOptionsMessage(Payment payment, Credit credit, Refund refund, bool isReprint)
            : base(Methods.SHOW_RECEIPT_OPTIONS)
        {
            this.disableCloverPrinting = false;
            this.payment = payment;
            this.credit = credit;
            this.refund = refund;
            this.isReprint = isReprint;
        }

        public ShowReceiptOptionsMessage(string orderId, string paymentId, string refundId, string creditId, bool disableCloverPrinting)
            : base(Methods.SHOW_RECEIPT_OPTIONS)
        {
            this.orderId = orderId;
            this.paymentId = paymentId;
            this.refundId = refundId;
            this.creditId = creditId;
            this.disableCloverPrinting = disableCloverPrinting;
        }
    }

    public class ShowReceiptOptionsResponseMessage : Message
    {
        public ResultStatus status;
        public string reason;

        public ShowReceiptOptionsResponseMessage(ResultStatus status, string reason)
            : base(Methods.SHOW_RECEIPT_OPTIONS_RESPONSE)
        {
            this.status = status;
            this.reason = reason;
        }
    }

    public class CustomerProvidedDataResponseMessage : Message
    {
        public string eventId;
        public DataProviderConfig config;
        public string data;

        public CustomerProvidedDataResponseMessage(string eventId, DataProviderConfig config, string data)
            : base(Methods.CUSTOMER_PROVIDED_DATA_MESSAGE)
        {
            this.eventId = eventId;
            this.config = config;
            this.data = data;
        }
    }

    /// <summary>
    /// request to retrieve a payment associated with the provided externalPaymentId
    /// </summary>
    public class RetrievePaymentRequest
    {
        public string externalPaymentId { get; set; }

        public RetrievePaymentRequest()
        {
        }

        public RetrievePaymentRequest(string externalPaymentId)
        {
            this.externalPaymentId = externalPaymentId;
        }
    }

    public class PrintJobStatusRequestMessage : Message
    {
        public string externalPrintJobId;

        public PrintJobStatusRequestMessage()
            : base(Methods.PRINT_JOB_STATUS_REQUEST)
        {
        }

        public PrintJobStatusRequestMessage(string printRequestId) : base(Methods.PRINT_JOB_STATUS_REQUEST)
        {
            this.externalPrintJobId = printRequestId;
        }
    }

    public class RetrieveDeviceStatusRequestMessage : Message
    {
        public bool sendLastMessage { get; set; }

        public RetrieveDeviceStatusRequestMessage()
            : base(Methods.RETRIEVE_DEVICE_STATUS_REQUEST)
        {
            this.sendLastMessage = false;
        }

        public RetrieveDeviceStatusRequestMessage(bool sendLastMessage) : base(Methods.RETRIEVE_DEVICE_STATUS_REQUEST)
        {
            this.sendLastMessage = sendLastMessage;
        }

    }

    public class RetrieveDeviceStatusResponseMessage : Message
    {
        public ResultStatus result { get; set; }
        public string reason { get; set; }
        public ExternalDeviceState state { get; set; }
        public ExternalDeviceSubState substate { get; set; }
        public ExternalDeviceStateData data { get; set; }

        public RetrieveDeviceStatusResponseMessage()
            : base(Methods.RETRIEVE_DEVICE_STATUS_RESPONSE)
        {
        }
    }

    public class ResetDeviceResponseMessage : Message
    {
        public ResultStatus result { get; set; }
        public string reason { get; set; }
        public ExternalDeviceState state { get; set; }

        public ResetDeviceResponseMessage()
            : base(Methods.RESET_DEVICE_RESPONSE)
        {
        }
    }

    public class AcknowledgementMessage : Message
    {
        public string sourceMessageId { get; set; }

        public AcknowledgementMessage()
            : base(Methods.ACK)
        {
        }
    }

    public class CreditPrintMessage : Message
    {
        public Credit credit { get; set; }

        public CreditPrintMessage()
            : base(Methods.PRINT_CREDIT)
        {
        }
    }

    public class DeclineCreditPrintMessage : Message
    {
        public Credit credit { get; set; }
        public string reason { get; set; }

        public DeclineCreditPrintMessage()
            : base(Methods.PRINT_CREDIT_DECLINE)
        {
        }
    }

    public class PaymentPrintMessage : Message
    {
        public Payment payment { get; set; }
        public Order order { get; set; }

        public PaymentPrintMessage()
            : base(Methods.PRINT_PAYMENT)
        {
        }
    }

    public class DeclinePaymentPrintMessage : Message
    {
        public Payment payment { get; set; }
        public string reason { get; set; }

        public DeclinePaymentPrintMessage()
            : base(Methods.PRINT_PAYMENT_DECLINE)
        {
        }
    }

    public class PaymentPrintMerchantCopyMessage : Message
    {
        public Payment payment { get; set; }

        public PaymentPrintMerchantCopyMessage()
            : base(Methods.PRINT_PAYMENT_MERCHANT_COPY)
        {
        }
    }

    public class CloverDeviceLogMessage : Message
    {
        public string Message { get; set; }

        public CloverDeviceLogMessage(string message)
            : base(Methods.CLOVER_DEVICE_LOG_REQUEST)
        {
            Message = message;
        }
    }

    public class RefundPaymentPrintMessage : Message
    {
        public Payment payment { get; set; }
        public Refund refund { get; set; }
        public Order order { get; set; }

        public RefundPaymentPrintMessage()
            : base(Methods.REFUND_PRINT_PAYMENT)
        {
        }
    }

    public class PairingRequestMessage
    {
        public readonly int version = 1;
        public string method { get; set; }
        public string payload { get; set; }

        public PairingRequestMessage(PairingRequest pr)
        {
            method = "PAIRING_REQUEST";
            payload = JsonUtils.Serialize(pr);
        }

        public PairingRequestMessage(string method)
        {
            this.method = method;
        }
    }

    public class PairingCodeMessage
    {
        public const string METHOD = "PAIRING_CODE";

        public string pairingCode { get; set; }
    }

    public class PairingRequest
    {
        public const string METHOD = "PAIRING_REQUEST";

        public string method = METHOD;
        public string serialNumber { get; set; }
        public string name { get; set; }
        public string authenticationToken { get; set; }
    }

    public class PairingResponseMessage : PairingRequestMessage
    {
        public PairingResponseMessage()
            : base("PAIRING_RESPONSE")
        {
        }
    }

    public class PairingResponse : PairingRequest
    {
        public const string METHOD = "PAIRING_RESPONSE";

        public const string PAIRED = "PAIRED";
        public const string INITIAL = "INITIAL";
        public const string FAILED = "FAILED";
        public const string AUTHENTICATING = "AUTHENTICATING";
        public const string PAIRING = "PAIRING";

        public string pairingState { get; set; }
        public string applicationName { get; set; }
        public long millis;
    }

    /// <summary>
    /// Request to retrieve a payment made to a specific device.
    /// </summary>
    /// 
    public class PaymentRequestMessage : Message
    {
        public string externalPaymentId { get; set; }

        public PaymentRequestMessage()
            : base(Methods.RETRIEVE_PAYMENT_REQUEST)
        {

        }
    }

    /// <summary>
    /// Response from a RetrievePaymentRequest.
    /// </summary>
    public class RetrievePaymentResponseMessage : Message
    {
        public ResultStatus status { get; set; }
        public string reason { get; set; }

        /// <summary>
        /// The status of the query
        /// </summary>
        public QueryStatus queryStatus { get; set; }

        /// <summary>
        /// Payment information
        /// </summary>
        public Payment payment { get; set; }

        /// <summary>
        /// The externalPaymentId used when a payment was created
        /// </summary>
        public string externalPaymentId { get; set; }

        public RetrievePaymentResponseMessage()
            : base(Methods.RETRIEVE_PAYMENT_RESPONSE)
        {
        }
    }

    /// <summary>
    /// Loyalty API register a set of configurations to send to the Clover Connector
    /// </summary>
    public class RegisterForCustomerProvidedDataMessage : Message
    {
        public List<LoyaltyDataConfig> configurations { get; set; }

        public RegisterForCustomerProvidedDataMessage()
            : base(Methods.REGISTER_FOR_CUST_DATA)
        {
        }
    }

    public class CustomerInfoMessage : Message
    {
        public CustomerInfo customer { get; set; }

        public CustomerInfoMessage()
            : base(Methods.CUSTOMER_INFO_MESSAGE)
        {
        }
    }

    public class InvalidStateTransitionMessage : Message
    {
        public string reason { get; set; }
        public string requestedTransition { get; set; }
        public string state { get; set; }
        public string substate { get; set; }
        public string result { get; set; }
        public ExternalDeviceStateData data { get; set; }

        public InvalidStateTransitionMessage(string reason, string requestedTransition, string state, string substate, string result, ExternalDeviceStateData data)
            : base(Methods.INVALID_STATE_TRANSITION)
        {
            this.reason = reason;
            this.requestedTransition = requestedTransition;
            this.state = state;
            this.substate = substate;
            this.result = result;
            this.data = data;
        }
    }

    /// <summary>
    /// The top level protocol message
    /// </summary
    public class RemoteMessage
    {
        public string id { get; set; }
        public Methods? method { get; set; }
        public MessageTypes type { get; set; }
        public string payload { get; set; }
        public string packageName { get; set; }
        public string remoteSourceSDK { get; set; }
        public string remoteApplicationID { get; set; }
        public int version { get; set; }
        public string attachment { get; set; }
        public string attachmentUri { get; set; }
        public string attachmentEncoding { get; set; }
        public bool? lastFragment;
        public int? fragmentIndex;

        public static RemoteMessage createMessage(Methods meth, MessageTypes msgType, Message payload, string packageName, string remoteSourceSDK, string remoteApplicationID)
        {
            RemoteMessage msg = new RemoteMessage();
            msg.method = meth;
            msg.type = msgType;
            if (null == payload)
            {
                payload = new Message(meth);
            }
            msg.payload = JsonUtils.SerializeSdk(payload);
            msg.packageName = packageName;
            msg.remoteSourceSDK = remoteSourceSDK;
            msg.remoteApplicationID = remoteApplicationID;
            msg.id = nextID();
            return msg;
        }

        public static RemoteMessage CreatePongMessage(string packageName, string remoteSourceSDK, string remoteApplicationID)
        {
            RemoteMessage msg = new RemoteMessage
            {
                method = null,
                type = MessageTypes.PONG,
                packageName = packageName,
                remoteSourceSDK = remoteSourceSDK,
                remoteApplicationID = remoteApplicationID,
                id = nextID()
            };
            return msg;
        }

        private static long _id = 0;
        private static string nextID()
        {
            return (++_id).ToString();
        }
    }

    public enum LogLevelEnum
    {
        VERBOSE,
        DEBUG,
        INFO,
        WARN,
        ERROR
    }

    public enum TxStartResponseResult
    {
        SUCCESS,
        ORDER_MODIFIED,
        ORDER_LOAD,
        FAIL,
        DUPLICATE
    }
}
