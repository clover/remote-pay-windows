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
using com.clover.remotepay.data;
using com.clover.remotepay.transport;
using com.clover.sdk.v3.order;
using com.clover.sdk.v3.payments;
using System.ComponentModel;
using System.Collections.Generic;
using com.clover.remote.order;
using com.clover.remote.order.operation;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Threading;

namespace com.clover.remotepay.transport
{
    public class DefaultCloverDevice : CloverDevice, CloverTransportObserver
    {
        private RefundResponseMessage lastRefundResponseMessage { get; set; }

        // Used to halt auto-accept of signature when payment confirmation is requested.
        private ManualResetEventSlim paymentConfirmationIdle = new ManualResetEventSlim(true);
        private bool paymentRejected = false;
        private object ackLock = new object();
        private Dictionary<String, BackgroundWorker> msgIdToTask = new Dictionary<string, BackgroundWorker>();

        public DefaultCloverDevice(CloverDeviceConfiguration configuration) :
            this(configuration.getMessagePackageName(), configuration.getCloverTransport(), configuration.getRemoteApplicationID())
        {
        }

        public DefaultCloverDevice(String packageName, CloverTransport transport, String remoteApplicationID) : base(packageName, transport, remoteApplicationID)
        {
            transport.Subscribe(this);
        }

        //---------------------------------------------------
        public void onDeviceConnected(CloverTransport transport)
        {
            deviceObservers.ForEach(x => x.onDeviceConnected());
        }

        public void onDeviceDisconnected(CloverTransport transport)
        {
            deviceObservers.ForEach(x => x.onDeviceDisconnected());
        }

        public void onDeviceReady(CloverTransport device)
        {
            doDiscoveryRequest();
        }

        public void onDeviceError(int code, string message)
        {
            deviceObservers.ForEach(x => x.onDeviceError(code, message));
        }

        private void setPaymentConfirmationIdle(bool value)
        {
            if (value)
            {
                paymentConfirmationIdle.Set();
            } else
            {
                paymentConfirmationIdle.Reset();
            }
        }
        /// <summary>
        /// This handles parsing the generic message and figuring
        /// out which handler should be used for processing
        /// </summary>
        /// <param name="message">The message.</param>
        public void onMessage(string message)
        {
#if DEBUG
            Console.WriteLine("Received raw message: " + message);
#endif
            //CloverTransportObserver
            // Deserialize the message object to a real object, and figure
            RemoteMessage rMessage = JsonUtils.deserializeSDK<RemoteMessage>(message);
            switch (rMessage.method)
            {
                case Methods.BREAK:
                    break;
                case Methods.ACK:
                    AcknowledgementMessage ackMessage = JsonUtils.deserializeSDK<AcknowledgementMessage>(rMessage.payload);
                    notifyObserverAck(ackMessage);
                    break;
                case Methods.CASHBACK_SELECTED:
                    CashbackSelectedMessage cbsMessage = JsonUtils.deserializeSDK<CashbackSelectedMessage>(rMessage.payload);
                    notifyObserversCashbackSelected(cbsMessage);
                    break;
                case Methods.DISCOVERY_RESPONSE:
                    DiscoveryResponseMessage drMessage = JsonUtils.deserializeSDK<DiscoveryResponseMessage>(rMessage.payload);
                    deviceInfo.name = drMessage.name;
                    deviceInfo.serial = drMessage.serial;
                    deviceInfo.model = drMessage.model;
                    notifyObserversDiscoveryResponse(drMessage);
                    break;
                case Methods.FINISH_CANCEL:
                    notifyObserversFinishCancel();
                    break;
                case Methods.FINISH_OK:
                    FinishOkMessage fokmsg = JsonUtils.deserializeSDK<FinishOkMessage>(rMessage.payload);
                    notifyObserversFinishOk(fokmsg);
                    break;
                case Methods.KEY_PRESS:
                    KeyPressMessage kpm = JsonUtils.deserializeSDK<KeyPressMessage>(rMessage.payload);
                    notifyObserversKeyPressed(kpm);
                    break;
                case Methods.ORDER_ACTION_RESPONSE:
                    break;
                case Methods.PARTIAL_AUTH:
                    PartialAuthMessage partialAuth = JsonUtils.deserializeSDK<PartialAuthMessage>(rMessage.payload);
                    notifyObserversPartialAuth(partialAuth);
                    break;
                case Methods.PAYMENT_VOIDED:
                    // this seems to only gets called if a Signature is "Canceled" on the device
                    break;
                case Methods.CONFIRM_PAYMENT_MESSAGE:
                    setPaymentConfirmationIdle(false);
                    ConfirmPaymentMessage confirmPaymentMessage = JsonUtils.deserializeSDK<ConfirmPaymentMessage>(rMessage.payload);
                    notifyObserversConfirmPayment(confirmPaymentMessage);
                    break;
                case Methods.TIP_ADDED:
                    TipAddedMessage tipMessage = JsonUtils.deserializeSDK<TipAddedMessage>(rMessage.payload);
                    notifyObserversTipAdded(tipMessage);
                    break;
                case Methods.TX_START_RESPONSE:
                    TxStartResponseMessage txsrm = JsonUtils.deserializeSDK<TxStartResponseMessage>(rMessage.payload);
                    notifyObserversTxStartResponse(txsrm);
                    break;
                case Methods.TX_STATE:
                    TxStateMessage txStateMsg = JsonUtils.deserializeSDK<TxStateMessage>(rMessage.payload);
                    notifyObserversTxState(txStateMsg);
                    break;
                case Methods.UI_STATE:
                    UiStateMessage uiStateMsg = JsonUtils.deserializeSDK<UiStateMessage>(rMessage.payload);
                    notifyObserversUiState(uiStateMsg);
                    break;
                case Methods.VERIFY_SIGNATURE:
                    paymentRejected = false;
                    VerifySignatureMessage vsigMsg = JsonUtils.deserializeSDK<VerifySignatureMessage>(rMessage.payload);
                    notifyObserversVerifySignature(vsigMsg);
                    break;
                case Methods.REFUND_RESPONSE:
                    RefundResponseMessage refRespMsg = JsonUtils.deserializeSDK<RefundResponseMessage>(rMessage.payload);
                    notifyObserversRefundPaymentResponse(refRespMsg);
                    break;
                case Methods.TIP_ADJUST_RESPONSE:
                    TipAdjustResponseMessage tipAdjustMsg = JsonUtils.deserializeSDK<TipAdjustResponseMessage>(rMessage.payload);
                    notifyObserversTipAdjusted(tipAdjustMsg);
                    break;
                case Methods.REFUND_REQUEST:
                    //Outbound no-op
                    break;
                case Methods.VAULT_CARD_RESPONSE:
                    VaultCardResponseMessage vcrMsg = JsonUtils.deserializeSDK<VaultCardResponseMessage>(rMessage.payload);
                    notifyObserversVaultCardResponse(vcrMsg);
                    break;
                case Methods.CARD_DATA_RESPONSE:
                    ReadCardDataResponseMessage rcdrMsg = JsonUtils.deserializeSDK<ReadCardDataResponseMessage>(rMessage.payload);
                    notifyObserversReadCardDataResponse(rcdrMsg);
                    break;
                case Methods.CAPTURE_PREAUTH_RESPONSE:
                    CapturePreAuthResponseMessage carMsg = JsonUtils.deserializeSDK<CapturePreAuthResponseMessage>(rMessage.payload);
                    notifyObserversCapturePreAuthResponse(carMsg);
                    break;
                case Methods.CLOSEOUT_RESPONSE:
                    CloseoutResponseMessage crMsg = JsonUtils.deserializeSDK<CloseoutResponseMessage>(rMessage.payload);
                    notifyObserversCloseoutResponse(crMsg);
                    break;
                case Methods.RETRIEVE_PENDING_PAYMENTS_RESPONSE:
                    RetrievePendingPaymentsResponseMessage rpprMsg = JsonUtils.deserializeSDK<RetrievePendingPaymentsResponseMessage>(rMessage.payload);
                    notifyObserversPendingPaymentsResponse(rpprMsg);
                    break;
                case Methods.DISCOVERY_REQUEST:
                    //Outbound no-op
                    break;
                case Methods.ORDER_ACTION_ADD_DISCOUNT:
                    //Outbound no-op
                    break;
                case Methods.ORDER_ACTION_ADD_LINE_ITEM:
                    //Outbound no-op
                    break;
                case Methods.ORDER_ACTION_REMOVE_LINE_ITEM:
                    //Outbound no-op
                    break;
                case Methods.ORDER_ACTION_REMOVE_DISCOUNT:
                    //Outbound no-op
                    break;
                case Methods.PRINT_CREDIT:
                    CreditPrintMessage cpm = JsonUtils.deserializeSDK<CreditPrintMessage>(rMessage.payload);
                    notifyObserversPrintCredit(cpm);
                    break;
                case Methods.PRINT_CREDIT_DECLINE:
                    DeclineCreditPrintMessage dcpm = JsonUtils.deserializeSDK<DeclineCreditPrintMessage>(rMessage.payload);
                    notifyObserversPrintCreditDecline(dcpm);
                    break;
                case Methods.PRINT_PAYMENT:
                    PaymentPrintMessage ppm = JsonUtils.deserializeSDK<PaymentPrintMessage>(rMessage.payload);
                    notifyObserversPrintPayment(ppm);
                    break;
                case Methods.PRINT_PAYMENT_DECLINE:
                    DeclinePaymentPrintMessage dppm = JsonUtils.deserializeSDK<DeclinePaymentPrintMessage>(rMessage.payload);
                    notifyObserversPrintPaymentDecline(dppm);
                    break;
                case Methods.PRINT_PAYMENT_MERCHANT_COPY:
                    PaymentPrintMerchantCopyMessage ppmcm = JsonUtils.deserializeSDK<PaymentPrintMerchantCopyMessage>(rMessage.payload);
                    notifyObserversPrintMerchantCopy(ppmcm);
                    break;
                case Methods.REFUND_PRINT_PAYMENT:
                    RefundPaymentPrintMessage rppm = JsonUtils.deserializeSDK<RefundPaymentPrintMessage>(rMessage.payload);
                    notifyObserversPrintRefund(rppm);
                    break;
                case Methods.PRINT_IMAGE:
                    //Outbound no-op
                    break;
                case Methods.PRINT_TEXT:
                    //Outbound no-op
                    break;
                case Methods.SHOW_ORDER_SCREEN:
                    //Outbound no-op
                    break;
                case Methods.SHOW_PAYMENT_RECEIPT_OPTIONS:
                    //Outbound no-op
                    break;
                case Methods.SHOW_REFUND_RECEIPT_OPTIONS:
                    //Outbound no-op
                    break;
                case Methods.SHOW_CREDIT_RECEIPT_OPTIONS:
                    //Outbound no-op
                    break;
                case Methods.SHOW_THANK_YOU_SCREEN:
                    //Outbound no-op
                    break;
                case Methods.SHOW_WELCOME_SCREEN:
                    //Outbound no-op
                    break;
                case Methods.SIGNATURE_VERIFIED:
                    //Outbound no-op
                    break;
                case Methods.TERMINAL_MESSAGE:
                    //Outbound no-op
                    break;
                case Methods.TX_START:
                    //Outbound no-op
                    break;
                case Methods.VOID_PAYMENT:
                    //Outbound no-op
                    break;
                case Methods.CLOSEOUT_REQUEST:
                    //Outbound no-op
                    break;
                case Methods.VAULT_CARD:
                    //Outbound no-op
                    break;
                case Methods.CARD_DATA:
                    //Outbound no-op
                    break;
            }
        }

        public void notifyObserversRefundPaymentResponse(RefundResponseMessage rrm)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                BackgroundWorker bw = new BackgroundWorker();
                // what to do in the background thread
                bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    observer.onRefundPaymentResponse(rrm.refund, rrm.orderId, rrm.paymentId, rrm.code, rrm.reason.ToString() + " " + rrm.message);
                });
                bw.RunWorkerAsync();
            }
        }
        public void notifyObserversTipAdjusted(TipAdjustResponseMessage tarm)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                BackgroundWorker bw = new BackgroundWorker();
                // what to do in the background thread
                bw.DoWork += new DoWorkEventHandler(
                    delegate (object o, DoWorkEventArgs args)
                    {
                        observer.onAuthTipAdjusted(tarm.paymentId, tarm.amount, tarm.success);
                });
                bw.RunWorkerAsync();
            }
        }
        public void notifyObserversVaultCardResponse(VaultCardResponseMessage vcrm)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args) {
                foreach (ICloverDeviceObserver observer in deviceObservers)
                {
                    observer.onVaultCardResponse(vcrm);
                }
            });
            bw.RunWorkerAsync();
        }
        public void notifyObserversReadCardDataResponse(ReadCardDataResponseMessage cdrm)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args) {
                foreach (ICloverDeviceObserver observer in deviceObservers)
                {
                    observer.onReadCardDataResponse(cdrm);
                }
            });
            bw.RunWorkerAsync();
        }
        public void notifyObserversDiscoveryResponse(DiscoveryResponseMessage drMessage)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args) {
                foreach (ICloverDeviceObserver observer in deviceObservers)
                {
                    if( drMessage.ready )
                    {
                        observer.onDeviceReady(this, drMessage);
                    }
                    else 
                    {
                        observer.onDeviceConnected(); 
                    }
                }
            });
            bw.RunWorkerAsync();
        }
        public void notifyObserversCapturePreAuthResponse(CapturePreAuthResponseMessage carm)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args) {
                foreach (ICloverDeviceObserver observer in deviceObservers)
                {
                    observer.onCapturePreAuthResponse(carm.paymentId, carm.amount, carm.tipAmount, carm.status, carm.reason);
                }
            });
            bw.RunWorkerAsync();
        }
        public void notifyObserversCloseoutResponse(CloseoutResponseMessage crm)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args) {
                foreach (ICloverDeviceObserver observer in deviceObservers)
                {
                    observer.onCloseoutResponse(crm.status, crm.reason, crm.batch);
                }
            });
            bw.RunWorkerAsync();
        }
        public void notifyObserversPendingPaymentsResponse(RetrievePendingPaymentsResponseMessage rpprm)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args) {
                foreach (ICloverDeviceObserver observer in deviceObservers)
                {
                    observer.onRetrievePendingPaymentsResponse(rpprm.status == ResultStatus.SUCCESS, rpprm.pendingPaymentEntries);
                }
            });
            bw.RunWorkerAsync();
        }
        public void notifyObserversKeyPressed(KeyPressMessage keyPress)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                BackgroundWorker bw = new BackgroundWorker();
                // what to do in the background thread
                bw.DoWork += new DoWorkEventHandler(
                    delegate (object o, DoWorkEventArgs args)
                    {
                        BackgroundWorker b = o as BackgroundWorker;
                        observer.onKeyPressed(keyPress.keyPress);
                });
                bw.RunWorkerAsync();
            }
        }

        public void notifyObserversCashbackSelected(CashbackSelectedMessage cbSelected)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                BackgroundWorker bw = new BackgroundWorker();
                // what to do in the background thread
                bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    BackgroundWorker b = o as BackgroundWorker;
                    observer.onCashbackSelected(cbSelected.cashbackAmount);
                });
                bw.RunWorkerAsync();
            }
        }

        public void notifyObserversConfirmPayment(ConfirmPaymentMessage message)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                BackgroundWorker bw = new BackgroundWorker();
                // what to do in the background thread
                bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    BackgroundWorker b = o as BackgroundWorker;
                    observer.onConfirmPayment(message.payment, message.challenges);
                });
                bw.RunWorkerAsync();
            }
        }

        public void notifyObserversTipAdded(TipAddedMessage tipAdded)
        {
            foreach(ICloverDeviceObserver observer in deviceObservers)
            {
                BackgroundWorker bw = new BackgroundWorker();
                // what to do in the background thread
                bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    BackgroundWorker b = o as BackgroundWorker;
                    observer.onTipAdded(tipAdded.tipAmount);
                });
                bw.RunWorkerAsync();
            }
        }

        public void notifyObserversTxStartResponse(TxStartResponseMessage txsrm)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                BackgroundWorker bw = new BackgroundWorker();
                // what to do in the background thread
                bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    BackgroundWorker b = o as BackgroundWorker;
                    observer.onTxStartResponse(txsrm.result, txsrm.externalId);
                });
                bw.RunWorkerAsync();
            }
        }

        public void notifyObserversPartialAuth(PartialAuthMessage partialAuth)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                BackgroundWorker bw = new BackgroundWorker();
                // what to do in the background thread
                bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    BackgroundWorker b = o as BackgroundWorker;
                    observer.onPartialAuth(partialAuth.partialAuthAmount);
                });
                bw.RunWorkerAsync();
            }
        }

        public void notifyObserversPaymentVoided(Payment payment, VoidReason reason)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                BackgroundWorker bw = new BackgroundWorker();
                // what to do in the background thread
                bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    BackgroundWorker b = o as BackgroundWorker;
                    observer.onPaymentVoided(payment, reason);
                });
                bw.RunWorkerAsync();
            }
        }

        public void notifyObserversVerifySignature(VerifySignatureMessage verifySigMsg)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onVerifySignature(verifySigMsg.payment, verifySigMsg.signature);
            }
        }


        public void notifyObserversUiState(UiStateMessage uiStateMsg)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onUiState(uiStateMsg.uiState, uiStateMsg.uiText, uiStateMsg.uiDirection, uiStateMsg.inputOptions);
            }
        }


        public void notifyObserversTxState(TxStateMessage txStateMsg)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onTxState(txStateMsg.txState);
            }
        }

        public void notifyObserversFinishCancel()
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onFinishCancel();
            }
        }
        public void notifyObserversFinishOk(FinishOkMessage msg)

        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                if (msg.payment != null)
                {
                    observer.onFinishOk(msg.payment, msg.signature);
                }
                else if (msg.credit != null)
                {
                    observer.onFinishOk(msg.credit);
                }
                else if (msg.refund != null)
                {
                    observer.onFinishOk(msg.refund);
                }
                else
                {
                    Console.WriteLine("Don't know what to do with this Finish OK message: " + JsonUtils.serialize(msg));
                }
            }
        }

        public void notifyObserverAck(AcknowledgementMessage ackMessage)
        {
            lock(ackLock)
            {
                BackgroundWorker worker = null;
                if(msgIdToTask.TryGetValue(ackMessage.sourceMessageId, out worker))
                {
                	// this allows DCD to register an action, initially void payment
                    if (worker != null)
                    {
                        msgIdToTask.Remove(ackMessage.sourceMessageId);
                        worker.RunWorkerAsync();
                    }

					// this allows other listeners to register a listener
                    foreach (ICloverDeviceObserver observer in deviceObservers)
                    {
                        observer.onMessageAck(ackMessage.sourceMessageId);
                    }
                }
                else
                {
                    // No task for messageId
                }
            }
        }

        public void notifyObserversPrintCredit(CreditPrintMessage cpm)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onPrintCredit(cpm.credit);
            }
        }

        public void notifyObserversPrintCreditDecline(DeclineCreditPrintMessage dcpm)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onPrintCreditDecline(dcpm.credit, dcpm.reason);
            }
        }

        public void notifyObserversPrintPayment(PaymentPrintMessage ppm)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onPrintPayment(ppm.payment, ppm.order);
            }
        }

        public void notifyObserversPrintPaymentDecline(DeclinePaymentPrintMessage dppm)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onPrintPaymentDecline(dppm.payment, dppm.reason);
            }
        }

        public void notifyObserversPrintMerchantCopy(PaymentPrintMerchantCopyMessage ppmcm)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onPrintMerchantReceipt(ppmcm.payment);
            }
        }

        public void notifyObserversPrintRefund(RefundPaymentPrintMessage rppm)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onPrintRefundPayment(rppm.payment, rppm.order, rppm.refund);
            }
        }

        public override void doShowPaymentReceiptScreen(string orderId, string paymentId)
        {
            sendObjectMessage(new PaymentReceiptMessage(orderId, paymentId));
        }

        public override void doLogMessages(LogLevelEnum logLevel, Dictionary<string, string> messages)
        {
            sendObjectMessage(new LogMessage(logLevel, messages));
        }

        /*
        public override void doShowRefundReceiptScreen(string orderId, string refundId)
        {
            sendObjectMessage(new RefundReceiptMessage(orderId, refundId));
        }

        public override void doShowCreditReceiptScreen(string orderId, string creditId)
        {
            sendObjectMessage(new CreditReceiptMessage(orderId, creditId));
        }
        */
        public override void doKeyPress(KeyPress keyPress)
        {
            sendObjectMessage(new KeyPressMessage(keyPress));
        }

        public override void doShowThankYouScreen()
        {
            sendObjectMessage(new Message(Methods.SHOW_THANK_YOU_SCREEN));
        }

        public override void doShowWelcomeScreen()
        {
            sendObjectMessage(new Message(Methods.SHOW_WELCOME_SCREEN));
        }

        public override void doRetrievePendingPayments()
        {
            sendObjectMessage(new Message(Methods.RETRIEVE_PENDING_PAYMENTS));
        }

        public override void doVerifySignature(Payment payment, bool verified)
        {
            paymentConfirmationIdle.Wait();
            if (!paymentRejected)
            {
                sendObjectMessage(new SignatureVerifiedMessage(payment, verified));
            }
        }

        public override void doTerminalMessage(string text)
        {
            sendObjectMessage(new TerminalMessage(text));
        }

        public override void doOpenCashDrawer(string reason)
        {
            sendObjectMessage(new OpenCashDrawerMessage(reason));
        }

        public override void doVaultCard(int? CardEntryMethods)
        {
            sendObjectMessage(new VaultCardMessage(CardEntryMethods)); // take defaults entry methods
        }

        public override void doReadCardData(PayIntent payIntent)
        {
            sendObjectMessage(new ReadCardDataMessage(payIntent)); 
        }

        public override void doCloseout(bool allowOpenTabs, string batchId)
        {
            sendObjectMessage(new CloseoutMessage(allowOpenTabs, batchId));
        }

        public override void doResetDevice()
        {
            sendObjectMessage(new BreakMessage());
        }

        public override void doTxStart(PayIntent payIntent, Order order)
        {
            sendObjectMessage(new TxStartRequestMessage(payIntent, order));
        }

        public override void doTipAdjustAuth(string orderId, string paymentId, long amount)
        {
            sendObjectMessage(new TipAdjustAuthMessage(orderId, paymentId, amount));
        }

        public override void doCapturePreAuth(string paymentID, long amount, long tipAmount)
        {
            sendObjectMessage(new CapturePreAuthMessage(paymentID, amount, tipAmount));
        }

        public override void doPrintText(List<string> textLines)
        {
            TextPrintMessage tpm = new TextPrintMessage();
            foreach (string line in textLines)
            {
                tpm.textLines.Add(line);
            }
            sendObjectMessage(tpm);
        }

        public override void doPrintImage(string base64String)
        {
            ImagePrintMessage ipm = new ImagePrintMessage();
            ipm.png = base64String;
            sendObjectMessage(ipm);
        }

        public override void doPrintImageURL(string urlString)
        {
            ImagePrintMessage ipm = new ImagePrintMessage();
            ipm.urlString = urlString;
            sendObjectMessage(ipm);
        }

        public override void doVoidPayment(Payment payment, VoidReason reason)
        {
            lock(ackLock)
            {
                VoidPaymentMessage vpm = new VoidPaymentMessage();
                vpm.payment = payment;
                vpm.voidReason = reason;
                string msgId = sendObjectMessage(vpm);


                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    notifyObserversPaymentVoided(payment, reason);
                });


                if (!SupportsAcks)
                {
                    bw.RunWorkerAsync();
                }
                else
                {
                    msgIdToTask.Add(msgId, bw);
                }
            }
        }

        public override void doRefundPayment(string orderId, string paymentId, long? amount, bool? fullRefund)
        {
            sendObjectMessage(new RefundRequestMessage(orderId, paymentId, amount, fullRefund));
        }

        public override void doDiscoveryRequest()
        {
            sendObjectMessage(new DiscoveryRequestMessage());
        }

        public override void doOrderUpdate(DisplayOrder order, DisplayOperation operation)
        {
            OrderUpdateMessage updateMessage = new OrderUpdateMessage(order);
            updateMessage.setOperation(operation);

            sendObjectMessage(updateMessage);
            
        }

        public override void doAcceptPayment(Payment payment)
        {
            setPaymentConfirmationIdle(true);
            PaymentConfirmedMessage message = new PaymentConfirmedMessage();
            message.payment = payment;

            sendObjectMessage(message);
        }

        public override void doRejectPayment(Payment payment, Challenge challenge)
        {
            paymentRejected = true;
            setPaymentConfirmationIdle(true);
            PaymentRejectedMessage message = new PaymentRejectedMessage();
            message.payment = payment;
            message.challenge = challenge;

            sendObjectMessage(message);
        }

        private string sendObjectMessage(Message message)
        {
            RemoteMessage remoteMessage = RemoteMessage.createMessage(
                message.method, MessageTypes.COMMAND, message, this.packageName, remoteSourceSDK, remoteApplicationID
            );
            string msg = JsonUtils.serializeSDK(remoteMessage);
            transport.sendMessage(msg);
#if DEBUG
            Console.WriteLine("Sent message: " + msg);
#endif
            return remoteMessage.id;
        }
    }
}
