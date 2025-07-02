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
    public class EntradaController : ApiController
    {
        private readonly IEntradaService _entradaService;

        public EntradaController()
        {
            _entradaService = new EntradaService();
        }

        #region Registrar Entradas y Detalles
        [HttpPost]
        [Route("RegistrarEntradayDetalles")]

        public IHttpActionResult RegistrarEntradayDetalles(EntradasDto entradasdto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = _entradaService.RegistrarEntradayDetalles(entradasdto);
                if (result) return Ok("La entrada fue registrada exitosamente!");
                else return BadRequest("Ocurrio un error al registrar la Entrada");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Ocurrió un error al registrar la entrada: " + ex.Message));
            }
        }
        #endregion

        #region Obtener Entradas por sede
        [HttpGet]
        [Route("ObtenerEntradasporSede")]
        public IHttpActionResult ObtenerEntradasporSede(int idSede)
        {
            try
            {
                var entradas = _entradaService.ObtenerEntradasporSede(idSede);
                if (entradas == null || entradas.Count == 0) 
                    return NotFound();
                return Ok(entradas);

            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener entradas: " + ex.Message)); 
            }
        }
        #endregion

        #region Modificar Entrada y Detalles
        [HttpPut]
        [Route("ActualizarEntradayDetalles")]

        public IHttpActionResult ActualizarEntradayDetalles(int idEntrada, GetEntradasDto dto)
        {
            if (dto == null) return BadRequest("Datos Invalidos");

            try
            {
                var actualizado = _entradaService.ActualizarEntradayDetalles(idEntrada, dto);
                if(!actualizado)
                    return NotFound();
                return Ok("Entrada y Detalles Actualizados Correctamente!");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al actualizar la entrada: " + ex.Message));
            }
        }
        // Json que se debe de enviar
//        {
//          "ID_Entradas": 7,
//          "Comentarios": "prueba de Cambio",
//          "ID_Proveedores": 1,
//          "ID_Movimiento": 2,
//          "ID_Sede": 1,
//          "Detalles": [
//        {
//          "ID_Articulo": 8,
//          "Cantidad": 10,
//          "Precio_Unitario": 10
//        }
//           ]
//        }
        #endregion
    }
}
