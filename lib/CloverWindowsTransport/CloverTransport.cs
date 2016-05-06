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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Diagnostics;

namespace com.clover.remotepay.transport
{
    public abstract class CloverTransport
    {
        private List<CloverTransportObserver> observers = new List<CloverTransportObserver>();
        bool ready = false;
        bool enableLogging = false;
        int pingSleepSeconds = 0;

        protected void EnableLogging()
        {
            this.enableLogging = true;
        }

        protected bool LoggingEnabled()
        {
            return this.enableLogging;
        }

        protected void EnablePinging(int pingSleepSeconds)
        {
            this.pingSleepSeconds = pingSleepSeconds;
        }

        protected int getPingSleepSeconds()
        {
            return this.pingSleepSeconds;
        }
        protected void TransportLog(string msg)
        {
            if (enableLogging)
            {
                Trace.WriteLine(msg);
            }
        }

        protected void onDeviceConnected()
        {
            observers.ForEach(x => x.onDeviceConnected(this));
        }

        protected virtual void onDeviceReady()
        {
            ready = true;
            observers.ForEach(x => x.onDeviceReady(this));
        }

        protected virtual void onDeviceDisconnected()
        {
            ready = false;
            observers.ForEach(x => x.onDeviceDisconnected(this));
        }

        public abstract void Dispose();
        protected void onDeviceError(int code, string message)
        {
            observers.ForEach(x => x.onDeviceError(code, message));
        }

        /// <summary>
        /// Should be called by subclasses when a message is received.
        /// </summary>
        /// <param name="message"></param>
        protected virtual void onMessage(string message)
        {
            observers.ForEach(x => x.onMessage(message));
        }

        public void Subscribe(CloverTransportObserver observer)
        {
            CloverTransport me = this;
            if (ready)
            {
                BackgroundWorker bw = new BackgroundWorker();
                // what to do in the background thread
                bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    BackgroundWorker b = o as BackgroundWorker;
                    observer.onDeviceReady(me);
                });
                bw.RunWorkerAsync();
            }
            observers.Add(observer);
        }

        public void Unsubscribe(CloverTransportObserver observer)
        {
            observers.Remove(observer);
        }

        // Implement this to send info
        public abstract int sendMessage(string message);
    }

    public interface CloverTransportObserver
    {
        /// <summary>
        /// Device is there but not yet ready for use
        /// </summary>
        void onDeviceConnected(CloverTransport transport);

        /// <summary>
        /// Device is there and ready for use
        /// </summary>
        void onDeviceReady(CloverTransport transport);

        /// <summary>
        /// Device is not there anymore
        /// </summary>
        /// <param name="transport"></param>
        void onDeviceDisconnected(CloverTransport transport);

        void onMessage(string message);
        void onDeviceError(int code, string message);
    }
}
