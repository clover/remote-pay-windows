using com.clover.remotepay.transport.remote;
using com.clover.remotepay.transport;
using com.clover.remotepay.transport.remote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace CloverExamplePOS
{
    public partial class StartupForm : OverlayForm
    {
        CloverDeviceConfiguration selectedConfig;

        CloverDeviceConfiguration USBConfig = new USBCloverDeviceConfiguration("__deviceID__");
        CloverDeviceConfiguration TestConfig = new TestCloverDeviceConfiguration();
        CloverDeviceConfiguration WebSocketConfig = new WebSocketCloverDeviceConfiguration("10.0.1.193", 14285);
        CloverDeviceConfiguration RestConfig = new RemoteRESTCloverConfiguration("localhost", 8181);
        CloverDeviceConfiguration RemoteWebSocketConfig = new RemoteWebSocketCloverConfiguration("localhost", 8889);

        public StartupForm(Form tocover) : base(tocover)
        {
            InitializeComponent();
            //base.OnLoad(null);
        }



        private void StartupDialog_Load(object sender, EventArgs e)
        {
            // populate the combo box
            var dataSource = new List<ConfigWrapper>();
            dataSource.Add(new ConfigWrapper("USB Device", USBConfig));
            dataSource.Add(new ConfigWrapper("Emulator", TestConfig));
            dataSource.Add(new ConfigWrapper("Web Socket (Device)", WebSocketConfig));
            dataSource.Add(new ConfigWrapper("REST Service (local)", RestConfig));
            dataSource.Add(new ConfigWrapper("Web Socket Service (local)", RemoteWebSocketConfig));

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
                    //TODO: validate IP and port
                    int port = Int32.Parse(tokens[0]);
                    selectedConfig = new RemoteWebSocketCloverConfiguration("localhost", port);
                    //InitializeConnector(WebSocketConfig);
                }
                ((CloverExamplePOSForm)this.Owner).InitializeConnector(selectedConfig);
                this.Close();
            }
        }

        private void InitWebSocket()
        {
            InputForm iform = new InputForm(this);
            iform.Title = "WebSocket Host Configuration";
            iform.Label = "Enter Device IP:Port(ex: 10.0.1.13:8080)";
            iform.Value = ((WebSocketCloverDeviceConfiguration)WebSocketConfig).hostname + ":" + ((WebSocketCloverDeviceConfiguration)WebSocketConfig).port;
            iform.FormClosed += WSForm_Closed;
            iform.Show();
        }

        private void WSForm_Closed(object sender, EventArgs e)
        {
            if (((InputForm)sender).Status == DialogResult.OK)
            {
                string val = ((InputForm)sender).Value;
                string[] tokens = val.Split(':');
                if (tokens.Length == 2)
                {
                    //TODO: validate IP and port
                    string ip = tokens[0];
                    int port = Int32.Parse(tokens[1]);
                    selectedConfig = new WebSocketCloverDeviceConfiguration(ip, port);
                    //InitializeConnector(WebSocketConfig);
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
