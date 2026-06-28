# Analisis Contra Guia 05 - Proyecto 2

## Resumen

Proyecto revisado: NomiCore - Sistema de Gestion de Nomina.  
Guia usada: Guia de Actividad N° 5 - Proyecto N° 2, Lenguaje de Programacion I.  
Resultado tecnico actual: el proyecto compila en .NET Framework 4.8 con Windows Forms y SQLite.

## Estructura Revisada

- Formularios: `FrmLogin`, `FrmMain`, `FrmDashboard`, `FrmEmpleados`, `FrmAsistencia`, `FrmNomina`, `FrmComprobantes`, `FrmReportes`, `FrmConfiguracion`, `FrmAcercaDe`.
- Modelos: `Usuario`, `Departamento`, `Empleado`, `Asistencia`, `PeriodoNomina`, `Nomina`, `NominaDetalle`, `Comprobante`, `ConfiguracionNomina`, `ReporteGenerado`.
- Servicios: autenticacion, empleados, asistencia, nomina, comprobantes, reportes, Excel, PDF, correo, backup, auditoria, permisos y reportes avanzados.
- Datos: `SQLiteConnectionFactory`, `DatabaseInitializer`, repositorios ADO.NET y base local `nomina.db`.
- Helpers: tema visual, estilos, validaciones, rutas y contrasenas.
- Librerias NuGet: `System.Data.SQLite.Core`, `ClosedXML`, `PDFsharp`, `ScottPlot.WinForms`, `FontAwesome.Sharp`, `MaterialSkin.2`, `ReaLTaiizor`.

## Tabla De Cumplimiento

| Requisito de la guia | Estado | Evidencia encontrada en el proyecto | Accion realizada o recomendada |
| --- | --- | --- | --- |
| Aplicacion C# modo grafico tipo escritorio | Cumple | Proyecto Windows Forms `.NET Framework 4.8` con `UseWindowsForms=true`. | Mantener estructura actual. |
| Problema planteado y resuelto | Cumple | NomiCore gestiona empleados, asistencia, nomina, comprobantes y reportes. | Documentado en resultados y sustentacion. |
| Formularios necesarios | Cumple | Existen 10 formularios principales con `.Designer.cs`. | Se verificara con smoke test. |
| Diseno agradable al usuario | Cumple | Tema oscuro, sidebar, cards, tablas e iconos FontAwesome. | Mantener sin redisenar desde cero. |
| Formulario con informacion general | Cumple | `FrmAcercaDe` contiene UTP, facultad, curso, profesora, grupo, equipo, integrantes, tema, problema, Visual Studio y boton cerrar. | Se ajusto nombre visible a NomiCore. |
| Programacion Orientada a Objetos | Cumple | Modelos, servicios, repositorios y formularios separados. | Explicado en `Documentacion/SUSTENTACION.md`. |
| Separacion UI, Datos y Logica | Cumple | UI llama servicios; servicios usan repositorios; repositorios usan SQLite. | Documentado en Word y sustentacion. |
| Diagrama UML de clases | Cumple | `Documentacion/UML/DiagramaClases.md`. | Crear diagrama claro, no gigante. |
| Buenas practicas de nombres de controles | Cumple | Controles funcionales usan prefijos como `btn`, `txt`, `cmb`, `dgv`, `lbl`. | No renombrar controles ya usados. |
| Labels adecuados | Cumple | Formularios tienen labels para campos y secciones. | Revisar manualmente en Visual Studio. |
| Titulos correctos de formularios | Cumple | Formularios tienen `Text` y titulos visibles acordes al modulo. | Se ajusto nombre principal a NomiCore. |
| Validaciones de entrada | Cumple | `ValidationHelper`, validaciones en entidades y servicios. | Mantener mensajes claros con `MessageBox`. |
| Comentarios adecuados | Cumple | Comentarios breves en calculo de nomina, impresion y flujo clave. | Evitar comentarios innecesarios. |
| Boton Salir o Terminar | Cumple | `FrmMain` tiene `btnSalir` con confirmacion y `Application.Exit()`. | Diferente de cerrar sesion. |
| Ejecucion y resultados obtenidos | Parcial | Build y smoke tests automatizados disponibles. | Validar flujo grafico manualmente y tomar capturas reales. |
| Tabla de datos usados para pruebas | Cumple | `Documentacion/Datos_Prueba.md`. | Resultados graficos marcados como validacion manual si no se ejecutan. |
| Evidencia mediante capturas | Parcial | Carpeta `Documentacion/Capturas/` e instrucciones. | Tomar capturas reales manualmente; no se inventan imagenes. |
| Aporte de integrantes | Parcial | Integrantes visibles en `FrmAcercaDe`. | Word deja aportes editables; indica que Jonathan Romero realizo partes complejas y validaciones. |
| Conclusion | Cumple | Incluida en `Proy2_Eq__Resultados.docx`. | Editar si el equipo desea ajustar tono. |
| Recomendaciones | Cumple | Incluidas en `Proy2_Eq__Resultados.docx`. | Editar si el equipo desea ajustar alcance. |
| Tema H: campos en clases + PrintDocument | Cumple | `Empleado`, `Asistencia`, `Nomina`, `NominaDetalle`, `Comprobante` usan campos privados y propiedades validadas; `FrmComprobantes` usa `PrintDocument`, `PrintPage`, `Graphics.DrawString` y `PrintPreviewDialog`. | Punto fuerte para sustentacion. |

## Correcciones Aplicadas

- Se centralizo el nombre academico como NomiCore - Sistema de Gestion de Nomina.
- Se preparo documentacion de entrega en `Documentacion/`.
- Se preparo Word editable `Proy2_Eq__Resultados.docx`.
- Se preparo carpeta de capturas con instrucciones para evidencia real.
- Se actualizo la estructura final de entrega y el reporte de revision.
- Se mantuvo el proyecto Windows Forms con `.Designer.cs`, sin reescritura desde cero.

## Pendiente De Validacion Manual

No se inventaron capturas. El estudiante debe ejecutar la aplicacion, iniciar sesion y tomar las capturas reales indicadas en `Documentacion/Capturas/README_CAPTURAS.md`.
