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
using com.clover.sdk.v3.order;
using com.clover.sdk.v3.payments;

namespace com.clover.remotepay.transport
{
    public class PayIntent
    {
        public enum TransactionType
        {
            PAYMENT,
            CREDIT,
            AUTH,
            DATA,
            BALANCE_INQUIRY
        }

        public string action { get; set; }
        public long amount { get; set; }
        public long? tipAmount { get; set; }
        public long? taxAmount { get; set; }
        public long? tippableAmount { get; set; }
        public string externalPaymentId { get; set; }
        public VaultedCard vaultedCard { get; set; }
        public string orderId { get; set; }
        public string employeeId { get; set; }
        public PayIntent.TransactionType transactionType { get; set; }
        public List<TaxableAmountRate> taxableAmountRateList { get; set; }
        public ServiceChargeAmount serviceChargeAmount { get; set; }
        [Obsolete("use TransactionSettings instead")]
        public bool isDisableCashBack { get; set; }
        public bool isTesting { get; set; }
        [Obsolete("use TransactionSettings instead")]
        public long cardEntryMethods { get; set; }
        public string voiceAuthCode { get; set; }
        public string postalCode { get; set; }
        public string streetAddress { get; set; }
        public bool isCardNotPresent { get; set; }
        public string cardDataMessage { get; set; }
        [Obsolete("use TransactionSettings instead")]
        public bool remotePrint { get; set; }
        public string transactionNo { get; set; }
        [Obsolete("use TransactionSettings instead")]
        public bool isForceSwipePinEntry { get; set; }
        [Obsolete("use TransactionSettings instead")]
        public bool? disableRestartTransactionWhenFailed { get; set; }
        private static readonly string BUNDLE_KEY_ACTION = "a";
        [Obsolete("use TransactionSettings instead")]
        public bool? allowOfflinePayment { get; set; }
        [Obsolete("use TransactionSettings instead")]
        public bool? approveOfflinePaymentWithoutPrompt { get; set; }
        public bool? requiresRemoteConfirmation { get; set; }
        public TransactionSettings transactionSettings { get; set; }
        public bool allowPartialAuth { get; set; } = true;
    }

    public enum ResultStatus
    {
        SUCCESS,
        FAIL,
        CANCEL
    }

    public enum QueryStatus
    {
        UNKNOWN,
        FOUND,
        NOT_FOUND,
        IN_PROGRESS
    }

    public enum KeyPress
    {
        NONE = (byte)0x00,
        ENTER = ((byte)0x28),
        ESC = ((byte)0x29),
        BACKSPACE = ((byte)0x2a),
        TAB = ((byte)0x2b),
        STAR = ((byte)0x55),

        BUTTON_1 = ((byte)0x3a),
        BUTTON_2 = ((byte)0x3b),
        BUTTON_3 = ((byte)0x3c),
        BUTTON_4 = ((byte)0x3d),
        BUTTON_5 = ((byte)0x3e),
        BUTTON_6 = ((byte)0x3f),

        DIGIT_1 = ((byte)0x59),
        DIGIT_2 = ((byte)0x5a),
        DIGIT_3 = ((byte)0x5b),
        DIGIT_4 = ((byte)0x5c),
        DIGIT_5 = ((byte)0x5d),
        DIGIT_6 = ((byte)0x5e),
        DIGIT_7 = ((byte)0x5f),
        DIGIT_8 = ((byte)0x60),
        DIGIT_9 = ((byte)0x61),
        DIGIT_0 = ((byte)0x62)
    }

    public class InputOption
    {
        public InputOption() { }
        public InputOption(KeyPress kp, string desc)
        {
            keyPress = kp;
            description = desc;
        }
        public KeyPress keyPress;
        public string description;
    }

    public enum ChallengeType
    {
        DUPLICATE_CHALLENGE,
        OFFLINE_CHALLENGE
    }

    public enum MessageType
    {
        PHONE_NUMBER,
        RATINGS,
        REQUEST_RATINGS,
        CUSTOMER_INFO,
        CONVERSATION_QUESTION,
        CONVERSATION_RESPONSE,
        WEBVIEW
    }

    public class PayloadMessage
    {
        public PayloadMessage(string PayloadClassName, MessageType MessageType)
        {
            payloadClassName = PayloadClassName;
            messageType = MessageType;
        }

        public string payloadClassName;
        public MessageType messageType;
    }

    public class CustomerInfo
    {
        public string phoneNumber;
        public string customerName;
    }

    public class CustomerInfoMessage
    {
        public CustomerInfo customerInfo;
        public string payloadClassName;
        public string messageType;
    }

    public class PhoneNumberMessage
    {
        public string phoneNumber;
        public string payloadClassName;
        public MessageType messageType;
    }

    public class Rating
    {
        public string id;
        public string question;
        public int value;
    }

    public class RatingsMessage
    {
        public string payloadClassName;
        public string messageType;
        public Rating[] ratings;
    }

    public class RequestRatingsMessage
    {
        public string payloadClassName;
        public MessageType messageType;
    }

    public class WebViewMessage
    {
        public string payloadClassName;
        public string messageType;
        public string url;
        public string html;
    }


    public class ConversationQuestionMessage
    {
        public string payloadClassName;
        public string messageType;
        public string message;
    }

    public class ConversationResponseMessage
    {
        public string payloadClassName;
        public string messageType;
        public string message;
    }

    public class Challenge
    {
        public string message;
        public ChallengeType type;
        public VoidReason reason;
    }

    public class Signature2
    {
        public int width;
        public int height;
        public List<Signature2.Stroke> strokes;


        public class Stroke
        {
            public List<Signature2.Point> points;
        }

        public class Point
        {
            public int x;
            public int y;
        }
    }

    public enum ResponseReasonCode
    {
        NONE,
        ORDER_NOT_FOUND,
        PAYMENT_NOT_FOUND,
        FAIL
    }

    public enum TxState
    {
        START,
        SUCCESS,
        FAIL
    }

    public enum UiState
    {
        // payment flow
        START,
        FAILED,
        FATAL,
        TRY_AGAIN,
        INPUT_ERROR,
        PIN_BYPASS_CONFIRM,
        CANCELED,
        TIMED_OUT,
        DECLINED,
        VOIDED,
        CONFIGURING,
        PROCESSING,
        REMOVE_CARD,
        PROCESSING_GO_ONLINE,
        PROCESSING_CREDIT,
        PROCESSING_SWIPE,
        SELECT_APPLICATION,
        PIN_PAD,
        MANUAL_CARD_NUMBER,
        MANUAL_CARD_CVV,
        MANUAL_CARD_CVV_UNREADABLE,
        MANUAL_CARD_EXPIRATION,
        SELECT_ACCOUNT,
        CASHBACK_CONFIRM,
        CASHBACK_SELECT,
        CONTACTLESS_TAP_REQUIRED,
        VOICE_REFERRAL_RESULT,
        CONFIRM_PARTIAL_AUTH,
        PACKET_EXCEPTION,
        CONFIRM_DUPLICATE_CHECK,

        // verify CVM flow
        VERIFY_SIGNATURE_ON_PAPER,
        VERIFY_SIGNATURE_ON_PAPER_CONFIRM_VOID,
        VERIFY_SIGNATURE_ON_SCREEN,
        VERIFY_SIGNATURE_ON_SCREEN_CONFIRM_VOID,
        ADD_SIGNATURE,
        SIGNATURE_ON_SCREEN_FALLBACK,
        RETURN_TO_MERCHANT,
        SIGNATURE_REJECT,
        ADD_SIGNATURE_CANCEL_CONFIRM,

        // add tip flow
        ADD_TIP,

        // receipt options flow
        RECEIPT_OPTIONS,

        // tender handling flow
        HANDLE_TENDER,

        // custom activity, optionally called from custom activity
        CUSTOM_ACTIVITY,
        // starting custom activity, called from RTKA
        STARTING_CUSTOM_ACTIVITY
    }

    public enum UiDirection
    {
        ENTER,
        EXIT
    }

    public class PendingPaymentEntry
    {
        public string paymentId;
        public long amount;
    }

    public class CardData
    {
        public string Track1 { get; set; }
        public string Track2 { get; set; }
        public string Track3 { get; set; }
        public bool Encrypted { get; set; }
        public string MaskedTrack1 { get; set; }
        public string MaskedTrack2 { get; set; }
        public string MaskedTrack3 { get; set; }
        public string Pan { get; set; }
        public string CardholderName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Exp { get; set; }
        public string Last4 { get; set; }
        public string First6 { get; set; }
    }
}
