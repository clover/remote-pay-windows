This document outlines the steps and expectations
for installing the Clover USB Windows device drivers
on a Windows 7 platform.

Expectation 1: LibUsbDotNet has been installed to
the default location of C:\Program Files\LibUsbDotNet
and that during installation, the libusb-win32 filter
server was also installed.

For successful configuration and device installation,
the Clover Mini device will need to be transitioned
from a standard Clover Merchant Facing device into
a Clover Customer Facing device.  Only after the
Mini has been successfully transitioned can the 
device drivers for the Customer Mode be installed.

The install_clover_mini.cmd file will step you through
the process of installing the Merchant Facing drivers,
transitioning the device using the demo application, 
then installing the customer facing drivers, and
finally: re-launching the demo application and 
processing a credit card transaction through your
Clover device.

NOTE:  by default we expect that install-filter.exe 
is located in:
C:\Program Files\LibUsbDotNet\libusb-win32

ALSO: by default we expect that the clover *.inf files
and related file structure are located under:
C:\Software\win-7
(Copying the win-7 directory into C:\Software will
ensure that the required files are present)

Before running install_clover_mini.cmd, make sure 
that the CloverExamplePOS.exe is accessible, but not
running.

Finally:
When running install_clover_mini.cmd, make sure to use
the "Run as Administrator" option when launching cmd.exe
