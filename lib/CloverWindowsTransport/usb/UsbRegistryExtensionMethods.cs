using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibUsbDotNet.Main;
using LibUsbDotNet.WinUsb;

namespace com.clover.remotepay.transport.usb
{
    internal static class UsbRegistryExtensionMethods
    {
        public static int GetInterfaceId(this UsbRegistry registry)
        {
            if (registry is WinUsbRegistry win)
            {
                return win.InterfaceID;
            }

            return 0;
        }
    }
}
