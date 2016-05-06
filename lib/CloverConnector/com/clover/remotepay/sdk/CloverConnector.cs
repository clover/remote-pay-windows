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

        List<CloverConnectorListener> listeners = new List<CloverConnectorListener>();

        private MerchantInfo merchantInfo { get; set; }
        private InnerDeviceObserver deviceObserver { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CardEntryMethod
        {
            get; set;
        }


        /// <summary>
        /// set to true to disable printing on the Clover Mini
        /// </summary>
        public bool DisablePrinting { get; set; }

        /// <summary>
        /// set to true to disable cashback on the Clover Mini
        /// </summary>
        public bool DisableCashBack { get; set; }

        /// <summary>
        /// set to true to disable tip on the Clover Mini
        /// </summary>
        public bool DisableTip { get; set; }

        /// <summary>
        /// set to true, so when a transaction fails the Clover Mini returns to the welcome screen,
        /// otherwise it restarts the payment transaction
        /// </summary>
        public bool DisableRestartTransactionOnFail { get; set; }

        /// <summary>
        /// set to true, so that when request responses are processed, the Clover Mini
        /// won't show default messages/ThankYou/Welcome screens.  Set to false
        /// to allow the default message flow that is built into the CloverConnector.
        /// </summary>
        public bool DisableDefaultDeviceScreenFlow { get; set; }
        
        //keep a last request
        private Object lastRequest;

        bool? AllowOfflinePayment { get; set; }

        bool? ApproveOfflinePaymentWithoutPrompt { get; set; }

        /// <summary>
        /// overload the + operator to add a new listener
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="connectorListener"></param>
        /// <returns></returns>
        public static CloverConnector operator +(CloverConnector connector, CloverConnectorListener connectorListener)
        {
            connector.AddCloverConnectorListener(connectorListener);
            return connector;
        }

        public void AddCloverConnectorListener(CloverConnectorListener connectorListener)
        {
            listeners.Add(connectorListener);
        }

        /// <summary>
        /// overload the - operator to remove a listener
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="connectorListener"></param>
        public static CloverConnector operator -(CloverConnector connector, CloverConnectorListener connectorListener)
        {
            connector.RemoveCloverConnectorListener(connectorListener);

            return connector;
        }

        public void RemoveCloverConnectorListener(CloverConnectorListener connectorListener)
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
            Initialize(config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="connectorListener"></param>
        public CloverConnector(CloverDeviceConfiguration config, CloverConnectorListener connectorListener) : this()
        {
            AddCloverConnectorListener(connectorListener);
            Initialize(config);
        }

        /// <summary>
        /// Initialize the connector with a given configuration
        /// </summary>
        /// <param name="config">A CloverDeviceConfiguration object; TestDeviceConfiguration can be used for testing</param>
        public void Initialize(CloverDeviceConfiguration config) {
            merchantInfo = new MerchantInfo();
            deviceObserver = new InnerDeviceObserver(this);

            DisableCashBack = false;
            DisablePrinting = false;
            DisableRestartTransactionOnFail = false;
            DisableTip = false;
            DisableDefaultDeviceScreenFlow = false;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    Device = CloverDeviceFactory.Get(config);
                    Device.Subscribe(deviceObserver);
                });
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// Sale method, aka "purchase"
        /// </summary>
        /// <param name="request">A SaleRequest object containing basic information needed for the transaction</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
        public int Sale(SaleRequest request)
        {
            //payment, finishOK(payment), finishCancel, onPaymentVoided
            if(Device == null)
            {
                return -1;
            }
            if (request == null) { throw new InvalidDataException("In Sale : SaleRequest - Object cannot be null. "); }
            if (request.Amount == 0) { throw new InvalidDataException("In Sale : SaleRequest - Amount cannot be zero. " + request); }
            if (request.VaultedCard != null && !merchantInfo.supportsVaultCards)
            {
                deviceObserver.onConfigError("In Sale : SaleRequest - Vault Card support is not offered by the merchant configured gateway. " + request);
                return -1;
            }
            lastRequest = request;
            PayIntent payIntent = new PayIntent();
            payIntent.transactionType = PayIntent.TransactionType.PAYMENT;
            payIntent.remotePrint = DisablePrinting;
            payIntent.isDisableCashBack = DisableCashBack;
            payIntent.cardEntryMethods = request.CardEntryMethod.HasValue ? request.CardEntryMethod.Value : CardEntryMethod;
            payIntent.amount = request.Amount;
            payIntent.tipAmount = request.TipAmount.HasValue ? request.TipAmount.Value : 0;
            payIntent.taxAmount = request.TaxAmount;
            payIntent.tippableAmount = request.TippableAmount;
            payIntent.vaultedCard = request.VaultedCard;
            payIntent.isCardNotPresent = request.CardNotPresent;
            payIntent.allowOfflinePayment = request.AllowOfflinePayment == null ? AllowOfflinePayment: request.AllowOfflinePayment;
            payIntent.approveOfflinePaymentWithoutPrompt = request.ApproveOfflinePaymentWithoutPrompt == null ? ApproveOfflinePaymentWithoutPrompt : request.ApproveOfflinePaymentWithoutPrompt;
            Device.doTxStart(payIntent, null, false);
            return 0;
        }

        /// <summary>
        /// If signature is captured during a Sale, this method accepts the signature as entered
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int AcceptSignature(SignatureVerifyRequest request)
        {
            if (request == null) { throw new InvalidDataException("In AcceptSignature : SignatureVerifyRequest object cannot be null. "); }
            Device.doSignatureVerified(request.Payment, true);
            return 0;
        }

        /// <summary>
        /// If signature is captured during a Sale, this method rejects the signature as entered
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int RejectSignature(SignatureVerifyRequest request)
        {
            if (request == null) { throw new InvalidDataException("In RejectSignature : SignatureVerifyRequest object cannot be null. "); }
            Device.doSignatureVerified(request.Payment, false);
            return 0;
        }

        /// <summary>
        /// Auth method to obtain an Auth.  While a Pre-Auth can also be accomplished
        /// by setting the IsPreAuth flag to true, the PreAuthRequest is the 
        /// preferred request type.  PreAuth functionality was retained for backward
        /// compatibility
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int Auth(AuthRequest request)
        {
            if (Device == null)
            {
                return -1;
            }
            if (request == null) { throw new InvalidDataException("In Auth : AuthRequest - Object cannot be null. "); }
            if (request.Amount == 0) { throw new InvalidDataException("In Auth : AuthRequest - Amount cannot be zero. " + request); }
            if (request.VaultedCard != null && !merchantInfo.supportsVaultCards) { throw new InvalidDataException("In Auth : AuthRequest - Vault Card support is not offered by the merchant configured gateway. " + request); }
            lastRequest = request;
            PayIntent payIntent = new PayIntent();
            payIntent.transactionType = request.IsPreAuth ? PayIntent.TransactionType.AUTH : PayIntent.TransactionType.PAYMENT;
            payIntent.remotePrint = DisablePrinting;
            payIntent.isDisableCashBack = DisableCashBack;
            payIntent.cardEntryMethods = request.CardEntryMethod.HasValue ? request.CardEntryMethod.Value : CardEntryMethod;
            payIntent.vaultedCard = request.VaultedCard;
            payIntent.amount = request.Amount;
            payIntent.tipAmount = null; // have to force this to null until PayIntent honors transactionType of AUTH
            payIntent.tippableAmount = request.TippableAmount;
            payIntent.isCardNotPresent = request.CardNotPresent;
            payIntent.allowOfflinePayment = request.AllowOfflinePayment == null ? AllowOfflinePayment : request.AllowOfflinePayment;
            payIntent.approveOfflinePaymentWithoutPrompt = request.ApproveOfflinePaymentWithoutPrompt == null ? ApproveOfflinePaymentWithoutPrompt : request.ApproveOfflinePaymentWithoutPrompt;
            Device.doTxStart(payIntent, null, true);
            return 0;
        }

        /// <summary>
        /// PreAuth method to obtain a PreAuth.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int PreAuth(PreAuthRequest request)
        {
            if (Device == null)
            {
                return -1;
            }
            if (request == null) { throw new InvalidDataException("In PreAuth : PreAuthRequest - Object cannot be null. "); }
            if (request.Amount == 0) { throw new InvalidDataException("In PreAuth : PreAuthRequest - Amount cannot be zero. " + request); }
            if (request.VaultedCard != null && !merchantInfo.supportsVaultCards) { throw new InvalidDataException("In PreAuth : PreAuthRequest - Vault Card support is not offered by the merchant configured gateway. " + request); }
            lastRequest = request;
            PayIntent payIntent = new PayIntent();
            payIntent.transactionType = PayIntent.TransactionType.AUTH;
            payIntent.remotePrint = DisablePrinting;
            payIntent.isDisableCashBack = DisableCashBack;
            payIntent.cardEntryMethods = request.CardEntryMethod.HasValue ? request.CardEntryMethod.Value : CardEntryMethod;
            payIntent.vaultedCard = request.VaultedCard;
            payIntent.amount = request.Amount;
            payIntent.tipAmount = null; // have to force this to null until PayIntent honors transactionType of AUTH
            payIntent.isCardNotPresent = request.CardNotPresent;
            Device.doTxStart(payIntent, null, true);
            return 0;
        }

        /// <summary>
        /// Capture a previous Auth. Note: Should only be called if request's PaymentID is from an AuthResponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int CaptureAuth(CaptureAuthRequest request)
        {
            if(Device == null)
            {
                return -1;
            }
            if (request != null)
            {
                if (request.TipAmount >= 0 && !merchantInfo.supportsPreAuths)
                {
                    deviceObserver.onConfigError("In CaptureAuth : CaptureAuthRequest - Capture Auth is not supported by the merchant configured gateway. ");
                    return -1;
                }
                Device.doCaptureAuth(request.PaymentID, request.Amount, request.TipAmount);
                return 0;
            }
            return -1;
        }

        /// <summary>
        /// Adjust the tip for a previous Auth. Note: Should only be called if request's PaymentID is from an AuthResponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int TipAdjustAuth(TipAdjustAuthRequest request)
        {
            if (Device == null)
            {
                return -1;
            }
            if (request == null) { throw new InvalidDataException("In TipAdjustAuth : TipAdjustAuthRequest object cannot be null. "); }
            if (request.PaymentID == null) { throw new InvalidDataException("In TipAdjustAuth : TipAdjustAuthRequest PaymentID cannot be null. " + request); }
            if (!(request.TipAmount >= 0)) { throw new InvalidDataException("In TipAdjustAuth : TipAdjustAuthRequest TipAmount cannot be less than zero. " + request); }
            if (!merchantInfo.supportsTipAdjust)
            {
                deviceObserver.onConfigError("In TipAdjustAuth : TipAdjustAuthRequest - Tip Adjust is not supported by the merchant configured gateway. " + request);
                return -1;
            }
            lastRequest = request;
            Device.doTipAdjustAuth(request.OrderID, request.PaymentID, request.TipAmount);
            return 0;
        }

        /// <summary>
        /// Void a transaction, given a previously used order ID and/or payment ID
        /// TBD - defining a payment or order ID to be used with a void without requiring a response from Sale()
        /// </summary>
        /// <param name="request">A VoidRequest object containing basic information needed to void the transaction</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
        public int VoidPayment(VoidPaymentRequest request) // SaleResponse is a Transaction? or create a Transaction from a SaleResponse
        {
            if (Device == null)
            {
                return -1;
            }
            if (request == null) { throw new InvalidDataException("In VoidPayment : VoidPaymentRequest object cannot be null. "); }
            if (request.PaymentId == null) { throw new InvalidDataException("In VoidPayment : VoidPaymentRequest PaymentId cannot be null. " + request); }
            lastRequest = request;
            Payment payment = new Payment();
            payment.id = request.PaymentId;
            payment.order = new Reference();
            payment.order.id = request.OrderId;
            payment.employee = new Reference();
            payment.employee.id = request.EmployeeId;
            VoidReason reason = (VoidReason)Enum.Parse(typeof(VoidReason), request.VoidReason, true);
            Device.doVoidPayment(payment, reason);
            return 0;
        }

        /// <summary>
        /// called when requesting a payment be voided when only the request UUID is available
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int VoidTransaction(VoidTransactionRequest request)
        {
            return 0;
        }

        /// <summary>
        /// Refund a specific payment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int RefundPayment(RefundPaymentRequest request)
        {
            if (Device == null)
            {
                return -1;
            }
            if (request == null) { throw new InvalidDataException("In RefundPayment : RefundPaymentRequest object cannot be null. "); }
            if (request.PaymentId == null) { throw new InvalidDataException("In RefundPayment : RefundPaymentRequest PaymentId cannot be null. " + request); }
            if (request.Amount < 0) { throw new InvalidDataException("In RefundPayment : RefundPaymentRequest Amount cannot be a negative amount. " + request); }
            lastRequest = request;
            Device.doPaymentRefund(request.OrderId, request.PaymentId, request.Amount);
            return 0;
        }

        /// <summary>
        /// Manual refund method, aka "naked credit"
        /// </summary>
        /// <param name="request">A ManualRefundRequest object</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
        public int ManualRefund(ManualRefundRequest request) // NakedRefund is a Transaction, with just negative amount
        {
            //payment, finishOK(credit), finishCancel, onPaymentVoided
            if (Device == null)
            {
                return -1;
            }
            if (request == null) { throw new InvalidDataException("In ManualRefund : ManualRefundRequest - Object cannot be null. "); }
            if (request.Amount == 0) { throw new InvalidDataException("In ManualRefund : ManualRefundRequest - Amount cannot be null. " + request); }
            if (!merchantInfo.supportsManualRefunds)
            {
                deviceObserver.onConfigError("In ManualRefund : ManualRefundRequest - Manual refunds are not supported by the merchant configured gateway.");
                return -1;
            }
            lastRequest = request;
            PayIntent payIntent = new PayIntent();
            payIntent.cardEntryMethods = request.CardEntryMethod.HasValue ? request.CardEntryMethod.Value : CardEntryMethod;
            payIntent.amount = -Math.Abs(request.Amount);
            payIntent.transactionType = PayIntent.TransactionType.CREDIT;
            Device.doTxStart(payIntent, null, true);
            return 0;
        }

        /*public int SendCancel(SendCancelRequest request)
        {
            return 0;
        }*/

        /// <summary>
        /// Send a request to the server to closeout all orders.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int Closeout(CloseoutRequest request)
        {
            Device.doCloseout(request.allowOpenTabs, request.batchId); // TODO: pass in request UUID so it can be returned with CloseoutResponse
            return 0;
        }

        /// <summary>
        /// Send a request to the mini to reset.  This can be used if the device gets into a non-recoverable state.
        /// </summary>
        /// <returns></returns>
        public int ResetDevice()
        {
            Device.doResetDevice(); 
            return 0;
        }

        /// <summary>
        /// Cancels the device from waiting for payment card
        /// </summary>
        /// <returns></returns>
        public int Cancel()
        {
            InvokeInputOption(CANCEL_INPUT_OPTION);
            return 0;
        }

        /// <summary>
        /// Print simple lines of text to the Clover Mini printer
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public int PrintText(List<string> messages)
        {
            if(Device == null)
            {
                return -1;
            }
            if (messages == null) { throw new InvalidDataException("In PrintText : message list cannot be null. "); }

            Device.doPrintText(messages);
            return 0;
        }

        /// <summary>
        /// Print an image on the Clover Mini printer
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public int PrintImage(Bitmap bitmap) //Bitmap img
        {
            if (bitmap == null) { throw new InvalidDataException("In PrintImage : Bitmap object cannot be null. "); }
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            byte[] imgBytes = ms.ToArray();
            string base64Image = Convert.ToBase64String(imgBytes);

            Device.doPrintImage(base64Image);
            return 0;
        }

        /// <summary>
        /// Show a message on the Clover Mini screen
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public int ShowMessage(string message)
        {
            return ShowOnDevice(message);
        }

        /// <summary>
        /// Return the device to the Welcome Screen
        /// </summary>
        public int ShowWelcomeScreen()
        {
            return ShowOnDevice(showWelcomeScreen: true);
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
        private int ShowOnDevice(string msg = null, Int32 milliSecondsTimeoutForMsg = 0, bool showThankYouScreen = false, Int32 milliSecondsTimeoutForThankYou = 0, bool showWelcomeScreen = false)
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
                return 0;
            } else
            {
                return -1;
            }
        }

        /// <summary>
        /// Vault Card information and payment token
        /// </summary>
        public int VaultCard(int? CardEntryMethods)
        {
            if (Device == null)
            {
                return -1;
            }

            if (!merchantInfo.supportsVaultCards)
            {
                deviceObserver.onConfigError("In VaultCard : Vaulting a card is not supported by the merchant configured gateway.");
                return -1;
            }

            Device.doVaultCard(CardEntryMethods.HasValue ? CardEntryMethods.Value : CardEntryMethod);
            return 0;
        }

        /// <summary>
        /// Show the customer facing receipt option screen for the specified Payment.
        /// </summary>
        public void DisplayPaymentReceiptOptions(string orderId, string paymentId)
        {
            if(Device != null)
            {
                Device.doShowPaymentReceiptScreen(orderId, paymentId);
            }
        }

        /// <summary>
        /// Show the customer facing receipt option screen for the specified Refund.
        /// </summary>
        public void DisplayRefundReceiptOptions(string orderId, string refundId)
        {
            if (Device != null)
            {
                Device.doShowRefundReceiptScreen(orderId, refundId);
            }
        }

        /// <summary>
        /// Show the customer facing receipt option screen for the specified Credit.
        /// </summary>
        public void DisplayCreditReceiptOptions(string orderId, string creditId)
        {
            if (Device != null)
            {
                Device.doShowCreditReceiptScreen(orderId, creditId);
            }
        }

        /// <summary>
        /// Will trigger cash drawer to open that is connected to Clover Mini
        /// </summary>
        public void OpenCashDrawer(String reason)
        {
            if(Device != null)
            {
                if (reason == null) { throw new InvalidDataException("In OpenCashDrawer : Reason string cannot be null. "); }
                Device.doOpenCashDrawer(reason);
            }
        }

        /// <summary>
        /// Show the DisplayOrder on the device. Replaces the existing DisplayOrder on the device.
        /// </summary>
        /// <param name="order"></param>
        public void DisplayOrder(DisplayOrder order)
        {
            if(Device != null)
            {
                if (order == null) { throw new InvalidDataException("In DisplayOrder : DisplayOrder object cannot be null. "); }
                Device.doOrderUpdate(order, null);
            }
        }

        /// <summary>
        /// Notify the device of a DisplayLineItem being added to a DisplayOrder
        /// </summary>
        /// <param name="order"></param>
        /// <param name="lineItem"></param>
        public void DisplayOrderLineItemAdded(DisplayOrder order, DisplayLineItem lineItem)
        {
            if (order == null) { throw new InvalidDataException("In DisplayOrderLineItemAdded : DisplayOrder object cannot be null. "); }
            if (order.id == null) { throw new InvalidDataException("In DisplayOrderLineItemAdded : DisplayOrder id cannot be null. " + order); }
            if (lineItem == null) { throw new InvalidDataException("In DisplayOrderLineItemAdded : DisplayLineItem object cannot be null. "); }
            LineItemsAddedOperation liao = new LineItemsAddedOperation();
            liao.orderId = order.id;
            liao.addId(lineItem.id);  
            if(Device != null)
            {
                Device.doOrderUpdate(order, liao);
            }
        }

        /// <summary>
        /// Notify the device of a DisplayLineItem being removed from a DisplayOrder
        /// </summary>
        /// <param name="order"></param>
        /// <param name="lineItem"></param>
        public void DisplayOrderLineItemRemoved(DisplayOrder order, DisplayLineItem lineItem)
        {
            if (order == null) { throw new InvalidDataException("In DisplayOrderLineItemRemoved : DisplayOrder object cannot be null. "); }
            if (order.id == null) { throw new InvalidDataException("In DisplayOrderLineItemRemoved : DisplayOrder id cannot be null. " + order); }
            if (lineItem == null) { throw new InvalidDataException("In DisplayOrderLineItemRemoved : DisplayLineItem object cannot be null. "); }
            LineItemsDeletedOperation lido = new LineItemsDeletedOperation();
            lido.orderId = order.id;
            lido.addId(lineItem.id);

            if(Device != null)
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
        public void DisplayOrderDiscountAdded(DisplayOrder order, DisplayDiscount discount)
        {
            if (order == null) { throw new InvalidDataException("In DisplayOrderDiscountAdded : DisplayOrder object cannot be null. "); }
            if (order.id == null) { throw new InvalidDataException("In DisplayOrderDiscountAdded : DisplayOrder id cannot be null. " + order); }
            if (discount == null) { throw new InvalidDataException("In DisplayOrderDiscountAdded : DisplayLineItem object cannot be null. "); }
            DiscountsAddedOperation dao = new DiscountsAddedOperation();
            dao.orderId = order.id;
            dao.addId(discount.id);

            if(Device != null)
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
        public void DisplayOrderDiscountRemoved(DisplayOrder order, DisplayDiscount discount)
        {
            DiscountsDeletedOperation dao = new DiscountsDeletedOperation();
            dao.orderId = order.id;
            dao.addId(discount.id);

            if(Device != null)
            {
                Device.doOrderUpdate(order, dao);
            }
        }

        /// <summary>
        /// Remove the DisplayOrder from the device.
        /// </summary>
        /// <param name="order"></param>
        public void DisplayOrderDelete(DisplayOrder order)
        {
            OrderDeletedOperation dao = new OrderDeletedOperation();
            dao.id = order.id;
            if(Device != null)
            {
                Device.doOrderUpdate(order, dao);
            }
        }


        /// <summary>
        /// return the Merchant object for the Merchant configured for the Clover Mini
        /// </summary>
        /// <returns></returns>
        public void GetMerchantInfo()
        {

        }

        // TODO: should we call through, repurpose or remove?
        public void Dispose()
        {
            if(Device != null)
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
        

        private class InnerDeviceObserver : CloverDeviceObserver
        {
            private RefundPaymentResponse lastPRR;

            class SVR : SignatureVerifyRequest
            {
                CloverDevice _Device;

                public SVR(CloverDevice device)
                {
                    _Device = device;
                }

                public override void Accept()
                {
                    _Device.doSignatureVerified(Payment, true);
                }

                public override void Reject()
                {
                    _Device.doSignatureVerified(Payment, false);
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
                cloverConnector.listeners.ForEach(listener => listener.OnDeviceReady());

            }
            public void onDeviceError(int code, string message)
            {
                cloverConnector.listeners.ForEach(listener => listener.OnDeviceError(new CloverDeviceErrorEvent(code, message)));
            }

            public void onTxState(TxState txState)
            {
                //Console.WriteLine("onTxTstate: " + txState.ToString());
            }

            public void onPartialAuth(long partialAmount)
            {
                //TODO: Implement
                cloverConnector.ShowOnDevice(msg: "CloverConnector.onPartialAuth() is not yet implemented!!!!");
            }

            public void onTipAdded(long tip)
            {
                cloverConnector.listeners.ForEach(listener => listener.OnTipAdded(new TipAddedMessage(tip)));
            }

            public void onAuthTipAdjusted(string paymentId, long amount, bool success)
            {
                TipAdjustAuthResponse taar = new TipAdjustAuthResponse();
                taar.PaymentId = paymentId;
                taar.Amount = amount;
                taar.Success = success;
                cloverConnector.listeners.ForEach(listener => listener.OnAuthTipAdjustResponse(taar));
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

            public void onPaymentRefundResponse(string orderId, string paymentId, Refund refund, TxState code, string msg)
            {
                RefundPaymentResponse prr = new RefundPaymentResponse();
                prr.OrderId = orderId;
                prr.PaymentId = paymentId;
                prr.RefundObj = refund;
                prr.Code = code.ToString();
                prr.Message = msg;
                lastPRR = prr;
                cloverConnector.listeners.ForEach(listener => listener.OnRefundPaymentResponse(prr));
                if (code == TxState.SUCCESS)
                {
                    cloverConnector.ShowOnDevice(showThankYouScreen: true, 
                                                 milliSecondsTimeoutForThankYou: 3000, 
                                                 showWelcomeScreen: true);
                }
                
            }

            public void onCloseoutResponse(ResultStatus status, string reason, Batch batch)
            {
                CloseoutResponse cr = new CloseoutResponse();
                cr.Code = status.ToString();
                cr.status = status;
                cr.reason = reason;
                cr.batch = batch;

                cloverConnector.listeners.ForEach(listener => listener.OnCloseoutResponse(cr));
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);
            }

            public void onUiState(UiState uiState, String uiText, UiDirection uiDirection, params InputOption[] inputOptions)
            {
                CloverDeviceEvent deviceEvent = new CloverDeviceEvent();
                deviceEvent.InputOptions = inputOptions;
                deviceEvent.EventState = (CloverDeviceEvent.DeviceEventState)Enum.Parse(typeof(CloverDeviceEvent.DeviceEventState), uiState.ToString());
                deviceEvent.Message = uiText;
                if (uiDirection == UiDirection.ENTER)
                {
                    cloverConnector.listeners.ForEach(listener => listener.OnDeviceActivityStart(deviceEvent));
                }
                else if (uiDirection == UiDirection.EXIT)
                {
                    cloverConnector.listeners.ForEach(listener => listener.OnDeviceActivityEnd(deviceEvent));
                    if (deviceEvent.EventState == CloverDeviceEvent.DeviceEventState.RECEIPT_OPTIONS)
                    {
                        // this is a catch all, for situations where the 
                        // device displays the receipt options panel after
                        // the main processing is complete.  The pay display
                        // usually sets off the spinner after actions, so it
                        // is up to the caller to reset the display as desired.
                        cloverConnector.ShowOnDevice(showThankYouScreen: true, 
                                                     milliSecondsTimeoutForThankYou: 3000, 
                                                     showWelcomeScreen: true);
                    }
                }

            }

            public void onFinishOk(Payment payment, Signature2 signature2)
            {
                cloverConnector.ShowOnDevice(showThankYouScreen: true,
                                             milliSecondsTimeoutForThankYou: 3000,
                                             showWelcomeScreen: true);
                if (cloverConnector.lastRequest is PreAuthRequest)
                {
                    PreAuthResponse response = new PreAuthResponse();
                    response.Code = TransactionResponse.SUCCESS;
                    response.Payment = payment;
                    response.Signature = signature2;
                    cloverConnector.listeners.ForEach(listener => listener.OnPreAuthResponse(response));
                    cloverConnector.lastRequest = null;
                }
                else if (cloverConnector.lastRequest is AuthRequest)
                {
                    AuthResponse response = new AuthResponse();
                    response.Code = TransactionResponse.SUCCESS;
                    response.Payment = payment;
                    response.Signature = signature2;
                    cloverConnector.listeners.ForEach(listener => listener.OnAuthResponse(response));
                    cloverConnector.lastRequest = null;
                }
                else if (cloverConnector.lastRequest is SaleRequest)
                {
                    SaleResponse response = new SaleResponse();
                    response.Code = TransactionResponse.SUCCESS;
                    response.Payment = payment;
                    response.Signature = signature2;
                    cloverConnector.listeners.ForEach(listener => listener.OnSaleResponse(response));
                    cloverConnector.lastRequest = null;
                }
            }

            public void onFinishOk(Credit credit)
            {
                ManualRefundResponse response = new ManualRefundResponse();
                response.Code = TransactionResponse.SUCCESS;
                response.Credit = credit;
                cloverConnector.listeners.ForEach(listener => listener.OnManualRefundResponse(response));
                cloverConnector.ShowOnDevice(showThankYouScreen: true, 
                                             milliSecondsTimeoutForThankYou: 3000, 
                                             showWelcomeScreen: true);
            }

            public void onFinishOk(Refund refund) // this is currently unused.  See onPaymentRefundResponse()
            {
                return;  // ignore this message, as the PaymentRefundResponse() is doing the correct logic

                /*cloverConnector.lastRequest = null;
                if (lastPRR == null || !lastPRR.RefundObj.id.Equals(refund.id))
                {
                    lastPRR = new RefundPaymentResponse();
                    lastPRR.OrderId = (refund.orderRef != null) ? refund.orderRef.id : null;
                    lastPRR.PaymentId = (refund.payment != null) ? refund.payment.id : null;
                    lastPRR.RefundObj = refund;
                    lastPRR.Code = TxState.SUCCESS.ToString();
                    cloverConnector.listeners.ForEach(listener => listener.OnRefundPaymentResponse(lastPRR));
                }
                cloverConnector.listeners.ForEach(listener => listener.OnRefundPaymentResponse(lastPRR));
                cloverConnector.ShowOnDevice(showThankYouScreen: true, 
                                             milliSecondsTimeoutForThankYou: 3000, 
                                             showWelcomeScreen: true);
                */
            }

            public void onFinishCancel()
            {
                if (cloverConnector.lastRequest is PreAuthRequest)
                {
                    PreAuthResponse preAuthResponse = new PreAuthResponse();
                    preAuthResponse.Code = TransactionResponse.CANCEL;
                    cloverConnector.listeners.ForEach(listener => listener.OnPreAuthResponse(preAuthResponse));
                }
                else if (cloverConnector.lastRequest is AuthRequest)
                {
                    AuthResponse authResponse = new AuthResponse();
                    authResponse.Code = TransactionResponse.CANCEL;
                    cloverConnector.listeners.ForEach(listener => listener.OnAuthResponse(authResponse));
                }
                else if (cloverConnector.lastRequest is SaleRequest)
                {
                    SaleResponse saleResponse = new SaleResponse();
                    saleResponse.Payment = null;
                    saleResponse.Code = TransactionResponse.CANCEL;
                    cloverConnector.listeners.ForEach(listener => listener.OnSaleResponse(saleResponse));
                }
                else if(cloverConnector.lastRequest is ManualRefundRequest)
                {
                    cloverConnector.lastRequest = null;
                    ManualRefundResponse refundResponse = new ManualRefundResponse();
                    refundResponse.Code = TransactionResponse.CANCEL;
                    cloverConnector.listeners.ForEach(listener => listener.OnManualRefundResponse(refundResponse));
                }
                else if (cloverConnector.lastRequest is RefundPaymentRequest)
                {
                    cloverConnector.lastRequest = null;
                    RefundPaymentResponse refundPaymentResponse = new RefundPaymentResponse();
                    refundPaymentResponse.Code = TransactionResponse.CANCEL;
                    cloverConnector.listeners.ForEach(listener => listener.OnRefundPaymentResponse(refundPaymentResponse));
                }
                cloverConnector.ShowOnDevice(msg: "The transaction was cancelled.", 
                                             milliSecondsTimeoutForMsg: 3000, 
                                             showThankYouScreen: true, 
                                             milliSecondsTimeoutForThankYou: 3000, 
                                             showWelcomeScreen: true);
            }

            public void onConfigError(string errorMessage)
            {
                ConfigErrorResponse ceResponse = new ConfigErrorResponse();
                ceResponse.Code = BaseResponse.ERROR;
                ceResponse.message = errorMessage;
                cloverConnector.listeners.ForEach(listener => listener.OnConfigError(ceResponse));
            }

            public void onVerifySignature(Payment payment, Signature2 signature)
            {
                SVR request = new SVR(cloverConnector.Device);
                request.Signature = signature;
                request.Payment = payment;
                cloverConnector.listeners.ForEach(listener => listener.OnSignatureVerifyRequest(request));
            }

            public void onPaymentVoided(Payment payment, VoidReason reason)
            {
                VoidPaymentResponse response = new VoidPaymentResponse();
                response.Code = TransactionResponse.SUCCESS;
                response.PaymentId = payment.id;
                response.TransactionNumber = (payment.cardTransaction != null) ? payment.cardTransaction.transactionNo : "";
                response.ResponseCode = payment.result.GetTypeCode().ToString(); //TODO: verify this value

                cloverConnector.listeners.ForEach(listener => listener.OnVoidPaymentResponse(response));
                cloverConnector.ShowOnDevice(msg:  "The transaction was voided.", 
                                             milliSecondsTimeoutForMsg: 3000, 
                                             showThankYouScreen: true, 
                                             milliSecondsTimeoutForThankYou: 3000, 
                                             showWelcomeScreen: true);
            }

            public void onTxStartResponse(bool success)
            {
                Console.WriteLine("Tx Started? " + success);
            }

            public void onVaultCardResponse(VaultCardResponseMessage vcrm)
            {
                VaultCardResponse vcr = new VaultCardResponse();
                vcr.Card = vcrm.cardObj;
                vcr.Status = vcrm.status;
                vcr.Reason = vcrm.reason;
                vcr.Code = vcrm.status.ToString();
                cloverConnector.listeners.ForEach(listener => listener.OnVaultCardResponse(vcr));
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);
            }

            public void onCaptureAuthResponse(CaptureAuthResponseMessage carm)
            {
                CaptureAuthResponse car = new CaptureAuthResponse();
                car.paymentId = carm.paymentId;
                car.amount = carm.amount;
                car.Code = carm.status.ToString();
                car.tipAmount = carm.tipAmount;
                cloverConnector.listeners.ForEach(listener => listener.OnAuthCaptureResponse(car));
                cloverConnector.ShowOnDevice(showWelcomeScreen: true);
            }
        }

        private class MerchantInfo
        {
            public MerchantInfo() {
                supportsManualRefunds = true;
                supportsTipAdjust = true;
                supportsVaultCards = true;
                supportsPreAuths = true;
                merchantID = "";
                merchantMId = "";
                merchantName = "";
            }
            public MerchantInfo(DiscoveryResponseMessage drm)
            {
                supportsManualRefunds = drm.supportsManualRefund;
                supportsTipAdjust = drm.supportsTipAdjust;
                supportsVaultCards = drm.supportsMultiPayToken;
                supportsPreAuths = drm.supportsTipAdjust;
                merchantID = drm.merchantId;
                merchantMId = drm.merchantMId;
                merchantName = drm.merchantName;
            }

            public String merchantID { get; set; }
            public String merchantName { get; set; }
            public String merchantMId { get; set; }

            public bool supportsPreAuths { get; set; }
            public bool supportsVaultCards { get; set; }
            public bool supportsManualRefunds { get; set; }
            public bool supportsTipAdjust { get; set; }
        }
    }
}
