$ErrorActionPreference = 'Stop'; # stop on all errors
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$url        = 'https://github.com/Wissance/Zerial/raw/master/app/Wissance.Zerial/Wissance.Zerial.Installer/Windows/Wissance.Zerial.WinInstaller.Bootstrap.X86.exe' # download url, HTTPS preferred
$url64      = 'https://github.com/Wissance/Zerial/raw/master/app/Wissance.Zerial/Wissance.Zerial.Installer/Windows/Wissance.Zerial.WinInstaller.Bootstrap.X64.exe' # 64bit URL here (HTTPS preferred) or remove - if installer contains both (very rare), use $url
$dotnetPath = $Env:SystemDrive + "\'Program Files'\dotnet\"

$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  unzipLocation = $toolsDir
  fileType      = 'EXE'
  url           = $url
  url64bit      = $url64

  softwareName  = 'Wissance.Zerial'

  checksum      = 'C1E2E9A5B7EEF3C64894B8247E539B6E5F6DE0EE47B4C18DC1A9B6BB032F8B18'
  checksumType  = 'sha256'
  checksum64    = '3EA0AB7BBCA0A19A885316306D2A95EC7A6DC3416E40188C637A07D72908E8FC'
  checksumType64= 'sha256'

  silentArgs    = "/SILENT /qn /norestart /l*v `"$($env:TEMP)\$($packageName).$($env:chocolateyPackageVersion).MsiInstall.log`"" # ALLUSERS=1 DISABLEDESKTOPSHORTCUT=1 ADDDESKTOPICON=0 ADDSTARTMENU=0
  validExitCodes= @(0, 3010, 1641)
}

Set-ExecutionPolicy Bypass -Scope Process
Invoke-WebRequest -Uri https://dot.net/v1/dotnet-install.ps1 -OutFile "dotnet-install.ps1"
powershell ".\dotnet-install.ps1 -InstallDir $dotnetPath -Runtime dotnet -Version 6.0.27"

$env:Path += '$dotnetPath' 
Install-ChocolateyPackage @packageArgs
