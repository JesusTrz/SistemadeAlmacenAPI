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
    public class SedesController : ApiController
    {
        private readonly ISedesService _sedesService;

        public SedesController()
        {
            _sedesService = new SedesService();
        }

        #region Obtener Sedes
        [HttpGet]
        [Route("GetAllSedes")]

        public IHttpActionResult GetAllSedes()
        {
            var result = _sedesService.GetAllSedes();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetByIdSedes/{id}")]

        public IHttpActionResult GetByIdSedes(int id)
        {
            var result = _sedesService.GetByIdSedes(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Sede no Encontrada");
            }
        }
        #endregion

        #region Crear y Editar
        [HttpPost]
        [Route("CreateSede")]
        public IHttpActionResult CreateSede(NombreSedeDto nombreSede)
        {
            if (nombreSede == null)
            {
                return BadRequest("La Nueva SEDE debe tener un Nombre.");
            }

            var result = _sedesService.CreateSede(nombreSede);
            if (result)
            {
                return Ok($"La sede '{nombreSede.Nombre_Sede}' Fue Creada con Exito!");
            }
            else
            {
                return BadRequest($"Error al Crear la sede '{nombreSede.Nombre_Sede}'.");
            }
        }

        [HttpPut]
        [Route("UpdateNombre/{id}")]

        public IHttpActionResult UpdateNombre(int id, NombreSedeDto updateDto)
        {
            if (updateDto == null || string.IsNullOrWhiteSpace(updateDto.Nombre_Sede))
            {
                return BadRequest("Nombre Invalido");
            }

            var result = _sedesService.UpdateNombre(id, updateDto);
            if (result)
            {
                return Ok("Nombre Actualizado con Exito");
            }
            else
            {
                return BadRequest("Error al Actualizar Nombre");
            }
        } 
        #endregion
    }
}
