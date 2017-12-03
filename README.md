![Clover logo](https://www.clover.com/assets/images/public-site/press/clover_primary_gray_rgb.png)

# Clover SDK for Windows POS integration

## Version

Current version: 1.4.0

## Overview

This SDK enables your Windows point-of-sale (POS) system to communicate with a [Clover® payment device](https://www.clover.com/pos-hardware/). Learn more about [Clover integrations](https://www.clover.com/integrations).

**Platforms supported:**
  * Windows 10
  * Windows 8
  * Windows 7
  
## Connecting to a Clover device 

The Windows project includes three options for connecting to a Clover device. 

1. .NET DLL - CloverConnector.DLL can be used directly by a .NET application. This option requires .NET 4.0.
2. Clover Connector REST API Service - This option requires your POS to implement a REST service in order to receive callbacks.
3. Clover Connector WebSocket API Service - This option requires your POS to implement a WebSocket client in order to communicate with the service and receive callbacks.

To work with the project effectively, you will need:
- A computer or virtual machine running Windows. The SDK supports Windows 10, Windows 8, and Windows 7.
- An IDE, such as [Visual Studio](https://www.visualstudio.com/downloads/).
- To experience transactions end-to-end from the merchant and customer perspectives, we also recommend ordering a [Clover DevKit](http://cloverdevkit.com/collections/devkits/products/clover-mini-dev-kit).


## Installing the SDK and Services


There are four separate installers.

### For development:

  * CloverSDKSetup.exe - This file will install the DLL, as well as the Clover Connector REST Service or Clover Connector WebSocket Service. It will also install the example POS application and source for testing.

### For deployment:

  * CloverUSBDriverSetup.exe - This file installs the USB driver for the Clover device only.
  * CloverRESTServiceSetup.exe - This file installs the REST Clover Connector Service and the USB driver for the Clover device.
  * CloverWebSocketServiceSetup.exe - This file installs the WebSocket Clover Connector Service and the USB driver for the Clover device.

## Building the SDK

The SDK has the following structure. 

```
.
├── examples                           # Contains the C# Example application
│	└── CloverExamplePOS
│	└── exampleMessages
├── lib
│	└── CloverConnector                # CloverConnector module code
│	└── CloverWindowsSDK               # C# port of Clover Android SDK classes
│	└── CloverWindowsTransport         # Transport definitions (USB, WebSocket)
├── packages                           # Contains third-party dependencies
│	└── lib
└── services
	└── CloverWindowsSDKRESTService    # Source for the Windows REST Service
	└── CloverWindowsWebSocketService  # Source for the Windows WebSocket Service
	└── shared                         # Contains code shared by the Services
```

## Working with the Example POS

To build and run the example POS application using .NET and VisualStudio 2015:
 1. Open **Clover.sln**
 2. Build all projects
 3. Select the **CloverExamplePOS**
 4. Click **Run**

## Setting up a virtal machine

1. Install [VMWare Fusion](https://www.vmware.com/products/fusion.html). (NOTE: VirtualBox will not see the device properly.)
2. Run the driver installers.

## Debugging

To debug the device in TCP/IP using Android Debug Bridge (adb):
1. Plug the device into a laptop, then select **Connect to Mac**. This will set the device up for debugging on port 5555.
2. Enter `>adb tcpip 5555` on the command line.

NOTE: If you see the message `error: more than one device and emulator`, you will need to specify the device by ID. To do this: 

3. Enter `>adb devices` on the command line. This will retrieve a list of available devices similar to the one below.

	```
	List of devices attached
	C010UC43040345	device
	C030UQ50550081	device
	```

4. Modify the command from step 2 to include the ID for the device you want to set up. 
	
	**Example:** `>adb -s C030UQ50550081 tcpip 5555`
	
	This should cause the device to disconnect. 
	
5. Select **Connect to Windows**.
6. Reconnect to the device using this command:
	
	`>adb connect 192.168.1.3:5555`

7. Enter `adb logcat`, or connect with the debugger via IP.

	Example: `>adb devices`
	
	Your results will look like this: 
	
	```
	List of devices attached
	C010UC43040345	device
	192.168.1.3:5555 device
	```
## Additional resources 

- [Release Notes](https://github.com/clover/remote-pay-windows/releases)
- [Tutorial for the Windows SDK](https://docs.clover.com/build/getting-started-with-cloverconnector/?sdk=windows)
- [API Documentation](http://clover.github.io/remote-pay-windows/1.4.0/cloverconnector/html/index.html)
- [Clover Developer Community](https://community.clover.com/index.html)
	
