# Estructura del Proyecto

## UI

Contiene los formularios Windows Forms:

- `FrmLogin`
- `FrmMain`
- `FrmDashboard`
- `FrmEmpleados`
- `FrmAsistencia`
- `FrmNomina`
- `FrmComprobantes`
- `FrmReportes`
- `FrmConfiguracion`
- `FrmAcercaDe`

También contiene controles reutilizables en `UI/Controls`, como paneles y botones redondeados.

## Models

Define las entidades principales:

- Usuario
- Departamento
- Empleado
- Asistencia
- PeriodoNomina
- Nomina
- NominaDetalle
- Comprobante
- ConfiguracionNomina
- ReporteGenerado

## Data

Responsable de persistencia:

- `SQLiteConnectionFactory`: crea conexiones a `nomina.db`.
- `DatabaseInitializer`: crea tablas y datos iniciales.
- Repositorios: ejecutan consultas SQL parametrizadas.

## Services

Contiene la lógica de negocio:

- Login
- CRUD de empleados
- Registro de asistencia
- Cálculo y confirmación de nómina
- Consulta de comprobantes
- Configuración
- Reportes
- Exportación Excel/PDF

## Helpers

Utilidades compartidas:

- `ThemeHelper`: colores, fuentes y estilos.
- `ValidationHelper`: validaciones comunes.
- `PasswordHelper`: hash SHA256.
- `PathHelper`: rutas de exportación.
