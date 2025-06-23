using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Services
{
    public class CuentaService :ICuentaService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public CuentaService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Obtener Cuenta
        public List<CuentaDto> GetAllCuenta()
        {
            return _context.Cuenta.Select(cuenta => new CuentaDto
            {
                ID_Cuenta = cuenta.ID_Cuenta,
                Nombre_Cuenta = cuenta.Nombre_Cuenta,
                Cuenta_Entrada = cuenta.Cuenta_Entrada,
                Cuenta_Salida = cuenta.Cuenta_Salida
            }).ToList();
        }

        public CuentaDto GetCuentasById(int id)
        {
            var cuenta = _context.Cuenta.FirstOrDefault(x => x.ID_Cuenta == id);
            if (cuenta == null)
            {
                return null;
            }
            else
            {
                return new CuentaDto
                {
                    ID_Cuenta = cuenta.ID_Cuenta,
                    Nombre_Cuenta = cuenta.Nombre_Cuenta,
                    Cuenta_Entrada = cuenta.Cuenta_Entrada,
                    Cuenta_Salida = cuenta.Cuenta_Salida
                };
            }
        }
        #endregion

        #region Crear Cuenta

        public bool CreateCuenta(DatosCuenta cuenta)
        {
            if (cuenta == null)
            {
                throw new ArgumentException(nameof(cuenta), "Los Datos no pueden ser vacios.");
            }
            try
            {
                var nuevaCuenta = new Cuenta
                {
                    Nombre_Cuenta = cuenta.Nombre_Cuenta,
                    Cuenta_Entrada = cuenta.Cuenta_Entrada,
                    Cuenta_Salida = cuenta.Cuenta_Salida
                };
                _context.Cuenta.Add(nuevaCuenta);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un Error al intentar agregar la Cuenta. " + ex.Message);
            }
        }

        #endregion

        #region Editar Cuenta
        // if(!string.IsNullOrEmpty(datosProv.(Campo de Modelo))) Linea para identificar si el dato fue cambiado o no
        public bool UpdateCuenta(int id, DatosCuenta datosCuenta)
        {
            try
            {
                var cuentaExistente = _context.Cuenta.FirstOrDefault(x => x.ID_Cuenta == id);
                if (cuentaExistente == null)
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(datosCuenta.Nombre_Cuenta))
                    cuentaExistente.Nombre_Cuenta = datosCuenta.Nombre_Cuenta;

                if (!string.IsNullOrEmpty(datosCuenta.Cuenta_Entrada))
                    cuentaExistente.Cuenta_Entrada = datosCuenta.Cuenta_Entrada;

                if (!string.IsNullOrEmpty(datosCuenta.Cuenta_Salida))
                    cuentaExistente.Cuenta_Salida = datosCuenta.Cuenta_Salida;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Actualizar el proveedor " + ex.Message);
            }
        }
        #endregion

        #region Eliminar Proveedor
        public bool DeleteCuenta(int id)
        {
            try
            {
                var cuenta = _context.Cuenta.FirstOrDefault(x => x.ID_Cuenta == id);
                if (cuenta == null)
                {
                    return false;
                }

                //Elimina la Relacion de Proveedor con la de entradas
                var deleteCuenta = _context.Linea.Where(dc => dc.ID_Cuenta == id).ToList();
                _context.Linea.RemoveRange(deleteCuenta);

                _context.Cuenta.Remove(cuenta);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el Proveedor: " + ex.Message);
            }
        }
        #endregion
    }
}