using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    public interface IRolesService
    {
        List<RolesDto> GetAllRoles();
        RolesDto GetRolesById(int id);
    }
}
