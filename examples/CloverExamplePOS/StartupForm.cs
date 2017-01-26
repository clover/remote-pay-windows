// Copyright (C) 2016 Clover Network, Inc.
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

        const String APPLICATION_ID = "CloverExamplePOS:1.1.0.1";

        CloverDeviceConfiguration USBConfig = new USBCloverDeviceConfiguration("__deviceID__", APPLICATION_ID, false, 1);
        CloverDeviceConfiguration RestConfig = new RemoteRESTCloverConfiguration("localhost", 8181, APPLICATION_ID, false, 1);
        CloverDeviceConfiguration RemoteWebSocketConfig = new RemoteWebSocketCloverConfiguration("localhost", 8889, APPLICATION_ID);

        public StartupForm(Form tocover) : base(tocover)
        {
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
            catch (InvalidCastException ice)
            {
                isSdkInstalled = false;
            }
            catch (NullReferenceException nre)
            {
                isSdkInstalled = false;
            }
            // isREST
            try
            {
                isRESTInstalled = ((int)IsREST == 1);
            }
            catch (InvalidCastException ice)
            {
                isRESTInstalled = false;
            }
            catch (NullReferenceException nre)
            {
                isRESTInstalled = false;
            }
            // isWS
            try
            {
                isWSInstalled = ((int)IsWS == 1);
            }
            catch (InvalidCastException ice)
            {
                isWSInstalled = false;
            }
            catch (NullReferenceException nre)
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
            iform.Label = "Enter Port (e.g. 8889)";
            iform.Value = ""+((RemoteWebSocketCloverConfiguration)RemoteWebSocketConfig).port;
            iform.FormClosed += WSRemoteForm_Closed;
            iform.Show();
        }

        private void WSRemoteForm_Closed(object sender, EventArgs e)
        {
            if (((InputForm)sender).Status == DialogResult.OK)
            {
                string val = ((InputForm)sender).Value;
                string[] tokens = val.Split(' ');
                if (tokens.Length == 1)
                {
                    int port = Int32.Parse(tokens[0]);
                    selectedConfig = new RemoteWebSocketCloverConfiguration("localhost", port, APPLICATION_ID);
                }
                ((CloverExamplePOSForm)this.Owner).InitializeConnector(selectedConfig);
                this.Close();
            }
        }

        private void WSForm_Closed(object sender, EventArgs e)
        {
            if (((InputForm)sender).Status == DialogResult.OK)
            {
                string val = ((InputForm)sender).Value;
                string[] tokens = val.Split(':');
                if (tokens.Length == 2)
                {
                    string ip = tokens[0];
                    int port = Int32.Parse(tokens[1]);
                    selectedConfig = new WebSocketCloverDeviceConfiguration(ip, port, APPLICATION_ID, false, 1);
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
