$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
$binDir = Join-Path $projectRoot "bin\Debug\net48"
$exePath = Join-Path $binDir "SistemaGestionNomina.exe"
$backupDir = Join-Path ([System.IO.Path]::GetTempPath()) "SistemaGestionNominaSmokeBackups"

[AppDomain]::CurrentDomain.add_AssemblyResolve({
    param($sender, $args)

    $assemblyName = ($args.Name -split ",")[0]
    $candidate = Join-Path $binDir ($assemblyName + ".dll")
    if (Test-Path -LiteralPath $candidate) {
        return [System.Reflection.Assembly]::LoadFrom($candidate)
    }

    return $null
})

$assembly = [System.Reflection.Assembly]::LoadFrom($exePath)
$initializer = $assembly.GetType("SistemaGestionNomina.Data.DatabaseInitializer")
$initializer.GetMethod("Initialize").Invoke($null, @())

$roleType = $assembly.GetType("SistemaGestionNomina.Services.RolePermissionService")
$roleService = [Activator]::CreateInstance($roleType)
if (-not $roleType.GetMethod("TienePermiso", [type[]]@([string], [string], [string])).Invoke($roleService, @("Admin", "nomina", "confirmar"))) {
    throw "RolePermissionService no autorizó Admin."
}
Write-Host "RolePermissionService OK"

$reportType = $assembly.GetType("SistemaGestionNomina.Services.AdvancedReportBuilder")
$reportBuilder = [Activator]::CreateInstance($reportType)
$report = $reportType.GetMethod("ConstruirReportePersonalizado").Invoke($reportBuilder, @("Smoke avanzado"))
if ([string]::IsNullOrWhiteSpace($report) -or -not $report.Contains("Empleados activos")) {
    throw "AdvancedReportBuilder no generó contenido esperado."
}
Write-Host "AdvancedReportBuilder OK"

$backupType = $assembly.GetType("SistemaGestionNomina.Services.BackupService")
$backupService = [Activator]::CreateInstance($backupType)
$backupPath = $backupType.GetMethod("CrearBackup").Invoke($backupService, [object[]]@([string]$backupDir))
$verified = $backupType.GetMethod("VerificarBackup").Invoke($backupService, @($backupPath))
if (-not $verified) {
    throw "BackupService no verificó el hash."
}
Write-Host "BackupService OK"
