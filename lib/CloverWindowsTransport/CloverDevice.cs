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
using Microsoft.Win32;
using System.Diagnostics;

namespace com.clover.remotepay.transport
{
    public abstract class CloverDevice
    {
        protected List<ICloverDeviceObserver> deviceObservers = new List<ICloverDeviceObserver>();
        protected CloverTransport transport;
        protected string shortTransportType = "UNKNOWN";
        protected string packageName { get; set; }
        protected readonly string remoteSourceSDK;
        protected readonly string remoteApplicationID;
        protected DeviceInfo deviceInfo;
        public bool SupportsAcks { get; set; }
        
        public CloverDevice(string packageName, CloverTransport transport, string remoteApplicationID)
        {
            string logSource = "_TransportEventLog";
            if (!EventLog.SourceExists(logSource))
                EventLog.CreateEventSource(logSource, logSource);

            EventLogTraceListener myTraceListener = new EventLogTraceListener(logSource);

            // Add the event log trace listener to the collection.
            Trace.Listeners.Add(myTraceListener);
            this.transport = transport;
            if (transport.GetType() == typeof(USBCloverTransport))
            {
                shortTransportType = "USB";
            } else if (transport.GetType() == typeof(WebSocketCloverTransport))
            {
                shortTransportType = "WS";
            } 
            this.packageName = packageName;
            this.remoteSourceSDK = getSDKInfoString();
            this.deviceInfo = new DeviceInfo();
            this.remoteApplicationID = remoteApplicationID;
        }


        private String getSDKInfoString()
        {
            String REG_KEY = "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\CloverSDK";
            String receiver = "";
            try
            {
                Object rReceiver = Registry.GetValue(REG_KEY, "DisplayName", "unset");
                if (rReceiver != null && !rReceiver.ToString().Equals("unset")) { 
                    receiver = rReceiver.ToString();
                } else
                {
                    receiver = "DLL";
                }
            }
            catch (Exception e)
            {
                receiver = "DLL";
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(this.packageName.GetType().ToString() + "->" + e.Message, EventLogEntryType.Information);
                }
//                EventLog.WriteEntry(this.packageName.GetType().ToString(), e.Message);
            }
            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("CloverConnector");
            String sdkInfoString = AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyDescriptionAttribute>(assembly).Description
            + "_" + receiver
            + "|" + shortTransportType
            + ":"
            + (AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyFileVersionAttribute>(assembly)).Version
            + (AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyInformationalVersionAttribute>(assembly)).InformationalVersion;
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry(this.packageName.GetType().ToString() + "->" + "SDKInfo from assembly and registry = " + sdkInfoString, EventLogEntryType.Information);
            }
            
            return sdkInfoString;
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
        public abstract void doTxStart(PayIntent payIntent, Order order);
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
        public abstract void doReadCardData(PayIntent payIntent);
        public abstract void doCapturePreAuth(string paymentID, long amount, long tipAmount);
        public abstract void doLogMessages(LogLevelEnum logLevel, Dictionary<string, string> messages);
        public abstract void doAcceptPayment(Payment payment);
        public abstract void doRejectPayment(Payment payment, Challenge challenge);
        public abstract void doRetrievePendingPayments();
        public abstract void doStartCustomActivity(string action, string payload, bool nonBlocking);
        public abstract void doSendMessageToActivity(string action, string payload);
        public abstract void doRetrieveDeviceStatus(bool sendLastMessage);
        public abstract void doRetrievePayment(string externalPaymentId);

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
        /// Called when a payment confirmation is requested.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="challenges">The confirmation challenges.</param>
        void onConfirmPayment(Payment payment, List<Challenge> challenges);
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
        /// Called when a retrieve card data response is received.
        /// </summary>
        /// <param name="cdrm">The response message for card data.</param>
        void onReadCardDataResponse(ReadCardDataResponseMessage cdrm);
        /// <summary>
        /// Called when a capture preauth response is received.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="amount"></param>
        /// <param name="tipAmount"></param>
        /// <param name="status"></param>
        /// <param name="reason"></param>
        void onCapturePreAuthResponse(String paymentId, long amount, long tipAmount, ResultStatus status, string reason);
        /// <summary>
        /// Called when a device is ready to receive messages.
        /// </summary>
        /// <param name="drMessage">The dr message.</param>
        void onDeviceReady(CloverDevice device, DiscoveryResponseMessage drMessage);
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
        /// <summary>
        /// Called when a RetrievePendingPaymentsResponse is received
        /// </summary>
        /// <param name="v"></param>
        /// <param name="pendingPaymentEntries"></param>
        void onRetrievePendingPaymentsResponse(bool success, List<PendingPaymentEntry> pendingPaymentEntries);
        /// <summary>
        /// Called when a custom activity is completed as part of a normal flow
        /// </summary>
        /// <param name="status"></param>
        /// <param name="action"></param>
        /// <param name="payload"></param>
        /// <param name="failReason"></param>
        void onActivityResponse(ResultStatus status, String action, String payload, String failReason);
        /// <summary>
        /// gets called with the calling requests id to confirm device got the message
        /// </summary>
        /// <param name="sourceMessageId"></param>
        void onMessageAck(string sourceMessageId);

        void onPrintCredit(Credit credit);
        void onPrintPayment(Payment payment, Order order);
        void onPrintCreditDecline(Credit credit, String reason);
        void onPrintRefundPayment(Payment payment, Order order, Refund refund);
        void onPrintPaymentDecline(Payment payment, String reason);
        void onPrintMerchantReceipt(Payment payment);
        void onMessageFromActivity(string action, string payload);
        void onResetDeviceResponse(ResultStatus status, string reason, ExternalDeviceState state);
        void onDeviceStatusResponse(ResultStatus status, string reason, ExternalDeviceState state, ExternalDeviceStateData data);
        void onRetrievePaymentResponse(ResultStatus status, string reason, String externalPaymentId, QueryStatus queryStatus, Payment payment);
    }

    public class DeviceInfo
    {
        public String name { get; set; }
        public String serial { get; set; }
        public String model { get; set; }
    }

}
