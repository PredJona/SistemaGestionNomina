$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Windows.Forms

function Assert-True {
    param([bool]$Condition, [string]$Message)
    if (-not $Condition) { throw $Message }
}

$projectRoot = Split-Path -Parent $PSScriptRoot
$binDir = Join-Path $projectRoot "bin\Debug\net48"
$exePath = Join-Path $binDir "ProyFinal_LPI_Eq01_NomiCore.exe"
$tempRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("Proy2_Fase2Forms_" + [Guid]::NewGuid().ToString("N"))
$dbPath = Join-Path $tempRoot "nomina_forms.db"
$previousDbPath = [Environment]::GetEnvironmentVariable("NOMINA_DB_PATH")

try {
    [System.IO.Directory]::CreateDirectory($tempRoot) | Out-Null
    [Environment]::SetEnvironmentVariable("NOMINA_DB_PATH", $dbPath)
    [AppDomain]::CurrentDomain.add_AssemblyResolve({
        param($sender, $args)
        $assemblyName = ($args.Name -split ",")[0]
        $candidate = Join-Path $binDir ($assemblyName + ".dll")
        if (Test-Path -LiteralPath $candidate) { return [System.Reflection.Assembly]::LoadFrom($candidate) }
        return $null
    })

    $assembly = [System.Reflection.Assembly]::LoadFrom($exePath)
    $initializer = $assembly.GetType("SistemaGestionNomina.Data.DatabaseInitializer")
    $initializer.GetMethod("Initialize").Invoke($null, @())
    $userType = $assembly.GetType("SistemaGestionNomina.Models.Usuario")
    $worker = [Activator]::CreateInstance($userType)
    $userType.GetProperty("IdUsuario").SetValue($worker, 700)
    $userType.GetProperty("NombreUsuario").SetValue($worker, "worker_forms")
    $userType.GetProperty("Rol").SetValue($worker, "Trabajador")
    $userType.GetProperty("Estado").SetValue($worker, "Activo")
    $userType.GetProperty("IdEmpleado").SetValue($worker, [Nullable[int]]1)
    $sessionType = $assembly.GetType("SistemaGestionNomina.Security.SessionContext")
    $sessionType.GetMethod("Begin").Invoke($null, @($worker))

    $forms = @("FrmPortalTrabajador", "FrmMiPerfil", "FrmMisAsistencias", "FrmMisComprobantes", "FrmCambiarPassword")
    foreach ($name in $forms) {
        $type = $assembly.GetType("SistemaGestionNomina.UI." + $name)
        Assert-True ($null -ne $type) "Tipo no encontrado: $name"
        $form = [Activator]::CreateInstance($type)
        Assert-True ($form.Controls.Count -gt 0) "$name no contiene controles."
        Assert-True (Test-Path -LiteralPath (Join-Path $projectRoot "UI\$name.Designer.cs")) "$name no tiene Designer.cs."
        Assert-True (Test-Path -LiteralPath (Join-Path $projectRoot "UI\$name.resx")) "$name no tiene resx."
        Write-Host "$name OK controles=$($form.Controls.Count)"
        $form.Dispose()
    }

    $mainType = $assembly.GetType("SistemaGestionNomina.UI.FrmMain")
    $main = [Activator]::CreateInstance($mainType, [object[]]@($worker))
    $main.Show()
    [System.Windows.Forms.Application]::DoEvents()
    $portalButton = $mainType.GetField("btnPortal")
    Assert-True ([bool]$portalButton.GetValue($main).Visible) "FrmMain no mostro el portal al Trabajador."
    Assert-True ($main.panelContent.Controls.Count -eq 1) "FrmMain no abrio el modulo inicial del Trabajador."
    $main.Dispose()
    $sessionType.GetMethod("Clear").Invoke($null, @())
    Write-Host "Fase 2 Form Smoke Test OK"
}
finally {
    [Environment]::SetEnvironmentVariable("NOMINA_DB_PATH", $previousDbPath)
    try {
        $sqliteType = [System.Type]::GetType("System.Data.SQLite.SQLiteConnection, System.Data.SQLite")
        if ($null -ne $sqliteType) { $sqliteType.GetMethod("ClearAllPools").Invoke($null, @()) }
    }
    catch { }
    if (Test-Path -LiteralPath $tempRoot) {
        $resolved = [System.IO.Path]::GetFullPath($tempRoot)
        $tempBase = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath())
        if ($resolved.StartsWith($tempBase, [System.StringComparison]::OrdinalIgnoreCase)) {
            try { Remove-Item -LiteralPath $resolved -Recurse -Force } catch { Write-Warning $_.Exception.Message }
        }
    }
}
