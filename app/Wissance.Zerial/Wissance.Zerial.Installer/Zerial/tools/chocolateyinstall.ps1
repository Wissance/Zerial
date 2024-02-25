$ErrorActionPreference = 'Stop'; # stop on all errors
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$url        = 'https://github.com/Wissance/Zerial/raw/master/app/Wissance.Zerial/Wissance.Zerial.Installer/Windows/Wissance.Zerial.WinInstaller.Bootstrap.X86.exe' # download url, HTTPS preferred
$url64      = 'https://github.com/Wissance/Zerial/raw/master/app/Wissance.Zerial/Wissance.Zerial.Installer/Windows/Wissance.Zerial.WinInstaller.Bootstrap.X64.exe' # 64bit URL here (HTTPS preferred) or remove - if installer contains both (very rare), use $url

$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  unzipLocation = $toolsDir
  fileType      = 'EXE'
  url           = $url
  url64bit      = $url64

  softwareName  = 'Zerial'

  checksum      = 'D4ACDFABEA3210D5EA103E9D2DE284C2271E575C141A6F11E53E546C38B5A81B'
  checksumType  = 'sha256'
  checksum64    = '3490E64CFA63AA767B55D78F63B3BE10A7C4900F3D0E486B3207BBBB1EB31129'
  checksumType64= 'sha256'

  silentArgs    = "/SILENT /qn /norestart /l*v `"$($env:TEMP)\$($packageName).$($env:chocolateyPackageVersion).MsiInstall.log`"" # ALLUSERS=1 DISABLEDESKTOPSHORTCUT=1 ADDDESKTOPICON=0 ADDSTARTMENU=0
  validExitCodes= @(0, 3010, 1641)
}

choco install dotnet-6.0-runtime -y

Install-ChocolateyPackage @packageArgs
