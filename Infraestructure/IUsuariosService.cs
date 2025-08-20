using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    public interface IUsuariosService
    {
        bool CambiarNombre(int idUsuario, UsuariosDto usuariosDto);
        bool CreateUsuario(UsuariosDatos usuarioDatos);
        bool DeleteUsuario(int id);
        List<UsuariosDto> GetAllUsuarios();
        List<UsuariosDto> GetUsuarioByIdSede(int idSede);
        UsuariosDto GetUsuariosById(int id);
        bool UpdatePassword(CambioContrasenia camcontra);
        bool ValidarLogin(string nombreUsuario, string contrasenia);
    }
}
