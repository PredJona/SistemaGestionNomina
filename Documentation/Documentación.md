# Sistema de Gestión de Nómina

Aplicación de escritorio académica para administrar empleados, asistencia, cálculo básico de nómina, comprobantes de pago y reportes administrativos.

## Tecnologías usadas

- Lenguaje: C#
- Interfaz: Windows Forms
- Framework: .NET Framework 4.8
- IDE objetivo: Visual Studio 2022
- Base de datos: SQLite con System.Data.SQLite.Core compatible con .NET Framework
- Exportación Excel: ClosedXML
- Exportación PDF: PDFsharp
- Gráficas: Chart nativo de Windows Forms
- Íconos: FontAwesome.Sharp

## Cómo ejecutar

1. Abrir `SistemaGestionNomina.sln` en Visual Studio 2022.
2. Restaurar paquetes NuGet si Visual Studio lo solicita.
3. Compilar la solución.
4. Ejecutar el proyecto `SistemaGestionNomina`.

La aplicación crea automáticamente `nomina.db` en la carpeta de salida de la aplicación si no existe.

## Usuario inicial

- Usuario: `admin`
- Contraseña: `admin123`
- Rol: `Admin`

La contraseña se guarda con hash SHA256 para fines académicos mediante `PasswordHelper`.

## Organización del código

El proyecto está separado por responsabilidades:

- `UI`: formularios y controles visuales.
- `Models`: entidades del dominio.
- `Data`: conexión SQLite, inicialización y repositorios.
- `Services`: lógica de negocio y exportaciones.
- `Helpers`: validaciones, tema visual, contraseñas y rutas.
- `Documentation`: documentos explicativos del proyecto.

Los formularios no ejecutan SQL directamente; llaman servicios. Los servicios consumen repositorios y los repositorios manejan SQLite con consultas parametrizadas.
