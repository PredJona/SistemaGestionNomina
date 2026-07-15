$ErrorActionPreference = "Stop"

function Assert-True {
    param([bool]$Condition, [string]$Message)
    if (-not $Condition) { throw $Message }
}

function Assert-Unauthorized {
    param([scriptblock]$Action, [string]$Message)
    try { & $Action }
    catch {
        $errorObject = $_.Exception
        while ($null -ne $errorObject.InnerException) { $errorObject = $errorObject.InnerException }
        if ($errorObject -is [System.UnauthorizedAccessException]) { return }
        throw
    }
    throw $Message
}

function Invoke-NonQuery {
    param($Connection, [string]$Sql)
    $command = $Connection.CreateCommand()
    try { $command.CommandText = $Sql; [void]$command.ExecuteNonQuery() }
    finally { $command.Dispose() }
}

function Get-Scalar {
    param($Connection, [string]$Sql)
    $command = $Connection.CreateCommand()
    try { $command.CommandText = $Sql; return $command.ExecuteScalar() }
    finally { $command.Dispose() }
}

function New-SessionUser {
    param($Assembly, [int]$Id, [string]$Username, [string]$Role, [Nullable[int]]$EmployeeId)
    $type = $Assembly.GetType("SistemaGestionNomina.Models.Usuario")
    $user = [Activator]::CreateInstance($type)
    $type.GetProperty("IdUsuario").SetValue($user, $Id)
    $type.GetProperty("NombreUsuario").SetValue($user, $Username)
    $type.GetProperty("Rol").SetValue($user, $Role)
    $type.GetProperty("Estado").SetValue($user, "Activo")
    if ($null -ne $EmployeeId) { $type.GetProperty("IdEmpleado").SetValue($user, $EmployeeId) }
    return $user
}

$projectRoot = Split-Path -Parent $PSScriptRoot
$binDir = Join-Path $projectRoot "bin\Debug\net48"
$exePath = Join-Path $binDir "ProyFinal_LPI_Eq01_NomiCore.exe"
$sqlitePath = Join-Path $binDir "System.Data.SQLite.dll"
$tempRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("Proy2_Fase2_" + [Guid]::NewGuid().ToString("N"))
$dbPath = Join-Path $tempRoot "nomina_fase2.db"
$pdfPath = Join-Path $tempRoot "comprobante_propio.pdf"
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
    $sqliteAssembly = [System.Reflection.Assembly]::LoadFrom($sqlitePath)
    $sqliteType = $sqliteAssembly.GetType("System.Data.SQLite.SQLiteConnection")
    $connection = [Activator]::CreateInstance($sqliteType, [object[]]@("Data Source=$dbPath;Version=3;Foreign Keys=True;"))
    $connection.Open()
    Invoke-NonQuery $connection "DELETE FROM Usuarios WHERE NombreUsuario IN ('supervisor','trabajador');"

    Invoke-NonQuery $connection @"
INSERT INTO Asistencias (IdEmpleado,Fecha,HoraEntrada,HoraSalida,HorasTrabajadas,Estado) VALUES
(1,'2031-05-03','08:00','17:00',9,'Puntual'),
(1,'2031-05-04','08:20','17:00',8.67,'Tardanza'),
(2,'2031-05-03','08:00','16:00',8,'Puntual');
INSERT INTO PeriodosNomina (Nombre,FechaInicio,FechaFin,Estado) VALUES
('Mayo 2031','2031-05-01','2031-05-31','Pagado');
INSERT INTO Nominas (IdPeriodo,FechaCalculo,TotalIngresos,TotalDeducciones,TotalNeto,Estado) VALUES
(1,'2031-05-31',4300,600,3700,'Pagada');
INSERT INTO NominaDetalle (IdNomina,IdEmpleado,SueldoBase,Bonos,HorasExtra,MontoHorasExtra,TotalIngresos,TotalDeducciones,NetoPagar) VALUES
(1,1,2450,100,1,20,2570,320,2250),
(1,2,1850,0,0,0,1850,280,1570);
INSERT INTO Comprobantes (IdNomina,IdEmpleado,NumeroComprobante,FechaGeneracion,RutaPdf) VALUES
(1,1,'COMP-F2-001','2031-05-31 12:00:00',''),
(1,2,'COMP-F2-002','2031-05-31 12:00:00','');
"@

    $passwordType = $assembly.GetType("SistemaGestionNomina.Helpers.PasswordHelper")
    $workerHash = [string]$passwordType.GetMethod("HashPassword").Invoke($null, @("Worker123"))
    $insertUser = $connection.CreateCommand()
    $insertUser.CommandText = "INSERT INTO Usuarios (NombreUsuario,PasswordHash,Rol,Estado,IdEmpleado) VALUES (@u,@p,'Trabajador','Activo',@e); SELECT last_insert_rowid();"
    [void]$insertUser.Parameters.AddWithValue("@u", "worker_fase2")
    [void]$insertUser.Parameters.AddWithValue("@p", $workerHash)
    [void]$insertUser.Parameters.AddWithValue("@e", 1)
    $workerUserId = [int]$insertUser.ExecuteScalar()
    $insertUser.Dispose()

    $ownPayslipId = [int](Get-Scalar $connection "SELECT IdComprobante FROM Comprobantes WHERE IdEmpleado=1;")
    $foreignPayslipId = [int](Get-Scalar $connection "SELECT IdComprobante FROM Comprobantes WHERE IdEmpleado=2;")
    $connection.Close(); $connection.Dispose()

    $sessionType = $assembly.GetType("SistemaGestionNomina.Security.SessionContext")
    $worker = New-SessionUser $assembly $workerUserId "worker_fase2" "Trabajador" 1
    $sessionType.GetMethod("Begin").Invoke($null, @($worker))

    $roleType = $assembly.GetType("SistemaGestionNomina.Services.RolePermissionService")
    $roleService = [Activator]::CreateInstance($roleType)
    $hasPermission = $roleType.GetMethod("TienePermiso", [type[]]@([string], [string]))
    foreach ($permission in @("portal.ver", "portal.perfil.ver", "portal.asistencia.ver", "portal.comprobantes.ver", "portal.comprobantes.descargar", "cuenta.password.cambiar")) {
        Assert-True ([bool]$hasPermission.Invoke($roleService, @("Trabajador", $permission))) "Falta permiso de portal: $permission"
    }
    Assert-True (-not [bool]$hasPermission.Invoke($roleService, @("Trabajador", "empleados.ver"))) "Trabajador recibio un permiso administrativo."

    $portalType = $assembly.GetType("SistemaGestionNomina.Services.EmployeePortalService")
    $portal = [Activator]::CreateInstance($portalType)
    $profile = $portalType.GetMethod("GetMyProfile").Invoke($portal, @())
    Assert-True ($profile.IdEmpleado -eq 1) "El portal devolvio otro perfil."
    $attendance = $portalType.GetMethod("GetMyAttendance").Invoke($portal, @($null, $null, "Todos"))
    Assert-True ($attendance.Count -eq 2) "La asistencia propia no coincide con los datos sembrados."
    foreach ($item in $attendance) { Assert-True ($item.IdEmpleado -eq 1) "Se filtro asistencia de otro empleado." }
    $payslips = $portalType.GetMethod("GetMyPayslips").Invoke($portal, @(""))
    Assert-True ($payslips.Count -eq 1 -and $payslips[0].IdEmpleado -eq 1) "Se filtraron comprobantes ajenos."
    Assert-Unauthorized { $portalType.GetMethod("GetMyPayslipById").Invoke($portal, @($foreignPayslipId)) } "El trabajador pudo consultar un comprobante ajeno."

    $hostSource = Join-Path $PSScriptRoot "Fase2PdfSmokeHost.cs"
    $hostExe = Join-Path $binDir "Fase2PdfSmokeHost.exe"
    $csc = Join-Path $env:WINDIR "Microsoft.NET\Framework64\v4.0.30319\csc.exe"
    if (-not (Test-Path -LiteralPath $csc)) { $csc = Join-Path $env:WINDIR "Microsoft.NET\Framework\v4.0.30319\csc.exe" }
    & $csc /nologo /target:exe "/out:$hostExe" "/reference:$exePath" $hostSource
    Assert-True ($LASTEXITCODE -eq 0) "No se pudo compilar el host de PDF."
    Copy-Item -LiteralPath ($exePath + ".config") -Destination ($hostExe + ".config") -Force
    & $hostExe $dbPath $pdfPath $ownPayslipId $foreignPayslipId $workerUserId "worker_fase2"
    Assert-True ($LASTEXITCODE -eq 0) "Fallo la generacion segura del PDF personal."
    Write-Host "Alcance personal y PDF seguro OK"

    $accountType = $assembly.GetType("SistemaGestionNomina.Services.AccountService")
    $account = [Activator]::CreateInstance($accountType)
    $accountType.GetMethod("ChangeOwnPassword").Invoke($account, @("Worker123", "NuevaClave456", "NuevaClave456"))
    $sessionType.GetMethod("Clear").Invoke($null, @())
    $authType = $assembly.GetType("SistemaGestionNomina.Services.AuthService")
    $auth = [Activator]::CreateInstance($authType)
    $oldResult = $authType.GetMethod("Authenticate").Invoke($auth, @("worker_fase2", "Worker123"))
    $newResult = $authType.GetMethod("Authenticate").Invoke($auth, @("worker_fase2", "NuevaClave456"))
    Assert-True (-not $oldResult.IsSuccess -and $newResult.IsSuccess) "El cambio de contrasena no invalido la clave anterior."

    $connection = [Activator]::CreateInstance($sqliteType, [object[]]@("Data Source=$dbPath;Version=3;Foreign Keys=True;")); $connection.Open()
    $secretAudit = [int](Get-Scalar $connection "SELECT COUNT(1) FROM Auditoria WHERE Detalle LIKE '%Worker123%' OR Detalle LIKE '%NuevaClave456%' OR Detalle LIKE '%pbkdf2%';")
    Assert-True ($secretAudit -eq 0) "La auditoria contiene secretos de autenticacion."
    Invoke-NonQuery $connection "UPDATE Empleados SET Estado='Inactivo', FechaEfectivaLaboral='2031-06-01' WHERE IdEmpleado=2;"
    $connection.Close(); $connection.Dispose()

    $sessionType.GetMethod("Begin").Invoke($null, @((New-SessionUser $assembly 900 "sin_vinculo" "Trabajador" $null)))
    Assert-Unauthorized { $portalType.GetMethod("GetMyProfile").Invoke($portal, @()) } "Un trabajador sin empleado accedio al portal."
    $sessionType.GetMethod("Begin").Invoke($null, @((New-SessionUser $assembly 901 "inactivo" "Trabajador" 2)))
    Assert-Unauthorized { $portalType.GetMethod("GetMyProfile").Invoke($portal, @()) } "Un trabajador inactivo accedio al portal."
    $sessionType.GetMethod("Begin").Invoke($null, @((New-SessionUser $assembly 1 "admin" "Admin" $null)))
    Assert-Unauthorized { $portalType.GetMethod("GetMyProfile").Invoke($portal, @()) } "Un rol administrativo uso el alcance personal del trabajador."
    $sessionType.GetMethod("Clear").Invoke($null, @())
    Write-Host "Vinculos, contrasena y auditoria OK"
    Write-Host "Fase 2 Employee Portal Smoke Test OK"
}
finally {
    [Environment]::SetEnvironmentVariable("NOMINA_DB_PATH", $previousDbPath)
    try { if ($null -ne $sqliteType) { $sqliteType.GetMethod("ClearAllPools").Invoke($null, @()) } } catch { }
    if (Test-Path -LiteralPath $tempRoot) {
        $resolved = [System.IO.Path]::GetFullPath($tempRoot)
        $tempBase = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath())
        if ($resolved.StartsWith($tempBase, [System.StringComparison]::OrdinalIgnoreCase)) {
            try { Remove-Item -LiteralPath $resolved -Recurse -Force } catch { Write-Warning $_.Exception.Message }
        }
    }
}
