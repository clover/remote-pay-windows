using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;

namespace CloverStarterExample
{
    public class SampleUtils
    {

        // private static readonly String APP_ID = "com.cloverconnector.windows.simple.sample:1.3.1";
        // private static readonly String POS_NAME = "Clover Simple Sample Windows";
        // private static readonly String DEVICE_NAME = "Clover Device";

        private SampleUtils() { }

        public static String GetNextId()
        {
            return ExternalIDUtil.GenerateRandomString(13);
        }

        public static CloverDeviceConfiguration GetNetworkConfiguration()
        {

            PairingDeviceConfiguration.OnPairingCodeHandler pairingCodeHandler = new PairingDeviceConfiguration.OnPairingCodeHandler(OnPairingCode);
            PairingDeviceConfiguration.OnPairingSuccessHandler pairingsuccessHandler = new PairingDeviceConfiguration.OnPairingSuccessHandler(OnPairingSuccess);

            // ws vs wss must match Network Pay Display setting. wss requires Clover root CA
            var endpoint = "ws://192.168.0.6:12345/remote_pay";

            // Network Pay Display must be installed and configured to allow
            // insecure connections for the above configuration
            // Create a WebSocketDeviceConfiguration with appropriate
            // remoteApplicationID (see above), POS name and POS serial number
            var websocketConfiguration = new WebSocketCloverDeviceConfiguration(endpoint,                                             //endpoint
                                                                                "com.cloverconnector.windows.simple.sample:1.3.1",    //remoteApplicationID
                                                                                false,                                                //enableLogging
                                                                                1,                                                    //pingSleepSeconds
                                                                                "Aisle 2",                                            //posName
                                                                                "ABC123",                                             //serialNumber
                                                                                null,                                                 //pairingAuthToken
                                                                                OnPairingCode,                                        //pairingCodeHandler
                                                                                OnPairingSuccess,                                     //pairingSuccessHandler
                                                                                OnPairingState);                                      //pairingStateHandler
            return websocketConfiguration;
        }
        public static void OnPairingCode(string pairingCode)
        {
            // present pairingCode to the user to be entered on the Clover Device
            Console.WriteLine("Enter this pairing code: " + pairingCode);
        }
        public static void OnPairingSuccess(string pairingAuthToken)
        {
            // the authToken can be used in the WebSocketDeviceConfiguration
            // for future connections to prevent the need for entering
            // the pairing code on screen for subsequent connections
        }
        public static void OnPairingState(string state, string message)
        {
            // Device may send multiple messages while pairing depending on configuration
            // for example: AUTHENTICATING if a manager pin needs to be entered to authenticate pairing
        }

    }
}
