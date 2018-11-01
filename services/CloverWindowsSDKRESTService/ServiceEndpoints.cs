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

using Grapevine.Server;
using System.Net;
using com.clover.remotepay.transport;
using com.clover.remotepay.sdk.service.client;
using CloverWindowsSDKREST;
using com.clover.remotepay.sdk;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.IO;
using com.clover.sdk.v3.payments;
using System;
using System.Drawing;

public sealed class ServiceEndpoints : RESTResource
{
    public bool? DisablePrinting { get; set; }
    public bool? DisableCashBack { get; set; }
    public bool? DisableTip { get; set; }
    public bool? DisableRestartTransactionOnFail { get; set; }

    public CloverRESTServer GetServer
    {
        get
        {
            return (CloverRESTServer)Server;
        }
    }


    public ServiceEndpoints() : base()
    {
    }



    [RESTRoute(Method = Grapevine.HttpMethod.GET, PathInfo = @"^/Clover/Status$")]
    public void Status(HttpListenerContext context)
    {
        GetServer.ForwardToClientListener.ResendStatus();
        JObject jobj = new JObject();
        jobj.Add("Status", GetServer.ForwardToClientListener.Status);
        string payload = JsonUtils.Serialize(jobj);
        this.SendTextResponse(context, payload);
    }


    [RESTRoute(Method = Grapevine.HttpMethod.GET, PathInfo = @"^/Clover/SDKInfo$")]
    public void SDKInfo(HttpListenerContext context)
    {
        string payload = JsonUtils.Serialize(GetServer.CloverConnector.SDKInfo);
        this.SendTextResponse(context, payload);
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ShowMessage$")]
    public void ShowMessage(HttpListenerContext context)
    {
        try
        {
            ShowMessage message = ParseRequest<ShowMessage>(context);
            GetServer.CloverConnector.ShowMessage(message.Message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ResetDevice$")]
    public void ResetDevice(HttpListenerContext context)
    {
        GetServer.CloverConnector.ResetDevice();
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ShowWelcomeScreen$")]
    public void ShowWelcomeScreen(HttpListenerContext context)
    {
        GetServer.CloverConnector.ShowWelcomeScreen();
        this.SendTextResponse(context, "");
    }


    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/RetrievePendingPayments$")]
    public void RetrievePendingPayments(HttpListenerContext context)
    {
        GetServer.CloverConnector.RetrievePendingPayments();
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ShowThankYouScreen$")]
    public void ShowThankYouScreen(HttpListenerContext context)
    {
        this.GetServer.CloverConnector.ShowThankYouScreen();
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/Auth$")]
    public void Auth(HttpListenerContext context)
    {
        try
        {
            AuthRequest message = ParseRequest<AuthRequest>(context);
            GetServer.CloverConnector.Auth(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/PreAuth$")]
    public void PreAuth(HttpListenerContext context)
    {
        try
        {
            PreAuthRequest message = ParseRequest<PreAuthRequest>(context);

            GetServer.CloverConnector.PreAuth(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/Sale$")]
    public void Sale(HttpListenerContext context)
    {
        try
        {
            SaleRequest message = ParseRequest<SaleRequest>(context);
            GetServer.CloverConnector.Sale(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

        //Console.WriteLine(context.Response.StatusCode);
        //Console.WriteLine(context.Response.StatusDescription);
        //context.Response.StatusCode

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/VoidPayment$")]
    public void VoidPayment(HttpListenerContext context)
    {
        try
        {
            VoidPaymentRequest message = ParseRequest<VoidPaymentRequest>(context);
            GetServer.CloverConnector.VoidPayment(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/RefundPayment$")]
    public void RefundPayment(HttpListenerContext context)
    {
        try
        {
            RefundPaymentRequest message = ParseRequest<RefundPaymentRequest>(context);
            GetServer.CloverConnector.RefundPayment(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ManualRefund$")]
    public void ManualRefund(HttpListenerContext context)
    {
        try
        {
            ManualRefundRequest message = ParseRequest<ManualRefundRequest>(context);
            GetServer.CloverConnector.ManualRefund(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/VaultCard$")]
    public void VaultCard(HttpListenerContext context)
    {
        try
        {
            VaultCard vaultCard = ParseRequest<VaultCard>(context);
            GetServer.CloverConnector.VaultCard(vaultCard.CardEntryMethods);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ReadCardData$")]
    public void ReadCardData(HttpListenerContext context)
    {
        try
        {
            ReadCardDataRequest readCardDataRequest = ParseRequest<ReadCardDataRequest>(context);
            GetServer.CloverConnector.ReadCardData(readCardDataRequest);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/CapturePreAuth$")]
    public void CapturePreAuth(HttpListenerContext context)
    {
        try
        {
            CapturePreAuthRequest message = ParseRequest<CapturePreAuthRequest>(context);
            GetServer.CloverConnector.CapturePreAuth(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/TipAdjustAuth$")]
    public void TipAdjustAuth(HttpListenerContext context)
    {
        try
        {
            TipAdjustAuthRequest message = ParseRequest<TipAdjustAuthRequest>(context);
            GetServer.CloverConnector.TipAdjustAuth(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/Closeout$")]
    public void Closeout(HttpListenerContext context)
    {
        try
        {
            CloseoutRequest message = ParseRequest<CloseoutRequest>(context);
            GetServer.CloverConnector.Closeout(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/Print$")]
    public void Print(HttpListenerContext context)
    {
        try
        {

            PrintRequest64 message = ParseRequest<PrintRequest64>(context);
            PrintRequest request = null;
            if (message != null)
            {
                if (message.base64strings.Count > 0)
                {

                    byte[] imgBytes = Convert.FromBase64String(message.base64strings[0]);
                    MemoryStream ms = new MemoryStream();
                    ms.Write(imgBytes, 0, imgBytes.Length);
                    Bitmap bp = new Bitmap(ms);
                    ms.Close();
                    request = new PrintRequest(bp, message.externalPrintJobId, message.printDeviceId);
                }
                else if (message.imgUrls.Count > 0)
                {
                    request = new PrintRequest(message.imgUrls[0], message.externalPrintJobId, message.printDeviceId);
                }
                else if (message.textLines.Count > 0)
                {
                    request = new PrintRequest(message.textLines, message.externalPrintJobId, message.printDeviceId);
                }
                GetServer.CloverConnector.Print(request);
                this.SendTextResponse(context, "");
            }
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/DisplayPaymentReceiptOptions$")]
    public void DisplayPaymentReceiptOptions(HttpListenerContext context)
    {
        try
        {
            DisplayPaymentReceiptOptionsRequest message = ParseRequest<DisplayPaymentReceiptOptionsRequest>(context);
            GetServer.CloverConnector.DisplayPaymentReceiptOptions(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/OpenCashDrawer$")]
    public void OpenCashDrawer(HttpListenerContext context)
    {
        try
        {
            OpenCashDrawerRequest message = ParseRequest<OpenCashDrawerRequest>(context);
            GetServer.CloverConnector.OpenCashDrawer(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/DisplayOrder$")]
    public void ShowDisplayOrder(HttpListenerContext context)
    {
        try
        {
            com.clover.remote.order.DisplayOrder order = ParseRequest<com.clover.remote.order.DisplayOrder>(context);
            GetServer.CloverConnector.ShowDisplayOrder(order);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    public void RemoveDisplayOrder(HttpListenerContext context)
    {
        try
        {
            com.clover.remote.order.DisplayOrder order = ParseRequest<com.clover.remote.order.DisplayOrder>(context);
            GetServer.CloverConnector.RemoveDisplayOrder(order);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/AcceptSignature$")]
    public void AcceptSignature(HttpListenerContext context)
    {
        try
        {
            VerifySignatureRequest message = ParseRequest<VerifySignatureRequest>(context);
            GetServer.CloverConnector.AcceptSignature(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/RejectSignature$")]
    public void RejectSignature(HttpListenerContext context)
    {
        try
        {
            VerifySignatureRequest message = ParseRequest<VerifySignatureRequest>(context);
            GetServer.CloverConnector.RejectSignature(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/InvokeInputOption$")]
    public void InvokeInputOption(HttpListenerContext context)
    {
        try
        {
            InputOption message = ParseRequest<InputOption>(context);
            GetServer.CloverConnector.InvokeInputOption(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/AcceptPayment$")]
    public void AcceptPayment(HttpListenerContext context)
    {
        try
        {
            Payment payment = ParseRequest<Payment>(context);
            GetServer.CloverConnector.AcceptPayment(payment);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/RejectPayment$")]
    public void RejectPayment(HttpListenerContext context)
    {
        try
        {
            RejectPaymentObject message = ParseRequest<RejectPaymentObject>(context);
            GetServer.CloverConnector.RejectPayment(message.Payment, message.Challenge);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/StartCustomActivity$")]
    public void StartCustomActivity(HttpListenerContext context)
    {
        try
        {
            CustomActivityRequest message = ParseRequest<CustomActivityRequest>(context);
            GetServer.CloverConnector.StartCustomActivity(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/SendMessageToActivity$")]
    public void SendMessageToActivity(HttpListenerContext context)
    {
        try
        {
            MessageToActivity message = ParseRequest<MessageToActivity>(context);
            GetServer.CloverConnector.SendMessageToActivity(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }

    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/RetrieveDeviceStatus$")]
    public void RetrieveDeviceStatus(HttpListenerContext context)
    {
        try
        {
            RetrieveDeviceStatusRequest message = ParseRequest<RetrieveDeviceStatusRequest>(context);
            GetServer.CloverConnector.RetrieveDeviceStatus(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/RetrievePayment$")]
    public void RetrievePayment(HttpListenerContext context)
    {
        try
        {
            RetrievePaymentRequest message = ParseRequest<RetrievePaymentRequest>(context);
            GetServer.CloverConnector.RetrievePayment(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/RetrievePrinters")]
    public void RetrievePrinters(HttpListenerContext context)
    {
        try
        {
            RetrievePrintersRequest message = ParseRequest<RetrievePrintersRequest>(context);
            GetServer.CloverConnector.RetrievePrinters(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/RetrievePrintJobStatus")]
    public void PrintJobStatus(HttpListenerContext context)
    {
        try
        {
            PrintJobStatusRequest message = ParseRequest<PrintJobStatusRequest>(context);
            GetServer.CloverConnector.RetrievePrintJobStatus(message);
            this.SendTextResponse(context, "");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.StatusDescription = e.Message;
            this.SendTextResponse(context, "error processing request");
        }
    }

    public void AddCloverConnectorListener(ICloverConnectorListener connectorListener)
    {
        GetServer.CloverConnector.AddCloverConnectorListener(connectorListener);
    }

    public void RemoveCloverConnectorListener(ICloverConnectorListener connectorListener)
    {
        GetServer.CloverConnector.RemoveCloverConnectorListener(connectorListener);
    }

    public void Dispose()
    {
        // not applicable to the service
    }

    private T ParseRequest<T>(HttpListenerContext context)
    {
        if (context.Request.ContentType != "application/json")
        {
            throw new HttpListenerException(500, "Unexpected Content Type. Expecting 'application/json'");
        }


        StreamReader stream = new StreamReader(context.Request.InputStream);
        string x = stream.ReadToEnd();  // added to view content of input stream

        T message = JsonUtils.Deserialize<T>(x, new Newtonsoft.Json.JsonConverter[] { new StringEnumConverter() });

        return message;
    }
}