using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Services
{
    public class AriculosService : IAriculosService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public AriculosService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }

        #region Obtener Datos
        public List<ArticuloDto> GetAllArticulos()
        {
            return _context.Articulo.Select(articulo => new ArticuloDto
            {
                ID_Articulo = articulo.ID_Articulo,
                Nombre_Articulo = articulo.Nombre_Articulo,
                Descripcion_Articulo = articulo.Descripcion_Articulo,
                Numero_Parte = articulo.Numero_Parte,
                ID_Linea = articulo.ID_Linea,
                ID_Medida = articulo.ID_Medida
            }).ToList();
        }

        public ArticuloDto GetArticuloById(int id)
        {
            var articulo = _context.Articulo.FirstOrDefault(x => x.ID_Articulo == id);
            if (articulo == null) return null;

            return new ArticuloDto
            {
                ID_Articulo = articulo.ID_Articulo,
                Nombre_Articulo = articulo.Nombre_Articulo,
                Descripcion_Articulo = articulo.Descripcion_Articulo,
                Numero_Parte = articulo.Numero_Parte,
                ID_Linea = articulo.ID_Linea,
                ID_Medida = articulo.ID_Medida
            };
        }
        #endregion

        #region Crear nuevos Articulos
        public bool CreateArticulo(ArticulosDatos articulo)
        {
            if(articulo == null) throw new ArgumentNullException(nameof(articulo), "Los datos no pueden ser nulos.");

            try
            {
                var lineaExistente = _context.Linea.Any(l=>l.ID_Linea == articulo.ID_Linea);
                if(!lineaExistente) throw new Exception("La Linea proporcionada no existe.");

                var medidaExistente = _context.Unidades_Medida.Any(u => u.ID_Medida == articulo.ID_Medida);
                if(!medidaExistente) throw new Exception("La Medida proporcionada no existe.");

                var nuevoArticulo = new Articulo
                {
                    Nombre_Articulo = articulo.Nombre_Articulo,
                    Descripcion_Articulo = articulo.Descripcion_Articulo,
                    Numero_Parte = articulo.Numero_Parte,
                    ID_Linea = articulo.ID_Linea,
                    ID_Medida = articulo.ID_Medida
                };
                _context.Articulo.Add(nuevoArticulo);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al Intetar Agregar un Articulo" + ex.Message);
            }
        }
        #endregion

        #region Editar Articulos
        public bool UpdateArticulos(int id, ArticulosDatos articulosDatos)
        {
            try
            {
                var articuloExistente = _context.Articulo.FirstOrDefault(x => x.ID_Articulo == id);
                if (articuloExistente == null) return false;

                if (!string.IsNullOrEmpty(articulosDatos.Nombre_Articulo))
                    articuloExistente.Nombre_Articulo = articulosDatos.Nombre_Articulo;

                if (!string.IsNullOrEmpty(articulosDatos.Descripcion_Articulo))
                    articuloExistente.Descripcion_Articulo = articulosDatos.Descripcion_Articulo;

                if (articulosDatos.ID_Linea.HasValue)
                {
                    var lineaExistente = _context.Linea.FirstOrDefault(l => l.ID_Linea == articulosDatos.ID_Linea.Value);
                    if(lineaExistente == null) throw new Exception("La Linea Ingresada no es Existente.");
                    articuloExistente.ID_Linea = articulosDatos.ID_Linea.Value;
                }

                if (articuloExistente.ID_Medida.HasValue)
                {
                    var medidaExistente = _context.Unidades_Medida.FirstOrDefault(m => m.ID_Medida == articulosDatos.ID_Medida.Value);
                    if(medidaExistente == null) throw new Exception("La Unidad de Medida Ingresada no es Existente.");
                    articuloExistente.ID_Medida = articulosDatos.ID_Medida.Value;
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Actualizar los Datos del Articulo" + ex.Message);
            }
        }
        #endregion

        #region Eliminar Articulos
        public bool DeleteArticulos(int id)
        {
            try
            {
                var articulo = _context.Articulo.FirstOrDefault(a=>a.ID_Articulo == id);
                if(articulo == null) return false;

                var articuloInventario = _context.Inventario.Where(i => i.ID_Articulo == id).ToList();
                _context.Inventario.RemoveRange(articuloInventario);
                _context.Articulo.Remove(articulo);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el Articulo " + ex.Message);
            }
        }
        #endregion
    }
}