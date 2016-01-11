using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.clover.remotepay.transport.remote
{
    class RemoteRESTCloverConfiguration : CloverDeviceConfiguration
    {
        private string hostname;
        private int port;

        public RemoteRESTCloverConfiguration(string host, int port)
        {
            this.hostname = host;
            this.port = port;
        }

        public string getCloverDeviceTypeName()
        {
 	        throw new NotImplementedException();
        }

        public string getMessagePackageName()
        {
 	        throw new NotImplementedException();
        }

        public string getName()
        {
            return "REST Service Mini";
        }

        public CloverTransport getCloverTransport()
        {
 	        throw new NotImplementedException();
        }
    }
}
