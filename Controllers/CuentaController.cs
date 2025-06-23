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
    public class CuentaController : ApiController
    {
        private readonly ICuentaService _cuentaService;
        
        public CuentaController()
        {
            _cuentaService = new CuentaService();
        }

        #region Obtener Datos
        [HttpGet]
        [Route("GetAllCuenta")]

        public IHttpActionResult GetAllCuenta()
        {
            var result = _cuentaService.GetAllCuenta();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetCuentasBy/{Id}")]

        public IHttpActionResult GetCuentasById(int id)
        {
            var result = _cuentaService.GetCuentasById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Cuenta no Encontrada");
            }
        }
        #endregion

        #region Insertar Nueva Cuenta

        [HttpPost]
        [Route("CreateCuenta")]

        public IHttpActionResult CreateCuenta(DatosCuenta cuenta)
        {
            if (cuenta == null)
            {
                return BadRequest("La Cuenta no debe contener Datos Vacios");
            }
            var result = _cuentaService.CreateCuenta(cuenta);
            if (result)
            {
                return Ok($"La Cuenta '{cuenta.Nombre_Cuenta}' Fue Agregada con Exito!");
            }
            else
            {
                return BadRequest($"La cuenta '{cuenta.Nombre_Cuenta}' No pudo ser agreagada.");
            }


        }

        #endregion

        #region Editar Cuenta
        [HttpPut]
        [Route("UpdateCuentaBy/{id}")]
        public IHttpActionResult UpdateCuenta(int id, DatosCuenta datosCuenta)
        {
            if (datosCuenta == null)
            {
                return BadRequest("Ocurrio un error al editar la Cuenta");
            }

            var result = _cuentaService.UpdateCuenta(id, datosCuenta);
            if (result)
            {
                return Ok("Cuenta Actualizada con Exito!");
            }
            else
            {
                return BadRequest("Error al Actualizar la Cuenta");
            }
        }
        #endregion

        #region Eliminar Cuenta

        [HttpDelete]
        [Route("DeleteCuenta")]
        public IHttpActionResult DeleteCuenta(int id)
        {
            var result = _cuentaService.DeleteCuenta(id);
            if (result)
            {
                return Ok("Cuenta Eliminada con exito");
            }
            else
            {
                return BadRequest("Error al eliminar la Cuenta");
            }
        }

        #endregion
    }
}
