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
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;


namespace CloverWindowsSDKWebSocketService
{

    [RunInstaller(true)]
    public class CloverWebSocketServiceInstaller : Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public CloverWebSocketServiceInstaller()
        {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.DisplayName = CloverWebSocketService.SERVICE_NAME;
            serviceInstaller.ServiceName = CloverWebSocketService.SERVICE_NAME;
            serviceInstaller.Description = "Provides a WebSocket wrapper for the Clover SDK";

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            string port = this.Context.Parameters["Port"];
            if (port == null)
            {
                port = "8889";
            }
            StringBuilder path = new StringBuilder(Context.Parameters["assemblypath"]);
            if (path[0] != '"')
            {
                path.Insert(0, '"');
                path.Append('"');
            }
            path.Append(" /P " + port);
            Context.Parameters["assemblypath"] = path.ToString();
            base.Install(stateSaver);
            SetRecoveryOptions(CloverWebSocketService.SERVICE_NAME);
        }

        static void SetRecoveryOptions(string serviceName)
        {
            int exitCode;
            using (var process = new Process())
            {
                var startInfo = process.StartInfo;
                startInfo.FileName = "sc";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                // tell Windows that the service should restart if it fails
                startInfo.Arguments = string.Format("failure \"{0}\" reset= 0 actions= restart/60000/restart/60000/restart/60000", serviceName);

                process.Start();
                process.WaitForExit();

                exitCode = process.ExitCode;
            }

            if (exitCode != 0)
                throw new InvalidOperationException();
        }
    }
}


