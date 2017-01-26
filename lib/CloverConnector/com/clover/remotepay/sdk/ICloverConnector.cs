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
using com.clover.remotepay.transport;
using com.clover.sdk.v3.payments;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace com.clover.remotepay.sdk
{
    /// <summary>
    /// Interface to define the available methods to send requests to 
    /// a connected Clover device.
    /// </summary>
    public interface ICloverConnector
    {
        /// <summary>
        /// Property for the current connection status of the device
        /// </summary>
        bool IsReady
        {
            get;
        }
        /// <summary>
        /// Starts communication with the device.  This needs to be called after the connector is created and listeners are added to the connector.
        /// </summary>
        void InitializeConnection();

        /// <summary>
        /// Adds a clover connector listener.
        /// </summary>
        /// <param name="connectorListener">The connector listener.</param>
        void AddCloverConnectorListener(ICloverConnectorListener connectorListener);

        /// <summary>
        /// Removes a clover connector listener.
        /// </summary>
        /// <param name="connectorListener">The connector listener.</param>
        void RemoveCloverConnectorListener(ICloverConnectorListener connectorListener);

        /// <summary>
        /// Sale method, aka "purchase"
        /// </summary>
        /// <param name="request">A SaleRequest object containing basic information needed for the transaction</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
        void Sale(SaleRequest request);

        /// <summary>
        /// If signature is captured during a Sale, this method accepts the signature as entered
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void AcceptSignature(VerifySignatureRequest request);

        /// <summary>
        /// If signature is captured during a Sale, this method rejects the signature as entered
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void RejectSignature(VerifySignatureRequest request);

        /// <summary>
        /// If signature is captured during a Sale, this method accepts the signature as entered
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void AcceptPayment(Payment payment);

        /// <summary>
        /// If signature is captured during a Sale, this method rejects the signature as entered
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void RejectPayment(Payment payment, Challenge challenge);

        /// <summary>
        /// Auth method to obtain an Auth or Pre-Auth, based on the AuthRequest IsPreAuth flag
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void Auth(AuthRequest request);

        /// <summary>
        /// Auth method to obtain an Auth or Pre-Auth, based on the AuthRequest IsPreAuth flag
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void PreAuth(PreAuthRequest request);

        /// <summary>
        /// Capture a previous Auth. Note: Should only be called if request's PaymentID is from an AuthResponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void CapturePreAuth(CapturePreAuthRequest request);

        /// <summary>
        /// Adjust the tip for a previous Auth. Note: Should only be called if request's PaymentID is from an AuthResponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void TipAdjustAuth(TipAdjustAuthRequest request);

        /// <summary>
        /// Void a transaction, given a previously used order ID and/or payment ID
        /// TBD - defining a payment or order ID to be used with a void without requiring a response from Sale()
        /// </summary>
        /// <param name="request">A VoidRequest object containing basic information needed to void the transaction</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
        void VoidPayment(VoidPaymentRequest request);


        /// <summary>
        /// Refund a specific payment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void RefundPayment(RefundPaymentRequest request);


        /// <summary>
        /// Manual refund method, aka "naked credit"
        /// </summary>
        /// <param name="request">A ManualRefundRequest object</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
        void ManualRefund(ManualRefundRequest request); // NakedRefund is a Transaction, with just negative amount

        /// <summary>
        /// Vault card, used to get payment token
        /// </summary>
        /// 
        void VaultCard(int? CardEntryMethods);

        /// <summary>
        /// Retrieve Card Data
        /// </summary>
        /// 
        void ReadCardData(ReadCardDataRequest CardDataRequest);

        /// <summary>
        /// Cancels the device from waiting for a payment card.
        /// </summary>
        /// <returns></returns>
        void Cancel();

        /// <summary>
        /// Send a request to the server to closeout all orders.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void Closeout(CloseoutRequest request);

        /// <summary>
        /// Send a request to the server to reset the Clover device.
        /// </summary>
        /// <returns></returns>
        void ResetDevice();

        /// <summary>
        /// Print simple lines of text to the Clover Mini printer
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        void PrintText(List<string> messages);

        /// <summary>
        /// Print an image on the Clover Mini printer
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        void PrintImage(Bitmap bitmap); //Bitmap img

        /// <summary>
        /// Show a message on the Clover Mini screen
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        void ShowMessage(string message);

        /// <summary>
        /// Return the device to the Welcome Screen
        /// </summary>
        void ShowWelcomeScreen();

        /// <summary>
        /// Show the thank you screen on the device
        /// </summary>
        void ShowThankYouScreen();

        /// <summary>
        /// Show the customer facing receipt option screen for the specified payment.
        /// </summary>
        void DisplayPaymentReceiptOptions(String orderId, String paymentId);

        /// <summary>
        /// Will trigger cash drawer to open that is connected to Clover Mini
        /// </summary>
        void OpenCashDrawer(String reason);

        /// <summary>
        /// Show the DisplayOrder on the device. Replaces the existing DisplayOrder on the device.
        /// </summary>
        /// <param name="order"></param>
        void ShowDisplayOrder(DisplayOrder order);


        /// <summary>
        /// Remove the DisplayOrder from the device.
        /// </summary>
        /// <param name="order"></param>
        void RemoveDisplayOrder(DisplayOrder order);


        // 
        void Dispose();

        /// <summary>
        /// Invoke the InputOption as sent in the DeviceActivityStart callback
        /// </summary>
        /// <param name="io"></param>
        void InvokeInputOption(transport.InputOption io);

        /// <summary>
        /// Print an image from a url
        /// </summary>
        /// <param name="ImgURL"></param>
        void PrintImageFromURL(String ImgURL);

        /// <summary>
        /// Request a list of payments taken offline,
        /// but that haven't been processed yet
        /// </summary>
        void RetrievePendingPayments();
    }
}
