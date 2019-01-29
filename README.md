![Clover logo](https://www.clover.com/assets/images/public-site/press/clover_primary_gray_rgb.png)
Clover SDK for Windows POS integration
======================================

Version
-------
Current version: 3.0.2

Overview
--------
This SDK enables your Windows point-of-sale (POS) system to communicate with a [Clover payment device](https://www.clover.com/pos-systems). Learn more about [Clover integrations](https://www.clover.com/integrations).

**Platforms supported:**
-   Windows 10
-   Windows 8
-   Windows 7

**.NET Frameworks supported:**
-   .NET 4.5
-   .NET 4.0 for backwards compatibility

Connecting to a Clover device
-----------------------------
The .NET Assembly(dll) - CloverConnector.DLL can be used directly by a .NET application. This option requires .NET 4.0 or 4.5. 

Clover devices (both production and DevKit devices) are connected to Microsoft Windows either by a USB connection or over the network. Clover USB drivers (found in the [releases page](https://github.com/clover/remote-pay-windows/releases)) are required for a USB connection.


To work with the project effectively, you will need:
-   A computer or virtual machine running Windows. The SDK supports Windows 10, Windows 8, and Windows 7.
-   An IDE, such as [Visual Studio](https://www.visualstudio.com/downloads/).
-   To experience transactions end-to-end from the merchant and customer perspectives, we also recommend ordering a [Clover DevKit](https://cloverdevkit.com/collections/devkits/products/clover-mini-2nd-gen-developer-kit).

NuGet Package
-------------
Use NuGet references to easily include Remote Pay Windows in your .NET project
<https://www.nuget.org/packages/Clover.RemotePayWindows>
```
Install-Package Clover.RemotePayWindows -Version 3.0.2
```

Installing the SDK and Services
-------------------------------
There are two sets of binaries supplied, targeting different .NET Frameworks, the .NET 4.5 and .NET 4.0 backwards compatible.
-   The .NET 4.5 binaries are able to use TLS 1.2 network security when talking to Clover Devices using [Secure Network Pay Display](https://docs.clover.com/build/secure-network-pay-display/).
-   The .NET 4.0 binaries are the same platform as the previous releases of the Clover Windows SDK, and are provided for backwards compatibility.

### For development:
-   CloverSDK.exe - This file will install the DLL. It will also install the example POS application and source for testing.

### For deployment:
-   CloverUSBDrivers.exe - This file installs the USB drivers for the Clover devices.

Building the SDK
----------------
The SDK has the following structure.
```
.
├── examples                           # Contains the C# Example application
│   └── CloverExamplePOS
│   └── exampleMessages
├── lib
│   └── CloverConnector                # CloverConnector module code
│   └── CloverWindowsSDK               # C# port of Clover Android SDK classes
│   └── CloverWindowsTransport         # Transport definitions (USB, WebSocket)
└── packages                           # Contains third-party dependencies
    └── lib
```

Working with the Example POS
----------------------------
To build and run the example POS application using .NET and VisualStudio 2015:
1.  Open **Clover.sln**.
2.  Build all projects.
3.  Select the **CloverExamplePOS**.
4.  Click **Run**.

Additional resources
--------------------
-   [Release Notes](https://github.com/clover/remote-pay-windows/releases)
-   [Windows SDK Wiki](https://github.com/clover/remote-pay-windows/wiki)
-   [Secure Network Pay Display](https://docs.clover.com/build/secure-network-pay-display/)
-   [Tutorial for the Windows SDK](https://docs.clover.com/build/getting-started-with-cloverconnector/?sdk=windows)
-   [API Documentation](http://clover.github.io/remote-pay-windows/3.0.2/cloverconnector/html/index.html)
-   [Clover Developer Community](https://community.clover.com/index.html)
