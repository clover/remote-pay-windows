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
using com.clover.remotepay.sdk;
using com.clover.remotepay.sdk.service.client;
using com.clover.remotepay.transport;

/// <summary>
/// Contains a set of classes to simplify using the Windows WebSocket service by providing
/// beans that can be serialized to invoke methods on the service.
/// </summary>
namespace com.clover.sdk.remote.websocket
{
    public class ServicePayloadConstants
    {
        public const string PROP_METHOD = "method";
        public const string PROP_PAYLOAD = "payload";
        public const string PROP_ID = "id";
        public const string PROP_TYPE = "type";
        public const string PROP_packageName = "packageName";
    }

    public enum WebSocketMethod
    {
        Status,
        Sale,
        Auth,
        PreAuth,
        Cancel,
        Break,
        CapturePreAuth,
        TipAdjustAuth,
        VoidPayment,
        RefundPayment,
        ManualRefund,
        Closeout,
        DisplayPaymentReceiptOptions,
        PrintText,
        PrintImage,
        PrintImageFromURL,
        OpenCashDrawer,
        ShowMessage,
        ShowWelcomeScreen,
        ShowThankYouScreen,
        ShowDisplayOrder,
        LineItemAddedToDisplayOrder,
        LineItemRemovedFromDisplayOrder,
        DiscountAddedToDisplayOrder,
        DiscountRemovedFromDisplayOrder,
        InvokeInputOption,
        AcceptSignature,
        RejectSignature,
        DeviceActivityStart,
        DeviceActivityEnd,
        DeviceDisconnected,
        DeviceConnected,
        DeviceReady,
        DeviceError,
        Error,
        SaleResponse,
        AuthResponse,
        PreAuthResponse,
        CapturePreAuthResponse,
        CloseoutResponse,
        TipAdjustAuthResponse,
        RefundPaymentResponse,
        ManualRefundResponse,
        VoidPaymentResponse,
        TipAdded,
        VerifySignatureRequest,
        VaultCard,
        VaultCardResponse,
        LogMessage
    }

    public class WebSocketMessage<T>
    {
        public WebSocketMessage() { }
        public WebSocketMessage(WebSocketMethod _method)
        {
            method = _method;
        }
        public string id { get; set; }
        public WebSocketMethod method { get; set; }
        public T payload { get; set; }
    }

    public class StatusRequestMessage : WebSocketMessage<object>
    {
        public StatusRequestMessage() : base(WebSocketMethod.Status)
        {
        }
    }
    public class SaleRequestMessage : WebSocketMessage<SaleRequest>
    {
        public SaleRequestMessage() : base(WebSocketMethod.Sale)
        {
            
        }
    }
    public class AuthRequestMessage : WebSocketMessage<AuthRequest>
    {
        public AuthRequestMessage() : base(WebSocketMethod.Auth)
        {
        }
    }
    public class PreAuthRequestMessage : WebSocketMessage<PreAuthRequest>
    {
        public PreAuthRequestMessage() : base(WebSocketMethod.PreAuth)
        {
        }
    }
    public class CancelRequestMessage : WebSocketMessage<object>
    {
        public CancelRequestMessage() : base(WebSocketMethod.Cancel)
        {
        }
    }
    public class BreakRequestMessage : WebSocketMessage<object>
    {
        public BreakRequestMessage() : base(WebSocketMethod.Break)
        {
        }
    }
    public class CapturePreAuthRequestMessage : WebSocketMessage<CapturePreAuthRequest>
    {
        public CapturePreAuthRequestMessage() : base(WebSocketMethod.CapturePreAuth)
        {
        }
    }
    public class TipAdjustAuthRequestMessage : WebSocketMessage<TipAdjustAuthRequest>
    {
        public TipAdjustAuthRequestMessage() : base(WebSocketMethod.TipAdjustAuth)
        {
        }
    }
    public class VoidPaymentRequestMessage : WebSocketMessage<VoidPaymentRequest>
    {
        public VoidPaymentRequestMessage() : base(WebSocketMethod.VoidPayment)
        {
        }
    }
    public class RefundPaymentRequestMessage : WebSocketMessage<RefundPaymentRequest>
    {
        public RefundPaymentRequestMessage() : base(WebSocketMethod.RefundPayment)
        {
        }
    }
    public class ManualRefundRequestMessage : WebSocketMessage<ManualRefundRequest>
    {
        public ManualRefundRequestMessage() : base(WebSocketMethod.ManualRefund)
        {
        }
    }
    public class CloseoutRequestMessage : WebSocketMessage<CloseoutRequest>
    {
        public CloseoutRequestMessage() : base(WebSocketMethod.Closeout)
        {
        }
    }
    public class DisplayPaymentReceiptOptionsRequestMessage : WebSocketMessage<object>
    {
        public DisplayPaymentReceiptOptionsRequestMessage() : base(WebSocketMethod.DisplayPaymentReceiptOptions)
        {
        }
    }
    public class PrintTextRequestMessage : WebSocketMessage<PrintText>
    {
        public PrintTextRequestMessage() : base(WebSocketMethod.PrintText)
        {
        }
    }
    public class PrintImageRequestMessage : WebSocketMessage<PrintImage>
    {
        public PrintImageRequestMessage() : base(WebSocketMethod.PrintImage)
        {
        }
    }
    public class PrintImageFromURLRequestMessage : WebSocketMessage<PrintImage>
    {
        public PrintImageFromURLRequestMessage() : base(WebSocketMethod.PrintImageFromURL)
        {
        }
    }
    public class OpenCashDrawerRequestMessage : WebSocketMessage<OpenCashDrawer>
    {
        public OpenCashDrawerRequestMessage() : base(WebSocketMethod.OpenCashDrawer)
        {
        }
    }
    public class ShowMessageRequestMessage : WebSocketMessage<ShowMessage>
    {
        public ShowMessageRequestMessage() : base(WebSocketMethod.ShowMessage)
        {
        }
    }
    public class ShowWelcomeScreenRequestMessage : WebSocketMessage<object>
    {
        public ShowWelcomeScreenRequestMessage() : base(WebSocketMethod.ShowWelcomeScreen)
        {
        }
    }
    public class ShowThankYouScreenRequestMessage : WebSocketMessage<object>
    {
        public ShowThankYouScreenRequestMessage() : base(WebSocketMethod.ShowThankYouScreen)
        {
        }
    }
    public class DisplayOrderRequestMessage : WebSocketMessage<DisplayOrder>
    {
        public DisplayOrderRequestMessage() : base(WebSocketMethod.ShowDisplayOrder)
        {
        }
    }
    public class LineItemAddedToDisplayOrderRequestMessage : WebSocketMessage<LineItemAddedToDisplayOrder>
    {
        public LineItemAddedToDisplayOrderRequestMessage() : base(WebSocketMethod.LineItemAddedToDisplayOrder)
        {
        }
    }
    public class LineItemRemovedFromDisplayOrderRequestMessage : WebSocketMessage<LineItemRemovedFromDisplayOrder>
    {
        public LineItemRemovedFromDisplayOrderRequestMessage() : base(WebSocketMethod.LineItemRemovedFromDisplayOrder)
        {
        }
    }
    public class DiscountAddedToDisplayOrderRequestMessage : WebSocketMessage<DiscountAddedToDisplayOrder>
    {
        public DiscountAddedToDisplayOrderRequestMessage() : base(WebSocketMethod.DiscountAddedToDisplayOrder)
        {
        }
    }
    public class DiscountRemovedFromDisplayOrderRequestMessage : WebSocketMessage<DiscountRemovedFromDisplayOrder>
    {
        public DiscountRemovedFromDisplayOrderRequestMessage() : base(WebSocketMethod.DiscountRemovedFromDisplayOrder)
        {
        }
    }
    public class InvokeInputOptionRequestMessage : WebSocketMessage<InputOption>
    {
        public InvokeInputOptionRequestMessage() : base(WebSocketMethod.InvokeInputOption)
        {
        }
    }
    public class AcceptSignatureRequestMessage : WebSocketMessage<VerifySignatureRequest>
    {
        public AcceptSignatureRequestMessage() : base(WebSocketMethod.AcceptSignature)
        {
        }
    }
    public class RejectSignatureRequestMessage : WebSocketMessage<VerifySignatureRequest>
    {
        public RejectSignatureRequestMessage() : base(WebSocketMethod.RejectSignature)
        {
        }
    }
    public class VaultCardRequestMessage : WebSocketMessage<VaultCardMessage>
    {
        public VaultCardRequestMessage() : base(WebSocketMethod.VaultCard)
        {
        }
    }
    public class LogMessageRequestMessage : WebSocketMessage<LogMessage>
    {
        public LogMessageRequestMessage() : base(WebSocketMethod.LogMessage)
        {
        }
    }



    // callback methods

    public class OnVaultCardResponseMessage : WebSocketMessage<VaultCardResponse>
    {
        public OnVaultCardResponseMessage() : base(WebSocketMethod.VaultCardResponse)
        {
        }
    }

    public class OnTipAddedMessage : WebSocketMessage<TipAddedMessage>
    {
        public OnTipAddedMessage() : base(WebSocketMethod.TipAdded)
        {
        }
    }
    public class OnDeviceDisconnectedMessage : WebSocketMessage<object>
    {
        public OnDeviceDisconnectedMessage() : base(WebSocketMethod.DeviceDisconnected)
        {
        }
    }
    public class OnDeviceConnectedMessage : WebSocketMessage<object>
    {
        public OnDeviceConnectedMessage() : base(WebSocketMethod.DeviceConnected)
        {
        }
    }
    public class OnDeviceReadyMessage : WebSocketMessage<object>
    {
        public OnDeviceReadyMessage() : base(WebSocketMethod.DeviceReady)
        {
        }
    }
    public class OnDeviceErrorMessage : WebSocketMessage<remotepay.sdk.CloverDeviceErrorEvent>
    {
        public OnDeviceErrorMessage() : base(WebSocketMethod.DeviceError)
        {
        }
    }
    public class OnDeviceActivityStartMessage : WebSocketMessage<CloverDeviceEvent>
    {
        public OnDeviceActivityStartMessage() : base(WebSocketMethod.DeviceActivityStart)
        {
        }
    }
    public class OnDeviceActivityEndMessage : WebSocketMessage<CloverDeviceEvent>
    {
        public OnDeviceActivityEndMessage() : base(WebSocketMethod.DeviceActivityEnd)
        {
        }
    }
    public class OnSaleResponseMessage : WebSocketMessage<SaleResponse>
    {
        public OnSaleResponseMessage() : base(WebSocketMethod.SaleResponse)
        {
        }
    }
    public class OnPreAuthResponseMessage : WebSocketMessage<PreAuthResponse>
    {
        public OnPreAuthResponseMessage() : base(WebSocketMethod.PreAuthResponse)
        {
        }
    }
    public class OnAuthResponseMessage : WebSocketMessage<AuthResponse>
    {
        public OnAuthResponseMessage() : base(WebSocketMethod.AuthResponse)
        {
        }
    }
    public class OnCloseoutResponseMessage : WebSocketMessage<CloseoutResponse>
    {
        public OnCloseoutResponseMessage() : base(WebSocketMethod.CloseoutResponse)
        {
        }
    }
    public class OnRefundPaymentResponseMessage : WebSocketMessage<RefundPaymentResponse>
    {
        public OnRefundPaymentResponseMessage() : base(WebSocketMethod.RefundPaymentResponse)
        {
        }
    }
    public class OnManualRefundResponseMessage : WebSocketMessage<ManualRefundResponse>
    {
        public OnManualRefundResponseMessage() : base(WebSocketMethod.ManualRefundResponse)
        {
        }
    }
    public class OnVoidPaymentResponseMessage : WebSocketMessage<VoidPaymentResponse>
    {
        public OnVoidPaymentResponseMessage() : base(WebSocketMethod.VoidPaymentResponse)
        {
        }
    }
    
    public class OnCapturePreAuthResponseMessage : WebSocketMessage<CapturePreAuthResponse>
    {
        public OnCapturePreAuthResponseMessage() : base(WebSocketMethod.CapturePreAuthResponse)
        {
        }
    }

    public class OnTipAdjustAuthResponseMessage : WebSocketMessage<TipAdjustAuthResponse>
    {
        public OnTipAdjustAuthResponseMessage() : base(WebSocketMethod.TipAdjustAuthResponse)
        {
        }
    }
    public class OnVerifySignatureRequestMessage : WebSocketMessage<VerifySignatureRequest>
    {
        public OnVerifySignatureRequestMessage() : base(WebSocketMethod.VerifySignatureRequest)
        {
        }
    }
}
