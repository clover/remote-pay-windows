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
using com.clover.remotepay.data;
using com.clover.sdk.v3.payments;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace com.clover.remotepay.sdk
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICloverConnector
    {
        /// <summary>
        /// define the card entry methods supported by the CloverMini
        /// </summary>
        int CardEntryMethod { get; set; }

        /// <summary>
        /// set to true to disable printing on the Clover Mini
        /// </summary>
         bool DisablePrinting { get; set; }

        /// <summary>
        /// set to true to disable cashback on the Clover Mini
        /// </summary>
         bool DisableCashBack { get; set; }

        /// <summary>
        /// set to true to disable tip on the Clover Mini
        /// </summary>
         bool DisableTip { get; set; }

        /// <summary>
        /// set to true, so when a transaction fails the Clover Mini returns to the welcome screen,
        /// otherwise it restarts the payment transaction
        /// </summary>
         bool DisableRestartTransactionOnFail { get; set; }

         void AddCloverConnectorListener(CloverConnectorListener connectorListener);
         void RemoveCloverConnectorListener(CloverConnectorListener connectorListener);

        /// <summary>
        /// Sale method, aka "purchase"
        /// </summary>
        /// <param name="request">A SaleRequest object containing basic information needed for the transaction</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
        int Sale(SaleRequest request);

        /// <summary>
        /// If signature is captured during a Sale, this method accepts the signature as entered
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        int AcceptSignature(SignatureVerifyRequest request);

        /// <summary>
        /// If signature is captured during a Sale, this method rejects the signature as entered
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        int RejectSignature(SignatureVerifyRequest request);


        /// <summary>
        /// Auth method to obtain an Auth or Pre-Auth, based on the AuthRequest IsPreAuth flag
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
         int Auth(AuthRequest request);

        /// <summary>
        /// Auth method to obtain an Auth or Pre-Auth, based on the AuthRequest IsPreAuth flag
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        int PreAuth(PreAuthRequest request);

        /// <summary>
        /// Capture a previous Auth. Note: Should only be called if request's PaymentID is from an AuthResponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        int CaptureAuth(CaptureAuthRequest request);

        /// <summary>
        /// Adjust the tip for a previous Auth. Note: Should only be called if request's PaymentID is from an AuthResponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
         int TipAdjustAuth(TipAdjustAuthRequest request);

        /// <summary>
        /// Void a transaction, given a previously used order ID and/or payment ID
        /// TBD - defining a payment or order ID to be used with a void without requiring a response from Sale()
        /// </summary>
        /// <param name="request">A VoidRequest object containing basic information needed to void the transaction</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
         int VoidPayment(VoidPaymentRequest request);


        /// <summary>
        /// called when requesting a payment be voided when only the request UUID is available
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
         int VoidTransaction(VoidTransactionRequest request);


        /// <summary>
        /// Refund a specific payment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
         int RefundPayment(RefundPaymentRequest request);


        /// <summary>
        /// Manual refund method, aka "naked credit"
        /// </summary>
        /// <param name="request">A ManualRefundRequest object</param>
        /// <returns>Status code, 0 for success, -1 for failure (need to use pre-defined constants)</returns>
         int ManualRefund(ManualRefundRequest request); // NakedRefund is a Transaction, with just negative amount

        /// <summary>
        /// Vault card, used to get payment token
        /// </summary>
        /// 
        int VaultCard(int? CardEntryMethods);

        /// <summary>
        /// Cancels the device from waiting for a payment card.
        /// </summary>
        /// <returns></returns>
        int Cancel();

        /// <summary>
        /// Send a request to the server to closeout all orders.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        int Closeout(CloseoutRequest request);

        /// <summary>
        /// Send a request to the server to reset the Clover device.
        /// </summary>
        /// <returns></returns>
        int ResetDevice();

        /// <summary>
        /// Print simple lines of text to the Clover Mini printer
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        int PrintText(List<string> messages);

        /// <summary>
        /// Print an image on the Clover Mini printer
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        int PrintImage(Bitmap bitmap); //Bitmap img

        /// <summary>
        /// Show a message on the Clover Mini screen
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        int ShowMessage(string message);

        /// <summary>
        /// Return the device to the Welcome Screen
        /// </summary>
        int ShowWelcomeScreen();

        /// <summary>
        /// Show the thank you screen on the device
        /// </summary>
        void ShowThankYouScreen();

        /// <summary>
        /// Show the customer facing receipt option screen for the specified payment.
        /// </summary>
        void DisplayPaymentReceiptOptions(String orderId, String paymentId);

        /// <summary>
        /// Show the customer facing receipt option screen for the specified refund.
        /// </summary>
        void DisplayRefundReceiptOptions(String orderId, String refundId);

        /// <summary>
        /// Show the customer facing receipt option screen for the specified credit.
        /// </summary>
        void DisplayCreditReceiptOptions(String orderId, String creditId);

        /// <summary>
        /// Will trigger cash drawer to open that is connected to Clover Mini
        /// </summary>
        void OpenCashDrawer(String reason);

        /// <summary>
        /// Show the DisplayOrder on the device. Replaces the existing DisplayOrder on the device.
        /// </summary>
        /// <param name="order"></param>
        void DisplayOrder(DisplayOrder order);

        /// <summary>
        /// Notify the device of a DisplayLineItem being added to a DisplayOrder
        /// </summary>
        /// <param name="order"></param>
        /// <param name="lineItem"></param>
        void DisplayOrderLineItemAdded(DisplayOrder order, DisplayLineItem lineItem);

        /// <summary>
        /// Notify the device of a DisplayLineItem being removed from a DisplayOrder
        /// </summary>
        /// <param name="order"></param>
        /// <param name="lineItem"></param>
        void DisplayOrderLineItemRemoved(DisplayOrder order, DisplayLineItem lineItem);

        /// <summary>
        /// Notify device of a discount being added to the order. 
        /// Note: This is independent of a discount being added to a display line item.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="discount"></param>
        void DisplayOrderDiscountAdded(DisplayOrder order, DisplayDiscount discount);

        /// <summary>
        /// Notify the device that a discount was removed from the order.
        /// Note: This is independent of a discount being removed from a display line item.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="discount"></param>
        void DisplayOrderDiscountRemoved(DisplayOrder order, DisplayDiscount discount);

        /// <summary>
        /// Remove the DisplayOrder from the device.
        /// </summary>
        /// <param name="order"></param>
        void DisplayOrderDelete(DisplayOrder order);


        /// <summary>
        /// return the Merchant object for the Merchant configured for the Clover Mini
        /// </summary>
        /// <returns></returns>
        //void GetMerchantInfo();

        // TODO: should we call through, repurpose or remove?
         void Dispose();


         void InvokeInputOption(transport.InputOption io);
    }
}
