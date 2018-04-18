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
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using com.clover.remote.order;
using com.clover.remote.order.operation;
using com.clover.remotepay.data;
using com.clover.sdk.v3.order;
using com.clover.sdk.v3.payments;
using com.clover.sdk.v3.printer;

namespace com.clover.remotepay.transport
{
    public class DefaultCloverDevice : CloverDevice, CloverTransportObserver
    {
        public int maxMessageSizeInChars = 1000;
        public long MAX_PAYLOAD_SIZE = 10000000;

        /// <summary>
        /// Used to halt auto-accept of signature when payment confirmation is requested.
        /// </summary>
        private ManualResetEventSlim paymentConfirmationIdle = new ManualResetEventSlim(true);
        private bool paymentRejected = false;
        private object ackLock = new object();
        private Dictionary<string, BackgroundWorker> msgIdToTask = new Dictionary<string, BackgroundWorker>();
        private int remoteMessageVersion = 1;

        public DefaultCloverDevice(CloverDeviceConfiguration configuration) : base(configuration)
        {
        }

        public override void Initialize(CloverDeviceConfiguration configuration)
        {
            base.Initialize(configuration);

            transport.Subscribe(this);
            maxMessageSizeInChars = Math.Max(maxMessageSizeInChars, configuration.getMaxMessageCharacters());
        }

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

        public void onDeviceError(int code, Exception cause, string message)
        {
            deviceObservers.ForEach(x => x.onDeviceError(code, cause, message));
        }

        private void setPaymentConfirmationIdle(bool value)
        {
            if (value)
            {
                paymentConfirmationIdle.Set();
            }
            else
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
            remoteMessageVersion = Math.Max(remoteMessageVersion, rMessage.version);

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
                    FinishCancelMessage finishCancelMessage = JsonUtils.deserializeSDK<FinishCancelMessage>(rMessage.payload);
                    notifyObserversFinishCancel(finishCancelMessage.requestInfo);
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
                case Methods.ACTIVITY_RESPONSE:
                    ActivityResponseMessage arm = JsonUtils.deserializeSDK<ActivityResponseMessage>(rMessage.payload);
                    notifyObserversActivityResponse(arm);
                    break;
                case Methods.ACTIVITY_MESSAGE_FROM_ACTIVITY:
                    ActivityMessageFromActivity amfa = JsonUtils.deserializeSDK<ActivityMessageFromActivity>(rMessage.payload);
                    notifyObserversActivityMessage(amfa);
                    break;
                case Methods.RESET_DEVICE_RESPONSE:
                    ResetDeviceResponseMessage rdrm = JsonUtils.deserializeSDK<ResetDeviceResponseMessage>(rMessage.payload);
                    notifyObserversDeviceReset(rdrm);
                    break;
                case Methods.RETRIEVE_DEVICE_STATUS_RESPONSE:
                    RetrieveDeviceStatusResponseMessage rdsrm = JsonUtils.deserializeSDK<RetrieveDeviceStatusResponseMessage>(rMessage.payload);
                    notifyObserversRetrieveDeviceStatusResponse(rdsrm);
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
                case Methods.RETRIEVE_PAYMENT_RESPONSE:
                    RetrievePaymentResponseMessage rprm = JsonUtils.deserializeSDK<RetrievePaymentResponseMessage>(rMessage.payload);
                    notifyObserversRetrievePaymentResponse(rprm);
                    break;
                case Methods.GET_PRINTERS_RESPONSE:
                    RetrievePrintersResponseMessage rtrm = JsonUtils.deserializeSDK<RetrievePrintersResponseMessage>(rMessage.payload);
                    notifyObserversRetrievePrinterResponse(rtrm);
                    break;
                case Methods.PRINT_JOB_STATUS_RESPONSE:
                    PrintJobStatusResponseMessage pjsrm = JsonUtils.deserializeSDK<PrintJobStatusResponseMessage>(rMessage.payload);
                    notifyObserversRetrievePrintJobStatus(pjsrm);
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
                        observer.onRefundPaymentResponse(rrm.refund, rrm.orderId, rrm.paymentId, rrm.code, rrm.reason.ToString() + " " + rrm.message, rrm.reason);
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
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                {
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
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                {
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
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                {
                    foreach (ICloverDeviceObserver observer in deviceObservers)
                    {
                        if (drMessage.ready)
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
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                {
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
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                {
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
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                {
                    foreach (ICloverDeviceObserver observer in deviceObservers)
                    {
                        observer.onRetrievePendingPaymentsResponse(rpprm.status == ResultStatus.SUCCESS, rpprm.pendingPaymentEntries);
                    }
                });
            bw.RunWorkerAsync();
        }

        public void notifyObserversActivityResponse(ActivityResponseMessage arm)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                {
                    ResultStatus status = arm.resultCode == -1 ? ResultStatus.SUCCESS : ResultStatus.CANCEL;
                    foreach (ICloverDeviceObserver observer in deviceObservers)
                    {
                        observer.onActivityResponse(status, arm.action, arm.payload, arm.failReason);
                    }
                });
            bw.RunWorkerAsync();
        }

        public void notifyObserversKeyPressed(KeyPressMessage keyPress)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                    {
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
                bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                    {
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
                bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                    {
                        observer.onConfirmPayment(message.payment, message.challenges);
                    });
                bw.RunWorkerAsync();
            }
        }

        public void notifyObserversTipAdded(TipAddedMessage tipAdded)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                BackgroundWorker bw = new BackgroundWorker();
                // what to do in the background thread
                bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                    {
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
                bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                   {
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
                bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                   {
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
                bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                   {
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

        public void notifyObserversFinishCancel(TxType requestInfo)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onFinishCancel(requestInfo);
            }
        }

        public void notifyObserversFinishOk(FinishOkMessage msg)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                if (msg.payment != null)
                {
                    observer.onFinishOk(msg.payment, msg.signature, msg.requestInfo);
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
            lock (ackLock)
            {
                if (msgIdToTask.TryGetValue(ackMessage.sourceMessageId, out BackgroundWorker worker))
                {
                    // this allows DCD to register an action, initially void payment
                    if (worker != null)
                    {
                        msgIdToTask.Remove(ackMessage.sourceMessageId);
                        worker.RunWorkerAsync();
                    }
                }

                foreach (ICloverDeviceObserver observer in deviceObservers)
                {
                    observer.onMessageAck(ackMessage.sourceMessageId);
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


        public void notifyObserversActivityMessage(ActivityMessageFromActivity amfa)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onMessageFromActivity(amfa.action, amfa.payload);
            }
        }

        public void notifyObserversDeviceReset(ResetDeviceResponseMessage rdrm)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onResetDeviceResponse(ResultStatus.SUCCESS, rdrm.reason, rdrm.state);
            }
        }

        public void notifyObserversRetrieveDeviceStatusResponse(RetrieveDeviceStatusResponseMessage rdsrm)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onDeviceStatusResponse(ResultStatus.SUCCESS, rdsrm.reason, rdsrm.state, rdsrm.data);
            }
        }

        public void notifyObserversRetrievePaymentResponse(RetrievePaymentResponseMessage rpr)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onRetrievePaymentResponse(ResultStatus.SUCCESS, rpr.reason, rpr.externalPaymentId, rpr.queryStatus, rpr.payment);
            }
        }

        public void notifyObserversRetrievePrinterResponse(RetrievePrintersResponseMessage response)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onRetrievePrintersResponse(response.printers);

            }
        }

        public void notifyObserversRetrievePrintJobStatus(PrintJobStatusResponseMessage response)
        {
            foreach (ICloverDeviceObserver observer in deviceObservers)
            {
                observer.onRetrievePrintJobStatus(response.externalPrintJobId, response.status);
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

        public override void doOpenCashDrawer(string reason, string deviceId)
        {
            Printer printer = null;
            if (deviceId != null)
            {
                printer = new Printer();
                printer.id = deviceId;
            }
            sendObjectMessage(new OpenCashDrawerMessage(reason, printer));
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

        public override void doTxStart(PayIntent payIntent, Order order, TxType requestInfo)
        {
            sendObjectMessage(new TxStartRequestMessage(payIntent, order, requestInfo));
        }

        public override void doTipAdjustAuth(string orderId, string paymentId, long amount)
        {
            sendObjectMessage(new TipAdjustAuthMessage(orderId, paymentId, amount));
        }

        public override void doCapturePreAuth(string paymentID, long amount, long tipAmount)
        {
            sendObjectMessage(new CapturePreAuthMessage(paymentID, amount, tipAmount));
        }

        public override void doPrintText(List<string> textLines, string printRequestId, string printDeviceId)
        {
            TextPrintMessage tpm = new TextPrintMessage();
            tpm.externalPrintJobId = printRequestId;
            foreach (string line in textLines)
            {
                tpm.textLines.Add(line);
            }

            sendObjectMessage(tpm);
        }

        public override void doPrintImageURL(string base64String, string printRequestId, string printDeviceId)
        {
            WebRequest request = WebRequest.Create(base64String);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            Bitmap bitmap = new Bitmap(responseStream);
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            byte[] imgBytes = ms.ToArray();

            ImagePrintMessage ipm = new ImagePrintMessage();
            ipm.externalPrintJobId = printRequestId;
            if (printDeviceId != null)
            {
                Printer printer = new Printer();
                printer.id = printDeviceId;
                ipm.printer = printer;
            }

            if (remoteMessageVersion > 1)
            {
                sendCommandMessage(ipm, ipm.method, version: 2, attachmentData: imgBytes);
            }
            else
            {
                string base64Image = Convert.ToBase64String(imgBytes);
                ipm.png = base64Image;
                sendObjectMessage(ipm);
            }
        }

        public override void doPrintImage(Bitmap img, string printRequestId, string printDeviceId)
        {
            if (img != null)
            {
                ImagePrintMessage ipm = new ImagePrintMessage();
                ipm.externalPrintJobId = printRequestId;

                if (printDeviceId != null)
                {
                    Printer printer = new Printer();
                    printer.id = printDeviceId;
                    ipm.printer = printer;
                }

                if (remoteMessageVersion > 1)
                {
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, ImageFormat.Png);
                    byte[] imgBytes = ms.ToArray();
                    sendCommandMessage(ipm, ipm.method, version: 2, attachmentData: imgBytes);
                }
                else
                {
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, ImageFormat.Png);
                    byte[] imgBytes = ms.ToArray();
                    string base64Image = Convert.ToBase64String(imgBytes);
                    ipm.png = base64Image;
                    sendObjectMessage(ipm);
                }
            }
        }

        public override void doVoidPayment(Payment payment, VoidReason reason)
        {
            lock (ackLock)
            {
                VoidPaymentMessage vpm = new VoidPaymentMessage();
                vpm.payment = payment;
                vpm.voidReason = reason;
                string msgId = sendObjectMessage(vpm);

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
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
            message.reason = challenge.reason;

            sendObjectMessage(message);
        }

        public override void doStartCustomActivity(string action, string payload, bool nonBlocking)
        {
            ActivityRequest ar = new ActivityRequest();
            ar.action = action;
            ar.payload = payload;
            ar.nonBlocking = nonBlocking;
            ar.forceLaunch = false;
            sendObjectMessage(ar);
        }

        public override void doSendMessageToActivity(string action, string payload)
        {
            ActivityMessageToActivity amta = new ActivityMessageToActivity(action, payload);
            sendObjectMessage(amta);
        }

        public override void doRetrieveDeviceStatus(bool sendLastMessage)
        {
            RetrieveDeviceStatusRequestMessage rdsrm = new RetrieveDeviceStatusRequestMessage(sendLastMessage);
            sendObjectMessage(rdsrm);
        }

        public override void doRetrievePrinters(RetrievePrintersRequest request)
        {
            RetrievePrintersRequestMessage message = new RetrievePrintersRequestMessage();
            message.category = request.category;

            sendObjectMessage(message);
        }

        public override void doRetrievePayment(string externalPaymentId)
        {
            PaymentRequestMessage rprm = new PaymentRequestMessage();
            rprm.externalPaymentId = externalPaymentId;
            sendObjectMessage(rprm);
        }

        private string sendObjectMessage(Message message)
        {
            RemoteMessage remoteMessage = RemoteMessage.createMessage(message.method, MessageTypes.COMMAND, message, this.packageName, remoteSourceSDK, remoteApplicationID);
            string msg = JsonUtils.serializeSDK(remoteMessage);

            transport.sendMessage(msg);
#if DEBUG
            Console.WriteLine("Sent message: " + msg);
#endif
            return remoteMessage.id;
        }

        private string sendCommandMessage(Message payload, Methods method, int version = 1, string attachment = null, string attachmentEncoding = null, byte[] attachmentData = null, string attachmentUrl = null)
        {
            RemoteMessage rm = RemoteMessage.createMessage(method, MessageTypes.COMMAND, payload, this.packageName, remoteSourceSDK, remoteApplicationID);
            rm.attachment = attachment;
            rm.attachmentEncoding = attachmentEncoding;

            return sendRemoteMessage(rm, version, attachmentData, attachmentUrl, attachmentEncoding);
        }

        private string sendRemoteMessage(RemoteMessage remoteMsg, int version = 1, byte[] attachmentData = null, string attachmentUrl = null, string attachmentEncoding = null)
        {
            remoteMsg.packageName = this.packageName;
            remoteMsg.remoteApplicationID = remoteApplicationID;
            remoteMsg.remoteSourceSDK = remoteSourceSDK;
            remoteMsg.version = version;

            if (remoteMsg.version > 1)
            {
                bool hasAttachmentURI = attachmentUrl != null;
                bool hasAttachmentData = attachmentData != null;
                int payloadHelper = 0;
                if (remoteMsg.attachment != null)
                {
                    payloadHelper = (remoteMsg.attachment.Length != 0 ? remoteMsg.attachment.Length : 0);
                }

                if (remoteMsg.payload != null)
                {
                    payloadHelper += (remoteMsg.payload.Length != 0 ? remoteMsg.payload.Length : 0);
                }

                // maxMessageSizeInChars is controlled by user, make sure it is a positive number or use a reasonable default to avoid infinite loop etc. problems
                int maxSize = maxMessageSizeInChars <= 0 ? 1000 : maxMessageSizeInChars;
                bool payloadTooLarge = payloadHelper > maxSize;
                bool shouldFrag = hasAttachmentURI || payloadTooLarge || hasAttachmentData;
                if (shouldFrag)
                {

                    if ((remoteMsg.attachment != null && remoteMsg.attachment.Length > MAX_PAYLOAD_SIZE))
                    {
                        Console.WriteLine("Error sending message - payload size is greater than the maximum allowed");
                        return null;
                    }

                    int fragmentIndex = 0;
                    string payloadStr = remoteMsg.payload != null ? remoteMsg.payload : "";

                    int startIndex = 0;
                    while (startIndex < payloadStr.Length)
                    {
                        int length = (maxSize < payloadStr.Length ? maxSize : payloadStr.Length);
                        string fPayload = payloadStr.Substring(startIndex, length);
                        startIndex += length;
                        bool attachmentAvailable = ((remoteMsg.attachment != null && remoteMsg.attachment.Length > 0 ? remoteMsg.attachment.Length : 0) == 0); //if attachment not null and is nonzero return false, otherwise return true
                        bool attachmentUriAvailable = ((remoteMsg.attachmentUri != null && remoteMsg.attachmentUri.Length > 0 ? remoteMsg.attachmentUri.Length : 0) == 0); //if attachment not null and is nonzero return false, otherwise return true
                        bool lastFragment = payloadStr.Length == 0 && (attachmentAvailable && attachmentUriAvailable);
                        sendMessageFragment(remoteMsg, fPayload, null, fragmentIndex++, lastFragment);
                    }

                    // now let's fragment the attachment or attachmentData
                    string attach = remoteMsg.attachment;
                    if (attach != null)
                    {
                        if (remoteMsg.attachmentEncoding == "BASE64")
                        {
                            remoteMsg.attachmentEncoding = "BASE64.ATTACHMENT";
                            int start = 0;
                            while (attach.Length > 0)
                            {
                                string aPayload = attach.Substring(start, maxSize < attach.Length ? maxSize : attach.Length);
                                start += maxSize < attach.Length ? maxSize : attach.Length;
                                sendMessageFragment(remoteMsg, null, aPayload, fragmentIndex++, attach.Length == 0);
                            }
                        }
                        else
                        {
                            // TODO: chunk as-is
                        }
                    }
                    else if (attachmentData != null)
                    {
                        int start = 0;
                        int count = attachmentData.Length;
                        remoteMsg.attachmentEncoding = "BASE64.FRAGMENT";
                        while (start < count)
                        {
                            int length = Math.Min(maxSize, count - start);
                            byte[] chunkData = new byte[length];
                            Array.Copy(attachmentData, start, chunkData, 0, length);
                            start = start + maxSize;

                            //FRAGMENT Payload
                            string fAttachment = Convert.ToBase64String(chunkData);
                            sendMessageFragment(remoteMsg: remoteMsg, fPayload: null, fAttachment: fAttachment, fragmentIndex: fragmentIndex++, lastFragment: start > count);
                        }
                    }
                }
                else //we DON'T need to fragment
                {
                    // note: attachmentData is always null here because we take earlier if branch "if shouldFrag" when it's not
                    if (attachmentData != null)
                    {
                        string base64string = Convert.ToBase64String(attachmentData);
                        doPrintImage(base64string);
                    }
                }
            }

            return remoteMsg.id;
        }

        private void sendMessageFragment(RemoteMessage remoteMsg, string fPayload, string fAttachment, int fragmentIndex, bool lastFragment)
        {
            RemoteMessage fRemoteMessage = new RemoteMessage();
            fRemoteMessage.id = remoteMsg.id;
            fRemoteMessage.method = remoteMsg.method;
            fRemoteMessage.type = remoteMsg.type;
            fRemoteMessage.packageName = remoteMsg.packageName;
            fRemoteMessage.remoteApplicationID = remoteMsg.remoteApplicationID;
            fRemoteMessage.remoteSourceSDK = remoteMsg.remoteSourceSDK;
            fRemoteMessage.version = remoteMsg.version;

            // changes for the fragment
            fRemoteMessage.payload = fPayload;
            fRemoteMessage.attachmentUri = null;
            fRemoteMessage.attachmentEncoding = remoteMsg.attachmentEncoding != null ? remoteMsg.attachmentEncoding : "BASE64.FRAGMENT";
            fRemoteMessage.attachment = fAttachment;
            fRemoteMessage.fragmentIndex = fragmentIndex;
            fRemoteMessage.lastFragment = lastFragment;

            string msg = JsonUtils.serializeSDK(fRemoteMessage);
            transport.sendMessage(msg);

#if DEBUG
            Console.WriteLine("Sent message: " + msg);
#endif
        }

        public override void doRetrievePrintJobStatus(string printRequestId)
        {
            PrintJobStatusRequestMessage msg = new PrintJobStatusRequestMessage(printRequestId);
            sendObjectMessage(msg);
        }

        public override void doPrintImage(string base64String)
        {
            ImagePrintMessage ipm = new ImagePrintMessage();
            ipm.png = base64String;
            sendObjectMessage(ipm);
        }
    }
}
