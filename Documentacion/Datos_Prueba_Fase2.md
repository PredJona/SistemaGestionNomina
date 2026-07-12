# Datos de prueba - Fase 2

## Preparación

Use `Tools/PrepararUsuariosPruebaFase1.ps1` para crear un usuario con rol `Trabajador`, contraseña definida durante la ejecución y asociación a un empleado activo. No escriba credenciales en archivos del proyecto.

## Casos manuales

| Caso | Acción | Resultado esperado |
|---|---|---|
| Trabajador asociado | Iniciar sesión con una cuenta vinculada a empleado activo | Se abre `Mi portal` automáticamente |
| Menú del trabajador | Revisar el sidebar | Solo aparecen Mi portal, Acerca de, Salir y Cerrar sesión |
| Perfil propio | Abrir Mi perfil | Datos laborales de solo lectura del empleado asociado |
| Asistencia propia | Filtrar por mes y estado | Solo aparecen registros del trabajador autenticado |
| Resumen | Revisar tarjetas de asistencia | Totales de puntualidad, tardanza, falta, permiso y horas extra |
| Comprobantes | Buscar por número o período | Solo aparecen comprobantes del trabajador autenticado |
| Descargar PDF | Seleccionar comprobante y ubicación | Se crea un PDF legible en la ubicación seleccionada |
| Abrir PDF | Abrir un PDF previamente descargado | El sistema abre el archivo asociado al comprobante propio |
| Imprimir | Abrir vista previa | `PrintPreviewDialog` muestra el comprobante sin imprimir automáticamente |
| Cambio válido | Usar clave actual y una nueva clave segura | Se confirma el cambio y se limpian los campos |
| Clave anterior | Cerrar sesión e intentar la contraseña anterior | El acceso es rechazado |
| Clave nueva | Iniciar sesión con la contraseña nueva | El acceso es correcto |
| Sin vínculo | Iniciar sesión con Trabajador sin `IdEmpleado` | Acceso personal rechazado con mensaje controlado |
| Empleado inactivo | Iniciar sesión con cuenta vinculada a empleado inactivo | Acceso personal rechazado |

## Datos usados por el smoke test

La prueba automática crea dos empleados aislados, asistencias para ambos, un período pagado, dos detalles de nómina y un comprobante por empleado. Después verifica que el primer trabajador no puede consultar el comprobante ni la asistencia del segundo.

Las contraseñas del smoke test existen únicamente dentro de una base temporal eliminada al finalizar. La prueba comprueba además que auditoría no contiene contraseñas ni hashes.
