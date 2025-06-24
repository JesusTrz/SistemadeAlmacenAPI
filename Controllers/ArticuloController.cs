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
    public class ArticuloController : ApiController
    {
        private readonly IAriculosService _ariculosService;

        public ArticuloController()
        {
            _ariculosService = new AriculosService();
        }

        #region Obtener Datos
        [HttpGet]
        [Route("GetAllArticulos")]

        public IHttpActionResult GetAllArticulos()
        {
            var result = _ariculosService.GetAllArticulos();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetArticuloById/{Id}")]

        public IHttpActionResult GetArticuloById(int id)
        {
            var result = _ariculosService.GetArticuloById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Articulo no Encontrado");
            }
        }
        #endregion

        #region Agregar Articulos
        [HttpPost]
        [Route("AgregarArticulos")]

        public IHttpActionResult CreateArticulo(ArticulosDatos articulo)
        {
            if (articulo == null || articulo.ID_Linea == null || articulo.ID_Medida == null)
                return BadRequest("Datos Incompletos");

            try
            {
                var result = _ariculosService.CreateArticulo(articulo);
                if (result)
                    return Ok($"El Articulo {articulo.Nombre_Articulo} fue Agregado Exitosamente!");
                else
                    return BadRequest($"No se pudo agregar el Articulo {articulo.Nombre_Articulo} .");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Editar Articulo

        [HttpPut]
        [Route("UpdateArticuloBy/{id}")]

        public IHttpActionResult UpdateArticulos(int id, ArticulosDatos articulosDatos)
        {
            if (articulosDatos == null) return BadRequest("Ocurrio un error al editar el Articulo.");

            var result = _ariculosService.UpdateArticulos(id, articulosDatos);
            if (result)
                return Ok($"El Articulo {articulosDatos.Nombre_Articulo} fue actualizado con Exito!");
            else
                return BadRequest($"Ocurrio un error al querer editar el articulo {articulosDatos.Nombre_Articulo}.");
        }
        #endregion

        #region Eliminar Articulo
        [HttpDelete]
        [Route("DeleteArticuloBy/{id}")]

        public IHttpActionResult DeleteArticulos(int id)
        {
            var result = _ariculosService.DeleteArticulos(id);
            if (result)
                return Ok("Ariculo Eliminado con Exito!");
            else
                return BadRequest("Ocurrio un error al Eliminar el Articulo.");
        }
        #endregion
    }
}
