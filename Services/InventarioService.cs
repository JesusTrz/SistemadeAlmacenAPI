using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Services
{
    public class InventarioService : IInventarioService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public InventarioService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Obtener Articulo de Inventario
        public List<InventarioArticulos> ObtenerArticulosaInventario(int idSede)
        {
            var resultado = _context.Inventario
                .Where(i => i.ID_Sede == idSede)
                .Select(i => new InventarioArticulos
                {
                    ID_Articulo = i.ID_Inventario,
                    Nombre_Articulo = i.Articulo.Nombre_Articulo,
                    Descripcion_Articulo = i.Articulo.Descripcion_Articulo,
                    ID_Medida = i.Articulo.ID_Medida,
                    Nombre_Unidad = i.Articulo.Unidades_Medida.Nombre_Unidad,
                    Numero_Parte = i.Articulo.Numero_Parte,
                    Ubicacion = i.Ubicacion,
                    Stock_Actual = i.Stock_Actual,
                    Stock_Minimo = i.Stock_Minimo,
                    Stock_Maximo = i.Stock_Maximo,
                    Costo_Promedio = i.Costo_Promedio,
                    Saldo = i.Saldo,
                    Ultimo_Costo = i.Ultimo_Costo,
                    Ultima_Compra = i.Ultima_Compra,
                    ID_Linea = i.Articulo.ID_Linea,
                    Nombre_Linea = i.Articulo.Linea.Nombre_Linea
                }).ToList();
            return resultado;
        }

        #endregion

        #region Filtros
        public List<ExpandoObject> ObtenerInventarioFiltrado(InventarioFiltro filtros)
        {
            var inventario = _context.Inventario
                .Where(i => i.ID_Sede == filtros.ID_Sede)
                .Select(i => new
                {
                    i.ID_Inventario,
                    i.Stock_Actual,
                    i.Stock_Minimo,
                    i.Stock_Maximo,
                    i.Costo_Promedio,
                    i.Saldo,
                    i.Ultimo_Costo,
                    i.Ultima_Compra,
                    i.Ubicacion,
                    Articulo = new
                    {
                        i.Articulo.ID_Articulo,
                        i.Articulo.Nombre_Articulo,
                        i.Articulo.Descripcion_Articulo,
                        i.Articulo.Numero_Parte,
                        i.Articulo.Unidades_Medida.Nombre_Unidad,
                        i.Articulo.Linea.Nombre_Linea
                    }
                }).ToList();
            var listaFiltrada = new List<ExpandoObject>();

            foreach (var inv in inventario)
            {
                dynamic obj = new ExpandoObject();
                var dic = (IDictionary<string, object>)obj;

                foreach (var campo in filtros.Campos)
                {
                    switch (campo)
                    {
                        case "Nombre_Articulo":
                            dic[campo] = inv.Articulo.Nombre_Articulo;
                            break;

                        case "Descripcion_Articulo":
                            dic[campo] = inv.Articulo.Descripcion_Articulo;
                            break;

                        case "Nombre_Unidad":
                            dic[campo] = inv.Articulo.Nombre_Unidad;
                            break;

                        case "Numero_Parte":
                            dic[campo] = inv.Articulo.Numero_Parte;
                            break;

                        case "Ubicacion":
                            dic[campo] = inv.Ubicacion;
                            break;

                        case "Stock_Actual":
                            dic[campo] = inv.Stock_Actual;
                            break;

                        case "Stock_Minimo":
                            dic[campo] = inv.Stock_Minimo;
                            break;

                        case "Stock_Maximo":
                            dic[campo] = inv.Stock_Maximo;
                            break;

                        case "Costo_Promedio":
                            dic[campo] = inv.Costo_Promedio;
                            break;

                        case "Saldo":
                            dic[campo] = inv.Saldo;
                            break;

                        case "Ultimo_Costo":
                            dic[campo] = inv.Ultimo_Costo;
                            break;

                        case "Ultima_Compra":
                            dic[campo] = inv.Ultima_Compra;
                            break;

                        case "Nombre_Linea":
                            dic[campo] = inv.Articulo.Nombre_Linea;
                            break;

                        default:
                            break;
                    }
                }
                listaFiltrada.Add(obj);
            }
            return listaFiltrada;
        }
        #endregion

        #region Agregar Articulo a Inventario
        public bool AgregarArticuloaInventario(AgregarArticuloaInventario agArtInv)
        {
            try
            {
                var sedeExistente = _context.Sedes.Any(s=>s.ID_Sede == agArtInv.ID_Sede);
                var articuloExistente = _context.Articulo.Any(a => a.ID_Articulo == agArtInv.ID_Articulo);

                if (!sedeExistente) throw new Exception("La sede no Existe");
                if(!articuloExistente) throw new Exception("El Articulo no Existe");

                var registroArticulo = _context.Inventario
                    .Any(r => r.ID_Sede == agArtInv.ID_Sede && r.ID_Articulo == agArtInv.ID_Articulo);

                if(registroArticulo) throw new Exception("Este Articulo ya esta registrado en la esta SEDE.");

                var nuevoInv = new Inventario
                {
                    ID_Sede = agArtInv.ID_Sede,  
                    ID_Articulo = agArtInv.ID_Articulo,
                    Stock_Actual = 0,
                    Stock_Minimo = 0,
                    Stock_Maximo = 0,
                    Ubicacion = null,
                    Costo_Promedio = 0,
                    Saldo = 0,
                    Ultimo_Costo = 0,
                    Ultima_Compra = DateTime.Now
                };

                _context.Inventario.Add(nuevoInv);
                _context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar artículo al inventario: " + ex.Message);
            
            }
        }
        #endregion

        #region Actualizar Inventario
        public bool ActualizarInventario(int idInv, InventarioDatos datos)
        {
            try
            {
                var inventario = _context.Inventario.FirstOrDefault(i=>i.ID_Inventario == idInv);
                if (inventario == null) return false;

                inventario.Ubicacion = string.IsNullOrEmpty(datos.Ubicacion) ? "Sin Ubicacion" : datos.Ubicacion;

               if(datos.Stock_Actual.HasValue)
                    inventario.Stock_Actual = datos.Stock_Actual.Value;

                if (datos.Stock_Minimo.HasValue)
                    inventario.Stock_Minimo = datos.Stock_Minimo.Value;

                if (datos.Stock_Maximo.HasValue)
                    inventario.Stock_Maximo = datos.Stock_Maximo.Value;

                if(!string.IsNullOrEmpty(datos.Ubicacion))
                    inventario.Ubicacion = datos.Ubicacion;

                if (datos.Costo_Promedio.HasValue)
                    inventario.Costo_Promedio = datos.Costo_Promedio.Value;

                if (datos.Ultimo_Costo.HasValue)
                    inventario.Ultimo_Costo = datos.Ultimo_Costo.Value;

                if (datos.Ultima_Compra.HasValue)
                    inventario.Ultima_Compra = datos.Ultima_Compra.Value;

                if(inventario.Stock_Actual.HasValue && inventario.Costo_Promedio.HasValue)
                {
                    inventario.Saldo = inventario.Stock_Actual.Value * inventario.Costo_Promedio.Value;
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el inventario: " + ex.Message);
            }
        }
        #endregion

        #region Actualizar Stock
        public bool AgregarStockAInventario(int idSede, int idArticulo, int cantidadAgregada)
        {
            try
            {
                var inventario = _context.Inventario.FirstOrDefault(i=>i.ID_Sede ==  idSede && i.ID_Articulo == idArticulo);
                if(inventario == null) throw new Exception("El artículo no está registrado en el inventario de esta sede.");

                inventario.Stock_Actual += cantidadAgregada;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar stock al inventario: " + ex.Message);
            }
        }
        #endregion

    }
}