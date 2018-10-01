// Copyright (C) 2018 Clover Network, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
//
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using com.clover.remotepay.transport.remote;
using com.clover.remotepay.transport;
using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Windows.Forms;

namespace CloverExamplePOS
{
    
    public partial class StartupForm : OverlayForm
    {
        private static readonly string REG_KEY = "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\CloverSDK";
        CloverDeviceConfiguration selectedConfig;

        const String APPLICATION_ID = "com.clover.CloverExamplePOS:1.4.3";

        CloverDeviceConfiguration USBConfig = new USBCloverDeviceConfiguration("__deviceID__", APPLICATION_ID, false, 1);
        CloverDeviceConfiguration RestConfig = new RemoteRESTCloverConfiguration("localhost", 8181, APPLICATION_ID, false, 1);
        CloverDeviceConfiguration RemoteWebSocketConfig = new RemoteWebSocketCloverConfiguration("ws://localhost:8889", APPLICATION_ID);
        
        WebSocketCloverDeviceConfiguration WebSocketConfig = new WebSocketCloverDeviceConfiguration("wss://192.168.1.14:12345/remote_pay", APPLICATION_ID, false, 1, "Clover Windows Example POS", "POS-3", Properties.Settings.Default.pairingAuthToken, null, null); // set the 2 delegates in the ctor

        PairingDeviceConfiguration.OnPairingCodeHandler pairingCodeHandler = null;
        PairingDeviceConfiguration.OnPairingSuccessHandler pairingSuccessHandler = null;


        public StartupForm(Form tocover, PairingDeviceConfiguration.OnPairingCodeHandler pairingHandler, PairingDeviceConfiguration.OnPairingSuccessHandler successHandler) : base(tocover)
        {
            this.pairingCodeHandler = pairingHandler;
            this.pairingSuccessHandler = successHandler;
            WebSocketConfig.OnPairingCode = pairingHandler;
            WebSocketConfig.OnPairingSuccess = successHandler;
            Properties.Settings.Default.Reload();
            WebSocketConfig.endpoint = Properties.Settings.Default.lastWSEndpoint;
            if(WebSocketConfig.endpoint == null || "".Equals(WebSocketConfig.endpoint))
            {
                WebSocketConfig.endpoint = "wss://192.168.1.15:12345/remote_pay"; // just a default...
            }
            WebSocketConfig.pairingAuthToken = Properties.Settings.Default.pairingAuthToken;

            InitializeComponent();
        }

        private void StartupDialog_Load(object sender, EventArgs e)
        {
            // populate the combo box

            Object IsSDK = Registry.GetValue(REG_KEY, "IsSDK", 0);
            Object IsREST = Registry.GetValue(REG_KEY, "IsREST", 0);
            Object IsWS = Registry.GetValue(REG_KEY, "IsWebSocket", 0);

            bool isSdkInstalled = false;
            bool isRESTInstalled = false;
            bool isWSInstalled = false;

            // IsSDK
            try
            {
                isSdkInstalled = ((int)IsSDK == 1);
            }
            catch (InvalidCastException)
            {
                isSdkInstalled = false;
            }
            catch (NullReferenceException)
            {
                isSdkInstalled = false;
            }
            // isREST
            try
            {
                isRESTInstalled = ((int)IsREST == 1);
            }
            catch (InvalidCastException)
            {
                isRESTInstalled = false;
            }
            catch (NullReferenceException)
            {
                isRESTInstalled = false;
            }
            // isWS
            try
            {
                isWSInstalled = ((int)IsWS == 1);
            }
            catch (InvalidCastException)
            {
                isWSInstalled = false;
            }
            catch (NullReferenceException)
            {
                isWSInstalled = false;
            }

            var dataSource = new List<ConfigWrapper>();
            if (isSdkInstalled || (!isRESTInstalled && !isWSInstalled))
            {
                dataSource.Add(new ConfigWrapper("Clover Connector USB", USBConfig));
            }
            if (isRESTInstalled)
            {
                dataSource.Add(new ConfigWrapper("Clover Connector REST service", RestConfig));
            }
            if (isWSInstalled)
            {
                dataSource.Add(new ConfigWrapper("Clover Connector Web Socket service", RemoteWebSocketConfig));
            }
            
            dataSource.Add(new ConfigWrapper("Network Pay Display", WebSocketConfig));

            ConnectionType.DataSource = dataSource;
            ConnectionType.DisplayMember = "Description";
            ConnectionType.ValueMember = "Config";

            ConnectionType.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        protected class ConfigWrapper
        {
            public string Description { get; set; }
            public CloverDeviceConfiguration Config { get; set; }

            public ConfigWrapper(string description, CloverDeviceConfiguration config)
            {
                Description = description;
                Config = config;
            }

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            selectedConfig = ((CloverDeviceConfiguration)ConnectionType.SelectedValue);
            if (selectedConfig is RemoteWebSocketCloverConfiguration)
            {
                InitLocalWebSocket();
            }
            else if (selectedConfig is WebSocketCloverDeviceConfiguration)
            {
                InitWebSocket();
            }
            else
            {
                ((CloverExamplePOSForm)this.Owner).InitializeConnector(selectedConfig);
                this.Close();
            }
            //new CloverExamplePOSForm(selectedConfig).Show();
        }


        private void InitLocalWebSocket()
        {
            InputForm iform = new InputForm(this);
            iform.Title = "WebSocket Port Configuration";
            iform.Label = "Enter Endpoint (e.g. ws://localhost:8889)";
            iform.Value = ""+((RemoteWebSocketCloverConfiguration)RemoteWebSocketConfig).endpoint;
            iform.FormClosed += WSRemoteForm_Closed;
            iform.Show();
        }

        private void WSRemoteForm_Closed(object sender, EventArgs e)
        {
            if (((InputForm)sender).Status == DialogResult.OK)
            {
                string val = ((InputForm)sender).Value;
                selectedConfig = new RemoteWebSocketCloverConfiguration(val, APPLICATION_ID);

                ((CloverExamplePOSForm)this.Owner).InitializeConnector(selectedConfig);
                this.Close();
            }
        }

        private void InitWebSocket()
        {
            InputForm iform = new InputForm(this);
            iform.Title = "WebSocket Host Configuration";
            iform.Label = "Enter Device Endpoint(ex: wss://192.168.1.13:12345/remote_pay)";
            iform.Value = ((WebSocketCloverDeviceConfiguration)WebSocketConfig).endpoint;
            iform.FormClosed += WSForm_Closed;
            iform.Show();
        }

        private void WSForm_Closed(object sender, EventArgs e)
        {
            if (((InputForm)sender).Status == DialogResult.OK)
            {
                string endpoint = ((InputForm)sender).Value;
                if (endpoint.Length > 0)
                {

                    Properties.Settings.Default.lastWSEndpoint = endpoint;
                    Properties.Settings.Default.Save();

                    selectedConfig = new WebSocketCloverDeviceConfiguration(endpoint, APPLICATION_ID, "Clover Windows Example POS","AISLE_3", Properties.Settings.Default.pairingAuthToken);
                    ((WebSocketCloverDeviceConfiguration)selectedConfig).OnPairingCode = pairingCodeHandler;
                    ((WebSocketCloverDeviceConfiguration)selectedConfig).OnPairingSuccess = pairingSuccessHandler;
                }
                ((CloverExamplePOSForm)this.Owner).InitializeConnector(selectedConfig);
                this.Close();
            }
        }

        private void cxButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
