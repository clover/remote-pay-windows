using System;
using System.IO;
using System.Collections.Generic;

using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using System.Threading;

namespace CloverExampleCLI
{
    public class CloverExampleCLI
    {
        const string VERSION_NUMBER = "1.1.1.0";
        const string APPLICATION_ID = "com.clover.example.windows.cli" + ":" + VERSION_NUMBER;
        const string SERIALNUMBER = "CLIPOS";
        const string POSNAME = "Clover Windows Example CLI POS";
        const string DEFAULT_ENDPOINT = "wss://192.168.1.14:12345/remote_pay";
        const bool enableLogging = false;

        private ICloverConnector cloverConnector;
        private string[] cmdArgs;
        private bool useUsb;
        private static bool inRead = false;

        private CloverDeviceConfiguration selectedConfig;
        WebSocketCloverDeviceConfiguration WebSocketConfig = new WebSocketCloverDeviceConfiguration(DEFAULT_ENDPOINT, APPLICATION_ID, enableLogging, 1, POSNAME, SERIALNUMBER, Properties.Settings.Default.pairingAuthToken, null, null, null); // set the 3 delegates in the ctor
        CloverDeviceConfiguration USBConfig = new USBCloverDeviceConfiguration("__deviceID__", APPLICATION_ID, enableLogging, 1);

        public CloverExampleCLI(string[] args)
        {

            cmdArgs = args;
            if (cmdArgs.Length > 0)
            {
                if (((ICollection<string>)args).Contains("-usb"))
                {
                    useUsb = true;
                }
            }
        }

        public void websocketInitialize()
        {
            string ip = Properties.Settings.Default.lastWSEndpoint;
            WebSocketConfig.pairingAuthToken = Properties.Settings.Default.pairingAuthToken;

            string uri = ip;
            string[] tokens;

            uri = readLine("Enter device endpoint (default " +DEFAULT_ENDPOINT + ") ");
            // if we don't get any input
            if (uri == null || uri.Trim().Length == 0)
            {
                uri = DEFAULT_ENDPOINT;
            }
            tokens = uri.Split(':');


            Properties.Settings.Default.lastWSEndpoint = uri;
            Properties.Settings.Default.Save();

            selectedConfig = new WebSocketCloverDeviceConfiguration(uri, APPLICATION_ID, true, 1, "Clover Windows Example CLI", "WinCLI", Properties.Settings.Default.pairingAuthToken, OnPairingCode, OnPairingSuccess, OnPairingState);
        }

        public void run()
        {

            System.Console.WriteLine("");
            System.Console.WriteLine(POSNAME);
            // create the device configuration. Currently, only the USBCloverDeviceConfiguration is supported
            if (useUsb)
            {
                selectedConfig = USBConfig;
            }
            else
            {
                websocketInitialize();
            }

            // construct a CloverConnector
            cloverConnector = CloverConnectorFactory.createICloverConnector(selectedConfig);

            // add an ICloverConnectorListener...this one only listens for connect, disconnect and ready calls
            CLIConnectorListener connListener = new CLIConnectorListener(cloverConnector, this);
            cloverConnector.AddCloverConnectorListener(connListener);

            // initialize the connection
            cloverConnector.InitializeConnection();

            // don't want this thread to exit or the app will exit
            var exitEvent = new ManualResetEvent(false);

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                exitEvent.Set();
            };

            exitEvent.WaitOne();
        }

        public void OnPairingCode(string pairingCode)
        {
            Console.WriteLine("Enter this code on the Clover Device: " + pairingCode);
        }

        public void OnPairingSuccess(string pairingAuthToken)
        {
            // Save off the auth token for reuse
            Properties.Settings.Default.pairingAuthToken = pairingAuthToken;
            Properties.Settings.Default.Save();
        }

        public void OnPairingState(string state, string message)
        {
            Console.WriteLine($"{message}\r\nPairing State: {state}");
        }

        /**
         * utility method for printing a message and read via Console.Readline to a String
         * @param message
         * @return
         */
        public static String readLine(String message)
        {
            System.Console.WriteLine("");
            System.Console.Write(message);
            String line = null;
            try
            {
                line = Console.ReadLine();
            }
            catch (IOException ioe)
            {
                Console.Error.WriteLine("Error reading: " + ioe.Message);
            }
            return line;
        }

        private void promptForAction()
        {
            System.Console.WriteLine("");
            System.Console.WriteLine("Enter option, or just Enter to exit.");
            System.Console.WriteLine("1. Sale");
            System.Console.WriteLine("2. Auth");
            System.Console.WriteLine("3. PreAuth");
            System.Console.WriteLine("4. Vault Card");
            System.Console.WriteLine("5. Manual Refund");
            System.Console.WriteLine("6. Read Card Data");
            System.Console.WriteLine("7. Show Welcome Screen");
            System.Console.WriteLine("8. Show Thank You Screen");
            System.Console.WriteLine("9. Display Message");
            System.Console.WriteLine("10. Reset Device");
            System.Console.WriteLine("99. Exit");

            String option = null;
            if (!inRead)
            {
                try
                {
                    option = readLine("Select Option: ");
                    int sel = Int32.Parse(option);

                    switch (sel)
                    {
                        case 1:
                            doSale();
                            break;
                        case 2:
                            doAuth();
                            break;
                        case 3:
                            doPreAuth();
                            break;
                        case 4:
                            doVaultCard();
                            break;
                        case 5:
                            doManualRefund();
                            break;
                        case 6:
                            doReadCardData();
                            break;
                        case 7:
                            doShowWelcomeScreen();
                            break;
                        case 8:
                            doShowThankYouScreen();
                            break;
                        case 9:
                            doDisplayMessage();
                            break;
                        case 10:
                            doResetDevice();
                            break;
                        default:
                            System.Environment.Exit(0);
                            break;
                    }
                }
                catch (FormatException)
                {
                    // Some invalid or empty value, just exit
                    System.Environment.Exit(0);
                }
            }
        }


        private void doSale()
        {
            long? amount = cmdArgs.Length > 1 ? long.Parse(cmdArgs[1]) : (long?)null;
            string externalId = cmdArgs.Length > 2 ? cmdArgs[2] : "" + new Random().Next().ToString();
            // long amount = 12345;
            SaleExecutor executor = new SaleExecutor(cloverConnector, amount, externalId);
            executor.run();
            promptForAction();
        }

        private void doAuth()
        {
            // parse arguments IF they were passed in, otherwise create a AuthExecutor which, when done, prompts for a new action
            long? amount = cmdArgs.Length > 1 ? long.Parse(cmdArgs[1]) : (long?)null;
            string externalId = new Random().Next().ToString();
            AuthExecutor auth = new AuthExecutor(cloverConnector, amount, externalId);
            auth.run();
            promptForAction();
        }

        private void doPreAuth()
        {
            // parse arguments IF they were passed in, otherwise create a PreAuthExecutor which, when done, prompts for a new action
            long? amount = cmdArgs.Length > 1 ? long.Parse(cmdArgs[1]) : (long?)null;
            string externalId = new Random().Next().ToString();
            PreAuthExecutor preAuth = new PreAuthExecutor(cloverConnector, amount, externalId);
            preAuth.run();
            promptForAction();
        }

        private void doVaultCard()
        {
            VaultCardExecutor vaultCard = new VaultCardExecutor(cloverConnector);
            vaultCard.run();
            promptForAction();

        }

        private void doManualRefund()
        {
            long? amount = cmdArgs.Length > 1 ? long.Parse(cmdArgs[1]) : (long?)null;
            string externalId = new Random().Next().ToString();
            ManualRefundExecutor manualRefund = new ManualRefundExecutor(cloverConnector, amount, externalId);
            manualRefund.run();
            promptForAction();
        }

        private void doReadCardData()
        {
            ReadCardDataExecutor readCardData = new ReadCardDataExecutor(cloverConnector);
            readCardData.run();
            promptForAction();
        }


        private void doShowWelcomeScreen()
        {
            cloverConnector.ShowWelcomeScreen();
            promptForAction();
        }

        private void doShowThankYouScreen()
        {
            cloverConnector.ShowThankYouScreen();
            promptForAction();
        }

        private void doDisplayMessage()
        {
            string msg = readLine("Enter Message: ");
            cloverConnector.ShowMessage(msg);
            promptForAction();
        }

        private void doResetDevice()
        {
            cloverConnector.ResetDevice();
            promptForAction();
        }

        protected class CLIConnectorListener : DefaultCloverConnectorListener
        {

            CloverExampleCLI outer;

            public CLIConnectorListener(ICloverConnector cloverConnector, CloverExampleCLI outer) : base(cloverConnector)
            {
                this.outer = outer;
            }

            public override void OnConfirmPaymentRequest(ConfirmPaymentRequest request) { }

            public override void OnDeviceDisconnected()
            {
                System.Console.WriteLine("    > Device Disconnected!");
            }

            public override void OnDeviceConnected()
            {
                System.Console.WriteLine("    > Device Connected...");
            }

            /*
             * Once the device is connected and ready, we can prompt
             */
            public override void OnDeviceReady(MerchantInfo merchantInfo)
            {
                outer.promptForAction();
            }

        }
    }
}

