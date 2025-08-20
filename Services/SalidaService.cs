using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Services
{
    public class SalidaService : ISalidaService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public SalidaService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Registrar Salidas y Detalles
        public bool RegistrarSalidasyDetalles(SalidaDto salidaDto)
        {
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {

                    foreach(var detalle in salidaDto.Detalles)
                    {
                        var inventario = _context.Inventario
                            .FirstOrDefault(i => i.ID_Articulo == detalle.ID_Articulo && i.ID_Sede == salidaDto.ID_Sede);

                        if(inventario == null)
                            throw new Exception($"No se encontró inventario para el artículo {detalle.ID_Articulo}");

                        if (inventario.Stock_Actual < detalle.Cantidad)
                            throw new Exception($"No hay suficiente stock para el artículo {detalle.ID_Articulo}. Stock actual: {inventario.Stock_Actual}, solicitado: {detalle.Cantidad}");
                    }

                    var nuevaSalida = new Salidas
                    {
                        Fecha = DateTime.Now.Date,
                        Hora = DateTime.Now.TimeOfDay,
                        ID_CenCost = salidaDto.ID_CenCost,
                        ID_Unidad = salidaDto.ID_Unidad,
                        ID_Personal = salidaDto.ID_Personal,
                        ID_Movimiento = salidaDto.ID_Movimiento,
                        Comentarios = salidaDto.Comentarios,
                        ID_Sede = salidaDto.ID_Sede,
                    };
                    _context.Salidas.Add(nuevaSalida);
                    _context.SaveChanges();

                    foreach ( var detalle in salidaDto.Detalles)
                    {
                        var inventario = _context.Inventario
                            .First(i => i.ID_Articulo == detalle.ID_Articulo && i.ID_Sede == salidaDto.ID_Sede);

                        var detalleSalida = new Detalle_Salida
                        {
                            ID_Salida = nuevaSalida.ID_Salida,
                            ID_Articulo = detalle.ID_Articulo,
                            Cantidad = detalle.Cantidad,
                            Precio_Unitario = detalle.Precio_Unitario,
                            Total = detalle.Cantidad * detalle.Precio_Unitario
                        };
                        _context.Detalle_Salida.Add(detalleSalida);
                            inventario.Stock_Actual -= detalle.Cantidad;
                            inventario.Ultimo_Costo = detalle.Precio_Unitario;
                            inventario.Ultima_Compra = DateTime.Now;
                            inventario.Saldo = inventario.Stock_Actual * inventario.Costo_Promedio;
                    }
                    _context.SaveChanges();
                    transaccion.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    throw new Exception("Error al registrar la Salida" + ex.Message);
                }
            }
        }
        #endregion

        #region Obtener Salidas
        public List<GetSalidasDto> ObtenerSalidasporSede(int idSede)
        {
            var result = _context.Salidas
                .Where(s => s.ID_Sede == idSede)
                .Select(s => new GetSalidasDto
                {
                    ID_Sede = s.ID_Sede,
                    ID_Salida = s.ID_Salida,
                    Fecha = s.Fecha,
                    Hora = s.Hora,
                    ID_Movimiento = s.ID_Movimiento,
                    Nombre_Movimiento = s.Movimientos.Nombre_Movimiento,
                    Descripcion_Movimiento = s.Movimientos.Descripcion_Movimiento,
                    ID_CenCost = s.ID_CenCost,
                    Nombre_CenCost = s.Centro_Costo.Nombre_CenCost,
                    ID_Unidad = s.ID_Unidad,
                    Numero_Placa = s.Unidades.Numero_Placa,
                    ID_Personal = s.ID_Personal,
                    Nombre = s.Personal.Nombre,
                    Comentarios = s.Comentarios,
                    Detalles = s.Detalle_Salida.Select(d => new GetDetalleSalidasDto
                    {
                        ID_Articulo = d.ID_Articulo,
                        Nombre_Articulo = d.Articulo.Nombre_Articulo,
                        Nombre_Unidad = d.Articulo.Unidades_Medida.Nombre_Unidad,
                        Cantidad = d.Cantidad,
                        Precio_Unitario = d.Precio_Unitario,
                        Total = d.Total
                    }).ToList()
                }).ToList();
            return result;
        }
        #endregion

        #region Actualizar Salidas y Detalles
        public bool ActualizarSalidasyDetalles(int idSalida, GetSalidasDto dto)
        {
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    var salida = _context.Salidas.FirstOrDefault(s => s.ID_Salida == idSalida);
                    if (salida == null) return false;
                    
                    salida.ID_CenCost = dto.ID_CenCost;
                    salida.ID_Unidad = dto.ID_Unidad;
                    salida.ID_Personal = dto.ID_Personal;
                    salida.Comentarios = dto.Comentarios;
                    salida.ID_Sede = dto.ID_Sede;
                    salida.Fecha = DateTime.Now.Date;
                    salida.Hora = DateTime.Now.TimeOfDay;

                    var detallesAntiguos = _context.Detalle_Salida.Where(d => d.ID_Salida == idSalida).ToList();
                    foreach (var antiguo in detallesAntiguos)
                    {
                        var inventario = _context.Inventario.FirstOrDefault(i => i.ID_Articulo == antiguo.ID_Articulo && i.ID_Sede == dto.ID_Sede);
                        if(inventario != null)
                        {
                            inventario.Stock_Actual -= antiguo.Cantidad;
                            inventario.Saldo = inventario.Stock_Actual * inventario.Costo_Promedio;
                        }
                    }
                    _context.Detalle_Salida.RemoveRange(detallesAntiguos);

                    foreach(var detalle in dto.Detalles)
                    {
                        var total = detalle.Cantidad * detalle.Precio_Unitario;

                        var nuevoDetalle = new Detalle_Salida
                        {
                            ID_Salida = idSalida,
                            ID_Articulo = detalle.ID_Articulo,
                            Cantidad = detalle.Cantidad,
                            Precio_Unitario = detalle.Precio_Unitario,
                            Total = detalle.Cantidad * detalle.Precio_Unitario
                        };
                        _context.Detalle_Salida.Add(nuevoDetalle);
                        var inventario = _context.Inventario.FirstOrDefault(i => i.ID_Articulo == detalle.ID_Articulo && i.ID_Sede == dto.ID_Sede);
                        if (inventario != null)
                        {
                            inventario.Stock_Actual -= detalle.Cantidad;
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
                    throw new Exception("Error al actualizar la salida: " + ex.Message);
                }
            }
        }
        #endregion
    }
}