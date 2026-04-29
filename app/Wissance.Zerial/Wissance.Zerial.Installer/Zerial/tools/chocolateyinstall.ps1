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

  softwareName  = 'Wissance.Zerial'

  checksum      = '3CD7AB506E2ECA55DD226656016DFDCD5E7A035E2650ED331E090F19C6B02C86'
  checksumType  = 'sha256'
  checksum64    = '8152D1295F0841432EE0754131826EB489F013C865A287F1AE4730FE5292D4FC'
  checksumType64= 'sha256'

  silentArgs    = "/SILENT /qn /norestart /l*v `"$($env:TEMP)\$($packageName).$($env:chocolateyPackageVersion).MsiInstall.log`"" # ALLUSERS=1 DISABLEDESKTOPSHORTCUT=1 ADDDESKTOPICON=0 ADDSTARTMENU=0
  validExitCodes= @(0, 3010, 1641)
}
# skip dotnet install if there is a newer version
$dotnetVersion = (Get-ItemProperty -Path "HKLM:\SOFTWARE\dotnet\Setup\InstalledVersions\x64" -Name "MostRecentVersion" -ErrorAction SilentlyContinue).MostRecentVersion
if ($dotnetVersion -and [version]$dotnetVersion -ge [version]"6.0.0") {
    Write-Host ".NET $dotnetVersion was already installed"
    $installDotnet = $false

Install-ChocolateyPackage @packageArgs
