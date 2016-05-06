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

using System.Collections;
using com.clover.remotepay.transport;
using System;

namespace com.clover.remotepay.sdk
{

    public class CloverDeviceListenerList : ArrayList, CloverDeviceListener
    {
        public static CloverDeviceListenerList operator +(CloverDeviceListenerList connectorList, CloverDeviceListener listener)
        {
            if (!connectorList.Contains(listener))
            {
                connectorList.Add(listener);
            }
            return connectorList;
        }

        public static CloverDeviceListenerList operator -(CloverDeviceListenerList connectorList, CloverDeviceListener listener)
        {
            connectorList.Remove(listener);
            return connectorList;
        }

        public void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {
            foreach (CloverDeviceListener deviceListener in this)
            {
                deviceListener.OnDeviceActivityStart(deviceEvent);
            }
        }

        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
            foreach (CloverDeviceListener deviceListener in this)
            {
                deviceListener.OnDeviceActivityEnd(deviceEvent);
            }
        }

        public void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {
            foreach (CloverDeviceListener deviceListener in this)
            {
                deviceListener.OnDeviceError(deviceErrorEvent);
            }
        }
    }
    public class CloverSignatureListenerList : ArrayList
    {
        public static CloverSignatureListenerList operator +(CloverSignatureListenerList list, CloverSignatureListener listener)
        {
            if (!list.Contains(listener))
            {
                list.Add(listener);
            }
            return list;
        }
        public static CloverSignatureListenerList operator -(CloverSignatureListenerList list, CloverSignatureListener listener)
        {
            list.Remove(listener);
            return list;
        }
        public void NotifyOnSignatureVerifyRequest(SignatureVerifyRequest request)
        {
            foreach (CloverSignatureListener sigVerifyListener in this)
            {
                sigVerifyListener.OnSignatureVerifyRequest(request);
            }
        }
    }
    public class CloverAuthListenerList : ArrayList
    {
        public static CloverAuthListenerList operator +(CloverAuthListenerList list, CloverAuthListener listener)
        {
            if (!list.Contains(listener))
            {
                list.Add(listener);
            }
            return list;
        }
        public static CloverAuthListenerList operator -(CloverAuthListenerList list, CloverAuthListener listener)
        {
            list.Remove(listener);
            return list;
        }
        public void NotifyOnAuthResponse(AuthResponse response)
        {
            foreach (CloverAuthListener authListener in this)
            {
                authListener.OnAuthResponse(response);
            }
        }
        public void NotifyOnTipAdjustResponse(TipAdjustAuthResponse response)
        {
            foreach (CloverAuthListener tipAdjustAuthListener in this)
            {
                tipAdjustAuthListener.OnAuthTipAdjustResponse(response);
            }
        }
        public void NotifyOnCaptureResponse(CaptureAuthResponse response)
        {
            foreach (CloverAuthListener authListener in this)
            {
                authListener.OnAuthCaptureResponse(response);
            }
        }

        public void NotifyOnTipAdjustAuthResponse(TipAdjustAuthResponse response)
        {
            foreach (CloverAuthListener authListener in this)
            {
                authListener.OnAuthTipAdjustResponse(response);
            }
        }
    }

    public class CloverCloseoutListenerList : ArrayList
    {
        public static CloverCloseoutListenerList operator +(CloverCloseoutListenerList list, CloverCloseoutListener listener)
        {
            if (!list.Contains(listener))
            {
                list.Add(listener);
            }
            return list;
        }
        public static CloverCloseoutListenerList operator -(CloverCloseoutListenerList list, CloverCloseoutListener listener)
        {
            list.Remove(listener);
            return list;
        }

        internal void NotifyCloseout(CloseoutResponse response)
        {
            foreach (CloverCloseoutListener closeoutListener in this)
            {
                closeoutListener.OnCloseoutResponse(response);
            }
        }
    }

    public class CloverDisplayListenerList : ArrayList
    {
        public static CloverDisplayListenerList operator +(CloverDisplayListenerList list, CloverReceiptListener listener)
        {
            if (!list.Contains(listener))
            {
                list.Add(listener);
            }
            return list;
        }
        public static CloverDisplayListenerList operator -(CloverDisplayListenerList list, CloverReceiptListener listener)
        {
            list.Remove(listener);
            return list;
        }
    }

    public class CloverSaleListenerList : ArrayList
    {
        public static CloverSaleListenerList operator +(CloverSaleListenerList list, CloverSaleListener listener)
        {
            if(!list.Contains(listener))
            {
                list.Add(listener);
            }
            return list;
        }
        public static CloverSaleListenerList operator -(CloverSaleListenerList list, CloverSaleListener listener)
        {
            list.Remove(listener);
            return list;
        }
        public void NotifyOnSaleResponse(SaleResponse response)
        {
            foreach (CloverSaleListener saleListener in this)
            {
                saleListener.OnSaleResponse(response);
            }
        }
    }
    public class CloverVoidListenerList : ArrayList
    {
        public static CloverVoidListenerList operator +(CloverVoidListenerList list, CloverVoidListener listener)
        {
            if (!list.Contains(listener))
            {
                list.Add(listener);
            }
            return list;
        }
        public static CloverVoidListenerList operator -(CloverVoidListenerList list, CloverVoidListener listener)
        {
            list.Remove(listener);
            return list;
        }
        public void NotifyOnVoidPaymentResponse(VoidPaymentResponse response)
        {
            foreach (CloverVoidListener listener in this)
            {
                listener.OnVoidPaymentResponse(response);
            }
        }
        public void NotifyOnVoidTransactionResponse(VoidTransactionResponse response)
        {
            foreach (CloverVoidListener voidListener in this)
            {
                voidListener.OnVoidTransactionResponse(response);
            }
        }
    }
    public class CloverRefundListenerList : ArrayList
    {
        public static CloverRefundListenerList operator +(CloverRefundListenerList list, CloverRefundListener listener)
        {
            if (!list.Contains(listener))
            {
                list.Add(listener);
            }
            return list;
        }
        public static CloverRefundListenerList operator -(CloverRefundListenerList list, CloverRefundListener listener)
        {
            list.Remove(listener);
            return list;
        }
        public void NotifyOnManualRefundResponse(ManualRefundResponse response)
        {
            foreach (CloverRefundListener listener in this)
            {
                listener.OnManualRefundResponse(response);
            }
        }
        public void NotifyOnRefundPaymentResponse(RefundPaymentResponse response)
        {
            for(int i=0; i<this.Count; i++)
            {
                CloverRefundListener listener = this[i] as CloverRefundListener;
                listener.OnRefundPaymentResponse(response);
            }
            // Why doesn't this work?
            /*foreach (CloverRefundListener refundPaymentListener in this)
            {
                Console.WriteLine(refundPaymentListener);
                refundPaymentListener.OnRefundPaymentResponse(response);
            }*/
        }
    }
    public class CloverConnectionListenerList : ArrayList
    {
        public static CloverConnectionListenerList operator +(CloverConnectionListenerList list, CloverConnectionListener listener)
        {
            if (!list.Contains(listener))
            {
                list.Add(listener);
            }
            return list;
        }
        public static CloverConnectionListenerList operator -(CloverConnectionListenerList list, CloverConnectionListener listener)
        {
            list.Remove(listener);
            return list;
        }
        public void NotifyOnConnect()
        {
            foreach (CloverConnectionListener listener in this)
            {
                listener.OnDeviceConnected();
            }
        }
        public void NotifyOnReady()
        {
            foreach(CloverConnectionListener listener in this)
            {
                listener.OnDeviceReady();
            }
        }
        public void NotifyOnDisconnect()
        {
            foreach (CloverConnectionListener connectionListener in this)
            {
                connectionListener.OnDeviceDisconnected();
            }
        }

    }

    public class CloverTipListenerList : ArrayList
    {
        public static CloverTipListenerList operator +(CloverTipListenerList list, CloverTipListener listener)
        {
            if (!list.Contains(listener))
            {
                list.Add(listener);
            }
            return list;
        }
        public static CloverTipListenerList operator -(CloverTipListenerList list, CloverTipListener listener)
        {
            list.Remove(listener);
            return list;
        }
        public void NotifyOnTipAdded(TipAddedMessage message)
        {
            foreach (CloverTipListener tipListener in this)
            {
                tipListener.OnTipAdded(message);
            }
        }
    }
}
