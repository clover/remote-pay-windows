# Clover SDK for Windows PoS Integration

Current version: 1.0.0.0

## Overview

This SDK provides an API to allow your Windows-based Point-of-Sale (POS) system to interface with a Clover® Mini device (https://www.clover.com/pos-hardware/mini)

The API is currently distributed in three class libraries

1. CloverConnector is the high-level API with methods like `Sale()`, `VoidTransaction()`, `ManualRefund()`, etc.
2. CloverWindowsSDK contains classes that map to standard Clover objects such as `Payment`, `CardTransaction`, `Order`, etc.  These objects will match those defined in  [clover-android-sdk](https://github.com/clover/clover-android-sdk) and the objects returned by the [Clover REST API](https://www.clover.com/api_docs).
3. CloverWindowsTransport contains functionality to interface with a Clover Mini device via USB. You may also simulate a Clover device (`CloverTestDevice`) so no connectivity is required.

The libraries currently require .NET 4.0 or higher, and are supported on Windows POSReady 2009, Windows 7 and Windows 8.

An example project (CloverExamplePOS) is provided to demonstrate how to interact with the APIs.  To open this, please use Visual Studio and open `Clover.sln`.

An installer is now available under the 'Releases' tab, which will automatically install the appropriate drivers needed to connect to Clover Mini.

Please report back to us any questions/comments/concerns, by emailing semi-integrations@clover.com. 

For more developer documentation and information about the Semi-Integration program, please visit our [semi-integration developer documents] (https://docs.clover.com/build/integration-overview-requirements/). 

## Release Notes

### Version 1.0.0.0

There are now four separate installers:
#####For Development:
  * CloverSDKSetup.exe - Installer that will lay down the dll, REST Clover Connector Service or WebSocket Clover Connector Service, along with the optional ExamplePOS application and source for testing.
  
 
#####For Deployment: 
  * CloverUSBDriverSetup.exe - Clover device USB Driver installation only (automatically installed
                               with the other installers)
  * CloverRESTServiceSetup.exe - Installer for the REST Clover Connector Service only
  * CloverWebSocketServiceSetup.exe - Installer for the WebSocket Clover Connector Service only
  

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

