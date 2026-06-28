# Estructura De Entrega

## Archivos Principales

- `SistemaGestionNomina.sln`: solucion de Visual Studio.
- `SistemaGestionNomina.csproj`: proyecto Windows Forms .NET Framework 4.8.
- `Program.cs`: punto de entrada, inicializacion de base de datos y flujo de login.
- `nomina.db`: base SQLite generada en ejecucion.

## Carpetas

- `Models`: entidades del dominio y validaciones de campos.
- `Data`: conexion, inicializacion y repositorios SQLite.
- `Services`: reglas de negocio, exportaciones, comprobantes y modulos avanzados seguros.
- `UI`: formularios Windows Forms con `.cs`, `.Designer.cs` y `.resx`.
- `UI/Controls`: controles reutilizables visuales.
- `Helpers`: tema, estilos, validaciones y utilidades comunes.
- `Exports/Excel`: archivos Excel generados.
- `Exports/PDF`: reportes PDF generados.
- `Exports/Comprobantes`: comprobantes de pago exportados.
- `Documentation`: documentos de base de datos, sustentacion, UML y revision.
- `Tools`: herramientas auxiliares para preparar entrega.
- `CodigosFuentePDF`: salida generada por el script de codigo fuente imprimible.

## Formularios

- `FrmLogin`: autenticacion.
- `FrmMain`: navegacion, sidebar, cerrar sesion y salir.
- `FrmDashboard`: resumen del sistema.
- `FrmEmpleados`: mantenimiento de empleados.
- `FrmAsistencia`: registro y consulta de asistencia.
- `FrmNomina`: calculo y confirmacion de pagos.
- `FrmComprobantes`: vista previa, exportacion e impresion de comprobantes.
- `FrmReportes`: generacion y exportacion de reportes.
- `FrmConfiguracion`: parametros de nomina.
- `FrmAcercaDe`: datos academicos del proyecto.

## Verificacion Recomendada

1. Ejecutar `dotnet restore SistemaGestionNomina.csproj`.
2. Ejecutar `dotnet build SistemaGestionNomina.csproj`.
3. Iniciar la app y entrar con `admin/admin123`.
4. Probar navegacion, empleados, asistencia, nomina, comprobantes, reportes y salida.
5. Abrir la vista previa de impresion desde `FrmComprobantes`.
