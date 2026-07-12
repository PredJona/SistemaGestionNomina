$ErrorActionPreference = "Stop"

function Assert-True {
    param([bool]$Condition, [string]$Message)
    if (-not $Condition) { throw $Message }
}

function Get-Scalar {
    param($Connection, [string]$Sql)
    $command = $Connection.CreateCommand()
    try {
        $command.CommandText = $Sql
        return $command.ExecuteScalar()
    }
    finally { $command.Dispose() }
}

function Invoke-NonQuery {
    param($Connection, [string]$Sql)
    $command = $Connection.CreateCommand()
    try {
        $command.CommandText = $Sql
        [void]$command.ExecuteNonQuery()
    }
    finally { $command.Dispose() }
}

function New-UserModel {
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

function Assert-Unauthorized {
    param([scriptblock]$Action, [string]$Message)
    try {
        & $Action
    }
    catch {
        $errorObject = $_.Exception
        while ($null -ne $errorObject.InnerException) { $errorObject = $errorObject.InnerException }
        if ($errorObject -is [System.UnauthorizedAccessException]) { return }
        throw
    }
    throw $Message
}

$projectRoot = Split-Path -Parent $PSScriptRoot
$binDir = Join-Path $projectRoot "bin\Debug\net48"
$exePath = Join-Path $binDir "Proy2_Eq01_CamposPD.exe"
$sqlitePath = Join-Path $binDir "System.Data.SQLite.dll"
$tempRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("Proy2_Fase1_" + [Guid]::NewGuid().ToString("N"))
$dbPath = Join-Path $tempRoot "nomina_legacy.db"
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

    $sqliteAssembly = [System.Reflection.Assembly]::LoadFrom($sqlitePath)
    $sqliteType = $sqliteAssembly.GetType("System.Data.SQLite.SQLiteConnection")
    $sqliteType.GetMethod("CreateFile", [type[]]@([string])).Invoke($null, [object[]]@([string]$dbPath))
    $legacyConnection = [Activator]::CreateInstance($sqliteType, [object[]]@("Data Source=$dbPath;Version=3;Foreign Keys=True;"))
    $legacyConnection.Open()
    Invoke-NonQuery $legacyConnection @"
CREATE TABLE Usuarios (
    IdUsuario INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreUsuario TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    Rol TEXT NOT NULL,
    Estado TEXT NOT NULL
);
"@
    $sha = [System.Security.Cryptography.SHA256]::Create()
    try {
        $legacyBytes = $sha.ComputeHash([System.Text.Encoding]::UTF8.GetBytes("admin123"))
        $legacyHash = -join ($legacyBytes | ForEach-Object { $_.ToString("x2") })
    }
    finally { $sha.Dispose() }
    $legacyInsert = $legacyConnection.CreateCommand()
    $legacyInsert.CommandText = "INSERT INTO Usuarios (NombreUsuario,PasswordHash,Rol,Estado) VALUES ('admin',@hash,'Admin','Activo');"
    [void]$legacyInsert.Parameters.AddWithValue("@hash", $legacyHash)
    [void]$legacyInsert.ExecuteNonQuery()
    $legacyInsert.Dispose()
    $legacyConnection.Close()
    $legacyConnection.Dispose()

    $assembly = [System.Reflection.Assembly]::LoadFrom($exePath)
    $initializer = $assembly.GetType("SistemaGestionNomina.Data.DatabaseInitializer")
    $initializer.GetMethod("Initialize").Invoke($null, @())
    $initializer.GetMethod("Initialize").Invoke($null, @())

    $connection = [Activator]::CreateInstance($sqliteType, [object[]]@("Data Source=$dbPath;Version=3;Foreign Keys=True;"))
    $connection.Open()
    Assert-True ((Get-Scalar $connection "PRAGMA user_version;") -eq 3) "Las migraciones no llegaron a la versión 3."
    Assert-True ((Get-Scalar $connection "SELECT COUNT(1) FROM pragma_table_info('Usuarios') WHERE name='IdEmpleado';") -eq 1) "Falta IdEmpleado en Usuarios."
    Assert-True ((Get-Scalar $connection "SELECT COUNT(1) FROM MigracionesLog;") -eq 3) "Las migraciones no son idempotentes."
    $connection.Close()
    $connection.Dispose()

    $migrationBackups = Get-ChildItem -LiteralPath (Join-Path $tempRoot "Backups\Migrations") -Filter "*.db"
    Assert-True ($migrationBackups.Count -ge 1) "No se creó el respaldo previo a la migración."
    Write-Host "Migraciones y respaldo OK"

    $authType = $assembly.GetType("SistemaGestionNomina.Services.AuthService")
    $auth = [Activator]::CreateInstance($authType)
    $authenticate = $authType.GetMethod("Authenticate")
    $adminResult = $authenticate.Invoke($auth, @("admin", "admin123"))
    Assert-True $adminResult.IsSuccess "El administrador heredado no pudo iniciar sesión."

    $connection = [Activator]::CreateInstance($sqliteType, [object[]]@("Data Source=$dbPath;Version=3;Foreign Keys=True;"))
    $connection.Open()
    $adminHash = [string](Get-Scalar $connection "SELECT PasswordHash FROM Usuarios WHERE NombreUsuario='admin';")
    Assert-True $adminHash.StartsWith("pbkdf2-sha256`$v1`$") "El hash SHA-256 heredado no se actualizó a PBKDF2."
    $connection.Close()
    $connection.Dispose()
    Write-Host "Compatibilidad SHA-256 y PBKDF2 OK"

    $roleType = $assembly.GetType("SistemaGestionNomina.Services.RolePermissionService")
    $roleService = [Activator]::CreateInstance($roleType)
    $hasPermission = $roleType.GetMethod("TienePermiso", [type[]]@([string], [string]))
    Assert-True ([bool]$hasPermission.Invoke($roleService, @("Admin", "auditoria.ver"))) "Admin debe ver auditoría."
    Assert-True ([bool]$hasPermission.Invoke($roleService, @("RRHH", "empleados.crear"))) "RRHH debe crear empleados."
    Assert-True ([bool]$hasPermission.Invoke($roleService, @("Contabilidad", "nomina.confirmar"))) "Contabilidad debe confirmar nómina."
    Assert-True ([bool]$hasPermission.Invoke($roleService, @("Supervisor", "asistencia.registrar"))) "Supervisor debe registrar asistencia."
    Assert-True (-not [bool]$hasPermission.Invoke($roleService, @("Trabajador", "dashboard.ver"))) "Trabajador no debe ver dashboard global."
    Assert-True (-not [bool]$hasPermission.Invoke($roleService, @("Desconocido", "empleados.ver"))) "Un rol desconocido fue autorizado."
    Write-Host "Matriz de permisos OK"

    $sessionType = $assembly.GetType("SistemaGestionNomina.Security.SessionContext")
    $sessionType.GetMethod("Begin").Invoke($null, @((New-UserModel $assembly 20 "rrhh_test" "RRHH" $null)))
    $payrollType = $assembly.GetType("SistemaGestionNomina.Services.NominaService")
    $payroll = [Activator]::CreateInstance($payrollType)
    Assert-Unauthorized {
        $payrollType.GetMethod("CalcularNomina").Invoke($payroll,
            @([DateTime]::Today.AddDays(-15), [DateTime]::Today, $null))
    } "RRHH pudo llamar directamente al cálculo de nómina."
    Write-Host "Autorización en servicios OK"

    $sessionType.GetMethod("Clear").Invoke($null, @())
    $passwordType = $assembly.GetType("SistemaGestionNomina.Helpers.PasswordHelper")
    $testHash = $passwordType.GetMethod("HashPassword").Invoke($null, @("ClaveSegura123"))
    $connection = [Activator]::CreateInstance($sqliteType, [object[]]@("Data Source=$dbPath;Version=3;Foreign Keys=True;"))
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = "INSERT INTO Usuarios (NombreUsuario, PasswordHash, Rol, Estado) VALUES (@u,@p,'RRHH','Activo');"
    [void]$command.Parameters.AddWithValue("@u", "bloqueo_test")
    [void]$command.Parameters.AddWithValue("@p", $testHash)
    [void]$command.ExecuteNonQuery()
    $command.Dispose()
    $connection.Close()
    $connection.Dispose()

    for ($i = 1; $i -le 5; $i++) { $lastResult = $authenticate.Invoke($auth, @("bloqueo_test", "incorrecta")) }
    Assert-True ($lastResult.Status.ToString() -eq "TemporarilyBlocked") "El usuario no se bloqueó al quinto intento."
    $blockedResult = $authenticate.Invoke($auth, @("bloqueo_test", "ClaveSegura123"))
    Assert-True (-not $blockedResult.IsSuccess) "Un usuario bloqueado pudo iniciar sesión."
    Write-Host "Bloqueo temporal OK"

    $sessionType.GetMethod("Begin").Invoke($null, @($adminResult.User))
    $attendanceType = $assembly.GetType("SistemaGestionNomina.Models.Asistencia")
    $attendanceServiceType = $assembly.GetType("SistemaGestionNomina.Services.AsistenciaService")
    $attendanceService = [Activator]::CreateInstance($attendanceServiceType)
    $attendance = [Activator]::CreateInstance($attendanceType)
    $attendanceType.GetProperty("IdEmpleado").SetValue($attendance, 1)
    $attendanceType.GetProperty("Fecha").SetValue($attendance, [DateTime]::Parse("2030-01-10"))
    $attendanceType.GetProperty("Estado").SetValue($attendance, "Falta")
    [void]$attendanceServiceType.GetMethod("Register").Invoke($attendanceService, @($attendance))

    $duplicateRejected = $false
    try { [void]$attendanceServiceType.GetMethod("Register").Invoke($attendanceService, @($attendance)) }
    catch { $duplicateRejected = $true }
    Assert-True $duplicateRejected "El servicio permitió una asistencia duplicada."

    $connection = [Activator]::CreateInstance($sqliteType, [object[]]@("Data Source=$dbPath;Version=3;Foreign Keys=True;"))
    $connection.Open()
    $triggerRejected = $false
    try {
        Invoke-NonQuery $connection "INSERT INTO Asistencias (IdEmpleado,Fecha,HorasTrabajadas,Estado) VALUES (1,'2030-01-10',0,'Falta');"
    }
    catch { $triggerRejected = $true }
    Assert-True $triggerRejected "SQLite permitió una asistencia duplicada."

    $departmentId = [int](Get-Scalar $connection "SELECT IdDepartamento FROM Empleados WHERE IdEmpleado=1;")
    $auditCount = [int](Get-Scalar $connection "SELECT COUNT(1) FROM Auditoria WHERE Usuario='admin' AND Modulo='Seguridad' AND Accion LIKE 'Inicio de sesi%';")
    $connection.Close()
    $connection.Dispose()
    Assert-True ($auditCount -ge 1) "La auditoría no registró al usuario real."
    Write-Host "Duplicados y auditoría OK"

    $supervisor = New-UserModel $assembly 30 "supervisor_test" "Supervisor" 1
    $sessionType.GetMethod("Begin").Invoke($null, @($supervisor))
    $employeeServiceType = $assembly.GetType("SistemaGestionNomina.Services.EmpleadoService")
    $employeeService = [Activator]::CreateInstance($employeeServiceType)
    $scopedEmployees = $employeeServiceType.GetMethod("GetActive").Invoke($employeeService, @($null))
    Assert-True ($scopedEmployees.Count -gt 0) "El Supervisor no recibió empleados de su departamento."
    foreach ($employee in $scopedEmployees) {
        Assert-True ($employee.IdDepartamento -eq $departmentId) "El Supervisor recibió empleados de otro departamento."
    }
    Write-Host "Alcance departamental del Supervisor OK"

    $sessionType.GetMethod("Clear").Invoke($null, @())
    Write-Host "Fase 1 Security Smoke Test OK"
}
finally {
    [Environment]::SetEnvironmentVariable("NOMINA_DB_PATH", $previousDbPath)
    try {
        if ($null -ne $sqliteType) { $sqliteType.GetMethod("ClearAllPools").Invoke($null, @()) }
    }
    catch { }
    if (Test-Path -LiteralPath $tempRoot) {
        $resolvedTemp = [System.IO.Path]::GetFullPath($tempRoot)
        $tempBase = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath())
        if ($resolvedTemp.StartsWith($tempBase, [System.StringComparison]::OrdinalIgnoreCase)) {
            try { Remove-Item -LiteralPath $resolvedTemp -Recurse -Force }
            catch { Write-Warning "No se pudo limpiar la carpeta temporal: $resolvedTemp" }
        }
    }
}
