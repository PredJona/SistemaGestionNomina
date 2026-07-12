$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Windows.Forms

$projectRoot = Split-Path -Parent $PSScriptRoot
$binDir = Join-Path $projectRoot "bin\Debug\net48"
$exePath = Join-Path $binDir "Proy2_Eq01_CamposPD.exe"

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
    "FrmAcercaDe"
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

    Write-Host "$name OK controles=$($form.Controls.Count)"
    $form.Dispose()
}
