using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.clover.remotepay.sdk;

namespace CloverExamplePOS.UiPages
{
    public partial class AboutSdkInfo : UserControl
    {
        CloverConnector _clover;
        public CloverConnector CloverConnector { get => _clover; set { _clover = value; DeviceInfo.Text = CreateConnectedDeviceInfo(value); } }

        public AboutSdkInfo()
        {
            InitializeComponent();
        }

        private void AboutSdkInfo_Load(object sender, EventArgs e)
        {
            SdkInfo.Text = CreateSdkInfo();
        }

        public string CreateSdkInfo()
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("Sdk File Version");
            output.AppendLine();

            Assembly assembly = typeof(CloverConnector).Assembly;
            FileVersionInfo version = FileVersionInfo.GetVersionInfo(assembly.Location);
            output.AppendLine($"  {version.InternalName}");
            output.AppendLine($"  version {version.FileVersion}");

            return output.ToString();
        }

        public string CreateConnectedDeviceInfo(CloverConnector clover)
        {
            StringBuilder output = new StringBuilder();

            if (clover != null)
            {
                if (clover.MerchantInfo != null)
                {
                    output.AppendLine("Merchant Info");
                    output.AppendLine();
                    output.AppendLine($"  {clover.MerchantInfo.merchantName}");
                    output.AppendLine($"  id {clover.MerchantInfo.merchantID}");
                    output.AppendLine($"  mid {clover.MerchantInfo.merchantMId}");
                    output.AppendLine();
                    output.AppendLine();
                }
                if (clover.MerchantInfo?.Device != null)
                {
                    output.AppendLine("Device Info");
                    output.AppendLine();
                    output.AppendLine($"  {clover.MerchantInfo.Device.Name}");
                    output.AppendLine($"  model {clover.MerchantInfo.Device.Model}");
                    output.AppendLine($"  serial number {clover.MerchantInfo.Device.Serial}");
                    output.AppendLine();
                    output.AppendLine();
                }
                if (clover.SDKInfo != null)
                {
                    output.AppendLine("SDK Info");
                    output.AppendLine();
                    output.AppendLine($"  {clover.SDKInfo.Name}");
                    output.AppendLine($"  version {clover.SDKInfo.Version}");
                    output.AppendLine();
                    output.AppendLine();
                }
            }

            return output.ToString();
        }
    }
}
