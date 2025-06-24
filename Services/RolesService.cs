using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Services
{
    public class RolesService : IRolesService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public RolesService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Obtener Roles
        public List<RolesDto> GetAllRoles()
        {
            return _context.Roles.Select(roles => new RolesDto
            {
                ID_Roles = roles.ID_Roles,
                Nombre_Rol = roles.Nombre_Rol
            }).ToList();
        }

        public RolesDto GetRolesById(int id)
        {
            var roles = _context.Roles.FirstOrDefault(x => x.ID_Roles == id);
            if (roles == null)
            {
                return null;
            }
            else
            {
                return new RolesDto
                {
                    ID_Roles = roles.ID_Roles,
                    Nombre_Rol = roles.Nombre_Rol
                };
            }
        } 
        #endregion

    }
}