using SistemadeAlmacenAPI.Database;
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
    public class PersonalController : ApiController
    {
        private readonly IPersonalService _personalService;

        public PersonalController()
        {
            _personalService = new PersonalService();
        }

        #region Obtener Datos
        [HttpGet]
        [Route("GetAllPersonal")]

        public IHttpActionResult GetAllPersonal()
        {
            var result = _personalService.GetAllPersonal();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetPersonalBy/{Id}")]

        public IHttpActionResult GetPersonalById(int id)
        {
            var result = _personalService.GetPersonalById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("La Persona no fue Encontrada");
            }
        }
        #endregion

        #region Agregar nuevo Personal
        [HttpPost]
        [Route("AgregarPersona")]

        public IHttpActionResult CreatePersonal(PersonalDatos personal)
        {
            if (personal == null || personal.ID_Sede == null)
            {
                return BadRequest("Datos Incompletos");
            }

            try
            {
                var result = _personalService.CreatePersonal(personal);
                if (result)
                {
                    return Ok($"La Persona '{personal.Nombre + personal.Apellidos}' Fue Agregada con Exito!");
                }
                else
                {
                    return BadRequest("No fue posible agregar el nuevo personal.");
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
        [Route("UpdatePersonalBy/{id}")]

        public IHttpActionResult UpdatePersonal(int id, PersonalDatos personalDatos)
        {
            if(personalDatos == null)
            {
                return BadRequest("Ocurrio un error al editar el personal");
            }

            var result = _personalService.UpdatePersonal(id, personalDatos);
            if (result)
            {
                return Ok("Personal Editado con Exito");
            }
            else
            {
                return BadRequest("Error al Actualizar al personal");
            }
        }
        #endregion

        #region Eliminar Personal
        [HttpDelete]
        [Route("DeletePersonalBy/{id}")]

        public IHttpActionResult DeletePersonal(int id)
        {
            var result = _personalService.DeletePersonal(id);
            if (result)
            {
                return Ok("Personal Eliminado con Exito");
            }
            else
            {
                return BadRequest("Error al Eliminar Personal");
            }
        }
        #endregion
    }
}
