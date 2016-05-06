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
using System;
using System.IO;

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
        //Console.WriteLine("new Endpoints");
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

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ShowMessage$")]
    public void ShowMessage(HttpListenerContext context)
    {
        ShowMessage message = ParseRequest<ShowMessage>(context);

        this.SendTextResponse(context, ""+GetServer.CloverConnector.ShowMessage(message.Message));
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ResetDevice$")]
    public void ResetDevice(HttpListenerContext context)
    {
        this.SendTextResponse(context, "" + GetServer.CloverConnector.ResetDevice());
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ShowWelcomeScreen$")]
    public void ShowWelcomeScreen(HttpListenerContext context)
    {
        this.SendTextResponse(context, ""+GetServer.CloverConnector.ShowWelcomeScreen());
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/Cancel$")]
    public void Cancel(HttpListenerContext context)
    {
        this.SendTextResponse(context, "" + GetServer.CloverConnector.Cancel());
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ShowThankYouScreen$")]
    public void ShowThankYouScreen(HttpListenerContext context)
    {
        this.GetServer.CloverConnector.ShowThankYouScreen();
        this.SendTextResponse(context, "0");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/PrintText$")]
    public void PrintText(HttpListenerContext context)
    {
        PrintText message = ParseRequest<PrintText>(context);

        this.SendTextResponse(context, "" + GetServer.CloverConnector.PrintText(message.Messages));
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/Auth$")]
    public void Auth(HttpListenerContext context)
    {
        AuthRequest message = ParseRequest<AuthRequest>(context);

        this.SendTextResponse(context, "" + GetServer.CloverConnector.Auth(message));
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/PreAuth$")]
    public void PreAuth(HttpListenerContext context)
    {
        PreAuthRequest message = ParseRequest<PreAuthRequest>(context);

        this.SendTextResponse(context, "" + GetServer.CloverConnector.PreAuth(message));
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/Sale$")]
    public void Sale(HttpListenerContext context)
    {
        SaleRequest message = ParseRequest<SaleRequest>(context);

        this.SendTextResponse(context, "" + GetServer.CloverConnector.Sale(message));
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/VoidPayment$")]
    public void VoidPayment(HttpListenerContext context)
    {
        VoidPaymentRequest message = ParseRequest<VoidPaymentRequest>(context);
        this.SendTextResponse(context, ""+GetServer.CloverConnector.VoidPayment(message));
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/RefundPayment$")]
    public void RefundPayment(HttpListenerContext context)
    {
        RefundPaymentRequest message = ParseRequest<RefundPaymentRequest>(context);
        this.SendTextResponse(context, "" + GetServer.CloverConnector.RefundPayment(message));
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/ManualRefund$")]
    public void ManualRefund(HttpListenerContext context)
    {
        ManualRefundRequest message = ParseRequest<ManualRefundRequest>(context);
        this.SendTextResponse(context, ""+GetServer.CloverConnector.ManualRefund(message));
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/VaultCard$")]
    public void VaultCard(HttpListenerContext context)
    {
        VaultCard vaultCard = ParseRequest<VaultCard>(context);
        GetServer.CloverConnector.VaultCard(vaultCard.CardEntryMethod);
        this.SendTextResponse(context, "0");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/CaptureAuth$")]
    public void CaptureAuth(HttpListenerContext context)
    {
        CaptureAuthRequest message = ParseRequest<CaptureAuthRequest>(context);
        GetServer.CloverConnector.CaptureAuth(message);
        this.SendTextResponse(context, "0");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/TipAdjustAuth$")]
    public void TipAdjustAuth(HttpListenerContext context)
    {
        TipAdjustAuthRequest message = ParseRequest<TipAdjustAuthRequest>(context);
        this.SendTextResponse(context, "" + GetServer.CloverConnector.TipAdjustAuth(message));
    }

    public void VoidTransaction(VoidTransactionRequest request)
    {
        //throw new NotImplementedException();
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/Closeout")]
    public void Closeout(HttpListenerContext context)
    {
        CloseoutRequest message = ParseRequest<CloseoutRequest>(context);
        this.SendTextResponse(context, ""+GetServer.CloverConnector.Closeout(message));
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/PrintImage$")]
    public void PrintImage(HttpListenerContext context)
    {
        PrintImage message = ParseRequest<PrintImage>(context);
        this.SendTextResponse(context, "" + GetServer.CloverConnector.PrintImage(message.GetBitmap()));
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/DisplayPaymentReceiptOptions$")]
    public void DisplayPaymentReceiptOptions(HttpListenerContext context)
    {
        DisplayPaymentReceiptOptionsRequest message = ParseRequest<DisplayPaymentReceiptOptionsRequest>(context);
        GetServer.CloverConnector.DisplayPaymentReceiptOptions(message.OrderID, message.PaymentID);
        this.SendTextResponse(context, "0");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/DisplayRefundReceiptOptions$")]
    public void DisplayRefundReceiptOptions(HttpListenerContext context)
    {
        DisplayRefundReceiptOptionsRequest message = ParseRequest<DisplayRefundReceiptOptionsRequest>(context);
        GetServer.CloverConnector.DisplayRefundReceiptOptions(message.OrderID, message.RefundID);
        this.SendTextResponse(context, "0");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/DisplayCreditReceiptOptions$")]
    public void DisplayCreditReceiptOptions(HttpListenerContext context)
    {
        DisplayCreditReceiptOptionsRequest message = ParseRequest<DisplayCreditReceiptOptionsRequest>(context);
        GetServer.CloverConnector.DisplayCreditReceiptOptions(message.OrderID, message.CreditID);
        this.SendTextResponse(context, "0");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/OpenCashDrawer$")]
    public void OpenCashDrawer(HttpListenerContext context)
    {
        OpenCashDrawer message = ParseRequest<OpenCashDrawer>(context);
        GetServer.CloverConnector.OpenCashDrawer(message.Reason);
        this.SendTextResponse(context, "0");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/DisplayOrder$")]
    public void DisplayOrder(HttpListenerContext context)
    {
        com.clover.remote.order.DisplayOrder order = ParseRequest<com.clover.remote.order.DisplayOrder>(context);
        GetServer.CloverConnector.DisplayOrder(order);
        this.SendTextResponse(context, "0");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/DisplayOrderLineItemAdded$")]
    public void DisplayOrderLineItemAdded(HttpListenerContext context)
    {
        DisplayOrderLineItemAdded dolia = ParseRequest<DisplayOrderLineItemAdded>(context);
        GetServer.CloverConnector.DisplayOrderLineItemAdded(dolia.DisplayOrder, dolia.DisplayLineItem);
        this.SendTextResponse(context, "0");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/DisplayOrderLineItemRemoved$")]
    public void DisplayOrderLineItemRemoved(HttpListenerContext context)
    {
        DisplayOrderLineItemRemoved dolir = ParseRequest<DisplayOrderLineItemRemoved>(context);
        GetServer.CloverConnector.DisplayOrderLineItemRemoved(dolir.DisplayOrder, dolir.DisplayLineItem);
        this.SendTextResponse(context, "0");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/DisplayOrderDiscountAdded$")]
    public void DisplayOrderDiscountAdded(HttpListenerContext context)
    {
        DisplayOrderDiscountAdded doda = ParseRequest<DisplayOrderDiscountAdded>(context);
        GetServer.CloverConnector.DisplayOrderDiscountAdded(doda.DisplayOrder, doda.DisplayDiscount);
        this.SendTextResponse(context, "0");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/DisplayOrderDiscountRemoved$")]
    public void DisplayOrderDiscountRemoved(HttpListenerContext context)
    {
        DisplayOrderDiscountRemoved dodr = ParseRequest<DisplayOrderDiscountRemoved>(context);
        GetServer.CloverConnector.DisplayOrderDiscountRemoved(dodr.DisplayOrder, dodr.DisplayDiscount);
        this.SendTextResponse(context, "0");
    }

    public void DisplayOrderDelete(HttpListenerContext context)
    {
        com.clover.remote.order.DisplayOrder order = ParseRequest<com.clover.remote.order.DisplayOrder>(context);
        GetServer.CloverConnector.DisplayOrderDelete(order);
        this.SendTextResponse(context, "0");
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/AcceptSignature")]
    public void AcceptSignature(HttpListenerContext context)
    {
        SignatureVerifyRequest message = ParseRequest<SignatureVerifyRequest>(context);

        this.SendTextResponse(context, "" + GetServer.CloverConnector.AcceptSignature(message));
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/RejectSignature")]
    public void RejectSignature(HttpListenerContext context)
    {
        SignatureVerifyRequest message = ParseRequest<SignatureVerifyRequest>(context);

        this.SendTextResponse(context, "" + GetServer.CloverConnector.RejectSignature(message));
    }

    [RESTRoute(Method = Grapevine.HttpMethod.POST, PathInfo = @"^/Clover/InvokeInputOption")]
    public void InvokeInputOption(HttpListenerContext context)
    {
        InputOption message = ParseRequest<InputOption>(context);

        GetServer.CloverConnector.InvokeInputOption(message);
        this.SendTextResponse(context, "0");
    }

    public void AddCloverConnectorListener(CloverConnectorListener connectorListener)
    {
        GetServer.CloverConnector.AddCloverConnectorListener(connectorListener);
    }

    public void RemoveCloverConnectorListener(CloverConnectorListener connectorListener)
    {
        GetServer.CloverConnector.RemoveCloverConnectorListener(connectorListener);
    }

    public void GetMerchantInfo()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        //TODO: close the listener
    }

    private T ParseRequest<T>(HttpListenerContext context)
    {
        if (context.Request.ContentType != "application/json")
        {
            throw new HttpListenerException(500, "Unexpected Content Type. Expecting 'application/json'");
        }


        StreamReader stream = new StreamReader(context.Request.InputStream);
        string x = stream.ReadToEnd();  // added to view content of input stream

        T message = JsonUtils.deserialize<T>(x);

        return message;
    }
}