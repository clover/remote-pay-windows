# Clover SDK for Windows PoS Integration

Current version: 0.6

## Overview

This SDK provides an API with which to allow your Windows-based Point-of-Sale (POS) system to interface with a Clover® Mini device (https://www.clover.com/pos-hardware/mini)

The API is currently distributed in three class libraries

1. CloverConnector is the high-level API with methods like `Sale()`, `VoidTransaction()`, `ManualRefund()`, etc.
2. CloverWindowsSDK contains classes that map to standard Clover objects such as `Payment`, `CardTransaction`, `Order`, etc.  These objects will match those defined in [clover-android-sdk](https://github.com/clover/clover-android-sdk) and the objects returned by the [Clover REST API](https://www.clover.com/api_docs)
3. CloverWindowsTransport contains functionality to interface with a Clover Mini device via USB or via LAN (WebSocket connection). You may also simulate a Clover device (`CloverTestDevice`) so no connectivity is required.

The libraries currently require .NET 4.0 or higher, and are supported on Windows POSReady 2009, Windows 7 and Windows 8.

An example project (CloverExamplePOS) is provided to demonstrate how to interact with the APIs.  To open this, please use Visual Studio and open `Clover.sln`

An installer is now available under the 'Releases' tab, which will automatically install the appropriate drivers needed to connect to Clover Mini.

Please report back to us any questions/comments/concerns.

## Release Notes

### Version 0.6
* Add Closeout method support, which allows the initiation of an account closeout for any pending authorizations
* Add extended validation of requests based on merchant/gateway configuration.  Passes back a new ConfigErrorResponse message for any related validation errors.
* Add PreAuth method to replace deprecated process of setting isPreAuth flag in an Auth request.  
* Changes to aid in the stability of the local REST service 
* Changes to usb transport code to aid in stability of the connection between the Clover device and the SDK transport
* Add message field to the RefundResponse message to provide extended information on error responses
* Add logging ability for the local REST service to aid in tracking/debugging of request/response message processing
* Add onDeviceError methods for capturing communication issues detected by the transport layer during request/response message processing
* Add new flags to enable/disable taking payments in an Offline mode, should gateway connectivity be interrupted  
* ([CloverSDKSetup.exe](https://github.com/clover/remote-pay-windows/releases/download/release-0.6/CloverSDKSetup.exe))

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
* CloverExamplePOS application has been enhanced to allow testing with USB, LAN, REST service or WebSocket service, or an in-memory/mock 'test device'

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

##Disclaimer

This is a beta release and will not be supported long term. There may be breaking changes in the general release, which is coming soon. 
