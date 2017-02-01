# Clover SDK for Windows PoS Integration

Current version: 1.1 (1.1.0.2)

## Overview

This SDK provides an API with which to allow your Windows Point-of-Sale (POS) system to interface with a [Clover® Mini device] (https://www.clover.com/pos-hardware/mini). From the Mini, merchants can accept payments using: credit, debit, EMV contact and contactless (including Apple Pay), gift cards, EBT (electronic benefit transfer), and more. Learn more about integrations at [clover.com/integrations](https://www.clover.com/integrations).

The Windows project includes a .NET dll, REST API Service, WebSocket API Service and an example POS. There are 3 ways to connect to a device
- CloverConnector.DLL can be used directly by a .NET application. Note: Requires .NET 4.0
- CloverConnectorRESTService provides a Windows service with a REST API and REST callbacks. To use this, the POS will have to implement a REST service to get callbacks from the service
- CloverConnectorWebSocketService provides a Windows service with a WebSocket endpoint. To use this, the 

To effectively work with the project you'll need:
- A Windows machine or VM
- An [IDE](https://www.visualstudio.com/downloads/), Visual Studio is one option

To complete a transaction end to end, we recommend getting a [Clover Mini Dev Kit](http://cloverdevkit.com/collections/devkits/products/clover-mini-dev-kit).

More documentation can be found at [Clover Docs Site](https://docs.clover.com/build/getting-started-with-cloverconnector/).

## Release Notes
# Version 1.1.0.2 
  * Fixed a version mismatch issue for confirming payments by the POS.
  * **Note:** *This version should be used instead of 1.1.0.1*

# Version 1.1
* Renamed/Added/Removed a number of API operations and request/response objects to establish
  better consistency across platforms

  * ICloverConnector (Operations)
    * Added
      * printImageFromURL
      * initializeConnection **(REQUIRED)**
      * addCloverConnectorListener
      * removeCloverConnectorListener
      * acceptPayment - (REQUIRED) Takes a payment object - possible response to a ConfirmPaymentRequest
      * rejectPayment - (REQUIRED) Takes a payment object and the challenge that was associated with
                        the rejection - possible response to a ConfirmPaymentRequest
      * retrievePendingPayments - retrieves a list of payments that were taken offline and are pending
                                  server submission/processing.
    * Renamed
      * capturePreAuth - formerly captureAuth
      * showDisplayOrder - formerly displayOrder - this is now the only operation needed
        to display/change order information displayed on the mini
      * removeDisplayOrder - formerly displayOrderDelete
    * Removed
      * displayOrderLineItemAdded - showDisplayOrder now handles this
      * displayOrderLineItemRemoved - showDisplayOrder now handles this
      * displayOrderDiscountAdded - showDisplayOrder now handles this
      * displayOrderDiscountRemoved - showDisplayOrder now handles this
    * Modified
      * SaleRequest, AuthRequest, PreAuthRequest and ManualRefund require ExternalId to be set. (REQUIRED)
        * ExternalId should be unique per transaction allowing the Clover device to detect, and potentially reject, if the same externalID is reused for subsequent transaction requests
      * changed all device action API calls to return void
      * CloverConnecter now requires ApplicationId to be set via configuration/installation of the third party application. This is provided as part of the device configuration that is passed in during the creation of the CloverConnector.
      * Behavior change for RefundPaymentRequest. In the prior versions, a value of zero for the amount field would trigger a refund of the full payment amount. With the 1.0 version, passing zero in the amount field will trigger a validation failure. Use FullRefund:boolean to specify a full refund amount. NOTE: This will attempt to refund the original (full) payment amount, not the remaining amount, in a partial refund scenario.    
  * ICloverConnectorListener (Notifications)
    * Added
      * onPaymentConfirmation - (REQUIRED) consists of a Payment and a list of challenges/void reasons  
      * onDeviceError - general callback when there is an error communicating with the device
      * onPrintManualRefundReceipt - if disablePrinting=true on the request, this will get called to indicate the POS can print this receipt
      * onPrintManualRefundDeclineReceipt - if disablePrinting=true on the request, this will get called to indicate the POS can print this receipt
      * onPrintPaymentReceipt - if disablePrinting=true on the request, this will get called to indicate the POS can print this receipt
      * onPrintPaymentDeclineReceipt - if disablePrinting=true on the request, this will get called to indicate the POS can print this receipt
      * onPrintPaymentMerchantCopyReceipt - if disablePrinting=true on the request, this will get called to indicate the POS can print this receipt
      * onPrintRefundPaymentReceipt - if disablePrinting=true on the request, this will get called to indicate the POS can print this receipt
      * onRetrievePendingPaymentsResponse - called with the list of payments taken on the device that aren't processed on the server yet
    * Renamed
      * onDeviceDisconnected - formerly onDisconnected
      * onDeviceConnected - formerly on onConnected
      * onDeviceReady - formerly onReady
      * onTipAdjustAuthResponse - formerly onAuthTipAdjustResponse
      * onCapturePreAuthResponse - formerly onPreAuthCaptureResponse
      * onVerifySignatureRequest - formerly onSignatureVerifyRequest
    * Removed
      * onTransactionState
      * onConfigErrorResponse - These are now processed as normal operation responses
      * onError - now handled by onDeviceError or through normal operation responses
      * onDebug
  * Request/Response Objects
    * Added
      * ConfirmPaymentRequest - Contains a Payment and a list of "challenges" from the
        Clover device during payment operations, if there are questions for the merchant
        on their willingness to accept whatever risk is associated with that payment's
        challenge.
      * RetrievePendingPaymentsResponse - Contains a list of PendingPaymentEntry objects,
                                          which have the paymentId and amount for each
                                          payment that has yet to be sent to the server
                                          for processing.
      * PrintManualRefundReceiptMessage - Contains the Credit object to be printed
      * PrintManualRefundDeclineReceiptMessage - Contains the declined Credit object to be printed
      * PrintPaymentReceiptMessage - Contains the Order and Payment to be printed
      * PrintPaymentDeclineReceiptMessage - Contains the declined Payment and reason to be printed
      * PrintPaymentMerchantCopyReceiptMessage - Contains the payment to be printed
      * PrintRefundPaymentReceiptMessage - Contains Payment, Refund and Order
    * Renamed
      * VerifySignatureRequest - formerly SignatureVerifyRequest
      * CapturePreAuthRequest - formerly CaptureAuthRequest
      * VoidPaymentRequest - formerly VoidTransactionRequest
      * CloseoutRequest - formerly separate field-level parameters
      * TipAdjustAuthResponse - formerly AuthTipAdjustResponse
    * Removed
      * ConfigErrorResponse - These are now processed as normal operation
    * Modified
      * All Response Messages now return the following:​
        * Success:boolean
        * Result:enum [SUCCESS|FAIL|CANCEL|ERROR|UNSUPPORTED] FAIL - failed to process with values/properties as-is CANCEL - canceled, retry could work ERROR - un expected exception occurred UNSUPPORTED - merchant config won't allow the request
        * Reason:String optional information about result value, if not SUCCESS
        * Message:String optional detail information about the result value, if not success
      * SaleResponse, AuthResponse and PreAuthResponse have 3 new flags (e.g. The payment gateway may force an AuthRequest to a SaleRequest)
      * IsSale:boolean - true if the payment is closed
      * IsAuth:boolean - true if the payment can be tip adjusted before closeout
      * IsPreAuth:boolean - true if the payment needs to be "captured" before closeout will close it
* voidPayment operation fix to verify connection status and check for void request
  acknowledgement from the Clover device prior to issuing a successful response
* Added DefaultCloverConnectorListener, which automatically accepts signature if a verify
  signature request is received
* Behavior change for RefundPaymentRequest - In the prior versions, a value of zero for
  the amount field would trigger a refund of the full payment amount. With the 1.1 version,
  passing zero in the amount field will trigger a validation failure.
  Set fullRefund:boolean to `true` to specify a full refund. NOTE: This will attempt to refund
  the original (full) payment amount, not the remaining amount, in a partial refund scenario.
* CloverConnecter now requires ApplicationId to be set via configuration of the
  third party application. This is provided as part of the device configuration
  that is passed in during the creation of the CloverConnector.  The String input parameter of
  "applicationId", which is passed in when instantiating the DefaultCloverDevice, should be
  set using the format of <company specific package>:<version> e.g. com.clover.ExamplePOS:1.2
* Modified remote pay so prompts to take orders offline and flagging duplicate orders appear only in merchant facing mode.
* Added ability to query pending payments.

## Getting Connected
1. Download the USB Pay Display app from the Clover App Market on your Clover Mini Dev Kit.
2. Open the USB Pay Display app
3. Run the Clover Connector Windows Example POS app on your Windows POS device
4. You should see the example POS screen and connection options listed based on the installed connectors. If everything worked you'll get a connected status. If it remains disconnected, you'll want to check that 
   1) The Clover USB Device drivers are installed
   2) You are connecting the correct cable to the correct connection point on the Clover Mini “hub” - port USB(port with Clover logo). You will need to use the USB cable that the device came with. 

## Working with the SDK

  ```
      ICloverConnect cloverConnector = new CloverConnector(new USBCloverDeviceConfiguration("com.yourcompany.app:2.1.1", false));
      cloverConnector.addCloverConnectorListener(new YourListener(cloverConnector))
      cloverConnector.InitializeConnection();
      ...

	  class YourListener : DefaultCloverConnectorListener
	  {
	        public MyListener(ICloverConnector cc) : base(cc) 
			{
			}
			
			public override void OnConfirmPaymentRequest(ConfirmPaymentRequest request)
            {
				// accepting by default... can pop a dialog or look at config properties to determine if the payment should be accepted or not
				// this will get called in at least 2 situations
				// 1. Detect possible duplicate payment
				// 2. Confirm offline payments if the merchant/Tx is configured to allow offline payments
                this.cloverConnector.AcceptPayment(request.Payment);
            }
			
			pubilc override void OnSaleResponse(SaleResponse response) 
			{
			    if(response.Success) 
				{
				    // payment was successful
					// do something with response.Payment
				}
				else
				{
				    // payment didn't complete, can look at response.Result, response.Reason for additional info
				}
			}
			
			// wait until this gets called to indicate the device
            // is ready to communicate before calling other methods
            public override void onDeviceReady(MerchantInfo merchantInfo) 
		    {
                super.onDeviceReady(merchantInfo);
			    Start()
            }
		  
            public void Start() 
            {
                SaleRequest saleRequest = new SaleRequest(2215, "b1234"); // $22.15 with externalID "b1234"
                cloverConnector.sale(saleRequest);
            }
	  }
  ```

## Building the SDK
Map of Contents:
```
.
├── examples                           # Contains C# Example application
│	└── CloverExamplePOS
│	└── exampleMessages
├── installers                         # root contains NSIS files to create installer
│	└── icons                          # 
│	└── Microsoft                      # Holds the .NET fmwk installer, packaged with the Clover installer
│	└── tools                          # Tools needed to build an installer. These need to be installed on the machine building the installer
├── lib
│	└── CloverConnector                # CloverConnector module code
│	└── CloverWindowsSDK               # C# port of Clover Android SDK classes
│	└── CloverWindowsTransport         # Transport definitions (USB, WebSocket)
├── packages                           # Contains 3rd party dependencies
│	└── 
├── services
│	└── CloverWindowsSDKRESTService    # Source for Windows REST Service
│	└── CloverWindowsWebSocketService  # Source for Windows WebSocket Service
│	└── shared                         # Contains code shared by the Services
├── tests
│	└── TestJson
│	└── TestTransport
└── windows-drivers
	└── DriverPackages                 # contains drivers for WinUSB
	│	└── Clover_Mini_Combined_Mode_WinUSB     #
	│	└── Clover_Mobile_Combined_Mode_WinUSB   #
	└── win-7                          # deprecated, was used for libusb implementation
	└── win-8                          # deprecated, was used for libusb implementation
	└── win-xp                         # deprecated, was used for libusb implementation
```

Building and running:

- .NET: using VisualStudio 2015:
 - open the Clover.sln, build all, select the CloverExamplePOS and run.

----VM SETUP-----
1. Install VMWare Fusion.  Virtual box does not see the device properly.
2. Run driver installers - see remote-pay-windows\windows-drivers\DriverPackages\Clover_Mini_Combined_Mode_WinUSB\InstallCloverMiniUSBDrivers.exe
-----END VM SETUP-----

-----DEBUGGING-----
```
To debug the device in tcpip:
Plug the device in to the laptop, select "Connect to Mac".
This sets the device up to be debugged on port 5555
>adb tcpip 5555
NOTE: if you get "error: more than one device and emulator" then you need to tell adb which device to set up explicitly.
List the devices:
>adb devices
List of devices attached
C010UC43040345	device
C030UQ50550081	device
Then explicitly tell adb which device to set up tcpip for:
>adb -s C030UQ50550081 tcpip 5555
This makes the device disconnect and reconnect, select "Connect to Windows".
You need to 'connect' to the device
>adb connect 192.168.1.3:5555
Now you can use 'adb logcat' or connect with the debugger via ip. For example:
>adb devices
List of devices attached
C010UC43040345	device
192.168.1.3:5555	device
```
-----END DEBUGGING----
