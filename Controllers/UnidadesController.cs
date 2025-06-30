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
    public class UnidadesController : ApiController
    {
        private readonly IUnidadesService _unidadesService;

        public UnidadesController()
        {
            _unidadesService = new UnidadesService();
        }

        #region Obtener Datos
        [HttpGet]
        [Route("GetAllUnidades")]

        public IHttpActionResult GetAllUnidades()
        {
            var result = _unidadesService.GetAllUnidades();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetUnidadlBy/{Id}")]

        public IHttpActionResult GetUnidadById(int id)
        {
            var result = _unidadesService.GetUnidadById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("La Unidad no fue Encontrada");
            }
        }

        [HttpGet]
        [Route("GetUnidadesByIdSede")]

        public IHttpActionResult GetUnidadesByIdSede(int idSede)
        {
            var result = _unidadesService.GetUnidadesByIdSede(idSede);
            if (result != null) return Ok(result);
            else return BadRequest("Unidad no Encontrada");
        }
        #endregion

        #region Agregar nuevo Personal
        [HttpPost]
        [Route("AgregarUnidad")]

        public IHttpActionResult CreateUnidad(UnidadesDatos unidad)
        {
            if (unidad == null || unidad.ID_Sede == null)
            {
                return BadRequest("Datos Incompletos");
            }

            try
            {
                var result = _unidadesService.CreateUnidad(unidad);
                if (result)
                {
                    return Ok($"La Unidad con la Placa: '{unidad.Numero_Placa}' Fue Agregada con Exito!");
                }
                else
                {
                    return BadRequest($"No fue posible agregar la Unidad con la Placa: '{unidad.Numero_Placa}'.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Editar Personal
        [HttpPut]
        [Route("UpdateUnidadBy/{id}")]

        public IHttpActionResult UpdateUnidad(int id, UnidadesDatos unidadDatos)
        {
            if (unidadDatos == null)
            {
                return BadRequest("Ocurrio un error al editar la Unidad.");
            }

            var result = _unidadesService.UpdateUnidad(id, unidadDatos);
            if (result)
            {
                return Ok("Unidad editada con Exito.");
            }
            else
            {
                return BadRequest("Error al Actualizar la Unidad.");
            }
        }
        #endregion

        #region Eliminar Personal
        [HttpDelete]
        [Route("DeleteUnidadBy/{id}")]

        public IHttpActionResult DeletePersonal(int id)
        {
            var result = _unidadesService.DeletePersonal(id);
            if (result)
            {
                return Ok("Unidad Eliminada con Exito.");
            }
            else
            {
                return BadRequest("Error al Eliminar la Unidad.");
            }
        }
        #endregion
    }
}
