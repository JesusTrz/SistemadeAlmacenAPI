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
    public class SalidaController : ApiController
    {
        private readonly ISalidaService _salidaService;

        public SalidaController()
        {
            _salidaService = new SalidaService();
        }

        #region Registrar Entradas y Detalles
        [HttpPost]
        [Route("RegistrarSalidasyDetalles")]

        public IHttpActionResult RegistrarSalidasyDetalles(SalidaDto salidaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = _salidaService.RegistrarSalidasyDetalles(salidaDto);
                if (result) return Ok("La Salida fue Registrada Exitosamente!");
                else return BadRequest("Ocurrio un Error al Registrar la Salida");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Ocurrió un error al registrar la salida: " + ex.Message));
            }
        }
        // JSON PARA AGREGAR SALIDA Y DETALLE
        //{
        //  "ID_Movimiento": 1,
        //  "ID_CenCost": 1,
        //  "ID_Unidad": 1,
        //  "ID_Personal": 1,
        //  "Comentarios": "Prueba de Comentario 2",
        //  "ID_Sede": 1,
        //  "Detalles": [
        //    {
        //      "ID_Articulo": 8,
        //      "Cantidad": 7,
        //      "Precio_Unitario": 15
        //    }
        //  ]
        //}
        #endregion

        #region Obtener Salidas Y Detalles
        [HttpGet]
        [Route(" ObtenerSalidasporSede")]

        public IHttpActionResult ObtenerSalidasporSede(int idSede)
        {
            try
            {
                var salidas = _salidaService.ObtenerSalidasporSede(idSede);
                if (salidas == null || salidas.Count == 0)
                    return NotFound();
                return Ok(salidas);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener salidas: " + ex.Message));
            }
        }
        #endregion

        #region Modificar Salida y Detalles
        [HttpPut]
        [Route("ActualizarSalidasyDetalles")]

        public IHttpActionResult ActualizarSalidasyDetalles(int idSalida, GetSalidasDto dto)
        {
            if (dto == null) return BadRequest("Datos Invalidos");
            try
            {
                var actualizado = _salidaService.ActualizarSalidasyDetalles(idSalida, dto);
                if (!actualizado)
                    return NotFound();
                return Ok("Salida Actualizada Correctamente");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al actualizar la entrada: " + ex.Message));
            }
        }
        #endregion
    }
}
