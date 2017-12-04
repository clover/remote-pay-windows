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
    /// The CloverConnector API serves as the interface for connecting to a Clover device. 
    /// This document defines the available methods.
    /// </summary>
    public interface ICloverConnector
    {
        /// <summary>
        /// Initializes the connection and starts communication with the Clover device. 
        /// This method is called after the connector has been created and listeners have 
        /// been added to it. 
        /// It must be called before any other method (other than those that add or remove 
        /// listeners).
        /// </summary>
        void InitializeConnection();

        /// <summary>
        /// Adds a Clover Connector listener.
        /// </summary>
        /// <param name="connectorListener">The connection listener.</param>
        void AddCloverConnectorListener(ICloverConnectorListener connectorListener);

        /// <summary>
        /// Removes a Clover Connector listener.
        /// </summary>
        /// <param name="connectorListener">The connection listener.</param>
        void RemoveCloverConnectorListener(ICloverConnectorListener connectorListener);

        /// <summary>
        /// Requests a Sale transaction (purchase).
        /// </summary>
        /// <param name="request">A SaleRequest object containing basic information needed 
        /// for the transaction.</param>
        void Sale(SaleRequest request);

        /// <summary>
        /// If a signature is captured during a transaction, this method accepts the 
        /// signature as entered.
        /// </summary>
        /// <param name="request">The accepted VerifySignatureRequest the device passed to 
        /// OnVerifySignatureRequest().</param>
        void AcceptSignature(VerifySignatureRequest request);

        /// <summary>
        /// If a signature is captured during a transaction, this method rejects the 
        /// signature as entered.
        /// </summary>
        /// <param name="request">The rejected VerifySignatureRequest the device passed to 
        /// OnVerifySignatureRequest().</param>
        void RejectSignature(VerifySignatureRequest request);

        /// <summary>
        /// If Payment confirmation is required during a transaction due to a Challenge, 
        /// this method accepts the Payment. A Challenge may be triggered by a potential 
        /// duplicate Payment or an offline Payment.
        /// </summary>
        /// <param name="payment">The Payment to accept.</param>
        void AcceptPayment(Payment payment);

        /// <summary>
        /// If Payment confirmation is required during a transaction due to a Challenge, 
        /// this method rejects the Payment. A Challenge may be triggered by a potential 
        /// duplicate Payment or an offline Payment.
        /// </summary>
        /// <param name="payment">The Payment to accept.</param>
        /// <param name="challenge">The Challenge that resulted in Payment 
        /// rejection.</param>
        void RejectPayment(Payment payment, Challenge challenge);

        /// <summary>
        /// Requests an Auth transaction. The tip for an Auth can be adjusted through the 
        /// TipAdjustAuth() call until the batch Closeout is processed.
        /// </summary>
        /// <param name="request">The AuthRequest details.</param>
        void Auth(AuthRequest request);

        /// <summary>
        /// Initiates a PreAuth transaction (a pre-authorization for a certain amount). 
        /// This transaction lets the merchant know whether the account associated with a 
        /// card has sufficient funds, without actually charging 
        /// the card. When the merchant is ready to charge a final amount, the POS will 
        /// call CapturePreAuth() to complete the Payment.
        /// </summary>
        /// <param name="request">The PreAuthRequest details.</param>
        void PreAuth(PreAuthRequest request);

        /// <summary>
        /// Marks a PreAuth Payment for capture by a Closeout process. After a PreAuth is 
        /// captured, it is effectively the same as an Auth Payment. Note: Should only be 
        /// called if the request's PaymentID is from a PreAuthResponse.
        /// </summary>
        /// <param name="request">The CapturePreAuthRequest details.</param>
        void CapturePreAuth(CapturePreAuthRequest request);

        /// <summary>
        /// Adjusts the tip for a previous Auth transaction. This call can be made until 
        /// the Auth Payment has been finalized by a Closeout. Note: Should only be called 
        /// if the request's PaymentID is from an AuthResponse.
        /// </summary>
        /// <param name="request">The TipAdjustAuthRequest details.</param>
        void TipAdjustAuth(TipAdjustAuthRequest request);

        /// <summary>
        /// Voids a transaction.
        /// </summary>
        /// <param name="request">A VoidRequest object containing basic information needed 
        /// to void the transaction.</param>
        void VoidPayment(VoidPaymentRequest request);

        /// <summary>
        /// Refunds the full or partial amount of a Payment.
        /// </summary>
        /// <param name="request">The RefundPaymentRequest details.</param>
        void RefundPayment(RefundPaymentRequest request);

        /// <summary>
        /// Initiates a Manual Refund transaction (a “Refund” or credit that is not 
        /// associated with a previous Payment).
        /// </summary>
        /// <param name="request">A ManualRefundRequest object with the request 
        /// details.</param>
        void ManualRefund(ManualRefundRequest request);

        /// <summary>
        /// Asks the Clover device to capture card information and request a payment token 
        /// from the payment gateway. The payment token can be used for future Sale and 
        /// Auth requests in place of the card details. 
        /// Note: The merchant account must be configured to allow payment tokens.
        /// </summary>
        /// <param name="CardEntryMethods">The card entry methods allowed to capture the 
        /// payment token. 
        /// If this parameter is null, the default values (CARD_ENTRY_METHOD_MAG_STRIPE, 
        /// CARD_ENTRY_METHOD_ICC_CONTACT, and CARD_ENTRY_METHOD_NFC_CONTACTLESS) 
        /// will be used.</param>
        void VaultCard(int? CardEntryMethods);

        /// <summary>
        /// Requests card information (specifically Track 1 and Track 2 card data).
        /// </summary>
        /// <param name="request">The ReadCardDataRequest details.</param>
        void ReadCardData(ReadCardDataRequest request);

        /// <summary>
        /// Sends a "cancel" button press to the Clover device.
        /// </summary>
        [System.Obsolete("Use InvokeInputOption(InputOption) instead.")]
        void Cancel();

        /// <summary>
        /// Sends a request to the Clover server to close out all transactions.
        /// Note: The merchant account must be configured to allow transaction closeout.
        /// </summary>
        /// <param name="request">The CloseoutRequest details.</param>
        void Closeout(CloseoutRequest request);

        /// <summary>
        /// Sends a request to reset the Clover device back to the welcome screen. Can be 
        /// used when the device is in an unknown or invalid state from the perspective of 
        /// the POS. 
        /// Note: This request could cause the POS to miss a transaction or other 
        /// information. Use cautiously as a last resort.
        /// </summary>
        void ResetDevice();

        /// <summary>
        /// Prints custom messages in plain text through the Clover Mini's built-in 
        /// printer.
        /// </summary>
        /// <param name="messages">An array of text messages to print.</param>
        [System.Obsolete("Use Print(PrintRequest request) instead.")]
        void PrintText(List<string> messages);

        /// <summary>
        /// Prints an image on paper receipts through the Clover Mini's built-in printer.
        /// </summary>
        /// <param name="bitmap">The image to print.</param>
        [System.Obsolete("Use Print(PrintRequest request) instead.")]
        void PrintImage(Bitmap bitmap);

        /// <summary>
        /// Displays a string-based message on the Clover device's screen.
        /// </summary>
        /// <param name="message">The string message to display.</param>
        void ShowMessage(string message);

        /// <summary>
        /// Displays the welcome screen on the Clover device.
        /// </summary>
        void ShowWelcomeScreen();

        /// <summary>
        /// Displays the thank you screen on the Clover device.
        /// </summary>
        void ShowThankYouScreen();

        /// <summary>
        /// Displays the customer-facing receipt options (print, email, etc.) for a 
        /// Payment on the Clover device. 
        /// </summary>
        /// <param name="paymentId">The ID of the Payment associated with the 
        /// receipt.</param>
        /// <param name="orderId">The ID of the Order associated with the receipt.</param>
        void DisplayPaymentReceiptOptions(String orderId, String paymentId);

        /// <summary>
        /// Opens the first cash drawer found connected to the Clover device.
        /// </summary>
        /// <param name="reason">The reason for opening the cash drawer.</param>
        [System.Obsolete("Use OpenCashDrawer(OpenCashDrawerRequest request) instead.")]
        void OpenCashDrawer(String reason);

        /// <summary>
        /// Opens the first cash drawer found connected to the Clover device. The reason 
        /// for opening the cash drawer must be provided.
        /// </summary>
        /// <param name="request">Text specifying the reason for opening the cash 
        /// drawer.</param>
        void OpenCashDrawer(OpenCashDrawerRequest request);

        /// <summary>
        /// Displays an Order and associated line items on the Clover device. Will replace 
        /// an Order that is already displayed on the device screen.
        /// </summary>
        /// <param name="order">The DisplayOrder to display.</param>
        void ShowDisplayOrder(DisplayOrder order);

        /// <summary>
        /// Removes the DisplayOrder object from the Clover device's screen.
        /// </summary>
        /// <param name="order">The DisplayOrder to remove.</param>
        void RemoveDisplayOrder(DisplayOrder order);

        /// <summary>
        /// Disposes the connection to the Clover device. After this is called, the 
        /// connection to the device is severed, and the CloverConnector object is no 
        /// longer usable. Instantiate a new CloverConnector object in 
        /// order to call InitializeConnection().
        /// </summary>
        void Dispose();

        /// <summary>
        /// Sends a keystroke to the Clover device that invokes an input option (OK, 
        /// CANCEL, DONE, etc.) on the customer's behalf. InputOptions are on the 
        /// CloverDeviceEvent passed to OnDeviceActivityStart().
        /// </summary>
        /// <param name="io">The input option to invoke.</param>
        void InvokeInputOption(transport.InputOption io);

        /// <summary>
        /// Prints an image from the web on paper receipts through the Clover device's 
        /// built-in printer.
        /// </summary>
        /// <param name="ImgURL">The URL for the image to print.</param>
        [System.Obsolete("Use Print(PrintRequest request) instead.")]
        void PrintImageFromURL(String ImgURL);

        /// <summary>
        /// Sends a print request using the PrintRequest object. Used to print text, 
        /// Bitmap image objects, and images from URLs using the specified printer.
        /// </summary>
        /// <param name="request">The PrintRequest details.</param>
        void Print(PrintRequest request);

        /// <summary>
        /// Queries available printers attached to the Clover device using the 
        /// RetrievePrintersRequest object.
        /// </summary>
        /// <param name="request">The RetrievePrintersRequest details.</param>
        void RetrievePrinters(RetrievePrintersRequest request);

        /// <summary>
        /// Queries the status of a print job using the PrintJobStatusRequest object.
        /// </summary>
        /// <param name="request">The PrintJobStatusRequest details.</param>
        void RetrievePrintJobStatus(PrintJobStatusRequest request);

        /// <summary>
        /// Retrieves a list of unprocessed Payments that were taken offline and are 
        /// pending submission to the server.
        /// </summary>
        void RetrievePendingPayments();

        /// <summary>
        /// Starts a Custom Activity on the Clover device.
        /// Note: The Custom Activity must already be set up and configured on the Clover 
        /// device.
        /// </summary>
        /// <param name="request">The CustomActivityRequest details.</param>
        void StartCustomActivity(CustomActivityRequest request);

        /// <summary>
        /// Sends a message to a Custom Activity running on a Clover device.
        /// </summary>
        /// <param name="request">The MessageToActivity details to send to the Custom 
        /// Activity.</param>
        void SendMessageToActivity(MessageToActivity request);

        /// <summary>
        /// Sends a message requesting the current status of the Clover device.
        /// </summary>
        /// <param name="request">The RetrieveDeviceStatusRequest details.</param>
        void RetrieveDeviceStatus(RetrieveDeviceStatusRequest request);

        /// <summary>
        /// Requests the Payment information associated with the externalPaymentId passed 
        /// in. Only valid for Payments made in the past 24 hours on the Clover device 
        /// queried.
        /// </summary>
        /// <param name="request">The RetrievePaymentRequest details.</param>
        void RetrievePayment(RetrievePaymentRequest request);
    }
}
