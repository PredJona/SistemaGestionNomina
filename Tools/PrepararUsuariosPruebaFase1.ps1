param(
    [string]$DatabasePath,
    [int]$SupervisorEmployeeId = 0,
    [int]$WorkerEmployeeId = 0
)

$ErrorActionPreference = "Stop"
$projectRoot = Split-Path -Parent $PSScriptRoot
$binDir = Join-Path $projectRoot "bin\Debug\net48"
$exePath = Join-Path $binDir "ProyFinal_LPI_Eq01_NomiCore.exe"
if ([string]::IsNullOrWhiteSpace($DatabasePath)) {
    $DatabasePath = Join-Path $binDir "nomina.db"
}
$DatabasePath = [System.IO.Path]::GetFullPath($DatabasePath)

[AppDomain]::CurrentDomain.add_AssemblyResolve({
    param($sender, $args)
    $assemblyName = ($args.Name -split ",")[0]
    $candidate = Join-Path $binDir ($assemblyName + ".dll")
    if (Test-Path -LiteralPath $candidate) { return [System.Reflection.Assembly]::LoadFrom($candidate) }
    return $null
})

$previousDbPath = [Environment]::GetEnvironmentVariable("NOMINA_DB_PATH")
try {
    [Environment]::SetEnvironmentVariable("NOMINA_DB_PATH", $DatabasePath)
    $assembly = [System.Reflection.Assembly]::LoadFrom($exePath)
    $assembly.GetType("SistemaGestionNomina.Data.DatabaseInitializer").GetMethod("Initialize").Invoke($null, @())
    $sqliteAssembly = [System.Reflection.Assembly]::LoadFrom((Join-Path $binDir "System.Data.SQLite.dll"))
    $sqliteType = $sqliteAssembly.GetType("System.Data.SQLite.SQLiteConnection")
    $connection = [Activator]::CreateInstance($sqliteType, [object[]]@("Data Source=$DatabasePath;Version=3;Foreign Keys=True;"))
    $connection.Open()

    Write-Host "Empleados disponibles para asociar Supervisor y Trabajador:"
    $listCommand = $connection.CreateCommand()
    $listCommand.CommandText = "SELECT IdEmpleado, Codigo, Nombre || ' ' || Apellido AS Nombre FROM Empleados WHERE Estado='Activo' ORDER BY IdEmpleado;"
    $reader = $listCommand.ExecuteReader()
    while ($reader.Read()) {
        Write-Host ("  {0}: {1} - {2}" -f $reader["IdEmpleado"], $reader["Codigo"], $reader["Nombre"])
    }
    $reader.Close()
    $listCommand.Dispose()

    if ($SupervisorEmployeeId -le 0) { $SupervisorEmployeeId = [int](Read-Host "IdEmpleado para Supervisor") }
    if ($WorkerEmployeeId -le 0) { $WorkerEmployeeId = [int](Read-Host "IdEmpleado para Trabajador") }
    if ($SupervisorEmployeeId -eq $WorkerEmployeeId) {
        throw "Supervisor y Trabajador deben asociarse a empleados diferentes."
    }

    foreach ($employeeId in @($SupervisorEmployeeId, $WorkerEmployeeId)) {
        $check = $connection.CreateCommand()
        $check.CommandText = "SELECT COUNT(1) FROM Empleados WHERE IdEmpleado=@id AND Estado='Activo';"
        [void]$check.Parameters.AddWithValue("@id", $employeeId)
        if ([int]$check.ExecuteScalar() -ne 1) { throw "El empleado $employeeId no existe o está inactivo." }
        $check.Dispose()
    }

    $passwordType = $assembly.GetType("SistemaGestionNomina.Helpers.PasswordHelper")
    $hashMethod = $passwordType.GetMethod("HashPassword")

    function Set-TestUser {
        param([string]$Username, [string]$Role, [Nullable[int]]$EmployeeId)
        $securePassword = Read-Host "Contraseña para $Username ($Role)" -AsSecureString
        $pointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($securePassword)
        try {
            $plainPassword = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($pointer)
            if ([string]::IsNullOrWhiteSpace($plainPassword) -or $plainPassword.Length -lt 8) {
                throw "La contraseña de $Username debe tener al menos 8 caracteres."
            }
            $hash = $hashMethod.Invoke($null, @($plainPassword))
        }
        finally {
            if ($pointer -ne [IntPtr]::Zero) { [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($pointer) }
            $plainPassword = $null
        }

        $command = $connection.CreateCommand()
        try {
            $command.CommandText = @"
INSERT INTO Usuarios (NombreUsuario, PasswordHash, Rol, Estado, IdEmpleado, IntentosFallidos, Bloqueado)
VALUES (@username, @hash, @role, 'Activo', @employee, 0, 0)
ON CONFLICT(NombreUsuario) DO UPDATE SET
    PasswordHash=excluded.PasswordHash, Rol=excluded.Rol, Estado='Activo', IdEmpleado=excluded.IdEmpleado,
    IntentosFallidos=0, Bloqueado=0, FechaBloqueo=NULL;
"@
            [void]$command.Parameters.AddWithValue("@username", $Username)
            [void]$command.Parameters.AddWithValue("@hash", $hash)
            [void]$command.Parameters.AddWithValue("@role", $Role)
            [void]$command.Parameters.AddWithValue("@employee", $(if ($null -eq $EmployeeId) { [DBNull]::Value } else { $EmployeeId }))
            [void]$command.ExecuteNonQuery()
        }
        finally { $command.Dispose() }
    }

    Set-TestUser "rrhh" "RRHH" $null
    Set-TestUser "contabilidad" "Contabilidad" $null
    Set-TestUser "supervisor" "Supervisor" $SupervisorEmployeeId
    Set-TestUser "trabajador" "Trabajador" $WorkerEmployeeId

    $audit = $connection.CreateCommand()
    $audit.CommandText = "INSERT INTO Auditoria (Usuario,Modulo,Accion,Detalle,Fecha) VALUES ('sistema','Seguridad','Preparar pruebas','Usuarios de roles creados sin contraseñas predeterminadas',@fecha);"
    [void]$audit.Parameters.AddWithValue("@fecha", [DateTime]::Now.ToString("yyyy-MM-dd HH:mm:ss"))
    [void]$audit.ExecuteNonQuery()
    $audit.Dispose()
    $connection.Close()
    $connection.Dispose()
    Write-Host "Usuarios de prueba creados correctamente en: $DatabasePath"
}
finally {
    [Environment]::SetEnvironmentVariable("NOMINA_DB_PATH", $previousDbPath)
}
