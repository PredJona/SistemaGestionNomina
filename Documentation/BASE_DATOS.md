# Base de Datos

La base de datos se crea automáticamente con el nombre `nomina.db`.

## Tablas principales

## Usuarios

Guarda usuarios de acceso al sistema.

Campos principales: `IdUsuario`, `NombreUsuario`, `PasswordHash`, `Rol`, `Estado`.

## Departamentos

Catálogo de departamentos.

Campos principales: `IdDepartamento`, `Nombre`.

## Empleados

Personal registrado.

Campos principales: `IdEmpleado`, `Codigo`, `Nombre`, `Apellido`, `Cedula`, `Cargo`, `IdDepartamento`, `SalarioBase`, `Estado`, `FechaIngreso`.

Relación: cada empleado pertenece a un departamento.

## Asistencias

Registros de entrada, salida, horas trabajadas y estado.

Campos principales: `IdAsistencia`, `IdEmpleado`, `Fecha`, `HoraEntrada`, `HoraSalida`, `HorasTrabajadas`, `Estado`.

Relación: cada asistencia pertenece a un empleado.

## PeriodosNomina

Periodos procesados de nómina.

Campos principales: `IdPeriodo`, `Nombre`, `FechaInicio`, `FechaFin`, `Estado`.

## Nominas

Cabecera de cálculo de nómina.

Campos principales: `IdNomina`, `IdPeriodo`, `FechaCalculo`, `TotalIngresos`, `TotalDeducciones`, `TotalNeto`, `Estado`.

Relación: cada nómina pertenece a un periodo.

## NominaDetalle

Desglose por empleado.

Campos principales: `IdDetalle`, `IdNomina`, `IdEmpleado`, `SueldoBase`, `Bonos`, `HorasExtra`, `MontoHorasExtra`, `TotalIngresos`, `TotalDeducciones`, `NetoPagar`.

Relación: cada detalle pertenece a una nómina y a un empleado.

## Comprobantes

Comprobantes generados a partir de nóminas confirmadas.

Campos principales: `IdComprobante`, `IdNomina`, `IdEmpleado`, `NumeroComprobante`, `FechaGeneracion`, `RutaPdf`.

## ConfiguracionNomina

Parámetros académicos para el cálculo de deducciones y horas.

Campos principales: `IdConfiguracion`, `NombreParametro`, `Valor`, `Descripcion`.

## ReportesGenerados

Historial de reportes exportados.

Campos principales: `IdReporte`, `NombreReporte`, `Tipo`, `GeneradoPor`, `FechaGeneracion`, `RutaArchivo`.
