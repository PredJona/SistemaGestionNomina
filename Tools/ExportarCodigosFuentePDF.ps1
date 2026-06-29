param(
    [string]$Salida = "CodigosFuentePDF",
    [switch]$Abrir
)

$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
$outputDir = Join-Path $projectRoot $Salida
$htmlPath = Join-Path $outputDir "CodigoFuente.html"

New-Item -ItemType Directory -Force -Path $outputDir | Out-Null

$ignoredFolders = @("\bin\", "\obj\", "\.git\", "\.vs\", "\packages\")

$files = Get-ChildItem -Path $projectRoot -Recurse -File -Include *.cs |
    Where-Object {
        $include = $true
        foreach ($folder in $ignoredFolders) {
            if ($_.FullName -like "*$folder*") {
                $include = $false
            }
        }

        $include
    } |
    Sort-Object FullName

$sections = foreach ($file in $files) {
    $relative = $file.FullName.Substring($projectRoot.Length).TrimStart("\")
    $content = [System.Net.WebUtility]::HtmlEncode((Get-Content -LiteralPath $file.FullName -Raw))
    "<h2>$relative</h2><pre><code>$content</code></pre>"
}

$html = @"
<!doctype html>
<html lang="es">
<head>
    <meta charset="utf-8">
    <title>Codigo Fuente - Proy2_Eq01_CamposPD</title>
    <style>
        body { font-family: Segoe UI, Arial, sans-serif; margin: 32px; color: #111827; }
        h1 { font-size: 24px; margin-bottom: 8px; }
        h2 { font-size: 16px; margin-top: 28px; border-bottom: 1px solid #d1d5db; padding-bottom: 6px; }
        pre { white-space: pre-wrap; font-family: Consolas, monospace; font-size: 11px; background: #f9fafb; padding: 12px; border: 1px solid #e5e7eb; }
        @media print { body { margin: 18mm; } h2 { page-break-before: always; } }
    </style>
</head>
<body>
    <h1>Codigo Fuente - Proy2_Eq01_CamposPD</h1>
    <p>Archivo generado para impresion o conversion a PDF. Use Imprimir > Guardar como PDF.</p>
    $($sections -join "`n")
</body>
</html>
"@

Set-Content -LiteralPath $htmlPath -Value $html -Encoding UTF8

Write-Host "Salida generada: $htmlPath"
Write-Host "Abra el HTML en un navegador y use Imprimir > Guardar como PDF."

if ($Abrir) {
    Start-Process -FilePath $htmlPath
}
