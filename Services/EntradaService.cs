using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Xml;

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

                    foreach (var detalle in entradasdto.Detalles)
                    {
                        var detalleEntrada = new Detalle_Entrada
                        {
                            ID_Entradas = nuevaEntrada.ID_Entradas,
                            ID_Articulo = detalle.ID_Articulo,
                            Cantidad = detalle.Cantidad,
                            Precio_Unitario = detalle.Precio_Unitario,
                            //Total = entradasdto.Detalles.Sum(d => d.Cantidad * d.Precio_Unitario)
                            Total = detalle.Cantidad * detalle.Precio_Unitario
                        };
                        _context.Detalle_Entrada.Add(detalleEntrada);

                        var inventario = _context.Inventario
                            .FirstOrDefault(i => i.ID_Articulo == detalle.ID_Articulo && i.ID_Sede == entradasdto.ID_Sede);

                        if (inventario != null)
                        {
                            inventario.Stock_Actual += detalle.Cantidad;
                            inventario.Ultimo_Costo = detalle.Precio_Unitario;
                            inventario.Ultima_Compra = DateTime.Now;
                            inventario.Costo_Promedio = (inventario.Costo_Promedio + detalle.Precio_Unitario) / 2;
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

        #region Obtener entradas de sede
        public List<GetEntradasDto> ObtenerEntradasporSede(int idSede)
        {
            var result = _context.Entradas
                .Where(e => e.ID_Sede == idSede)
                .Select(e => new GetEntradasDto
                {
                    ID_Sede = e.ID_Sede,
                    ID_Entradas = e.ID_Entradas,
                    Fecha = e.Fecha,
                    Hora = e.Hora,
                    ID_Movimiento = e.ID_Movimiento,
                    Nombre_Movimiento = e.Movimientos.Nombre_Movimiento,
                    Descripcion_Movimiento = e.Movimientos.Descripcion_Movimiento,
                    ID_Proveedores = e.ID_Proveedores,
                    Razon_Social = e.Proveedores.Razon_Social,
                    Comentarios = e.Comentarios,
                    Detalles = e.Detalle_Entrada.Select(d => new GetDetallesEntradasDto
                    {
                        ID_Articulo = d.ID_Articulo,
                        Cantidad = d.Cantidad,
                        Precio_Unitario = d.Precio_Unitario,
                        Total = d.Total
                    }).ToList()
                }).ToList();
            return result;
        }
        #endregion

        #region Modificar Entradas y Detalles
        public bool ActualizarEntradayDetalles(int idEntrada, GetEntradasDto dto)
        {
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    var entrada = _context.Entradas.FirstOrDefault(e => e.ID_Entradas == idEntrada);
                    if (entrada == null) return false;

                    entrada.ID_Proveedores = dto.ID_Proveedores;
                    entrada.ID_Movimiento = dto.ID_Movimiento;
                    entrada.Comentarios = dto.Comentarios;
                    entrada.ID_Sede = dto.ID_Sede;
                    entrada.Fecha = DateTime.Now.Date;
                    entrada.Hora = DateTime.Now.TimeOfDay;

                    var detallesAntiguos = _context.Detalle_Entrada.Where(d=>d.ID_Entradas == idEntrada).ToList();
                    foreach (var antiguo in detallesAntiguos)
                    {
                        var inventario = _context.Inventario.FirstOrDefault(i => i.ID_Articulo == antiguo.ID_Articulo && i.ID_Sede == dto.ID_Sede);
                        if (inventario != null)
                        {
                            inventario.Stock_Actual -= antiguo.Cantidad;
                            inventario.Saldo = inventario.Stock_Actual * inventario.Costo_Promedio;
                        }
                    }
                    _context.Detalle_Entrada.RemoveRange(detallesAntiguos);

                    foreach(var detalle in dto.Detalles)
                    {
                        var total = detalle.Cantidad * detalle.Precio_Unitario;

                        var nuevoDetalle = new Detalle_Entrada
                        {
                            ID_Entradas = idEntrada,
                            ID_Articulo = detalle.ID_Articulo,
                            Cantidad = detalle.Cantidad,
                            Precio_Unitario = detalle.Precio_Unitario,
                            Total = total
                        };
                        _context.Detalle_Entrada.Add(nuevoDetalle);
                        var inventario = _context.Inventario.FirstOrDefault(i => i.ID_Articulo == detalle.ID_Articulo && i.ID_Sede == dto.ID_Sede);
                        if (inventario != null)
                        {
                            inventario.Stock_Actual += detalle.Cantidad;
                            inventario.Ultimo_Costo = detalle.Precio_Unitario;
                            inventario.Ultima_Compra = DateTime.Now;
                            inventario.Costo_Promedio = (inventario.Costo_Promedio + detalle.Precio_Unitario) / 2;
                            inventario.Saldo = inventario.Stock_Actual * inventario.Costo_Promedio;
                        }
                        else
                        {
                            throw new Exception("Inventario no encontrado para el artículo " + detalle.ID_Articulo);
                        }
                    }
                    _context.SaveChanges();
                    transaccion.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    throw new Exception("Error al actualizar la entrada: " + ex.Message);
                }
            }
        }
        #endregion
    }
}