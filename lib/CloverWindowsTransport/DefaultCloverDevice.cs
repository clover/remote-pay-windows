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
using com.clover.sdk.v3;
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

        // Track device connection discovery state for reconnection flow
        private ConnectionState startupConnectionState = ConnectionState.Disconnected;
        private enum ConnectionState { Disconnected, Discovering, Discovered }

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
            startupConnectionState = ConnectionState.Discovering;
            NotifyObservers(observer => observer.onDeviceConnected());
        }

        public void onDeviceDisconnected(CloverTransport transport)
        {
            startupConnectionState = ConnectionState.Disconnected;

            NotifyObservers(observer => observer.onDeviceDisconnected());
        }

        public void onDeviceReady(CloverTransport device)
        {
            startupConnectionState = ConnectionState.Discovering;
            doDiscoveryRequest();
        }

        public void onDeviceError(int code, Exception cause, string message)
        {
            NotifyObservers(observer => observer.onDeviceError(code, cause, message));
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
        /// Parse the generic message and figure out which handler should be used for processing
        /// </summary>
        /// <param name="message">The message.</param>
        public void onMessage(string message)
        {
#if DEBUG
            Console.WriteLine("Received raw message: " + message);
#endif
            RemoteMessage rMessage = null;
            try
            {
                // Note: This handling was changed after the 1.4.2 release to properly suppress uknown messages.
                //       Old versions of the WinSDK will crash with unknown messages, and old versions are still expected to be in customer's hands.
                //       When testing releases, make sure any suppressed errors here are paid proper backwards-compatible attention and tested

                // Deserialize the message object to a real object
                rMessage = JsonUtils.DeserializeSdk<RemoteMessage>(message);
                remoteMessageVersion = Math.Max(remoteMessageVersion, rMessage.version);
            }
            catch (Exception exception)
            {
                // if a remote message can't be parsed, ignore this unknown message; log as appropriate
                // - and verify backwards compatiblility story since old WinSDK releases will crash

                // TODO: Log message and exception in new logging
            }

            // Handle and route known messages appropriately
            if (rMessage != null)
            {
                switch (rMessage.method)
                {
                    case Methods.BREAK:
                        break;
                    case Methods.ACK:
                        AcknowledgementMessage ackMessage = JsonUtils.DeserializeSdk<AcknowledgementMessage>(rMessage.payload);
                        notifyObserverAck(ackMessage);
                        break;
                    case Methods.CASHBACK_SELECTED:
                        CashbackSelectedMessage cbsMessage = JsonUtils.DeserializeSdk<CashbackSelectedMessage>(rMessage.payload);
                        notifyObserversCashbackSelected(cbsMessage);
                        break;
                    case Methods.DISCOVERY_RESPONSE:
                        DiscoveryResponseMessage drMessage = JsonUtils.DeserializeSdk<DiscoveryResponseMessage>(rMessage.payload);
                        deviceInfo.name = drMessage.name;
                        deviceInfo.serial = drMessage.serial;
                        deviceInfo.model = drMessage.model;
                        notifyObserversDiscoveryResponse(drMessage);
                        break;
                    case Methods.FINISH_CANCEL:
                        FinishCancelMessage finishCancelMessage = JsonUtils.DeserializeSdk<FinishCancelMessage>(rMessage.payload);
                        notifyObserversFinishCancel(finishCancelMessage.requestInfo);
                        break;
                    case Methods.FINISH_OK:
                        FinishOkMessage fokmsg = JsonUtils.DeserializeSdk<FinishOkMessage>(rMessage.payload);
                        notifyObserversFinishOk(fokmsg);
                        break;
                    case Methods.KEY_PRESS:
                        KeyPressMessage kpm = JsonUtils.DeserializeSdk<KeyPressMessage>(rMessage.payload);
                        notifyObserversKeyPressed(kpm);
                        break;
                    case Methods.ORDER_ACTION_RESPONSE:
                        break;
                    case Methods.PARTIAL_AUTH:
                        PartialAuthMessage partialAuth = JsonUtils.DeserializeSdk<PartialAuthMessage>(rMessage.payload);
                        notifyObserversPartialAuth(partialAuth);
                        break;
                    case Methods.PAYMENT_VOIDED:
                        // this seems to only gets called if a Signature is "Canceled" on the device
                        break;
                    case Methods.CONFIRM_PAYMENT_MESSAGE:
                        setPaymentConfirmationIdle(false);
                        ConfirmPaymentMessage confirmPaymentMessage = JsonUtils.DeserializeSdk<ConfirmPaymentMessage>(rMessage.payload);
                        notifyObserversConfirmPayment(confirmPaymentMessage);
                        break;
                    case Methods.TIP_ADDED:
                        TipAddedMessage tipMessage = JsonUtils.DeserializeSdk<TipAddedMessage>(rMessage.payload);
                        notifyObserversTipAdded(tipMessage);
                        break;
                    case Methods.TX_START_RESPONSE:
                        TxStartResponseMessage txsrm = JsonUtils.DeserializeSdk<TxStartResponseMessage>(rMessage.payload);
                        notifyObserversTxStartResponse(txsrm);
                        break;
                    case Methods.TX_STATE:
                        TxStateMessage txStateMsg = JsonUtils.DeserializeSdk<TxStateMessage>(rMessage.payload);
                        notifyObserversTxState(txStateMsg);
                        break;
                    case Methods.UI_STATE:
                        UiStateMessage uiStateMsg = JsonUtils.DeserializeSdk<UiStateMessage>(rMessage.payload);
                        notifyObserversUiState(uiStateMsg);
                        break;
                    case Methods.VERIFY_SIGNATURE:
                        paymentRejected = false;
                        VerifySignatureMessage vsigMsg = JsonUtils.DeserializeSdk<VerifySignatureMessage>(rMessage.payload);
                        notifyObserversVerifySignature(vsigMsg);
                        break;
                    case Methods.REFUND_RESPONSE:
                        RefundResponseMessage refRespMsg = JsonUtils.DeserializeSdk<RefundResponseMessage>(rMessage.payload);
                        notifyObserversRefundPaymentResponse(refRespMsg);
                        break;
                    case Methods.TIP_ADJUST_RESPONSE:
                        TipAdjustResponseMessage tipAdjustMsg = JsonUtils.DeserializeSdk<TipAdjustResponseMessage>(rMessage.payload);
                        notifyObserversTipAdjusted(tipAdjustMsg);
                        break;
                    case Methods.REFUND_REQUEST:
                        //Outbound no-op
                        break;
                    case Methods.VAULT_CARD_RESPONSE:
                        VaultCardResponseMessage vcrMsg = JsonUtils.DeserializeSdk<VaultCardResponseMessage>(rMessage.payload);
                        notifyObserversVaultCardResponse(vcrMsg);
                        break;
                    case Methods.CARD_DATA_RESPONSE:
                        ReadCardDataResponseMessage rcdrMsg = JsonUtils.DeserializeSdk<ReadCardDataResponseMessage>(rMessage.payload);
                        notifyObserversReadCardDataResponse(rcdrMsg);
                        break;
                    case Methods.CAPTURE_PREAUTH_RESPONSE:
                        CapturePreAuthResponseMessage carMsg = JsonUtils.DeserializeSdk<CapturePreAuthResponseMessage>(rMessage.payload);
                        notifyObserversCapturePreAuthResponse(carMsg);
                        break;
                    case Methods.CLOSEOUT_RESPONSE:
                        CloseoutResponseMessage crMsg = JsonUtils.DeserializeSdk<CloseoutResponseMessage>(rMessage.payload);
                        notifyObserversCloseoutResponse(crMsg);
                        break;
                    case Methods.RETRIEVE_PENDING_PAYMENTS_RESPONSE:
                        RetrievePendingPaymentsResponseMessage rpprMsg = JsonUtils.DeserializeSdk<RetrievePendingPaymentsResponseMessage>(rMessage.payload);
                        notifyObserversPendingPaymentsResponse(rpprMsg);
                        break;
                    case Methods.ACTIVITY_RESPONSE:
                        ActivityResponseMessage arm = JsonUtils.DeserializeSdk<ActivityResponseMessage>(rMessage.payload);
                        notifyObserversActivityResponse(arm);
                        break;
                    case Methods.ACTIVITY_MESSAGE_FROM_ACTIVITY:
                        ActivityMessageFromActivity amfa = JsonUtils.DeserializeSdk<ActivityMessageFromActivity>(rMessage.payload);
                        notifyObserversActivityMessage(amfa);
                        break;
                    case Methods.RESET_DEVICE_RESPONSE:
                        ResetDeviceResponseMessage rdrm = JsonUtils.DeserializeSdk<ResetDeviceResponseMessage>(rMessage.payload);
                        notifyObserversDeviceReset(rdrm);
                        break;
                    case Methods.RETRIEVE_DEVICE_STATUS_RESPONSE:
                        RetrieveDeviceStatusResponseMessage rdsrm = JsonUtils.DeserializeSdk<RetrieveDeviceStatusResponseMessage>(rMessage.payload);
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
                        CreditPrintMessage cpm = JsonUtils.DeserializeSdk<CreditPrintMessage>(rMessage.payload);
                        notifyObserversPrintCredit(cpm);
                        break;
                    case Methods.PRINT_CREDIT_DECLINE:
                        DeclineCreditPrintMessage dcpm = JsonUtils.DeserializeSdk<DeclineCreditPrintMessage>(rMessage.payload);
                        notifyObserversPrintCreditDecline(dcpm);
                        break;
                    case Methods.PRINT_PAYMENT:
                        PaymentPrintMessage ppm = JsonUtils.DeserializeSdk<PaymentPrintMessage>(rMessage.payload);
                        notifyObserversPrintPayment(ppm);
                        break;
                    case Methods.PRINT_PAYMENT_DECLINE:
                        DeclinePaymentPrintMessage dppm = JsonUtils.DeserializeSdk<DeclinePaymentPrintMessage>(rMessage.payload);
                        notifyObserversPrintPaymentDecline(dppm);
                        break;
                    case Methods.PRINT_PAYMENT_MERCHANT_COPY:
                        PaymentPrintMerchantCopyMessage ppmcm = JsonUtils.DeserializeSdk<PaymentPrintMerchantCopyMessage>(rMessage.payload);
                        notifyObserversPrintMerchantCopy(ppmcm);
                        break;
                    case Methods.REFUND_PRINT_PAYMENT:
                        RefundPaymentPrintMessage rppm = JsonUtils.DeserializeSdk<RefundPaymentPrintMessage>(rMessage.payload);
                        notifyObserversPrintRefund(rppm);
                        break;
                    case Methods.RETRIEVE_PAYMENT_RESPONSE:
                        RetrievePaymentResponseMessage rprm = JsonUtils.DeserializeSdk<RetrievePaymentResponseMessage>(rMessage.payload);
                        notifyObserversRetrievePaymentResponse(rprm);
                        break;
                    case Methods.GET_PRINTERS_RESPONSE:
                        RetrievePrintersResponseMessage rtrm = JsonUtils.DeserializeSdk<RetrievePrintersResponseMessage>(rMessage.payload);
                        notifyObserversRetrievePrinterResponse(rtrm);
                        break;
                    case Methods.PRINT_JOB_STATUS_RESPONSE:
                        PrintJobStatusResponseMessage pjsrm = JsonUtils.DeserializeSdk<PrintJobStatusResponseMessage>(rMessage.payload);
                        notifyObserversRetrievePrintJobStatus(pjsrm);
                        break;
                    case Methods.SHOW_RECEIPT_OPTIONS_RESPONSE:
                        ShowReceiptOptionsResponseMessage srorm = JsonUtils.DeserializeSdk<ShowReceiptOptionsResponseMessage>(rMessage.payload);
                        notifyObserverDisplayReceiptOptionsResponse(srorm);
                        break;
                    case Methods.CUSTOMER_PROVIDED_DATA_MESSAGE:
                        notifyObserversCustomerProvidedData(JsonUtils.DeserializeSdk<CustomerProvidedDataResponseMessage>(rMessage.payload));
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
                    case Methods.VOID_PAYMENT_RESPONSE:
                        VoidPaymentResponseMessage vprm = JsonUtils.DeserializeSdk<VoidPaymentResponseMessage>(rMessage.payload);
                        notifyObserversPaymentVoided(vprm);
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
                    case Methods.CLOVER_DEVICE_LOG_REQUEST:
                        //Outbound no-op
                        break;
                    default:
                        // Messsage Method not recognized or null: usually rMessage.type == MessageTypes.PING instead of normal Command message
                        if (rMessage.type == MessageTypes.PING)
                        {
                            onPing();
                        }
                        break;
                }
            }
        }

        private void onPing()
        {
            // if pong then pong
            doPong();

            // if in discovery state and receiving ping, probably the DISCOVERY RESPONSE went astray, send another DISCOVERY REQUEST to get another one
            if (startupConnectionState == ConnectionState.Discovering)
            {
                doDiscoveryRequest();
            }
        }

        public void notifyObserversRefundPaymentResponse(RefundResponseMessage rrm)
        {
            NotifyObservers(observer =>
            {
                observer.onRefundPaymentResponse(rrm.refund, rrm.orderId, rrm.paymentId, rrm.code, rrm.reason.ToString() + " " + rrm.message, rrm.reason);
            });
        }

        public void notifyObserversTipAdjusted(TipAdjustResponseMessage tarm)
        {
            NotifyObservers(observer =>
            {
                observer.onAuthTipAdjusted(tarm.paymentId, tarm.amount, tarm.success);
            });
        }

        public void notifyObserversVaultCardResponse(VaultCardResponseMessage vcrm)
        {
            NotifyObservers(observer =>
            {
                observer.onVaultCardResponse(vcrm);
            });
        }

        public void notifyObserversReadCardDataResponse(ReadCardDataResponseMessage cdrm)
        {
            NotifyObservers(observer =>
            {
                observer.onReadCardDataResponse(cdrm);
            });
        }

        public void notifyObserversDiscoveryResponse(DiscoveryResponseMessage drMessage)
        {
            startupConnectionState = ConnectionState.Discovered;

            NotifyObservers(observer =>
            {
                if (drMessage.ready)
                {
                    observer.onDeviceReady(this, drMessage);
                }
                else
                {
                    observer.onDeviceConnected();
                }
            });
        }

        public void notifyObserversCapturePreAuthResponse(CapturePreAuthResponseMessage carm)
        {
            NotifyObservers(observer =>
            {
                observer.onCapturePreAuthResponse(carm.paymentId, carm.amount, carm.tipAmount, carm.status, carm.reason);
            });
        }

        public void notifyObserversCloseoutResponse(CloseoutResponseMessage crm)
        {
            NotifyObservers(observer =>
            {
                observer.onCloseoutResponse(crm.status, crm.reason, crm.batch);
            });
        }

        public void notifyObserversPendingPaymentsResponse(RetrievePendingPaymentsResponseMessage rpprm)
        {
            NotifyObservers(observer =>
            {
                observer.onRetrievePendingPaymentsResponse(rpprm.status == ResultStatus.SUCCESS, rpprm.pendingPaymentEntries);
            });
        }

        public void notifyObserversActivityResponse(ActivityResponseMessage arm)
        {
            NotifyObservers(observer =>
            {
                ResultStatus status = arm.resultCode == -1 ? ResultStatus.SUCCESS : ResultStatus.CANCEL;
                observer.onActivityResponse(status, arm.action, arm.payload, arm.failReason);
            });
        }

        public void notifyObserversKeyPressed(KeyPressMessage keyPress)
        {
            NotifyObservers(observer =>
            {
                observer.onKeyPressed(keyPress.keyPress);
            });
        }

        public void notifyObserversCashbackSelected(CashbackSelectedMessage cbSelected)
        {
            NotifyObservers(observer =>
            {
                observer.onCashbackSelected(cbSelected.cashbackAmount);
            });
        }

        public void notifyObserversConfirmPayment(ConfirmPaymentMessage message)
        {
            NotifyObservers(observer =>
            {
                observer.onConfirmPayment(message.payment, message.challenges);
            });
        }

        public void notifyObserversTipAdded(TipAddedMessage tipAdded)
        {
            NotifyObservers(observer =>
            {
                observer.onTipAdded(tipAdded.tipAmount);
            });
        }

        public void notifyObserversTxStartResponse(TxStartResponseMessage txsrm)
        {
            NotifyObservers(observer =>
            {
                observer.onTxStartResponse(txsrm.result, txsrm.externalId);
            });
        }

        public void notifyObserversPartialAuth(PartialAuthMessage partialAuth)
        {
            NotifyObservers(observer =>
            {
                observer.onPartialAuth(partialAuth.partialAuthAmount);
            });
        }

        public void notifyObserversPaymentVoided(VoidPaymentResponseMessage vprm)
        {
            NotifyObservers(observer =>
            {
                observer.onPaymentVoided(vprm.payment, vprm.voidReason, vprm.status, vprm.reason, vprm.message);
            });
        }

        public void notifyObserversVerifySignature(VerifySignatureMessage verifySigMsg)
        {
            NotifyObservers(observer =>
            {
                observer.onVerifySignature(verifySigMsg.payment, verifySigMsg.signature);
            });
        }


        public void notifyObserversUiState(UiStateMessage uiStateMsg)
        {
            NotifyObservers(observer =>
            {
                observer.onUiState(uiStateMsg.uiState, uiStateMsg.uiText, uiStateMsg.uiDirection, uiStateMsg.inputOptions);
            });
        }


        public void notifyObserversTxState(TxStateMessage txStateMsg)
        {
            NotifyObservers(observer =>
            {
                observer.onTxState(txStateMsg.txState);
            });
        }

        public void notifyObserversFinishCancel(TxType requestInfo)
        {
            NotifyObservers(observer =>
            {
                observer.onFinishCancel(requestInfo);
            });
        }

        public void notifyObserversFinishOk(FinishOkMessage msg)
        {
            NotifyObservers(observer =>
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
                    // Console.WriteLine("Don't know what to do with this Finish OK message: " + JsonUtils.Serialize(msg));
                }
            });
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

                NotifyObservers(observer =>
                {
                    observer.onMessageAck(ackMessage.sourceMessageId);
                });
            }
        }

        public void notifyObserversPrintCredit(CreditPrintMessage cpm)
        {
            NotifyObservers(observer =>
            {
                observer.onPrintCredit(cpm.credit);
            });
        }

        public void notifyObserversPrintCreditDecline(DeclineCreditPrintMessage dcpm)
        {
            NotifyObservers(observer =>
            {
                observer.onPrintCreditDecline(dcpm.credit, dcpm.reason);
            });
        }

        public void notifyObserversPrintPayment(PaymentPrintMessage ppm)
        {
            NotifyObservers(observer =>
            {
                observer.onPrintPayment(ppm.payment, ppm.order);
            });
        }

        public void notifyObserversPrintPaymentDecline(DeclinePaymentPrintMessage dppm)
        {
            NotifyObservers(observer =>
            {
                observer.onPrintPaymentDecline(dppm.payment, dppm.reason);
            });
        }

        public void notifyObserversPrintMerchantCopy(PaymentPrintMerchantCopyMessage ppmcm)
        {
            NotifyObservers(observer =>
            {
                observer.onPrintMerchantReceipt(ppmcm.payment);
            });
        }

        public void notifyObserversPrintRefund(RefundPaymentPrintMessage rppm)
        {
            NotifyObservers(observer =>
            {
                observer.onPrintRefundPayment(rppm.payment, rppm.order, rppm.refund);
            });
        }


        public void notifyObserversActivityMessage(ActivityMessageFromActivity amfa)
        {
            NotifyObservers(observer =>
            {
                observer.onMessageFromActivity(amfa.action, amfa.payload);
            });
        }

        public void notifyObserversDeviceReset(ResetDeviceResponseMessage rdrm)
        {
            NotifyObservers(observer =>
            {
                observer.onResetDeviceResponse(ResultStatus.SUCCESS, rdrm.reason, rdrm.state);
            });
        }

        public void notifyObserversRetrieveDeviceStatusResponse(RetrieveDeviceStatusResponseMessage rdsrm)
        {
            NotifyObservers(observer =>
            {
                observer.onDeviceStatusResponse(ResultStatus.SUCCESS, rdsrm.reason, rdsrm.state, rdsrm.data);
            });
        }

        public void notifyObserversRetrievePaymentResponse(RetrievePaymentResponseMessage rpr)
        {
            NotifyObservers(observer =>
            {
                observer.onRetrievePaymentResponse(ResultStatus.SUCCESS, rpr.reason, rpr.externalPaymentId, rpr.queryStatus, rpr.payment);
            });
        }

        public void notifyObserversRetrievePrinterResponse(RetrievePrintersResponseMessage response)
        {
            NotifyObservers(observer =>
            {
                observer.onRetrievePrintersResponse(response.printers);
            });
        }

        public void notifyObserversRetrievePrintJobStatus(PrintJobStatusResponseMessage response)
        {
            NotifyObservers(observer =>
            {
                observer.onRetrievePrintJobStatus(response.externalPrintJobId, response.status);
            });
        }

        public void notifyObserverDisplayReceiptOptionsResponse(ShowReceiptOptionsResponseMessage srorm)
        {
            NotifyObservers(observer =>
            {
                observer.onDisplayReceiptOptionsResponse(srorm.status, srorm.reason);
            });
        }

        public void notifyObserversCustomerProvidedData(CustomerProvidedDataResponseMessage response)
        {
            NotifyObservers(observer =>
            {
                observer.onCustomerProvidedDataResponse(response.eventId, response.config, response.data);
            });
        }

        private void NotifyObservers(Action<ICloverDeviceObserver> action)
        {
            List<ICloverDeviceObserver> localObservers = new List<ICloverDeviceObserver>(deviceObservers);
            foreach (ICloverDeviceObserver observer in localObservers)
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += delegate
                {
                    try
                    {
                        action(observer);
                    }
                    catch (Exception exception)
                    {
                        // eat unhandled exceptions from user code - any logging other than debug?
                        System.Diagnostics.Debug.WriteLine("DefaultCloverDevice: Error calling custom code: " + exception);
                    }
                };
                bw.RunWorkerAsync();
            }
        }

        public override void doShowPaymentReceiptScreen(string orderId, string paymentId, bool disablePrinting)
        {
            sendObjectMessage(new ShowPaymentReceiptOptionsMessage(orderId, paymentId, disablePrinting));
        }

        public override void doShowReceiptScreen(string orderId, string paymentId, string refundId, string creditId, bool disablePrinting)
        {
            sendObjectMessage(new ShowReceiptOptionsMessage(orderId, paymentId, refundId, creditId, disablePrinting));
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

        public override void doSendDebugLog(string message)
        {
            sendObjectMessage(new CloverDeviceLogMessage(message));
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

        public override void doTipAdjustAuth(string orderId, string paymentId, long? amount)
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
            VoidPaymentMessage vpm = new VoidPaymentMessage
            {
                payment = payment,
                voidReason = reason
            };
            sendObjectMessage(vpm);
        }

        public override void doVoidPaymentRefund(string orderId, string refundId, bool disablePrinting, bool disableReceiptSelection, string employeeId)
        {
            // Clover connector should report a nice error and block ever getting here, but stop the process if we get here.
            throw new NotSupportedException("VoidPaymentRefund is not supported in this version");
        }

        public override void doRefundPayment(string orderId, string paymentId, long? amount, bool? fullRefund, bool? disableCloverPrinting, bool? disableReceiptSelection)
        {
            sendObjectMessage(new RefundRequestMessage(orderId, paymentId, amount, fullRefund, disableCloverPrinting, disableReceiptSelection));
        }

        private void doPong()
        {
            // Send special Pong message
            RemoteMessage remoteMessage = RemoteMessage.CreatePongMessage(packageName, remoteSourceSDK, remoteApplicationID);
            string msg = JsonUtils.SerializeSdk(remoteMessage);
            transport.sendMessage(msg);
#if DEBUG
            Console.WriteLine("Sent message: " + msg);
#endif
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

        #region Loyalty API

        /// <summary>
        /// Loyalty API: Register to receive specific loyalty data types
        /// </summary>
        /// <param name="configs"></param>
        public override void doRegisterForCustomerProvidedData(List<LoyaltyDataConfig> configs) => sendObjectMessage(new RegisterForCustomerProvidedDataMessage() { configurations = configs });

        /// <summary>
        /// Loyalty API: Set customer info 
        /// </summary>
        /// <param name="customerInfo"></param>
        public override void doSetCustomerInfo(CustomerInfo customerInfo)
        {
            CustomerInfoMessage message = new CustomerInfoMessage
            {
                customer = customerInfo
            };

            sendObjectMessage(message);
        }

        #endregion

        #region Send Messages via Transport
        private string sendObjectMessage(Message message)
        {
            RemoteMessage remoteMessage = RemoteMessage.createMessage(message.method, MessageTypes.COMMAND, message, this.packageName, remoteSourceSDK, remoteApplicationID);
            string msg = JsonUtils.SerializeSdk(remoteMessage);

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
                        // Console.WriteLine("Error sending message - payload size is greater than the maximum allowed");
                        return null;
                    }

                    int fragmentIndex = 0;
                    string payloadStr = remoteMsg.payload ?? "";

                    int startIndex = 0;
                    while (startIndex < payloadStr.Length)
                    {
                        int length = (maxSize < payloadStr.Length ? maxSize : payloadStr.Length);
                        string fPayload = payloadStr.Substring(startIndex, length);
                        startIndex += length;
                        bool noAttachmentAvailable = string.IsNullOrEmpty(remoteMsg.attachment);
                        bool noAttachmentUriAvailable = string.IsNullOrEmpty(remoteMsg.attachmentUri);
                        bool lastFragment = payloadStr.Length == 0 && (noAttachmentAvailable && noAttachmentUriAvailable);
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
            fRemoteMessage.attachmentEncoding = remoteMsg.attachmentEncoding ?? "BASE64.FRAGMENT";
            fRemoteMessage.attachment = fAttachment;
            fRemoteMessage.fragmentIndex = fragmentIndex;
            fRemoteMessage.lastFragment = lastFragment;

            string msg = JsonUtils.SerializeSdk(fRemoteMessage);
            transport.sendMessage(msg);

#if DEBUG
            Console.WriteLine("Sent message: " + msg);
#endif
        }

        #endregion
    }
}
