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
    public class LineaService : ILineaService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public LineaService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Obtener Linea
        public List<LineaDto> GetAllLinea()
        {
            return _context.Linea.Select(linea => new LineaDto
            {
                ID_Linea = linea.ID_Linea,
                Nombre_Linea = linea.Nombre_Linea,
                Descripcion_Linea = linea.Descripcion_Linea,
                ID_Cuenta = linea.ID_Cuenta,
                Nombre_Cuenta = linea.Cuenta.Nombre_Cuenta
            }).ToList();
        }

        public LineaDto GetLineaById(int id)
        {
            var linea = _context.Linea.FirstOrDefault(x => x.ID_Linea == id);
            if (linea == null)
            {
                return null;
            }
            else
            {
                return new LineaDto
                {
                    ID_Linea = linea.ID_Linea,
                    Nombre_Linea = linea.Nombre_Linea,
                    Descripcion_Linea = linea.Descripcion_Linea,
                    ID_Cuenta = linea.ID_Cuenta,
                    Nombre_Cuenta = linea.Cuenta.Nombre_Cuenta
                };
            }
        }
        #endregion

        #region Crear Linea

        public bool CreateLinea(LineaDatos linea)
        {
           
            try
            {
                var cuenta = _context.Cuenta.FirstOrDefault(x => x.ID_Cuenta == linea.ID_Cuenta);
                if (cuenta == null)
                {
                    throw new ArgumentException(nameof(linea), "La Cuenta Asociada no Existe.");
                }
                var nuevaLinea = new Linea
                {
                    Nombre_Linea = linea.Nombre_Linea,
                    Descripcion_Linea = linea.Descripcion_Linea,
                    ID_Cuenta = linea.ID_Cuenta
                };
                _context.Linea.Add(nuevaLinea);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un Error al intentar agregar la Linea. " + ex.Message);
            }
        }

        #endregion

        #region Editar Linea
        // if(!string.IsNullOrEmpty(datosProv.(Campo de Modelo))) Linea para identificar si el dato fue cambiado o no
        public bool UpdateLinea(int id, LineaDatos datosLinea)
        {
            try
            {
                var lineaExistente = _context.Linea.FirstOrDefault(x => x.ID_Linea == id);
                if (lineaExistente == null)
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(datosLinea.Nombre_Linea))
                    lineaExistente.Nombre_Linea = datosLinea.Nombre_Linea;

                if (!string.IsNullOrEmpty(datosLinea.Descripcion_Linea))
                    lineaExistente.Descripcion_Linea = datosLinea.Descripcion_Linea;

                if (datosLinea.ID_Cuenta.HasValue)
                {
                    var cuentaExistente = _context.Cuenta.FirstOrDefault(c => c.ID_Cuenta == datosLinea.ID_Cuenta.Value);
                    if(cuentaExistente == null)
                    {
                        throw new Exception("La cuenta proporcionada no existe. ");
                    }
                    lineaExistente.ID_Cuenta = datosLinea.ID_Cuenta.Value;
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Actualizar la Linea " + ex.Message);
            }
        }
        #endregion

        #region Eliminar Proveedor
        public bool DeleteLinea(int id)
        {
            try
            {
                var linea = _context.Linea.FirstOrDefault(x => x.ID_Linea == id);
                if (linea == null)
                {
                    return false;
                }

                var deleteLinea = _context.Articulo.Where(dl => dl.ID_Linea == id).ToList();
                _context.Articulo.RemoveRange(deleteLinea);

                _context.Linea.Remove(linea);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la Cuenta: " + ex.Message);
            }
        }
        #endregion
    }
}