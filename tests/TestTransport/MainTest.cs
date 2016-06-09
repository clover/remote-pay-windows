using System;
using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using com.clover.sdk.v3.order;
using com.clover.remote.order;



namespace TestTransport
{
    class MainTest
    {
        static void Main(string[] args)
        {

            CloverDeviceConfiguration config = new USBCloverDeviceConfiguration("__deviceID__");
            CloverConnector cloverConnector = new CloverConnector(config);
            cloverConnector.InitializeConnection();
            TestConnectorListener connListener = new TestConnectorListener(cloverConnector);

            while (!connListener.ready)
            {
                System.Console.WriteLine("Connected:" + connListener.connected);
                System.Console.WriteLine("Ready:" + connListener.ready);
                System.Threading.Thread.Sleep(3 * 1000);
            }

            System.Console.WriteLine("Connected:" + connListener.connected);
            System.Console.WriteLine("Ready:" + connListener.ready);

            //TEST DisplayOrder
            testDisplayOrder(cloverConnector, connListener);

            //TEST Payment and Void of that Payment
            testPaymentAndVoid(cloverConnector, connListener);

            //TEST Manual Refund (Naked Credit)
            testManualRefund(cloverConnector, connListener);

            
        }

        public static void testDisplayOrder(CloverConnector cloverConnector, TestConnectorListener connListener)
        {
            DisplayOrder order = DisplayFactory.createDisplayOrder();
            order.title = "Get Ready!";
            order.note = "Here is the note field";
            order.serviceChargeName = "Gonna Getcha";
            order.serviceChargeAmount = "$2,123.34";

            DisplayLineItem lineItem = DisplayFactory.createDisplayLineItem();
            lineItem.name = "My Item Name";
            lineItem.price = "$123.43";

            order.addDisplayLineItem(lineItem);

            DisplayDiscount discount = DisplayFactory.createDisplayDiscount();
            discount.amount = "$543.21";
            discount.name = "Nice Guy discount";

            order.addDisplayDiscount(discount);
            
            cloverConnector.ShowDisplayOrder(order);

            DisplayLineItem line2 = DisplayFactory.createDisplayLineItem();
            line2.name = "another item";
            line2.price = "$4.68";
            line2.quantity = "2";
            line2.unitPrice = "$2.34";

            cloverConnector.LineItemAddedToDisplayOrder(order, line2);

        }

        public static void testManualRefund(CloverConnector cloverConnector, TestConnectorListener connListener)
        {
            //BEGIN: Test Refund
            ManualRefundRequest refundRequest = new ManualRefundRequest();
            refundRequest.Amount = 5432;


            System.Console.WriteLine("Preparing To Test Refund: $" + refundRequest.Amount * 100.00);
            System.Console.WriteLine("Press Any Key to Continue...");
            System.Console.ReadKey();

            //cloverConnector.Refunds += refundListener;
            cloverConnector.ManualRefund(refundRequest);

            while (connListener.hasResponse)
            {
                System.Console.WriteLine("Waiting for refundResponse");
                System.Threading.Thread.Sleep(1000);
            }

            System.Console.WriteLine("RefundResponse:" + connListener.manualRefundResponse.Result);
            System.Console.WriteLine("RefundResponse:" + connListener.manualRefundResponse.Credit.amount);

            //END: Test Refund
        }
        public static void testPaymentAndVoid(CloverConnector cloverConnector, TestConnectorListener connListener)
        {
            //BEGIN: Test Void
            SaleRequest paymentRequest = new SaleRequest();
            paymentRequest.ExternalId = ExternalIDUtil.GenerateRandomString(13);

            paymentRequest.Amount = 1324;
            paymentRequest.TipAmount = 123;

            System.Console.WriteLine("Preparing To Test Sale: $" + paymentRequest.Amount * 100.00);
            System.Console.WriteLine("Press Any Key to Continue...");
            System.Console.ReadKey();

            //cloverConnector.Sales += paymentListener;
            cloverConnector.Sale(paymentRequest);

            while (!connListener.hasResponse)
            {
                System.Console.WriteLine("Waiting for paymentResponse");
                System.Threading.Thread.Sleep(1000);
            }

            SaleResponse response = connListener.saleResponse;
            string paymentId = response.Payment.id;
            string orderId = response.Payment.order.id;
            string employeeId = response.Payment.employee.id;

            VoidPaymentRequest voidRequest = new VoidPaymentRequest();
            voidRequest.OrderId = orderId;
            voidRequest.PaymentId = paymentId;
            voidRequest.EmployeeId = employeeId;
            voidRequest.VoidReason = VoidReason.USER_CANCEL.ToString();

            cloverConnector.VoidPayment(voidRequest);

            while (!connListener.hasResponse)
            {
                System.Console.WriteLine("Waiting for voidResponse");
                System.Threading.Thread.Sleep(1000);
            }

            VoidPaymentResponse voidResponse = connListener.voidPaymentResponse;
            System.Console.WriteLine(voidResponse.Result);
            //END: Test Void

        }
    }

    class TestConnectorListener : ICloverConnectorListener
    {
        CloverConnector cloverConnector { get; set; }
        public Boolean hasResponse { get; set; }
        public ManualRefundResponse manualRefundResponse { get; set; }
        public RefundPaymentResponse refundPaymentResponse { get; set; }
        public VoidPaymentResponse voidPaymentResponse { get; set; }
        public SaleResponse saleResponse { get; set; }
        public Boolean connected { get; set; }
        public Boolean ready { get; set; }


        public TestConnectorListener(CloverConnector cc)
        {
            hasResponse = false;
            manualRefundResponse = null;
            voidPaymentResponse = null;
            refundPaymentResponse = null;
            saleResponse = null;
            this.cloverConnector = cc;
        }

        public void OnManualRefundResponse(ManualRefundResponse response)
        {
            System.Console.WriteLine("Manual Refund Response:" + response.Credit.amount);
            this.manualRefundResponse = response;
            this.hasResponse = true;
        }

        public void OnRefundPaymentResponse(RefundPaymentResponse response)
        {
            System.Console.WriteLine("Refund Payment Response:" + response.Refund.amount);
            this.refundPaymentResponse = response;
            this.hasResponse = true;
        }

        public void OnVoidPaymentResponse(VoidPaymentResponse response)
        {
            System.Console.WriteLine("Void Response: " + response.Result);
            //this.cloverConnector.Voids.Remove(this);
            this.cloverConnector = null;
            this.voidPaymentResponse = response;
            this.hasResponse = true;
        }

        public void OnSaleResponse(SaleResponse response)
        {
            System.Console.WriteLine("BlockableSaleResponse: " + response.Result);
            //this.cloverConnector.Sales.Remove(this);
            this.cloverConnector = null;
            this.saleResponse = response;
            this.hasResponse = true;
        }

        public void OnDeviceConnected()
        {
            this.connected = true;

        }

        public void OnDeviceDisconnected()
        {
            this.connected = false;
            this.ready = false;
        }

        public void OnDeviceReady(MerchantInfo merchantInfo)
        {
            this.ready = true;
        }

        public void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {
            throw new NotImplementedException();
        }

        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
            throw new NotImplementedException();
        }

        public void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {
            throw new NotImplementedException();
        }

        public void OnPreAuthResponse(PreAuthResponse response)
        {
            throw new NotImplementedException();
        }

        public void OnAuthResponse(AuthResponse response)
        {
            throw new NotImplementedException();
        }

        public void OnTipAdjustAuthResponse(TipAdjustAuthResponse response)
        {
            throw new NotImplementedException();
        }

        public void OnCapturePreAuthResponse(CapturePreAuthResponse response)
        {
            throw new NotImplementedException();
        }

        public void OnVerifySignatureRequest(VerifySignatureRequest request)
        {
            throw new NotImplementedException();
        }

        public void OnCloseoutResponse(CloseoutResponse response)
        {
            throw new NotImplementedException();
        }

        public void OnTipAdded(TipAddedMessage message)
        {
            throw new NotImplementedException();
        }

        public void OnVaultCardResponse(VaultCardResponse response)
        {
            throw new NotImplementedException();
        }
        public void OnDeviceError(Exception e)
        {
            throw new NotImplementedException();
        }


    }

}
