using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using SistemadeAlmacenAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SistemadeAlmacenAPI.Controllers
{
    public class LineaController : ApiController
    {
        private readonly ILineaService _lineaService;
        public LineaController()
        {
            _lineaService = new LineaService();
        }

        #region Obtener Datos
        [HttpGet]
        [Route("GetAllLineas")]

        public IHttpActionResult GetAllLinea()
        {
            var result = _lineaService.GetAllLinea();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetLineasBy/{Id}")]

        public IHttpActionResult GetLineaById(int id)
        {
            var result = _lineaService.GetLineaById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Linea no Encontrada");
            }
        }
        #endregion

        #region Insertar Nueva Linea

        [HttpPost]
        [Route("CreateLinea")]

        public IHttpActionResult CreateLinea(LineaDatos lineaDto)
        {
            if (lineaDto == null || lineaDto.ID_Cuenta == null)
                return BadRequest("Datos incompletos.");

            try
            {
                var resultado = _lineaService.CreateLinea(lineaDto);

                if (resultado)
                    return Ok($"La línea '{lineaDto.Nombre_Linea}' fue registrada con éxito.");
                else
                    return BadRequest("No se pudo registrar la línea.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Editar Cuenta

        [HttpPut]
        [Route("UpdateLineaBy/{id}")]
        public IHttpActionResult UpdateLinea(int id, LineaDatos datosLinea)
        {
            if (datosLinea == null)
            {
                return BadRequest("Ocurrio un error al editar la Linea");
            }

            var result = _lineaService.UpdateLinea(id, datosLinea);
            if (result)
            {
                return Ok("Linea Actualizada con Exito!");
            }
            else
            {
                return BadRequest("Error al Actualizar la Linea");
            }
        }
        #endregion

        #region Eliminar Linea

        [HttpDelete]
        [Route("DeleteLineaBy/{id}")]

        public IHttpActionResult DeleteLinea(int id)
        {
            var result = _lineaService.DeleteLinea(id);
            if (result)
            {
                return Ok("Linea Eliminada con exito");
            }
            else
            {
                return BadRequest("Error al eliminar la Linea");
            }
        }

        #endregion
    }
}
