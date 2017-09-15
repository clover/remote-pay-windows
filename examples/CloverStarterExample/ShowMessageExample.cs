

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using com.clover.remotepay.sdk;
using com.clover.sdk.v3.payments;
using com.clover.remotepay.transport;

/**
 * This example class illustrates how to connect to a clover device and display a custom message.  The dialog
 * which occurs is:
 *    1) Create CloverConnector based upon the specified configuration
 *    2) Register the CloverConnectorListener with the CloverConnector
 *    3) Initialize the CloverConnector via initializeConnection() method
 *      a) If network connection configured, input the pairing code provided by the device callback
 *    4) Handle onDeviceReady() callback from device indicating connection was made
 *    5) Call the showMessage() API method to request the device to display the custom message
 */

namespace CloverStarterExample
{
    class ShowMessageExample
    {
        public static ICloverConnector cloverConnector;
        public static void Main(string[] args)
        {
            cloverConnector = new CloverConnector(SampleUtils.GetNetworkConfiguration());
            var ccl = new ExampleCloverConnectionListener(cloverConnector);
            cloverConnector.AddCloverConnectorListener(ccl);
            cloverConnector.InitializeConnection();

            while(!ccl.deviceReady)
            {
                Thread.Sleep(1000);
            }

            cloverConnector.ShowMessage("Welcome to Clover Connector!");
            Console.ReadKey();

        }
    }
}
