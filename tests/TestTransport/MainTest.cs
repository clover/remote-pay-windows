using System;
using System.Collections.Generic;
using System.Text;
using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using com.clover.sdk.v3.order;
using com.clover.sdk.v3.payments;
using com.clover.remote.order;



namespace TestTransport
{
    class MainTest
    {
        static void Main(string[] args)
        {
            TestDeviceListener devListener = new TestDeviceListener();
            TestConnectionListener connListener = new TestConnectionListener();

            CloverDeviceConfiguration config = new USBCloverDeviceConfiguration("__deviceID__");
            CloverConnector cloverConnector = new CloverConnector(config);

            //cloverConnector.Connections.Add(connListener);
            //cloverConnector.Devices.Add(devListener);

            while (!connListener.ready)
            {
                System.Console.WriteLine("Connected:" + connListener.connected);
                System.Console.WriteLine("Ready:" + connListener.ready);
                System.Threading.Thread.Sleep(3 * 1000);
            }

            System.Console.WriteLine("Connected:" + connListener.connected);
            System.Console.WriteLine("Ready:" + connListener.ready);

            //TEST DisplayOrder
            testDisplayOrder(cloverConnector);

            //TEST Payment and Void of that Payment
            testPaymentAndVoid(cloverConnector);

            //TEST Manual Refund (Naked Credit)
            testNakedCredit(cloverConnector);

            
        }

        public static void testDisplayOrder(CloverConnector cloverConnector)
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
            
            cloverConnector.DisplayOrder(order);

            DisplayLineItem line2 = DisplayFactory.createDisplayLineItem();
            line2.name = "another item";
            line2.price = "$4.68";
            line2.quantity = "2";
            line2.unitPrice = "$2.34";

            cloverConnector.DisplayOrderLineItemAdded(order, line2);

        }
        public static void testNakedCredit(CloverConnector cloverConnector)
        {
            //BEGIN: Test Refund
            TestBlockableRefundListener refundListener = new TestBlockableRefundListener(cloverConnector);

            ManualRefundRequest refundRequest = new ManualRefundRequest();
            refundRequest.Amount = 5432;


            System.Console.WriteLine("Preparing To Test Refund: $" + refundRequest.Amount * 100.00);
            System.Console.WriteLine("Press Any Key to Continue...");
            System.Console.ReadKey();

            //cloverConnector.Refunds += refundListener;
            cloverConnector.ManualRefund(refundRequest);

            while (!refundListener.hasResponse)
            {
                System.Console.WriteLine("Waiting for refundResponse");
                System.Threading.Thread.Sleep(1000);
            }

            System.Console.WriteLine("RefundResponse:" + refundListener.response.Code);
            System.Console.WriteLine("RefundResponse:" + refundListener.response.Credit.amount);

            //END: Test Refund
        }
        public static void testPaymentAndVoid(CloverConnector cloverConnector)
        {
            //BEGIN: Test Void
            TestBlockableSaleListener paymentListener = new TestBlockableSaleListener(cloverConnector);
            SaleRequest paymentRequest = new SaleRequest();
            paymentRequest.Amount = 1324;
            paymentRequest.TipAmount = 123;

            System.Console.WriteLine("Preparing To Test Sale: $" + paymentRequest.Amount * 100.00);
            System.Console.WriteLine("Press Any Key to Continue...");
            System.Console.ReadKey();

            //cloverConnector.Sales += paymentListener;
            cloverConnector.Sale(paymentRequest);

            while (!paymentListener.hasResponse)
            {
                System.Console.WriteLine("Waiting for paymentResponse");
                System.Threading.Thread.Sleep(1000);
            }

            SaleResponse response = paymentListener.response;
            string paymentId = response.Payment.id;
            string orderId = response.Payment.order.id;
            string employeeId = response.Payment.employee.id;

            VoidPaymentRequest voidRequest = new VoidPaymentRequest();
            voidRequest.OrderId = orderId;
            voidRequest.PaymentId = paymentId;
            voidRequest.EmployeeId = employeeId;
            voidRequest.VoidReason = VoidReason.USER_CANCEL.ToString();

            TestBlockableVoidListener voidListener = new TestBlockableVoidListener(cloverConnector);

            //cloverConnector.Voids += voidListener;
            cloverConnector.VoidPayment(voidRequest);

            while (!voidListener.hasResponse)
            {
                System.Console.WriteLine("Waiting for voidResponse");
                System.Threading.Thread.Sleep(1000);
            }

            VoidPaymentResponse voidResponse = voidListener.response;
            System.Console.WriteLine(voidResponse.Code);
            //END: Test Void

        }
    }

    class TestBlockableRefundListener : CloverRefundListener
    {
        CloverConnector cloverConnector { get; set; }
        public Boolean hasResponse { get; set; }
        public ManualRefundResponse response { get; set; }
        public RefundPaymentResponse paymentResponse { get; set; }

        public TestBlockableRefundListener(CloverConnector cc)
        {
            hasResponse = false;
            response = null;
            paymentResponse = null;
            this.cloverConnector = cc;
        }

        public void OnManualRefundResponse(ManualRefundResponse response)
        {
            System.Console.WriteLine("Manual Refund Response:" + response.Credit.amount);
            this.response = response;
            this.hasResponse = true;
        }

        public void OnRefundPaymentResponse(RefundPaymentResponse response)
        {
            System.Console.WriteLine("Refund Payment Response:" + response.RefundObj.amount);
            this.paymentResponse = response;
        }
    }
    class TestBlockableVoidListener : CloverVoidListener
    {
        CloverConnector cloverConnector { get; set; }
        public VoidPaymentResponse response { get; set; }
        public VoidTransactionResponse transactionResponse { get; set; }
        public Boolean hasResponse { get; set; }

        public TestBlockableVoidListener(CloverConnector cc)
        {
            this.cloverConnector = cc;
            hasResponse = false;
        }

        public void OnVoidTransactionResponse(VoidTransactionResponse response)
        {
            System.Console.WriteLine("Void Response: " + response.Code);
            //this.cloverConnector.Voids.Remove(this);
            this.cloverConnector = null;
            this.transactionResponse = response;
            this.hasResponse = true;
        }

        public void OnVoidPaymentResponse(VoidPaymentResponse response)
        {
            System.Console.WriteLine("Void Response: " + response.Code);
            //this.cloverConnector.Voids.Remove(this);
            this.cloverConnector = null;
            this.response = response;
            this.hasResponse = true;
        }
    }

    class TestBlockableSaleListener : CloverSaleListener
    {
        CloverConnector cloverConnector { get; set; }
        public SaleResponse response { get; set; }
        public Boolean hasResponse { get; set; }

        public TestBlockableSaleListener(CloverConnector cc)
        {
            this.cloverConnector = cc;
            hasResponse = false;
        }

        public void OnSaleResponse(SaleResponse response)
        {
            System.Console.WriteLine("BlockableSaleResponse: " + response.Code);
            //this.cloverConnector.Sales.Remove(this);
            this.cloverConnector = null;
            this.response = response;
            this.hasResponse = true;
        }
    }
    class TestSaleListener : CloverSaleListener
    {
        CloverConnector cloverConnector { get; set; }

        public TestSaleListener(CloverConnector cc)
        {
            this.cloverConnector = cc;
        }

        public void OnSaleResponse(SaleResponse response)
        {
            System.Console.WriteLine("SaleResponse: " + response.Code);
            //this.cloverConnector.Sales.Remove(this);
            this.cloverConnector = null;
        }
    }

    class TestConnectionListener : CloverConnectionListener
    {
        public Boolean connected { get; set; }
        public Boolean ready { get; set; }

        public TestConnectionListener()
        {
            connected = false;
            ready = false;
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

        public void OnDeviceReady()
        {
            this.ready = true;
        }
    }

    class TestDeviceListener : CloverDeviceListener
    {

        public Boolean ready { get; set; }
        
        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
            System.Console.WriteLine("DeviceEvent: " + deviceEvent.Code + " | " + deviceEvent.Message);
        }

        public void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {
            System.Console.WriteLine("DeviceEvent: " + deviceEvent.Code + " | " + deviceEvent.Message);
        }

        public void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {
            System.Console.WriteLine("DeviceError: " + deviceErrorEvent.Code + " | " + deviceErrorEvent.Message);
        }
    }
}
