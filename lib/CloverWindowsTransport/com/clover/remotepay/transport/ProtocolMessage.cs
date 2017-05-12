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

using com.clover.remote.order;
using com.clover.remote.order.operation;
using com.clover.remotepay.data;
using com.clover.sdk.v3.order;
using com.clover.sdk.v3.payments;
using com.clover.sdk.v3.customers;
using System;
using System.Collections.Generic;

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
        public readonly String merchantId;
        public readonly String merchantName;
        public readonly String merchantMId;
        public readonly String name;
        public readonly String serial;
        public readonly String model;
        public readonly bool ready;
        public readonly bool supportsAcknowledgement;
        public readonly bool supportsTipAdjust;
        public readonly bool supportsManualRefund;
        public readonly bool supportsMultiPayToken;
        public readonly bool supportsRemoteConfirmation;
        public DiscoveryResponseMessage(String merchantId,
                                        String merchantName,
                                        String merchantMId,
                                        String name,
                                        String serial,
                                        String model,
                                        Boolean ready,
                                        Boolean? supportsAcknowledgement,
                                        Boolean? supportsTipAdjust,
                                        Boolean? supportsManualRefund,
                                        Boolean? supportsMultiPayToken)
            : base(Methods.DISCOVERY_RESPONSE)
        {
            this.merchantId = merchantId;
            this.merchantName = merchantName != null ? merchantName : "";
            this.merchantMId = merchantMId != null ? merchantMId : "";
            this.name = name;
            this.serial = serial;
            this.model = model;
            this.ready = ready;
            this.supportsAcknowledgement = supportsAcknowledgement.HasValue ? supportsAcknowledgement.Value : false;
            this.supportsTipAdjust = supportsTipAdjust.HasValue ? supportsTipAdjust.Value : true;
            this.supportsManualRefund = supportsManualRefund.HasValue ? supportsManualRefund.Value : true;
            this.supportsMultiPayToken = supportsMultiPayToken.HasValue ? supportsMultiPayToken.Value : true;
        }
    }
    public class PaymentReceiptMessage : Message
    {
        public string orderId { get; set; }
        public string paymentId { get; set; }
        public PaymentReceiptMessage(string orderId, string paymentId)
            : base(Methods.SHOW_PAYMENT_RECEIPT_OPTIONS, 2) //automatically sets the version to the newer, preferable version
        {
            this.orderId = orderId;
            this.paymentId = paymentId;
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

        public TextPrintMessage()
            : base(Methods.PRINT_TEXT)
        {
            textLines = new List<string>();
        }
    }

    public class ImagePrintMessage : Message
    {
        public ImagePrintMessage() : base(Methods.PRINT_IMAGE)
        {
        }

        public string png { get; set; }
        public string urlString { get; set; }
    }

    public class FinishCancelMessage : Message
    {
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

        public FinishOkMessage()
            : base(Methods.FINISH_OK)
        {
        }
    }

    public class TxStartRequestMessage : Message
    {
        public readonly PayIntent payIntent;
        public readonly Order order;
        [Obsolete("use TipMode in PayIntent.transactionSettings instead")]
        public readonly bool? suppressOnScreenTips;

        public TxStartRequestMessage(PayIntent payIntent, Order order, bool suppressOnScreenTips)
            : base(Methods.TX_START)
        {
            this.payIntent = payIntent;
            this.order = order;
            this.suppressOnScreenTips = suppressOnScreenTips;
        }

        public TxStartRequestMessage(PayIntent payIntent, Order order)
            : base(Methods.TX_START, 2)
        {
            this.payIntent = payIntent;
            this.order = order;
            this.suppressOnScreenTips = null;
        }
    }

    public class TxStartResponseMessage : Message
    {
        public Order order { get; set; }
        public TxStartResponseResult result { get; set; }
        public string externalId { get; set; }

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

        public PartialAuthMessage() : base(Methods.PARTIAL_AUTH) { }

        public PartialAuthMessage(long partialAuth)
            : base(Methods.PARTIAL_AUTH)
        {
            this.partialAuthAmount = partialAuth;
        }
    }

    public class CashbackSelectedMessage : Message
    {
        public long cashbackAmount { get; set; }

        public CashbackSelectedMessage() : base(Methods.CASHBACK_SELECTED) { }

        public CashbackSelectedMessage(long cashback)
            : base(Methods.CASHBACK_SELECTED)
        {
            this.cashbackAmount = cashback;
        }
    }

    public class TipAddedMessage : Message
    {
        public long tipAmount { get; set; }

        public TipAddedMessage() : base(Methods.TIP_ADDED) { }

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

        public RefundRequestMessage() : base(Methods.REFUND_REQUEST, 2)
        {

        }

        public RefundRequestMessage(string oid, string pid, long? amt, bool? fullRefund) : base(Methods.REFUND_REQUEST, 2)
        {
            this.orderId = oid;
            this.paymentId = pid;
            this.amount = amt;
            this.fullRefund = fullRefund;
        }
    }

    public class TipAdjustAuthMessage : Message
    {
        public string orderId { get; set; }
        public string paymentId { get; set; }
        public long tipAmount { get; set; }

        public TipAdjustAuthMessage(string orderId, string paymentId, long tipAmount) : base(Methods.TIP_ADJUST)
        {
            this.orderId = orderId;
            this.paymentId = paymentId;
            this.tipAmount = tipAmount;
        }
    }

    public class TipAdjustResponseMessage : Message
    {
        public string orderId { get; set; }
        public string paymentId { get; set; }
        public bool success { get; set; }
        public long amount { get; set; }

        public TipAdjustResponseMessage(string orderId, string paymentId, long amount, bool success) : base(Methods.TIP_ADJUST_RESPONSE)
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

        public CapturePreAuthMessage(string paymentID, long amount, long tipAmount) : base(Methods.CAPTURE_PREAUTH)
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

        public CapturePreAuthResponseMessage(ResultStatus status, string reason, string paymentID, long amount, long tipAmount) : base(paymentID, amount, tipAmount)
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
        public String orderId { get; set; }
        /// <summary>
        /// Unique identifier for a payment
        /// </summary>
        public String paymentId { get; set; }
        /// <summary>
        /// The refund
        /// </summary>
        public com.clover.sdk.v3.payments.Refund refund { get; set; }
        /// <summary>
        /// Detail code if an error is encountered
        /// </summary>
        public ResponseReasonCode? reason { get; set; }
        /// <summary>
        /// Detail message
        /// </summary>
        public String message { get; set; }
        /// <summary>
        /// Transaction state (success|fail)
        /// </summary>
        public TxState code { get; set; }

        public RefundResponseMessage() : base(Methods.REFUND_RESPONSE) { }

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
        public readonly String uiText;
        public readonly UiDirection uiDirection;
        public readonly InputOption[] inputOptions;

        public UiStateMessage(UiState uiState, String uiText, UiDirection uiDirection, InputOption[] inputOptions)
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

        public VoidPaymentMessage() : base(Methods.VOID_PAYMENT)
        {
        }

    }


    public class TerminalMessage : Message
    {
        public readonly String text;

        public TerminalMessage(String text)
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

    public class VaultCardResponseMessage : Message
    {
        public VaultedCard card { get; set; }
        public string reason { get; set; }
        public ResultStatus status { get; set; }

        public VaultCardResponseMessage() : base(Methods.VAULT_CARD_RESPONSE)
        {

        }
    }

    public class ReadCardDataMessage : Message
    {
        public readonly PayIntent payIntent;
        public ReadCardDataMessage(PayIntent PayIntentIn) : base(Methods.CARD_DATA)
        {
            this.payIntent = PayIntentIn;
        }
    }

    public class ReadCardDataResponseMessage : Message
    {
        public CardData cardData { get; set; }
        public string reason { get; set; }
        public ResultStatus status { get; set; }

        public ReadCardDataResponseMessage() : base(Methods.CARD_DATA_RESPONSE)
        {

        }
    }

    public class OpenCashDrawerMessage : Message
    {
        public String reason { get; set; }

        public OpenCashDrawerMessage() : base(Methods.OPEN_CASH_DRAWER)
        {

        }

        public OpenCashDrawerMessage(String reaseon) : base(Methods.OPEN_CASH_DRAWER)
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

        public ConfirmPaymentMessage(Payment payment, List<Challenge> challenges) : base(Methods.CONFIRM_PAYMENT_MESSAGE)
        {
            this.payment = payment;
            this.challenges = challenges;
        }
    }

    public class PaymentConfirmedMessage : Message
    {
        public Payment payment { get; set; }

        public PaymentConfirmedMessage() : base(Methods.PAYMENT_CONFIRMED)
        {
        }
    }

    public class PaymentRejectedMessage : Message
    {
        public Payment payment { get; set; }
        public VoidReason reason { get; set; }

        public PaymentRejectedMessage() : base(Methods.PAYMENT_REJECTED)
        {
        }
    }

    public class KeyPressMessage : Message
    {
        public KeyPress keyPress { get; set; }

        public KeyPressMessage() : base(Methods.KEY_PRESS) { }

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


        public OrderUpdateMessage() : base(Methods.SHOW_ORDER_SCREEN)
        {

        }

        public OrderUpdateMessage(DisplayOrder displayOrder) : base(Methods.SHOW_ORDER_SCREEN)
        {
            this.order = JsonUtils.serialize(displayOrder);
            //System.Console.WriteLine("Serialized Order:" + this.order );
        }

        public void setOperation(DisplayOperation operation)
        {
            if (null == operation) { return; }

            if (operation is LineItemsAddedOperation)
            {
                this.lineItemsAddedOperation = JsonUtils.serialize(operation);
            }
            else if (operation is LineItemsDeletedOperation)
            {
                this.lineItemsDeletedOperation = JsonUtils.serialize(operation);
            }
            else if (operation is DiscountsDeletedOperation)
            {
                this.discountsDeletedOperation = JsonUtils.serialize(operation);
            }
            else if (operation is DiscountsAddedOperation)
            {
                this.discountsAddedOperation = JsonUtils.serialize(operation);
            }
            else if (operation is OrderDeletedOperation)
            {
                this.orderDeletedOperation = JsonUtils.serialize(operation);
            }
        }
    }

    /// <summary>
    /// Message used when logging from a remote source to the Clover device.
    /// </summary
    public class LogMessage : Message
    {
        public LogLevelEnum logLevel;
        public Dictionary<String, String> messages;

        public LogMessage(LogLevelEnum logLevel, Dictionary<String, String> messages) : base(Methods.LOG_MESSAGE)
        {
            this.logLevel = logLevel;
            this.messages = messages;
        }
    }

    public class RetrievePendingPaymentsMessage : Message
    {
        public RetrievePendingPaymentsMessage() : base(Methods.RETRIEVE_PENDING_PAYMENTS)
        {

        }
    }

    public class RetrievePendingPaymentsResponseMessage : Message
    {
        public ResultStatus status { get; set; }
        public string message { get; set; }
        public List<PendingPaymentEntry> pendingPaymentEntries { get; set; }

        public RetrievePendingPaymentsResponseMessage() : base(Methods.RETRIEVE_PENDING_PAYMENTS_RESPONSE)
        {

        }
    }

    public class AcknowledgementMessage : Message
    {
        public string sourceMessageId { get; set; }

        public AcknowledgementMessage() : base(Methods.ACK)
        {

        }
    }

    public class CreditPrintMessage : Message
    {
        public Credit credit { get; set; }
        public CreditPrintMessage() : base(Methods.PRINT_CREDIT)
        {

        }
    }

    public class DeclineCreditPrintMessage : Message
    {
        public Credit credit { get; set; }
        public String reason { get; set; }
        public DeclineCreditPrintMessage() : base(Methods.PRINT_CREDIT_DECLINE)
        {

        }
    }

    public class PaymentPrintMessage : Message
    {
        public Payment payment { get; set; }
        public Order order { get; set; }
        public PaymentPrintMessage() : base(Methods.PRINT_PAYMENT)
        {

        }
    }

    public class DeclinePaymentPrintMessage : Message
    {
        public Payment payment { get; set; }
        public String reason { get; set; }
        public DeclinePaymentPrintMessage() : base(Methods.PRINT_PAYMENT_DECLINE)
        {

        }
    }

    public class PaymentPrintMerchantCopyMessage : Message
    {
        public Payment payment { get; set; }
        public PaymentPrintMerchantCopyMessage() : base(Methods.PRINT_PAYMENT_MERCHANT_COPY)
        {

        }
    }

    public class RefundPaymentPrintMessage : Message
    {
        public Payment payment { get; set; }
        public Refund refund { get; set; }
        public Order order { get; set; }
        public RefundPaymentPrintMessage() : base(Methods.REFUND_PRINT_PAYMENT)
        {

        }
    }

    public class PairingRequestMessage
    {
        public readonly int version = 1;
        public String method { get; set; }// = "PAIRING_REQUEST";
        public string payload { get; set; }

        public PairingRequestMessage(PairingRequest pr)
        {
            method = "PAIRING_REQUEST";
            payload = JsonUtils.serialize(pr);
        }

        public PairingRequestMessage(String method)
        {

        }
    }

    public class PairingCodeMessage
    {
        public const string METHOD = "PAIRING_CODE";
        public String pairingCode { get; set; }
    }

    public class PairingRequest
    {
        public const String METHOD = "PAIRING_REQUEST";
        public String method = METHOD;
        public String serialNumber { get; set; }
        public String name { get; set; }
        public String authenticationToken { get; set; }
    }

    public class PairingResponseMessage : PairingRequestMessage
    {
        public PairingResponseMessage() : base("PAIRING_RESPONSE")
        {

        }
    }

    public class PairingResponse : PairingRequest
    {
        public const String PAIRED = "PAIRED";
        public const String INITIAL = "INITIAL";
        public const String FAILED = "FAILED";
        public const String METHOD = "PAIRING_RESPONSE";
        public String pairingState { get; set; }
        public String applicationName { get; set; }
        public long millis;
    }

    /// <summary>
    /// The top level protocol message 
    /// </summary
    public class RemoteMessage
    {
        public string id { get; set; }
        public Methods method { get; set; }
        public MessageTypes type { get; set; }
        public string payload { get; set; }
        public string packageName { get; set; }
        public string remoteSourceSDK { get; set; }
        public string remoteApplicationID { get; set; }

        public static RemoteMessage createMessage(Methods meth, MessageTypes msgType, Message payload, string packageName, string remoteSourceSDK, string remoteApplicationID)
        {
            RemoteMessage msg = new RemoteMessage();
            msg.method = meth;
            msg.type = msgType;
            if (null == payload)
            {
                payload = new Message(meth);
            }
            msg.payload = JsonUtils.serialize(payload);
            msg.packageName = packageName;
            msg.remoteSourceSDK = remoteSourceSDK;
            msg.remoteApplicationID = remoteApplicationID;
            msg.id = nextID();
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
        VERBOSE, DEBUG, INFO, WARN, ERROR
    }

    public enum TxStartResponseResult
    {
        SUCCESS, ORDER_MODIFIED, ORDER_LOAD, FAIL, DUPLICATE
    }

}
