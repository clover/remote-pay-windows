# Clover SDK for Windows PoS Integration

Current version: 1.3.1

## Overview

This SDK provides an API with which to allow your Windows Point-of-Sale (POS) system to interface with a [Clover® Mini device] (https://www.clover.com/pos-hardware/mini). From the Mini, merchants can accept payments using: credit, debit, EMV contact and contactless (including Apple Pay), gift cards, EBT (electronic benefit transfer), and more. Learn more about integrations at [clover.com/integrations](https://www.clover.com/integrations).

The Windows project includes a .NET dll, REST API Service, WebSocket API Service and an example POS. There are 3 ways to connect to a device
- CloverConnector.DLL can be used directly by a .NET application. Note: Requires .NET 4.0
- CloverConnectorRESTService provides a Windows service with a REST API and REST callbacks. To use this, the POS will have to implement a REST service to get callbacks from the service
- CloverConnectorWebSocketService provides a Windows service with a WebSocket endpoint. To use this, the POS will have to implement a WebSocket client to communicate with the WebSocketService and receive callbacks.

To effectively work with the project you'll need:
- A computer or virtual machine running Windows. The SDK supports Windows 10, Windows 8, and Windows 7.
- An IDE, such as [Visual Studio](https://www.visualstudio.com/downloads/).

## Installers

There are four separate installers:
##### For Development:
  * CloverSDKSetup.exe - Installer that will lay down the dll, REST Clover Connector Service or WebSocket Clover Connector Service, along with the optional ExamplePOS application and source for testing.

##### For Deployment:
  * CloverUSBDriverSetup.exe - Clover device USB Driver installation only (automatically installed with the other installers)
  * CloverRESTServiceSetup.exe - Installer for the REST Clover Connector Service only
  * CloverWebSocketServiceSetup.exe - Installer for the WebSocket Clover Connector Service only

To complete a transaction end to end, we recommend getting a [Clover Mini Dev Kit](http://cloverdevkit.com/collections/devkits/products/clover-mini-dev-kit).


More documentation can be found at [Clover Docs Site](https://docs.clover.com/build/getting-started-with-cloverconnector/).

# Version 1.3.1
* Enhanced EventLog source creation methods to provide better message debugging
* Added support for starting Custom Activities and handling messaging with those activities
* Added example custom activity processing to the CloverExamplePOS application
* Added Device Status messaging to provide better insight into the current state of the connected payment device
* Added RetrievePayment request/response to provide the ability to query for a payment by external ID on the device
* Added some new starter examples to aid in quickly getting connected to a Clover device for a few simple scenarios
that originally attempted to process said payment.  Provides additional situational awareness after unexpected
disconnects with the payment device.
* Bug fixes for request validation handling
* Updates for the Secure Network Pay Display support and configuration
* General improvements to the CloverExamplePOS application

# Version 1.2.1
* Additional logging for connector communication and selected transport
* Added USB Device Hardening via persistent service debug options
* Corrected a bug in the definition of one of the internal message classes.  Customer API not impacted

# Version 1.2.0
* PreAuthRequest - no longer prompts for signature or signature verification.  Duplicate payment checking and print options can also be disabled if desired by setting the appropriate PreAuthRequest overrides of disablePrinting, disableDuplicateChecking & disableReceiptSelection to true.
* Changes to support certain transaction level overrides have been included in this version. To facilitate the addition of the new override capabilities, some new   options were added to the SaleRequest & TransactionRequest classes.
  * TransactionRequest - extended by SaleRequest, AuthRequest, PreAuthRequest & ManualRefundRequest
    * (Long) signatureThreshold was added to enable the override of the signature threshold in the Merchant settings for payments.
    * (DataEntryLocation) signatureEntryLocation was added to enable the override of the Signature Entry Location in the Merchant Signature Settings for Payments.  Value of NONE will cause the device to skip requesting a signature for the specified transaction.
    Possible values:
      * ON_SCREEN
      * ON_PAPER
      * NONE
    * (Boolean) disableReceiptSelection was added to enable bypassing the customer-facing receipt selection screen.
    * (Boolean) disableDuplicateChecking was added to enable bypassing any duplicate transaction logic and associated requests for confirmation.
    * (Boolean) autoAcceptPaymentConfirmations was added to enable the automatic acceptance of any payment confirmations that might be applicable for the given transaction (e.g. offline payment confirmation).  This override prevents any payment confirmation requests from being transmitted back to the calling program and continues processing as if a confirmPayment() was initiated by the caller.
    * (Boolean) autoAcceptSignature was added to enable the automatic acceptance of a signature (on screen or on paper) if applicable for the given transaction.  This override prevents signature confirmation requests from being transmitted back to the calling program and continues processing as if a acceptSignature() was initiated by the caller.
  * SaleRequest (extends TransactionRequest)
    * (TipMode) tipMode was added to specify the location from which to accept the tip.  You can now provide a tip up front or specify no tip to override the merchant configured (on screen/on paper) settings. **NOTE** If you desire to take the tip on paper, populate the signatureEntryLocation with ON_PAPER
    Possible values:
      * TIP_PROVIDED - tip is included in the request
      * ON_SCREEN_BEFORE_PAYMENT - valid when requested via Mini or Mobile
      * NO_TIP - tip will not be requested for this payment

# Version 1.1.0.3
  * Corrected an issue with handling large messages resulting in USB communication failures to connected devices.

# Version 1.1.0.2
  * Fix issue where requiresRemoteConfirmation was false

## Release Notes
# Version 1.1
* Renamed/Added/Removed a number of API operations and request/response objects to establish
  better consistency across platforms

  * ICloverConnector (Operations)
    * Added
      * PrintImageFromURL
      * InitializeConnection **(REQUIRED)**
      * AddCloverConnectorListener
      * RemoveCloverConnectorListener
      * AcceptPayment - (REQUIRED) Takes a payment object - possible response to a ConfirmPaymentRequest
      * RejectPayment - (REQUIRED) Takes a payment object and the challenge that was associated with
                        the rejection - possible response to a ConfirmPaymentRequest
      * RetrievePendingPayments - retrieves a list of payments that were taken offline and are pending
                                  server submission/processing.
    * Renamed
      * CapturePreAuth - formerly captureAuth
      * ShowDisplayOrder - formerly displayOrder - this is now the only operation needed
        to display/change order information displayed on the mini
      * RemoveDisplayOrder - formerly displayOrderDelete
    * Removed
      * DisplayOrderLineItemAdded - ShowDisplayOrder now handles this
      * DisplayOrderLineItemRemoved - ShowDisplayOrder now handles this
      * DisplayOrderDiscountAdded - ShowDisplayOrder now handles this
      * DisplayOrderDiscountRemoved - ShowDisplayOrder now handles this
    * Modified
      * SaleRequest, AuthRequest, PreAuthRequest and ManualRefund require ExternalId to be set. (REQUIRED)
        * ExternalId should be unique per transaction allowing the Clover device to detect, and potentially reject, if the same externalID is reused for subsequent transaction requests
      * changed all device action API calls to return void
      * CloverConnecter now requires ApplicationId to be set via configuration/installation of the third party application. This is provided as part of the device configuration that is passed in during the creation of the CloverConnector.
      * Behavior change for RefundPaymentRequest. In the prior versions, a value of zero for the amount field would trigger a refund of the full payment amount. With the 1.0 version, passing zero in the amount field will trigger a validation failure. Use FullRefund:boolean to specify a full refund amount. NOTE: This will attempt to refund the original (full) payment amount, not the remaining amount, in a partial refund scenario.    
  * ICloverConnectorListener (Notifications)
    * Added
      * OnPaymentConfirmation - (REQUIRED) consists of a Payment and a list of challenges/void reasons  
      * OnDeviceError - general callback when there is an error communicating with the device
      * OnPrintManualRefundReceipt - if disablePrinting=true on the request, this will get called to indicate the POS can print this receipt
      * OnPrintManualRefundDeclineReceipt - if disablePrinting=true on the request, this will get called to indicate the POS can print this receipt
      * OnPrintPaymentReceipt - if disablePrinting=true on the request, this will get called to indicate the POS can print this receipt
      * OnPrintPaymentDeclineReceipt - if disablePrinting=true on the request, this will get called to indicate the POS can print this receipt
      * OnPrintPaymentMerchantCopyReceipt - if disablePrinting=true on the request, this will get called to indicate the POS can print this receipt
      * OnPrintRefundPaymentReceipt - if disablePrinting=true on the request, this will get called to indicate the POS can print this receipt
      * OnRetrievePendingPaymentsResponse - called with the list of payments taken on the device that aren't processed on the server yet
    * Renamed
      * OnDeviceDisconnected - formerly onDisconnected
      * OnDeviceConnected - formerly on onConnected
      * OnDeviceReady - formerly onReady
      * OnTipAdjustAuthResponse - formerly onAuthTipAdjustResponse
      * OnCapturePreAuthResponse - formerly onPreAuthCaptureResponse
      * OnVerifySignatureRequest - formerly onSignatureVerifyRequest
    * Removed
      * OnTransactionState
      * OnConfigErrorResponse - These are now processed as normal operation responses
      * OnError - now handled by onDeviceError or through normal operation responses
      * OnDebug
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
* VoidPayment operation fix to verify connection status and check for void request
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

### Version 1.0.0.0
ICloverConnector
* Added InitializeConnection method that needs to be called directly after creation
  of the CloverConnector and prior to attempting to send any actions to the device
* Added PrintImageFromURL
* Removed VoidTransaction
* Changed all device action API calls to return void
* Method Name/Signature Changes
  * Changed SignatureVerifyRequest to VerifySignatureRequest.
  * Changed CaptureAuth to CapturePreAuth.
  * Changed CaptureAuthRequest to CapturePreAuthRequest.
  * Changed DisplayOrder to ShowDisplayOrder.
  * Changed DisplayOrderLineItemAdded to LineItemAddedToDisplayOrder (method arg order change).
  * Changed DisplayOrderLineItemRemoved to LineItemRemovedFromDisplayOrder (method arg order change).
  * Changed DisplayOrderDiscountAdded to DiscountAddedToDisplayOrder (method arg order change).
  * Changed DisplayOrderDiscountRemoved to DiscountRemovedFromDisplayOrder (method arg order change).
  * Changed DisplayOrderDelete to RemoveDisplayOrder.
  * Behavior change for RefundPaymentRequest.  In the prior versions, a value of zero for the amount
    field would trigger a refund of the full payment amount.  With the 1.0 version, passing zero
    in the amount field will trigger a validation failure.  Use FullRefund:boolean to specify a
    full refund amount. NOTE: This will attempt to refund the original (full) payment amount,
    not the remaining amount, in a partial refund scenario.
  * CloverConnecter now requires ApplicationId to be set via configuration/installation of the third party application. This is provided as part of the device configuration that is passed in during the creation of the CloverConnector.
  * SaleRequest, AuthRequest, PreAuthRequest and ManualRefund require ExternalId to be set.
    * ExternalId should be unique per transaction request and will prevent the Clover device
      from re-processing the last transaction.

ICloverConnectorListener
* Method Name/Signature Changes
  * Changed OnCaptureAuthResponse to OnCapturePreAuthResponse
  * Changed CaptureAuthResponse to CapturePreAuthResponse
  * Changed OnPaymentRefundResponse to OnRefundPaymentResponse
  * Changed PaymentRefundResponse to RefundPaymentResponse
  * Changed OnSignatureVerifyRequest to OnVerifySignatureRequest
  * Changed SignatureVerifyRequest to VerifySignatureRequest
  * Changed OnAuthTipAdjustResponse to OnTipAdjustAuthResponse
  * Changed OnDeviceReady to pass back the MerchantInfo
  * Removed OnConfigError. In this release, if a merchant attempts to perform an operation that their account is not configured to support (e.g. vaulting a card), then the SDK will call back with a failed transaction and an UNSUPPORTED result.
  * All Response Messages now return the following:​
    * Success:boolean
    * Result:enum [SUCCESS|FAIL|CANCEL|ERROR|UNSUPPORTED]
        FAIL - failed to process with values/properties as-is
        CANCEL - canceled, retry could work
        ERROR - un expected exception occurred
        UNSUPPORTED - merchant config won't allow the request
    * Reason:String optional information about result value, if not SUCCESS
    * Message:String optional detail information about the result value, if not success
* SaleResponse, AuthResponse and PreAuthResponse have 3 new flags (e.g. The payment gateway may
  force an AuthRequest to a SaleRequest)
  * IsSale:boolean - true if the payment is closed
  * IsAuth:boolean - true if the payment can be tip adjusted before closeout
  * IsPreAuth:boolean - true if the payment needs to be "captured" before closeout will close it


REST Service
* renamed the service to Clover Connector REST Service
* Enum values are now passed as Strings instead of Int

  REST Service Endpoints
     * Changed to return "" instead of zero to match DLL api
     * Added SDKInfo GET call returns the version Service's SDK information/version
     * Changed /CaptureAuth to /CapturePreAuth
     * Changed /DisplayOrderLineItemAdded to /LineItemAddeToDisplayOrder
     * Changed /DisplayOrderLineItemRemoved to /LineItemRemovedFromDisplayOrder
     * Changed /DisplayOrderDiscountAdded to /DiscountAddedToDisplayOrder
     * Changed /DisplayOrderDiscountRemoved to /DiscountRemovedFromDisplayOrder
     * Changed /DisplayOrderDelete to /RemoveDisplayOrder
  REST Callbacks
     * See notes above in ICloverConnectorListener about changes to payloads
     * Changed /SignatureVerifyRequest to /VerifySignatureRequest
     * Changed /CaptureAuthResponse to /CapturePreAuthResponse

### Version 0.6
* Add Closeout method support, which allows the initiation of an account closeout for any pending authorizations.
* Add extended validation of requests based on merchant/gateway configuration.  Passes back a new ConfigErrorResponse message for any related validation errors.
* Add PreAuth method to replace deprecated process of setting isPreAuth flag in an Auth request.  
* Changes to aid in the stability of the local REST service.
* Changes to usb transport code to aid in stability of the connection between the Clover device and the SDK transport.
* Add message field to the RefundResponse message to provide extended information on error responses.
* Add logging ability for the local REST service to aid in tracking/debugging of request/response message processing.
* Add onDeviceError methods for capturing communication issues detected by the transport layer during request/response message processing.
* Add new flags to enable/disable taking payments in an Offline mode, should gateway connectivity be interrupted.  
* 0.6 Installer can be downloaded here - [CloverSDKSetup.exe](https://github.com/clover/remote-pay-windows/releases/download/release-0.6/CloverSDKSetup.exe).

### Version 0.5
* Add VaultCard method which prompts for card information on the Clover device to be captured for later use with Sale/Auth VaultedCard functionality
* RefundResponse now supports `Code` attribute indicating whether response has been processed by device
* CaptureAuth method now fully supported

### Version 0.4
* Add CardEntryMethod to CloverConnector which allows customization of entry methods (contactless, EMV/contact, swipe, manual entry)
* Add a constructor to CloverConnector which takes a CloverDeviceConfiguration and a CloverConnectorListener
* Add AcceptSignature and RejectSignature methods to CloverConnector
* Add Cancel method to CloverConnector which should cancel the current card transaction whenever possible
* Add InvokeInputOption method to CloverConnector which allows the POS to control most screens on the device (excluding PIN entry)
* Payment.CardTransaction now returns a 'token' attribute, which will contain a TransArmor multi-pay token if the merchant is configured correctly
* SaleRequest now accepts a VaultedCard object, which allows for completion of a transaction using multi-pay token
* An installer ([CloverSDKSetup.exe](https://github.com/clover/remote-pay-windows/releases/download/release-0.5/CloverSDKSetup.exe)) is now available, which automatically installs appropriate USB drivers for Clover Mini; this has been tested on Windows 7 and above
* In addition to the .NET class libraries, there is now an option to connect to a WebSocket service or REST service (with socket callbacks) from the Windows PC.  This was added to provide better support for programming/integration environments other than .NET
* CloverExamplePOS application has been enhanced to allow testing with USB, REST service or WebSocket service, or an in-memory/mock 'test device'

### Version 0.3
* Add properties to CloverConnector: DisablePrinting, DisableCashBack, DisableTip, and DisableRestartTransactionOnFail
* Add Auth method, which allows for transactions that may be adjusted.  IsPreAuth flag on AuthRequest indicates a 'Pre-Auth', which must be captured via CaptureAuth method later
* Add TipAdjustAuth method, which allows for tip adjustment on Auth or PreAuth transactions
* Update VoidTransaction and add VoidPayment and RefundPayment
* Add Closeout method
* Add PrintText, PrintImage (prints on the built-in Clover Mini printer) and ShowMessage methods
* Add OpenCashDrawer method
* Add GetMerchantInfo method, which returns some basic info about the merchant
* Refactor all request/response objects and listeners

### Version 0.2

* Add methods for customer-facing order display on the CloverConnector library: ShowWelcomeScreen(), ShowThankYouScreen(), ShowReceiptScreen(), DisplayOrder(), DisplayOrderLineItemAdded(), DisplayOrderLineItemRemoved(), DisplayOrderDiscountAdded(), DisplayOrderDiscountRemoved(), DisplayOrderDelete()
* Add objects for displaying line items, orders, etc. to CloverWindowsSDK in `com.clover.remote.order` package
* Add method for PaymentRefund() (refunding an existing payment)
* Add functionality to CloverExamplePOS for displaying orders, line items, and discounts on the Clover Mini
* Add switcher for CloverExamplePOS to switch between "test" configuration and USB configuration

### Version 0.1

* Initial release




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
├── lib
│	└── CloverConnector                # CloverConnector module code
│	└── CloverWindowsSDK               # C# port of Clover Android SDK classes
│	└── CloverWindowsTransport         # Transport definitions (USB, WebSocket)
├── packages                           # Contains 3rd party dependencies
│	└── lib
└── services
	└── CloverWindowsSDKRESTService    # Source for Windows REST Service
	└── CloverWindowsWebSocketService  # Source for Windows WebSocket Service
	└── shared                         # Contains code shared by the Services
```

Building and running the Example POS application:

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
