# Fase 2 - Portal del trabajador

## Objetivo

La Fase 2 incorpora un portal de autoservicio para que cada trabajador consulte exclusivamente su perfil laboral, asistencias y comprobantes de pago. La identidad del empleado nunca se recibe desde un control de la interfaz: se obtiene de `SessionContext.EmployeeId` y se valida en servicios y consultas SQL.

## Permisos

El rol Trabajador recibe únicamente:

- `portal.ver`
- `portal.perfil.ver`
- `portal.asistencia.ver`
- `portal.comprobantes.ver`
- `portal.comprobantes.descargar`
- `cuenta.password.cambiar`

No recibe permisos administrativos de empleados, asistencia general, nómina, reportes, configuración o auditoría. Los demás roles conservan la matriz de la Fase 1.

## Flujo de seguridad

1. El usuario inicia sesión y `SessionContext` conserva su usuario, rol e `IdEmpleado`.
2. `AuthorizationService` exige el permiso de la operación.
3. `EmployeeScopeService` comprueba que sea Trabajador, tenga empleado asociado y que ese empleado exista y esté activo.
4. Los repositorios ejecutan SQL parametrizado con `WHERE IdEmpleado = @empleado`.
5. Las consultas, descargas, impresión, apertura de PDF y cambios de contraseña quedan auditados.

Un identificador de comprobante ajeno devuelve un rechazo controlado. El sistema no informa a qué trabajador pertenece el registro solicitado.

## Formularios

- `FrmPortalTrabajador`: resumen laboral, último pago y asistencia reciente.
- `FrmMiPerfil`: datos laborales de solo lectura.
- `FrmMisAsistencias`: filtros por fecha y estado, tabla y resumen mensual.
- `FrmMisComprobantes`: búsqueda, vista previa, descarga PDF, apertura e impresión.
- `FrmCambiarPassword`: actualización segura de la contraseña propia.

Los formularios conservan los archivos `.cs`, `.Designer.cs` y `.resx`. Los controles visuales están declarados en Designer y la lógica de eventos permanece en el code-behind.

## Contraseña y comprobantes

El cambio de contraseña verifica la clave actual, confirmación, longitud mínima, mayúscula, minúscula y número. La nueva clave se almacena con PBKDF2-HMAC-SHA256 y nunca se registra en auditoría.

La descarga de un comprobante vuelve a comprobar la propiedad antes de generar el archivo. El trabajador selecciona la ubicación mediante `SaveFileDialog`; la ruta se actualiza únicamente cuando coinciden el comprobante y el empleado de la sesión. La impresión usa `PrintDocument`, `PrintPreviewDialog` y un renderer compartido con el módulo administrativo.

La aplicación usa `PDFsharp-GDI` 6.2.4, variante oficial para Windows y .NET Framework. Conserva la API de PDFsharp y resuelve las fuentes instaladas mediante GDI+, evitando depender del resolver multiplataforma del paquete Core.

## Verificación

Ejecutar desde la raíz del proyecto:

```powershell
dotnet restore ProyFinal_LPI_Eq01_NomiCore.csproj
dotnet build ProyFinal_LPI_Eq01_NomiCore.csproj
powershell -ExecutionPolicy Bypass -File Tools\Fase1SecuritySmokeTest.ps1
powershell -ExecutionPolicy Bypass -File Tools\Fase2EmployeePortalSmokeTest.ps1
powershell -ExecutionPolicy Bypass -File Tools\Fase2FormSmokeTest.ps1
```

Los smoke tests usan bases temporales mediante `NOMINA_DB_PATH` y no alteran la base de uso normal.
