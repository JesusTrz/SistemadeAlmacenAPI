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
    public class ProveedoresController : ApiController
    {
        private readonly IProveedoresService _proveedoresService;

        public ProveedoresController()
        {
            _proveedoresService = new ProveedoresService();
        }

        #region Obtener Datos
        [HttpGet]
        [Route("GetAllProveedores")]

        public IHttpActionResult GetAllProveedores()
        {
            var result = _proveedoresService.GetAllProveedores();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetProveedoresBy/{Id}")]

        public IHttpActionResult GetProveedoresById(int id)
        {
            var result = _proveedoresService.GetProveedoresById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Proveedor no Encontrado");
            }
        }
        #endregion

        #region Insertar Nuevo Proveedor

        [HttpPost]
        [Route("CreateProveedor")]

        public IHttpActionResult CreateProveedor(ProveedoresDatos datosProv)
        {
            if (datosProv == null)
            {
                return BadRequest("El Proveedor no debe contener Datos Vacios");
            }
            var result = _proveedoresService.CreateProveedor(datosProv);
            if (result)
            {
                return Ok($"El Proveedor '{datosProv.Razon_Social}' Fue Agregado con Exito!");
            }
            else
            {
                return BadRequest($"El Proveedor '{datosProv.Razon_Social}' No se pudo agreagar.");
            }
            
            
        }

        #endregion

        #region Editar Proveedor

        [HttpPut]
        [Route("UpdateProveedorBy/{id}")]
        public IHttpActionResult UpdateProveedor(int id, ProveedoresDatos datosProv)
        {
            if (datosProv == null)
            {
                return BadRequest("Ocurrio un error al editar el Proveedor");
            }

            var result = _proveedoresService.UpdateProveedor(id, datosProv);
            if (result)
            {
                return Ok("Proveedor Actualizado con Exito!");
            }
            else
            {
                return BadRequest("Error al Actualizar el Proveedor");
            }
        }
        #endregion

        #region Eliminar Proveedor

        [HttpDelete]
        [Route("DeleteProveedorBy/{id}")]
        public IHttpActionResult DeleteProveedor(int id)
        {
            var result = _proveedoresService.DeleteProveedor(id);
            if (result)
            {
                return Ok("Proveedor Eliminado con exito");
            }
            else
            {
                return BadRequest("Error al eliminar el Proveedor");
            }
        }

        #endregion
    }
}
