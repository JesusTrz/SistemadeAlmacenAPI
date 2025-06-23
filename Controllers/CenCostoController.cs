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
    public class CenCostoController : ApiController
    {
        private readonly ICenCostoService _cenCostoService;

        public CenCostoController()
        {
            _cenCostoService = new CenCostoService();
        }

        #region Obtener Datos
        [HttpGet]
        [Route("GetAllCentroCosto")]

        public IHttpActionResult GetAllCentroCosto()
        {
            var result = _cenCostoService.GetAllCentroCosto();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetCentroCostoBy/{Id}")]

        public IHttpActionResult GetCentroCostoById(int id)
        {
            var result = _cenCostoService.GetCentroCostoById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Centro de Costo no Encontrado");
            }
        }
        #endregion

        #region Agregar Centro de Costo
        [HttpPost]
        [Route("CreateCentro")]

        public IHttpActionResult CrearCentroCosto(CenCostoDatos costo)
        {
            if(costo == null)
            {
                return BadRequest("El Centro de Costo no puede contener Datos Vacios.");
            }
            var result = _cenCostoService.CrearCentroCosto(costo);
            if (result)
            {
                return Ok($"El Centro de Costo '{costo.Nombre_CenCost}' Fue Agregado con Exito!");
            }
            else
            {
                return BadRequest($"El Centro de Costo '{costo.Nombre_CenCost}' No Pudo ser Agregado.");
            }
        }
        #endregion

        #region Editar Centro de Costo
        [HttpPut]
        [Route("UpdateCentroBy/{id}")]

        public IHttpActionResult UpdateCentroCosto(int id, CenCostoDatos datosCentro)
        {
            if (datosCentro == null)
            {
                return BadRequest($"Ocurrio un error al editar el Centro de Costo: '{datosCentro.Nombre_CenCost}'.");
            }

            var result = _cenCostoService.UpdateCentroCosto(id, datosCentro);
            if (result)
            {
                return Ok("El Centro de Costo fue Actualizado con Exito.");
            }
            else
            {
                return BadRequest("Error al Actualizar el Centro de Costo.");
            }
        }
        #endregion

        #region Eliminar Centro de Costo
        [HttpDelete]
        [Route("DeleteCentroBy/{id}")]

        public IHttpActionResult DeleteCentroCosto(int id)
        {
            var result = _cenCostoService.DeleteCentroCosto(id);
            if (result)
            {
                return Ok("Centro de Costo Eliminado con Exito!");
            }
            else
            {
                return BadRequest("Error al Eliminar el Centro de Costo");
            }
        }
        #endregion
    }
}
