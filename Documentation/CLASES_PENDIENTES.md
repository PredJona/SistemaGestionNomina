# Modulos Avanzados Implementados

Estas clases separan responsabilidades avanzadas del sistema y ya cuentan con comportamiento operativo. Algunas funciones siguen siendo extensibles, pero no son simples esqueletos.

## AdvancedNominaRules.cs

Responsable de reglas avanzadas de nómina, como topes, subsidios, ajustes especiales o reglas por contrato.

Estado actual: calcula ajuste por antiguedad, asistencia perfecta, faltas y tardanzas dentro de un periodo.

## AttendanceDeviceImportService.cs

Responsable de importar asistencia desde reloj marcador, archivo CSV, Excel u otro dispositivo externo.

Estado actual: importa CSV con formato `Codigo,Fecha,HoraEntrada,HoraSalida,Estado`, valida datos y actualiza o inserta registros por empleado y fecha.

## EmailService.cs

Responsable de enviar comprobantes de pago por correo electrónico, incluyendo configuración SMTP y adjuntos PDF.

Estado actual: crea un borrador `.eml` con el PDF adjunto para abrirlo en un cliente de correo.

## RolePermissionService.cs

Responsable de permisos avanzados por rol para restringir acciones por módulo.

Estado actual: aplica una matriz de permisos para `Admin`, `RRHH`, `Contabilidad` y `Consulta`.

## AdvancedReportBuilder.cs

Responsable de construir reportes personalizados con filtros, agrupaciones y métricas dinámicas.

Estado actual: genera un resumen ejecutivo con métricas de empleados, nóminas, comprobantes, asistencia y netos por departamento.

## AuditTrailService.cs

Responsable de registrar historial detallado de cambios, usuario, módulo, acción y fecha.

Estado actual: registra acciones en la tabla SQLite `Auditoria` y permite consultar los últimos registros.

## BackupService.cs

Responsable de crear copias de seguridad de `nomina.db` y registrar respaldos.

Estado actual: genera backup de `nomina.db`, calcula archivo `.sha256`, lista respaldos y verifica integridad.
