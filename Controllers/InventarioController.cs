using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using SistemadeAlmacenAPI.Models;
using SistemadeAlmacenAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SistemadeAlmacenAPI.Controllers
{
    public class InventarioController : ApiController
    {
        private readonly IInventarioService _inventarioService;

        public InventarioController()
        {
            _inventarioService = new InventarioService();
        }

        #region Obtener los Articulos del Inventario por SEDE
        [HttpGet]
        [Route("ObtenerArticulosaInventarioPor")]
        public IHttpActionResult ObtenerArticulosaInventario(int idSede)
        {
            var result = _inventarioService.ObtenerArticulosaInventario(idSede);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("No Encontrado");
            }
        }
        #endregion

        #region Filtros
        [HttpPost]
        [Route("ObtenerInventarioFiltrado")]

        public IHttpActionResult ObtenerInventarioFiltrado(InventarioFiltro filtros)
        {
            var result = _inventarioService.ObtenerInventarioFiltrado(filtros);
            return Ok(result);

        }
        #endregion

        #region Agregar Articulo al Inventario
        [HttpPost]
        [Route("AgregarArticuloInventario")]

        public IHttpActionResult AgregarArticuloaInventario(AgregarArticuloaInventario agArtInv)
        {
            if (agArtInv == null) return BadRequest("Datos Invalidos");

            var agregar = _inventarioService.AgregarArticuloaInventario(agArtInv);
            if (agregar)
            {
                return Ok($"El Articulo{agArtInv.ID_Articulo} fue agregado con exito!");
            }
            else
            {
                return BadRequest("Ocurrio un error al Agregar el Articulo.");
            }
        }
        #endregion

        #region Editar datos del Articulo
        [HttpPut]
        [Route("InventarioaArticulo")]

        public IHttpActionResult ActualizarInventario(int idInv, InventarioDatos datos)
        {
            if (datos == null) return BadRequest("Datos Invalidos");

            var servicio = new InventarioService();
            var modificar = servicio.ActualizarInventario(idInv, datos);
            if (modificar)
            {
                return Ok("Articulo Actualizado con Exito.");
            }
            else
            {
                return BadRequest("Error al Actualizar el Articulo.");
            } 
        }
        #endregion

        #region Agregar Stock del articulo ya Existente
        [HttpPost]
        [Route("AgregarStock")]

        public IHttpActionResult AgregarStockAInventario(StockEntrada dto)
        {
            try
            {

                var result = _inventarioService.AgregarStockAInventario(dto.ID_Sede, dto.ID_Articulo, dto.Cantidad);
                if (result)
                    return Ok($"El Stock del Articulo {dto.ID_Articulo} fue actualizado a {dto.Cantidad} unidades.");
                else
                    return BadRequest("Ocurrio un error al Actualizar el Stock.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        } 
        #endregion
    }
}
