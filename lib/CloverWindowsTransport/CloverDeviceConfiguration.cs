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

using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace com.clover.remotepay.transport
{
    public interface CloverDeviceConfiguration
    {
        string getCloverDeviceTypeName();
        string getMessagePackageName();
        string getName();
        bool getEnableLogging();
        int getPingSleepSeconds();
        string getRemoteApplicationID();
        CloverTransport getCloverTransport();
        int getMaxMessageCharacters();
    }

    public static class CloverDeviceConfigurationExtensionMethods
    {
        internal static string getRemoteSdk(this CloverDeviceConfiguration config, CloverTransport transport)
        {
            string REG_KEY = "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\CloverSDK";
            string receiver = "DLL";
            try
            {
                object rReceiver = Registry.GetValue(REG_KEY, "DisplayName", "unset");
                if (rReceiver != null && !rReceiver.ToString().Equals("unset"))
                {
                    receiver = rReceiver.ToString();
                }
            }
            catch (Exception e)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($"{config.getMessagePackageName().GetType()}->{e.Message}");
                }
            }

            string shortTitle = transport.ShortTitle();
            string shortTransportType = String.IsNullOrWhiteSpace(shortTitle) ? "UNKNOWN" : shortTitle;

            // Build SdkInfo string
            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("CloverConnector");
            string sdkInfoString = AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyDescriptionAttribute>(assembly).Description
                + "_" + receiver
                + "|" + shortTransportType
                + ":"
                + (assembly.GetAssemblyAttribute<System.Reflection.AssemblyFileVersionAttribute>()).Version
                + (assembly.GetAssemblyAttribute<System.Reflection.AssemblyInformationalVersionAttribute>()).InformationalVersion;

            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry($"{config.getMessagePackageName().GetType()}->SDKInfo from assembly and registry = {sdkInfoString}");
            }

            return sdkInfoString;
        }
    }
}
