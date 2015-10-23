set LIB_USB_LOC=C:\Program Files
set WIN_7_INF_FILES=c:\software\win-7
echo off
cls
echo Make sure you are running this from %WIN_7_INF_FILES%
echo Make sure you are running in Administrator mode
echo Make sure LibUsbDotNet is installed
echo (http://sourceforge.net/projects/libusbdotnet/files/LibUsbDotNet/LibUsbDotNet%20v2.2.8/)
pause
ECHO Make sure your Clover Mini is un-plugged
pause
ECHO Now plug in the Clover Mini
pause
"%LIB_USB_LOC%\LibUsbDotNet\libusb-win32\install-filter" install --inf=%WIN_7_INF_FILES%\clover-merchant.inf
"%LIB_USB_LOC%\LibUsbDotNet\libusb-win32\install-filter" install --device="USB\Vid_28f3&Pid_3000"
ECHO If you saw your device successfully installed, continue
pause
ECHO Un-plug the Clover Mini
pause
ECHO Launch the CloverExamplePOS app and switch to USB mode
pause
echo Plug in the Clover Mini
pause 
"%LIB_USB_LOC%\LibUsbDotNet\libusb-win32\install-filter" install --inf=%WIN_7_INF_FILES%\clover-customer.inf
"%LIB_USB_LOC%\LibUsbDotNet\libusb-win32\install-filter" install --device="USB\Vid_28f3&Pid_3004&MI_00"
echo Un-plug your Clover Mini
pause
echo Restart the CloverExamplePOS app and switch to USB mode
pause
echo Plug in the Clover Mini
pause
echo If the CloverExamplePOS app displays 'connected', you may attempt to run a transaction
pause
echo goodbye