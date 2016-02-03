using com.clover.remotepay.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace com.clover.remotepay.transport.remote
{
    [ServiceContract]
    interface ICloverCallbackService
    {
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        BodyStyle = WebMessageBodyStyle.Bare,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/AuthResponse")]
        void AuthResponse(AuthResponse authResponse);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/SaleResponse")]
        void SaleResponse(SaleResponse saleResponse);


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/RefundPaymentResponse")]
        void RefundPaymentResponse(RefundPaymentResponse response);


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/TipAdjustAuthResponse")]
        void TipAdjustAuthResponse(TipAdjustAuthResponse response);


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/CaptureAuthResponse")]
        void CaptureAuthResponse(CaptureAuthResponse response);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/ManualRefundResponse")]
        void ManualRefundResponse(ManualRefundResponse response);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/VaultCardResponse")]
        void VaultCardResponse(VaultCardResponse response);


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/VoidPaymentResponse")]
        void VoidPaymentResponse(VoidPaymentResponse response);


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/SignatureVerifyRequest")]
        void SignatureVerifyRequest(SignatureVerifyRequest request);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/DeviceActivityStart")]
        void OnDeviceActivityStart(CloverDeviceEvent deviceEvent);


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/DeviceActivityEnd")]
        void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent);


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/TipAdded")]
        void OnTipAdded(TipAddedEvent msg);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/DeviceConnected")]
        void OnDeviceConnected();


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/DeviceDisconnected")]
        void OnDeviceDisconnected();

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/DeviceReady")]
        void OnDeviceReady();

    }

    public class TipAddedEvent
    {
        public long tipAmount { get; set; }
    }
}
