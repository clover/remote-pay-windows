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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace com.clover.remotepay.transport
{
    public abstract class CloverTransport
    {
        List<CloverTransportObserver> observers = new List<CloverTransportObserver>();
        bool ready = false;
        int pingSleepSeconds = 0;
        protected int logLevel = 1000;

        public void SetLogLevel(int level)
        {
            logLevel = level;
        }

        /// <summary>
        /// Enable / disable ping
        /// </summary>
        /// <param name="pingSleepSeconds"></param>
        protected void EnablePinging(int pingSleepSeconds)
        {
            this.pingSleepSeconds = pingSleepSeconds;
        }

        /// <summary>
        /// Configured seconds between pings
        /// </summary>
        /// <returns></returns>
        protected int getPingSleepSeconds()
        {
            return pingSleepSeconds;
        }

        /// <summary>
        /// Log a message to the transport log, bacwards compatability, set level to below default (Minimal) but above 0 
        /// </summary>
        /// <param name="msg"></param>
        protected void TransportLog(string msg) => TransportLog(500, msg);

        /// <summary>
        /// Log a message to the transport log
        /// </summary>
        /// <param name="msg"></param>
        protected void TransportLog(int level, string msg)
        {
            if (level <= logLevel)
            {
                // Trim long messages if loglevel is on lower half of 0...9999 scale
                if (msg.Length > 5000 && logLevel < 5000)
                {
                    msg = msg.Substring(0, 5000) + "...";
                }
                Trace.WriteLine(msg);
            }
        }

        /// <summary>
        /// Device was connected, communication channel able to be established
        /// </summary>
        protected void onDeviceConnected()
        {
            observers.ForEach(x => x.onDeviceConnected(this));
        }

        /// <summary>
        /// Device connection is initialized and ready for communication
        /// </summary>
        protected virtual void onDeviceReady()
        {
            ready = true;
            observers.ForEach(x => x.onDeviceReady(this));
        }

        /// <summary>
        /// Device communication was lost: device was disconnected, powered off, or communication link otherwise broken
        /// </summary>
        protected virtual void onDeviceDisconnected()
        {
            ready = false;
            observers.ForEach(x => x.onDeviceDisconnected(this));
        }

        /// <summary>
        /// Standard .Net resources cleanup
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Error from device or SDK layer
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cause"></param>
        /// <param name="message"></param>
        protected void onDeviceError(int code, Exception cause, string message)
        {
            observers.ForEach(x => x.onDeviceError(code, cause, message));
        }

        /// <summary>
        /// Should be called by subclasses when a message is received.
        /// </summary>
        /// <param name="message"></param>
        protected virtual void onMessage(string message)
        {
            observers.ForEach(x => x.onMessage(message));
        }

        /// <summary>
        /// Add observer listener to this transport
        /// </summary>
        /// <param name="observer"></param>
        public void Subscribe(CloverTransportObserver observer)
        {
            if (observer != null && !observers.Contains(observer))
            {
                CloverTransport me = this;
                if (ready)
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    // what to do in the background thread
                    bw.DoWork += delegate
                    {
                        observer.onDeviceReady(me);
                    };
                    bw.RunWorkerAsync();
                }
                observers.Add(observer);
            }
        }

        /// <summary>
        /// Remove observer listener from this transport
        /// </summary>
        /// <param name="observer"></param>
        public void Unsubscribe(CloverTransportObserver observer)
        {
            if (observer != null && observers.Contains(observer))
            {
                observers.Remove(observer);
            }
        }

        /// <summary>
        /// Implement this to send info to the device
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract int sendMessage(string message);

        /// <summary>
        /// Remote message version, always 1 in current version
        /// </summary>
        /// <returns></returns>
        public virtual int getRemoteMessageVersion()
        {
            return 1;
        }

        public abstract string ShortTitle();

        public virtual string Title => "";
        public virtual string Summary => "";
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

        /// <summary>
        /// Message received from device
        /// </summary>
        /// <param name="message"></param>
        void onMessage(string message);

        /// <summary>
        /// Error from device or SDK layer
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cause"></param>
        /// <param name="message"></param>
        void onDeviceError(int code, Exception cause, string message);
    }
}
