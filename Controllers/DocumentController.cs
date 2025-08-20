using MigraDoc.DocumentObjectModel;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace SistemadeAlmacenAPI.Controllers
{
    public class DocumentController : ApiController
    {
        private readonly IDocumentService _documentService;

        public DocumentController()
        {
            _documentService = new DocumentService();
        }

        [HttpGet]
        [Route("api/reporte-entradas")]
        public HttpResponseMessage GenerarReporteEntradasPorFecha(DateTime fechaInicio, DateTime fechaFinal, int idSede)
        {
            var pdfBytes = _documentService.GenerarReporteEntradasPorFecha(fechaInicio, fechaFinal, idSede);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(pdfBytes)
            };

            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "ReporteEntradas.pdf"
            };

            return result;
        }
    }
}
