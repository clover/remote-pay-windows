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
using com.clover.remotepay.sdk;

namespace com.clover.remotepay.transport
{
    public abstract class CloverDevice
    {
        protected List<ICloverDeviceObserver> deviceObservers = new List<ICloverDeviceObserver>();
        protected CloverTransport transport;
        protected string packageName { get; set; }
        protected readonly string remoteSourceSDK;
        protected readonly string remoteApplicationID;
        protected DeviceInfo deviceInfo;
        
        public CloverDevice(string packageName, CloverTransport transport, string remoteApplicationID)
        {
            this.transport = transport;
            this.packageName = packageName;
            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("CloverConnector");
            this.remoteSourceSDK = AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyDescriptionAttribute>(assembly).Description + ":"
                + (AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyFileVersionAttribute>(assembly)).Version
                + (AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyInformationalVersionAttribute>(assembly)).InformationalVersion;
            this.deviceInfo = new DeviceInfo();
            this.remoteApplicationID = remoteApplicationID;
        }

        public string getSDKInfo()
        {
            return remoteSourceSDK;
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

        public void Subscribe(ICloverDeviceObserver observer)
        {
            deviceObservers.Add(observer);
        }

        public void Unsubscribe(ICloverDeviceObserver observer)
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
        public abstract void doVerifySignature(Payment payment, bool verified);
        public abstract void doTerminalMessage(String text);
        public abstract void doRefundPayment(string orderId, string paymentId, long? amount, bool? fullRefund); // manual refunds are handled via doTxStart
        public abstract void doTipAdjustAuth(string orderId, string paymentId, long amount);
        public abstract void doPrintText(List<String> textLines);
        public abstract void doShowWelcomeScreen();
        public abstract void doShowPaymentReceiptScreen(string orderId, string paymentId);
        public abstract void doShowThankYouScreen();
        public abstract void doOpenCashDrawer(string reason);
        public abstract void doPrintImage(string base64String);
        public abstract void doPrintImageURL(string urlString);
        public abstract void doCloseout(bool allowOpenTabs, string batchId);
        public abstract void doResetDevice();
        public abstract void doVaultCard(int? CardEntryMethods);
        public abstract void doCapturePreAuth(string paymentID, long amount, long tipAmount);
        public abstract void doLogMessages(LogLevelEnum logLevel, Dictionary<string, string> messages);
    }

    public interface ICloverDeviceObserver
    {
        /// <summary>
        /// called when the state of the tx changes.
        /// </summary>
        /// <param name="txState">State of the tx.</param>
        void onTxState(TxState txState);
        /// <summary>
        /// Called when the state of the UI changes.
        /// </summary>
        /// <param name="uiState">State of the UI.</param>
        /// <param name="uiText">The UI text.</param>
        /// <param name="uiDirection">The UI direction.</param>
        /// <param name="inputOptions">The input options.</param>
        void onUiState(UiState uiState, String uiText, UiDirection uiDirection, params InputOption[] inputOptions);
        /// <summary>
        /// called when a tip is added to a transaction by the customer
        /// </summary>
        /// <param name="tipAmount">The tip amount.</param>
        void onTipAdded(long tipAmount);
        /// <summary>
        /// called after a TipAdjust call has been made
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        void onAuthTipAdjusted(string paymentId, long amount, bool success);
        /// <summary>
        /// Called when cashback is selected.
        /// </summary>
        /// <param name="cashbackAmount">The cashback amount.</param>
        void onCashbackSelected(long cashbackAmount);
        /// <summary>
        /// Called on a partial authorization.
        /// </summary>
        /// <param name="partialAuthAmount">The partial authorization amount.</param>
        void onPartialAuth(long partialAuthAmount);
        /// <summary>
        /// Called when a finish ok for a payment is received.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="signature2">The signature2.</param>
        void onFinishOk(Payment payment, Signature2 signature2);
        /// <summary>
        /// Called when a finish ok for a manual refund is received.
        /// </summary>
        /// <param name="credit">The credit.</param>
        void onFinishOk(Credit credit);
        /// <summary>
        /// Called when a finish ok for a payment refund is received.
        /// </summary>
        /// <param name="refund">The refund.</param>
        void onFinishOk(Refund refund);
        /// <summary>
        /// Called when a finish cancel is received.
        /// </summary>
        void onFinishCancel();
        /// <summary>
        /// Called when a verify signature is requested.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="signature">The signature.</param>
        void onVerifySignature(Payment payment, Signature2 signature);
        /// <summary>
        /// Called when a payment is successfully voided.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="voidReason">The void reason.</param>
        void onPaymentVoided(Payment payment, VoidReason voidReason);
        /// <summary>
        /// Called when a key is pressed.
        /// </summary>
        /// <param name="keyPress">The key press.</param>
        void onKeyPressed(KeyPress keyPress);
        /// <summary>
        /// Called when a refund payment response is received.
        /// </summary>
        /// <param name="refund">The refund.</param>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="code">The code.</param>
        /// <param name="msg">The MSG.</param>
        /// TODO Edit XML Comment Template for onRefundPaymentResponse
        void onRefundPaymentResponse(Refund refund, String orderId, String paymentId, TxState code, string msg);
        /// <summary>
        /// Called when a closeout response is received.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="batch">The batch.</param>
        void onCloseoutResponse(ResultStatus status, string reason, Batch batch);
        /// <summary>
        /// Called when a tx start response is received.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="externalId">The external identifier.</param>
        void onTxStartResponse(TxStartResponseResult result, string externalId);
        /// <summary>
        /// Called when a vault card response is received.
        /// </summary>
        /// <param name="vcrm">The VCRM.</param>
        void onVaultCardResponse(VaultCardResponseMessage vcrm);
        /// <summary>
        /// Called when a capture pre authorization response is received.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="tipAmount">The tip amount.</param>
        /// <param name="status">The status.</param>
        /// <param name="reason">The reason.</param>
        void onCapturePreAuthResponse(String paymentId, long amount, long tipAmount, ResultStatus status, string reason);
        /// <summary>
        /// Called when a device is ready to receive messages.
        /// </summary>
        /// <param name="drMessage">The dr message.</param>
        void onDeviceReady(DiscoveryResponseMessage drMessage);
        /// <summary>
        /// Called when a device is connected to the SDK.
        /// </summary>
        void onDeviceConnected();
        /// <summary>
        /// Called when a device is disconnected from the SDK.
        /// </summary>
        void onDeviceDisconnected();
        /// <summary>
        /// Called when an error condition is detected while communicating with the device.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        void onDeviceError(int code, string message);
    }

    public class DeviceInfo
    {
        public String name { get; set; }
        public String serial { get; set; }
        public String model { get; set; }
    }

}
