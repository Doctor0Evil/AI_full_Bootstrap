# BuildAndDeploy.ps1 - Build and deploy script for Windows/Linux environments

param(
    [string]$Configuration = "Release",
    [string]$OutputDir = "./deploy"
)

Write-Host "Starting build process..."

# Clean output directory
if (Test-Path $OutputDir) { Remove-Item $OutputDir -Recurse -Force }
New-Item -ItemType Directory -Path $OutputDir

# Build solution
dotnet build UniversalAI.sln -c $Configuration -o $OutputDir

if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed. Aborting deployment."
    exit 1
}

Write-Host "Build succeeded. Copying plugins..."

# Copy plugins
Copy-Item -Path "./plugins/*" -Destination "$OutputDir/plugins" -Recurse

Write-Host "Deployment artifacts prepared at $OutputDir"
