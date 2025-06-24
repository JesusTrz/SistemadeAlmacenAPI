using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Services
{
    public class UnidadesMedidaService : IUnidadesMedidaService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public UnidadesMedidaService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Obtener Medidas
        public List<UnidadesMedidaDto> GetAllUMedida()
        {
            return _context.Unidades_Medida.Select(umedida => new UnidadesMedidaDto
            {
                ID_Medida = umedida.ID_Medida,
                Nombre_Unidad = umedida.Nombre_Unidad
            }).ToList();
        }

        public UnidadesMedidaDto GetUMedidaById(int id)
        {
            var umedida = _context.Unidades_Medida.FirstOrDefault(x=>x.ID_Medida == id);
            if (umedida == null)
            {
                return null;
            }
            else
            {
                return new UnidadesMedidaDto
                {
                    ID_Medida = umedida.ID_Medida,
                    Nombre_Unidad = umedida.Nombre_Unidad
                };
            }
        }
        #endregion

        #region Agregar Unidad de Medida
        public bool CreateUMedida(UnidadesMedidaNombre umedidanom)
        {
            if (umedidanom == null)
            {
                throw new ArgumentException(nameof(umedidanom), "Los Datos no pueden ser vacios.");
            }
            try
            {
                var nuevaMedida = new Unidades_Medida
                {
                    Nombre_Unidad = umedidanom.Nombre_Unidad
                };
                _context.Unidades_Medida.Add(nuevaMedida);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un Error al intentar agregar la Unidad. " + ex.Message);
            }
        }
        #endregion

        #region Editar Unidad de Medida
        public bool UpdateUMedida(int id, UnidadesMedidaNombre umedidanom)
        {
            try
            {
                var unidadExistente = _context.Unidades_Medida.FirstOrDefault(x=>x.ID_Medida == id);
                if (unidadExistente == null)
                {
                    return false;
                }
                unidadExistente.Nombre_Unidad = umedidanom.Nombre_Unidad;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Actualizar la Unidad de Medida: " + ex.Message);
            }
        }
        #endregion

        #region Eliminar Unidad de Medida
        public bool DeleteUMedida(int id)
        {
            try
            {
                var umedida = _context.Unidades_Medida.FirstOrDefault(x => x.ID_Medida == id);
                if (umedida == null)
                {
                    return false;
                }
                else
                {
                    var eliminarMedida = _context.Articulo.Where(em => em.ID_Medida == id).ToList();
                    _context.Articulo.RemoveRange(eliminarMedida);
                    _context.Unidades_Medida.Remove(umedida);
                    _context.SaveChanges();
                    return true;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la Unidad de Medida: " + ex.Message);
            }
        }
        #endregion
    }
}