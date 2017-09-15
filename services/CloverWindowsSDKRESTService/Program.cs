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

using System;
using System.ServiceProcess;

namespace CloverWindowsSDKREST
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // /P - port, /C - certificate path, /T - test/emulator connector, /L - lan adapter
            // /D - developer/debug mode, runs in console
            bool console = false;

            foreach (string s in args)
            {
                if ("/D".Equals(s))
                {
                    console = true;
                    break;
                }
            }

            if (console)
            {
                CloverRESTService service = new CloverRESTService();
                service.DebugStart(args);
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            }
            else
            {
                ServiceBase.Run(new CloverRESTService());
            }
        }
    }
}
