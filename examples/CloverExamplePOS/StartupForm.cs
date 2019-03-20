﻿// Copyright (C) 2018 Clover Network, Inc.
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

using com.clover.remotepay.transport;
using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Windows.Forms;

namespace CloverExamplePOS
{

    public partial class StartupForm : OverlayForm
    {
        CloverDeviceConfiguration selectedConfig;

        const String APPLICATION_ID = "com.clover.CloverExamplePOS:3.0.2";

        CloverDeviceConfiguration USBConfig = new USBCloverDeviceConfiguration("__deviceID__", APPLICATION_ID, false, 1);

        WebSocketCloverDeviceConfiguration WebSocketConfig = new WebSocketCloverDeviceConfiguration("wss://192.168.1.14:12345/remote_pay", APPLICATION_ID, false, 1, "Clover Windows Example POS", "POS-3", Properties.Settings.Default.pairingAuthToken, null, null, null); // set the 3 delegates in the ctor

        PairingDeviceConfiguration.OnPairingCodeHandler pairingCodeHandler = null;
        PairingDeviceConfiguration.OnPairingSuccessHandler pairingSuccessHandler = null;
        PairingDeviceConfiguration.OnPairingStateHandler pairingStateHandler = null;


        public StartupForm(Form tocover, PairingDeviceConfiguration.OnPairingCodeHandler pairingHandler, PairingDeviceConfiguration.OnPairingSuccessHandler successHandler, PairingDeviceConfiguration.OnPairingStateHandler stateHandler) : base(tocover)
        {
            pairingCodeHandler = pairingHandler;
            pairingSuccessHandler = successHandler;
            pairingStateHandler = stateHandler;
            WebSocketConfig.OnPairingCode = pairingHandler;
            WebSocketConfig.OnPairingSuccess = successHandler;
            WebSocketConfig.OnPairingState = stateHandler;
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
            // populate the combo box with connection types
            List<ConfigWrapper> dataSource = new List<ConfigWrapper>();

            dataSource.Add(new ConfigWrapper("Clover Connector USB", USBConfig));
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
            if (selectedConfig is WebSocketCloverDeviceConfiguration)
            {
                InitWebSocket();
            }
            else
            {
                ((CloverExamplePOSForm)this.Owner).InitializeConnector(selectedConfig);
                Close();
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
                    ((WebSocketCloverDeviceConfiguration)selectedConfig).OnPairingState = pairingStateHandler;
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
