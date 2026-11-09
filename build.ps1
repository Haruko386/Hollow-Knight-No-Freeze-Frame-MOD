param(
    [string]$GamePath = 'E:\SteamLibrary\steamapps\common\Hollow Knight',
    [switch]$Install
)

$ErrorActionPreference = 'Stop'
$project = Join-Path $PSScriptRoot 'src\NoFreezeFrame\NoFreezeFrame.csproj'
& dotnet build $project -c Release "-p:GamePath=$GamePath" --nologo
if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }

$package = Join-Path $PSScriptRoot 'artifacts\NoFreezeFrame'
New-Item -ItemType Directory -Force -Path $package | Out-Null
$dll = Join-Path $PSScriptRoot 'src\NoFreezeFrame\bin\Release\net472\NoFreezeFrame.dll'
Copy-Item -LiteralPath $dll -Destination $package -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'README.md') -Destination $package -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'LICENSE') -Destination $package -Force
Compress-Archive -LiteralPath $package -DestinationPath (Join-Path $PSScriptRoot 'artifacts\NoFreezeFrame-1.0.0.zip') -Force

if ($Install) {
    if (Get-Process -Name hollow_knight -ErrorAction SilentlyContinue) {
        throw 'Close Hollow Knight before installing. The built package is available in artifacts.'
    }
    $mods = Join-Path $GamePath 'hollow_knight_Data\Managed\Mods'
    if (!(Test-Path -LiteralPath $mods -PathType Container)) { throw "Mods directory not found: $mods" }
    $destination = Join-Path $mods 'NoFreezeFrame'
    New-Item -ItemType Directory -Force -Path $destination | Out-Null
    Copy-Item -LiteralPath $dll -Destination $destination -Force
    Write-Output "Installed: $destination\NoFreezeFrame.dll"
}
Write-Output "Package: $PSScriptRoot\artifacts\NoFreezeFrame-1.0.0.zip"
