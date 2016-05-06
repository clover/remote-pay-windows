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
using com.clover.remotepay.transport;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using System.ServiceModel.Web;

namespace CloverWindowsSDKREST
{
    public class CloverRESTConnectorListener : CloverConnectorListener
    {
        //public Fleck.IWebSocketConnection WebSocket { get; set; }
        public RestClient RestClient { get; set; }
        public string Status { get; internal set; }
        //public WebServiceHost Host { get; internal set; }

        public CloverRESTConnectorListener()
        {
            Status = "Disconnected";
        }

        public void OnAuthResponse(AuthResponse response)
        {
            Send("/AuthResponse", Serialize(response));
        }

        public void OnPreAuthResponse(PreAuthResponse response)
        {
            Send("/PreAuthResponse", Serialize(response));
        }

        public void OnAuthCaptureResponse(CaptureAuthResponse response)
        {
            Send("/CaptureAuthResponse", Serialize(response));
        }

        public void OnAuthTipAdjustResponse(TipAdjustAuthResponse response)
        {
            Send("/TipAdjustAuthResponse", Serialize(response));
        }

        public void OnCloseoutResponse(CloseoutResponse response)
        {
            Send("/CloseoutResponse", Serialize(response));
        }

        public void OnDeviceConnected()
        {
            Status = "Connected";
            Send("/DeviceConnected", null);
            //Console.WriteLine("Host: ${0}", Host.State);
        }

        public void OnDeviceDisconnected()
        {
            Status = "Disconnected";
            Send("/DeviceDisconnected", null);
            //Console.WriteLine("Host: ${0}", Host.State);
            //Console.WriteLine(Host.Description.Endpoints);
        }

        public void OnDeviceReady()
        {
            Status = "Ready";
            Send("/DeviceReady", null);
            //Console.WriteLine("Host: ${0}", Host.State);
        }

        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
            Send("/DeviceActivityEnd", Serialize(deviceEvent));
        }

        public void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {
            Send("/DeviceActivityStart", Serialize(deviceEvent));
        }

        public void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {
            Send("/DeviceError", Serialize(deviceErrorEvent));
        }

        public void OnDisplayReceiptOptionsResponse(DisplayReceiptOptionsResponse response)
        {
            Send("/DisplayreceiptOptionsResponse", Serialize(response));
        }

        public void OnError(Exception e)
        {
            Send("/Error", e.Message);
        }

        public void OnManualRefundResponse(ManualRefundResponse response)
        {
            Send("/ManualRefundResponse", Serialize(response));
        }

        public void OnRefundPaymentResponse(RefundPaymentResponse response)
        {
            Send("/RefundPaymentResponse", Serialize(response));
        }

        public void OnSaleResponse(SaleResponse response)
        {
            Send("/SaleResponse", Serialize(response));
        }

        public void OnSignatureVerifyRequest(SignatureVerifyRequest request)
        {
            Send("/SignatureVerifyRequest", Serialize(request));
        }

        public void OnVoidPaymentResponse(VoidPaymentResponse response)
        {
            Send("/VoidPaymentResponse", Serialize(response));
        }

        public void OnVoidTransactionResponse(VoidTransactionResponse response)
        {
            Send("/VoidTransactionResponse", Serialize(response));
        }

        public void OnTipAdded(com.clover.remotepay.transport.TipAddedMessage message)
        {
            Send("/TipAdded", Serialize(message));
        }

        public void OnVaultCardResponse(VaultCardResponse message)
        {
            Send("/VaultCardResponse", Serialize(message));
        }

        public void OnConfigError(ConfigErrorResponse ceResponse)
        {
            Send("/ConfigErrorResponse", Serialize(ceResponse));
        }

        public void ResendStatus()
        {
            switch (Status)
            {
                case "Ready":
                    {
                        this.OnDeviceReady();
                        break;
                    }
                case "Disconnected" :
                    {
                        OnDeviceDisconnected();
                        break;
                    }
                case "Connected":
                    {
                        OnDeviceConnected();
                        break;
                    }
            }

        }

        private object Serialize(object obj) 
        {
            return obj;
            /*
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();

            serializer.Serialize(ms, obj);
            ms.Position = 0;
            var sr = new StreamReader(ms);
            var myStr = sr.ReadToEnd();
            ms.Close();
            sr.Close();
            */

            //var myStr = JsonUtils.serialize(obj);
            //return myStr;
        }

        private void Send(string endpoint, object msg)
        {
            //Console.WriteLine(msg.ToString());
            if (RestClient != null)
            {
                RestRequest request = new RestRequest(endpoint, Method.POST);
                if (msg != null)
                {
                    //Console.WriteLine("sending: " + msg + " to " + endpoint);
                    request.AddJsonBody(msg);
                }
                else
                {
                    request.AddJsonBody("");
                }
                request.RequestFormat = DataFormat.Json;
                //IRestResponse response = RestClient.Execute(request);
                //Console.WriteLine("response: " + response.ResponseStatus);
                
                var asyncHandle = RestClient.ExecuteAsync(request, response =>
                {
                    if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
                    {
                        Console.WriteLine("Response on callback is code: " + response.StatusCode + " for " + endpoint);
                    }
#if DEBUG
                    Console.WriteLine(response.StatusCode + " for " + endpoint + " with payload: " + request.Parameters[0].ToString());
#endif
                    //Console.WriteLine("Send to Callback");
                    // don't care about a response
                });

            }
        }
    }
}
