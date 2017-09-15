
To Install the service, Run 'Command Prompt' as Administrator. Right click on Start > All Programs > Accessories > Command Prompt [Run as Adminstrator]
c:> InstallUtil.exe CloverWindowsSDKWebSocketService.exe
the service defaults to port 8889, to change the default the service must be installed specifying the port
c:> InstallUtil.exe /Port=8765 CloverWindowsSDKWebSocketService.exe

InstallUtil.exe will be installed in the .NET Framework directory. e.g. c:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe

The full path to CloverWindowsSDKWebSocketService.exe is required if you are not in the directory containing the executable.