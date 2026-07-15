$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Windows.Forms

$projectRoot = Split-Path -Parent $PSScriptRoot
$binDir = Join-Path $projectRoot "bin\Debug\net48"
$exePath = Join-Path $binDir "Proy2_Eq01_CamposPD.exe"
$tempRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("Proy2_FormSmoke_" + [Guid]::NewGuid().ToString("N"))
$dbPath = Join-Path $tempRoot "nomina_forms.db"
$previousDbPath = [Environment]::GetEnvironmentVariable("NOMINA_DB_PATH")

try {
    [System.IO.Directory]::CreateDirectory($tempRoot) | Out-Null
    [Environment]::SetEnvironmentVariable("NOMINA_DB_PATH", $dbPath)

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

    $forms = @(
        "FrmLogin",
        "FrmMain",
        "FrmDashboard",
        "FrmEmpleados",
        "FrmAsistencia",
        "FrmNomina",
        "FrmComprobantes",
        "FrmReportes",
        "FrmConfiguracion",
        "FrmAuditoria",
        "FrmAcercaDe",
        "FrmPortalTrabajador",
        "FrmMiPerfil",
        "FrmMisAsistencias",
        "FrmMisComprobantes",
        "FrmCambiarPassword",
        "FrmCambioLaboral",
        "FrmHistorialEmpleado",
        "FrmSolicitudesAusencia",
        "FrmSolicitudAusenciaDetalle",
        "FrmGestionAusencias"
    )

    foreach ($name in $forms) {
        $type = $assembly.GetType("SistemaGestionNomina.UI." + $name)
        if ($null -eq $type) {
            throw "Tipo no encontrado: $name"
        }

        $form = [Activator]::CreateInstance($type)
        if ($form.Controls.Count -eq 0) {
            throw "$name no tiene controles cargados"
        }

        if (-not (Test-Path -LiteralPath (Join-Path $projectRoot "UI\$name.Designer.cs"))) {
            throw "$name no tiene archivo Designer.cs"
        }

        if (-not (Test-Path -LiteralPath (Join-Path $projectRoot "UI\$name.resx"))) {
            throw "$name no tiene archivo resx"
        }

        Write-Host "$name OK controles=$($form.Controls.Count)"
        $form.Dispose()
    }
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
            Remove-Item -LiteralPath $resolved -Recurse -Force -ErrorAction SilentlyContinue
        }
    }
}
