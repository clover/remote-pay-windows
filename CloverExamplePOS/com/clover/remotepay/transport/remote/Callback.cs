using com.clover.remotepay.sdk;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com.clover.remotepay.transport.remote
{
    class CallbackController
    {

        CloverCallbackService cloverCallbackService;

        public CallbackController(ICloverConnector cloverConnector)
        {
            cloverCallbackService = new CloverCallbackService(cloverConnector);
        }
        public void init(RestClient restClient)
        {
            Uri baseAddress = new Uri("http://127.0.0.1:8182/");
            using (WebServiceHost host = new WebServiceHost(cloverCallbackService, baseAddress))
            {

                
                //Service.CloverConnector = new CloverConnector(new USBCloverDeviceConfiguration(null));
                //Service.CloverConnector.AddCloverConnectorListener(new CloverRESTConnectorListener());


                ServiceEndpoint ep = host.AddServiceEndpoint(typeof(ICloverCallbackService), new WebHttpBinding(), "CloverCallback");
                ServiceBehaviorAttribute sba = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                sba.InstanceContextMode = InstanceContextMode.Single;
                //ServiceDebugBehavior stp = host.Description.Behaviors.Find<ServiceDebugBehavior>();
                //stp.HttpHelpPageEnabled = false;

                try
                {
                    host.Open();
                }
                catch(AddressAccessDeniedException exception)
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
                        // can look at response or wait for callback
                    }
                });

                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
                //TODO: send test message, discovery, anything to see if someone is listening.
                //ccListener.OnDeviceReady();// once the service is up, without sending a ping, we must assume it's ready because http stateless


                



                /*
                foreach (ServiceEndpoint se in host.Description.Endpoints)
                {
                    Console.WriteLine("Address: {0}, Binding: {1}, Contract: {2}",
                        se.Address,
                        se.Binding.Name,
                        se.Contract.Name);
                }


                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                // Close the ServiceHost.
                host.Close();
                */
            }
        }

        internal void AddListener(CloverConnectorListener connectorListener)
        {
            cloverCallbackService.AddListener(connectorListener);
        }
    }
}
