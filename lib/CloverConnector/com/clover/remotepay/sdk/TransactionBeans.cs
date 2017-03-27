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
using com.clover.sdk.v3.payments;
using com.clover.sdk.v3.order;
using System.Collections.Generic;
using System;

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
        protected String RequestId
        {
            get; set;
        }
    }

    public enum ResponseCode
    {
        SUCCESS, FAIL, UNSUPPORTED, CANCEL, ERROR
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
        public String Reason { get; set; }
        /// <summary>
        /// Detailed information about result.
        /// </summary>
        public String Message { get; set; }

    }

    /// <summary>
    ///
    /// </summary>
    public abstract class TransactionRequest : BaseRequest
    {
        public bool? DisablePrinting { get; set; }
        public bool? CardNotPresent { get; set; }
        public bool? DisableRestartTransactionOnFail { get; set; }
        public long Amount { get; set; }
        public long? CardEntryMethods { get; set; }
        public VaultedCard VaultedCard { get; set; }
        public string ExternalId { get; set; }

        public PayIntent.TransactionType Type { get; set; }
        public long? SignatureThreshold { get; set; }
        public DataEntryLocation? SignatureEntryLocation { get; set; }
        public bool? DisableReceiptSelection { get; set; }
        public bool? DisableDuplicateChecking { get; set; }
        public bool? AutoAcceptPaymentConfirmations { get; set; }
        public bool? AutoAcceptSignature { get; set; }
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
        [System.Obsolete("DisableTipOnScreen is deprecated, please use TipMode of None instead.")]
        public bool? DisableTipOnScreen { get; set; } //
        public long? TaxAmount { get; set; }
        public long? TippableAmount { get; set; } // the amount that tip should be calculated on
        public long? TipAmount { get; set; }
        public bool? DisableCashback { get; set; } //
        public bool? AllowOfflinePayment { get; set; }
        public bool? ApproveOfflinePaymentWithoutPrompt { get; set; }
        public TipMode? TipMode { get; set; }
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
    public class PreAuthRequest : TransactionRequest
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
        public long? CardEntryMethods { get; set; }
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
        public long TipAmount { get; set; }
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
        public long TipAmount { get; set; }
    }

    /// <summary>
    /// Object passed in to OnTipAdjustAuthResponse
    /// </summary>
    public class TipAdjustAuthResponse : BaseResponse
    {
        public string PaymentId { get; set; }
        public long TipAmount { get; set; }
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
        public string VoidReason { get; set; } // {USER_CANCEL}

        public string EmployeeId { get; set; }//optional TODO: Revisit
        public string OrderId { get; set; } //optional TODO: Revisit
    }

    /// <summary>
    /// Object passed in to OnVoidPaymentResopnse
    /// </summary>
    public class VoidPaymentResponse : BaseResponse
    {
        public string PaymentId { get; set; }
    }

    /// <summary>
    /// This should be used to request a manual refund via the ManualRefund method
    /// </summary>
    public class ManualRefundRequest : TransactionRequest
    {
        public ManualRefundRequest()
        {
            this.Type = PayIntent.TransactionType.CREDIT;
        }
        public long Amount { get; set; }
        public long? CardEntryMethods { get; set; }
    }

    /// <summary>
    /// The object passed in to OnManualRefundResponse
    /// </summary>
    public class ManualRefundResponse : BaseResponse
    {
        public Credit Credit { get; set; }
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
        public long TipAmount { get; set; }
    }

    /// <summary>
    /// The request used to show the receipt options screen
    /// </summary>
    public class DisplayPaymentReceiptOptionsRequest : BaseRequest
    {
        public string OrderID { get; set; }
        public string PaymentID { get; set; }
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
        public List<PendingPaymentEntry> PendingPayments;
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
}
