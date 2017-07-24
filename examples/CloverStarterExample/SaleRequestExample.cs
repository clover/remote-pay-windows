using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.clover.remotepay.sdk;
using com.clover.sdk.v3.payments;
using com.clover.remotepay.transport;
using System.Runtime.CompilerServices;
using System.Threading;

/**
 * This example class illustrates how to connect to a clover device and processes a sale transaction.  The dialog
 * which occurs is:
 *    1) Create CloverConnector based upon the specified configuration
 *    2) Register the CloverConnectorListener with the CloverConnector
 *    3) Initialize the CloverConnector via initializeConnection() method
 *      a) If network connection configured, input the pairing code provided by the device callback
 *    4) Handle onDeviceReady() callback from device indicating connection was made
 *    5) Create a SaleRequest consisting of an amount and a UNIQUE ID and call the sale() API method to request the
 *    device to begin processing the sale.  Depending on merchant settings, card used in the transaction, device
 *    configuration, etc, the device may prompt for
 *      a) Payment (including possible Credit/Debit selection)
 *      b) Signature
 *      c) Notify merchant signature verification (NOTE:  This is handled by the onVerifySignatureRequest() callback
 *      which is configured in this handler to automatically accept signature verification
 *      d) Receipt choice
 *      NOTE:  During sale processing, the device may challenge the transaction (double charge, offline payment, etc).
 *      The registered listener MUST handle the onConfirmPaymentRequest() callback.
 *    6) After all device prompts have completed, the onSaleResponse() callback is made by the device which
 *    provide the status of the sale request
 */

namespace CloverStarterExample
{
    class SaleRequestExample
    {
        public static ICloverConnector cloverConnector;
        private static SaleRequest pendingSale;

        static void Main(string[] args)
        {
           

            cloverConnector = new CloverConnector(SampleUtils.GetNetworkConfiguration());
            var ccl = new ExampleCloverConnectionListener(cloverConnector);
            cloverConnector.AddCloverConnectorListener(ccl);
            cloverConnector.InitializeConnection();

            while(!ccl.deviceReady)
            {
                Thread.Sleep(1000);
            }
            
            var pendingSale = new SaleRequest();
            pendingSale.ExternalId = ExternalIDUtil.GenerateRandomString(13);
            pendingSale.Amount = 1000;
            pendingSale.AutoAcceptSignature = true;
            pendingSale.AutoAcceptPaymentConfirmations = true;
            pendingSale.DisableDuplicateChecking = true;
            cloverConnector.Sale(pendingSale);
            
            while(!ccl.saleDone)
            {
                Thread.Sleep(1000);
            }
            Console.ReadKey();
        }
        
     
    }

    
}
