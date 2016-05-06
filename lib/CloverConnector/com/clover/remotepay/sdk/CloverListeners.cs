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

using com.clover.remotepay.transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.clover.remotepay.sdk
{
    public interface CloverDeviceListener
    {
        void OnDeviceActivityStart(CloverDeviceEvent deviceEvent);
        void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent);
        void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent);
    }

    public interface CloverAuthListener
    {
        void OnPreAuthResponse(PreAuthResponse response);
        void OnAuthResponse(AuthResponse response);
        void OnAuthTipAdjustResponse(TipAdjustAuthResponse response);
        void OnAuthCaptureResponse(CaptureAuthResponse response);
    }
    public interface CloverSignatureListener
    {
        void OnSignatureVerifyRequest(SignatureVerifyRequest request);
    }
    public interface CloverCloseoutListener
    {
        void OnCloseoutResponse(CloseoutResponse response);
    }
    public interface CloverSaleListener
    {
        void OnSaleResponse(SaleResponse response);
    }
    public  interface CloverRefundListener
    {
        void OnManualRefundResponse(ManualRefundResponse response);
        void OnRefundPaymentResponse(RefundPaymentResponse response);
    }
    public interface CloverTipListener
    {
        void OnTipAdded(TipAddedMessage message);
    }
    public interface CloverVoidListener
    {
        void OnVoidTransactionResponse(VoidTransactionResponse response);
        void OnVoidPaymentResponse(VoidPaymentResponse response);
    }
    /// <summary>
    /// This listener is only for callbacks when a print receipt is requested
    /// </summary>
    public interface CloverReceiptListener
    {
        void OnDisplayReceiptOptionsResponse(DisplayReceiptOptionsResponse response);
    }
    public interface CloverErrorListener
    {
        void OnError(Exception e);
        void OnConfigError(ConfigErrorResponse ceResponse);
    }
    public interface CloverConnectionListener
    {
        void OnDeviceConnected();
        void OnDeviceReady();
        void OnDeviceDisconnected();
    }

    public interface CloverConnectorListener : CloverAuthListener, CloverCloseoutListener, CloverConnectionListener, CloverDeviceListener, CloverReceiptListener, CloverErrorListener, CloverRefundListener, CloverSaleListener, CloverSignatureListener, CloverVoidListener, CloverTipListener
    {
        void OnVaultCardResponse(VaultCardResponse response);
    }

    public class DefaultCloverConnectorListener : CloverConnectorListener
    {
        public void OnConfigError(ConfigErrorResponse response)
        {

        }

        public void OnVaultCardResponse(VaultCardResponse response)
        {

        }

        public void OnAuthCaptureResponse(CaptureAuthResponse response)
        {
            
        }

        public void OnAuthResponse(AuthResponse response)
        {
            
        }

        public void OnPreAuthResponse(PreAuthResponse response)
        {

        }

        public void OnAuthTipAdjustResponse(TipAdjustAuthResponse response)
        {
            
        }

        public void OnCloseoutResponse(CloseoutResponse response)
        {
            
        }

        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
            
        }

        public void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {
            
        }

        public void OnDeviceConnected()
        {
            
        }

        public void OnDeviceDisconnected()
        {
            
        }

        public void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {
            
        }

        public void OnDeviceReady()
        {
            
        }

        public void OnDisplayReceiptOptionsResponse(DisplayReceiptOptionsResponse response)
        {

        }

        public void OnError(Exception e)
        {
            
        }

        public void OnManualRefundResponse(ManualRefundResponse response)
        {
            
        }

        public void OnRefundPaymentResponse(RefundPaymentResponse response)
        {
            
        }

        public void OnSaleResponse(SaleResponse response)
        {
            
        }

        public void OnSignatureVerifyRequest(SignatureVerifyRequest request)
        {
            
        }

        public void OnVoidPaymentResponse(VoidPaymentResponse response)
        {
            
        }

        public void OnVoidTransactionResponse(VoidTransactionResponse response)
        {
            
        }

        public void OnTipAdded(TipAddedMessage message)
        {

        }
    }
}
