using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using LibUsbDotNet.WinUsb;

namespace com.clover.remotepay.transport.usb
{
    public class UsbDeviceWatcher : IDisposable
    {
        private readonly object Lock = new object();
        private CancellationTokenSource cancel = new CancellationTokenSource();
        private Task watch;
        private Dictionary<string, UsbRegistry> previous;

        public event EventHandler<UsbRegistryEventArgs> Added;
        public event EventHandler<UsbRegistryEventArgs> Removed;

        public void Start()
        {
            Stop();

            cancel = new CancellationTokenSource();
            watch = Task.Run(() => Watch(cancel.Token), cancel.Token);
        }

        public void Stop()
        {
            cancel?.Cancel();
            watch?.Wait();
        }

        private void Watch(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(100);

                lock (Lock)
                {
                    try
                    {
                        var current = UsbDevice
                            .AllDevices
                            .Cast<UsbRegistry>()
                            .ToDictionary(r => r is WinUsbRegistry win ? win.DeviceID : $"{r.Pid:X}:{r.Vid:X}");

                        if (previous == null)
                        {
                            current
                                .Select(r => new UsbRegistryEventArgs { UsbRegistry = r.Value })
                                .ToList()
                                .ForEach(e => Added?.Invoke(this, e));
                            previous = current;
                            continue;
                        }

                        previous
                            .Where(kvp => !current.ContainsKey(kvp.Key))
                            .Select(r => new UsbRegistryEventArgs { UsbRegistry = r.Value })
                            .ToList()
                            .ForEach(e => Removed?.Invoke(this, e));

                        current
                            .Where(kvp => !previous.ContainsKey(kvp.Key))
                            .Select(r => new UsbRegistryEventArgs { UsbRegistry = r.Value })
                            .ToList()
                            .ForEach(e => Added?.Invoke(this, e));

                        previous = current;
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine("Exception in UsbDeviceWatcher.Watch(): " + ex.Message);
                    }
                }
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
