using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using com.clover.remotepay.sdk;
using com.clover.sdk.v3;
using com.clover.sdk.v3.customers;
using com.clover.sdk.v3.payments;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloverExamplePOS
{
    partial class LoyaltyApiPage : UserControl
    {
        #region Loyalty Sample Constants
        // CFP constants
        public static string CUSTOMER_INFO_EXTRA = "com.clover.extra.CUSTOMER_INFO";
        public static string DISPLAY_ORDER_EXTRA = "com.clover.extra.DISPLAY_ORDER";

        public static string ACTION_SESSION_DATA = "com.clover.cfp.SESSION_DATA_ACTION";
        public static string EXTRA_PAYLOAD = "com.clover.remote.terminal.remotecontrol.extra.EXTRA_PAYLOAD";
        public static string V1_MESSAGE_TO_ACTIVITY = "com.clover.remote-terminal.remotecontrol.action.V1_MESSAGE_TO_ACTIVITY";
        public static string V1_MESSAGE_FROM_ACTIVITY = "com.clover.remote-terminal.remotecontrol.action.V1_MESSAGE_FROM_ACTIVITY";
        public static string CATEGORY_CUSTOM_ACTIVITY = "com.clover.cfp.ACTIVITY";

        // Loyalty Sample names and constants
        public const string LOYALTY_SAMPLE_ACTIVITY = "com.clover.remote.clover.loyalty.CloverLoyaltyCustomActivity"; // This is the activity's action name, not the activity name
        #endregion

        #region Example Customer Data
        // A few customer phone numbers and ids to show auto-responding with customer info when a customer data is received
        // fields are Name, Phone, Welcome message, Customer ID, and Loyalty Points
        Dictionary<string, string> CustomerDatabase = new Dictionary<string, string>
        {
            { "phone:8675309", "Jenny, 8675309, Welcome back Jenny!, 1, 30" },
            { "phone:6345789", "Wilson Pickett, 6345789, Welcome back Wicked Pickett!, 2, 251" },
            { "com.loyalty.accountnumber:411", "Operator, 0, Hello Operator, 411, 0" },
            { "com.loyalty.accountnumber:42", "Douglas Adams, N/A, Welcome Mr. Adams, 42, 42" }
        };
        #endregion

        public CloverExamplePosData Data { get; set; }

        public LoyaltyApiPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Show a log of the loyalty api message exchange in the UI
        /// Handle invoking UI thread from background threads CloverConnector as appropriate
        /// </summary>
        /// <param name="message"></param>
        private void Log(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action<string>)Log, message);
            }
            else
            {
                LoyaltyLog.Text += message + Environment.NewLine;
                Debug.WriteLine("LoyaltyAPI: " + message);
            }
        }

        /// <summary>
        /// Clear the log display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearLoyaltyLogButton_Click(object sender, EventArgs e)
        {
            LoyaltyLog.Clear();
        }

        /// <summary>
        /// Start the LOYALTY_SAMPLE_ACTIVITY provided in the Clover CFP Loyalty API Samples
        /// * If the sample custom activity isn't installed, CloverConnector API will send back an ActivityResponse message like "Custom activity cancelled by transition to remote-pay activity" as it fails back to RemotePay
        /// * If the sample custom activity is already running, CloverConnector API will send back an ActivityResponse message indicating the old activity is closing and replaced by the new activity, even though it's the same activity.
        /// 
        /// com.clover.remote_clover_loyalty.CloverLoyaltyCustomActivity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowUiButton_Click(object sender, EventArgs e)
        {
            Log("\n \u2022 Starting Loyalty Sample Activity");

            Data.CloverConnector.StartCustomActivity(new CustomActivityRequest() { Action = LOYALTY_SAMPLE_ACTIVITY, Payload = "", NonBlocking = true });
        }

        /// <summary>
        /// Register a new set of Loyalty data subscription configurations with the Loyalty API
        /// 
        /// The actual loyalty data type strings will depend on the loyalty data providers
        /// 
        /// The Loyalty sample activity provides EMAIL and PHONE data as a sample, as well as
        /// the built in VAS (Apple Vas and Google Smart Tap) data provided by Clover.
        /// 
        /// There is also a CLEAR "type" send by the Loyalty sample activity to signal that
        /// the activity wishes to clear the associated customer data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            Log("\n \u2022 Registering new Loyalty Config");

            List<DataProviderConfig> config = new List<DataProviderConfig>();

            // Identify customer by Phone number, Barcode, Account Number, VAS data as appropriate
            if (PhoneCheck.Checked)
            {
                config.Add(new DataProviderConfig(LoyaltyDataTypes.PHONE_TYPE));
            }
            if (BarcodeCheck.Checked)
            {
                config.Add(new DataProviderConfig(LoyaltyDataTypes.BARCODE_TYPE));
            }
            if (CustomAccountCheck.Checked)
            {
                // Custom types are fine, a Loyalty Provider subscribes a data type name (here's one in the sample Activity), and CloverConnector API consumers subscribe for that data by name
                config.Add(new DataProviderConfig("com.loyalty.AccountNumber"));
            }
            if (VasCheck.Checked)
            {
                LoyaltyDataTypes.VasConfig vasConfig = new LoyaltyDataTypes.VasConfig
                {
                    ProviderPackage = "com.clover.loyalty.CLE",
                    ProtocolId = VasProtocol.ST.ToString(),
                    SupportedServices = new List<string> { VasDataTypeType.ALL.ToString() },
                    PushUrl = "st.clover.com",
                    PushTitle = "Test ST Push Url"
                };

                config.Add(new DataProviderConfig(LoyaltyDataTypes.VAS_TYPE, vasConfig.Serialize()));
            }

            // "special" Clear Customer signal provided from sample loyalty activity apk on the device
            config.Add(new DataProviderConfig(LoyaltyDataTypes.CLEAR_TYPE));

            Data.CloverConnector.RegisterForCustomerProvidedData(new RegisterForCustomerProvidedDataRequest() { Configurations = config.ToArray() });
        }

        /// <summary>
        /// Clear the current customer information
        /// Reset the Loyalty API's Customer Information state by sending a null customer info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearCustomerButton_Click(object sender, EventArgs e)
        {
            Log("\n \u2022 Clearing Customer Info");

            // Send a null or empty SetCustomerInfoRequest object to clear the customer info state
            Data.CloverConnector.SetCustomerInfo(null);
        }

        /// <summary>
        /// Set the current CustomerInfo state of the Loyalty API
        ///
        /// The Point of Sale can set the current customer information at any time. The Loyalty API keeps that customer
        /// information data until it is cleared or changed, across any number of transactions and settings. The Loyalty
        /// API only ever has one CustomerInfo object set.
        ///
        /// The expected use is the Point of Sale will receive a CUSTOMER_PROVIDED_DATA_MESSAGE,
        /// attempt to look up the customer however it prefers,
        /// and then call CloverConnector.SetCustomerInfo() with the data that was found.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetCustomerButton_Click(object sender, EventArgs e)
        {
            Log("\n \u2022 Set Customer Info");

            // Get a customer first name and welcome message from the UI, or use a default
            string name = CustomerNameEdit.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                name = "John";
            }

            SendCustomerInfoMessage(name, "5551212", $"Welcome back, {name}", Guid.NewGuid().ToString(), "52");
        }

        /// <summary>
        /// Call the Loyalty API CloverConnector.SetCustomerInfo with a simple CustomerInfo object assembled from parameters
        /// </summary>
        private void SendCustomerInfoMessage(string name, string phone, string welcome, string id, string points)
        {
            // create a customer object with a first name and sample home phone number - in real life this would be looked up in a customer data source
            Customer customer = new Customer
            {
                firstName = name,
                phoneNumbers = new List<PhoneNumber>() { new PhoneNumber { id = "home", phoneNumber = phone } }
            };

            // create the customer info request object with some canned offers
            SetCustomerInfoRequest request = new SetCustomerInfoRequest
            {
                customer = customer,
                displayString = welcome,
                externalId = id,
                extras =
                {
                    ["POINTS"] = points,
                    ["OFFERS"] = SampleOffer.OfferList(
                        "5_PCT", "5% off this order",
                        "MUG", "Free Branded Coffee Mug",
                        "FRIEND", "Giftcard for Sharing")
                }
            };

            // Custom Activity data, not part of the Clover API, can be anything serializable to json and understood by a custom activity
            Data.CloverConnector.SetCustomerInfo(request);
        }

        /// <summary>
        /// Close the Loyalty Sample Activity
        /// Send the message to the Sample Loyalty activity that causes it to close itself and return to RemotePay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndUiButton_Click(object sender, EventArgs e)
        {
            Log("\n \u2022 Ending Loyalty Sample Activity");

            // If the custom activity supports it, send a close command
            // otherwise just send a CloverConnector command like ShowWelcome or DisplayOrder to bring it to the front

            Data.CloverConnector.ShowWelcomeScreen();
        }

        /// <summary>
        /// The LOYALTY_SAMPLE_ACTIVITY takes a custom Offer object list
        /// </summary>
        private class SampleOffer
        {
            public string id { get; set; }
            public string label { get; set; }

            public SampleOffer() { }
            public SampleOffer(string id, string label) { this.id = id; this.label = label; }

            /// <summary>
            /// The LOYALTY_SAMPLE_ACTIVITY needs a json string of Offers, here's an easy way to build it
            /// Each actual Loyalty activity on the device will have its own custom data requirements
            /// </summary>
            /// <returns></returns>
            public static string OfferList(params string[] values)
            {
                List<SampleOffer> offers = new List<SampleOffer>();
                string prevId = null;
                foreach (string value in values)
                {
                    if (prevId == null)
                    {
                        prevId = value;
                    }
                    else
                    {
                        offers.Add(new SampleOffer(prevId, value));
                        prevId = null;
                    }
                }
                if (prevId != null)
                {
                    offers.Add(new SampleOffer(prevId, ""));
                }
                return JsonConvert.SerializeObject(offers);
            }
        }

        #region Communication from the Clover Device: Loyalty API and Custom Activity Messages

        /// <summary>
        /// When the Loyalty API on the device receives messages and forwards it to the Clover SDK
        /// ICloverEventListener calls the OnCustomerProvidedData message interface
        /// and the Clover Example POS routes that message here for handling
        /// </summary>
        public void OnCustomerProvidedData(CustomerProvidedDataEvent response)
        {
            Log($"\n \u2022 Customer Provided Data provided by Loyalty API");
            Log($"      registered config type: {response.config.type}");
            Log($"      data payload: {response.data.Replace("\n", "\n          ")}");


            // As an example, here's a lookup of a few phone or account numbers that auto-respond with customer information
            string key = $"{response.config.type.ToLower()}:{response.data}";
            if (CustomerDatabase.ContainsKey(key))
            {
                // quick and dirty split a 5 field CSV string into customer info to pretend like a database lookup
                string[] fields = CustomerDatabase[key].Split(',');

                Log("");
                Log($"      found customer: {fields[0]}: {fields[2]}");
                Log($"      auto-sending customer data via SendCustomerInfo");
                SendCustomerInfoMessage(fields[0], fields[1], fields[2], fields[3], fields[4]);
            }
            else
            {
                // If the customer wasn't found, maybe it's a new customer and needs onboarding? Or maybe it was a typo, or a new phone for an existing customer.
            }
        }

        /// <summary>
        /// The LOYALTY_SAMPLE_ACTIVITY sends messages using the 
        /// CloverConnectorListener.OnMessageFromActivity() Custom Activity messaging system
        /// and the Clover Example POS routes that message here for handling
        /// </summary>
        public void OnMessageFromActivity(MessageFromActivity message)
        {
            Log($"\n \u2022 Message From Activity received from the loyalty sample activity");
            Log($"      activity: {message.Action}");
            Log($"      payload: {message.Payload.Replace("\n", "\n          ")}");

            // Route message as appropriate - sample messages don't have an action, so we'll detect by payload variables
            if (message.Payload.Contains("customerUUID") && message.Payload.Contains("offerId"))
            {
                if (JsonConvert.DeserializeObject(message.Payload) is JObject offer)
                {
                    Log($"\n   Offer '{offer["offerId"]}' accepted by customer '{offer["customerUUID"]}'");
                }
            }
            else if (message.Payload.Contains("LoyaltyRegistrationConfigs") || message.Payload.Contains("CustomerInfo"))
            {
                // The custom activity sends the registered configs and customer info upon request
            }
            else
            {
                Log("  ** unrecognized message ** ");
            }
        }

        #endregion

        #region Headless custom activity messages
        // Messages for testing the headless version of the Loyalty Custom Activity we'll use in automated testing

        private void SendCustomerProvidedDataButton_Click(object sender, EventArgs e)
        {
            Log("\n \u2022 Request: Send Customer Provided Data");

            // Send a null or empty SetCustomerInfoRequest object to clear the customer info state
            Data.CloverConnector.SendMessageToActivity(new MessageToActivity() { Action = LOYALTY_SAMPLE_ACTIVITY, Payload = "{\"command\":\"SendCustomerProvidedData\", \"config\": {\"type\":\"PHONE\"}, \"data\":\"5551212\"}" });
        }

        private void SendRegistrationConfigsButton_Click(object sender, EventArgs e)
        {
            Log("\n \u2022 Request: Send Registration Configs");

            // Send a null or empty SetCustomerInfoRequest object to clear the customer info state
            Data.CloverConnector.SendMessageToActivity(new MessageToActivity() { Action = LOYALTY_SAMPLE_ACTIVITY, Payload = "{\"command\":\"SendRegistrationConfigs\"}" });
        }

        private void SendCustomerInfoButton_Click(object sender, EventArgs e)
        {
            Log("\n \u2022 Request: Send Customer Info");

            // Send a null or empty SetCustomerInfoRequest object to clear the customer info state
            Data.CloverConnector.SendMessageToActivity(new MessageToActivity() { Action = LOYALTY_SAMPLE_ACTIVITY, Payload = "{\"command\":\"SendCustomerInfo\"}" });
        }
        #endregion
    }
}

