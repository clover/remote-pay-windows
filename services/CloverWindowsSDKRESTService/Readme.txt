
To Install the service, Run 'Command Prompt' as Administrator. Right click on Start > All Programs > Accessories > Command Prompt [Run as Adminstrator]
c:> InstallUtil.exe CloverWindowsSDKRESTService.exe
the service defaults to port 8181, to change the default, the service must be installed specifying the port
c:> InstallUtil.exe /Port=8765 CloverWindowsSDKRESTService.exe
the default callback is to port 8182, to change the callback endpoint, the service must be installed specifying the endpiont
c:>InstallUtil.exe /CallbackEndpoint=http://localhost:8585/MyService
the default device connection is via USB, to change to the LAN endpoint, the service must be installed specifying the LAN endpoint of the device
c:>InstallUtil.exe /LANHost=192.168.1.55:14631

InstallUtil.exe will be installed in the .NET Framework directory. e.g. c:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe

The full path to CloverWindowsSDKWebSocketService.exe is required if you are not in the directory containing the executable.