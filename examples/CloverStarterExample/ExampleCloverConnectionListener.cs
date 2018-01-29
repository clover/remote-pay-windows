using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using com.clover.sdk.v3.payments;

namespace CloverStarterExample
{
    // Create an implementation of ICloverConnectorListener
    public class ExampleCloverConnectionListener : DefaultCloverConnectorListener
    {
        public Boolean deviceReady { get; set; }
        public Boolean deviceConnected { get; set; }
        public Boolean saleDone { get; set; }
        public Boolean refundDone { get; set; }
        public String paymentId { get; set; }
        public String orderId { get; set; }
        
        public ExampleCloverConnectionListener(ICloverConnector cloverConnector) : base(cloverConnector)
        {
        }

        public override void OnDeviceReady(MerchantInfo merchantInfo)
        {
            base.OnDeviceReady(merchantInfo);
            deviceReady = true;
            //Connected and available to process requests
        }

        public override void OnDeviceConnected()
        {
            base.OnDeviceConnected();
            deviceConnected = true;
            // Connected, but not available to process requests
                
            }

        public override void OnDeviceDisconnected()
        {
            base.OnDeviceDisconnected();
            Console.WriteLine("Disconnected");
            //Disconnected
        }

        public override void OnSaleResponse(SaleResponse response)
        {
            base.OnSaleResponse(response);
            saleDone = true;
            paymentId = response.Payment.id;
            orderId = response.Payment.order.id;
            
        }

        public override void OnConfirmPaymentRequest(ConfirmPaymentRequest request)
        {
            
        }

        public override void OnRefundPaymentResponse(RefundPaymentResponse response)
        {
            base.OnRefundPaymentResponse(response);
            try
            {
                if (response.Success)
                {
                    Refund refund = response.Refund;
                    Console.WriteLine("Refund request successful");
                    Console.WriteLine(" ID: " + refund.id);
                    Console.WriteLine(" Amount: " + refund.amount);
                    Console.WriteLine(" Order ID: " + response.OrderId);
                    Console.WriteLine(" Payment ID: " + response.PaymentId);
                }
                else
                {
                    Console.Error.WriteLine("Refund request failed - " + response.Reason);
                }
            }
            catch
            {
                Console.Error.WriteLine("Error handling sale response");
            }

            refundDone = true;

        }

    }
}
