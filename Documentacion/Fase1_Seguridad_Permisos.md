# Fase 1 - Seguridad, Permisos Y Auditoria

## Estado

La Fase 1 agrega una base versionada y segura sin cambiar .NET Framework 4.8,
Windows Forms, SQLite ni los calculos actuales de nomina.

## Migraciones

SQLite utiliza `PRAGMA user_version` y actualmente llega a la version 3:

1. Seguridad de usuarios: relacion con empleados, ultimo acceso, intentos y bloqueo.
2. Indices y proteccion contra asistencias duplicadas.
3. Validaciones de roles, asociaciones, salarios, departamentos y horas.

Antes de actualizar una base existente se guarda una copia en
`Backups/Migrations` junto con su archivo SHA-256. Cada migracion se ejecuta en
una transaccion y queda registrada en `MigracionesLog`.

Los duplicados historicos de asistencia no se eliminan. Si existen, se
conservan y los triggers impiden agregar nuevos duplicados.

## Matriz De Acceso

| Rol | Acceso |
| --- | --- |
| Admin | Dashboard, todos los modulos, configuracion y auditoria |
| RRHH | Empleados, asistencia, comprobantes y reportes de personal |
| Contabilidad | Nomina, comprobantes y reportes financieros |
| Supervisor | Asistencia del departamento asociado |
| Trabajador | Acerca de, salir y cerrar sesion hasta el portal del Dia 2 |

Los permisos se validan en `FrmMain`, en los controles de cada formulario y en
los servicios. Ocultar un boton no permite saltarse la autorizacion mediante
una llamada directa.

## Inicio De Sesion

- Las contrasenas nuevas usan PBKDF2-HMAC-SHA256, sal aleatoria y 120 000 iteraciones.
- Los hashes SHA-256 anteriores siguen funcionando y se convierten a PBKDF2 despues de un acceso correcto.
- Cinco intentos incorrectos bloquean la cuenta durante 15 minutos.
- Los usuarios inactivos, bloqueados o con rol desconocido no pueden entrar.
- Recordar usuario guarda solamente el nombre de usuario en LocalAppData.
- La auditoria nunca almacena contrasenas ni hashes.

## Preparar Cuentas Manuales

Primero compile la aplicacion y luego ejecute:

```powershell
dotnet build Proy2_Eq01_CamposPD.csproj
powershell -ExecutionPolicy Bypass -File Tools/PrepararUsuariosPruebaFase1.ps1
```

El script muestra los empleados activos, solicita las asociaciones para
Supervisor y Trabajador y pide cada contrasena de forma segura. No existen
contrasenas predeterminadas para esos roles.

## Pruebas Automatizadas

```powershell
powershell -ExecutionPolicy Bypass -File Tools/Fase1SecuritySmokeTest.ps1
powershell -ExecutionPolicy Bypass -File Tools/FormSmokeTest.ps1
powershell -ExecutionPolicy Bypass -File Tools/AdvancedServicesSmokeTest.ps1
```

`Fase1SecuritySmokeTest.ps1` usa una base temporal y valida migraciones
idempotentes, respaldo, PBKDF2, compatibilidad SHA-256, bloqueo, permisos,
rechazo en servicios, duplicados, auditoria y alcance departamental.

## Preparado Para El Dia 2

`Usuarios.IdEmpleado`, `SessionContext`, los roles y el alcance por empleado ya
permiten construir el portal del trabajador sin exponer datos globales. El rol
Trabajador queda deliberadamente sin modulos administrativos hasta completar
ese portal.
