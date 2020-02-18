using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.clover.remotepay.transport.usb
{
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
