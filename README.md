# StandupTimer


## Description
Tool to keep Scrum stand-ups timing at bay


## Run from File Explorer

By default solution is configured to run only from inside VS and generate an installer.

To run executable directly from file explorer change as follows:
- Project file : `<WindowsPackageType>None</WindowsPackageType>`
- Properties\launchSettings.json : `"commandName": "Project",`


## Generate installer MSIX pacakge (from commandline)

To build an installer change project configuration as follows:
- Project file : comment out line `<WindowsPackageType>None</WindowsPackageType>`
- Properties\launchSettings.json : `"commandName": "MsixPackage",`

The installer needs to be signed. Can use a selft-signed certificate as follows:
- In powershell, create certificate running command:
  ```
  New-SelfSignedCertificate -Type Custom -Subject "CN=GFGM" -KeyUsage DigitalSignature -FriendlyName "GFGMCertf" -CertStoreLocation "Cert:\CurrentUser\My" -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.3", "2.5.29.19={text}")
	
  Results
	Thumbprint                                Subject
	----------                                -------
	F8FA0E5950AFD603F24D9A7FED8A8782E10A022B  CN=GFGM
  ```	
  
Generate the installer as follows:
- In VS terminal, go to solution folder and run command:
  ```
  dotnet publish -f net7.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win10-x64 -p:PackageCertificateThumbprint=F8FA0E5950AFD603F24D9A7FED8A8782E10A022B
  ```
  
Find installer at

`StandupTimer\StandupsTimer\bin\Release\net7.0-windows10.0.19041.0\win10-x64\AppPackages\StandupTimer_1.0.0.0_Test\StandupTimer_1.0.0.0_x64.msix`

Check if the msix file is digitally signed:
- In file explorer, look for digital signature tab
- If not present, need to manually add the signature
- Export the certificate from powershell
	$password = ConvertTo-SecureString -String standup-timer-gfgm -Force -AsPlainText
	Export-PfxCertificate -cert "Cert:\CurrentUser\My\F8FA0E5950AFD603F24D9A7FED8A8782E10A022B" -FilePath gfgm-certf.pfx -Password $password


ERRORR
For the moment (2023-9-24) the msix file doesnt get the digital signature on the prev process ... ???
