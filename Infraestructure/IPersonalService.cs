using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    public interface IPersonalService
    {
        bool CreatePersonal(PersonalDatos personal);
        bool DeletePersonal(int id);
        List<PersonalDto> GetAllPersonal();
        PersonalDto GetPersonalById(int id);
        bool UpdatePersonal(int id, PersonalDatos personalDatos);
    }
}
