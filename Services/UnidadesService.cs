using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Services
{
    public class UnidadesService : IUnidadesService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public UnidadesService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Obtener Unidades
        public List<UnidadesDto> GetAllUnidades()
        {
            return _context.Unidades.Select(unidad => new UnidadesDto
            {
                ID_Unidad = unidad.ID_Unidad,
                Numero_Placa = unidad.Numero_Placa,
                Descripcion_Unidad = unidad.Descripcion_Unidad,
                ID_Sede = unidad.ID_Sede
            }).ToList();
        }

        public UnidadesDto GetUnidadById(int id)
        {
            var unidad = _context.Unidades.FirstOrDefault(x => x.ID_Unidad == id);
            if (unidad == null)
            {
                return null;
            }
            else
            {
                return new UnidadesDto
                {
                    ID_Unidad = unidad.ID_Unidad,
                    Numero_Placa = unidad.Numero_Placa,
                    Descripcion_Unidad = unidad.Descripcion_Unidad,
                    ID_Sede = unidad.ID_Sede
                };
            }
        }

        public List<UnidadesDto> GetUnidadesByIdSede(int idSede)
        {
            var result = _context.Unidades
                .Where(i=>i.ID_Sede == idSede)
                .Select(i=> new UnidadesDto
                {
                    ID_Unidad = i.ID_Unidad,
                    Numero_Placa= i.Numero_Placa,
                    Descripcion_Unidad = i.Descripcion_Unidad,
                    ID_Sede = i.ID_Sede
                }).ToList();
            return result;
        }
        #endregion

        #region Agregar Unidad
        public bool CreateUnidad(UnidadesDatos unidad)
        {
            try
            {
                var sedeUnidad = _context.Sedes.FirstOrDefault(u => u.ID_Sede == unidad.ID_Sede);
                if (sedeUnidad == null)
                {
                    throw new ArgumentException(nameof(unidad), "La sede Seleccionada no Existe");
                }
                var nuevaUnidad = new Unidades
                {
                    Numero_Placa = unidad.Numero_Placa,
                    Descripcion_Unidad = unidad.Descripcion_Unidad,
                    ID_Sede = unidad.ID_Sede
                };
                _context.Unidades.Add(nuevaUnidad);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al Intetar Agregar la Unidad" + ex.Message);
            }
        }

        #endregion

        #region Editar Personal
        public bool UpdateUnidad(int id, UnidadesDatos unidadDatos)
        {
            try
            {
                var unidadExistente = _context.Unidades.FirstOrDefault(x => x.ID_Unidad == id);
                if (unidadExistente == null)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(unidadExistente.Numero_Placa))
                    unidadExistente.Numero_Placa = unidadExistente.Numero_Placa;

                if (!string.IsNullOrEmpty(unidadExistente.Descripcion_Unidad))
                    unidadExistente.Descripcion_Unidad = unidadExistente.Descripcion_Unidad;

                if (unidadDatos.ID_Sede.HasValue)
                {
                    var sedeExistente = _context.Sedes.FirstOrDefault(s => s.ID_Sede == unidadDatos.ID_Sede.Value);
                    if (sedeExistente == null)
                    {
                        throw new Exception("La sede Ingresada no es Existente.");
                    }
                    unidadExistente.ID_Sede = unidadDatos.ID_Sede.Value;
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Actualizar los Datos de la Unidad" + ex.Message);
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
                throw new Exception("Error al eliminar la cuenta " + ex.Message);
            }
        }
        #endregion
    }
}