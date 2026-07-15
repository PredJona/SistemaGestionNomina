# Fase 4: Recursos Humanos Avanzado

## Estado

La Fase 4 esta implementada sobre Windows Forms, .NET Framework 4.8, C# 7.3 y SQLite. Los formularios nuevos conservan la estructura compatible con Visual Studio Designer: `.cs`, `.Designer.cs` y `.resx`.

## Arquitectura

La implementacion mantiene la separacion existente:

- `Models`: contratos de historial laboral, cambios efectivos y solicitudes de ausencia.
- `Data`: repositorios con SQL parametrizado y migraciones incrementales.
- `Services`: reglas de negocio, permisos, alcance, transacciones y auditoria.
- `UI`: formularios que consumen servicios y no ejecutan SQL.
- `Tools`: smoke tests sobre bases SQLite temporales.

Las migraciones son idempotentes y usan `PRAGMA user_version`:

- Version 5: historial laboral, fecha efectiva, snapshots de nomina, indices y protecciones de periodos.
- Version 6: solicitudes de ausencia, relacion con asistencias, indices, restricciones y triggers.

## Historial laboral

`HistorialEmpleado` registra cambios de salario, cargo, departamento y estado con valor anterior, valor nuevo, motivo, usuario y fecha efectiva.

Reglas principales:

- Un cambio vigente se aplica junto con su historial y auditoria en una transaccion.
- Un cambio futuro queda programado y se aplica cuando alcanza su fecha efectiva.
- Los cambios vencidos se procesan al iniciar la aplicacion y antes de consultar empleados o calcular nomina.
- No se permite afectar un periodo de nomina cerrado.
- La nomina resuelve los datos efectivos segun la fecha final del periodo.
- `NominaDetalle` conserva snapshots de codigo, nombre, cargo y departamento.

Permisos:

| Permiso | Admin | RRHH | Supervisor | Contabilidad | Trabajador |
|---|---:|---:|---:|---:|---:|
| Ver historial | Si | Si | No | No | No |
| Exportar historial | Si | Si | No | No | No |
| Programar cambios | Si | Si | No | No | No |

Los formularios `FrmCambioLaboral` y `FrmHistorialEmpleado` permiten capturar el motivo y la fecha efectiva, consultar el historial y exportarlo a Excel o PDF mediante `SaveFileDialog`.

## Solicitudes de ausencia

Tipos admitidos:

- Vacaciones
- Permiso remunerado
- Permiso no remunerado
- Incapacidad
- Licencia
- Ausencia justificada
- Ausencia injustificada
- Suspension

Estados y transiciones:

```text
Pendiente -> Aprobada, Rechazada o Cancelada
Aprobada  -> Cancelada
Rechazada -> estado terminal
Cancelada -> estado terminal
```

Al aprobar una solicitud se crean asistencias solo para lunes a viernes. Vacaciones, permiso remunerado, incapacidad, licencia y ausencia justificada se registran como `Permiso`; permiso no remunerado, ausencia injustificada y suspension se registran como `Falta`. Estas asistencias tienen cero horas extra y no crean descuentos monetarios automaticos.

La aprobacion se rechaza completa si un dia ya tiene asistencia. Cancelar una solicitud aprobada elimina exclusivamente las asistencias vinculadas por `IdSolicitudAusencia`, siempre que el periodo permanezca abierto.

## Alcance por rol

- Admin y RRHH: consulta, creacion, aprobacion, rechazo, cancelacion y exportacion.
- Supervisor: consulta, aprobacion y rechazo solo dentro del departamento asociado.
- Contabilidad: consulta de solicitudes.
- Trabajador: consulta, creacion y cancelacion de solicitudes propias pendientes.

Los formularios `FrmGestionAusencias`, `FrmSolicitudAusenciaDetalle` y `FrmSolicitudesAusencia` aplican la misma autorizacion que los servicios. El menu administrativo incluye `Ausencias` y el portal del trabajador incluye `Mis solicitudes`.

## Integridad y auditoria

Los servicios y triggers bloquean operaciones que intersecten nominas cerradas mediante la regla:

```text
FechaInicio <= fin consultado AND FechaFin >= inicio consultado
```

Las operaciones relevantes registran usuario real, modulo, accion, detalle y fecha. No se registran contrasenas, hashes ni secretos.

## Pruebas

Ejecutar desde la raiz del proyecto:

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\Tools\Fase4RrhhAvanzadoSmokeTest.ps1
powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\Tools\FormSmokeTest.ps1
```

`Fase4RrhhAvanzadoSmokeTest.ps1` valida permisos, aislamiento personal y departamental, aprobacion, mapeo de asistencias, cancelacion, rollback ante conflictos, periodos cerrados y auditoria. `FormSmokeTest.ps1` instancia los 21 formularios usando una base temporal.

## Limitaciones declaradas

- Se consideran dias laborables de lunes a viernes; todavia no existe calendario de feriados ni turnos.
- Los cambios futuros se aplican al iniciar o consultar; no existe un servicio de Windows en segundo plano.
- Las ausencias no generan descuentos monetarios automaticos porque no se definio una politica financiera para ellos.
- No se implementan conceptos configurables, prestamos, adelantos ni instalador en esta fase.
