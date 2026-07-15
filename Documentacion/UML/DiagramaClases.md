# Diagrama UML - ProyFinal_LPI_Eq01_NomiCore

Este diagrama representa las clases principales que existen en la aplicacion **NomiCore - Sistema de Gestion de Nomina**. Se priorizan las entidades del dominio, los servicios que contienen reglas de negocio, los repositorios SQLite, la seguridad y los formularios que usan el usuario.

## 1. Modelo del dominio

```mermaid
classDiagram
    class Usuario {
        +int IdUsuario
        +string NombreUsuario
        +string Rol
        +int? IdEmpleado
        +bool Bloqueado
        +DateTime? UltimoAcceso
    }

    class Departamento {
        +int IdDepartamento
        +string Nombre
        +string Descripcion
    }

    class Empleado {
        -string codigo
        -string nombre
        -decimal salarioBase
        +int IdEmpleado
        +string Codigo
        +string Nombre
        +string Cedula
        +string Cargo
        +decimal SalarioBase
        +bool Activo
        +DateTime FechaEfectivaLaboral
    }

    class Asistencia {
        -int idEmpleado
        -DateTime fecha
        -decimal horasTrabajadas
        +int IdAsistencia
        +int IdEmpleado
        +DateTime Fecha
        +decimal HorasTrabajadas
        +string Estado
        +int? IdSolicitudAusencia
    }

    class PeriodoNomina {
        +int IdPeriodo
        +DateTime FechaInicio
        +DateTime FechaFin
        +string Estado
    }

    class Nomina {
        -decimal totalIngresos
        -decimal totalDeducciones
        +int IdNomina
        +int IdPeriodo
        +string Estado
        +decimal TotalIngresos
        +decimal TotalDeducciones
        +decimal TotalNeto
    }

    class NominaDetalle {
        -decimal salarioBase
        -decimal netoPagar
        +int IdDetalle
        +int IdNomina
        +int IdEmpleado
        +decimal SalarioBase
        +decimal HorasExtra
        +decimal TotalDeducciones
        +decimal NetoPagar
        +string NombreEmpleadoSnapshot
    }

    class Comprobante {
        -string numeroComprobante
        -decimal netoPagar
        +int IdComprobante
        +int IdNomina
        +int IdEmpleado
        +string NumeroComprobante
        +decimal NetoPagar
        +string RutaPdf
    }

    class HistorialEmpleado {
        +int IdHistorial
        +int IdEmpleado
        +DateTime FechaEfectiva
        +string CampoModificado
        +string ValorAnterior
        +string ValorNuevo
        +bool Aplicado
    }

    class SolicitudAusencia {
        +int IdSolicitudAusencia
        +int IdEmpleado
        +string Tipo
        +string Estado
        +DateTime FechaInicio
        +DateTime FechaFin
        +string Motivo
    }

    class AuditRecord {
        +int IdAuditoria
        +int? IdUsuario
        +string Modulo
        +string Accion
        +DateTime Fecha
        +string Detalle
    }

    Departamento "1" --> "0..*" Empleado : agrupa
    Usuario "0..1" --> "1" Empleado : se asocia a
    Empleado "1" --> "0..*" Asistencia : registra
    Empleado "1" --> "0..*" HistorialEmpleado : conserva cambios
    Empleado "1" --> "0..*" SolicitudAusencia : solicita
    SolicitudAusencia "0..1" --> "0..*" Asistencia : origina
    PeriodoNomina "1" --> "0..*" Nomina : contiene
    Nomina "1" *-- "1..*" NominaDetalle : detalla
    Empleado "1" --> "0..*" NominaDetalle : corresponde a
    Nomina "1" --> "0..*" Comprobante : genera
    Empleado "1" --> "0..*" Comprobante : recibe
    Usuario "0..1" --> "0..*" AuditRecord : deja trazabilidad
```

`Empleado`, `Asistencia`, `Nomina`, `NominaDetalle` y `Comprobante` aplican el Tema H: manejan campos privados y propiedades publicas validadas. De esta forma, los montos, codigos, fechas, estados y datos esenciales se validan antes de persistirse.

## 2. Capas de la aplicacion

```mermaid
flowchart TB
    subgraph UI[Interfaz Windows Forms]
        Login[FrmLogin]
        Main[FrmMain]
        Adm[Empleados, Asistencia, Nomina, Comprobantes, Reportes]
        Portal[Portal Trabajador, Mis Asistencias, Mis Comprobantes]
        RRHH[Historial Empleado, Cambio Laboral, Gestion Ausencias]
        Audit[FrmAuditoria]
    end

    subgraph Security[Seguridad y sesion]
        Session[SessionContext]
        Authz[AuthorizationService]
        Scope[EmployeeScopeService]
        Roles[Roles y Permissions]
    end

    subgraph Services[Reglas de negocio]
        Auth[AuthService y AccountService]
        Employees[EmpleadoService y EmployeeHistoryService]
        Attendance[AsistenciaService y AbsenceRequestService]
        Payroll[NominaService y PayrollLifecycleService]
        Payslips[ComprobanteService y EmployeePortalService]
        Exports[ExcelExportService y PdfExportService]
        AuditService[AuditTrailService]
    end

    subgraph Data[Acceso a datos]
        Repos[Repositorios de usuarios, empleados, asistencia, nomina y comprobantes]
        Migrations[DatabaseInitializer y DatabaseMigrationRunner]
        SQLite[(SQLite nomina.db)]
    end

    UI --> Security
    UI --> Services
    Security --> Services
    Services --> Repos
    Services --> Migrations
    Repos --> SQLite
    Migrations --> SQLite
```

## 3. Servicios y repositorios principales

```mermaid
classDiagram
    class AuthService {
        +Authenticate(username, password) AuthenticationResult
    }
    class AccountService {
        +ChangeOwnPassword(current, new, confirmation)
    }
    class EmpleadoService {
        +Save(empleado)
        +GetAll()
    }
    class EmployeeHistoryService {
        +SaveChange(context) EmployeeSaveResult
        +GetHistory(query)
    }
    class AsistenciaService {
        +Register(asistencia)
        +GetFiltered(...)
    }
    class AbsenceRequestService {
        +Create(request)
        +Approve(id, reason)
        +Reject(id, reason)
        +Cancel(id, reason)
    }
    class NominaService {
        +Calculate(periodo)
        +Confirm(idNomina)
    }
    class PayrollLifecycleService {
        +Pay(idNomina, motivo)
        +Void(idNomina, motivo)
        +Recalculate(idNomina)
    }
    class ComprobanteService {
        +GetByNomina(idNomina)
    }
    class EmployeePortalService {
        +GetDashboard()
        +GetMyProfile()
        +GetMyAttendance(...)
        +DownloadMyPayslipPdf(id)
    }
    class AuditTrailService {
        +Log(...)
    }
    class UsuarioRepository
    class EmpleadoRepository
    class AsistenciaRepository
    class NominaRepository
    class ComprobanteRepository
    class EmployeeHistoryRepository
    class AbsenceRequestRepository
    class AuditRepository
    class AuthenticationRepository

    AuthService --> AuthenticationRepository
    AccountService --> AuthenticationRepository
    EmpleadoService --> EmpleadoRepository
    EmployeeHistoryService --> EmployeeHistoryRepository
    EmployeeHistoryService --> EmpleadoRepository
    AsistenciaService --> AsistenciaRepository
    AbsenceRequestService --> AbsenceRequestRepository
    AbsenceRequestService --> AsistenciaRepository
    NominaService --> NominaRepository
    PayrollLifecycleService --> NominaRepository
    PayrollLifecycleService --> ComprobanteRepository
    ComprobanteService --> ComprobanteRepository
    EmployeePortalService --> ComprobanteRepository
    EmployeePortalService --> AsistenciaRepository
    AuditTrailService --> AuditRepository
```

## 4. Seguridad y permisos

```mermaid
classDiagram
    class SessionContext {
        +CurrentUser Usuario
        +EmployeeId int?
        +IsAuthenticated bool
        +Start(usuario)
        +Clear()
    }
    class AuthorizationService {
        +HasPermission(permission) bool
        +DemandPermission(permission)
        +DemandAny(permissions)
    }
    class EmployeeScopeService {
        +RequireCurrentEmployeeId() int
        +DemandCurrentEmployee(employeeId)
        +DemandSupervisorDepartment(employeeId)
    }
    class Roles {
        <<static>>
        +Admin
        +RRHH
        +Contabilidad
        +Supervisor
        +Trabajador
    }
    class Permissions {
        <<static>>
        +Empleados
        +Asistencia
        +Nomina
        +Comprobantes
        +Reportes
        +Portal
        +Auditoria
    }
    class AuditTrailService {
        +Log(...)
    }

    AuthorizationService --> SessionContext
    AuthorizationService --> Roles
    AuthorizationService --> Permissions
    EmployeeScopeService --> SessionContext
    AuthorizationService --> AuditTrailService : registra rechazos
```

## Notas para la sustentacion

- La interfaz no contiene SQL ni calculos de negocio: los formularios llaman servicios.
- Los servicios usan repositorios con consultas parametrizadas y SQLite como almacenamiento local.
- `SessionContext`, `AuthorizationService` y `EmployeeScopeService` protegen los modulos por rol y por alcance del empleado.
- `AuditTrailService` registra operaciones relevantes sin guardar contrasenas ni hashes.
- `ComprobantePrintRenderer`, `PdfExportService` y `PrintDocument` reutilizan la logica de presentacion de comprobantes para vista previa, impresion y PDF.
