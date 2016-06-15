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

using System;
using com.clover.remotepay.transport;
using com.clover.sdk.v3.base_;
using com.clover.sdk.v3.order;
using com.clover.sdk.v3.payments;
using System.ComponentModel;
using com.clover.remote.order;
using com.clover.remote.order.operation;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
namespace com.clover.remotepay.sdk
{
    /// <summary>
    /// 
    /// </summary>
    public class CloverConnector : ICloverConnector
    {
        private const int ENABLE_KIOSK_ENTRY_METHODS = 1 << 15;
        public const int CARD_ENTRY_METHOD_MAG_STRIPE = 1 | (1 << 8) | ENABLE_KIOSK_ENTRY_METHODS; //257 + the flag to indicate the flags are passed in
        public const int CARD_ENTRY_METHOD_ICC_CONTACT = 2 | (2 << 8) | ENABLE_KIOSK_ENTRY_METHODS; //514 + the flag to indicate the flags are passed in
        public const int CARD_ENTRY_METHOD_NFC_CONTACTLESS = 4 | (4 << 8) | ENABLE_KIOSK_ENTRY_METHODS; //1028 + the flag to indicate the flags are passed in
        public const int CARD_ENTRY_METHOD_MANUAL = 8 | (8 << 8) | ENABLE_KIOSK_ENTRY_METHODS; // 2056 + the flag to indicate the flags are passed in

        public static readonly InputOption CANCEL_INPUT_OPTION = new InputOption(KeyPress.ESC, "Cancel");

        protected CloverDevice Device;
        protected CloverDeviceConfiguration Config;
        protected SDKInfo _SDKInfo = new SDKInfo();
        public SDKInfo SDKInfo
        {
            get { return _SDKInfo; }
            private set { _SDKInfo = value; }
        }

        List<ICloverConnectorListener> listeners = new List<ICloverConnectorListener>();

        private MerchantInfo merchantInfo { get; set; }
        private InnerDeviceObserver deviceObserver { get; set; }

        // to hold the default value of CardEntryMethod
        private int CardEntryMethod { get; set; }


        /// <summary>
        /// set to true, so that when request responses are processed, the Clover Mini
        /// won't show default messages/ThankYou/Welcome screens.  Set to false
        /// to allow the default message flow that is built into the CloverConnector.
        /// </summary>
        bool DisableDefaultDeviceScreenFlow { get; set; }

        //keep a last request
        private Object lastRequest;

        public void AddCloverConnectorListener(ICloverConnectorListener connectorListener)
        {
            listeners.Add(connectorListener);
        }

        public void RemoveCloverConnectorListener(ICloverConnectorListener connectorListener)
        {
            listeners.Remove(connectorListener);
        }

        public CloverConnector()
        {
            // default constructor for COM-Interop
            CardEntryMethod = CARD_ENTRY_METHOD_MAG_STRIPE | CARD_ENTRY_METHOD_ICC_CONTACT | CARD_ENTRY_METHOD_NFC_CONTACTLESS;
        }

        /// <summary>
        /// CloverConnector constructor
        /// </summary>
        /// <param name="config">A CloverDeviceConfiguration object; TestDeviceConfiguration can be used for testing
        /// </param>
        public CloverConnector(CloverDeviceConfiguration config) : this()
        {
            Config = config;
            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("CloverConnector");
            _SDKInfo.Name = AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyDescriptionAttribute>(assembly).Description;
            _SDKInfo.Version = (AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyFileVersionAttribute>(assembly)).Version
                + (AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyInformationalVersionAttribute>(assembly)).InformationalVersion;
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
            bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    Device = CloverDeviceFactory.Get(Config);
                    Device.Subscribe(deviceObserver);
                });
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// Sale method, aka "purchase"
        /// </summary>
        /// <param name="request">A SaleRequest object containing basic information needed for the transaction</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
        public void Sale(SaleRequest request)
        {
            lastRequest = request;
            if (Device == null)
            {
                deviceObserver.onFinishCancel(ResponseCode.ERROR,
                                              "Device Connection Error",
                                              "In Sale : SaleRequest - The Clover device is not connected.");
                return;
            }
            if (request == null)
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                  "Request Validation Error",
                                  "In Sale : SaleRequest - The request that was passed in for processing is empty.");
                return;
            }
            if (String.IsNullOrEmpty(request.ExternalId))
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              "Request Validation Error",
                                              "In Sale : SaleRequest - The request ExternalId cannot be null or blank. Original Request = " + request);
                return;
            }
            if (request.Amount == 0)
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              "Request Validation Error",
                                              "In Sale : SaleRequest - The request amount cannot be zero. Original Request = " + request);
                return;
            }
            if (request.VaultedCard != null && !merchantInfo.supportsVaultCards)
            {
                deviceObserver.onFinishCancel(ResponseCode.UNSUPPORTED,
                                              "Merchant Configuration Validation Error",
                                              "In Sale : SaleRequest - Vault Card support is not offered by the merchant configured gateway. Original Request = " + request);
                return;
            }
            PayIntent payIntent = new PayIntent();
            payIntent.externalPaymentId = request.ExternalId;
            payIntent.transactionType = PayIntent.TransactionType.PAYMENT;
            payIntent.remotePrint = request.DisablePrinting;
            payIntent.isDisableCashBack = request.DisableCashback;
            payIntent.cardEntryMethods = request.CardEntryMethods.HasValue ? request.CardEntryMethods.Value : CardEntryMethod;
            payIntent.amount = request.Amount;
            payIntent.tipAmount = request.TipAmount.HasValue ? request.TipAmount.Value : 0;
            payIntent.taxAmount = request.TaxAmount;
            payIntent.tippableAmount = request.TippableAmount;
            payIntent.vaultedCard = request.VaultedCard;
            payIntent.isCardNotPresent = request.CardNotPresent;
            payIntent.disableRestartTransactionWhenFailed = request.DisableRestartTransactionOnFail;
            payIntent.allowOfflinePayment = request.AllowOfflinePayment;
            payIntent.approveOfflinePaymentWithoutPrompt = request.ApproveOfflinePaymentWithoutPrompt;
            Device.doTxStart(payIntent, null, request.DisableTipOnScreen.HasValue ? request.DisableTipOnScreen.Value : false);
        }

        /// <summary>
        /// If signature is captured during a Sale, this method accepts the signature as entered
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void AcceptSignature(VerifySignatureRequest request)
        {
            if (request == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In AcceptSignature : VerifySignatureRequest object cannot be null. "));
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
            if (request == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In RejectSignature : VerifySignatureRequest object cannot be null. "));
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
            lastRequest = request;
            if (Device == null)
            {
                deviceObserver.onFinishCancel(ResponseCode.ERROR,
                                              "Device Connection Error",
                                              "In Auth : AuthRequest - The Clover device is not connected.");
                return;
            }
            if (request == null)
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                  "Request Validation Error",
                                  "In Auth : AuthRequest - The request that was passed in for processing is empty.");
                return;
            }
            if (String.IsNullOrEmpty(request.ExternalId))
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              "Request Validation Error",
                                              "In Auth : AuthRequest - The request ExternalId cannot be null or blank. Original Request = " + request);
                return;
            }
            if (request.Amount == 0)
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              "Request Validation Error",
                                              "In Auth : AuthRequest - The request amount cannot be zero. Original Request = " + request);
                return;
            }
            if (request.VaultedCard != null && !merchantInfo.supportsVaultCards)
            {
                deviceObserver.onFinishCancel(ResponseCode.UNSUPPORTED,
                                              "Merchant Configuration Validation Error",
                                              "In Auth : AuthRequest - Vault Card support is not offered by the merchant configured gateway. Original Request = " + request);
                return;
            }
            PayIntent payIntent = new PayIntent();
            payIntent.externalPaymentId = request.ExternalId;
            payIntent.transactionType = request.Type;
            payIntent.remotePrint = request.DisablePrinting;
            payIntent.isDisableCashBack = request.DisableCashback;
            payIntent.cardEntryMethods = request.CardEntryMethods.HasValue ? request.CardEntryMethods.Value : CardEntryMethod;
            payIntent.vaultedCard = request.VaultedCard;
            payIntent.amount = request.Amount;
            payIntent.tipAmount = null; // have to force this to null until PayIntent honors transactionType of AUTH
            payIntent.isCardNotPresent = request.CardNotPresent;
            payIntent.disableRestartTransactionWhenFailed = request.DisableRestartTransactionOnFail;
            payIntent.allowOfflinePayment = request.AllowOfflinePayment;
            payIntent.approveOfflinePaymentWithoutPrompt = request.ApproveOfflinePaymentWithoutPrompt;
            Device.doTxStart(payIntent, null, true);
        }

        /// <summary>
        /// PreAuth method to obtain a PreAuth.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void PreAuth(PreAuthRequest request)
        {
            lastRequest = request;
            if (Device == null)
            {
                deviceObserver.onFinishCancel(ResponseCode.ERROR,
                                              "Device Connection Error",
                                              "In PreAuth : PreAuthRequest - The Clover device is not connected.");
                return;
            }
            if (request == null)
            {
                deviceObserver.onFinishCancel(ResponseCode.ERROR,
                                              "Request Validation Error",
                                              "In PreAuth : PreAuthRequest - The request that was passed in for processing is empty.");
                return;
            }
            if (String.IsNullOrEmpty(request.ExternalId))
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              "Request Validation Error",
                                              "In PreAuth : PreAuthRequest - The request ExternalId cannot be null or blank. Original Request = " + request);
                return;
            }
            if (request.Amount == 0)
            {
                deviceObserver.onFinishCancel(ResponseCode.ERROR,
                                              "Request Validation Error",
                                              "In PreAuth : PreAuthRequest - The request amount cannot be zero. Original Request = " + request);
                return;
            }
            if (request.VaultedCard != null && !merchantInfo.supportsVaultCards)
            {
                deviceObserver.onFinishCancel(ResponseCode.UNSUPPORTED,
                                              "Merchant Configuration Validation Error",
                                              "In PreAuth : PreAuthRequest - Vault Card support is not offered by the merchant configured gateway. Original Request = " + request);
                return;
            }
            PayIntent payIntent = new PayIntent();
            payIntent.externalPaymentId = request.ExternalId;
            payIntent.transactionType = PayIntent.TransactionType.AUTH;
            payIntent.remotePrint = request.DisablePrinting;
            payIntent.cardEntryMethods = request.CardEntryMethods.HasValue ? request.CardEntryMethods.Value : CardEntryMethod;
            payIntent.vaultedCard = request.VaultedCard;
            payIntent.amount = request.Amount;
            payIntent.tipAmount = null; // have to force this to null until PayIntent honors transactionType of AUTH
            payIntent.isCardNotPresent = request.CardNotPresent;
            payIntent.disableRestartTransactionWhenFailed = request.DisableRestartTransactionOnFail;
            Device.doTxStart(payIntent, null, true);
        }

        /// <summary>
        /// Capture a previous Auth. Note: Should only be called if request's PaymentID is from an AuthResponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void CapturePreAuth(CapturePreAuthRequest request)
        {
            if (Device == null)
            {
                deviceObserver.onCapturePreAuthResponse(ResponseCode.ERROR,
                                                        "Device Connection Error",
                                                        "In CapturePreAuth : CapturePreAuthRequest - The Clover device is not connected.");
                return;
            }
            if (request == null)
            {
                deviceObserver.onCapturePreAuthResponse(ResponseCode.FAIL,
                                                        "Request Validation Error",
                                                        "In CapturePreAuth : CapturePreAuthRequest - The request that was passed in for processing is empty.");
                return;
            }
            if (request.Amount == 0)
            {
                deviceObserver.onCapturePreAuthResponse(ResponseCode.FAIL,
                                                        "Request Validation Error",
                                                        "In CapturePreAuth : CapturePreAuthRequest - The Request amount cannot be zero. Original Request = " + request);
                return;
            }
            if (request.TipAmount >= 0 && !merchantInfo.supportsPreAuths)
            {
                deviceObserver.onCapturePreAuthResponse(ResponseCode.UNSUPPORTED,
                                                        "Merchant Configuration Validation Error",
                                                        "In CapturePreAuth : CapturePreAuthRequest - Capture PreAuth is not supported by the merchant configured gateway. Original Request = " + request);
                return;
            }
            Device.doCapturePreAuth(request.PaymentID, request.Amount, request.TipAmount);
        }

        /// <summary>
        /// Adjust the tip for a previous Auth. Note: Should only be called if request's PaymentID is from an AuthResponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void TipAdjustAuth(TipAdjustAuthRequest request)
        {
            if (Device == null)
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
            Device.doTipAdjustAuth(request.OrderID, request.PaymentID, request.TipAmount);
        }

        /// <summary>
        /// Void a transaction, given a previously used order ID and/or payment ID
        /// TBD - defining a payment or order ID to be used with a void without requiring a response from Sale()
        /// </summary>
        /// <param name="request">A VoidRequest object containing basic information needed to void the transaction</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
        public void VoidPayment(VoidPaymentRequest request) // SaleResponse is a Transaction? or create a Transaction from a SaleResponse
        {
            if (Device == null)
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
            if (request.PaymentId == null)
            {
                deviceObserver.onPaymentVoided(ResponseCode.FAIL,
                                                 "Request Validation Error",
                                                 "In VoidPayment : VoidPaymentRequest PaymentId cannot be empty. " + request);
                return;
            }
            Payment payment = new Payment();
            payment.id = request.PaymentId;
            payment.order = new Reference();
            payment.order.id = request.OrderId;
            payment.employee = new Reference();
            payment.employee.id = request.EmployeeId;
            VoidReason reason = (VoidReason)Enum.Parse(typeof(VoidReason), request.VoidReason, true);
            Device.doVoidPayment(payment, reason);
        }

        /// <summary>
        /// Refund a specific payment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void RefundPayment(RefundPaymentRequest request)
        {
            if (Device == null)
            {
                RefundPaymentResponse prr = new RefundPaymentResponse();
                prr.Refund = null;
                prr.Success = false;
                prr.Result = ResponseCode.ERROR;
                prr.Reason = "Device Connection Error";
                prr.Message = "In RefundPayment : RefundPaymentRequest - The Clover device is not connected.";
                deviceObserver.lastPRR = prr;
                deviceObserver.onFinishCancel();
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
                deviceObserver.onFinishCancel();
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
                deviceObserver.onFinishCancel();
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
                deviceObserver.onFinishCancel();
                return;
            }
            Device.doRefundPayment(request.OrderId, request.PaymentId, request.Amount, request.FullRefund);
        }

        /// <summary>
        /// Manual refund method, aka "naked credit"
        /// </summary>
        /// <param name="request">A ManualRefundRequest object</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
        public void ManualRefund(ManualRefundRequest request) // NakedRefund is a Transaction, with just negative amount
        {
            //payment, finishOK(credit), finishCancel, onPaymentVoided
            lastRequest = request;
            if (Device == null)
            {
                deviceObserver.onFinishCancel(ResponseCode.ERROR,
                                              "Device Connection Error",
                                              "In ManualRefund : ManualRefundRequest - The Clover device is not connected.");
                return;
            }
            if (request == null)
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              "Request Validation Error",
                                              "In ManualRefund : ManualRefundRequest - The request that was passed in for processing is empty.");
                return;
            }
            if (String.IsNullOrEmpty(request.ExternalId))
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              "Request Validation Error",
                                              "In ManualRefund : ManualRefundRequest - The request ExternalId cannot be null or blank. Original Request = " + request);
                return;
            }
            if (request.Amount < 0)
            {
                deviceObserver.onFinishCancel(ResponseCode.FAIL,
                                              "Request Validation Error",
                                              "In ManualRefund : ManualRefundRequest Amount must be greater than zero. " + request);
                return;
            }
            if (!merchantInfo.supportsManualRefunds)
            {
                deviceObserver.onFinishCancel(ResponseCode.UNSUPPORTED,
                                              "Merchant Configuration Error",
                                              "In ManualRefund: ManualRefundRequest - Manual refunds are not supported by the merchant configured gateway." + request);
                return;
            }
            if (request.VaultedCard != null && !merchantInfo.supportsVaultCards)
            {
                deviceObserver.onFinishCancel(ResponseCode.UNSUPPORTED,
                                              "Merchant Configuration Validation Error",
                                              "In ManualRefund : RefundRequest - Vault Card support is not offered by the merchant configured gateway. Original Request = " + request);
                return;
            }

            PayIntent payIntent = new PayIntent();
            payIntent.cardEntryMethods = request.CardEntryMethods.HasValue ? request.CardEntryMethods.Value : CardEntryMethod;
            payIntent.amount = -Math.Abs(request.Amount);
            payIntent.transactionType = PayIntent.TransactionType.CREDIT;
            payIntent.externalPaymentId = request.ExternalId;
            payIntent.disableRestartTransactionWhenFailed = request.DisableRestartTransactionOnFail;
            payIntent.remotePrint = request.DisablePrinting;
            payIntent.vaultedCard = request.VaultedCard;
            Device.doTxStart(payIntent, null, true);
        }

        /// <summary>
        /// Send a request to the server to closeout all orders.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void Closeout(CloseoutRequest request)
        {
            Device.doCloseout(request.AllowOpenTabs, request.BatchId); // TODO: pass in request UUID so it can be returned with CloseoutResponse
        }

        /// <summary>
        /// Send a request to the mini to reset.  This can be used if the device gets into a non-recoverable state.
        /// </summary>
        /// <returns></returns>
        public void ResetDevice()
        {
            Device.doResetDevice();
        }

        /// <summary>
        /// Cancels the device from waiting for payment card
        /// </summary>
        /// <returns></returns>
        public void Cancel()
        {
            InvokeInputOption(CANCEL_INPUT_OPTION);
        }

        /// <summary>
        /// Print simple lines of text to the Clover Mini printer
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public void PrintText(List<string> messages)
        {
            if (Device == null)
            {
                return;
            }
            if (messages == null)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In PrintText : message list cannot be null. "));
                return;
            }

            Device.doPrintText(messages);
        }

        /// <summary>
        /// Print an image on the Clover Mini printer
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public void PrintImage(Bitmap bitmap) //Bitmap img
        {
            if (bitmap == null) { OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In PrintImage : Bitmap object cannot be null. ")); }
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            byte[] imgBytes = ms.ToArray();
            string base64Image = Convert.ToBase64String(imgBytes);

            Device.doPrintImage(base64Image);
        }

        /// <summary>
        /// Show a message on the Clover Mini screen
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public void ShowMessage(string message)
        {
            ShowOnDevice(message);
        }

        /// <summary>
        /// Return the device to the Welcome Screen
        /// </summary>
        public void ShowWelcomeScreen()
        {
            ShowOnDevice(showWelcomeScreen: true);
        }

        /// <summary>
        /// Show the thank you screen on the device
        /// </summary>
        public void ShowThankYouScreen()
        {
            ShowOnDevice(showThankYouScreen: true);
        }

        /// <summary>
        /// This method provides a way to control the default screen flow on the device, for instances
        /// where it makes sense to display some combination of message/thankyou/welcome screens with 
        /// configurable timing between them
        /// </summary>
        private void ShowOnDevice(string msg = null, Int32 milliSecondsTimeoutForMsg = 0, bool showThankYouScreen = false, Int32 milliSecondsTimeoutForThankYou = 0, bool showWelcomeScreen = false)
        {
            if (Device != null)
            {
                if (!DisableDefaultDeviceScreenFlow)
                {
                    BackgroundWorker bgWorker = new BackgroundWorker();
                    bgWorker.DoWork += new DoWorkEventHandler(
                        delegate (object o, DoWorkEventArgs args)
                        {
                            if (msg != null)
                            {
                                Device.doTerminalMessage(msg);
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
                        }
                    );
                    bgWorker.RunWorkerAsync();
                }
                return;
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Vault Card information and payment token
        /// </summary>
        public void VaultCard(int? CardEntryMethods)
        {
            if (Device == null)
            {
                return;
            }

            if (!merchantInfo.supportsVaultCards)
            {
                deviceObserver.onVaultCardResponse(ResponseCode.UNSUPPORTED,
                                                  "Merchant Configuration Error",
                                                  "In VaultCard: - Vault card is not supported by the merchant configured gateway.");
                return;
            }

            Device.doVaultCard(CardEntryMethods.HasValue ? CardEntryMethods.Value : CardEntryMethod);
        }

        /// <summary>
        /// Show the customer facing receipt option screen for the specified Payment.
        /// </summary>
        public void DisplayPaymentReceiptOptions(string orderId, string paymentId)
        {
            if (Device != null)
            {
                Device.doShowPaymentReceiptScreen(orderId, paymentId);
            }
        }

        /// <summary>
        /// Will trigger cash drawer to open that is connected to Clover Mini
        /// </summary>
        public void OpenCashDrawer(String reason)
        {
            if (Device != null)
            {
                if (reason == null)
                {
                    OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In OpenCashDrawer : Reason string cannot be null. "));
                    return;
                }
                Device.doOpenCashDrawer(reason);
            }
        }

        /// <summary>
        /// Show the DisplayOrder on the device. Replaces the existing DisplayOrder on the device.
        /// </summary>
        /// <param name="order"></param>
        public void ShowDisplayOrder(DisplayOrder order)
        {
            if (Device != null)
            {
                if (order == null)
                {
                    OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In DisplayOrder : DisplayOrder object cannot be null. "));
                    return;
                }
                Device.doOrderUpdate(order, null);
            }
        }

        /// <summary>
        /// Notify the device of a DisplayLineItem being added to a DisplayOrder
        /// </summary>
        /// <param name="order"></param>
        /// <param name="lineItem"></param>
        public void LineItemAddedToDisplayOrder(DisplayOrder order, DisplayLineItem lineItem)
        {
            if (order == null) { OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In LineItemAddedToDisplayOrder : DisplayOrder object cannot be null. ")); }
            if (order.id == null) { OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In LineItemAddedToDisplayOrder : DisplayOrder id cannot be null. " + order)); }
            if (lineItem == null) { OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In LineItemAddedToDisplayOrder : DisplayLineItem object cannot be null. ")); }
            LineItemsAddedOperation liao = new LineItemsAddedOperation();
            liao.orderId = order.id;
            liao.addId(lineItem.id);
            if (Device != null)
            {
                Device.doOrderUpdate(order, liao);
            }
        }

        /// <summary>
        /// Notify the device of a DisplayLineItem being removed from a DisplayOrder
        /// </summary>
        /// <param name="order"></param>
        /// <param name="lineItem"></param>
        public void LineItemRemovedFromDisplayOrder(DisplayOrder order, DisplayLineItem lineItem)
        {
            if (order == null) { OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In LineItemRemovedFromDisplayOrder : DisplayOrder object cannot be null. ")); }
            if (order.id == null) { OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In LineItemRemovedFromDisplayOrder : DisplayOrder id cannot be null. " + order)); }
            if (lineItem == null) { OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In LineItemRemovedFromDisplayOrder : DisplayLineItem object cannot be null. ")); }
            LineItemsDeletedOperation lido = new LineItemsDeletedOperation();
            lido.orderId = order.id;
            lido.addId(lineItem.id);

            if (Device != null)
            {
                Device.doOrderUpdate(order, lido);
            }
        }

        /// <summary>
        /// Notify device of a discount being added to the order. 
        /// Note: This is independent of a discount being added to a display line item.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="discount"></param>
        public void DiscountAddedToDisplayOrder(DisplayOrder order, DisplayDiscount discount)
        {
            if (order == null) { OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In DiscountAddedToDisplayOrder : DisplayOrder object cannot be null. ")); }
            if (order.id == null) { OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In DiscountAddedToDisplayOrder : DisplayOrder id cannot be null. " + order)); }
            if (discount == null) { OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "In DiscountAddedToDisplayOrder : DisplayLineItem object cannot be null. ")); }
            DiscountsAddedOperation dao = new DiscountsAddedOperation();
            dao.orderId = order.id;
            dao.addId(discount.id);

            if (Device != null)
            {
                Device.doOrderUpdate(order, dao);
            }
        }

        /// <summary>
        /// Notify the device that a discount was removed from the order.
        /// Note: This is independent of a discount being removed from a display line item.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="discount"></param>
        public void DiscountRemovedFromDisplayOrder(DisplayOrder order, DisplayDiscount discount)
        {
            DiscountsDeletedOperation dao = new DiscountsDeletedOperation();
            dao.orderId = order.id;
            dao.addId(discount.id);

            if (Device != null)
            {
                Device.doOrderUpdate(order, dao);
            }
        }

        /// <summary>
        /// Remove the DisplayOrder from the device.
        /// </summary>
        /// <param name="order"></param>
        public void RemoveDisplayOrder(DisplayOrder order)
        {
            OrderDeletedOperation dao = new OrderDeletedOperation();
            dao.id = order.id;
            if (Device != null)
            {
                Device.doOrderUpdate(order, dao);
            }
            ShowWelcomeScreen();
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
            if (Device != null)
            {
                Device.doKeyPress(io.keyPress);
            }
        }

        public void PrintImageFromURL(string ImgURL)
        {
            Device.doPrintImageURL(ImgURL);
        }

        public T GetEnumFromString<T>(string stringValue, bool isCaseInsensitive = false) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.EXCEPTION, 0, "ArgumentException: T must be an enumerated type"));
                return default(T);
            }
            return (T)Enum.Parse(typeof(T), stringValue, isCaseInsensitive);
        }

        public void OnDeviceError(CloverDeviceErrorEvent ee)
        {
            listeners.ForEach(listener => listener.OnDeviceError(ee));
        }


        private class InnerDeviceObserver : ICloverDeviceObserver
        {
            public RefundPaymentResponse lastPRR;
            private CloverDeviceEvent lastStartEvent = null;
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
                this.cloverConnector = cc;

            }
            public void onDeviceConnected()
            {
                cloverConnector.listeners.ForEach(listener => listener.OnDeviceConnected());
            }
            public void onDeviceDisconnected()
            {
                cloverConnector.listeners.ForEach(listener => listener.OnDeviceDisconnected());
            }
            public void onDeviceReady(DiscoveryResponseMessage drm)
            {
                this.cloverConnector.merchantInfo = new MerchantInfo(drm);
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);
                cloverConnector.listeners.ForEach(listener => listener.OnDeviceReady(this.cloverConnector.merchantInfo));
            }
            public void onDeviceError(int code, string message)
            {
                cloverConnector.listeners.ForEach(listener => listener.OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.COMMUNICATION_ERROR, code, message)));
            }

            public void onTxState(TxState txState)
            {
                //Console.WriteLine("onTxTstate: " + txState.ToString());
            }

            public void onTipAdded(long tip)
            {
                cloverConnector.listeners.ForEach(listener => listener.OnTipAdded(new TipAddedMessage(tip)));
            }

            public void onPartialAuth(long amount)
            {
                // not implemented yet
            }

            public void onAuthTipAdjusted(string paymentId,
                                          long tipAmount,
                                          bool success)

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
                cloverConnector.listeners.ForEach(listener => listener.OnTipAdjustAuthResponse(taar));
            }

            public void onAuthTipAdjusted(ResponseCode responseCode,
                                          String reason = null,
                                          String message = null)

            {
                TipAdjustAuthResponse taar = new TipAdjustAuthResponse();
                taar.PaymentId = null;
                taar.TipAmount = 0;
                taar.Success = responseCode == ResponseCode.SUCCESS;
                taar.Result = responseCode;
                taar.Reason = reason;
                taar.Message = message;
                cloverConnector.listeners.ForEach(listener => listener.OnTipAdjustAuthResponse(taar));
            }

            public void onCashbackSelected(long cashbackAmount)
            {
                //TODO: Implement
                cloverConnector.ShowOnDevice(msg: "CloverConnector.onCashbackSelected() is not yet implemented!!!!");
            }

            public void onKeyPressed(KeyPress keyPress)
            {
                //TODO: Implement
                cloverConnector.ShowOnDevice(msg: "CloverConnector.onKeyPressed() is not yet implemented!!!!");
            }

            public void onRefundPaymentResponse(Refund refund, String orderId, String paymentId, TxState code, string msg)
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

                cloverConnector.listeners.ForEach(listener => listener.OnCloseoutResponse(cr));
            }

            public void onUiState(UiState uiState, String uiText, UiDirection uiDirection, params InputOption[] inputOptions)
            {
                CloverDeviceEvent deviceEvent = new CloverDeviceEvent();
                deviceEvent.InputOptions = inputOptions;
                deviceEvent.EventState = (CloverDeviceEvent.DeviceEventState)Enum.Parse(typeof(CloverDeviceEvent.DeviceEventState), uiState.ToString());
                deviceEvent.Message = uiText;
                if (uiDirection == UiDirection.ENTER)
                {
                    lastStartEvent = deviceEvent;
                    cloverConnector.listeners.ForEach(listener => listener.OnDeviceActivityStart(deviceEvent));
                }
                else if (uiDirection == UiDirection.EXIT)
                {
                    // because we can get events out of order, need to make sure we don't wipe out the wrong Options
                    if (lastStartEvent != null && lastStartEvent.EventState.Equals(deviceEvent.EventState))
                    {
                        cloverConnector.listeners.ForEach(listener => listener.OnDeviceActivityEnd(deviceEvent));
                        lastStartEvent = null;
                    }
                }
            }

            public void onFinishOk(Payment payment, Signature2 signature2)
            {
                if (cloverConnector.lastRequest is PreAuthRequest)
                {
                    PreAuthResponse response = new PreAuthResponse();
                    response.Success = true;
                    response.Result = ResponseCode.SUCCESS;
                    response.Payment = payment;
                    response.Signature = signature2;
                    cloverConnector.listeners.ForEach(listener => listener.OnPreAuthResponse(response));
                    cloverConnector.lastRequest = null;
                }
                else if (cloverConnector.lastRequest is AuthRequest)
                {
                    AuthResponse response = new AuthResponse();
                    response.Success = true;
                    response.Result = ResponseCode.SUCCESS;
                    response.Payment = payment;
                    response.Signature = signature2;
                    cloverConnector.listeners.ForEach(listener => listener.OnAuthResponse(response));
                    cloverConnector.lastRequest = null;
                }
                else if (cloverConnector.lastRequest is SaleRequest)
                {
                    SaleResponse response = new SaleResponse();
                    response.Success = true;
                    response.Result = ResponseCode.SUCCESS;
                    response.Payment = payment;
                    response.Signature = signature2;
                    cloverConnector.listeners.ForEach(listener => listener.OnSaleResponse(response));
                    cloverConnector.lastRequest = null;
                }
                else if (cloverConnector.lastRequest == null)
                {
                    cloverConnector.ShowOnDevice(showWelcomeScreen: true);
                    return;
                }
                else
                {
                    cloverConnector.OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, "Failed to pair this response. " + payment));
                }
                cloverConnector.ShowOnDevice(showThankYouScreen: true,
                                             milliSecondsTimeoutForThankYou: 3000,
                                             showWelcomeScreen: true);
            }

            public void onFinishOk(Credit credit)
            {
                ManualRefundResponse response = new ManualRefundResponse();
                response.Success = true;
                response.Result = ResponseCode.SUCCESS;
                response.Credit = credit;
                cloverConnector.listeners.ForEach(listener => listener.OnManualRefundResponse(response));
                cloverConnector.ShowOnDevice(showThankYouScreen: true,
                                             milliSecondsTimeoutForThankYou: 3000,
                                             showWelcomeScreen: true);
                cloverConnector.lastRequest = null;
            }

            public void onFinishOk(Refund refund)
            {
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);

                cloverConnector.lastRequest = null;
                RefundPaymentResponse lastRefundResponse = lastPRR;
                lastPRR = null;

                if (lastRefundResponse != null)
                {
                    if (lastRefundResponse.Refund.id.Equals(refund.id))
                    {
                        cloverConnector.listeners.ForEach(listener => listener.OnRefundPaymentResponse(lastRefundResponse));
                    }
                    else
                    {
                        Console.WriteLine("The last PaymentRefundResponse has a different ID than this refund in finishOk");
                    }
                }
                else
                {
                    Console.WriteLine("Shouldn't get an onFinishOk without having gotten an onPaymentRefund!");
                }
            }

            public void onFinishCancel()
            {
                onFinishCancel(ResponseCode.CANCEL, null, null);
            }

            public void onFinishCancel(ResponseCode result = ResponseCode.CANCEL, String reason = null, String message = null)
            {
                if (cloverConnector.lastRequest is PreAuthRequest)
                {
                    PreAuthResponse response = new PreAuthResponse();
                    response.Success = false;
                    response.Result = result;
                    response.Reason = reason != null ? reason : "Request Cancelled";
                    response.Message = message != null ? message : "PreAuth Request cancelled by user";
                    cloverConnector.listeners.ForEach(listener => listener.OnPreAuthResponse(response));
                    cloverConnector.lastRequest = null;
                }
                else if (cloverConnector.lastRequest is AuthRequest)
                {
                    AuthResponse response = new AuthResponse();
                    response.Success = false;
                    response.Result = result;
                    response.Reason = reason != null ? reason : "Request Cancelled";
                    response.Message = message != null ? message : "Auth Request cancelled by user";
                    cloverConnector.listeners.ForEach(listener => listener.OnAuthResponse(response));
                    cloverConnector.lastRequest = null;
                }
                else if (cloverConnector.lastRequest is SaleRequest)
                {
                    SaleResponse response = new SaleResponse();
                    response.Payment = null;
                    response.Success = false;
                    response.Result = result;
                    response.Reason = reason != null ? reason : "Request Cancelled";
                    response.Message = message != null ? message : "Sale Request cancelled by user";
                    cloverConnector.listeners.ForEach(listener => listener.OnSaleResponse(response));
                    cloverConnector.lastRequest = null;
                }
                else if (cloverConnector.lastRequest is ManualRefundRequest)
                {
                    cloverConnector.lastRequest = null;
                    ManualRefundResponse response = new ManualRefundResponse();
                    response.Success = false;
                    response.Result = result;
                    response.Reason = reason != null ? reason : "Request Cancelled";
                    response.Message = message != null ? message : "Manual Refund Request cancelled by user";
                    cloverConnector.listeners.ForEach(listener => listener.OnManualRefundResponse(response));
                    cloverConnector.lastRequest = null;
                }
                else if (lastPRR is RefundPaymentResponse)
                {
                    cloverConnector.listeners.ForEach(listener => listener.OnRefundPaymentResponse(lastPRR));
                    lastPRR = null;
                }
            }

            public void onVerifySignature(Payment payment, Signature2 signature)
            {
                SVR request = new SVR(cloverConnector.Device);
                request.Signature = signature;
                request.Payment = payment;
                cloverConnector.listeners.ForEach(listener => listener.OnVerifySignatureRequest(request));
            }

            public void onPaymentVoided(Payment payment, VoidReason reason)
            {
                VoidPaymentResponse response = new VoidPaymentResponse();
                response.Success = true;
                response.Result = ResponseCode.SUCCESS;
                response.Reason = reason.ToString();
                response.PaymentId = payment.id;

                cloverConnector.listeners.ForEach(listener => listener.OnVoidPaymentResponse(response));
                cloverConnector.ShowOnDevice(msg: "The transaction was voided.",
                                             milliSecondsTimeoutForMsg: 3000,
                                             showThankYouScreen: true,
                                             milliSecondsTimeoutForThankYou: 3000,
                                             showWelcomeScreen: true);
            }

            //For Error/Fail scenarios that don't have a Payment object to send back
            public void onPaymentVoided(ResponseCode result, String reason = null, String message = null)
            {
                VoidPaymentResponse response = new VoidPaymentResponse();
                response.Success = false;
                response.Result = result;
                response.Reason = reason != null ? reason : result.ToString();
                response.Message = message != null ? message : "No extended information provided.";
                response.PaymentId = null;
                cloverConnector.listeners.ForEach(listener => listener.OnVoidPaymentResponse(response));
            }

            public void onVaultCardResponse(VaultCardResponseMessage vcrm)
            {
                VaultCardResponse vcr = new VaultCardResponse();
                vcr.Card = vcrm.card;
                vcr.Success = vcrm.status == ResultStatus.SUCCESS;
                vcr.Reason = vcrm.reason;
                cloverConnector.listeners.ForEach(listener => listener.OnVaultCardResponse(vcr));
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);
            }

            //For Error/Fail scenarios where a valid Card object does not exist
            public void onVaultCardResponse(ResponseCode result, String reason = null, String message = null)
            {
                VaultCardResponse vcr = new VaultCardResponse();
                vcr.Card = null;
                vcr.Success = false;
                vcr.Reason = reason;
                vcr.Message = message;
                cloverConnector.listeners.ForEach(listener => listener.OnVaultCardResponse(vcr));
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);
            }

            public void onCapturePreAuthResponse(String paymentId,
                                                 long amount,
                                                 long tipAmount, 
                                                 ResultStatus status,
                                                 string reason)
            {
                CapturePreAuthResponse car = new CapturePreAuthResponse();
                car.Success = status.Equals(ResultStatus.SUCCESS);
                car.Result = car.Success ? ResponseCode.SUCCESS : ResponseCode.FAIL;
                car.Reason = reason;
                car.PaymentId = paymentId;
                car.Amount = amount;
                car.TipAmount = tipAmount;
                cloverConnector.listeners.ForEach(listener => listener.OnCapturePreAuthResponse(car));
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);
            }

            //For Error/Fail scenarios where a payment was never processed
            public void onCapturePreAuthResponse(ResponseCode responseCode,
                                                 String reason = null,
                                                 String message = null)
            {
                CapturePreAuthResponse car = new CapturePreAuthResponse();
                car.Success = responseCode == ResponseCode.SUCCESS;
                car.Result = responseCode;
                car.Reason = reason != null ? reason : responseCode.ToString();
                car.Message = message != null ? message : "No extended information provided.";
                car.PaymentId = null;
                car.Amount = 0;
                car.TipAmount = 0;
                cloverConnector.listeners.ForEach(listener => listener.OnCapturePreAuthResponse(car));
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);
            }

            public void onTxStartResponse(TxStartResponseResult result, string externalId)
            {
                bool success = result.Equals(TxStartResponseResult.SUCCESS) ? true : false;
                if (success)
                {
                    return;  //move it along folks, nothing to see here
                }
                bool duplicate = result.Equals(TxStartResponseResult.DUPLICATE);
                try
                {

                    if (cloverConnector.lastRequest is PreAuthRequest)
                    {
                        PreAuthResponse response = new PreAuthResponse();
                        if (duplicate)
                        {
                            response.Result = ResponseCode.CANCEL;
                            response.Reason = result.ToString();
                            response.Message = "The provided transaction id of " + externalId + " has already been processed and cannot be resubmitted.";
                        }
                        else
                        {
                            response.Result = ResponseCode.FAIL;
                            response.Reason = result.ToString();
                        }
                        cloverConnector.listeners.ForEach(listener => listener.OnPreAuthResponse(response));
                    }
                    else if (cloverConnector.lastRequest is AuthRequest)
                    {
                        AuthResponse response = new AuthResponse();
                        if (duplicate)
                        {
                            response.Result = ResponseCode.CANCEL;
                            response.Reason = result.ToString();
                            response.Message = "The provided transaction id of " + externalId + " has already been processed and cannot be resubmitted.";
                        }
                        else
                        {
                            response.Result = ResponseCode.FAIL;
                            response.Reason = result.ToString();
                        }
                        cloverConnector.listeners.ForEach(listener => listener.OnAuthResponse(response));
                    }
                    else if (cloverConnector.lastRequest is SaleRequest)
                    {
                        SaleResponse response = new SaleResponse();
                        if (duplicate)
                        {
                            response.Result = ResponseCode.CANCEL;
                            response.Reason = result.ToString();
                            response.Message = "The provided transaction id of " + externalId + " has already been processed and cannot be resubmitted.";
                        }
                        else
                        {
                            response.Result = ResponseCode.FAIL;
                            response.Reason = result.ToString();
                        }
                        cloverConnector.listeners.ForEach(listener => listener.OnSaleResponse(response));
                    }
                    else if (cloverConnector.lastRequest is ManualRefundRequest)
                    {
                        ManualRefundResponse response = new ManualRefundResponse();
                        if (duplicate)
                        {
                            response.Result = ResponseCode.CANCEL;
                            response.Reason = result.ToString();
                            response.Message = "The provided transaction id of " + externalId + " has already been processed and cannot be resubmitted.";
                        }
                        else
                        {
                            response.Result = ResponseCode.FAIL;
                            response.Reason = result.ToString();
                        }
                        cloverConnector.listeners.ForEach(listener => listener.OnManualRefundResponse(response));
                    }
                }
                finally
                {
                    cloverConnector.lastRequest = null;
                }
            }
        }
    }
}
