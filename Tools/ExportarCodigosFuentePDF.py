"""Genera un PDF paginado con todos los archivos fuente C# del proyecto."""

from __future__ import print_function

import os
from datetime import datetime
from pathlib import Path

from reportlab.lib.colors import HexColor, black, white
from reportlab.lib.pagesizes import landscape, letter
from reportlab.pdfbase.pdfmetrics import stringWidth
from reportlab.pdfgen import canvas


PROJECT_ROOT = Path(__file__).resolve().parent.parent
OUTPUT_PATH = PROJECT_ROOT / "CodigosFuentePDF" / "CodigoFuente.pdf"
EXCLUDED_PARTS = {".git", ".vs", "bin", "obj", "packages", "CodigosFuentePDF"}
PAGE_SIZE = landscape(letter)
PAGE_WIDTH, PAGE_HEIGHT = PAGE_SIZE
LEFT = 42
RIGHT = 42
TOP = 44
BOTTOM = 38
FONT = "Courier"
FONT_SIZE = 7.1
LINE_HEIGHT = 8.7
CODE_WIDTH = PAGE_WIDTH - LEFT - RIGHT - 42
NUMBER_WIDTH = 36


def source_files():
    """Return application C# files while omitting build and repository artifacts."""
    files = []
    for path in PROJECT_ROOT.rglob("*.cs"):
        relative = path.relative_to(PROJECT_ROOT)
        if any(part in EXCLUDED_PARTS for part in relative.parts):
            continue
        files.append(path)
    return sorted(files, key=lambda item: str(item.relative_to(PROJECT_ROOT)).lower())


def printable(value):
    """Keep the generated PDF readable with the built-in Courier font."""
    return value.encode("cp1252", "replace").decode("cp1252")


def wrap_code_line(line):
    """Split only visual PDF lines; source text itself remains unchanged on disk."""
    if not line:
        return [""]

    pieces = []
    remaining = printable(line.replace("\t", "    "))
    while remaining:
        if stringWidth(remaining, FONT, FONT_SIZE) <= CODE_WIDTH:
            pieces.append(remaining)
            break

        cut = len(remaining)
        while cut > 1 and stringWidth(remaining[:cut] + " \\", FONT, FONT_SIZE) > CODE_WIDTH:
            cut -= 1

        preferred = remaining.rfind(" ", 0, cut)
        if preferred > max(20, cut // 2):
            cut = preferred + 1
        pieces.append(remaining[:cut] + " \\")
        remaining = "    " + remaining[cut:]
    return pieces


def draw_header(pdf, file_name, page_number):
    pdf.setFillColor(HexColor("#24133F"))
    pdf.rect(0, PAGE_HEIGHT - 30, PAGE_WIDTH, 30, fill=1, stroke=0)
    pdf.setFillColor(white)
    pdf.setFont("Helvetica-Bold", 9)
    pdf.drawString(LEFT, PAGE_HEIGHT - 19, "ProyFinal_LPI_Eq01_NomiCore - Codigo fuente")
    pdf.setFont("Helvetica", 8)
    pdf.drawRightString(PAGE_WIDTH - RIGHT, PAGE_HEIGHT - 19, "Pagina {0}".format(page_number))
    pdf.setFillColor(HexColor("#374151"))
    pdf.setFont("Helvetica-Oblique", 8)
    pdf.drawString(LEFT, PAGE_HEIGHT - 41, file_name)
    pdf.setStrokeColor(HexColor("#D1D5DB"))
    pdf.line(LEFT, PAGE_HEIGHT - 46, PAGE_WIDTH - RIGHT, PAGE_HEIGHT - 46)


def draw_cover(pdf, files, total_lines):
    pdf.setFillColor(HexColor("#24133F"))
    pdf.rect(0, 0, PAGE_WIDTH, PAGE_HEIGHT, fill=1, stroke=0)
    pdf.setFillColor(HexColor("#8B5CF6"))
    pdf.rect(0, PAGE_HEIGHT - 16, PAGE_WIDTH, 16, fill=1, stroke=0)
    pdf.setFillColor(white)
    pdf.setFont("Helvetica-Bold", 28)
    pdf.drawString(70, PAGE_HEIGHT - 120, "Codigo Fuente")
    pdf.setFont("Helvetica-Bold", 20)
    pdf.drawString(70, PAGE_HEIGHT - 152, "NomiCore - Sistema de Gestion de Nomina")
    pdf.setFont("Helvetica", 13)
    pdf.drawString(70, PAGE_HEIGHT - 188, "Proyecto: ProyFinal_LPI_Eq01_NomiCore")
    pdf.drawString(70, PAGE_HEIGHT - 211, "Tecnologia: C# 7.3, Windows Forms .NET Framework 4.8 y SQLite")
    pdf.drawString(70, PAGE_HEIGHT - 234, "Tema: Manejo de campos en las clases y PrintDocument")

    pdf.setFillColor(HexColor("#E9D5FF"))
    pdf.setFont("Helvetica-Bold", 14)
    pdf.drawString(70, PAGE_HEIGHT - 295, "Contenido del documento")
    pdf.setFillColor(white)
    pdf.setFont("Helvetica", 12)
    pdf.drawString(70, PAGE_HEIGHT - 322, "{0} archivos C# incluidos".format(len(files)))
    pdf.drawString(70, PAGE_HEIGHT - 345, "{0:,} lineas de codigo y herramientas".format(total_lines))
    pdf.drawString(70, PAGE_HEIGHT - 368, "Se excluyen: bin, obj, .git, .vs, packages y salidas de exportacion.")
    pdf.setFont("Helvetica-Oblique", 9)
    pdf.drawString(70, 56, "Generado el {0}.".format(datetime.now().strftime("%d/%m/%Y %H:%M")))
    pdf.showPage()


def build_pdf():
    files = source_files()
    sources = []
    total_lines = 0
    for file_path in files:
        text = file_path.read_text(encoding="utf-8-sig", errors="replace")
        lines = text.splitlines()
        sources.append((file_path, lines))
        total_lines += len(lines)

    OUTPUT_PATH.parent.mkdir(parents=True, exist_ok=True)
    pdf = canvas.Canvas(str(OUTPUT_PATH), pagesize=PAGE_SIZE, pageCompression=1)
    pdf.setTitle("Codigo Fuente - ProyFinal_LPI_Eq01_NomiCore")
    pdf.setAuthor("Equipo 01")
    draw_cover(pdf, files, total_lines)

    page_number = 1
    for file_path, lines in sources:
        relative = str(file_path.relative_to(PROJECT_ROOT)).replace("\\", "/")
        y = PAGE_HEIGHT - TOP - 16
        draw_header(pdf, relative, page_number)
        pdf.setFont(FONT, FONT_SIZE)
        pdf.setFillColor(black)

        for line_number, source_line in enumerate(lines, start=1):
            visual_lines = wrap_code_line(source_line)
            for index, visual_line in enumerate(visual_lines):
                if y < BOTTOM:
                    pdf.showPage()
                    page_number += 1
                    draw_header(pdf, relative + " (continuacion)", page_number)
                    pdf.setFont(FONT, FONT_SIZE)
                    pdf.setFillColor(black)
                    y = PAGE_HEIGHT - TOP - 16

                if index == 0:
                    pdf.setFillColor(HexColor("#6B7280"))
                    pdf.drawRightString(LEFT + NUMBER_WIDTH - 6, y, str(line_number))
                    pdf.setFillColor(black)
                pdf.drawString(LEFT + NUMBER_WIDTH, y, visual_line)
                y -= LINE_HEIGHT

        pdf.showPage()
        page_number += 1

    pdf.save()
    return len(files), total_lines


if __name__ == "__main__":
    count, lines = build_pdf()
    print("PDF generado: {0}".format(OUTPUT_PATH))
    print("Archivos incluidos: {0}; lineas: {1}".format(count, lines))
