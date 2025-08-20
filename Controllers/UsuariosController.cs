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

        [HttpGet]
        [Route("GetUsuariosByIdSede/{IdSede}")]

        public IHttpActionResult GetUsuarioByIdSede(int idSede)
        {
            var result = _usuariosService.GetUsuarioByIdSede(idSede);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Usuarios no Encontrados");
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

        #region Editar Contraseña y Nombre
        [HttpPut]
        [Route("UpdatePassword/{id}")]

        public IHttpActionResult UpdatePassword(CambioContrasenia camcontra)
        {
            if (camcontra == null) throw new Exception("Los datos de la solicitud son invalidos");

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

        [HttpPut]
        [Route("UpdateName")]

        public IHttpActionResult CambiarNombre(int idUsuario, UsuariosDto usuariosDto)
        {
            if(usuariosDto == null || string.IsNullOrWhiteSpace(usuariosDto.Nombre_Usuario))
            {
                return BadRequest("Nombre Invalido");
            }

            var result = _usuariosService.CambiarNombre(idUsuario, usuariosDto);
            if (result)
            {
                return Ok("Nombre Actualizado con Exito!");
            }
            else
            {
                return BadRequest("Ocurrio un error al cambiar el nombre.");
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

        public IHttpActionResult ValidarLogin(string nombreUsuario, string contrasenia)
        {
            if (nombreUsuario == null)
                return BadRequest("Debes Ingresar un Nombre de Usuario");

            if (nombreUsuario == null)
                return BadRequest("Debes Ingresar la Contraseña!");

            var result = _usuariosService.ValidarLogin(nombreUsuario, contrasenia);
            if (result)
            {
                return Ok($"Bienvenido {nombreUsuario}");
            }
            else
            {
                return BadRequest("Ocurrio un error al iniciar sesion");
            }
        }
        #endregion
    }
}
