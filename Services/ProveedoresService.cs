using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Services
{
    public class ProveedoresService : IProveedoresService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public ProveedoresService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Obtener Proveedores
        public List<ProveedoresDto> GetAllProveedores()
        {
            return _context.Proveedores.Select(proveedores => new ProveedoresDto
            {
                ID_Proveedores = proveedores.ID_Proveedores,
                RFC = proveedores.RFC,
                Razon_Social = proveedores.Razon_Social,
                Direccion = proveedores.Direccion,
                Telefono = proveedores.Telefono,
                Email = proveedores.Email
            }).ToList();
        }

        public ProveedoresDto GetProveedoresById(int id)
        {
            var proveedores = _context.Proveedores.FirstOrDefault(x => x.ID_Proveedores == id);
            if (proveedores == null)
            {
                return null;
            }
            else
            {
                return new ProveedoresDto
                {
                    ID_Proveedores = proveedores.ID_Proveedores,
                    RFC = proveedores.RFC,
                    Razon_Social = proveedores.Razon_Social,
                    Direccion = proveedores.Direccion,
                    Telefono = proveedores.Telefono,
                    Email = proveedores.Email
                };
            }
        }
        #endregion

        #region Crear Proveedores

        public bool CreateProveedor(ProveedoresDatos datosProv)
        {
            if (datosProv == null)
            {
                throw new ArgumentException(nameof(datosProv), "Los Datos no pueden ser vacios.");
            }
            try
            {
                var nuevoProv = new Proveedores
                {
                    RFC = datosProv.RFC,
                    Razon_Social = datosProv.Razon_Social,
                    Direccion = datosProv.Direccion,
                    Telefono = datosProv.Telefono,
                    Email = datosProv.Email
                };
                _context.Proveedores.Add(nuevoProv);
                _context.SaveChanges();
                return true;
                
                
            }
            catch (Exception ex) 
            {
                throw new Exception("Ocurrio un Error al intentar agregar el proveedor " + ex.Message);
            }
        }

        #endregion

        #region Editar Proveedor
        // if(!string.IsNullOrEmpty(datosProv.(Campo de Modelo))) Linea para identificar si el dato fue cambiado o no
        public bool UpdateProveedor(int id, ProveedoresDatos datosProv)
        {
            try
            {
                var proveedorExistente = _context.Proveedores.FirstOrDefault(x => x.ID_Proveedores == id);
                if(proveedorExistente == null)
                {
                    return false;
                }
                if(!string.IsNullOrEmpty(datosProv.RFC))
                    proveedorExistente.RFC = datosProv.RFC;

                if (!string.IsNullOrEmpty(datosProv.Razon_Social))
                    proveedorExistente.Razon_Social = datosProv.Razon_Social;

                if (!string.IsNullOrEmpty(datosProv.Direccion))
                    proveedorExistente.Direccion = datosProv.Direccion;

                if (!string.IsNullOrEmpty(datosProv.Telefono))
                    proveedorExistente.Telefono = datosProv.Telefono;

                if (!string.IsNullOrEmpty(datosProv.Email))
                    proveedorExistente.Email = datosProv.Email;

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
        public bool DeleteProveedor(int id)
        {
            try
            {
                var proveedor = _context.Proveedores.FirstOrDefault(x => x.ID_Proveedores == id);
                if (proveedor == null)
                {
                    return false;
                }

                //Elimina la Relacion de Proveedor con la de entradas
                var deleteEntradas = _context.Entradas.Where(de => de.ID_Proveedores == id).ToList();
                _context.Entradas.RemoveRange(deleteEntradas);

                _context.Proveedores.Remove(proveedor);
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