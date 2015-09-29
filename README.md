# Clover SDK for Windows PoS Integration

Current version: 0.2

## Overview

This SDK provides an API with which to allow your Windows-based Point-of-Sale (POS) system to interface with a Clover® Mini device (https://www.clover.com/pos-hardware/mini)

The API is currently distributed in three class libraries

1. CloverConnector is the high-level API with methods like `Sale()`, `VoidTransaction()`, `ManualRefund()`, etc.
2. CloverWindowsSDK contains classes that map to standard Clover objects such as `Payment`, `CardTransaction`, `Order`, etc.  These objects will match those defined in [clover-android-sdk](https://github.com/clover/clover-android-sdk) and the objects returned by the [Clover REST API](https://www.clover.com/api_docs)
3. CloverWindowsTransport contains functionality to interface with a Clover Mini device via USB, though this version simulates a Clover device (`CloverTestDevice`) so no connectivity is required.

The libraries currently require .NET 2.0 or higher, and are supported on Windows POSReady 2009, Windows 7 and Windows 8.

An example project (CloverExamplePOS) is provided to demonstrate how to interact with the APIs.  To open this, please use Visual Studio and open `Clover.sln`

Please report back to us any questions/comments/concerns.

## Release Notes

### Version 0.2

* Add methods for customer-facing order display on the CloverConnector library: ShowWelcomeScreen(), ShowThankYouScreen(), ShowReceiptScreen(), DisplayOrder(), DisplayOrderLineItemAdded(), DisplayOrderLineItemRemoved(), DisplayOrderDiscountAdded(), DisplayOrderDiscountRemoved(), DisplayOrderDelete()
* Add objects for displaying line items, orders, etc. to CloverWindowsSDK in `com.clover.remote.order` package
* Add method for PaymentRefund() (refunding an existing payment)
* Add functionality to CloverExamplePOS for displaying orders, line items, and discounts on the Clover Mini
* Add switcher for CloverExamplePOS to switch between "test" configuration and USB configuration

### Version 0.1

* Initial release

