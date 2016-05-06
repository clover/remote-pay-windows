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
using com.clover.remote.order.operation;
using com.clover.remotepay.transport;
using com.clover.remotepay.data;
using com.clover.sdk.v3.order;
using com.clover.sdk.v3.payments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace com.clover.remotepay.transport
{
    public abstract class CloverDevice
    {
        protected List<CloverDeviceObserver> deviceObservers = new List<CloverDeviceObserver>();
        protected CloverTransport transport;
        protected string packageName { get; set; }
        protected readonly string remoteSourceSDK;
        protected readonly string remoteApplicationID;
        protected DeviceInfo deviceInfo;
        
        public CloverDevice(string packageName, CloverTransport transport, string remoteApplicationID)
        {
            this.transport = transport;
            this.packageName = packageName;
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            this.remoteSourceSDK = AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyDescriptionAttribute>(assembly).Description + ":" + (AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyFileVersionAttribute>(assembly)).Version;
            this.deviceInfo = new DeviceInfo();
            this.remoteApplicationID = remoteApplicationID;
        }

        /// <summary>
        /// Adds a observer for transport events to the member transport object to notify
        /// </summary>
        /// <param name="observer"></param>
        public void Subscribe(CloverTransportObserver observer)
        {
            this.transport.Subscribe(observer);
        }

        public void Dispose()
        {
            if(this.transport != null)
            {
                transport.Dispose();
            }
        }

        public void Subscribe(CloverDeviceObserver observer)
        {
            deviceObservers.Add(observer);
        }

        public void Unsubscribe(CloverDeviceObserver observer)
        {
            deviceObservers.Remove(observer);
        }

        public abstract void doDiscoveryRequest();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="payIntent"></param>
        /// <param name="order">can be null.  If it is, an order will implicitly be created on the other end</param>
        public abstract void doTxStart(PayIntent payIntent, Order order, bool suppressOnScreenTips);
        public abstract void doKeyPress(KeyPress keyPress);
        public abstract void doVoidPayment(Payment payment, VoidReason reason);
        public abstract void doOrderUpdate(DisplayOrder order, DisplayOperation operation);
        public abstract void doSignatureVerified(Payment payment, bool verified);
        public abstract void doTerminalMessage(String text);
        public abstract void doPaymentRefund(string orderId, string paymentId, long amount); // manual refunds are handled via doTxStart
        public abstract void doTipAdjustAuth(string orderId, string paymentId, long amount);
        //void doBreak();
        public abstract void doPrintText(List<String> textLines);
        public abstract void doShowWelcomeScreen();
        public abstract void doShowPaymentReceiptScreen(string orderId, string paymentId);
        public abstract void doShowRefundReceiptScreen(string orderId, string refundId);
        public abstract void doShowCreditReceiptScreen(string orderId, string creditId);
        public abstract void doShowThankYouScreen();
        public abstract void doOpenCashDrawer(string reason);
        public abstract void doPrintImage(string base64String);

        public abstract void doCloseout(bool allowOpenTabs, string batchId);
        public abstract void doResetDevice();
        public abstract void doVaultCard(int? CardEntryMethods);
        public abstract void doCaptureAuth(string paymentID, long amount, long tipAmount);
    }

    public interface CloverDeviceObserver
    { 
        void onTxState(TxState txState);
        void onUiState(UiState uiState, String uiText, UiDirection uiDirection, params InputOption[] inputOptions);
        /// <summary>
        /// called when a tip is added to a transaction by the customer
        /// </summary>
        /// <param name="tipAmount"></param>
        void onTipAdded(long tipAmount); 
        /// <summary>
        /// called after a TipAdjust call has been made
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="amount"></param>
        /// <param name="success"></param>
        void onAuthTipAdjusted(string paymentId, long amount, bool success);
        void onCashbackSelected(long cashbackAmount);
        void onPartialAuth(long partialAuthAmount);
        void onFinishOk(Payment payment, Signature2 signature2);
        void onFinishOk(Credit credit);
        void onFinishOk(Refund refund);
        void onFinishCancel();
        void onVerifySignature(Payment payment, Signature2 signature);
        void onPaymentVoided(Payment payment, VoidReason voidReason);
        void onKeyPressed(KeyPress keyPress);
        void onPaymentRefundResponse(string orderId, string paymentId, Refund refund, TxState code, string msg);
        void onCloseoutResponse(ResultStatus status, string reason, Batch batch);
        void onTxStartResponse(bool success);
        void onVaultCardResponse(VaultCardResponseMessage vcrm);
        void onCaptureAuthResponse(CaptureAuthResponseMessage carm);

        void onDeviceReady(DiscoveryResponseMessage drMessage);
        void onDeviceConnected();
        void onDeviceDisconnected();
        void onDeviceError(int code, string message);
    }

    public class DeviceInfo
    {
        public String name { get; set; }
        public String serial { get; set; }
        public String model { get; set; }
    }

}
