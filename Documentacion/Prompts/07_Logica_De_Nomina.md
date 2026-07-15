# Logica de negocio de nomina

```text
Trabaja sobre la logica de negocio de ProyFinal_LPI_Eq01_NomiCore - NomiCore.

Antes de cambiar codigo, identifica los servicios, repositorios, entidades, permisos y migraciones SQLite relacionados. No modifiques formularios ni archivos .Designer.cs salvo que el cambio requiera mostrar una validacion existente.

Reglas que debes conservar:
- Las entidades Empleado, Asistencia, Nomina, NominaDetalle y Comprobante usan campos privados y propiedades publicas validadas.
- Empleado debe tener codigo y cedula unicos, cargo valido y salario no negativo.
- Asistencia requiere empleado valido, fecha, horas no negativas y un estado permitido. No se permiten duplicados por empleado y fecha.
- Un periodo de nomina no puede solaparse con otro periodo incompatible.
- La nomina inicia como Calculada; solo puede pasar a Confirmada. Una confirmada puede pagarse o anularse; una pagada tambien puede anularse con motivo obligatorio.
- Una nomina confirmada, pagada o anulada debe conservar trazabilidad y no puede alterarse directamente.
- Los comprobantes se generan para nominas confirmadas, pertenecen a un empleado y deben conservar importes no negativos.
- Los cambios de salario, cargo, departamento o estado deben respetar fecha efectiva, historial laboral y periodos cerrados.
- Las ausencias aprobadas generan asistencias vinculadas solo en dias laborables. No deben sobrescribir asistencias existentes ni afectar automaticamente el salario.
- Los roles y permisos se validan tanto en la interfaz como en los servicios. Supervisor se limita a su departamento; Trabajador solo consulta o modifica su propia informacion autorizada.
- Toda operacion importante debe quedar auditada sin registrar contrasenas, hashes u otros secretos.

Implementa [CAMBIO_SOLICITADO] usando consultas SQL parametrizadas, transacciones cuando se modifiquen varias tablas y rollback ante cualquier error. No hagas cambios destructivos de base de datos ni elimines historiales.

Al finalizar:
1. Explica la regla de negocio aplicada.
2. Indica archivos modificados y por que.
3. Agrega o ajusta pruebas seguras con una base temporal cuando el cambio tenga riesgo.
4. Compila el proyecto y reporta errores o advertencias reales.
```
