build: dotnet publish -c Release -r win-x64 --self-contained
create service: New-Service -Name "LibreHardwareMonitorExporterService" -Binary "C:\Users\angge\projects\librehardwaremonitor-exporter\bin\Release\net8.0\win-x64\librehardwaremonitor-exporter.exe" -DisplayName "LibreHardwareMonitor Exporter Service" -StartupType Automatic -Credential (Get-Credential)
start service: Start-Service -Name "LibreHardwareMonitorExporterService"

got error:
```
New-Service : Service 'LibreHardwareMonitor Exporter Service (LibreHardwareMonitorExporterService)' cannot be created
due to the following error: The specified domain either does not exist or could not be contacted
At line:1 char:1
+ New-Service -Name "LibreHardwareMonitorExporterService" -Binary "C:\U ...
+ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : PermissionDenied: (LibreHardwareMonitorExporterService:String) [New-Service], ServiceCom
   mandException
    + FullyQualifiedErrorId : CouldNotNewService,Microsoft.PowerShell.Commands.NewServiceCommand
```

debug: eventvwr.msc -> windows logs -> System:
```
A timeout was reached (30000 milliseconds) while waiting for the LibreHardwareMonitor Exporter Service service to connect.

The LibreHardwareMonitor Exporter Service service failed to start due to the following error: 
The service did not respond to the start or control request in a timely fashion.
```

try increasing timeout : https://answers.microsoft.com/en-us/windows/forum/all/a-timeout-was-reached-30000-milliseconds-while/3f6ca683-1f88-4a9f-9f4f-cfb59a6967a3 -> regedit -> restart