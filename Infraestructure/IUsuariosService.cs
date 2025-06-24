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
        bool CreateUsuario(UsuariosDatos usuarioDatos);
        bool DeleteUsuario(int id);
        List<UsuariosDto> GetAllUsuarios();
        UsuariosDto GetUsuariosById(int id);
        bool UpdatePassword(CambioContrasenia camcontra);
        bool ValidarLogin(string nombreUsuario, string contrasenia, int idSede);
    }
}
