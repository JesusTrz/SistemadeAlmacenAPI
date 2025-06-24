using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SistemadeAlmacenAPI.Controllers
{
    public class RolesController : ApiController
    {
        private readonly IRolesService _rolesService;

        public RolesController()
        {
            _rolesService = new RolesService();
        }

        #region Obtener Datos
        [HttpGet]
        [Route("GetAllRoles")]

        public IHttpActionResult GetAllRoles()
        {
            var result = _rolesService.GetAllRoles();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetRolesById/{Id}")]

        public IHttpActionResult GetRolesById(int id)
        {
            var result = _rolesService.GetRolesById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Rol no Existente");
            }
        }
        #endregion
    }
}
