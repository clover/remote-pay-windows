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

using com.clover.remotepay.sdk;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace com.clover.remotepay.transport.remote
{
    /// <summary>
    /// manages/start/stops the REST server used
    /// to listen for callback messages
    /// </summary>
    class CallbackController
    {

        ICloverConnector connector { get; set; }
        CloverRESTServer restServer;
        List<ICloverConnectorListener> listeners = new List<ICloverConnectorListener>();

        public CallbackController(ICloverConnector cloverConnector)
        {
            connector = cloverConnector;
        }
        public void init(RestClient restClient)
        {
            restServer = new CloverRESTServer("localhost", "8182", "http");
            restServer.CloverConnector = connector;
            listeners.ForEach(listener => restServer.AddCloverConnectorListener(listener));
            listeners.Clear();
            {

                try
                {
                    restServer.Start();
                }
                catch(Exception)
                {
                    MessageBox.Show("Couldn't open callback listener service. Are you running as administrator?");
                }


                IRestRequest restRequest = new RestRequest("/Status", Method.GET);
                restClient.ExecuteAsync(restRequest, response =>
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {

                        Console.WriteLine(response.ResponseStatus + " : " + response.StatusCode + " : " + response.ErrorMessage);
                    }
                    else
                    {
                        // response is ok, so should process
                    }
                });

            }
        }

        internal void AddListener(ICloverConnectorListener connectorListener)
        {
            
            if(restServer != null)
            {
                restServer.AddCloverConnectorListener(connectorListener);
            }
            else
            {
                listeners.Add(connectorListener);
            }
        }

        internal void Shutdown()
        {
            try
            {
                restServer.Stop();
                restServer = null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
