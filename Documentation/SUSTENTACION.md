# Sustentacion Del Proyecto

## Datos Generales

- Proyecto: Sistema de Nomina Empresarial con Generacion e Impresion de Comprobantes de Pago.
- Tema: H - Manejo de los campos en las clases + PrintDocument.
- Universidad: Universidad Tecnologica de Panama.
- Facultad: Facultad de Ingenieria de Sistemas Computacionales.
- Curso: Lenguaje de Programacion I.
- Profesora: Anna Araba de Ruiz.
- Grupo: 1SF121.
- Equipo: Eq__.
- Herramientas: Visual Studio 2022, Windows Forms, .NET Framework 4.8, C# 7.3 y SQLite.

## Como Defender La Programacion Orientada A Objetos

El proyecto separa responsabilidades por capas. Los formularios muestran informacion y capturan datos, los servicios aplican reglas de negocio, los repositorios acceden a SQLite y los modelos representan los datos principales del dominio.

Las clases principales `Empleado`, `Asistencia`, `Nomina`, `NominaDetalle` y `Comprobante` usan campos privados y propiedades publicas validadas. Esto permite defender encapsulamiento: el dato no se modifica directamente, sino a traves de una propiedad que revisa valores obligatorios, estados permitidos, fechas validas y montos no negativos.

## Tema H: Campos En Clases

Ejemplos defendibles:

- `Empleado` valida codigo, nombre, cedula, cargo, salario, estado y fecha de ingreso.
- `Asistencia` valida empleado, fecha, horas y estados permitidos: `Puntual`, `Tardanza`, `Falta`, `Permiso`.
- `NominaDetalle` evita montos negativos en sueldo, bonos, horas extra, deducciones y neto.
- `Comprobante` valida numero, fecha y totales.

Los nombres publicos de las propiedades se conservaron para que los formularios, repositorios, exportaciones y reportes sigan funcionando.

## PrintDocument

`FrmComprobantes` usa un `PrintDocument` de instancia. El evento `PrintPage` se suscribe una sola vez en el constructor y el usuario abre `PrintPreviewDialog` antes de imprimir. El recibo se dibuja con `Graphics.DrawString`, lineas y fuentes, por lo que se puede explicar claramente el flujo:

1. El usuario selecciona un comprobante.
2. El sistema valida que exista una seleccion.
3. Se abre la vista previa de impresion.
4. `PrintPage` dibuja encabezado, empleado, periodo, ingresos, deducciones, neto y firma.

## Calculo De Nomina

La formula academica esta en `NominaService.CalcularNomina`:

- Sueldo base del empleado.
- Bono academico del 3%.
- Horas extra segun asistencia y recargo configurado.
- Deducciones configurables: seguro social, ISR y seguro educativo.
- Neto a pagar: ingresos menos deducciones.

Al confirmar el pago, el sistema crea periodo, nomina, detalle y comprobantes.

## SQLite Y Separacion Por Capas

SQLite se crea automaticamente al iniciar el sistema. Los repositorios usan consultas parametrizadas y `using`, lo que reduce errores de conexion y evita concatenar valores del usuario en SQL. Los formularios no consultan la base directamente: llaman servicios.

## Controles Windows Forms Usados

- `DataGridView` para tablas de empleados, asistencias, nomina, comprobantes y reportes.
- `TextBox`, `ComboBox`, `DateTimePicker` y botones para captura.
- `Panel` y labels para tarjetas visuales.
- `PrintPreviewDialog` y `PrintDocument` para impresion.
- Botones con iconos FontAwesome en navegacion y acciones.

## Preguntas Probables

**Por que SQLite?**  
Porque permite una base local simple, portable y suficiente para el alcance academico.

**Donde esta la validacion principal?**  
En formularios para mensajes inmediatos y en entidades/servicios para proteger la logica.

**Que pasa si se intenta imprimir sin seleccionar comprobante?**  
El sistema muestra un `MessageBox` y no abre la vista previa.

**Que diferencia hay entre cerrar sesion y salir?**  
Cerrar sesion vuelve al flujo de autenticacion. Salir confirma y cierra la aplicacion completa.

**Que clases demuestran encapsulamiento?**  
`Empleado`, `Asistencia`, `Nomina`, `NominaDetalle` y `Comprobante`.

## Alcance Defendible

El sistema cumple el objetivo academico: administra empleados, registra asistencia, calcula nomina, genera comprobantes, exporta documentos y demuestra impresion con `PrintDocument`. Los modulos avanzados quedan aislados y no afectan el flujo principal.
