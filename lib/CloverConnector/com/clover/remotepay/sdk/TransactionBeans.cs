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
using com.clover.remotepay.transport;
using com.clover.sdk.v3.payments;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace com.clover.remotepay.sdk
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseRequest {
        protected BaseRequest()
        {
        }
        private Guid _RequestMessageUUID;
        /// <summary>
        /// The UUID used to correlate a request and response message.
        /// </summary>
        public Guid RequestMessageUUID
        { 
            get 
            {
                if (_RequestMessageUUID == Guid.Empty)
                {
                    _RequestMessageUUID = Guid.NewGuid();
                }
                return _RequestMessageUUID;
            } 
            set
            {
                if (_RequestMessageUUID == Guid.Empty)
                {
                    _RequestMessageUUID = value;
                }
                else
                {
                    throw new ArgumentException("Request Message UUID is already set!");
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseResponse
    {
        public static readonly string SUCCESS = "SUCCESS";
        public static readonly string CANCEL = "CANCEL";
        public static readonly string FAIL = "FAIL";
        public static readonly string ERROR = "ERROR";

        protected BaseResponse()
        {

        }
        protected BaseResponse(Guid requestUUID)
        {
            RequestMessageUUID = requestUUID;
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid RequestMessageUUID { get; set; }

        /// <summary>
        /// the status of the transaction activity.
        /// </summary>
        public string Code { get; set; } //SUCCESS, CANCEL, ERROR, FAIL - TODO: enum

    }

    /// <summary>
    /// This request should be used for Auth only, but is backward
    /// compatible for older implementations.
    /// If you are currently using an AuthRequest with IsPreAuth = true,
    /// please change your code to use PreAuthRequest/PreAuthResponse
    /// for all PreAuth transactions.
    /// </summary>
    public class AuthRequest : SaleRequest
    {
        public AuthRequest()
        {
            IsPreAuth = false;
        }
        public bool IsPreAuth { get; set; } 
        public override PayIntent.TransactionType Type 
        {
            get {return IsPreAuth ? PayIntent.TransactionType.AUTH : PayIntent.TransactionType.PAYMENT;} 
        } 
        //public string OrderID { get; set; } // optional TODO: Revisit
        //public string EmployeeID { get; set; } // optional TODO: Revisit
    }

    /// <summary>
    /// 
    /// </summary>
    public class AuthResponse : SaleResponse
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
        public override PayIntent.TransactionType Type
        {
            get
            {
                return PayIntent.TransactionType.AUTH;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PreAuthResponse : AuthResponse
    {
    }
    /// <summary>
    /// 
    /// </summary>
    public class CaptureAuthRequest : BaseRequest
    {
        public string PaymentID { get; set; }
        public long Amount { get; set; }
        public long TipAmount { get; set; }
    }

    public class VaultCardResponse : TransactionResponse
    {
        public VaultedCard Card { get; set; }
        public ResultStatus Status { get; set; }
        public string Reason { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CaptureAuthResponse : TransactionResponse
    {
        public string paymentId { get; set; }
        public long amount { get; set; }
        public long tipAmount { get; set; }
    }

    public class CloseoutRequest : BaseRequest
    {
        public bool allowOpenTabs { get; set; }
        public string batchId { get; set; }
    }

    public class CloseoutResponse : BaseResponse
    {
        public ResultStatus status { get; set; }
        public string reason { get; set; }
        public Batch batch { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TipAdjustAuthRequest : BaseRequest
    {
        public string PaymentID { get; set; }
        public string OrderID { get; set; }
        public long TipAmount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TipAdjustAuthResponse : BaseResponse
    {
        public string PaymentId { get; set; }
        public long Amount { get; set; }
        public bool Success { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConfigErrorResponse : BaseResponse
    {
        public string message { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class TransactionRequest : BaseRequest
    {
        public long Amount { get; set; }
        public bool CardNotPresent { get; set; }
        public long? CardEntryMethod { get; set; }
        public VaultedCard VaultedCard { get; set; }
        public string ExternalPaymentId { get; set; } 
        //public string EmployeeID { get; set; } // optional TODO: Revisit

        public abstract PayIntent.TransactionType Type { get; } // what type of transaction is this
    }
    /// <summary>
    /// 
    /// </summary>
    public class TransactionResponse : BaseResponse
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class SaleRequest : TransactionRequest
    {

        public SaleRequest() 
        {
            DisableCashback = false;
            DisablePrinting = false;
            DisableTip = false;
            DisableRestartTransactionOnFail = false;
        }
        public override PayIntent.TransactionType Type
        {
            get
            {
                return PayIntent.TransactionType.PAYMENT;
            }
        }
        public long? TaxAmount { get; set; }
        public long? TippableAmount { get; set; } // the amount that tip should be calculated on
        public long? TipAmount { get; set; }
        public bool DisableCashback { get; set; } // 
        public bool DisableTip { get; set; } // if the merchant account is
        public bool DisablePrinting { get; set; }
        public bool DisableRestartTransactionOnFail { get; set; }
        public bool? AllowOfflinePayment { get; set; }
        public bool? ApproveOfflinePaymentWithoutPrompt { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SaleResponse : TransactionResponse
    {
        public Payment Payment { get; set; }
        public Signature2 Signature { get; set; } // optional
    }

    /// <summary>
    /// 
    /// </summary>
    public class SignatureVerifyRequest
    {
        public virtual void Accept() { }
        public virtual void Reject() { }
        public Signature2 Signature { get; set; }
        public Payment Payment { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VoidPaymentRequest : BaseRequest
    {
        public string PaymentId { get; set; }
        public string VoidReason { get; set; } // {USER_CANCEL}

        public string EmployeeId { get; set; }//optional TODO: Revisit
        public string OrderId { get; set; } //optional TODO: Revisit
    }

    /// <summary>
    /// 
    /// </summary>
    public class VoidPaymentResponse : TransactionResponse
    {
        public string PaymentId { get; set; }
        public string TransactionNumber { get; set; } //optional?
        public string ResponseCode { get; set; } //optional?
        public string ResponseText { get; set; } //optional?
    }


    /// <summary>
    /// Request used to void a transaction, where no response was received. 
    /// </summary>
    public class VoidTransactionRequest : BaseRequest
    {
        public Guid OriginalRequestUUID { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VoidTransactionResponse : TransactionResponse
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class ManualRefundRequest : BaseRequest
    {
        public long Amount { get; set; }
        public long? CardEntryMethod { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ManualRefundResponse : TransactionResponse
    {
        public Credit Credit { get; set; }
        public string TransactionNumber { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseText { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RefundPaymentRequest : BaseRequest
    {
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public long Amount { get; set; } // optional
    }

    /// <summary>
    /// 
    /// </summary>
    public class RefundPaymentResponse : BaseResponse
    {
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public Refund RefundObj { get; set; }
        public string Message { get; set; }

        //public TxState Code { get; set; }// BaseResponse.Case is a string, so won't serialize
    }

    /// <summary>
    /// 
    /// </summary>
    public class PingRequest : BaseRequest
    {
        public long Timestamp { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PingResponse : BaseResponse
    {
        public long RequestTime { get; set; }
        public long ResponseTime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DisplayMessageRequest
    {
        public string Message { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DisplayPaymentReceiptOptionsRequest : BaseRequest
    {
        // TODO: add flags for options? SMS = false; EMAIL = true; PRINT = true; etc.
        public string OrderID { get; set; }
        public string PaymentID { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DisplayRefundReceiptOptionsRequest : BaseRequest
    {
        // TODO: add flags for options? SMS = false; EMAIL = true; PRINT = true; etc.
        public string OrderID { get; set; }
        public string RefundID { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DisplayCreditReceiptOptionsRequest : BaseRequest
    {
        // TODO: add flags for options? SMS = false; EMAIL = true; PRINT = true; etc.
        public string OrderID { get; set; }
        public string CreditID { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DisplayReceiptOptionsResponse : BaseResponse
    {

    }

}
