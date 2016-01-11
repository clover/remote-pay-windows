using com.clover.remotepay.transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.clover.remotepay.transport.remote
{
    class RemoteWebSocketCloverConfiguration : WebSocketCloverDeviceConfiguration
    {
        public RemoteWebSocketCloverConfiguration(string hostname, int port) : base(hostname, port)
        {
        }
    }
}
