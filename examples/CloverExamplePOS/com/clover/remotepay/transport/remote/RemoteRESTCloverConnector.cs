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

using com.clover.remotepay.sdk;
using com.clover.remotepay.transport.remote;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using com.clover.remote.order;
using System.IO;
using System.Drawing.Imaging;
using com.clover.remotepay.sdk.service.client;
using com.clover.remotepay.data;

namespace com.clover.remotepay.transport.remote
{
    public class RemoteRESTCloverConnector : ICloverConnector
    {
        RestClient restClient = new RestClient("http://localhost:8181/Clover");

        CallbackController callbackService { get; set; }
        public RemoteRESTCloverConnector(CloverDeviceConfiguration config)
        {
            Thread workerThread = new Thread(DoWork);
            workerThread.IsBackground = true;

            callbackService = new CallbackController(this);
            workerThread.Start();

            CardEntryMethod = CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT | CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE | CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS;
        }

        public void DoWork()
        {
            callbackService.init(restClient);
            
            // call GetStatus to see if the service is connected...
           
        }

        public int CardEntryMethod { get; set; }
        public bool DisablePrinting { get; set; }
        public bool DisableCashBack { get; set;}
        public bool DisableTip { get; set; }
        public bool DisableRestartTransactionOnFail { get; set; }

        List<CloverConnectorListener> listeners = new List<CloverConnectorListener>();
        private CloverDeviceConfiguration config;

        public void AddCloverConnectorListener(CloverConnectorListener connectorListener)
        {
            listeners.Add(connectorListener);
            callbackService.AddListener(connectorListener);
        }

        public void RemoveCloverConnectorListener(CloverConnectorListener connectorListener)
        {
            listeners.Remove(connectorListener);
        }

        public int Sale(SaleRequest request)
        {
            Send("/Sale", request);
            return 0;
        }

        public int AcceptSignature(SignatureVerifyRequest request)
        {
            Send("/AcceptSignature", request);
            return 0;
        }

        public int RejectSignature(SignatureVerifyRequest request)
        {
            Send("/RejectSignature", request);
            return 0;
        }

        public int Auth(AuthRequest request)
        {
            Send("/Auth", request);
            return 0;
        }

        public int PreAuth(PreAuthRequest request)
        {
            Send("/PreAuth", request);
            return 0;
        }

        public int CaptureAuth(CaptureAuthRequest request)
        {
            Send("/CaptureAuth", request);
            return 0;
        }

        public int TipAdjustAuth(TipAdjustAuthRequest request)
        {
            Send("/TipAdjustAuth", request);
            return 0;
        }

        public int VoidPayment(VoidPaymentRequest request)
        {
            Send("/VoidPayment", request);
            return 0;
        }

        public int VoidTransaction(VoidTransactionRequest request)
        {
            Send("/VoidTransaction", request);
            return 0;
        }

        public int RefundPayment(RefundPaymentRequest request)
        {
            Send("/RefundPayment", request);
            return 0;
        }

        public int ManualRefund(ManualRefundRequest request)
        {
            Send("/ManualRefund", request);
            return 0;
        }

        public int VaultCard(int? CardEntryMethods)
        {
            VaultCard vc = new VaultCard();
            vc.CardEntryMethod = CardEntryMethods;
            Send("/VaultCard", vc);
            return 0;
        }

        public int Closeout(CloseoutRequest request)
        {
            Send("/Closeout", request);
            return 0;
        }

        public int ResetDevice()
        {
            Send("/ResetDevice", null);
            return 0;
        }

        public int Cancel()
        {
            Send("/Cancel", null);
            return 0;
        }

        public int PrintText(List<string> messages)
        {
            PrintText pt = new PrintText();
            pt.Messages = messages;
            Send("/PrintText", pt);
            return 0;
        }

        public int PrintImage(System.Drawing.Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            byte[] imgBytes = ms.ToArray();
            string base64Image = Convert.ToBase64String(imgBytes);

            PrintImage pi = new PrintImage();
            pi.Bitmap = base64Image;
            Send("/PrintImage", pi);
            return 0;
        }

        public int ShowMessage(string message)
        {
            ShowMessage msg = new ShowMessage();
            msg.Message = message;
            Send("/ShowMessage", msg);
            return 0;
        }

        public int ShowWelcomeScreen()
        {
            Send("/ShowWelcomeScreen", null);
            return 0;
        }

        public void ShowThankYouScreen()
        {
            Send("/ShowThankYouScreen", null);
        }

        public void DisplayPaymentReceiptOptions(String orderId, String paymentId)
        {
            DisplayPaymentReceiptOptionsRequest req = new DisplayPaymentReceiptOptionsRequest();
            req.OrderID = orderId;
            req.PaymentID = paymentId;
            Send("/DisplayPaymentReceiptOptions", req);
        }

        public void DisplayRefundReceiptOptions(string orderId, string refundId)
        {
            DisplayRefundReceiptOptionsRequest req = new DisplayRefundReceiptOptionsRequest();
            req.OrderID = orderId;
            req.RefundID = refundId;
            Send("/DisplayRefundReceiptOptions", req);
        }

        public void DisplayCreditReceiptOptions(string orderId, string creditId)
        {
            DisplayCreditReceiptOptionsRequest req = new DisplayCreditReceiptOptionsRequest();
            req.OrderID = orderId;
            req.CreditID = creditId;
            Send("/DisplayCreditReceiptOptions", req);
        }

        public void OpenCashDrawer(string reason)
        {
            OpenCashDrawer ocd = new OpenCashDrawer();
            ocd.Reason = reason;
            Send("/OpenCashDrawer", ocd);
        }

        public void DisplayOrder(global::com.clover.remote.order.DisplayOrder order)
        {
            Send("/DisplayOrder", order);            
        }

        public void DisplayOrderLineItemAdded(global::com.clover.remote.order.DisplayOrder order, DisplayLineItem lineItem)
        {
            DisplayOrderLineItemAdded dolia = new DisplayOrderLineItemAdded();
            dolia.DisplayOrder = order;
            dolia.DisplayLineItem = lineItem;
            Send("/DisplayOrderLineItemAdded", dolia);
        }

        public void DisplayOrderLineItemRemoved(global::com.clover.remote.order.DisplayOrder order, DisplayLineItem lineItem)
        {
            DisplayOrderLineItemRemoved dolia = new DisplayOrderLineItemRemoved();
            dolia.DisplayOrder = order;
            dolia.DisplayLineItem = lineItem;
            Send("/DisplayOrderLineItemRemoved", dolia);
        }

        public void DisplayOrderDiscountAdded(global::com.clover.remote.order.DisplayOrder order, DisplayDiscount discount)
        {
            DisplayOrderDiscountAdded doda = new DisplayOrderDiscountAdded();
            doda.DisplayOrder = order;
            doda.DisplayDiscount = discount;
            Send("/DisplayOrderDiscountAdded", doda);   
        }

        public void DisplayOrderDiscountRemoved(global::com.clover.remote.order.DisplayOrder order, DisplayDiscount discount)
        {
            DisplayOrderDiscountRemoved doda = new DisplayOrderDiscountRemoved();
            doda.DisplayOrder = order;
            doda.DisplayDiscount = discount;
            Send("/DisplayOrderDiscountRemoved", doda);  
        }

        public void DisplayOrderDelete(global::com.clover.remote.order.DisplayOrder order)
        {
            
        }

        public void GetMerchantInfo()
        {
            
        }

        public void Dispose()
        {
            
        }

        public void InvokeInputOption(global::com.clover.remotepay.transport.InputOption io)
        {
            Send("/InvokeInputOption", io);
        }

        public int Send(string target, object payload)
        {
            IRestRequest restRequest = new RestRequest(target, Method.POST);
            restRequest.AddJsonBody(payload);
            restRequest.RequestFormat = DataFormat.Json;
            restClient.ExecuteAsync(restRequest, response =>
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine(response.ResponseStatus + " : " + response.StatusCode + " : " + response.ErrorMessage);
                }
            });
            return 0;
        }

        public class RESTSigVerRequestHandler : SignatureVerifyRequest
        {
            SignatureVerifyRequest svr;
            RemoteRESTCloverConnector restCloverConnector;
            public RESTSigVerRequestHandler(RemoteRESTCloverConnector cloverConnector, SignatureVerifyRequest request)
            {
                restCloverConnector = cloverConnector;
                svr = request;
                Payment = request.Payment;
                Signature = request.Signature;
            }
            public override void Accept()
            {
                restCloverConnector.Send("/AcceptSignature", svr);
            }

            public override void Reject()
            {
                restCloverConnector.Send("/RejectSignature", svr);
            }
        }
    }
}
