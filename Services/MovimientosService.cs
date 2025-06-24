using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SistemadeAlmacenAPI.Services
{
    public class MovimientosService : IMovimientosService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public MovimientosService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Obtener Datos
        public List<MovimientosDto> GetAllMovimientos()
        {
            return _context.Movimientos.Select(movimientos => new MovimientosDto
            {
                ID_Movimiento = movimientos.ID_Movimiento,
                Nombre_Movimiento = movimientos.Nombre_Movimiento,
                Descripcion_Movimiento = movimientos.Descripcion_Movimiento,
                Tipo = movimientos.Tipo
            }).ToList();
        }

        public MovimientosDto GetMovimientosById(int id)
        {
            var movimientos = _context.Movimientos.FirstOrDefault(x => x.ID_Movimiento == id);
            if (movimientos == null)
            {
                return null;
            }
            else
            {
                return new MovimientosDto
                {
                    ID_Movimiento = movimientos.ID_Movimiento,
                    Nombre_Movimiento = movimientos.Nombre_Movimiento,
                    Descripcion_Movimiento = movimientos.Descripcion_Movimiento,
                    Tipo = movimientos.Tipo
                };
            }
        }
        #endregion

        #region Agregar Movimiento
        public bool CreateMovimiento(MovimientosDatos movimientos)
        {
            if (movimientos == null)
            {
                throw new ArgumentException(nameof(movimientos), "Los Datos no pueden ser vacios.");
            }
            try
            {

                var tipoValido = new[] { "Entrada", "Salida" };
                if (!tipoValido.Contains(movimientos.Tipo))
                {
                    throw new ArgumentException("Tipo de movimiento inválido. Solo se permite 'Entrada' o 'Salida'.");
                }

                var nuevoMovimiento = new Movimientos
                {
                    Nombre_Movimiento = movimientos.Nombre_Movimiento,
                    Descripcion_Movimiento = movimientos.Descripcion_Movimiento,
                    Tipo = movimientos.Tipo
                };
                _context.Movimientos.Add(nuevoMovimiento);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un Error al intentar agregar el Movimiento. " + ex.Message);
            }
        }
        #endregion

        #region Editar Movimiento
        public bool UpdateMovimiento(int id, MovimientosDatos movimiento)
        {
            try
            {
                var tipoValido = new[] { "Entrada", "Salida" };
                if (!tipoValido.Contains(movimiento.Tipo))
                {
                    throw new ArgumentException("Tipo de movimiento inválido. Solo se permite 'Entrada' o 'Salida'.");
                }

                var movimientoExistente = _context.Movimientos.FirstOrDefault(x=>x.ID_Movimiento == id);
                if (movimientoExistente == null)
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(movimiento.Nombre_Movimiento))
                    movimientoExistente.Nombre_Movimiento = movimiento.Nombre_Movimiento;
                if (!string.IsNullOrEmpty(movimiento.Descripcion_Movimiento))
                    movimientoExistente.Descripcion_Movimiento = movimiento.Descripcion_Movimiento;
                if (!string.IsNullOrEmpty(movimiento.Tipo))
                    movimientoExistente.Tipo = movimiento.Tipo;

                _context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al Actualizar el Movimiento " + ex.Message);
            }
        }
        #endregion

        #region Eliminar Movimiento
        public bool DeleteMovimiento(int id)
        {
            try
            {
                var movimiento = _context.Movimientos.FirstOrDefault(x=>x.ID_Movimiento==id);
                if (movimiento== null)
                {
                    return false;
                }
                else
                {
                    _context.Movimientos.Remove(movimiento);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el Movimiento: " + ex.Message);
            }
        }
        #endregion
    }
}