# Guia De Sustentacion

## Aplicacion

NomiCore - Sistema de Gestion de Nomina es una aplicacion Windows Forms que permite administrar empleados, registrar asistencia, calcular nomina, generar comprobantes, imprimir recibos y exportar reportes.

## Problema Resuelto

El sistema ayuda a controlar informacion laboral basica: empleados, asistencia, pagos, deducciones, comprobantes y reportes. Evita calculos manuales repetitivos y organiza la informacion en una base SQLite local.

## Tema H

El Tema H se aplica de dos formas:

- Campos privados en clases: `Empleado`, `Asistencia`, `Nomina`, `NominaDetalle` y `Comprobante`.
- Impresion con `PrintDocument`: `FrmComprobantes` dibuja el recibo con `Graphics.DrawString` y usa `PrintPreviewDialog`.

## Controles Usados

- `TextBox`: captura de usuario, contrasena, busquedas y datos de empleados.
- `ComboBox`: seleccion de departamentos, estados y filtros.
- `DateTimePicker`: fechas y horas de asistencia o periodo.
- `DataGridView`: listados de empleados, asistencia, nomina, comprobantes y reportes.
- `Button` e `IconButton`: acciones principales y navegacion.
- `Panel` y `Label`: organizacion visual, tarjetas y encabezados.

## Eventos Programados

- Login: validacion de credenciales.
- Empleados: guardar, limpiar, desactivar y exportar.
- Asistencia: registrar, filtrar, importar CSV y exportar.
- Nomina: calcular, confirmar pago y exportar.
- Comprobantes: seleccionar, exportar PDF/Excel, imprimir y crear email.
- Configuracion: guardar parametros y crear backup.
- Main: navegar, cerrar sesion y salir.

## Clases Principales

- `Empleado`: datos personales, cargo, departamento, salario y estado.
- `Asistencia`: fecha, entrada, salida, horas y estado.
- `Nomina`: totales generales y lista de detalles.
- `NominaDetalle`: calculo por empleado.
- `Comprobante`: recibo generado por nomina confirmada.
- `Usuario`: acceso al sistema.

## Servicios Y Repositorios

Los formularios no acceden directamente a SQLite. Llaman servicios como `EmpleadoService`, `AsistenciaService`, `NominaService` y `ComprobanteService`. Los servicios usan repositorios ADO.NET para consultar o guardar datos.

## SQLite

SQLite guarda usuarios, departamentos, empleados, asistencias, periodos, nominas, detalles, comprobantes, configuracion, reportes y auditoria. La base se crea automaticamente al iniciar.

## PrintDocument

`FrmComprobantes` valida que exista un comprobante seleccionado, abre vista previa y dibuja el recibo. No requiere imprimir fisicamente durante la sustentacion.

## Validaciones

Hay validaciones en formularios, servicios y modelos. Se validan campos obligatorios, salarios positivos, fechas, horas, estados permitidos y exportaciones con datos.

## Exportaciones

El sistema genera Excel con ClosedXML y PDF con PDFsharp. Tambien prepara emails `.eml` con comprobantes adjuntos.

## Preguntas Probables

**Donde se aplico el Tema H?**  
En las clases con campos privados y en la impresion de comprobantes con `PrintDocument`.

**Que son campos privados?**  
Son variables internas de una clase que no se modifican directamente desde fuera.

**Por que no usar campos publicos?**  
Porque se perderia control sobre datos invalidos. Las propiedades permiten validar antes de guardar.

**Que es encapsulamiento?**  
Es proteger los datos internos de una clase y exponer acceso controlado mediante propiedades o metodos.

**Como funciona PrintDocument?**  
Dispara el evento `PrintPage`, donde se dibuja el contenido que se va a imprimir o previsualizar.

**Que hace PrintPreviewDialog?**  
Muestra una vista previa del documento sin necesitar una impresora fisica.

**Como separaron UI, Datos y Logica?**  
Los formularios muestran y capturan datos; los servicios procesan reglas; los repositorios consultan SQLite.

**Que aporto cada integrante?**  
Jonathan Romero realizo las partes mas complejas y validaciones. Los aportes del resto quedan editables en el documento Word para que el equipo los complete con precision.
