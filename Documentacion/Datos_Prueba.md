# Datos De Prueba

## Usuarios de demostracion

| Rol | Usuario | Contrasena | Empleado asociado |
| --- | --- | --- | --- |
| Administrador | `admin` | `admin123` | No aplica |
| Recursos Humanos | `rrhh` | `Rrhh2026*` | No aplica |
| Contabilidad | `contador` | `Conta2026*` | No aplica |
| Supervisor | `supervisor` | `Super2026*` | `EMP-1004 - Javier Ramirez` |
| Trabajador | `trabajador` | `Trabaja2026*` | `EMP-1001 - Carlos Mendoza` |

Estas cuentas son exclusivamente academicas. Se crean si el nombre de usuario todavia no existe y las contrasenas se almacenan mediante PBKDF2. El inicializador no reemplaza posteriormente una cuenta existente ni restablece una contrasena cambiada desde la aplicacion.

| N° | Modulo | Datos ingresados | Resultado esperado | Resultado obtenido | Estado |
| --- | --- | --- | --- | --- | --- |
| 1 | Login | Usuario `admin`, contrasena `admin123` | Acceso al sistema y apertura del dashboard | Pendiente de validacion manual | Revisar |
| 2 | Empleados | Codigo `EMP-2001`, nombre `Laura Perez`, cedula `8-200-100`, cargo `Analista`, salario `1200.00` | Empleado guardado y visible en tabla | Pendiente de validacion manual | Revisar |
| 3 | Empleados | Empleado con salario alto `3500.00` | El sistema acepta salario positivo y permite calcular nomina | Pendiente de validacion manual | Revisar |
| 4 | Empleados | Empleado con salario bajo `650.00` | El sistema acepta salario positivo y muestra valores correctos | Pendiente de validacion manual | Revisar |
| 5 | Asistencia | Estado `Puntual`, entrada `08:00`, salida `17:00` | Registro con 9 horas y estado puntual | Pendiente de validacion manual | Revisar |
| 6 | Asistencia | Estado `Tardanza`, entrada `08:45`, salida `17:00` | Registro con tardanza y horas calculadas | Pendiente de validacion manual | Revisar |
| 7 | Asistencia | Estado `Falta` sin horas | Registro sin horas trabajadas | Pendiente de validacion manual | Revisar |
| 8 | Nomina | Periodo mensual, empleados activos | Calculo de ingresos, deducciones y neto | Pendiente de validacion manual | Revisar |
| 9 | Comprobantes | Confirmar nomina y abrir comprobantes | Comprobante generado por empleado | Pendiente de validacion manual | Revisar |
| 10 | Impresion | Seleccionar comprobante y presionar Imprimir | Se abre `PrintPreviewDialog` con recibo legible | Pendiente de validacion manual | Revisar |
| 11 | Reportes | Generar reporte de empleados o nomina | Archivo PDF/Excel generado | Pendiente de validacion manual | Revisar |
| 12 | Salir | Presionar `Salir` en sidebar | Se muestra confirmacion y al aceptar se cierra la app | Pendiente de validacion manual | Revisar |
| 13 | Migraciones | Ejecutar inicializador dos veces sobre base heredada | Version 3, sin perdida de datos y con respaldo previo | Prueba automatizada aprobada | Aprobado |
| 14 | Seguridad | Iniciar con un hash SHA-256 valido | Acceso correcto y conversion inmediata a PBKDF2 | Prueba automatizada aprobada | Aprobado |
| 15 | Bloqueo | Cinco contrasenas incorrectas | Bloqueo temporal durante 15 minutos | Prueba automatizada aprobada | Aprobado |
| 16 | Permisos | Probar Admin, RRHH, Contabilidad, Supervisor y Trabajador | Cada rol recibe solamente sus permisos | Prueba automatizada aprobada | Aprobado |
| 17 | Integridad | Registrar dos asistencias para empleado y fecha iguales | Servicio y SQLite rechazan el duplicado | Prueba automatizada aprobada | Aprobado |
| 18 | Auditoria | Autenticar y ejecutar operaciones protegidas | Se registra el usuario real y los rechazos | Prueba automatizada aprobada | Aprobado |
| 19 | Supervisor | Consultar empleados desde un Supervisor asociado | Solo aparecen empleados de su departamento | Prueba automatizada aprobada | Aprobado |

## Nota

Los resultados obtenidos deben actualizarse despues de ejecutar la aplicacion graficamente y tomar capturas reales.
