using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Services
{
    public class CenCostoService : ICenCostoService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public CenCostoService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Obtener Centro de Costo
        public List<CenCostoDto> GetAllCentroCosto()
        {
            return _context.Centro_Costo.Select(costo => new CenCostoDto
            {
                ID_CenCost = costo.ID_CenCost,
                Nombre_CenCost = costo.Nombre_CenCost,
                Descripcion_CenCost = costo.Descripcion_CenCost
            }).ToList();
        }

        public CenCostoDto GetCentroCostoById(int id)
        {
            var costo = _context.Centro_Costo.FirstOrDefault(x => x.ID_CenCost == id);
            if (costo == null)
            {
                return null;
            }
            else
            {
                return new CenCostoDto
                {
                    ID_CenCost = costo.ID_CenCost,
                    Nombre_CenCost = costo.Nombre_CenCost,
                    Descripcion_CenCost = costo.Descripcion_CenCost
                };
            }
        }
        #endregion

        #region Agregar nuevo Centro de Costo
        public bool CrearCentroCosto(CenCostoDatos costo)
        {
            if (costo == null)
            {
                throw new ArgumentNullException(nameof(costo), "Los Datos no pueden ser Vacios");
            }
            try
            {
                var nuevoCentro = new Centro_Costo
                {
                    Nombre_CenCost = costo.Nombre_CenCost,
                    Descripcion_CenCost = costo.Descripcion_CenCost
                };
                _context.Centro_Costo.Add(nuevoCentro);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al intentar agregar el Centro de Costo" + ex.Message);
            }
        }
        #endregion

        #region Editar Centro Costo
        public bool UpdateCentroCosto(int id, CenCostoDatos datosCentro)
        {
            try
            {
                var centroExistente = _context.Centro_Costo.FirstOrDefault(x => x.ID_CenCost == id);
                if(centroExistente == null)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(datosCentro.Nombre_CenCost))
                    datosCentro.Nombre_CenCost = datosCentro.Nombre_CenCost;

                if (!string.IsNullOrEmpty(datosCentro.Descripcion_CenCost))
                    datosCentro.Descripcion_CenCost = datosCentro.Descripcion_CenCost;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Actualizar la Unidad " + ex.Message);
            }
        }
        #endregion

        #region Eliminar Centro de Costo
        public bool DeleteCentroCosto(int id)
        {
            try
            {
                var centro = _context.Centro_Costo.FirstOrDefault(x => x.ID_CenCost == id);
                if (centro == null)
                {
                    return false;
                }

                _context.Centro_Costo.Remove(centro);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el Centro de Costo: " + ex.Message);
            }
        }
        #endregion
    }
}