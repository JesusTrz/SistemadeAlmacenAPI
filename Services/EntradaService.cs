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
    public class EntradaService : IEntradaService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public EntradaService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Registrar Entradas

        public bool RegistrarEntradayDetalles(EntradasDto entradasdto)
        {
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    var nuevaEntrada = new Entradas
                    {
                        Fecha = DateTime.Now.Date,
                        Hora = DateTime.Now.TimeOfDay,
                        ID_Proveedores = entradasdto.ID_Proveedores,
                        ID_Movimiento = entradasdto.ID_Movimiento,
                        Comentarios = entradasdto.Comentarios,
                        ID_Sede = entradasdto.ID_Sede,
                    };
                    _context.Entradas.Add(nuevaEntrada);
                    _context.SaveChanges();

                    foreach(var detalle in entradasdto.Detalles)
                    {
                        var detalleEntrada = new Detalle_Entrada
                        {
                            ID_Entradas = nuevaEntrada.ID_Entradas,
                            ID_Articulo = detalle.ID_Articulo,
                            Cantidad = detalle.Cantidad,
                            Precio_Unitario = detalle.Precio_Unitario,
                            Total = entradasdto.Detalles.Sum(d=>d.Cantidad*d.Precio_Unitario)
                        };
                        _context.Detalle_Entrada.Add(detalleEntrada);

                        var inventario = _context.Inventario
                            .FirstOrDefault(i => i.ID_Articulo == detalle.ID_Articulo && i.ID_Sede == entradasdto.ID_Sede);

                        if(inventario != null)
                        {
                            inventario.Stock_Actual += detalle.Cantidad;
                            inventario.Ultimo_Costo = detalle.Precio_Unitario;
                            inventario.Ultima_Compra = DateTime.Now;
                            inventario.Costo_Promedio = (inventario.Costo_Promedio + detalle.Precio_Unitario)/2;
                            inventario.Saldo = inventario.Stock_Actual * inventario.Costo_Promedio;
                            
                        }
                        else
                        {
                            return false;
                        }
                    }
                    _context.SaveChanges();
                    transaccion.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    throw new Exception("Error al registrar la Entrada" + ex.Message);
                }
            }
        }
        #endregion
    }
}