using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Services
{
    public class PersonalService : IPersonalService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public PersonalService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Obtener Cuenta
        public List<PersonalDto> GetAllPersonal()
        {
            return _context.Personal.Select(personal => new PersonalDto
            {
                ID_Personal = personal.ID_Personal,
                Nombre = personal.Nombre,  
                Apellidos = personal.Apellidos,
                ID_Sede = personal.ID_Sede
            }).ToList();
        }

        public PersonalDto GetPersonalById(int id)
        {
            var personal = _context.Personal.FirstOrDefault(x => x.ID_Personal == id);
            if (personal == null)
            {
                return null;
            }
            else
            {
                return new PersonalDto
                {
                    ID_Personal = personal.ID_Personal,
                    Nombre = personal.Nombre,
                    Apellidos = personal.Apellidos,
                    ID_Sede = personal.ID_Sede
                };
            }
        }
        #endregion

        #region Agregar Personal
        public bool CreatePersonal(PersonalDatos personal)
        {
            try
            {
                var sedePersonal = _context.Sedes.FirstOrDefault(p => p.ID_Sede == personal.ID_Sede);
                if (sedePersonal == null)
                {
                    throw new ArgumentException(nameof(personal), "La sede Seleccionada no Existe");
                }
                var nuevoPersonal = new Personal
                {
                    Nombre = personal.Nombre,
                    Apellidos = personal.Apellidos,
                    ID_Sede = personal.ID_Sede
                };
                _context.Personal.Add(nuevoPersonal);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al Intetar Agregar la Persona" + ex.Message);
            }
        }

        #endregion

        #region Editar Personal
        public bool UpdatePersonal(int id, PersonalDatos personalDatos)
        {
            try
            {
                var personalExistente = _context.Personal.FirstOrDefault(x => x.ID_Personal == id);
                if (personalExistente == null)
                {
                    return false;
                }

                if(!string.IsNullOrEmpty(personalDatos.Nombre))
                    personalExistente.Nombre = personalDatos.Nombre;

                if (!string.IsNullOrEmpty(personalDatos.Apellidos))
                    personalExistente.Apellidos = personalDatos.Apellidos;

                if (personalDatos.ID_Sede.HasValue)
                {
                    var sedeExistente = _context.Sedes.FirstOrDefault(s => s.ID_Sede == personalDatos.ID_Sede.Value);
                    if (sedeExistente == null)
                    {
                        throw new Exception("La sede Ingresada no es Existente.");
                    }
                    personalExistente.ID_Sede = personalDatos.ID_Sede.Value;
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Actualizar lpos Datos del Personal" + ex.Message);
            }
        }
        #endregion

        #region Eliminar Personal
        public bool DeletePersonal(int id)
        {
            try
            {
                var personal = _context.Personal.FirstOrDefault(x => x.ID_Personal == id);
                if (personal == null)
                {
                    return false;
                }
                _context.Personal.Remove(personal);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la cuenta "+ex.Message);
            }
        }
        #endregion
    }
}