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

namespace com.clover.remotepay.transport
{
    public class DefaultCloverDevice : CloverDevice, CloverTransportObserver
    {
        private RefundResponseMessage lastRefundResponseMessage { get; set; }

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
        public void onMessage(string message)
        {
#if DEBUG
            Console.WriteLine("Received message: " + message);
#endif
            //CloverTransportObserver
            // Deserialize the message object to a real object, and figure
            RemoteMessage rMessage = JsonUtils.deserialize<RemoteMessage>(message);
            switch (rMessage.method)
            {
                case Methods.BREAK:
                    break;
                case Methods.CASHBACK_SELECTED:
                    CashbackSelectedMessage cbsMessage = JsonUtils.deserialize<CashbackSelectedMessage>(rMessage.payload);
                    notifyObserversCashbackSelected(cbsMessage);
                    break;
                case Methods.DISCOVERY_RESPONSE:
                    DiscoveryResponseMessage drMessage = JsonUtils.deserialize<DiscoveryResponseMessage>(rMessage.payload);
                    deviceInfo.name = drMessage.name;
                    deviceInfo.serial = drMessage.serial;
                    deviceInfo.model = drMessage.model;
                    notifyObserversDiscoveryResponse(drMessage);
                    break;
                case Methods.FINISH_CANCEL:
                    notifyObserversFinishCancel();
                    break;
                case Methods.FINISH_OK:
                    FinishOkMessage fokmsg = JsonUtils.deserialize<FinishOkMessage>(rMessage.payload);
                    if (null!= fokmsg.payment) fokmsg.paymentObj = JsonUtils.deserialize<Payment>(fokmsg.payment);
                    if (null != fokmsg.credit) fokmsg.creditObj = JsonUtils.deserialize<Credit>(fokmsg.credit);
                    //if (null != fokmsg.refund) fokmsg.refundObj = parseRefund(fokmsg.refund); //not currently implemented, uses REFUND_RESPONSE
                    notifyObserversFinishOk(fokmsg);
                    break;
                case Methods.KEY_PRESS:
                    KeyPressMessage kpm = JsonUtils.deserialize<KeyPressMessage>(rMessage.payload);
                    notifyObserversKeyPressed(kpm);
                    break;
                case Methods.ORDER_ACTION_RESPONSE:
                    break;
                case Methods.PARTIAL_AUTH:
                    PartialAuthMessage partialAuth = JsonUtils.deserialize<PartialAuthMessage>(rMessage.payload);
                    notifyObserversPartialAuth(partialAuth);
                    break;
                case Methods.PAYMENT_VOIDED:
                    //VoidPaymentMessage vpMessage = JsonUtils.deserialize<VoidPaymentMessage>(rMessage.payload);
                    //Payment payment = JsonUtils.deserialize<Payment>(vpMessage.payment);
                    //notifyObserversPaymentVoided(payment, vpMessage.voidReason);
                    // this seems to only gets called if a Signature is "Canceled" on the device
                    break;
                case Methods.TIP_ADDED:
                    TipAddedMessage tipMessage = JsonUtils.deserialize<TipAddedMessage>(rMessage.payload);
                    notifyObserversTipAdded(tipMessage);
                    break;
                case Methods.TX_START_RESPONSE:
                    break;
                case Methods.TX_STATE:
                    TxStateMessage txStateMsg = JsonUtils.deserialize<TxStateMessage>(rMessage.payload);
                    notifyObserversTxState(txStateMsg);
                    break;
                case Methods.UI_STATE:
                    UiStateMessage uiStateMsg = JsonUtils.deserialize<UiStateMessage>(rMessage.payload);
                    notifyObserversUiState(uiStateMsg);
                    break;
                case Methods.VERIFY_SIGNATURE:
                    VerifySignatureMessage vsigMsg = JsonUtils.deserialize<VerifySignatureMessage>(rMessage.payload);
                    if (null != vsigMsg.payment) vsigMsg.paymentObj = JsonUtils.deserialize<Payment>(vsigMsg.payment);
                    notifyObserversVerifySignature(vsigMsg);
                    break;
                case Methods.REFUND_RESPONSE:
                    RefundResponseMessage refRespMsg = JsonUtils.deserialize<RefundResponseMessage>(rMessage.payload);
                    notifyObserversPaymentRefundResponse(refRespMsg);
                    break;
                case Methods.TIP_ADJUST_RESPONSE:
                    TipAdjustResponseMessage tipAdjustMsg = JsonUtils.deserialize<TipAdjustResponseMessage>(rMessage.payload);
                    notifyObserversTipAdjusted(tipAdjustMsg);
                    break;
                case Methods.REFUND_REQUEST:
                    //Outbound no-op
                    break;
                case Methods.VAULT_CARD_RESPONSE:
                    VaultCardResponseMessage vcrMsg = JsonUtils.deserialize<VaultCardResponseMessage>(rMessage.payload);
                    if(vcrMsg.card != null)
                    {
                        vcrMsg.cardObj = JsonUtils.deserialize<VaultedCard>(vcrMsg.card);
                    }
                    notifyObserversVaultCardResponse(vcrMsg);
                    break;
                case Methods.CAPTURE_PREAUTH_RESPONSE:
                    CaptureAuthResponseMessage carMsg = JsonUtils.deserialize<CaptureAuthResponseMessage>(rMessage.payload);
                    notifyObserversCapturePreAuthResponse(carMsg);
                    break;
                case Methods.CLOSEOUT_RESPONSE:
                    CloseoutResponseMessage crMsg = JsonUtils.deserialize<CloseoutResponseMessage>(rMessage.payload);
                    if (null != crMsg.batch) crMsg.batchObj = parseBatch(crMsg.batch);
                    notifyObserversCloseoutResponse(crMsg);
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
                    //Outbound no-op
                    break;
                case Methods.PRINT_CREDIT_DECLINE:
                    //Outbound no-op
                    break;
                case Methods.PRINT_IMAGE:
                    //Outbound no-op
                    break;
                case Methods.PRINT_PAYMENT:
                    //Outbound no-op
                    break;
                case Methods.PRINT_PAYMENT_DECLINE:
                    //Outbound no-op
                    break;
                case Methods.PRINT_PAYMENT_MERCHANT_COPY:
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

            }
            // out what to do with it.
        }

        private Batch parseBatch(string batchString)
        {
            // the refund has serverTotals and cardTotals, with the array as a value of elements,
            // need to remove the elements and set the value of elements
            // to the value of serverTotals and cardTotals
            JObject mainobject = JsonConvert.DeserializeObject<JObject>(batchString);
            JObject jobject = ((JObject)mainobject.GetValue("batchDetails"));

            JObject serverTotals = ((JObject)jobject.GetValue("serverTotals"));
            if (serverTotals != null)
            {
                // playing parsing games. bean assumes no "elements" element, so we will just replace it.
                // convert {serverTotals:{elements:[A,B,C]}} to {serverTotals:[A,B,C]}
                JArray elements = (JArray)serverTotals.GetValue("elements");
                serverTotals.Replace(elements);
            }
            JObject cardTotals = ((JObject)jobject.GetValue("cardTotals"));
            if (cardTotals != null)
            {
                // playing parsing games. bean assumes no "elements" element, so we will just replace it.
                // convert {cardTotals:{elements:[A,B,C]}} to {cardTotals:[A,B,C]}
                JArray elements = (JArray)cardTotals.GetValue("elements");
                cardTotals.Replace(elements);
            }

            string modifiedBatch = JsonConvert.SerializeObject(jobject);
            Batch batch = JsonUtils.deserialize<Batch>(modifiedBatch);
            return batch;
        }
        private Refund parseRefund(string refundString)
        {
            // the refund has taxableAmountRates, with the array as a value of elements,
            // need to remove the elements and set the value of elements
            // to the value of taxableAmountRates
            JObject jobject = JsonConvert.DeserializeObject<JObject>(refundString);
            JObject taxableAmountRates = ((JObject)jobject.GetValue("taxableAmountRates"));
            if (taxableAmountRates != null)
            {
                // playing parsing games. bean assumes no "elements" element, so we will just replace it.
                // convert {taxableAmountRates:{elements:[A,B,C]}} to {taxableAmountRates:[A,B,C]}
                JArray elements = (JArray)taxableAmountRates.GetValue("elements");
                taxableAmountRates.Replace(elements);
            }

            string modifiedRefund = JsonConvert.SerializeObject(jobject);
            Refund refund = JsonUtils.deserialize<Refund>(modifiedRefund);
            return refund;
        }
        //---------------------------------------------------
        /// <summary>
        /// this is for a payment refund
        /// </summary>
        /// <param name="rrm"></param>
        public void notifyObserversPaymentRefundResponse(RefundResponseMessage rrm)
        {

            foreach (CloverDeviceObserver observer in deviceObservers)
            {
                BackgroundWorker bw = new BackgroundWorker();
                // what to do in the background thread
                bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    string refundString = rrm.refund;
                    Refund refund = null;
                    if (refundString != null)
                    {
                        refund = parseRefund(refundString);
                    }
                    observer.onPaymentRefundResponse(rrm.orderId, rrm.paymentId, refund, rrm.code, rrm.message);
                });
                bw.RunWorkerAsync();
            }
        }
        public void notifyObserversTipAdjusted(TipAdjustResponseMessage tarm)
        {
            foreach (CloverDeviceObserver observer in deviceObservers)
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
                foreach (CloverDeviceObserver observer in deviceObservers)
                {
                    observer.onVaultCardResponse(vcrm);
                }
            });
            bw.RunWorkerAsync();
        }
        public void notifyObserversDiscoveryResponse(DiscoveryResponseMessage drMessage)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args) {
                foreach (CloverDeviceObserver observer in deviceObservers)
                {
                    observer.onDeviceReady(drMessage);
                    //observer.onDiscoveryResponse(drMessage);
                }
            });
            bw.RunWorkerAsync();
        }
        public void notifyObserversCapturePreAuthResponse(CaptureAuthResponseMessage carm)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args) {
                foreach (CloverDeviceObserver observer in deviceObservers)
                {
                    observer.onCaptureAuthResponse(carm);
                }
            });
            bw.RunWorkerAsync();
        }
        public void notifyObserversCloseoutResponse(CloseoutResponseMessage crm)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args) {
                foreach (CloverDeviceObserver observer in deviceObservers)
                {
                    Batch batch = JsonConvert.DeserializeObject<Batch>(crm.batch);
                    observer.onCloseoutResponse(crm.status, crm.reason, batch);
                }
            });
            bw.RunWorkerAsync();
        }
        public void notifyObserversKeyPressed(KeyPressMessage keyPress)
        {
            foreach (CloverDeviceObserver observer in deviceObservers)
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
            foreach (CloverDeviceObserver observer in deviceObservers)
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

        public void notifyObserversTipAdded(TipAddedMessage tipAdded)
        {
            foreach(CloverDeviceObserver observer in deviceObservers)
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

        public void notifyObserversPartialAuth(PartialAuthMessage partialAuth)
        {
            foreach (CloverDeviceObserver observer in deviceObservers)
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
            foreach (CloverDeviceObserver observer in deviceObservers)
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
            foreach (CloverDeviceObserver observer in deviceObservers)
            {
                observer.onVerifySignature(verifySigMsg.paymentObj, verifySigMsg.signature);
            }
        }


        public void notifyObserversUiState(UiStateMessage uiStateMsg)
        {
            foreach (CloverDeviceObserver observer in deviceObservers)
            {
                observer.onUiState(uiStateMsg.uiState, uiStateMsg.uiText, uiStateMsg.uiDirection, uiStateMsg.inputOptions);
            }
        }


        public void notifyObserversTxState(TxStateMessage txStateMsg)
        {
            foreach (CloverDeviceObserver observer in deviceObservers)
            {
                observer.onTxState(txStateMsg.txState);
            }
        }

        public void notifyObserversFinishCancel()
        {
            foreach (CloverDeviceObserver observer in deviceObservers)
            {
                observer.onFinishCancel();
            }
        }
        public void notifyObserversFinishOk(FinishOkMessage msg)

        {
            foreach (CloverDeviceObserver observer in deviceObservers)
            {
                if (msg.payment != null)
                {
                    observer.onFinishOk(msg.paymentObj, msg.signature);
                }
                else if (msg.credit != null)
                {
                    observer.onFinishOk(msg.creditObj);
                }
                else if (msg.refund != null)
                {
                    observer.onFinishOk(msg.refundObj);
                }
            }
        }

        public override void doShowPaymentReceiptScreen(string orderId, string paymentId)
        {
            sendObjectMessage(new PaymentReceiptMessage(orderId, paymentId));
        }

        public override void doShowRefundReceiptScreen(string orderId, string refundId)
        {
            sendObjectMessage(new RefundReceiptMessage(orderId, refundId));
        }

        public override void doShowCreditReceiptScreen(string orderId, string creditId)
        {
            sendObjectMessage(new CreditReceiptMessage(orderId, creditId));
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

        public override void doSignatureVerified(Payment payment, bool verified)
        {
            sendObjectMessage(new SignatureVerifiedMessage(payment, verified));
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

        public override void doCloseout(bool allowOpenTabs, string batchId)
        {
            sendObjectMessage(new CloseoutMessage(allowOpenTabs, batchId));
        }

        public override void doResetDevice()
        {
            sendObjectMessage(new BreakMessage());
        }

        public override void doTxStart(PayIntent payIntent, Order order, bool suppressOnScreenTips)
        {
            sendObjectMessage(new TxStartRequestMessage(payIntent, order, suppressOnScreenTips));
        }

        public override void doTipAdjustAuth(string orderId, string paymentId, long amount)
        {
            sendObjectMessage(new TipAdjustAuthMessage(orderId, paymentId, amount));
        }

        public override void doCaptureAuth(string paymentID, long amount, long tipAmount)
        {
            sendObjectMessage(new CaptureAuthMessage(paymentID, amount, tipAmount));
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


        public override void doVoidPayment(Payment payment, VoidReason reason)
        {
            VoidPaymentMessage vpm = new VoidPaymentMessage();
            vpm.paymentObj = payment;
            vpm.voidReason = reason;
            sendObjectMessage(vpm);

            BackgroundWorker bw = new BackgroundWorker();
            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                notifyObserversPaymentVoided(payment, reason);
            });
            bw.RunWorkerAsync();
        }

        public override void doPaymentRefund(string orderId, string paymentId, long amount)
        {
            sendObjectMessage(new RefundRequestMessage(orderId, paymentId, amount));
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

        private void sendObjectMessage(Message message)
        {
            RemoteMessage remoteMessage = RemoteMessage.createMessage(
                message.method, MessageTypes.COMMAND, message, this.packageName, remoteSourceSDK, remoteApplicationID
            );

            string msg = JsonUtils.serialize(remoteMessage);
            // string msg = JsonConvert.SerializeObject(remoteMessage, new JsonConverter[] { new StringEnumConverter() });
            transport.sendMessage(msg);
        }

    }
}
