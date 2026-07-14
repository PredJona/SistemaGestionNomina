# Fase 3 - Nómina robusta

## Objetivo

La Fase 3 incorpora un ciclo de vida completo para la nómina con estados (Borrador, Calculada, Confirmada, Pagada, Anulada), cierre de períodos, registro de pago, anulación con trazabilidad, recálculo controlado e historial de versiones. El objetivo es que ninguna nómina se elimine; todas las correcciones quedan registradas como nuevas versiones.

## Base de datos

La migración v4 (`DatabaseMigrationRunner.LatestVersion = 4`) agrega las siguientes estructuras:

### Columnas nuevas en `PeriodosNomina`

| Columna | Tipo | Descripción |
|---|---|---|
| `Cerrado` | INTEGER (0/1) | Indica si el período está cerrado |
| `FechaCierre` | TEXT NULL | Fecha y hora en que se cerró |
| `CerradoPor` | TEXT NULL | Usuario que cerró el período |

### Columnas nuevas en `Nominas`

| Columna | Tipo | Descripción |
|---|---|---|
| `FechaConfirmacion` | TEXT NULL | Momento en que se confirmó |
| `ConfirmadaPor` | TEXT NULL | Usuario que confirmó |
| `FechaPago` | TEXT NULL | Momento en que se pagó |
| `PagadaPor` | TEXT NULL | Usuario que pagó |
| `FechaAnulacion` | TEXT NULL | Momento en que se anuló |
| `AnuladaPor` | TEXT NULL | Usuario que anuló |
| `MotivoAnulacion` | TEXT NULL | Razón de la anulación |

### Tabla `NominaVersiones`

```sql
CREATE TABLE NominaVersiones (
    IdVersion INTEGER PRIMARY KEY AUTOINCREMENT,
    IdNominaOriginal INTEGER NOT NULL,
    IdNominaNueva INTEGER NULL,
    MotivoCambio TEXT NOT NULL,
    UsuarioResponsable TEXT NOT NULL,
    FechaCambio TEXT NOT NULL,
    FOREIGN KEY(IdNominaOriginal) REFERENCES Nominas(IdNomina),
    FOREIGN KEY(IdNominaNueva) REFERENCES Nominas(IdNomina)
);
```

### Índices

- `IX_Nominas_IdPeriodo_Estado` sobre `Nominas(IdPeriodo, Estado)`
- `IX_NominaVersiones_IdNominaOriginal` sobre `NominaVersiones(IdNominaOriginal)`

### Trigger de bloqueo de asistencia

```sql
CREATE TRIGGER TR_Asistencias_PeriodoCerrado_Insert
BEFORE INSERT ON Asistencias
WHEN EXISTS (SELECT 1 FROM PeriodosNomina
    WHERE Cerrado = 1
      AND FechaInicio <= NEW.Fecha
      AND FechaFin >= NEW.Fecha)
BEGIN
    SELECT RAISE(ABORT, 'No se puede registrar asistencia en un período cerrado.');
END;
```

Impide insertar asistencias en fechas que pertenezcan a un período cerrado. La validación también se aplica en código mediante `PayrollPeriodPolicyService.VerificarFechasAbiertas`.

## Permisos

Se agregaron cuatro permisos nuevos en `Permissions.cs`:

| Constante | Valor | Descripción |
|---|---|---|
| `PayrollPay` | `nomina.pagar` | Pagar una nómina confirmada |
| `PayrollAnnul` | `nomina.anular` | Anular una nómina confirmada o pagada |
| `PayrollRecalculate` | `nomina.recalcular` | Recalcular desde una nómina anulada |
| `PayrollHistoryView` | `nomina.historial.ver` | Ver historial de versiones |

### Asignación por rol (`RolePermissionService`)

- **Admin**: todos los permisos de nómina, incluidos los cuatro nuevos.
- **Contabilidad**: `PayrollPay`, `PayrollAnnul`, `PayrollRecalculate`, `PayrollHistoryView`.
- **RRHH**: solo `PayrollHistoryView` (consulta histórica).
- **Trabajador**: ninguno de estos (solo accede al portal).

## Máquina de estados

```
                    Recalcular (crea nueva nómina)
                    ┌──────────────────────────────┐
                    │                              ▼
  Borrador ──► Calculada ──► Confirmada ──► Pagada
                   │              │              │
                   │              │              │
                   │              ▼              ▼
                   │           Anulada ◄──────────┘
                   │              │
                   └──────────────┘
                        (reabre período)
```

### Transiciones permitidas

| Desde | Hacia | Requisito |
|---|---|---|
| Calculada | Confirmada | Haber calculado la nómina |
| Confirmada | Pagada | Estar confirmada |
| Confirmada | Anulada | Motivo obligatorio |
| Pagada | Anulada | Motivo obligatorio |
| Anulada | Recalcular | Tener versión pendiente |

## Servicios

### `PayrollPeriodPolicyService`

Ubicado en `Services/PayrollPeriodPolicyService.cs`. Valida que un período esté abierto antes de permitir operaciones.

- `EstaAbierto(DateTime fecha)` — retorna `true` si la fecha no pertenece a un período cerrado.
- `EstaAbierto(DateTime inicio, DateTime fin)` — igual pero por rango.
- `VerificarPeriodoAbierto(int idPeriodo)` — lanza excepción si el período está cerrado.
- `VerificarFechasAbiertas(DateTime inicio, DateTime fin)` — lanza excepción si el rango pertenece a un período cerrado.

### `PayrollLifecycleService`

Ubicado en `Services/PayrollLifecycleService.cs`. Centraliza toda la lógica del ciclo de vida de nómina con autorización.

#### Confirmar

```csharp
public int Confirmar(Nomina nominaCalculada, DateTime fechaInicio, DateTime fechaFin)
```

1. Exige permiso `PayrollConfirm`.
2. Valida que la nómina esté en estado `Calculada` y tenga detalles.
3. Valida que el rango de fechas esté abierto (no pertenezca a un período cerrado).
4. En una **transacción SQLite**:
   - Inserta `PeriodosNomina` con `Cerrado = 1`, `FechaCierre` y `CerradoPor`.
   - Inserta `Nominas` con estado `Confirmada`, `FechaConfirmacion` y `ConfirmadaPor`.
   - Inserta cada `NominaDetalle`.
   - Inserta un `Comprobante` por cada empleado (con número `COMP-{IdNomina}-{IdEmpleado}`).
5. Registra auditoría de confirmación y generación de comprobantes.
6. Retorna el `IdNomina` generado.

#### Pagar

```csharp
public void Pagar(int idNomina)
```

1. Exige permiso `PayrollPay`.
2. Valida que la nómina exista y esté en estado `Confirmada`.
3. Actualiza el estado a `Pagada`, establece `FechaPago` y `PagadaPor`.
4. Registra auditoría.

#### Anular

```csharp
public void Anular(int idNomina, string motivo)
```

1. Exige permiso `PayrollAnnul`.
2. Valida que la nómina exista y esté en estado `Confirmada` o `Pagada`.
3. Actualiza el estado a `Anulada`, establece `FechaAnulacion`, `AnuladaPor` y `MotivoAnulacion`.
4. Crea un registro en `NominaVersiones` con `IdNominaOriginal` = idNomina y `IdNominaNueva` = null.
5. Si el período asociado estaba cerrado, lo reabre (`Cerrado = 0`).
6. Registra auditoría.

#### Recalcular

```csharp
public int Recalcular(int idNominaAnulada, DateTime fechaInicio, DateTime fechaFin, Nomina nominaRecalculada)
```

1. Exige permiso `PayrollRecalculate`.
2. Valida que la nómina original exista y esté en estado `Anulada`.
3. Busca en `NominaVersiones` una versión con `IdNominaOriginal = idNominaAnulada` y `IdNominaNueva IS NULL` (versión pendiente).
4. Llama internamente a `Confirmar` para crear la nueva nómina.
5. Actualiza la versión pendiente con el `IdNominaNueva` generado.
6. Retorna el nuevo `IdNomina`.

#### ObtenerHistorial

```csharp
public List<NominaVersion> ObtenerHistorial(int idNomina)
```

1. Exige permiso `PayrollHistoryView`.
2. Usa `NominaVersionRepository.GetHistorialChain(idNomina)` que recorre la cadena hacia atrás: busca la versión donde `IdNominaNueva = idNomina`, luego continúa con el `IdNominaOriginal` de esa versión, y así sucesivamente hasta llegar al origen.
3. Retorna todas las versiones de la cadena, no solo una fila.

### `NominaVersionRepository`

Métodos disponibles:

| Método | Descripción |
|---|---|
| `Add(NominaVersion)` | Inserta una nueva versión |
| `GetByIdNominaOriginal(int)` | Versiones donde `IdNominaOriginal = id` |
| `UpdateIdNominaNueva(int idVersion, int idNominaNueva)` | Vincula la nómina nueva a una versión |
| `GetHistorialChain(int idNomina)` | Recorre toda la cadena de versiones hacia atrás |

## Formularios

### `FrmNomina` (modificaciones)

Se agregaron los siguientes controles en el Designer (`FrmNomina.Designer.cs`):

- `btnPagar` — Solo visible cuando la nómina está en estado `Confirmada`.
- `btnAnular` — Solo visible cuando la nómina está en estado `Confirmada` o `Pagada`.
- `btnRecalcular` — Solo visible cuando la nómina está en estado `Anulada`.
- `btnVerHistorial` — Siempre visible (si hay nómina seleccionada).
- `lblEstadoNomina` — Muestra el estado actual de la nómina seleccionada.

La visibilidad de los botones se actualiza en `ActualizarEstadoBotones()` según el estado de la nómina actual.

### `FrmAnularNomina`

Diálogo autocontenido (sin `.Designer.cs` separado). Muestra:

- Resumen de la nómina (nombre, estado, total neto, cantidad de empleados).
- Advertencia en rojo: "La nómina anterior seguirá existiendo como registro histórico."
- Cuadro de texto multilínea para el motivo (obligatorio).
- Botón rojo "Anular" (confirma con `DialogResult.OK`).
- Botón "Cancelar".

### `FrmHistorialNomina`

Diálogo con un `DataGridView` de solo lectura con estilo oscuro (forecolor claro, headers en morado, renglones alternos). Columnas:

| Columna | Contenido |
|---|---|
| Versión | `IdVersion` |
| Original → Nueva | Ejemplo: `7 → 9` |
| Motivo | Razón del cambio |
| Usuario | Quién lo hizo |
| Fecha | `yyyy-MM-dd HH:mm` |

Al abrir desde la nómina actual se muestran **todas las versiones de la cadena** (recálculos previos), no solo la directamente vinculada.

## Reglas de negocio

| Operación | Validación |
|---|---|
| Confirmar | La nómina debe estar en estado `Calculada` y tener detalles. El período de fechas no debe estar cerrado. |
| Pagar | La nómina debe estar en estado `Confirmada`. |
| Anular | La nómina debe estar en estado `Confirmada` o `Pagada`. El motivo es obligatorio. |
| Recalcular | La nómina original debe estar en estado `Anulada` y debe existir una versión pendiente. |
| Registrar asistencia | No se permite insertar asistencias en fechas que pertenezcan a un período cerrado. |
| Editar período cerrado | No se permite modificar períodos con `Cerrado = 1` mediante `PayrollPeriodPolicyService`. |

## Verificación

Ejecutar desde la raíz del proyecto:

```powershell
dotnet restore Proy2_Eq01_CamposPD.csproj
dotnet build Proy2_Eq01_CamposPD.csproj
powershell -ExecutionPolicy Bypass -File Tools\Fase3NominaRobustaSmokeTest.ps1
```

Los smoke tests usan bases temporales mediante `NOMINA_DB_PATH` y no alteran la base de uso normal.
