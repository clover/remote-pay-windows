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
using System.Threading;
using System.Collections.Generic;
using com.clover.remote.order.operation;
using com.clover.remote.order;
using com.clover.remotepay.data;

namespace com.clover.remotepay.transport
{
    class TestCloverDevice : CloverDevice, CloverTransportObserver
    {
        private string finishOKCredit { get; set; }
        private string finishOkPaymentNoSig { get; set; }
        private string finishOkPaymentWiSig { get; set; }
        private Dictionary<string, Object> TempObjectMap = new Dictionary<string, Object>();

        public TestCloverDevice(TestCloverDeviceConfiguration configuration) :
            this(configuration.getMessagePackageName())
        {
            transport = new TestCloverTransport();
        }

        public TestCloverDevice(String packageName) : base(packageName, null, null)
        {

        }

        public override void doRefundPayment(string orderId, string paymentId, long? amount, bool? fullRefund)
        {
            notifyObserversUiState(new UiStateMessage(UiState.RECEIPT_OPTIONS, "Customer is selecting receipt type.", UiDirection.ENTER, new InputOption[0]));
            Console.WriteLine("Received UiStateMessage: RECEIPT_OPTIONS Customer is selecting receipt type. Enter");
            Thread.Sleep(3000);
            notifyObserversUiState(new UiStateMessage(UiState.RECEIPT_OPTIONS, "Customer is selecting receipt type.", UiDirection.EXIT, new InputOption[0]));
            Console.WriteLine("Received UiStateMessage: RECEIPT_OPTIONS Customer is selecting receipt type. Exit");
            Thread.Sleep(3000);

            Refund refund = new Refund();
            refund.payment = new Reference();
            if (paymentId != null)
            {
                refund.payment.id = paymentId;
            }
            refund.orderRef = new Reference();
            if (orderId != null)
            {
                refund.orderRef.id = orderId;
            }
            Object tempPayment;
            TempObjectMap.TryGetValue(paymentId, out tempPayment);
            if (tempPayment != null)
            {
                if (((Payment)tempPayment).tipAmount > 0)
                {
                    refund.amount = ((Payment)tempPayment).amount + ((Payment)tempPayment).tipAmount;
                }
                else
                {
                    refund.amount = ((Payment)tempPayment).amount;
                }
            }
            refund.id = Guid.NewGuid().ToString();
            FinishOkMessage okMsg = new FinishOkMessage();
            okMsg.refund = refund;
            notifyObserversFinishOk(okMsg);
            Console.WriteLine("Received FINISH_OK: " + okMsg.refund);
        }

        public override void doTipAdjustAuth(string orderId, string paymentId, long amount)
        {
            TipAdjustResponseMessage tarm = new TipAdjustResponseMessage(orderId, paymentId, amount, true);
            Object tempPayment;
            TempObjectMap.TryGetValue(paymentId, out tempPayment);
            if (tempPayment != null)
            {
                ((Payment)tempPayment).tipAmount = amount;
                TempObjectMap.Remove(paymentId);
                TempObjectMap.Add(paymentId, tempPayment);
            }
            Console.WriteLine("Received TIP_ADJUST_RESONSE: " + JsonUtils.serialize(tarm));
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

        public override void doCapturePreAuth(string paymentID, long amount, long tipAmount)
        {
            throw new NotImplementedException();
        }

        //---------------------------------------------------
        public void onDeviceConnected(CloverTransport transport)
        {
            //CloverTransportObserver
        }

        public void onDeviceDisconnected(CloverTransport transport)
        {
            //CloverTransportObserver
        }

        public void onDeviceReady(CloverTransport device)
        {
            //CloverTransportObserver]
            
            foreach (CloverTransportObserver devOvs in deviceObservers)
            {
                devOvs.onDeviceReady(transport);
            }
        }

        //---------------------------------------------------

        public void notifyObserversPaymentVoided(Payment payment, VoidReason reason)
        {

                BackgroundWorker bw = new BackgroundWorker();
                // what to do in the background thread
                bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    foreach (ICloverDeviceObserver observer in deviceObservers)
                    {
                        observer.onPaymentVoided(payment, reason);
                    }
                });
                bw.RunWorkerAsync();
            
        }
        public void notifyObserversCardVaulted(VaultCardResponseMessage vcrm)
        {

            BackgroundWorker bw = new BackgroundWorker();
            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                foreach (ICloverDeviceObserver observer in deviceObservers)
                {
                    observer.onVaultCardResponse(vcrm);
                }
            });
            bw.RunWorkerAsync();

        }
        public void notifyObserversVerifySignature(VerifySignatureMessage verifySigMsg)
        {
            BackgroundWorker bw = new BackgroundWorker();
            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                foreach (ICloverDeviceObserver observer in deviceObservers)
                {
                    observer.onVerifySignature(verifySigMsg.payment, verifySigMsg.signature);
                }
            });
            bw.RunWorkerAsync();
        }


        public void notifyObserversUiState(UiStateMessage uiStateMsg)
        {
            BackgroundWorker bw = new BackgroundWorker();
            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                foreach (ICloverDeviceObserver observer in deviceObservers)
                {
                    observer.onUiState(uiStateMsg.uiState, uiStateMsg.uiText, uiStateMsg.uiDirection, uiStateMsg.inputOptions);
                }
            });
            bw.RunWorkerAsync();
        }


        public void notifyObserversTxState(TxStateMessage txStateMsg)
        {
            BackgroundWorker bw = new BackgroundWorker();
            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                foreach (ICloverDeviceObserver observer in deviceObservers)
                {
                    observer.onTxState(txStateMsg.txState);
                }
            });
            bw.RunWorkerAsync();
        }

        public void notifyObserversFinishCancel()
        {
            BackgroundWorker bw = new BackgroundWorker();
            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                foreach (ICloverDeviceObserver observer in deviceObservers)
                {
                    observer.onFinishCancel();
                }
            });
            bw.RunWorkerAsync();
        }
        public void notifyObserversFinishOk(FinishOkMessage msg)
        {
            BackgroundWorker bw = new BackgroundWorker();
            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
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
                }
            });
            bw.RunWorkerAsync();
        }
        public void notifyObserversCloseout()
        {
            BackgroundWorker bw = new BackgroundWorker();
            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs args)
            {
                foreach (ICloverDeviceObserver observer in deviceObservers)
                {
                    observer.onCloseoutResponse(ResultStatus.SUCCESS, "", null);
                }
            });
            bw.RunWorkerAsync();
        }
        public override void doKeyPress(KeyPress keyPress)
        {
            //TODO: implement
        }

        public override void doPrintText(List<string> textLines)
        {
            //TODO: implement
        }

        public override void doPrintImage(string base64String)
        {
            //TODO: implement
        }

        public override void doPrintImageURL(string urlString)
        {
            //TODO: implement
        }

        public override void doLogMessages(LogLevelEnum loglevel, Dictionary<string, string> messages)
        {
            //TODO: implement
        }

        public override void doShowPaymentReceiptScreen(string orderId, string paymentId)
        {
            //sendObjectMessage(new Message(Methods.SHOW_PAYMENT_RECEIPT_OPTIONS));
        }

        /*
        public override void doShowRefundReceiptScreen(string orderId, string refundId)
        {
            //sendObjectMessage(new Message(Methods.SHOW_REFUND_RECEIPT_OPTIONS));
        }

        public override void doShowCreditReceiptScreen(string orderId, string creditId)
        {
            //sendObjectMessage(new Message(Methods.SHOW_CREDIT_RECEIPT_OPTIONS));
        }
        */

        public override void doShowThankYouScreen()
        {
            //sendObjectMessage(new Message(Methods.SHOW_THANK_YOU_SCREEN));
        }

        public override void doShowWelcomeScreen()
        {
            //sendObjectMessage(new Message(Methods.SHOW_WELCOME_SCREEN));
        }

        public override void doVerifySignature(Payment payment, bool verified)
        {
            if(verified)
            {
                BackgroundWorker bw = new BackgroundWorker();
                // what to do in the background thread
                bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    notifyObserversUiState(new UiStateMessage(UiState.RECEIPT_OPTIONS, "Customer is selecting receipt type.", UiDirection.ENTER, new InputOption[0]));
                    Console.WriteLine("Received UiStateMessage: RECEIPT_OPTIONS Customer is selecting receipt type. Enter");
                    Thread.Sleep(3000);
                    notifyObserversUiState(new UiStateMessage(UiState.RECEIPT_OPTIONS, "Customer is selecting receipt type.", UiDirection.EXIT, new InputOption[0]));
                    Console.WriteLine("Received UiStateMessage: RECEIPT_OPTIONS Customer is selecting receipt type. Exit");

                    FinishOkMessage msg = new FinishOkMessage();
                    msg.payment = payment;
                    msg.payment.order = new Reference();
                    msg.payment.order.id = "abc123";
                    notifyObserversFinishOk(msg);
                    Console.WriteLine("Received FINISH_OK: " + msg.payment);
                });
                bw.RunWorkerAsync();
            }
            else
            {
                notifyObserversFinishCancel();
                Console.WriteLine("Received FinishCancelMessage: FINISH_CANCEL");
            }


        }

        public override void doTerminalMessage(string text)
        {
            //sendObjectMessage(new TerminalMessage(text));
        }

        public override void doOpenCashDrawer(string reason)
        {
            //sendObjectMessage(new OpenCashDrawer(reason));
        }

        public override void doTxStart(PayIntent payIntent, Order order)
        {
            BackgroundWorker bw = new BackgroundWorker();
            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                if (payIntent.amount > 0 && payIntent.transactionType == PayIntent.TransactionType.PAYMENT)
                {
                    if(payIntent.amount > 4000 && payIntent.transactionSettings != null &&
                       payIntent.transactionSettings.tipMode.HasValue &&
                       (!payIntent.transactionSettings.tipMode.Value.HasFlag(TipMode.NO_TIP) &&
                        !payIntent.transactionSettings.tipMode.Value.HasFlag(TipMode.TIP_PROVIDED)))
                    {
                        // let's tip
                        notifyObserversUiState(new UiStateMessage(UiState.ADD_TIP, "Customer is tipping...", UiDirection.ENTER, new InputOption[0]));
                        Console.WriteLine("Received UiStateMessage: ADD_TIP Customer is tipping... Enter");
                        Thread.Sleep(1000);
                        notifyObserversUiState(new UiStateMessage(UiState.ADD_TIP, "Customer is tipping...", UiDirection.EXIT, new InputOption[0]));
                        Console.WriteLine("Received UiStateMessage: ADD_TIP Customer is tipping... Exit");
                        payIntent.tipAmount = (long)(payIntent.amount * 0.1f);
                    }
                    notifyObserversUiState(new UiStateMessage(UiState.START, "Customer is choosing payment.", UiDirection.ENTER, new InputOption[0]));
                    Console.WriteLine("Received UiStateMessage: START Customer is choosing payment. Enter");
                    Thread.Sleep(1000);
                    notifyObserversUiState(new UiStateMessage(UiState.START, "Customer is choosing payment.", UiDirection.EXIT, new InputOption[0]));
                    Console.WriteLine("Received UiStateMessage: START Customer is choosing payment. Exit");
                    notifyObserversUiState(new UiStateMessage(UiState.PROCESSING, "Processing...", UiDirection.ENTER, new InputOption[0]));
                    Console.WriteLine("Received UiStateMessage: PROCESSING Processing... Enter");
                    Thread.Sleep(1000);
                    notifyObserversUiState(new UiStateMessage(UiState.PROCESSING, "Processing...", UiDirection.EXIT, new InputOption[0]));
                    Console.WriteLine("Received UiStateMessage: PROCESSING Processing... Exit");

                    if (payIntent.amount < 5000) // less than 50
                    {
                        Payment payment = new Payment();
                        payment.amount =  payIntent.amount;
                        payment.id = Guid.NewGuid().ToString();
                        payment.employee = new Reference();
                        payment.employee.id = Guid.NewGuid().ToString();
                        payment.order = new Reference();
                        if (order != null)
                        {
                            payment.order.id = order.id;
                        }
                        FinishOkMessage okMsg = new FinishOkMessage();
                        payment.cardTransaction = new CardTransaction();
                        payment.cardTransaction.last4 = "0123";
                        payment.cardTransaction.token = "1234567890123456";
                        TempObjectMap.Add(payment.id, payment);
                        okMsg.payment = payment;
                        notifyObserversFinishOk(okMsg);
                        Console.WriteLine("Received FINISH_OK: " + okMsg.payment);
                    }
                    else if(payIntent.amount > 10000 && payIntent.amount < 15000)
                    {
                        // if payment without tip is $100-$150 then simulate cancel transaction
                        notifyObserversFinishCancel();
                        Console.WriteLine("Received FinishCancel Message: ");
                    }
                    else
                    {

                        notifyObserversUiState(new UiStateMessage(UiState.ADD_SIGNATURE, "Waiting for signature...", UiDirection.ENTER, new InputOption[0]));
                        Console.WriteLine("Received UiStateMessage: ADD_SIGNATURE Waiting for Signature... Enter");
                        Thread.Sleep(3000);
                        notifyObserversUiState(new UiStateMessage(UiState.ADD_SIGNATURE, "Waiting for signature...", UiDirection.EXIT, new InputOption[0]));
                        Console.WriteLine("Received UiStateMessage: ADD_SIGNATURE Waiting for Signature... Exit");

                        Signature2 sig = JsonUtils.deserialize<Signature2>("{\"strokes\":[{\"points\":[{\"x\":174,\"y\":341},{\"x\":177,\"y\":338},{\"x\":183,\"y\":330},{\"x\":189,\"y\":323},{\"x\":200,\"y\":313},{\"x\":208,\"y\":305},{\"x\":217,\"y\":296},{\"x\":230,\"y\":284},{\"x\":241,\"y\":271},{\"x\":252,\"y\":257},{\"x\":261,\"y\":243},{\"x\":267,\"y\":233},{\"x\":273,\"y\":220},{\"x\":277,\"y\":206},{\"x\":279,\"y\":196},{\"x\":278,\"y\":182},{\"x\":275,\"y\":173},{\"x\":272,\"y\":164},{\"x\":267,\"y\":156},{\"x\":262,\"y\":151},{\"x\":252,\"y\":144},{\"x\":244,\"y\":141},{\"x\":235,\"y\":141},{\"x\":226,\"y\":144},{\"x\":217,\"y\":148},{\"x\":209,\"y\":154},{\"x\":202,\"y\":162},{\"x\":197,\"y\":171},{\"x\":195,\"y\":183},{\"x\":193,\"y\":195},{\"x\":195,\"y\":209},{\"x\":202,\"y\":222},{\"x\":208,\"y\":231},{\"x\":219,\"y\":241},{\"x\":231,\"y\":248},{\"x\":242,\"y\":253},{\"x\":253,\"y\":255},{\"x\":271,\"y\":256},{\"x\":285,\"y\":254},{\"x\":302,\"y\":252},{\"x\":311,\"y\":249},{\"x\":320,\"y\":245},{\"x\":329,\"y\":241},{\"x\":337,\"y\":236},{\"x\":344,\"y\":233},{\"x\":349,\"y\":231},{\"x\":352,\"y\":229},{\"x\":351,\"y\":234},{\"x\":350,\"y\":240},{\"x\":347,\"y\":247},{\"x\":343,\"y\":255},{\"x\":336,\"y\":267},{\"x\":327,\"y\":277},{\"x\":316,\"y\":287},{\"x\":302,\"y\":296},{\"x\":283,\"y\":301},{\"x\":264,\"y\":304},{\"x\":244,\"y\":305},{\"x\":227,\"y\":301},{\"x\":213,\"y\":298},{\"x\":204,\"y\":292},{\"x\":195,\"y\":287},{\"x\":190,\"y\":283},{\"x\":187,\"y\":280},{\"x\":193,\"y\":284},{\"x\":200,\"y\":288},{\"x\":214,\"y\":294},{\"x\":232,\"y\":303},{\"x\":251,\"y\":312},{\"x\":272,\"y\":320},{\"x\":292,\"y\":328},{\"x\":313,\"y\":332},{\"x\":333,\"y\":334},{\"x\":351,\"y\":332},{\"x\":368,\"y\":326},{\"x\":378,\"y\":317},{\"x\":390,\"y\":307},{\"x\":400,\"y\":300},{\"x\":407,\"y\":292},{\"x\":412,\"y\":284},{\"x\":415,\"y\":275},{\"x\":416,\"y\":268},{\"x\":415,\"y\":261},{\"x\":410,\"y\":259},{\"x\":401,\"y\":261},{\"x\":393,\"y\":266},{\"x\":381,\"y\":275},{\"x\":376,\"y\":282},{\"x\":372,\"y\":295},{\"x\":371,\"y\":304},{\"x\":375,\"y\":312},{\"x\":372,\"y\":318},{\"x\":387,\"y\":326},{\"x\":396,\"y\":329},{\"x\":404,\"y\":330},{\"x\":412,\"y\":328},{\"x\":419,\"y\":322},{\"x\":424,\"y\":314},{\"x\":426,\"y\":305},{\"x\":424,\"y\":294},{\"x\":419,\"y\":284},{\"x\":415,\"y\":278},{\"x\":405,\"y\":270},{\"x\":398,\"y\":267},{\"x\":393,\"y\":265},{\"x\":391,\"y\":266},{\"x\":394,\"y\":273},{\"x\":399,\"y\":278},{\"x\":410,\"y\":286},{\"x\":426,\"y\":292},{\"x\":444,\"y\":295},{\"x\":463,\"y\":294},{\"x\":477,\"y\":292},{\"x\":486,\"y\":287},{\"x\":495,\"y\":282},{\"x\":501,\"y\":276},{\"x\":507,\"y\":270},{\"x\":511,\"y\":265},{\"x\":513,\"y\":260},{\"x\":508,\"y\":260},{\"x\":502,\"y\":263},{\"x\":495,\"y\":268},{\"x\":489,\"y\":276},{\"x\":485,\"y\":285},{\"x\":482,\"y\":293},{\"x\":485,\"y\":300},{\"x\":489,\"y\":304},{\"x\":494,\"y\":308},{\"x\":501,\"y\":308},{\"x\":507,\"y\":304},{\"x\":512,\"y\":300},{\"x\":517,\"y\":294},{\"x\":518,\"y\":289},{\"x\":520,\"y\":283},{\"x\":523,\"y\":279},{\"x\":523,\"y\":276},{\"x\":525,\"y\":274},{\"x\":530,\"y\":272},{\"x\":538,\"y\":270},{\"x\":548,\"y\":268},{\"x\":563,\"y\":267},{\"x\":580,\"y\":267},{\"x\":590,\"y\":266},{\"x\":600,\"y\":265},{\"x\":609,\"y\":264},{\"x\":616,\"y\":263},{\"x\":622,\"y\":262},{\"x\":626,\"y\":261},{\"x\":622,\"y\":262},{\"x\":613,\"y\":264},{\"x\":597,\"y\":270},{\"x\":582,\"y\":279},{\"x\":568,\"y\":290},{\"x\":561,\"y\":299},{\"x\":557,\"y\":306},{\"x\":559,\"y\":315},{\"x\":565,\"y\":321},{\"x\":572,\"y\":326},{\"x\":583,\"y\":325},{\"x\":600,\"y\":316},{\"x\":614,\"y\":304},{\"x\":625,\"y\":289},{\"x\":631,\"y\":269},{\"x\":634,\"y\":247},{\"x\":633,\"y\":222},{\"x\":630,\"y\":196},{\"x\":627,\"y\":170},{\"x\":626,\"y\":150},{\"x\":624,\"y\":134},{\"x\":625,\"y\":125},{\"x\":626,\"y\":117},{\"x\":627,\"y\":111},{\"x\":628,\"y\":109},{\"x\":629,\"y\":115},{\"x\":632,\"y\":129},{\"x\":638,\"y\":152},{\"x\":645,\"y\":177},{\"x\":654,\"y\":205},{\"x\":663,\"y\":235},{\"x\":674,\"y\":264},{\"x\":684,\"y\":288},{\"x\":693,\"y\":304},{\"x\":697,\"y\":313},{\"x\":701,\"y\":314}]}],\"height\":666,\"width\":799}");

                        // send signature...
                        Payment payment = new Payment();
                        payment.amount = payIntent.amount;
                        payment.id = Guid.NewGuid().ToString();
                        payment.employee = new Reference();
                        payment.employee.id = Guid.NewGuid().ToString();
                        payment.order = new Reference();
                        if (order != null)
                        {
                            payment.order.id = order.id;
                        }

                        if(payIntent.tipAmount.HasValue)
                        {
                            payment.tipAmount = payIntent.tipAmount.Value;
                        }
                        TempObjectMap.Add(payment.id, payment);
                        VerifySignatureMessage msg = new VerifySignatureMessage();
                        msg.signature = sig;
                        msg.payment = payment;
                        notifyObserversVerifySignature(msg);
                        Console.WriteLine("Received VerifySignatureMessage: " + JsonUtils.serialize(msg));

                    }
                }
                else if (payIntent.amount < 0 && payIntent.transactionType == PayIntent.TransactionType.CREDIT)
                {
                    notifyObserversUiState(new UiStateMessage(UiState.START, "Customer is choosing payment.", UiDirection.ENTER, new InputOption[0]));
                    Console.WriteLine("Received UiStateMessage: START Customer is choosing payment... Enter");
                    Thread.Sleep(1000);
                    notifyObserversUiState(new UiStateMessage(UiState.START, "Customer is choosing payment.", UiDirection.EXIT, new InputOption[0]));
                    Console.WriteLine("Received UiStateMessage: START Customer is choosing payment... Exit");
                    notifyObserversUiState(new UiStateMessage(UiState.PROCESSING, "Processing...", UiDirection.ENTER, new InputOption[0]));
                    Console.WriteLine("Received UiStateMessage: PROCESSING Processing... Enter");
                    Thread.Sleep(3000);
                    notifyObserversUiState(new UiStateMessage(UiState.PROCESSING, "Processing...", UiDirection.EXIT, new InputOption[0]));
                    Console.WriteLine("Received UiStateMessage: PROCESSING Processing... Exit");
                    notifyObserversUiState(new UiStateMessage(UiState.RECEIPT_OPTIONS, "Customer is selecting receipt type.", UiDirection.ENTER, new InputOption[0]));
                    Console.WriteLine("Received UiStateMessage: RECEIPT_OPTIONS Customer is selecting receipt type. Enter");
                    Thread.Sleep(3000);
                    notifyObserversUiState(new UiStateMessage(UiState.RECEIPT_OPTIONS, "Customer is selecting receipt type.", UiDirection.EXIT, new InputOption[0]));
                    Console.WriteLine("Received UiStateMessage: RECEIPT_OPTIONS Customer is selecting receipt type. Exit");

                    Credit credit = new Credit();
                    credit.amount = payIntent.amount;
                    credit.id = Guid.NewGuid().ToString();
                    FinishOkMessage okMsg = new FinishOkMessage();
                    credit.cardTransaction = new CardTransaction();
                    credit.cardTransaction.last4 = "0123";
                    okMsg.credit = credit;
                    TempObjectMap.Add(credit.id, credit);
                    notifyObserversFinishOk(okMsg);
                    Console.WriteLine("Received FinishOkMessage: FINISH_OK");
                }
            });
            bw.RunWorkerAsync();
        }

        public override void doVoidPayment(Payment payment, VoidReason reason)
        {
            BackgroundWorker bw = new BackgroundWorker();
            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                notifyObserversUiState(new UiStateMessage(UiState.PROCESSING, "Processing...", UiDirection.ENTER, new InputOption[0]));
                Console.WriteLine("Received UiStateMessage: PROCESSING Processing... Enter");
                Thread.Sleep(3000);
                notifyObserversUiState(new UiStateMessage(UiState.PROCESSING, "Processing...", UiDirection.EXIT, new InputOption[0]));
                Console.WriteLine("Received UiStateMessage: PROCESSING Processing... Exit");

                notifyObserversPaymentVoided(payment, reason);
                Console.WriteLine("Received PaymentVoided Message: " + JsonUtils.serialize(payment));
            });
            bw.RunWorkerAsync();
        }

        public override void doCloseout(bool allowOpenTabs, string batchId)
        {
            notifyObserversCloseout();
        }

        public override void doDiscoveryRequest()
        {
            
        }

        public override void doRetrievePendingPayments()
        {

        }

        public void onMessage(string message)
        {
            throw new NotImplementedException();
        }

        public override void doOrderUpdate(DisplayOrder order, DisplayOperation operation)
        {
            // this is blind to the client
        }

        public override void doVaultCard(int? CardEntryMethods)
        {
            BackgroundWorker bw = new BackgroundWorker();
            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                notifyObserversUiState(new UiStateMessage(UiState.PROCESSING, "Processing...", UiDirection.ENTER, new InputOption[0]));
                Console.WriteLine("Received UiStateMessage: PROCESSING Processing... Enter");
                Thread.Sleep(3000);
                notifyObserversUiState(new UiStateMessage(UiState.PROCESSING, "Processing...", UiDirection.EXIT, new InputOption[0]));
                Console.WriteLine("Received UiStateMessage: PROCESSING Processing... Exit");
                notifyObserversUiState(new UiStateMessage(UiState.START, "Customer is choosing payment.", UiDirection.ENTER, new InputOption[0]));
                Console.WriteLine("Received UiStateMessage: START Customer is choosing payment... Enter");
                Thread.Sleep(1000);
                notifyObserversUiState(new UiStateMessage(UiState.START, "Customer is choosing payment.", UiDirection.EXIT, new InputOption[0]));
                Console.WriteLine("Received UiStateMessage: START Customer is choosing payment... Exit");
                notifyObserversUiState(new UiStateMessage(UiState.PROCESSING, "Processing...", UiDirection.ENTER, new InputOption[0]));
                Console.WriteLine("Received UiStateMessage: PROCESSING Processing... Enter");
                Thread.Sleep(3000);
                notifyObserversUiState(new UiStateMessage(UiState.PROCESSING, "Processing...", UiDirection.EXIT, new InputOption[0]));
                Console.WriteLine("Received UiStateMessage: PROCESSING Processing... Exit");
                VaultCardResponseMessage vcrm = new VaultCardResponseMessage();
                VaultedCard card = new VaultedCard();
                card.cardholderName = "EMULATOR DISCOVER CARD";
                card.first6 = "123456";
                card.expirationDate = "1218";
                card.last4 = "4321";
                card.token = "1234567890123456";
                vcrm.card = card;
                notifyObserversCardVaulted(vcrm);
                Console.WriteLine("Received VaultCardResponse Message: " + JsonUtils.serialize(vcrm));
            });
            bw.RunWorkerAsync();
        }

        public override void doReadCardData(PayIntent payIntent)
        {
            throw new NotImplementedException();
        }

        public override void doResetDevice()
        {
            // Blind to the client
        }

        public void onDeviceError(int code, string message)
        {
            foreach (ICloverDeviceObserver devOvs in deviceObservers)
            {
                devOvs.onDeviceError(code, message);
            }

        }

        public override void doAcceptPayment(Payment payment)
        {
            //throw new NotImplementedException();
        }

        public override void doRejectPayment(Payment payment, Challenge challenge)
        {
            //throw new NotImplementedException();
        }
    }
}
