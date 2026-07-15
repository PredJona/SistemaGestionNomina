$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
$binDir = Join-Path $projectRoot "bin\Debug\net48"
$projectPath = Join-Path $projectRoot "ProyFinal_LPI_Eq01_NomiCore.csproj"
$appPath = Join-Path $binDir "ProyFinal_LPI_Eq01_NomiCore.exe"
$sqlitePath = Join-Path $binDir "System.Data.SQLite.dll"
$hostSource = Join-Path $PSScriptRoot "Fase4RrhhAvanzadoSmokeHost.cs"
$hostExe = Join-Path $binDir "Fase4RrhhAvanzadoSmokeHost.exe"
$tempRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("Proy2_Fase4_" + [Guid]::NewGuid().ToString("N"))
$dbPath = Join-Path $tempRoot "nomina_fase4.db"
$previousDbPath = [Environment]::GetEnvironmentVariable("NOMINA_DB_PATH")

try {
    [System.IO.Directory]::CreateDirectory($tempRoot) | Out-Null
    dotnet build $projectPath --no-restore --nologo
    if ($LASTEXITCODE -ne 0) { throw "La compilación del proyecto falló." }
    $csc = Join-Path $env:WINDIR "Microsoft.NET\Framework64\v4.0.30319\csc.exe"
    if (-not (Test-Path -LiteralPath $csc)) { $csc = Join-Path $env:WINDIR "Microsoft.NET\Framework\v4.0.30319\csc.exe" }
    & $csc /nologo /target:exe "/out:$hostExe" "/reference:$appPath" "/reference:$sqlitePath" $hostSource
    if ($LASTEXITCODE -ne 0) { throw "No se pudo compilar el host real de Fase 4." }
    Copy-Item -LiteralPath ($appPath + ".config") -Destination ($hostExe + ".config") -Force
    [Environment]::SetEnvironmentVariable("NOMINA_DB_PATH", $dbPath)
    & $hostExe $dbPath
    if ($LASTEXITCODE -ne 0) { throw "El smoke test de Fase 4 devolvió código $LASTEXITCODE." }
    Write-Host "Fase 4 RRHH Avanzado Smoke Test OK" -ForegroundColor Green
}
finally {
    [Environment]::SetEnvironmentVariable("NOMINA_DB_PATH", $previousDbPath)
    if (Test-Path -LiteralPath $hostExe) { Remove-Item -LiteralPath $hostExe -Force }
    if (Test-Path -LiteralPath ($hostExe + ".config")) { Remove-Item -LiteralPath ($hostExe + ".config") -Force }
    if (Test-Path -LiteralPath $tempRoot) {
        $resolved = [System.IO.Path]::GetFullPath($tempRoot)
        $tempBase = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath())
        if ($resolved.StartsWith($tempBase, [System.StringComparison]::OrdinalIgnoreCase)) {
            Remove-Item -LiteralPath $resolved -Recurse -Force -ErrorAction SilentlyContinue
        }
    }
}
