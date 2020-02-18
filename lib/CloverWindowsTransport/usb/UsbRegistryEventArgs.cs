using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibUsbDotNet.Main;

namespace com.clover.remotepay.transport.usb
{
    public class UsbRegistryEventArgs : EventArgs
    {
        public UsbRegistry UsbRegistry { get; set; }
    }
}
