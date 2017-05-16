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

    [RESTRoute(Method =Grapevine.HttpMethod.GET, PathInfo = @"^/Clover/Status$")]
    public void Status(HttpListenerContext context)
    {
        GetServer.ForwardToClientListener.ResendStatus();
        JObject jobj = new JObject();
        jobj.Add("Status", GetServer.ForwardToClientListener.Status);
        string payload = JsonUtils.serialize(jobj);
        this.SendTextResponse(context, payload);
    }


    [RESTRoute(Method = Grapevine.HttpMethod.GET, PathInfo = @"^/Clover/SDKInfo$")]
    public void SDKInfo(HttpListenerContext context)
    {
        string payload = JsonUtils.serialize(GetServer.CloverConnector.SDKInfo);
        this.SendTextResponse(context, payload);
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ShowMessage$")]
    public void ShowMessage(HttpListenerContext context)
    {
        ShowMessage message = ParseRequest<ShowMessage>(context);

        GetServer.CloverConnector.ShowMessage(message.Message);
        this.SendTextResponse(context, "");
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

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/Cancel$")]
    public void Cancel(HttpListenerContext context)
    {
        GetServer.CloverConnector.Cancel();
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ShowThankYouScreen$")]
    public void ShowThankYouScreen(HttpListenerContext context)
    {
        this.GetServer.CloverConnector.ShowThankYouScreen();
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/PrintText$")]
    public void PrintText(HttpListenerContext context)
    {
        PrintText message = ParseRequest<PrintText>(context);

        GetServer.CloverConnector.PrintText(message.Messages);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/Auth$")]
    public void Auth(HttpListenerContext context)
    {
        AuthRequest message = ParseRequest<AuthRequest>(context);

        GetServer.CloverConnector.Auth(message);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/PreAuth$")]
    public void PreAuth(HttpListenerContext context)
    {
        PreAuthRequest message = ParseRequest<PreAuthRequest>(context);

        GetServer.CloverConnector.PreAuth(message);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/Sale$")]
    public void Sale(HttpListenerContext context)
    {
        SaleRequest message = ParseRequest<SaleRequest>(context);

        GetServer.CloverConnector.Sale(message);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/VoidPayment$")]
    public void VoidPayment(HttpListenerContext context)
    {
        VoidPaymentRequest message = ParseRequest<VoidPaymentRequest>(context);
        GetServer.CloverConnector.VoidPayment(message);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/RefundPayment$")]
    public void RefundPayment(HttpListenerContext context)
    {
        RefundPaymentRequest message = ParseRequest<RefundPaymentRequest>(context);
        GetServer.CloverConnector.RefundPayment(message);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ManualRefund$")]
    public void ManualRefund(HttpListenerContext context)
    {
        ManualRefundRequest message = ParseRequest<ManualRefundRequest>(context);
        GetServer.CloverConnector.ManualRefund(message);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/VaultCard$")]
    public void VaultCard(HttpListenerContext context)
    {
        VaultCard vaultCard = ParseRequest<VaultCard>(context);
        GetServer.CloverConnector.VaultCard(vaultCard.CardEntryMethods);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ReadCardData$")]
    public void ReadCardData(HttpListenerContext context)
    {
        ReadCardDataRequest readCardDataRequest = ParseRequest<ReadCardDataRequest>(context);
        GetServer.CloverConnector.ReadCardData(readCardDataRequest);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/CapturePreAuth$")]
    public void CapturePreAuth(HttpListenerContext context)
    {
        CapturePreAuthRequest message = ParseRequest<CapturePreAuthRequest>(context);
        GetServer.CloverConnector.CapturePreAuth(message);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/TipAdjustAuth$")]
    public void TipAdjustAuth(HttpListenerContext context)
    {
        TipAdjustAuthRequest message = ParseRequest<TipAdjustAuthRequest>(context);
        GetServer.CloverConnector.TipAdjustAuth(message);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/Closeout$")]
    public void Closeout(HttpListenerContext context)
    {
        CloseoutRequest message = ParseRequest<CloseoutRequest>(context);
        GetServer.CloverConnector.Closeout(message);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/PrintImage$")]
    public void PrintImage(HttpListenerContext context)
    {
        PrintImage message = ParseRequest<PrintImage>(context);
        if(message.Bitmap != null)
        {
            GetServer.CloverConnector.PrintImage(message.GetBitmap());
            this.SendTextResponse(context, "");
        }
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/PrintImageFromURL$")]
    public void PrintImageFromURL(HttpListenerContext context)
    {
        PrintImage message = ParseRequest<PrintImage>(context);
        if (message.Url != null)
        {
            GetServer.CloverConnector.PrintImageFromURL(message.Url);
            this.SendTextResponse(context, "");
        }
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/DisplayPaymentReceiptOptions$")]
    public void DisplayPaymentReceiptOptions(HttpListenerContext context)
    {
        DisplayPaymentReceiptOptionsRequest message = ParseRequest<DisplayPaymentReceiptOptionsRequest>(context);
        GetServer.CloverConnector.DisplayPaymentReceiptOptions(message.OrderID, message.PaymentID);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/OpenCashDrawer$")]
    public void OpenCashDrawer(HttpListenerContext context)
    {
        OpenCashDrawer message = ParseRequest<OpenCashDrawer>(context);
        GetServer.CloverConnector.OpenCashDrawer(message.Reason);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/DisplayOrder$")]
    public void ShowDisplayOrder(HttpListenerContext context)
    {
        com.clover.remote.order.DisplayOrder order = ParseRequest<com.clover.remote.order.DisplayOrder>(context);
        GetServer.CloverConnector.ShowDisplayOrder(order);
        this.SendTextResponse(context, "");
    }

    public void RemoveDisplayOrder(HttpListenerContext context)
    {
        com.clover.remote.order.DisplayOrder order = ParseRequest<com.clover.remote.order.DisplayOrder>(context);
        GetServer.CloverConnector.RemoveDisplayOrder(order);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/AcceptSignature$")]
    public void AcceptSignature(HttpListenerContext context)
    {
        VerifySignatureRequest message = ParseRequest<VerifySignatureRequest>(context);

        GetServer.CloverConnector.AcceptSignature(message);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/RejectSignature$")]
    public void RejectSignature(HttpListenerContext context)
    {
        VerifySignatureRequest message = ParseRequest<VerifySignatureRequest>(context);

        GetServer.CloverConnector.RejectSignature(message);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/InvokeInputOption$")]
    public void InvokeInputOption(HttpListenerContext context)
    {
        InputOption message = ParseRequest<InputOption>(context);

        GetServer.CloverConnector.InvokeInputOption(message);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/AcceptPayment$")]
    public void AcceptPayment(HttpListenerContext context)
    {
        Payment payment = ParseRequest<Payment>(context);

        GetServer.CloverConnector.AcceptPayment(payment);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/RejectPayment$")]
    public void RejectPayment(HttpListenerContext context)
    {
        RejectPaymentObject message = ParseRequest<RejectPaymentObject>(context);

        GetServer.CloverConnector.RejectPayment(message.Payment, message.Challenge);
        this.SendTextResponse(context, "");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/StartCustomActivity$")]
    public void StartCustomActivity(HttpListenerContext context)
    {
        CustomActivityRequest message = ParseRequest<CustomActivityRequest>(context);

        GetServer.CloverConnector.StartCustomActivity(message);
        this.SendTextResponse(context, "");
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

        T message = JsonUtils.deserialize<T>(x, new Newtonsoft.Json.JsonConverter[] { new StringEnumConverter() });

        return message;
    }
}