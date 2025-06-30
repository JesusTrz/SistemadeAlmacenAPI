using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    public interface IUnidadesService
    {
        bool CreateUnidad(UnidadesDatos unidad);
        bool DeletePersonal(int id);
        List<UnidadesDto> GetAllUnidades();
        UnidadesDto GetUnidadById(int id);
        List<UnidadesDto> GetUnidadesByIdSede(int idSede);
        bool UpdateUnidad(int id, UnidadesDatos unidadDatos);
    }
}
