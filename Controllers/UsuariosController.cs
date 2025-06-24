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
    public class UsuariosController : ApiController
    {
        private readonly IUsuariosService _usuariosService;

        public UsuariosController()
        {
            _usuariosService = new UsuariosService();
        }

        #region Obtener Datos
        [HttpGet]
        [Route("GetAllUsuarios")]

        public IHttpActionResult GetAllUsuarios()
        {
            var result = _usuariosService.GetAllUsuarios();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetUsuariosById/{Id}")]

        public IHttpActionResult GetCuentasByGetUsuariosByIdId(int id)
        {
            var result = _usuariosService.GetUsuariosById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Usuario no Encontrado");
            }
        }
        #endregion

        #region Crear Nuevos Usuarios
        [HttpPost]
        [Route("CreateUsuario")]

        public IHttpActionResult CreateUsuario(UsuariosDatos usuarioDatos)
        {
            if (usuarioDatos == null) return BadRequest("");

            var result = _usuariosService.CreateUsuario(usuarioDatos);
            if (result)
            {
                return Ok($"El usuario {usuarioDatos.Nombre_Usuario} fue Creado con Exito!");
            }
            else
            {
                return BadRequest($"Ocurrio un error al crear el usario {usuarioDatos.Nombre_Usuario}");
            }
        }
        #endregion

        #region Editar Contraseña
        [HttpPut]
        [Route("UpdatePassword/{id}")]

        public IHttpActionResult UpdatePassword(CambioContrasenia camcontra)
        {
            if(camcontra == null) throw new Exception("Los datos de la solicitud son invalidos");

            try
            {
                var result = _usuariosService.UpdatePassword(camcontra);
                if (result)
                {
                    return Ok("Contraseña actualizada con Exito!");
                }
                else
                {
                    return BadRequest("Error al Actualizar contraseña.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Eliminar Usuario
        [HttpDelete]
        [Route("DeleteUsuario")]

        public IHttpActionResult DeleteUsuario(int id)
        {
            var result = _usuariosService.DeleteUsuario(id);
            if (result)
            {
                return Ok("Usuario Eliminado con Exito!");
            }
            else
            {
                return BadRequest("Error al Eliminar el Usuario.");
            }
        }
        #endregion

        #region Login de Usuario
        [HttpPost]
        [Route("api/login")]

        public IHttpActionResult Login(LoginUser login)
        {
            var service = new UsuariosService();

            bool valido = service.ValidarLogin(login.Nombre_Usuario, login.Contrasenia, login.ID_Sede);

            if (valido) return Ok($"El Usuario {login.Nombre_Usuario} fue Autenticado correctamente!");
            return Unauthorized();
        }
        #endregion
    }
}
