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

using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;


namespace CloverWindowsSDKREST
{

    [RunInstaller(true)]
    public class CloverRESTServiceInstaller : Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public CloverRESTServiceInstaller()
        {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.DisplayName = CloverRESTService.SERVICE_NAME;
            serviceInstaller.ServiceName = CloverRESTService.SERVICE_NAME;
            serviceInstaller.Description = "Provides a REST Service wrapper for the Clover Connector";

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            string port = this.Context.Parameters["Port"];
            if (port == null)
            {
                port = "8181";
            }
            StringBuilder path = new StringBuilder(Context.Parameters["assemblypath"]);
            if (path[0] != '"')
            {
                path.Insert(0, '"');
                path.Append('"');
            }
            path.Append(" /P " + port);

            string lanHost = this.Context.Parameters["LANHost"];
            if (lanHost != null)
            {
                path.Append(" /L " + lanHost);
            }

            string callbackEndpoint = this.Context.Parameters["CallbackEndpoint"];
            if (callbackEndpoint != null)
            {
                path.Append(" /C " + callbackEndpoint);
            }

            Context.Parameters["assemblypath"] = path.ToString();
            base.Install(stateSaver);
            SetRecoveryOptions(CloverRESTService.SERVICE_NAME);
        }

        static void SetRecoveryOptions(string serviceName)
        {
            int exitCode;
            using (var process = new System.Diagnostics.Process())
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
                throw new System.InvalidOperationException();
        }
    }
}


