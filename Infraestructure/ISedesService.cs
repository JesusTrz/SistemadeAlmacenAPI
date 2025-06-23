using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    internal interface ISedesService
    {
        bool CreateSede(NombreSedeDto nombreSede);
        List<SedesDto> GetAllSedes();
        SedesDto GetByIdSedes(int id);
        bool UpdateNombre(int id, NombreSedeDto updatesedesDto);
    }
}
