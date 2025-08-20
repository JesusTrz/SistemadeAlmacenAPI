using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SistemadeAlmacenAPI.Services
{
    public class UsuariosService : IUsuariosService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public UsuariosService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Obtener Usuarios
        public List<UsuariosDto> GetAllUsuarios()
        {
            return _context.Usuarios.Select(usuario => new UsuariosDto
            {
                ID_Usuario = usuario.ID_Usuario,
                Nombre_Usuario = usuario.Nombre_Usuario,
                ID_Roles = usuario.ID_Roles,
                ID_Sede = usuario.ID_Sede
            }).ToList();
        }

        public UsuariosDto GetUsuariosById(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(x => x.ID_Usuario == id);
            if (usuario == null)
            {
                return null;
            }
            else
            {
                return new UsuariosDto
                {
                    ID_Usuario = usuario.ID_Usuario,
                    Nombre_Usuario = usuario.Nombre_Usuario,
                    ID_Roles = usuario.ID_Roles,
                    ID_Sede = usuario.ID_Sede
                };
            }
        }

        public List<UsuariosDto> GetUsuarioByIdSede(int idSede)
        {
            var result = _context.Usuarios
                .Where(u => u.ID_Sede == idSede)
                .Select(u => new UsuariosDto
                {
                    ID_Usuario = u.ID_Usuario,
                    Nombre_Usuario = u.Nombre_Usuario,
                    ID_Roles = u.ID_Roles,
                    ID_Sede = u.ID_Sede
                }).ToList();
            return result;
        }
        #endregion

        #region Crear Nuevos Usuarios
        public bool CreateUsuario(UsuariosDatos usuarioDatos)
        {
            if (usuarioDatos == null)
            {
                throw new ArgumentNullException(nameof(usuarioDatos), "Los datos no pueden ser nulos.");
            }
            try
            {
                var usuarioExistente = _context.Usuarios.Any(u => u.Nombre_Usuario == usuarioDatos.Nombre_Usuario);
                if (usuarioExistente)
                {
                    throw new Exception("El nombre de usuario ya existe!");
                }

                var sedeExistente = _context.Sedes.Any(s => s.ID_Sede == usuarioDatos.ID_Sede);
                if (!sedeExistente)
                {
                    throw new Exception("La sede proporcionada no existe.");
                }

                var rolExistente = _context.Roles.Any(r => r.ID_Roles == usuarioDatos.ID_Roles);
                if (!rolExistente)
                {
                    throw new Exception("El rol proporcionado no existe.");

                }
                if (!ContraseniaValida(usuarioDatos.Contrasenia))
                {
                    throw new Exception("La contraseña debe tener al menos 6 caracteres, una mayúscula, una minúscula y un número.");
                }

                var result = new Usuarios
                
                {
                    Nombre_Usuario = usuarioDatos.Nombre_Usuario,
                    Contrasenia = HashPassword(usuarioDatos.Contrasenia),
                    ID_Roles = usuarioDatos.ID_Roles,
                    ID_Sede = usuarioDatos.ID_Sede
                };
                _context.Usuarios.Add(result);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear usuario: " + ex.Message);
            }
        }
        #endregion

        #region Editar Contraseña de Usuario y Nombre
        public bool UpdatePassword(CambioContrasenia camcontra)
        {
            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(x=>x.ID_Usuario == camcontra.ID_Usuario);
                if (usuario == null)
                {
                    throw new Exception("Usuario no encontrado");
                }

                string contraVieja = HashPassword(camcontra.ViejaContrasenia);
                if(usuario.Contrasenia != contraVieja)
                {
                    throw new Exception("La contraseña actual es incorrecta");
                }

                usuario.Contrasenia = HashPassword(camcontra.NuevaContrasenia);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la contraseña: " + ex.Message);
            }
        }

        public bool CambiarNombre(int idUsuario, UsuariosDto usuariosDto)
        {
            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.ID_Usuario == idUsuario);
                if(usuario == null)
                {
                    throw new Exception("Usuario no encontrado");
                }

                usuario.Nombre_Usuario = usuariosDto.Nombre_Usuario;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Actualizar el nombre del Usuario: " + ex.Message);
            }
        }
        #endregion

        #region Eliminar Usuarios
        public bool DeleteUsuario(int id)
        {
            try
            {
                var usuarios = _context.Usuarios.FirstOrDefault(x => x.ID_Usuario == id);
                if (usuarios == null) return false;

                var usuarioSede = _context.Sedes.Where(us => us.ID_Sede == id).ToList();
                _context.Sedes.RemoveRange(usuarioSede);
                _context.Usuarios.Remove(usuarios);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar usuario: " + ex.Message);
            }
        }
        #endregion

        #region Login
        public bool ValidarLogin(string nombreUsuario, string contrasenia)
        {

            string hashedpassword = HashPassword(contrasenia);

            var usuario = _context.Usuarios.FirstOrDefault(u =>
            u.Nombre_Usuario == nombreUsuario &&
            u.Contrasenia == hashedpassword);

            return usuario != null;
        }
        #endregion

        #region Metodo para Encriptar Contraseña y Validar Contraseña
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private bool ContraseniaValida(string password)
        {
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$");
            return regex.IsMatch(password);
        }
        #endregion
    }
}