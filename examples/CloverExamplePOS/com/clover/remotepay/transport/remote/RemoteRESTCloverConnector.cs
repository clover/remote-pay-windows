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

using com.clover.remotepay.sdk;
using com.clover.remote.order;
using com.clover.remotepay.sdk.service.client;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using com.clover.sdk.v3.payments;
using System.Drawing;

namespace com.clover.remotepay.transport.remote
{
    /// <summary>
    /// Custom ICloverConnector that talks to the 
    /// Clover Connector REST Service. This wouldn't normally
    /// be used because it is in .NET, and it would normally
    /// make more sense to use the DLL directly in .NET
    /// </summary>
    public class RemoteRESTCloverConnector : ICloverConnector
    {
        public bool IsReady
        {
            get { return true; }
            set { }
        }
        RestClient restClient = new RestClient("http://localhost:8181/Clover");
        CloverDeviceConfiguration Config;
        CallbackController callbackService { get; set; }
        public RemoteRESTCloverConnector(CloverDeviceConfiguration config)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("CloverConnector");
            _SDKInfo = assembly.GetAssemblyAttribute<System.Reflection.AssemblyDescriptionAttribute>().Description + ":"
                + (assembly.GetAssemblyAttribute<System.Reflection.AssemblyFileVersionAttribute>()).Version
                + (assembly.GetAssemblyAttribute<System.Reflection.AssemblyInformationalVersionAttribute>()).InformationalVersion;
            Config = config;
        }
        public void InitializeConnection()
        {
            if (callbackService == null)
            {
                callbackService = new CallbackController(this);
                callbackService.init(restClient);

                CardEntryMethod = CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT | CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE | CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS;

                listeners.ForEach(listener => callbackService.AddListener(listener));
            }
        }
        string _SDKInfo;
        public string SDKInfo
        {
            get
            {
                return this._SDKInfo;
            }
        }
        public int CardEntryMethod { get; set; }
        public bool DisablePrinting { get; set; }
        public bool DisableCashBack { get; set; }
        public bool DisableTip { get; set; }
        public bool DisableRestartTransactionOnFail { get; set; }

        List<ICloverConnectorListener> listeners = new List<ICloverConnectorListener>();

        public void AddCloverConnectorListener(ICloverConnectorListener connectorListener)
        {
            listeners.Add(connectorListener);
            if (callbackService != null)
            {
                callbackService.AddListener(connectorListener);
            }
        }

        public void RemoveCloverConnectorListener(ICloverConnectorListener connectorListener)
        {
            listeners.Remove(connectorListener);
        }

        public void Sale(SaleRequest request)
        {
            Send("/Sale", request);
        }

        public void AcceptSignature(VerifySignatureRequest request)
        {
            Send("/AcceptSignature", request);
        }

        public void RejectSignature(VerifySignatureRequest request)
        {
            Send("/RejectSignature", request);
        }

        public void Auth(AuthRequest request)
        {
            Send("/Auth", request);
        }

        public void PreAuth(PreAuthRequest request)
        {
            Send("/PreAuth", request);
        }

        public void CapturePreAuth(CapturePreAuthRequest request)
        {
            Send("/CapturePreAuth", request);
        }

        public void TipAdjustAuth(TipAdjustAuthRequest request)
        {
            Send("/TipAdjustAuth", request);
        }

        public void VoidPayment(VoidPaymentRequest request)
        {
            Send("/VoidPayment", request);
        }

        public void RefundPayment(RefundPaymentRequest request)
        {
            Send("/RefundPayment", request);
        }

        public void ManualRefund(ManualRefundRequest request)
        {
            Send("/ManualRefund", request);
        }

        public void VaultCard(int? CardEntryMethods)
        {
            VaultCard vc = new VaultCard();
            vc.CardEntryMethods = CardEntryMethods;
            Send("/VaultCard", vc);
        }

        public void ReadCardData(ReadCardDataRequest request)
        {
            Send("/ReadCardData", request);
        }

        public void StartCustomActivity(CustomActivityRequest request)
        {
            Send("/StartCustomActivity", request);
        }

        public void Closeout(CloseoutRequest request)
        {
            Send("/Closeout", request);
        }

        public void ResetDevice()
        {
            Send("/ResetDevice", null);
        }

        public void Cancel()
        {
            Send("/Cancel", null);
        }

        public void Print(PrintRequest request)
        {
            PrintRequest64 printRequest = new PrintRequest64();
            if (request.images.Count >0)
            {
                Bitmap bitmap = request.images[0];
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Png);
                byte[] imgBytes = ms.ToArray();
                string base64Image = Convert.ToBase64String(imgBytes);

                printRequest.setBase64Strings(base64Image); 
            }
            else if(request.imageURLs.Count > 0) 
            {
                printRequest.setImageUrls(request.imageURLs[0]);
            }
            else if(request.text.Count > 0)
            {
                printRequest.setText(request.text);
            }
            printRequest.externalPrintJobId = request.printRequestId;
            printRequest.printDeviceId = request.printDeviceId;
            Send("/Print", printRequest);

        }

        public void PrintText(List<string> messages)
        {
            PrintText pt = new PrintText();
            pt.Messages = messages;
            Send("/PrintText", pt);
        }

        public void PrintImage(System.Drawing.Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            byte[] imgBytes = ms.ToArray();
            string base64Image = Convert.ToBase64String(imgBytes);

            PrintImage pi = new PrintImage();
            pi.Bitmap = base64Image;
            Send("/PrintImage", pi);
        }

        public void ShowMessage(string message)
        {
            ShowMessage msg = new ShowMessage();
            msg.Message = message;
            Send("/ShowMessage", msg);
        }

        public void ShowWelcomeScreen()
        {
            Send("/ShowWelcomeScreen", null);
        }

        public void ShowThankYouScreen()
        {
            Send("/ShowThankYouScreen", null);
        }

        public void RetrievePendingPayments()
        {
            Send("/RetrievePendingPayments", null);
        }

        public void DisplayPaymentReceiptOptions(String orderId, String paymentId)
        {
            DisplayPaymentReceiptOptionsRequest req = new DisplayPaymentReceiptOptionsRequest();
            req.OrderID = orderId;
            req.PaymentID = paymentId;
            Send("/DisplayPaymentReceiptOptions", req);
        }

        public void OpenCashDrawer(string reason)
        {
            OpenCashDrawerRequest ocd = new OpenCashDrawerRequest(reason);
            Send("/OpenCashDrawer", ocd);
        }

        public void ShowDisplayOrder(DisplayOrder order)
        {
            Send("/DisplayOrder", order);
        }

        public void LineItemAddedToDisplayOrder(DisplayOrder order, DisplayLineItem lineItem)
        {
            LineItemAddedToDisplayOrder dolia = new LineItemAddedToDisplayOrder();
            dolia.DisplayOrder = order;
            dolia.DisplayLineItem = lineItem;
            Send("/LineItemAddedToDisplayOrder", dolia);
        }

        public void LineItemRemovedFromDisplayOrder(DisplayOrder order, DisplayLineItem lineItem)
        {
            LineItemRemovedFromDisplayOrder dolia = new LineItemRemovedFromDisplayOrder();
            dolia.DisplayOrder = order;
            dolia.DisplayLineItem = lineItem;
            Send("/LineItemRemovedFromDisplayOrder", dolia);
        }

        public void DiscountAddedToDisplayOrder(DisplayOrder order, DisplayDiscount discount)
        {
            DiscountAddedToDisplayOrder doda = new DiscountAddedToDisplayOrder();
            doda.DisplayOrder = order;
            doda.DisplayDiscount = discount;
            Send("/DiscountAddedToDisplayOrder", doda);
        }

        public void DiscountRemovedFromDisplayOrder(DisplayOrder order, DisplayDiscount discount)
        {
            DiscountRemovedFromDisplayOrder doda = new DiscountRemovedFromDisplayOrder();
            doda.DisplayOrder = order;
            doda.DisplayDiscount = discount;
            Send("/DiscountRemovedFromDisplayOrder", doda);
        }


        public void Dispose()
        {
            callbackService.Shutdown();
        }

        public void InvokeInputOption(global::com.clover.remotepay.transport.InputOption io)
        {
            Send("/InvokeInputOption", io);
        }

        public void Send(string target, object payload)
        {
            IRestRequest restRequest = new RestRequest(target, Method.POST);
            string payloadMessage = JsonUtils.serialize(payload);
#if DEBUG
            Console.WriteLine("Sending: " + target + " JSON: " + payloadMessage);
#endif
            restRequest.AddParameter("application/json", payloadMessage, ParameterType.RequestBody);
            restClient.ExecuteAsync(restRequest, response =>
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine(response.ResponseStatus + " : " + response.StatusCode + " : " + response.ErrorMessage + " => " + response.Content);
                }
            });
        }

        public void PrintImageFromURL(string ImgURL)
        {
            PrintImage pi = new PrintImage();
            pi.Url = ImgURL;
            Send("/PrintImageFromURL", pi);
        }

        public void RemoveDisplayOrder(DisplayOrder displayOrder)
        {
            ShowWelcomeScreen();
        }

        public void AcceptPayment(Payment payment)
        {
            Send("/AcceptPayment", payment);
        }

        public void RejectPayment(Payment payment, Challenge challenge)
        {
            RejectPaymentObject rejectPayment = new RejectPaymentObject();
            rejectPayment.Payment = payment;
            rejectPayment.Challenge = challenge;
            Send("/RejectPayment", rejectPayment);
        }

        public void SendMessageToActivity(MessageToActivity msg)
        {
            Send("/SendMessageToActivity", msg);
        }

        public void RetrieveDeviceStatus(RetrieveDeviceStatusRequest request)
        {
            Send("/RetrieveDeviceStatus", request);
        }

        public void RetrievePayment(RetrievePaymentRequest request)
        {
            Send("/RetrievePayment", request);
        }

        public void OpenCashDrawer(OpenCashDrawerRequest request)
        {
            Send("/OpenCashDrawer", request);
        }

        
        public void RetrievePrinters(RetrievePrintersRequest request)
        {
            Send("/RetrievePrinters", request);
        }

        public void RetrievePrintJobStatus(PrintJobStatusRequest request)
        {
            Send("/RetrievePrintJobStatus", request);
        }


        public class RESTSigVerRequestHandler : VerifySignatureRequest
        {
            VerifySignatureRequest svr;
            RemoteRESTCloverConnector restCloverConnector;
            public RESTSigVerRequestHandler(RemoteRESTCloverConnector cloverConnector, VerifySignatureRequest request)
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
