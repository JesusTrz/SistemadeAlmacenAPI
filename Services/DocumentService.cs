using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public DocumentService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        public byte[] GenerarReporteEntradasPorFecha(DateTime fechaInicio, DateTime fechaFinal, int idSede)
        {
            var doc = new Document();
            var section = doc.AddSection();

            doc.Styles["Normal"].Font.Name = "Verdana";
            doc.Styles["Heading1"].Font.Size = 14;
            doc.Styles["Heading1"].Font.Bold = true;
            doc.Styles["Heading1"].ParagraphFormat.SpaceAfter = "1cm";
            doc.Styles["Heading2"].Font.Size = 12;
            doc.Styles["Heading2"].Font.Bold = true;


            section.AddParagraph("Reporte de Entradas", "Heading1");

            var entradas = _context.Entradas
                .Where(e => e.Fecha >= fechaInicio && e.Fecha <= fechaFinal && e.ID_Sede == idSede).ToList();

            foreach(var entrada in entradas)
            {
                section.AddParagraph($"Fecha: {entrada.Fecha:dd/MM/yyy} - Hora: {entrada.Hora}");
                section.AddParagraph($"Movimiento: {entrada.Movimientos.Nombre_Movimiento} - {entrada.Movimientos.Descripcion_Movimiento}", "Heading2");
                section.AddParagraph($"Proveedor: {entrada.Proveedores.Razon_Social}");
                section.AddParagraph($"Referencia: {entrada.Referencias}");
                

                var table = section.AddTable();
                table.Borders.Width = 0.75;
                table.AddColumn("2cm"); // ID
                table.AddColumn("4cm"); // Descripción
                table.AddColumn("2.5cm"); // Unidad
                table.AddColumn("2.5cm"); // Cantidad
                table.AddColumn("3cm"); // Costo Unitario
                table.AddColumn("3cm"); // Total

                var header = table.AddRow();
                header.Shading.Color = Colors.LightGray;
                header.Cells[0].AddParagraph("ID Artículo");
                header.Cells[1].AddParagraph("Descripción");
                header.Cells[2].AddParagraph("Unidad");
                header.Cells[3].AddParagraph("Cantidad");
                header.Cells[4].AddParagraph("Costo Unitario");
                header.Cells[5].AddParagraph("Total");

                decimal totalGeneral = 0;
                decimal totalCantidad = 0;

                foreach (var detalle in entrada.Detalle_Entrada)
                {
                    var articulo = _context.Articulo.FirstOrDefault(a => a.ID_Articulo == detalle.ID_Articulo);
                    if (articulo == null) continue;

                    var unidad = articulo.Unidades_Medida?.Nombre_Unidad ?? "N/A";
                    decimal total = (decimal)(detalle.Cantidad * detalle.Precio_Unitario);

                    totalGeneral += total;
                    totalCantidad += (decimal)detalle.Cantidad;

                    var row = table.AddRow();
                    row.Cells[0].AddParagraph(detalle.ID_Articulo.ToString());
                    row.Cells[1].AddParagraph(articulo.Nombre_Articulo);
                    row.Cells[2].AddParagraph(unidad);
                    row.Cells[3].AddParagraph($"{detalle.Cantidad:N2}");
                    row.Cells[4].AddParagraph($"{detalle.Precio_Unitario:C2}");
                    row.Cells[5].AddParagraph($"{total:C2}");
                }

                section.AddParagraph($"Totales: Cantidad = {totalCantidad:N2} - Total = {totalGeneral:C2}");
                section.AddParagraph(" ");
                section.AddParagraph(new string('-', 100));
            }

            var renderer = new PdfDocumentRenderer(true) { Document = doc };
            renderer.RenderDocument();

            using(var ms = new MemoryStream())
            {
                renderer.PdfDocument.Save(ms, false);
                return ms.ToArray();
            }

        }
    }
}