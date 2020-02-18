using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibUsbDotNet.Main;

namespace com.clover.remotepay.transport.usb
{
    public partial class UsbCloverTransport
    {
        [Obsolete("Deprecated. This will be removed in a future version.")]
        public static Dictionary<string, List<UsbDeviceFinder>> VendorToFinder = new Dictionary<string, List<UsbDeviceFinder>>();

        [Obsolete("Deprecated. This will be removed in a future version.")]
        public static List<UsbDeviceFinder> MerchantUsbFinders = new List<UsbDeviceFinder>();

        [Obsolete("Deprecated. This will be removed in a future version.")]
        public static List<UsbDeviceFinder> CustomerUsbFinders = new List<UsbDeviceFinder>();

        [Obsolete("Deprecated. This will be removed in a future version. Use the default constructor instead.")]
        public UsbCloverTransport(List<USBDevice> merchantDevices, List<USBDevice> customerDevices)
            : this()
        {
        }

        [Obsolete("Deprecated. This will be removed in a future version. Use the default constructor instead.")]
        public UsbCloverTransport(string deviceId)
            : this()
        {
        }

        [Obsolete("Deprecated. This will be removed in a future version. Use the default constructor instead.")]
        public UsbCloverTransport(string deviceId, int pingSleepSeconds)
            : this()
        {
        }

        [Obsolete("Deprecated. This will be removed in a future version.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Obsolete")]
        protected static int getMaxDataTransferSize() => throw new NotImplementedException();

        [Obsolete("Deprecated. This will be removed in a future version.")]
        public bool IsUsbDeviceConnected(string pid, string vid, string mi) => throw new NotImplementedException();

        [Obsolete("Deprecated. This will be removed in a future version.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Obsolete")]
        public int sendMessageSync(string message) => throw new NotImplementedException();

        [Obsolete("Deprecated. This will be removed in a future version.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Obsolete")]
        public void writePacket(BinaryWriter outDataBuffer) => throw new NotImplementedException();

        [Obsolete("Deprecated. This will be removed in a future version.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Obsolete")]
        public void startListeningForMessages() => throw new NotImplementedException();

        [Obsolete("Deprecated. This will be removed in a future version.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Obsolete")]
        public bool isConnected() => throw new NotImplementedException();

        [Obsolete("Deprecated. This will be removed in a future version.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Obsolete")]
        public void disconnect() => throw new NotImplementedException();
    }

    public partial class UsbCloverTransport : CloverTransport
    {
        private CloverUsbDevice CloverDevice { get; } = new CloverUsbDevice();
        private BlockingQueue<string> MessageQueue { get; } = new BlockingQueue<string>();
        private CancellationTokenSource SendMessagesCancel { get; } = new CancellationTokenSource();
        private Task SendMessagesTask { get; set; }

        public UsbCloverTransport()
        {
            CloverDevice.Connected += CloverDevice_Connected;
            CloverDevice.Disconnected += CloverDevice_Disconnected;
            CloverDevice.Message += CloverDevice_Message;
            CloverDevice.Error += CloverDevice_Error;

            SendMessagesTask = Task.Run(() => SendMessages(SendMessagesCancel.Token), SendMessagesCancel.Token);
        }

        public override void Dispose()
        {
            try
            {
                CloverDevice?.Dispose();
            }
            catch { }            
            
            SendMessagesCancel.Cancel();
            lock (MessageQueue)
            {
                Monitor.PulseAll(MessageQueue);
            }
            SendMessagesTask.Wait();
        }

        public override int sendMessage(string message)
        {
            if (CloverDevice?.Device?.IsOpen == true)
            {
                TransportLog($"In sendMessage() just before the Enqueue: MessageQueue.Count={MessageQueue.Count}, message={message}");
                MessageQueue.Enqueue(message);
                return 1;
            }
            return 0;
        }

        public override string ShortTitle() => "USB";

        public override string Title => "USB PD";

        public override string Summary => "USB";

        private void SendMessages(CancellationToken cancel)
        {
            TransportLog("Starting SendMessages");

            while (!cancel.IsCancellationRequested)
            {
                try
                {
                    while (MessageQueue.Count > 0)
                    {
                        string message = MessageQueue.Peek();
                        if (message != null)
                        {
                            CloverDevice.Write(message);
                        }
                        MessageQueue.DequeueIf(message);
                        TransportLog($"In SendMessages() just after the Dequeue: MessageQueue.Count={MessageQueue.Count}, message={message}");
                    }

                    lock (MessageQueue)
                    {
                        // NOTE: Wait for a pulse from either a new message being enqueued or the transport being disposed.
                        Monitor.Wait(MessageQueue, 1000);
                    }
                }
                catch (Exception ex)
                {
                    TransportLog($"Error occurred in SendMessages(): {ex.Message}");
                    TransportLog(ex.StackTrace);
                }
            }

            TransportLog("Terminating SendMessages");
        }

        private void CloverDevice_Connected(object sender, EventArgs e)
        {
            onDeviceConnected();
            onDeviceReady();
        }

        private void CloverDevice_Disconnected(object sender, EventArgs e)
        {
            onDeviceDisconnected();
        }

        private void CloverDevice_Error(object sender, ErrorEventArgs e)
        {
            const int DeviceIoControl = -16379; // CloverDeviceErrorEvent.DeviceIoControl
            onDeviceError(DeviceIoControl, e.Error, e.Error.Message);
        }

        private void CloverDevice_Message(object sender, MessageEventArgs e)
        {
            onMessage(e.Message);
        }

        /// <summary>
        /// Log a message to the transport log, backwards compatability, set level to below default (Minimal) but above 0.
        /// </summary>
        /// <param name="msg"></param>
        protected void TransportLog(string msg) => TransportLog(500, msg);
    }
}
