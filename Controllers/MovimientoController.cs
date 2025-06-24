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
    public class MovimientoController : ApiController
    {
        private readonly IMovimientosService _movimientosService;

        public MovimientoController()
        {
            _movimientosService = new MovimientosService();
        }

        #region Obtener Datos
        [HttpGet]
        [Route("GetAllMovimientos")]

        public IHttpActionResult GetAllMovimientos()
        {
            var result = _movimientosService.GetAllMovimientos();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetMovimientosById/{Id}")]

        public IHttpActionResult GetMovimientosById(int id)
        {
            var result = _movimientosService.GetMovimientosById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Movimiento no Encontrado");
            }
        }
        #endregion

        #region Agregar nuevo Movimiento
        [HttpPost]
        [Route("CreateMovimiento")]
        public IHttpActionResult CreateMovimiento(MovimientosDatos movimientos)
        {
            if (movimientos == null)
            {
                return BadRequest("Los Movimientos no debe contener Datos Vacios");
            }
            else
            {
                var result = _movimientosService.CreateMovimiento(movimientos);
                if (result)
                {
                    return Ok($"El Movimiento '{movimientos.Nombre_Movimiento}' fue agregado con exito!");
                }
                else
                {
                    return BadRequest($"Ocurrio un error al querer agregar el movimiento '{movimientos.Nombre_Movimiento}'");
                }
            }
        }
        #endregion

        #region Editar Movimiento
        [HttpPut]
        [Route("UpdateMovimientoBy/{id}")]
        public IHttpActionResult UpdateMovimiento(int id, MovimientosDatos movimiento)
        {
            if (movimiento == null)
            {
                return BadRequest("El Movimiento no pudo ser Modificado");
            }
            else
            {
                var result = _movimientosService.UpdateMovimiento(id, movimiento);
                if (result)
                {
                    return Ok("El Movimiento fue Actualizado con Exito!");
                }
                else
                {
                    return BadRequest("Ocurrio un Error al querer modificar el Movimiento.");
                }
            }
        }
        #endregion

        #region Eliminar Movimietno
        [HttpDelete]
        [Route("DeleteMovimiento")]

        public IHttpActionResult DeleteMovimiento(int id)
        {
            var result = _movimientosService.DeleteMovimiento(id);
            if (result)
            {
                return Ok("Movimiento Eliminado con Exito!");
            }
            else
            {
                return BadRequest("Error al Eliminar el Movimiento");
            }
        }
        #endregion
    }
}
