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
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using com.clover.remote.order;
using com.clover.remote.order.operation;
using com.clover.remotepay.transport;
using com.clover.sdk.v3;
using com.clover.sdk.v3.base_;
using com.clover.sdk.v3.order;
using com.clover.sdk.v3.payments;
using com.clover.sdk.v3.printer;
using TxType = com.clover.remotepay.transport.TxType;


namespace com.clover.remotepay.sdk
{
    /// <summary>
    /// The main CloverConnector API for connecting to Clover Payment Devices, created via a CloverConnectorFactory transport setup call
    /// </summary>
    public class CloverConnector : ICloverConnector
    {
        private const int ENABLE_KIOSK_ENTRY_METHODS = 1 << 15;
        public const int CARD_ENTRY_METHOD_MAG_STRIPE = 1 | (1 << 8) | ENABLE_KIOSK_ENTRY_METHODS; //257 + the flag to indicate the flags are passed in
        public const int CARD_ENTRY_METHOD_ICC_CONTACT = 2 | (2 << 8) | ENABLE_KIOSK_ENTRY_METHODS; //514 + the flag to indicate the flags are passed in
        public const int CARD_ENTRY_METHOD_NFC_CONTACTLESS = 4 | (4 << 8) | ENABLE_KIOSK_ENTRY_METHODS; //1028 + the flag to indicate the flags are passed in
        public const int CARD_ENTRY_METHOD_MANUAL = 8 | (8 << 8) | ENABLE_KIOSK_ENTRY_METHODS; // 2056 + the flag to indicate the flags are passed in

        // The default value of CardEntryMethod if unspecified
        private int CardEntryMethod => CARD_ENTRY_METHOD_MAG_STRIPE | CARD_ENTRY_METHOD_ICC_CONTACT | CARD_ENTRY_METHOD_NFC_CONTACTLESS;

        protected CloverDevice Device;
        protected CloverDeviceConfiguration Config;

        List<ICloverConnectorListener> listeners = new List<ICloverConnectorListener>();
        private MerchantInfo merchantInfo { get; set; }
        private InnerDeviceObserver deviceObserver { get; set; }

        private int logLevel = LogLevel.MINIMAL;


        /// <summary>
        /// A copy of the current CloverConnector MerchantInfo from the DeviceReady message
        /// </summary>
        public MerchantInfo MerchantInfo => merchantInfo?.Clone();

        /// <summary>
        /// The Configured Transport's Title, indicating USB, WebSocket, etc. in a display friendly format
        /// </summary>
        public string ConnectionTitle => Config?.getCloverTransport()?.Title ?? "";

        /// <summary>
        /// The Configured Transport's Summary, intended to be the web connection string or similar info as appropriate
        /// </summary>
        public string ConnectionSummary => Config?.getCloverTransport()?.Summary ?? "";

        /// <summary>
        /// SDK identification for reporting
        /// </summary>
        public SDKInfo SDKInfo { get; protected set; } = new SDKInfo();

        /// <summary>
        /// set to true, so that when request responses are processed, the Clover Mini
        /// won't show default messages/ThankYou/Welcome screens.  Set to false
        /// to allow the default message flow that is built into the CloverConnector.
        /// </summary>
        bool DisableDefaultDeviceScreenFlow { get; set; }

        /// <summary>
        /// Holds the current connection state
        /// </summary>
        public bool IsReady { get; private set; }

        //keep a last request
        private object lastRequest;

        public void AddCloverConnectorListener(ICloverConnectorListener connectorListener)
        {
            if (connectorListener != null && !listeners.Contains(connectorListener))
            {
                listeners.Add(connectorListener);
            }
        }

        public void RemoveCloverConnectorListener(ICloverConnectorListener connectorListener)
        {
            if (connectorListener != null && listeners.Contains(connectorListener))
            {
                listeners.Remove(connectorListener);
            }
        }

        public CloverConnector()
        {
        }

        /// <summary>
        /// CloverConnector constructor
        /// </summary>
        /// <param name="config">A CloverDeviceConfiguration object; TestDeviceConfiguration can be used for testing
        /// </param>
        public CloverConnector(CloverDeviceConfiguration config)
        {
            Config = config;
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(GetType());
            SDKInfo.Name = assembly.GetAssemblyAttribute<System.Reflection.AssemblyDescriptionAttribute>().Description;
            SDKInfo.Version = (assembly.GetAssemblyAttribute<System.Reflection.AssemblyFileVersionAttribute>()).Version
                + (assembly.GetAssemblyAttribute<System.Reflection.AssemblyInformationalVersionAttribute>()).InformationalVersion;
        }

        /// <summary>
        /// Initialize the connector with a given configuration
        /// </summary>
        public void InitializeConnection()
        {
            merchantInfo = new MerchantInfo();
            deviceObserver = new InnerDeviceObserver(this);

            DisableDefaultDeviceScreenFlow = false;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                try
                {
                    Device = CloverDeviceFactory.Get(Config);
                    Device.Subscribe(deviceObserver);
                    Device.Initialize(Config);
                }
                catch (Exception exception)
                {
                    OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, CloverDeviceErrorEvent.InvalidConfig, exception, exception.Message));
                }
            };
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// Sale method, aka "purchase"
        /// </summary>
        /// <param name="request">A SaleRequest object containing basic information needed for the transaction</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
        public void Sale(SaleRequest request)
        {
            lastRequest = request ?? new SaleRequest();

            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                deviceObserver.onFinishCancel(ResponseCode.ERROR,
                                              TxType.SALE,
                                              "Device Connection Error",
                                              "In Sale : SaleRequest - The Clover device is not connected.");
                return;
            }
            if (request == null)
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              TxType.SALE,
                                              "Request Validation Error",
                                              "In Sale : SaleRequest - The request that was passed in for processing is empty.");
                return;
            }
            if (String.IsNullOrEmpty(request.ExternalId))
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              TxType.SALE,
                                              "Request Validation Error",
                                              "In Sale : SaleRequest - The request ExternalId cannot be null or blank. Original Request = " + request);
                return;
            }
            if (request.Amount <= 0)
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              TxType.SALE,
                                              "Request Validation Error",
                                              "In Sale : SaleRequest - The request amount cannot be zero. Original Request = " + request);
                return;
            }
            if (request.TipAmount != null && request.TipAmount < 0)
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              TxType.SALE,
                                              "Request Validation Error",
                                              "In Sale : SaleRequest - The request tip amount cannot be less than zero. Original Request = " + request);
                return;
            }
            if (request.VaultedCard != null && !merchantInfo.supportsVaultCards)
            {
                deviceObserver.onFinishCancel(ResponseCode.UNSUPPORTED,
                                              TxType.SALE,
                                              "Merchant Configuration Validation Error",
                                              "In Sale : SaleRequest - Vault Card support is not offered by the merchant configured gateway. Original Request = " + request);
                return;
            }

            PayIntent payIntent = new PayIntent();
            payIntent.externalPaymentId = request.ExternalId;
            payIntent.externalReferenceId = request.ExternalReferenceId;
            payIntent.transactionType = PayIntent.TransactionType.PAYMENT;
            payIntent.amount = request.Amount;
            payIntent.tipAmount = request.TipAmount ?? 0;
            payIntent.taxAmount = request.TaxAmount;
            payIntent.vaultedCard = request.VaultedCard;
            payIntent.requiresRemoteConfirmation = true;

            if (request.Extras != null && request.Extras.Count > 0)
            {
                payIntent.passThroughValues = request.Extras;
            }

            TransactionSettings ts = getTransactionRequestOverrides(request);
            ts.tippableAmount = request.TippableAmount;
            if (request.CardNotPresent.HasValue && request.CardNotPresent.Value)
            {
                payIntent.isCardNotPresent = true;
            }
            if (request.DisableCashback.HasValue && request.DisableCashback.Value)
            {
                ts.disableCashBack = true;
            }
            if (request.ForceOfflinePayment.HasValue)
            {
                ts.forceOfflinePayment = request.ForceOfflinePayment;
            }
            if (request.AllowOfflinePayment.HasValue)
            {
                ts.allowOfflinePayment = request.AllowOfflinePayment;
            }
            if (request.ApproveOfflinePaymentWithoutPrompt.HasValue)
            {
                ts.approveOfflinePaymentWithoutPrompt = request.ApproveOfflinePaymentWithoutPrompt;
            }
            if (request.TipMode.HasValue)
            {
                ts.tipMode = request.TipMode.GetAltTipMode();
            }
            if (request.TippableAmount.HasValue)
            {
                ts.tippableAmount = request.TippableAmount.Value;
            }
            payIntent.transactionSettings = ts;
            Device.doTxStart(payIntent, null, TxType.SALE);
        }

        private TransactionSettings getTransactionRequestOverrides(TransactionRequest request)
        {
            TransactionSettings settings = getBaseTransactionRequestOverrides(request);

            if (request.SignatureThreshold.HasValue)
            {
                settings.signatureThreshold = request.SignatureThreshold.Value;
            }
            if (request.AutoAcceptSignature.HasValue && request.AutoAcceptSignature.Value)
            {
                settings.autoAcceptSignature = true;
            }
            if (request.SignatureEntryLocation.HasValue)
            {
                settings.signatureEntryLocation = request.SignatureEntryLocation.Value;
            }
            if (request.TipSuggestions != null && request.TipSuggestions.Count > 0)
            {
                settings.tipSuggestions = request.TipSuggestions;
            }

            return settings;
        }

        private TransactionSettings getBaseTransactionRequestOverrides(BaseTransactionRequest request)
        {
            TransactionSettings settings = new TransactionSettings();

            settings.cardEntryMethods = request.CardEntryMethods ?? CardEntryMethod;
            if (request.DisablePrinting.HasValue && request.DisablePrinting.Value)
            {
                settings.cloverShouldHandleReceipts = false;
            }
            if (request.DisableRestartTransactionOnFail.HasValue && request.DisableRestartTransactionOnFail.Value)
            {
                settings.disableRestartTransactionOnFailure = true;
            }
            if (request.DisableDuplicateChecking.HasValue && request.DisableDuplicateChecking.Value)
            {
                settings.disableDuplicateCheck = true;
            }
            if (request.DisableReceiptSelection.HasValue && request.DisableReceiptSelection.Value)
            {
                settings.disableReceiptSelection = true;
            }
            if (request.AutoAcceptPaymentConfirmations.HasValue && request.AutoAcceptPaymentConfirmations.Value)
            {
                settings.autoAcceptPaymentConfirmations = true;
            }
            if (request.RegionalExtras != null && request.RegionalExtras.Count > 0)
            {
                settings.regionalExtras = new Dictionary<string, string>(request.RegionalExtras);
            }

            return settings;
        }

        /// <summary>
        /// If signature is captured during a Sale, this method accepts the signature as entered
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void AcceptSignature(VerifySignatureRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In AcceptSignature : Device is not connected. "));
                return;
            }
            if (request == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In AcceptSignature : VerifySignatureRequest object cannot be null. "));
                return;
            }
            if (request.Payment == null || request.Payment.id == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In AcceptSignature : VerifySignatureRequest.Payment must have an ID. "));
                return;
            }

            Device.doVerifySignature(request.Payment, true);
        }

        /// <summary>
        /// If signature is captured during a Sale, this method rejects the signature as entered
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void RejectSignature(VerifySignatureRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In RejectSignature : Device is not connected. "));
                return;
            }
            if (request == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In RejectSignature : VerifySignatureRequest object cannot be null. "));
                return;
            }
            if (request.Payment == null || request.Payment.id == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In RejectSignature : VerifySignatureRequest.Payment must have an ID. "));
                return;
            }

            Device.doVerifySignature(request.Payment, false);
        }

        /// <summary>
        /// Auth method to obtain an Auth.  While a Pre-Auth can also be accomplished
        /// by setting the IsPreAuth flag to true, the PreAuthRequest is the
        /// preferred request type.  PreAuth functionality was retained for backward
        /// compatibility
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void Auth(AuthRequest request)
        {
            lastRequest = request ?? new AuthRequest();

            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                deviceObserver.onFinishCancel(ResponseCode.ERROR,
                                              TxType.AUTH,
                                              "Device Connection Error",
                                              "In Auth : AuthRequest - The Clover device is not connected.");
                return;
            }
            if (request == null)
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              TxType.AUTH,
                                              "Request Validation Error",
                                              "In Auth : AuthRequest - The request that was passed in for processing is empty.");
                return;
            }
            if (String.IsNullOrEmpty(request.ExternalId))
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              TxType.AUTH,
                                              "Request Validation Error",
                                              "In Auth : AuthRequest - The request ExternalId cannot be null or blank. Original Request = " + request);
                return;
            }
            if (request.Amount <= 0)
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              TxType.AUTH,
                                              "Request Validation Error",
                                              "In Auth : AuthRequest - The request amount must be greater than zero. Original Request = " + request);
                return;
            }
            if (!merchantInfo.supportsAuth)
            {
                deviceObserver.onFinishCancel(ResponseCode.UNSUPPORTED,
                                              TxType.AUTH,
                                              "Merchant Configuration Validation Error",
                                              "In Auth : AuthRequest - Auths are not enabled for the payment gateway. Original Request = " + request);
                return;
            }
            if (request.VaultedCard != null && !merchantInfo.supportsVaultCards)
            {
                deviceObserver.onFinishCancel(ResponseCode.UNSUPPORTED,
                                              TxType.AUTH,
                                              "Merchant Configuration Validation Error",
                                              "In Auth : AuthRequest - Vault Card support is not offered by the merchant configured gateway. Original Request = " + request);
                return;
            }

            PayIntent payIntent = new PayIntent();
            payIntent.externalPaymentId = request.ExternalId;
            payIntent.externalReferenceId = request.ExternalReferenceId;
            payIntent.transactionType = request.Type;
            payIntent.vaultedCard = request.VaultedCard;
            payIntent.amount = request.Amount;
            payIntent.tipAmount = null; // have to force this to null until PayIntent honors transactionType of AUTH
            payIntent.taxAmount = request.TaxAmount;
            if (request.CardNotPresent.HasValue && request.CardNotPresent.Value)
            {
                payIntent.isCardNotPresent = true;
            }

            if (request.Extras != null && request.Extras.Count > 0)
            {
                payIntent.passThroughValues = request.Extras;
            }

            TransactionSettings ts = getTransactionRequestOverrides(request);
            if (request.DisableCashback.HasValue && request.DisableCashback.Value)
            {
                ts.disableCashBack = true;
            }
            if (request.AllowOfflinePayment.HasValue)
            {
                ts.allowOfflinePayment = request.AllowOfflinePayment;
            }
            if (request.ApproveOfflinePaymentWithoutPrompt.HasValue && request.ApproveOfflinePaymentWithoutPrompt.Value)
            {
                ts.approveOfflinePaymentWithoutPrompt = true;
            }
            if (request.ForceOfflinePayment.HasValue && request.ForceOfflinePayment.Value)
            {
                ts.forceOfflinePayment = true;
            }
            ts.tipMode = com.clover.sdk.v3.payments.TipMode.ON_PAPER;
            payIntent.transactionSettings = ts;
            payIntent.requiresRemoteConfirmation = true;
            Device.doTxStart(payIntent, null, TxType.AUTH);
        }

        /// <summary>
        /// PreAuth method to obtain a PreAuth.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void PreAuth(PreAuthRequest request)
        {
            lastRequest = request ?? new PreAuthRequest();

            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                deviceObserver.onFinishCancel(ResponseCode.ERROR,
                                              TxType.PREAUTH,
                                              "Device Connection Error",
                                              "In PreAuth : PreAuthRequest - The Clover device is not connected.");
                return;
            }
            if (request == null)
            {
                deviceObserver.onFinishCancel(ResponseCode.ERROR,
                                              TxType.PREAUTH,
                                              "Request Validation Error",
                                              "In PreAuth : PreAuthRequest - The request that was passed in for processing is empty.");
                return;
            }
            if (String.IsNullOrEmpty(request.ExternalId))
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              TxType.PREAUTH,
                                              "Request Validation Error",
                                              "In PreAuth : PreAuthRequest - The request ExternalId cannot be null or blank. Original Request = " + request);
                return;
            }
            if (request.Amount <= 0)
            {
                deviceObserver.onFinishCancel(ResponseCode.ERROR,
                                              TxType.PREAUTH,
                                              "Request Validation Error",
                                              "In PreAuth : PreAuthRequest - The request amount cannot be zero. Original Request = " + request);
                return;
            }
            if (!merchantInfo.supportsPreAuths)
            {
                deviceObserver.onFinishCancel(ResponseCode.UNSUPPORTED,
                                              TxType.PREAUTH,
                                              "Merchant Configuration Validation Error",
                                              "In PreAuth : PreAuthRequest - PreAuths are not enabled for the payment gateway. Original Request = " + request);
                return;
            }
            if (request.VaultedCard != null && !merchantInfo.supportsVaultCards)
            {
                deviceObserver.onFinishCancel(ResponseCode.UNSUPPORTED,
                                              TxType.PREAUTH,
                                              "Merchant Configuration Validation Error",
                                              "In PreAuth : PreAuthRequest - Vault Card support is not offered by the merchant configured gateway. Original Request = " + request);
                return;
            }

            PayIntent payIntent = new PayIntent();
            payIntent.externalPaymentId = request.ExternalId;
            payIntent.externalReferenceId = request.ExternalReferenceId;
            payIntent.transactionType = PayIntent.TransactionType.AUTH;
            TransactionSettings ts = getBaseTransactionRequestOverrides(request);
            payIntent.transactionSettings = ts;
            ts.tipMode = clover.sdk.v3.payments.TipMode.NO_TIP;
            payIntent.vaultedCard = request.VaultedCard;
            payIntent.amount = request.Amount;
            payIntent.tipAmount = null; // have to force this to null until PayIntent honors transactionType of AUTH

            if (request.CardNotPresent.HasValue && request.CardNotPresent.Value)
            {
                payIntent.isCardNotPresent = true;
            }

            if (request.Extras != null && request.Extras.Count > 0)
            {
                payIntent.passThroughValues = new Dictionary<string, string>(request.Extras);
            }

            payIntent.requiresRemoteConfirmation = true;
            Device.doTxStart(payIntent, null, TxType.PREAUTH);
        }

        /// <summary>
        /// Capture a previous Auth. Note: Should only be called if request's PaymentID is from an AuthResponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void CapturePreAuth(CapturePreAuthRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                deviceObserver.onCapturePreAuthResponse(ResponseCode.ERROR,
                                                        "Device Connection Error",
                                                        "In CapturePreAuth : CapturePreAuthRequest - The Clover device is not connected.");
                return;
            }
            if (!merchantInfo.supportsPreAuths)
            {
                deviceObserver.onCapturePreAuthResponse(ResponseCode.UNSUPPORTED,
                                                        "Merchant Configuration Validation Error",
                                                        "In CapturePreAuth : CapturePreAuthRequest - Capture PreAuth is not supported by the merchant configured gateway. Original Request = " + request);
                return;
            }
            if (request == null)
            {
                deviceObserver.onCapturePreAuthResponse(ResponseCode.FAIL,
                                                        "Request Validation Error",
                                                        "In CapturePreAuth : CapturePreAuthRequest - The request that was passed in for processing is empty.");
                return;
            }
            if (request.Amount <= 0)
            {
                deviceObserver.onCapturePreAuthResponse(ResponseCode.FAIL,
                                                        "Request Validation Error",
                                                        "In CapturePreAuth : CapturePreAuthRequest - The Request amount cannot be zero. Original Request = " + request);
                return;
            }
            if (request.TipAmount < 0)
            {
                deviceObserver.onCapturePreAuthResponse(ResponseCode.FAIL,
                                                        "Request Validation Error",
                                                        "In CapturePreAuth : CapturePreAuthRequest - The Request TipAmount cannot be less than zero. Original Request = " + request);
                return;
            }

            Device.doCapturePreAuth(request.PaymentID, request.Amount, request.TipAmount);
        }

        /// <summary>
        /// Increment a previous Auth. Note: Should only be called if request's PaymentID is from an AuthResponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void IncrementPreAuth(IncrementPreAuthRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                deviceObserver.onIncrementPreAuthResponse(ResponseCode.ERROR,
                                                        "Device Connection Error",
                                                        "In IncrementPreAuth : IncrementPreAuthRequest - The Clover device is not connected.");
                return;
            }
            if (!merchantInfo.supportsPreAuths)
            {
                deviceObserver.onIncrementPreAuthResponse(ResponseCode.UNSUPPORTED,
                                                        "Merchant Configuration Validation Error",
                                                        "In IncrementPreAuth : IncrementPreAuthRequest - Capture PreAuth is not supported by the merchant configured gateway. Original Request = " + request);
                return;
            }
            if (request == null)
            {
                deviceObserver.onIncrementPreAuthResponse(ResponseCode.FAIL,
                                                        "Request Validation Error",
                                                        "In IncrementPreAuth : IncrementPreAuthRequest - The request that was passed in for processing is empty.");
                return;
            }
            if (request.Amount <= 0)
            {
                deviceObserver.onIncrementPreAuthResponse(ResponseCode.FAIL,
                                                        "Request Validation Error",
                                                        "In IncrementPreAuth : IncrementPreAuthRequest - The Request amount cannot be zero. Original Request = " + request);
                return;
            }

            Device.doIncrementPreAuth(request.PaymentID, request.Amount);
        }

        /// <summary>
        /// Adjust the tip for a previous Auth. Note: Should only be called if request's PaymentID is from an AuthResponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void TipAdjustAuth(TipAdjustAuthRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                deviceObserver.onAuthTipAdjusted(ResponseCode.ERROR,
                                                 "Device Connection Error",
                                                 "In TipAdjustAuth : TipAdjustAuthRequest - The Clover device is not connected.");
                return;
            }
            if (request == null)
            {
                deviceObserver.onAuthTipAdjusted(ResponseCode.FAIL,
                                                 "Request Validation Error",
                                                 "In TipAdjustAuth : TipAdjustAuthRequest - The request that was passed in for processing is empty.");
                return;
            }
            if (request.PaymentID == null)
            {
                deviceObserver.onAuthTipAdjusted(ResponseCode.FAIL,
                                                 "Request Validation Error",
                                                 "In TipAdjustAuth : TipAdjustAuthRequest PaymentID cannot be empty. " + request);
                return;
            }
            if (request.TipAmount < 0)
            {
                deviceObserver.onAuthTipAdjusted(ResponseCode.FAIL,
                                                        "Request Validation Error",
                                                        "In TipAdjustAuth : TipAdjustAuthRequest TipAmount cannot be less than zero. " + request);
                return;
            }
            if (!merchantInfo.supportsTipAdjust)
            {
                deviceObserver.onAuthTipAdjusted(ResponseCode.UNSUPPORTED,
                                                        "Merchant Configuration Validation Error",
                                                        "In TipAdjustAuth : TipAdjustAuthRequest - Tip Adjust is not supported by the merchant configured gateway. " + request);
                return;
            }

            Device.doTipAdjustAuth(request.OrderID, request.PaymentID, request.TipAmount, null);
        }

        /// <summary>
        /// Void a transaction, given a previously used order ID and/or payment ID
        /// TBD - defining a payment or order ID to be used with a void without requiring a response from Sale()
        /// </summary>
        /// <param name="request">A VoidRequest object containing basic information needed to void the transaction</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
        public void VoidPayment(VoidPaymentRequest request) // SaleResponse is a Transaction? or create a Transaction from a SaleResponse
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                deviceObserver.onPaymentVoided(ResponseCode.ERROR,
                                               "Device Connection Error",
                                               "In VoidPayment : VoidPaymentRequest - The Clover device is not connected.");
                return;
            }
            if (request == null)
            {
                deviceObserver.onPaymentVoided(ResponseCode.FAIL,
                                                 "Request Validation Error",
                                                 "In VoidPayment : VoidPaymentRequest - The request that was passed in for processing is empty.");
                return;
            }
            if (string.IsNullOrEmpty(request.PaymentId))
            {
                deviceObserver.onPaymentVoided(ResponseCode.FAIL,
                                                 "Request Validation Error",
                                                 "In VoidPayment : VoidPaymentRequest PaymentId cannot be empty. " + request);
                return;
            }
            if (string.IsNullOrEmpty(request.VoidReason))
            {
                deviceObserver.onPaymentVoided(ResponseCode.FAIL,
                                                 "Request Validation Error",
                                                 "In VoidPayment : VoidPaymentRequest VoidReason cannot be empty. " + request);
                return;
            }
            VoidReason reason;
            try
            {
                reason = (VoidReason)Enum.Parse(typeof(VoidReason), request.VoidReason, true);
            }
            catch
            {
                deviceObserver.onPaymentVoided(ResponseCode.FAIL, "Request Validation Error", "In VoidPayment : VoidPaymentRequest VoidReason invalid value." + request);
                return;
            }

            Payment payment = new Payment
            {
                id = request.PaymentId,
                order = new Reference(request.OrderId),
                employee = new Reference(request.EmployeeId)
            };
            Device.doVoidPayment(payment, reason, request.Extras);
        }

        public void VoidPaymentRefund(VoidPaymentRefundRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                deviceObserver.onPaymentRefundVoided(ResponseCode.ERROR,
                    "Device Connection Error",
                    "In VoidPaymentRefund : VoidPaymentRefundRequest - The Clover device is not connected.");
                return;
            }

            deviceObserver.onPaymentRefundVoided(ResponseCode.ERROR,
                    "Feature disabled in current version",
                    "In VoidPaymentRefund : VoidPaymentRefundRequest - This feature is not enabled in the Clover Connector.");

            // Device.doVoidPaymentRefund(request.OrderId, request.RefundId, request.DisablePrinting, request.DisableReceiptSelection, request.EmployeeId, request.Extras);
        }

        /// <summary>
        /// Refund a specific payment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void RefundPayment(RefundPaymentRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                RefundPaymentResponse prr = new RefundPaymentResponse();
                prr.Refund = null;
                prr.Success = false;
                prr.Result = ResponseCode.ERROR;
                prr.Reason = "Device Connection Error";
                prr.Message = "In RefundPayment : RefundPaymentRequest - The Clover device is not connected.";
                deviceObserver.lastPRR = prr;
                deviceObserver.onFinishCancel(prr.Result, TxType.REFUND, prr.Reason, prr.Message);
                return;
            }
            if (request == null)
            {
                RefundPaymentResponse prr = new RefundPaymentResponse();
                prr.Refund = null;
                prr.Success = false;
                prr.Result = ResponseCode.FAIL;
                prr.Reason = "Request Validation Error";
                prr.Message = "In RefundPayment : RefundPaymentRequest - The request that was passed in for processing is empty.";
                deviceObserver.lastPRR = prr;
                deviceObserver.onFinishCancel(prr.Result, TxType.REFUND, prr.Reason, prr.Message);
                return;
            }
            if (request.PaymentId == null)
            {
                RefundPaymentResponse prr = new RefundPaymentResponse();
                prr.Refund = null;
                prr.Success = false;
                prr.Result = ResponseCode.FAIL;
                prr.Reason = "Request Validation Error";
                prr.Message = "In RefundPayment : RefundPaymentRequest PaymentID cannot be empty. " + request;
                deviceObserver.lastPRR = prr;
                deviceObserver.onFinishCancel(prr.Result, TxType.REFUND, prr.Reason, prr.Message);
                return;
            }
            if (request.Amount <= 0 && request.FullRefund == false)
            {
                RefundPaymentResponse prr = new RefundPaymentResponse();
                prr.Refund = null;
                prr.Success = false;
                prr.Result = ResponseCode.FAIL;
                prr.Reason = "Request Validation Error";
                prr.Message = "In RefundPayment : RefundPaymentRequest Amount must be greater than zero when FullRefund is set to false. " + request;
                deviceObserver.lastPRR = prr;
                deviceObserver.onFinishCancel(prr.Result, TxType.REFUND, prr.Reason, prr.Message);
                return;
            }

            // set up a cached error object for FINISH_CANCEL, will be ignored and cleared on FINISH_OK
            RefundPaymentResponse rpr = new RefundPaymentResponse();
            rpr.Success = false;
            rpr.Result = ResponseCode.FAIL;
            rpr.Reason = "Unknown Error";
            rpr.Message = "Device returned an unspecified error";
            deviceObserver.lastPRR = rpr;

            Device.doRefundPayment(request.OrderId, request.PaymentId, !request.FullRefund || request.Amount > 0 ? request.Amount : (long?)null, request.FullRefund, request.DisablePrinting, request.DisableReceiptSelection, request.Extras);
        }

        /// <summary>
        /// Manual refund method, aka "naked credit"
        /// </summary>
        /// <param name="request">A ManualRefundRequest object</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
        public void ManualRefund(ManualRefundRequest request) // NakedRefund is a Transaction, with just negative amount
        {
            lastRequest = request ?? new ManualRefundRequest();

            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                deviceObserver.onFinishCancel(ResponseCode.ERROR,
                                              TxType.MANUAL_REFUND,
                                              "Device Connection Error",
                                              "In ManualRefund : ManualRefundRequest - The Clover device is not connected.");
                return;
            }
            if (request == null)
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              TxType.MANUAL_REFUND,
                                              "Request Validation Error",
                                              "In ManualRefund : ManualRefundRequest - The request that was passed in for processing is empty.");
                return;
            }
            if (String.IsNullOrEmpty(request.ExternalId))
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              TxType.MANUAL_REFUND,
                                              "Request Validation Error",
                                              "In ManualRefund : ManualRefundRequest - The request ExternalId cannot be null or blank. Original Request = " + request);
                return;
            }
            if (request.Amount <= 0)
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              TxType.MANUAL_REFUND,
                                              "Request Validation Error",
                                              "In ManualRefund : ManualRefundRequest Amount must be greater than zero. " + request);
                return;
            }
            if (!merchantInfo.supportsManualRefunds)
            {
                deviceObserver.onFinishCancel(ResponseCode.UNSUPPORTED,
                                              TxType.MANUAL_REFUND,
                                              "Merchant Configuration Error",
                                              "In ManualRefund: ManualRefundRequest - Manual refunds are not supported by the merchant configured gateway." + request);
                return;
            }
            if (request.VaultedCard != null && !merchantInfo.supportsVaultCards)
            {
                deviceObserver.onFinishCancel(ResponseCode.UNSUPPORTED,
                                              TxType.MANUAL_REFUND,
                                              "Merchant Configuration Validation Error",
                                              "In ManualRefund : RefundRequest - Vault Card support is not offered by the merchant configured gateway. Original Request = " + request);
                return;
            }

            PayIntent payIntent = new PayIntent();
            payIntent.amount = -Math.Abs(request.Amount);
            payIntent.transactionType = PayIntent.TransactionType.CREDIT;
            payIntent.externalPaymentId = request.ExternalId;
            payIntent.externalReferenceId = request.ExternalReferenceId;
            payIntent.vaultedCard = request.VaultedCard;
            payIntent.requiresRemoteConfirmation = true;

            if (request.Extras != null && request.Extras.Count > 0)
            {
                payIntent.passThroughValues = request.Extras;
            }

            TransactionSettings ts = getBaseTransactionRequestOverrides(request);
            ts.tipMode = clover.sdk.v3.payments.TipMode.NO_TIP;
            ts.cardEntryMethods = request.CardEntryMethods ?? CardEntryMethod;
            payIntent.transactionSettings = ts;
            Device.doTxStart(payIntent, null, TxType.MANUAL_REFUND);
        }

        /// <summary>
        /// Send a request to the server to closeout all orders.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void Closeout(CloseoutRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In Closeout: CloseoutRequest - The Clover device is not connected."));
                return;
            }
            if (request == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In Closeout : CloseoutRequest object cannot be null. "));
                return;
            }

            Device.doCloseout(request.AllowOpenTabs, request.BatchId); // TODO: pass in request UUID so it can be returned with CloseoutResponse
        }

        /// <summary>
        /// Send a request to the mini to reset.  This can be used if the device gets into a non-recoverable state.
        /// </summary>
        /// <returns></returns>
        public void ResetDevice()
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In ResetDevice: The Clover device is not connected."));
                return;
            }

            Device.doResetDevice();
        }

        /// <summary>
        /// Show a message on the Clover Mini screen
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public void ShowMessage(string message)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In ShowMessage: The Clover device is not connected."));
                return;
            }

            ShowOnDevice(message);
        }

        public void SendDebugLog(string message)
        {
            if (deviceObserver == null && merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In ShowMessage: The Clover device is not connected."));
                return;
            }
            if (message == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In SendDebugLog: Invalid argument 'message'. Null is not allowed."));
                return;
            }

            Device.doSendDebugLog(message);
        }

        /// <summary>
        /// Return the device to the Welcome Screen
        /// </summary>
        public void ShowWelcomeScreen()
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In ShowWelcomeScreen: The Clover device is not connected."));
                return;
            }

            ShowOnDevice(showWelcomeScreen: true);
        }

        /// <summary>
        /// Show the thank you screen on the device
        /// </summary>
        public void ShowThankYouScreen()
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In ShowThankYouScreen: The Clover device is not connected."));
                return;
            }

            ShowOnDevice(showThankYouScreen: true);
        }

        /// <summary>
        /// This method provides a way to control the default screen flow on the device, for instances
        /// where it makes sense to display some combination of message/thankyou/welcome screens with
        /// configurable timing between them
        /// </summary>
        private void ShowOnDevice(string message = null, int milliSecondsTimeoutForMsg = 0, bool showThankYouScreen = false, int milliSecondsTimeoutForThankYou = 0, bool showWelcomeScreen = false)
        {
            if (Device != null)
            {
                if (!DisableDefaultDeviceScreenFlow)
                {
                    bool hasData = message != null || showThankYouScreen || showWelcomeScreen;
                    if (hasData)
                    {
                        BackgroundWorker bgWorker = new BackgroundWorker();
                        bgWorker.DoWork += delegate
                        {
                            if (message != null)
                            {
                                Device.doTerminalMessage(message);
                                if (milliSecondsTimeoutForMsg > 0)
                                {
                                    Thread.Sleep(milliSecondsTimeoutForMsg);
                                }
                            }
                            if (showThankYouScreen)
                            {
                                Device.doShowThankYouScreen();
                                if (milliSecondsTimeoutForThankYou > 0)
                                {
                                    Thread.Sleep(milliSecondsTimeoutForThankYou);
                                }
                            }
                            if (showWelcomeScreen)
                            {
                                Device.doShowWelcomeScreen();
                            }
                        };
                        bgWorker.RunWorkerAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Vault Card information and payment token
        /// </summary>
        public void VaultCard(int? CardEntryMethods)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In VaultCard: The Clover device is not connected."));
                return;
            }

            if (!merchantInfo.supportsVaultCards)
            {
                deviceObserver.onVaultCardResponse(ResponseCode.UNSUPPORTED,
                                                  "Merchant Configuration Error",
                                                  "In VaultCard: - Vault card is not supported by the merchant configured gateway.");
                return;
            }

            Device.doVaultCard(CardEntryMethods ?? CardEntryMethod);
        }

        /// <summary>
        /// Retrieve Card Data
        /// </summary>
        public void ReadCardData(ReadCardDataRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                deviceObserver.onReadCardDataResponse(ResponseCode.ERROR,
                                              "Device Connection Error",
                                              "In ReadCardData : ReadCardDataRequest - The Clover device is not connected.");
                return;
            }
            if (request == null)
            {
                deviceObserver.onReadCardDataResponse(ResponseCode.FAIL,
                                              "Request Validation Error",
                                              "In ReadCardData : ReadCardDataRequest - The request that was passed in for processing is empty.");
                return;
            }
            if (request.CardEntryMethods.HasValue && request.CardEntryMethods.Value == 0)
            {
                deviceObserver.onReadCardDataResponse(ResponseCode.FAIL,
                                              "Request Validation Error",
                                              "In ReadCardData : ReadCardDataRequest - The CardEntryMethods field cannot be zero.");
                return;
            }

            PayIntent payIntent = new PayIntent();
            payIntent.transactionType = PayIntent.TransactionType.DATA;
            TransactionSettings ts = new TransactionSettings();
            ts.forcePinEntryOnSwipe = request.IsForceSwipePinEntry ?? false;
            ts.cardEntryMethods = request.CardEntryMethods ?? CardEntryMethod;
            payIntent.transactionSettings = ts;
            Device.doReadCardData(payIntent);
        }

        /// <summary>
        /// Show the customer facing receipt option screen for the specified Payment.
        /// </summary>
        public void DisplayPaymentReceiptOptions(DisplayPaymentReceiptOptionsRequest request)
        {
            if (deviceObserver == null && merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In DisplayPaymentReceiptOptions: The Clover device is not connected."));
                return;
            }
            if (request == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In DisplayPaymentReceiptOptions: The request cannot be null."));
                return;
            }
            if (request.OrderID == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In DisplayPaymentReceiptOptions: The orderId cannot be null."));
                return;
            }
            if (request.PaymentID == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In DisplayPaymentReceiptOptions: The paymentId cannot be null."));
                return;
            }

            Device.doShowPaymentReceiptScreen(request.OrderID, request.PaymentID, request.DisablePrinting);
        }

        /// <summary>
        /// Show the DisplayOrder on the device. Replaces the existing DisplayOrder on the device.
        /// </summary>
        /// <param name="order"></param>
        public void ShowDisplayOrder(DisplayOrder order)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In ShowDisplayOrder: The Clover device is not connected."));
                return;
            }
            if (order == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In ShowDisplayOrder : DisplayOrder object cannot be null."));
                return;
            }

            Device.doOrderUpdate(order, null);
        }

        /// <summary>
        /// Remove the DisplayOrder from the device.
        /// </summary>
        /// <param name="order"></param>
        public void RemoveDisplayOrder(DisplayOrder order)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In RemoveDisplayOrder: The Clover device is not connected."));
                return;
            }
            if (order == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In RemoveDisplayOrder: DisplayOrder object cannot be null."));
                return;
            }

            OrderDeletedOperation dao = new OrderDeletedOperation();
            dao.id = order.id;
            Device.doOrderUpdate(order, dao);
            ShowWelcomeScreen();
        }

        /// <summary>
        /// Request a list of pending payments from the device.
        /// Pending payments are payments taken offline that have
        /// not yet been sent to the server
        /// </summary>
        public void RetrievePendingPayments()
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                deviceObserver.onRetrievePendingPaymentsResponse(ResponseCode.ERROR,
                                              "Device Connection Error",
                                              "In RetrievePendingPayments : RetrievePendingPaymentsRequest - The Clover device is not connected.");
                return;
            }

            Device.doRetrievePendingPayments();
        }

        // TODO: should we call through, repurpose or remove?
        public void Dispose()
        {
            if (Device != null)
            {
                Device.Dispose();
            }
        }

        /// <summary>
        /// Invoke the InputOption on the device
        /// </summary>
        /// <param name="io"></param>
        public void InvokeInputOption(InputOption io)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In InvokeInputOption: The Clover device is not connected."));
                return;
            }
            if (io == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In InvokeInputOption: The InputOption object cannot be null."));
                return;
            }

            Device.doKeyPress(io.keyPress);
        }

        public T GetEnumFromString<T>(string stringValue, bool isCaseInsensitive = false) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.EXCEPTION, 0, null, "ArgumentException: T must be an enumerated type"));
                return default(T);
            }
            return (T)Enum.Parse(typeof(T), stringValue, isCaseInsensitive);
        }

        public void OnDeviceError(CloverDeviceErrorEvent ee)
        {
            deviceObserver.NotifyListeners(listener => listener.OnDeviceError(ee));
        }

        public void AcceptPayment(Payment payment)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In AcceptPayment: The Clover device is not connected."));
                return;
            }
            if (payment == null || payment.id == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In AcceptPayment: The Payment ID cannot be null."));
                return;
            }

            Device.doAcceptPayment(payment);
        }

        public void RejectPayment(Payment payment, Challenge challenge)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In RejectPayment: The Clover device is not connected."));
                return;
            }
            if (payment == null || payment.id == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In RejectPayment: The Payment ID cannot be null."));
                return;
            }
            if (challenge == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In RejectPayment: The Challenge cannot be null."));
                return;
            }

            Device.doRejectPayment(payment, challenge);
        }

        public void StartCustomActivity(CustomActivityRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In StartCustomActivity: The Clover device is not connected."));
                return;
            }
            if (request == null || request.Action == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In StartCustomActivity: The Action cannot be null."));
                return;
            }

            Device.doStartCustomActivity(request.Action, request.Payload, request.NonBlocking);
        }

        public void SendMessageToActivity(MessageToActivity request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In SendMessageToActivity: The Clover device is not connected."));
                return;
            }
            if (request == null || request.Action == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In SendMessageToActivity: The Action is required."));
                return;
            }

            Device.doSendMessageToActivity(request.Action, request.Payload);
        }

        public void RetrieveDeviceStatus(RetrieveDeviceStatusRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In RetrieveDeviceStatus: The Clover device is not connected."));
                return;
            }
            // null is a valid value for request

            Device.doRetrieveDeviceStatus(request?.sendLastMessage ?? false);
        }

        public void RetrievePayment(RetrievePaymentRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In RetrievePayment: The Clover device is not connected."));
                return;
            }
            if (request == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In RetrievePayment : RetrievePaymentRequest object cannot be null. "));
                return;
            }

            Device.doRetrievePayment(request.externalPaymentId);
        }

        public void OpenCashDrawer(OpenCashDrawerRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In OpenCashDrawer: The Clover device is not connected."));
                return;
            }
            if (request == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In OpenCashDrawer : OpenCashDrawerRequest object cannot be null. "));
                return;
            }

            Device.doOpenCashDrawer(request.reason, request.printerId);
        }

        public void Print(PrintRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In Print: The Clover device is not connected."));
                return;
            }
            if (request == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In Print : PrintRequest object cannot be null. "));
                return;
            }
            if (request.text == null || request.images == null || request.imageURLs == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In Print : PrintRequest text and image lists cannot be null. "));
                return;
            }
            if (request.images.Count == 0 && request.imageURLs.Count == 0 && request.text.Count == 0)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In Print : PrintRequest text and image lists cannot all be empty. "));
                return;
            }
            if (((request.images.Count > 0 ? 1 : 0) + (request.imageURLs.Count > 0 ? 1 : 0) + (request.text.Count > 0 ? 1 : 0)) > 1)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In Print : PrintRequest multiple print lists specified (text, image, image url), only one would be printed, send one list at a time"));
                return;
            }
            if (request.images.Count > 1 || request.imageURLs.Count > 1)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In Print : PrintRequest multiple images specified in image list, only one image per print call is currently supported."));
                return;
            }

            if (request.text.Count > 0)
            {
                Device.doPrintText(request.text, request.printRequestId, request.printDeviceId);
            }
            else if (request.images.Count > 0)
            {
                Device.doPrintImage(request.images[0], request.printRequestId, request.printDeviceId);
            }
            else if (request.imageURLs.Count > 0)
            {
                Device.doPrintImageURL(request.imageURLs[0], request.printRequestId, request.printDeviceId);
            }
        }

        public void RetrievePrinters(RetrievePrintersRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In RetrievePrinters: The Clover device is not connected."));
                return;
            }
            if (request == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In RetrievePrinters : RetrievePrintersRequest object cannot be null. "));
                return;
            }

            Device.doRetrievePrinters(request);
        }

        public void RetrievePrintJobStatus(PrintJobStatusRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In RetrievePrintJobStatus: The Clover device is not connected."));
                return;
            }
            if (request == null || request.printRequestId == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In RetrievePrintJobStatus : PrintJobStatusRequest.printRequestId object cannot be null. "));
                return;
            }

            Device.doRetrievePrintJobStatus(request.printRequestId);
        }

        /// <summary>
        /// Display receipt options for a Credit, Refund, or Payment
        /// </summary>
        /// <param name="request">The DisplayReceiptOptionsRequest details</param>
        public void DisplayReceiptOptions(DisplayReceiptOptionsRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In DisplayReceiptOptions: The Clover device is not connected."));
                return;
            }
            if (request == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In DisplayReceiptOptions : request parameter cannot be null. "));
                return;
            }
            if (string.IsNullOrEmpty(request.orderId) && string.IsNullOrEmpty(request.paymentId) && string.IsNullOrEmpty(request.refundId) && string.IsNullOrEmpty(request.creditId))
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In DisplayReceiptOptions : all the ids cannot be empty, specify one to display"));
                return;
            }

            Device.doShowReceiptScreen(request.orderId, request.paymentId, request.refundId, request.creditId, request.disablePrinting);
        }

        /// <summary>
        /// Register to receive customer data with the Clover Loyalty API
        /// </summary>
        /// <param name="request"></param>
        public void RegisterForCustomerProvidedData(RegisterForCustomerProvidedDataRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In RegisterForCustomerProvidedData: The Clover device is not connected."));
                return;
            }
            if (request == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "In RegisterForCustomerProvidedData : request parameter cannot be null. "));
                return;
            }

            Device.doRegisterForCustomerProvidedData(request.Configurations.AsLoyaltyDataConfig());
        }

        /// <summary>
        /// Set the Loyalty API's current customer info
        /// </summary>
        /// <param name="request"></param>
        public void SetCustomerInfo(SetCustomerInfoRequest request)
        {
            if (deviceObserver == null || merchantInfo == null)
            {
                throw new Exception("InitializeConnection() has not been called on Clover Connector");
            }
            if (Device == null || !IsReady)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "In SetCustomerInfo: The Clover device is not connected."));
                return;
            }

            // Send new customer, or null info if request is null or empty
            CustomerInfo info = null;
            if (request != null && (request.customer != null || request.displayString != null || request.externalId != null || request.externalSystemName != null || (request.extras != null && request.extras.Count > 0)))
            {
                info = new CustomerInfo
                {
                    customer = request.customer,
                    displayString = request.displayString,
                    externalId = request.externalId,
                    externalSystemName = request.externalSystemName,
                    extras = request.extras?.Count > 0 ? request.extras : null
                };
            }
            Device.doSetCustomerInfo(info);
        }

        public void SetLogLevel(int level)
        {
            logLevel = level;
            Device.SetLogLevel(level);
        }

        private class InnerDeviceObserver : ICloverDeviceObserver
        {
            public RefundPaymentResponse lastPRR;

            class SVR : VerifySignatureRequest
            {
                CloverDevice _Device;

                public SVR(CloverDevice device)
                {
                    _Device = device;
                }

                public override void Accept()
                {
                    _Device.doVerifySignature(Payment, true);
                }

                public override void Reject()
                {
                    _Device.doVerifySignature(Payment, false);
                }
            }

            CloverConnector cloverConnector { get; set; }

            public InnerDeviceObserver(CloverConnector cc)
            {
                cloverConnector = cc;

            }

            public void onDeviceConnected()
            {
                cloverConnector.IsReady = false;

                NotifyListeners(listener => listener.OnDeviceConnected());
            }

            public void onDeviceDisconnected()
            {
                cloverConnector.IsReady = false;

                NotifyListeners(listener => listener.OnDeviceDisconnected());
            }

            public void onDeviceReady(CloverDevice device, DiscoveryResponseMessage drm)
            {
                cloverConnector.merchantInfo = new MerchantInfo(drm);
                cloverConnector.IsReady = drm.ready;
                device.SupportsAcks = drm.supportsAcknowledgement;

                if (drm.ready)
                {
                    NotifyListeners(listener => listener.OnDeviceReady(cloverConnector.merchantInfo));
                }
                else
                {
                    NotifyListeners(listener => listener.OnDeviceConnected());
                }
            }

            public void onDeviceError(int code, Exception cause, string message)
            {
                NotifyListeners(listener => listener.OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, code, cause, message)));
            }

            public void onTxState(TxState txState)
            {
                //Console.WriteLine("onTxTstate: " + txState.ToString());
            }

            public void onTipAdded(long tip)
            {
                NotifyListeners(listener => listener.OnTipAdded(new TipAddedMessage(tip)));
            }

            public void onPartialAuth(long amount)
            {
                // not implemented yet
            }

            public void onAuthTipAdjusted(string paymentId, long tipAmount, bool success)
            {
                TipAdjustAuthResponse taar = new TipAdjustAuthResponse();
                taar.PaymentId = paymentId;
                taar.TipAmount = tipAmount;
                taar.Success = success;
                if (success)
                {
                    taar.Result = ResponseCode.SUCCESS;
                }
                else
                {
                    taar.Result = ResponseCode.FAIL;
                    taar.Reason = "Failure";
                    taar.Message = "TipAdjustAuth failed to process for payment ID: " + paymentId;
                }

                NotifyListeners(listener => listener.OnTipAdjustAuthResponse(taar));
            }

            public void onAuthTipAdjusted(ResponseCode responseCode, String reason = null, String message = null)
            {
                TipAdjustAuthResponse taar = new TipAdjustAuthResponse();
                taar.PaymentId = null;
                taar.TipAmount = 0;
                taar.Success = responseCode == ResponseCode.SUCCESS;
                taar.Result = responseCode;
                taar.Reason = reason;
                taar.Message = message;

                NotifyListeners(listener => listener.OnTipAdjustAuthResponse(taar));
            }

            public void onCashbackSelected(long cashbackAmount)
            {
                //TODO: Implement
                cloverConnector.ShowOnDevice(message: "CloverConnector.onCashbackSelected() is not yet implemented!!!!");
            }

            public void onKeyPressed(KeyPress keyPress)
            {
                //TODO: Implement
                cloverConnector.ShowOnDevice(message: "CloverConnector.onKeyPressed() is not yet implemented!!!!");
            }

            public void onRefundPaymentResponse(Refund refund, String orderId, String paymentId, TxState code, string msg, ResponseReasonCode reason)
            {
                RefundPaymentResponse prr = new RefundPaymentResponse();
                prr.Refund = refund;
                prr.OrderId = orderId;
                prr.PaymentId = paymentId;
                prr.Success = code == TxState.SUCCESS;
                if (code == TxState.SUCCESS)
                {
                    prr.Result = ResponseCode.SUCCESS;
                }
                else
                {
                    prr.Result = ResponseCode.FAIL;
                }
                prr.Message = msg;
                prr.Reason = reason.ToString();
                lastPRR = prr;
                // Don't send the response to the listeners just yet.  We need to wait
                // until the FinishOK or FinishCancel are received to notify the listeners,
                // so that logic is there instead.  Also, wait to send any instructions to
                // the device.
            }

            public void onCloseoutResponse(ResultStatus status, string reason, Batch batch)
            {
                CloseoutResponse cr = new CloseoutResponse();
                cr.Success = status == ResultStatus.SUCCESS;
                if (cr.Success)
                {
                    cr.Result = ResponseCode.SUCCESS;
                }
                else
                {
                    if (status == ResultStatus.FAIL)
                    {
                        cr.Result = ResponseCode.FAIL;
                    }
                }

                cr.Reason = reason;
                cr.Batch = batch;

                NotifyListeners(listener => listener.OnCloseoutResponse(cr));
            }

            public void onUiState(UiState uiState, String uiText, UiDirection uiDirection, params InputOption[] inputOptions)
            {
                CloverDeviceEvent.DeviceEventState state;
                try
                {
                    state = (CloverDeviceEvent.DeviceEventState)Enum.Parse(typeof(CloverDeviceEvent.DeviceEventState), uiState.ToString());
                }
                catch
                {
                    // if we can't map UiState to DeviceEventState, drop the message on the floor
                    return;
                }

                CloverDeviceEvent deviceEvent = new CloverDeviceEvent
                {
                    InputOptions = inputOptions,
                    EventState = state,
                    Message = uiText
                };
                if (uiDirection == UiDirection.ENTER)
                {
                    NotifyListeners(listener => listener.OnDeviceActivityStart(deviceEvent));
                }
                else if (uiDirection == UiDirection.EXIT)
                {
                    NotifyListeners(listener => listener.OnDeviceActivityEnd(deviceEvent));

                    if (uiState.ToString().Equals(CloverDeviceEvent.DeviceEventState.RECEIPT_OPTIONS.ToString()))
                    {
                        cloverConnector.ShowOnDevice(showWelcomeScreen: true);
                    }
                }
            }

            public void onFinishOk(Payment payment, Signature2 signature2, TxType requestInfo)
            {

                // depricated lastRequest support: remove when feature no longer necessary. NB: Refund/LastPRR has some additional complexity to it.
                if (requestInfo == TxType.NONE)
                {
                    if (cloverConnector.lastRequest is PreAuthRequest)
                    {
                        requestInfo = TxType.PREAUTH;
                    }
                    else if (cloverConnector.lastRequest is AuthRequest)
                    {
                        requestInfo = TxType.AUTH;
                    }
                    else if (cloverConnector.lastRequest is SaleRequest)
                    {
                        requestInfo = TxType.SALE;
                    }
                    else if (cloverConnector.lastRequest != null)
                    {
                        // "Invalid" value to cover fallback "unpaired response" message when lastRequest isn't one of the valid OnFinishOk(payment) values
                        requestInfo = (TxType)1000;
                    }
                }
                cloverConnector.lastRequest = null;

                if (requestInfo == TxType.PREAUTH)
                {
                    cloverConnector.ShowOnDevice(showThankYouScreen: true,
                                                 milliSecondsTimeoutForThankYou: 3000,
                                                 showWelcomeScreen: true);
                    PreAuthResponse response = new PreAuthResponse();
                    response.Success = true;
                    response.Result = ResponseCode.SUCCESS;
                    response.Payment = payment;
                    response.Signature = signature2;

                    NotifyListeners(listener => listener.OnPreAuthResponse(response));
                }
                else if (requestInfo == TxType.AUTH)
                {
                    cloverConnector.ShowOnDevice(showThankYouScreen: true,
                                                 milliSecondsTimeoutForThankYou: 3000,
                                                 showWelcomeScreen: true);
                    AuthResponse response = new AuthResponse();
                    response.Success = true;
                    response.Result = ResponseCode.SUCCESS;
                    response.Payment = payment;
                    response.Signature = signature2;

                    NotifyListeners(listener => listener.OnAuthResponse(response));
                }
                else if (requestInfo == TxType.SALE)
                {
                    cloverConnector.ShowOnDevice(showThankYouScreen: true,
                                                 milliSecondsTimeoutForThankYou: 3000,
                                                 showWelcomeScreen: true);
                    SaleResponse response = new SaleResponse();
                    response.Success = true;
                    response.Result = ResponseCode.SUCCESS;
                    response.Payment = payment;
                    response.Signature = signature2;

                    NotifyListeners(listener => listener.OnSaleResponse(response));
                }
                else if (requestInfo == TxType.NONE)
                {
                    cloverConnector.ShowOnDevice(showWelcomeScreen: true);
                }
                else
                {
                    // The lastRequest isn't one of the valid OnFinishOk(Payment...) messages, so we're out of sync and can't pair request to response
                    cloverConnector.ShowOnDevice(showThankYouScreen: true,
                                                 milliSecondsTimeoutForThankYou: 3000,
                                                 showWelcomeScreen: true);
                    cloverConnector.OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "Failed to pair this response. " + payment));
                }
            }

            public void onFinishOk(Credit credit)
            {
                cloverConnector.ShowOnDevice(showThankYouScreen: true,
                                             milliSecondsTimeoutForThankYou: 3000,
                                             showWelcomeScreen: true);
                ManualRefundResponse response = new ManualRefundResponse();
                response.Success = true;
                response.Result = ResponseCode.SUCCESS;
                response.Credit = credit;

                NotifyListeners(listener => listener.OnManualRefundResponse(response));
                cloverConnector.lastRequest = null;
            }

            public void onFinishOk(Refund refund)
            {
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);

                cloverConnector.lastRequest = null;
                RefundPaymentResponse lastRefundResponse = lastPRR;
                lastPRR = null;

                if (refund.orderRef != null)
                {
                    RefundPaymentResponse response = new RefundPaymentResponse();
                    response.OrderId = refund.orderRef.id;
                    response.PaymentId = refund.payment.id;
                    response.Refund = refund;
                    response.Result = ResponseCode.SUCCESS;
                    response.Reason = ResponseReasonCode.NONE.ToString();
                    response.Success = true;

                    NotifyListeners(listener => listener.OnRefundPaymentResponse(response));
                }
                else
                {
                    if (lastRefundResponse != null && lastRefundResponse.Refund.id == refund.id)
                    {
                        NotifyListeners(listener => listener.OnRefundPaymentResponse(lastRefundResponse));
                    }
                    else
                    {
                        RefundPaymentResponse response = new RefundPaymentResponse();
                        response.PaymentId = refund.payment.id;
                        response.Refund = refund;
                        response.Result = ResponseCode.SUCCESS;
                        response.Reason = ResponseReasonCode.NONE.ToString();
                        response.Success = true;

                        NotifyListeners(listener => listener.OnRefundPaymentResponse(response));
                    }
                }
            }

            public void onFinishCancel(TxType requestInfo)
            {
                onFinishCancel(ResponseCode.CANCEL, requestInfo);
            }

            public void onFinishCancel(ResponseCode result, TxType requestInfo, String reason = null, String message = null)
            {
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);

                // depricated lastRequest support: remove when feature no longer necessary. NB: Refund/LastPRR has some additional complexity to it.
                if (requestInfo == TxType.NONE)
                {
                    if (cloverConnector.lastRequest is PreAuthRequest)
                    {
                        requestInfo = TxType.PREAUTH;
                    }
                    else if (cloverConnector.lastRequest is AuthRequest)
                    {
                        requestInfo = TxType.AUTH;
                    }
                    else if (cloverConnector.lastRequest is SaleRequest)
                    {
                        requestInfo = TxType.SALE;
                    }
                    else if (cloverConnector.lastRequest is ManualRefundRequest)
                    {
                        requestInfo = TxType.MANUAL_REFUND;
                    }
                    else if (lastPRR != null)
                    {
                        requestInfo = TxType.REFUND;
                    }
                }
                cloverConnector.lastRequest = null;

                if (requestInfo == TxType.PREAUTH)
                {
                    NotifyListeners(listener =>
                    {
                        PreAuthResponse response = new PreAuthResponse
                        {
                            Success = false,
                            Result = result,
                            Reason = reason ?? "Request Canceled",
                            Message = message ?? "PreAuth Request canceled by user"
                        };

                        listener.OnPreAuthResponse(response);
                    });
                }
                else if (requestInfo == TxType.AUTH)
                {
                    NotifyListeners(listener =>
                    {
                        AuthResponse response = new AuthResponse
                        {
                            Success = false,
                            Result = result,
                            Reason = reason ?? "Request Canceled",
                            Message = message ?? "Auth Request canceled by user"
                        };

                        listener.OnAuthResponse(response);
                    });
                }
                else if (requestInfo == TxType.SALE)
                {
                    NotifyListeners(listener =>
                    {
                        SaleResponse response = new SaleResponse
                        {
                            Payment = null,
                            Success = false,
                            Result = result,
                            Reason = reason ?? "Request Canceled",
                            Message = message ?? "Sale Request canceled by user"
                        };

                        listener.OnSaleResponse(response);
                    });
                }
                else if (requestInfo == TxType.MANUAL_REFUND)
                {
                    NotifyListeners(listener =>
                    {
                        ManualRefundResponse response = new ManualRefundResponse
                        {
                            Success = false,
                            Result = result,
                            Reason = reason ?? "Request Canceled",
                            Message = message ?? "Manual Refund Request canceled by user"
                        };

                        listener.OnManualRefundResponse(response);
                    });
                }
                else if (requestInfo == TxType.REFUND)
                {
                    RefundPaymentResponse localPRR = lastPRR;
                    lastPRR = null;

                    NotifyListeners(listener =>
                    {
                        RefundPaymentResponse response = new RefundPaymentResponse();
                        response.Result = localPRR.Result;
                        response.Success = localPRR.Success;
                        response.Reason = localPRR.Reason;
                        response.OrderId = localPRR.OrderId;
                        response.PaymentId = localPRR.PaymentId;
                        response.Refund = localPRR.Refund;
                        response.Message = localPRR.Message;

                        listener.OnRefundPaymentResponse(response);
                    });
                }
                // else
                // {
                //     // We'd like to report an error here, a FinishCancel with no match. In testing it popped up on initial connection at least once might count as a change in api behavior.
                //     cloverConnector.OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, 0, null, "Unknown type of onFinishCancel response received"));
                // }
            }

            public void onVerifySignature(Payment payment, Signature2 signature)
            {
                NotifyListeners(listener =>
                {
                    SVR request = new SVR(cloverConnector.Device)
                    {
                        Signature = signature,
                        Payment = payment
                    };

                    listener.OnVerifySignatureRequest(request);
                });
            }

            public void onPaymentVoided(Payment payment, VoidReason voidReason, ResultStatus result, string reason, string message)
            {
                cloverConnector.ShowOnDevice("The transaction was voided.", 3000, true, 3000, true);

                NotifyListeners(listener =>
                {
                    VoidPaymentResponse response = new VoidPaymentResponse
                    {
                        Success = result == ResultStatus.SUCCESS,
                        Result = result.ToResponseCode(),
                        Message = message ?? "No extended information provided.",
                        Reason = reason ?? result.ToString(),
                        PaymentId = payment.id,
                        Payment = payment
                    };

                    listener.OnVoidPaymentResponse(response);
                });
            }

            //For Error/Fail scenarios that don't have a Payment object to send back
            public void onPaymentVoided(ResponseCode result, String reason = null, String message = null)
            {
                NotifyListeners(listener =>
                {
                    VoidPaymentResponse response = new VoidPaymentResponse
                    {
                        Success = false,
                        Result = result,
                        Reason = reason ?? result.ToString(),
                        Message = message ?? "No extended information provided.",
                        PaymentId = null
                    };

                    listener.OnVoidPaymentResponse(response);
                });
            }

            //For Error/Fail scenarios that don't have a Payment object to send back
            public void onPaymentRefundVoided(ResponseCode result, String reason = null, String message = null)
            {
                NotifyListeners(listener =>
                {
                    VoidPaymentRefundResponse response = new VoidPaymentRefundResponse
                    {
                        Success = false,
                        Result = result,
                        Reason = reason ?? result.ToString(),
                        Message = message ?? "No extended information provided.",
                        RefundId = null
                    };

                    listener.OnVoidPaymentRefundResponse(response);
                });
            }

            public void onVaultCardResponse(VaultCardResponseMessage vcrm)
            {
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);

                NotifyListeners(listener =>
                {
                    VaultCardResponse vcr = new VaultCardResponse
                    {
                        Card = vcrm.card,
                        Success = vcrm.status == ResultStatus.SUCCESS,
                        Reason = vcrm.reason
                    };
                    switch (vcrm.status)
                    {
                        case ResultStatus.SUCCESS:
                            vcr.Result = ResponseCode.SUCCESS;
                            break;
                        case ResultStatus.CANCEL:
                            vcr.Result = ResponseCode.CANCEL;
                            break;
                        case ResultStatus.FAIL:
                            vcr.Result = ResponseCode.FAIL;
                            break;
                        default:
                            vcr.Result = ResponseCode.ERROR;
                            break;
                    }

                    listener.OnVaultCardResponse(vcr);
                });
            }

            //For Error/Fail scenarios where a valid Card object does not exist
            public void onVaultCardResponse(ResponseCode result, String reason = null, String message = null)
            {
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);

                NotifyListeners(listener =>
                {
                    VaultCardResponse vcr = new VaultCardResponse
                    {
                        Card = null,
                        Success = false,
                        Reason = reason,
                        Message = message
                    };

                    listener.OnVaultCardResponse(vcr);
                });
            }

            public void onReadCardDataResponse(ReadCardDataResponseMessage cdrm)
            {
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);

                NotifyListeners(listener =>
                {
                    ReadCardDataResponse cdr = new ReadCardDataResponse
                    {
                        CardData = cdrm.cardData,
                        Success = cdrm.status == ResultStatus.SUCCESS,
                        Reason = cdrm.reason
                    };

                    listener.OnReadCardDataResponse(cdr);
                });
            }

            //For Error/Fail scenarios where a valid CardData object does not exist
            public void onReadCardDataResponse(ResponseCode result, String reason = null, String message = null)
            {
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);

                NotifyListeners(listener =>
                {
                    ReadCardDataResponse rcdr = new ReadCardDataResponse
                    {
                        CardData = null,
                        Success = false,
                        Reason = reason,
                        Message = message
                    };

                    listener.OnReadCardDataResponse(rcdr);
                });
            }

            public void onCapturePreAuthResponse(String paymentId, long amount, long? tipAmount, ResultStatus status, string reason)
            {
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);

                NotifyListeners(listener =>
                {
                    CapturePreAuthResponse car = new CapturePreAuthResponse
                    {
                        Success = status == ResultStatus.SUCCESS,
                        Result = status == ResultStatus.SUCCESS ? ResponseCode.SUCCESS : ResponseCode.FAIL,
                        Reason = reason,
                        PaymentId = paymentId,
                        Amount = amount,
                        TipAmount = tipAmount
                    };

                    listener.OnCapturePreAuthResponse(car);
                });
            }

            //For Error/Fail scenarios where a payment was never processed
            public void onCapturePreAuthResponse(ResponseCode responseCode, String reason = null, String message = null)
            {
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);

                NotifyListeners(listener =>
                {
                    CapturePreAuthResponse car = new CapturePreAuthResponse
                    {
                        Success = responseCode == ResponseCode.SUCCESS,
                        Result = responseCode,
                        Reason = reason ?? responseCode.ToString(),
                        Message = message ?? "No extended information provided.",
                        PaymentId = null,
                        Amount = 0,
                        TipAmount = 0
                    };

                    listener.OnCapturePreAuthResponse(car);
                });
            }

            public void onIncrementPreAuthResponse(IncrementPreAuthResponseMessage message)
            {
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);

                NotifyListeners(listener =>
                {
                    IncrementPreAuthResponse ipar = new IncrementPreAuthResponse
                    {
                        Success = message.status == ResultStatus.SUCCESS,
                        Result = message.status == ResultStatus.SUCCESS ? ResponseCode.SUCCESS : ResponseCode.FAIL,
                        Reason = message.reason,
                        Authorization = message.authorization,
                    };

                    listener.OnIncrementPreAuthResponse(ipar);
                });
            }

            //For Error/Fail scenarios where a payment was never processed
            public void onIncrementPreAuthResponse(ResponseCode responseCode, String reason = null, String message = null)
            {
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);

                NotifyListeners(listener =>
                {
                    IncrementPreAuthResponse ipar = new IncrementPreAuthResponse
                    {
                        Success = responseCode == ResponseCode.SUCCESS,
                        Result = responseCode,
                        Reason = reason ?? responseCode.ToString(),
                        Message = message ?? "No extended information provided.",
                        Authorization = null,
                    };

                    listener.OnIncrementPreAuthResponse(ipar);
                });
            }

            public void onRetrievePendingPaymentsResponse(ResponseCode result, String reason = null, String message = null)
            {
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);

                NotifyListeners(listener =>
                {
                    RetrievePendingPaymentsResponse response = new RetrievePendingPaymentsResponse
                    {
                        PendingPayments = null,
                        Success = false,
                        Reason = reason,
                        Message = message
                    };

                    listener.OnRetrievePendingPaymentsResponse(response);
                });
            }

            public void onRetrievePendingPaymentsResponse(bool success, List<PendingPaymentEntry> pendingPayments)
            {
                NotifyListeners(listener =>
                {
                    RetrievePendingPaymentsResponse response = new RetrievePendingPaymentsResponse
                    {
                        PendingPayments = pendingPayments,
                        Success = true
                    };

                    listener.OnRetrievePendingPaymentsResponse(response);
                });
            }

            public void onTxStartResponse(TxStartResponseResult result, string externalId, string reason, string message, string requestInfo)
            {
                // TxStart Successes posted separately via FINISH_OK and FINISH_CANCEL messages; silently exit
                if (result == TxStartResponseResult.SUCCESS)
                {
                    return;
                }

                bool duplicate = result.Equals(TxStartResponseResult.DUPLICATE);

                try
                {
                    if (cloverConnector.lastRequest is PreAuthRequest)
                    {
                        NotifyListeners(listener =>
                        {
                            PreAuthResponse response = new PreAuthResponse();
                            response.Success = false;
                            if (duplicate)
                            {
                                response.Result = ResponseCode.CANCEL;
                                response.Reason = reason ?? response.Result.ToString();
                                response.Message = "The provided transaction id of " + externalId + " has already been processed and cannot be resubmitted.";
                            }
                            else
                            {
                                response.Result = ResponseCode.FAIL;
                                response.Reason = reason ?? response.Result.ToString();
                                response.Message = message ?? "";
                            }
                            listener.OnPreAuthResponse(response);
                        });
                    }
                    else if (cloverConnector.lastRequest is AuthRequest)
                    {
                        NotifyListeners(listener =>
                        {
                            AuthResponse response = new AuthResponse();
                            response.Success = false;
                            if (duplicate)
                            {
                                response.Result = ResponseCode.CANCEL;
                                response.Reason = reason ?? response.Result.ToString();
                                response.Message = "The provided transaction id of " + externalId + " has already been processed and cannot be resubmitted.";
                            }
                            else
                            {
                                response.Result = ResponseCode.FAIL;
                                response.Reason = reason ?? response.Result.ToString();
                                response.Message = message ?? "";
                            }
                            listener.OnAuthResponse(response);
                        });
                    }
                    else if (cloverConnector.lastRequest is SaleRequest)
                    {
                        NotifyListeners(listener =>
                        {
                            SaleResponse response = new SaleResponse();
                            response.Success = false;
                            if (duplicate)
                            {
                                response.Result = ResponseCode.CANCEL;
                                response.Reason = reason ?? response.Result.ToString();
                                response.Message = "The provided transaction id of " + externalId + " has already been processed and cannot be resubmitted.";
                            }
                            else
                            {
                                response.Result = ResponseCode.FAIL;
                                response.Reason = reason ?? response.Result.ToString();
                                response.Message = message ?? "";
                            }
                            listener.OnSaleResponse(response);
                        });
                    }
                    else if (cloverConnector.lastRequest is ManualRefundRequest)
                    {
                        NotifyListeners(listener =>
                        {
                            ManualRefundResponse response = new ManualRefundResponse();
                            response.Success = false;
                            if (duplicate)
                            {
                                response.Result = ResponseCode.CANCEL;
                                response.Reason = reason ?? response.Result.ToString();
                                response.Message = "The provided transaction id of " + externalId + " has already been processed and cannot be resubmitted.";
                            }
                            else
                            {
                                response.Result = ResponseCode.FAIL;
                                response.Reason = reason ?? response.Result.ToString();
                                response.Message = message ?? "";
                            }
                            listener.OnManualRefundResponse(response);
                        });
                    }
                }
                finally
                {
                    cloverConnector.lastRequest = null;
                }
            }

            public void onConfirmPayment(Payment payment, List<Challenge> challenges)
            {
                NotifyListeners(listener =>
                {
                    ConfirmPaymentRequest request = new ConfirmPaymentRequest
                    {
                        Challenges = challenges,
                        Payment = payment
                    };

                    listener.OnConfirmPaymentRequest(request);
                });
            }

            public void onMessageAck(string sourceMessageId)
            {
                // Acks are not pushed through to the public interface. Source message id is private: unknown/unmatchable outside CloverConnector > CloverDevice class internals
                // To be useful, we would need to track the outgoing messages and map the id to calling function

                // NotifyListeners(listener =>
                // {
                // });
            }

            public void onActivityResponse(ResultStatus status, String action, String payload, String failReason)
            {
                NotifyListeners(listener =>
                {
                    CustomActivityResponse car = new CustomActivityResponse
                    {
                        Action = action,
                        Payload = payload,
                        Success = status == ResultStatus.SUCCESS,
                        Reason = failReason
                    };

                    listener.OnCustomActivityResponse(car);
                });
            }

            public void onPrintCredit(Credit credit)
            {
                NotifyListeners(listener =>
                {
                    PrintManualRefundReceiptMessage message = new PrintManualRefundReceiptMessage
                    {
                        Success = true,
                        Credit = credit
                    };

                    listener.OnPrintManualRefundReceipt(message);
                });
            }

            public void onPrintPayment(Payment payment, Order order)
            {
                NotifyListeners(listener =>
                {
                    PrintPaymentReceiptMessage message = new PrintPaymentReceiptMessage
                    {
                        Success = true,
                        Payment = payment,
                        Order = order
                    };

                    listener.OnPrintPaymentReceipt(message);
                });
            }

            public void onPrintCreditDecline(Credit credit, String reason)
            {
                NotifyListeners(listener =>
                {
                    PrintManualRefundDeclineReceiptMessage message = new PrintManualRefundDeclineReceiptMessage
                    {
                        Success = true,
                        Credit = credit,
                        Reason = reason
                    };

                    listener.OnPrintManualRefundDeclineReceipt(message);
                });
            }

            public void onPrintRefundPayment(Payment payment, Order order, Refund refund)
            {
                NotifyListeners(listener =>
                {
                    PrintRefundPaymentReceiptMessage message = new PrintRefundPaymentReceiptMessage
                    {
                        Success = true,
                        Payment = payment,
                        Order = order,
                        Refund = refund
                    };

                    listener.OnPrintRefundPaymentReceipt(message);
                });
            }

            public void onPrintPaymentDecline(Payment payment, String reason)
            {
                NotifyListeners(listener =>
                {
                    PrintPaymentDeclineReceiptMessage message = new PrintPaymentDeclineReceiptMessage
                    {
                        Success = true,
                        Payment = payment,
                        Reason = reason
                    };

                    listener.OnPrintPaymentDeclineReceipt(message);
                });
            }

            public void onPrintMerchantReceipt(Payment payment)
            {
                NotifyListeners(listener =>
                {
                    PrintPaymentMerchantCopyReceiptMessage message = new PrintPaymentMerchantCopyReceiptMessage
                    {
                        Success = true,
                        Payment = payment
                    };

                    listener.OnPrintPaymentMerchantCopyReceipt(message);
                });
            }

            public void onMessageFromActivity(string action, string payload)
            {
                NotifyListeners(listener =>
                {
                    MessageFromActivity mfa = new MessageFromActivity
                    {
                        Action = action,
                        Payload = payload
                    };

                    listener.OnMessageFromActivity(mfa);
                });
            }

            public void onResetDeviceResponse(ResultStatus status, string reason, transport.ExternalDeviceState state)
            {
                NotifyListeners(listener =>
                {
                    ResetDeviceResponse rdr = new ResetDeviceResponse
                    {
                        Success = status == ResultStatus.SUCCESS,
                        Result = status == ResultStatus.SUCCESS ? ResponseCode.SUCCESS : ResponseCode.CANCEL
                    };
                    if (Enum.TryParse(state.ToString(), out ExternalDeviceState st))
                    {
                        rdr.State = st;
                    }

                    listener.OnResetDeviceResponse(rdr);
                });
            }

            public void onDeviceStatusResponse(ResultStatus status, string reason, transport.ExternalDeviceState state, transport.ExternalDeviceStateData data)
            {
                NotifyListeners(listener =>
                {
                    RetrieveDeviceStatusResponse rdsr = new RetrieveDeviceStatusResponse
                    {
                        Success = status == ResultStatus.SUCCESS,
                        Result = status == ResultStatus.SUCCESS ? ResponseCode.SUCCESS : ResponseCode.CANCEL
                    };
                    if (Enum.TryParse(state.ToString(), out ExternalDeviceState st))
                    {
                        rdsr.State = st;
                    }

                    try
                    {
                        rdsr.Data = JsonUtils.Deserialize<ExternalDeviceStateData>(JsonUtils.Serialize(data));
                    }
                    catch (InvalidOperationException)
                    {
                        rdsr.Data = null;
                    }

                    listener.OnRetrieveDeviceStatusResponse(rdsr);
                });
            }

            public void onRetrievePaymentResponse(ResultStatus status, string reason, string externalPaymentId, QueryStatus queryStatus, Payment payment)
            {
                NotifyListeners(listener =>
                {
                    RetrievePaymentResponse rpr = new RetrievePaymentResponse
                    {
                        Success = status == ResultStatus.SUCCESS,
                        Result = status == ResultStatus.SUCCESS ? ResponseCode.SUCCESS : ResponseCode.CANCEL,
                        Reason = reason,
                        QueryStatus = queryStatus,
                        ExternalPaymentId = externalPaymentId

                    };
                    try
                    {
                        rpr.Payment = JsonUtils.Deserialize<Payment>(JsonUtils.Serialize(payment));
                    }
                    catch (InvalidOperationException)
                    {
                        rpr.Payment = null;
                    }

                    listener.OnRetrievePaymentResponse(rpr);
                });
            }

            public void onRetrievePrintersResponse(List<Printer> printers)
            {
                NotifyListeners(listener =>
                {
                    RetrievePrintersResponse rpr = new RetrievePrintersResponse(printers);
                    rpr.Result = printers != null ? ResponseCode.SUCCESS : ResponseCode.FAIL;
                    rpr.Success = rpr.Result == ResponseCode.SUCCESS;

                    listener.OnRetrievePrintersResponse(rpr);
                });
            }

            public void onRetrievePrintJobStatus(string printRequestId, string status)
            {
                NotifyListeners(listener =>
                {
                    PrintJobStatusResponse pjsr = new PrintJobStatusResponse(printRequestId, status);
                    pjsr.Success = true;
                    listener.OnPrintJobStatusResponse(pjsr);
                });
            }

            public void onDisplayReceiptOptionsResponse(ResultStatus status, string reason)
            {
                NotifyListeners(listener =>
                {
                    DisplayReceiptOptionsResponse dror = new DisplayReceiptOptionsResponse
                    {
                        Reason = reason,
                        status = status,
                        Success = status == ResultStatus.SUCCESS
                    };
                    listener.OnDisplayReceiptOptionsResponse(dror);
                });
            }

            public void onCustomerProvidedDataResponse(string eventId, DataProviderConfig config, string data)
            {
                NotifyListeners(listener =>
                {
                    CustomerProvidedDataEvent response = new CustomerProvidedDataEvent
                    {
                        Success = true,
                        eventId = eventId,
                        config = config,
                        data = data
                    };
                    listener.OnCustomerProvidedData(response);
                });
            }

            public void onInvalidStateTransition(string reason, string requestedTransition, string state, string substate, transport.ExternalDeviceStateData data)
            {
                NotifyListeners(listener =>
                {
                    InvalidStateTransitionNotification message = new InvalidStateTransitionNotification
                    {
                        Reason = reason,
                        RequestedTransition = requestedTransition,
                        State = state,
                        Substate = substate,
                        Success = true
                    };
                    if (data != null)
                    {
                        message.Data = new ExternalDeviceStateData()
                        {
                            CustomActivityId = data.customActivityId,
                            ExternalPaymentId = data.externalPaymentId
                        };
                    }
                    listener.OnInvalidStateTransitionResponse(message);
                });
            }

            public void NotifyListeners(Action<ICloverConnectorListener> action)
            {
                List<ICloverConnectorListener> localListeners = new List<ICloverConnectorListener>(cloverConnector.listeners);
                foreach (ICloverConnectorListener listener in localListeners)
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += delegate
                        {
                            try
                            {
                                action(listener);
                            }
                            catch (Exception exception)
                            {
                                // eat unhandled user exceptions - any logging other than debug?
                                System.Diagnostics.Debug.WriteLine("CloverConnector: Error calling custom code: " + exception);
                            }
                        };
                    bw.RunWorkerAsync();
                }
            }
        }
    }
}
