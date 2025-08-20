using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;

namespace SistemadeAlmacenAPI.Services
{
    public class SedesService :ISedesService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public SedesService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Obtener Datos
        public List<SedesDto> GetAllSedes()
        {
            return _context.Sedes.Select(sedes => new SedesDto
            {
                ID_Sede = sedes.ID_Sede,
                Nombre_Sede = sedes.Nombre_Sede
            }).ToList();
        }

        public SedesDto GetByIdSedes(int id)
        {
            var sedes = _context.Sedes.FirstOrDefault(x => x.ID_Sede == id);
            if (sedes == null)
            {
                return null;
            }
            return new SedesDto
            {
                ID_Sede = sedes.ID_Sede,
                Nombre_Sede = sedes.Nombre_Sede
            };
        }
        #endregion

        #region Crear Nueva SEDE
        public bool CreateSede(NombreSedeDto nombreSede)
        {
            if (nombreSede == null)
            {
                throw new ArgumentException(nameof(nombreSede), "El nombre no puede ser Vacio.");
            }
            try
            {
                var nuevaSede = new Sedes
                {
                    Nombre_Sede = nombreSede.Nombre_Sede
                };
                _context.Sedes.Add(nuevaSede);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al Crear la Nueva SEDE" + ex.Message);
            }
        } 
        #endregion

        #region Editar Nombres
        public bool UpdateNombre(int id, NombreSedeDto updatesedesDto)
        {
            try
            {
                var sedeExistente = _context.Sedes.FirstOrDefault(x => x.ID_Sede == id);
                if (sedeExistente == null)
                {
                    return false;
                }
                    sedeExistente.Nombre_Sede = updatesedesDto.Nombre_Sede;
                    _context.SaveChanges();
                    return true;
            }
            catch (Exception ex) 
            {
                throw new Exception("Error al Actualizar el nombre de la Sede: " + ex.Message);
            }
        }
        #endregion

    }
} 