param (
    [switch]$release
)

Set-Location .\Mumbleline

dotnet build -c $(If($release) { "release" } else { "debug"} )

Set-Location ..

$buildDir = If($release) { "Release" } else { "Debug"}
$compress = @{
    Path = ".\Mumbleline\everest.yaml", ".\Mumbleline\Dialog", ".\Mumbleline\bin\$buildDir\Mumbleline.dll", ".\Mumbleline\bin\$buildDir\MumbleLinkSharp.dll"
    CompressionLevel = "Optimal"
    DestinationPath = "mumbleline-$(If($release) { "release" } else { "debug"} ).zip"
}

Compress-Archive -Update @compress