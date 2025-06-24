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
    public class UnidadesMedidaController : ApiController
    {
        private readonly IUnidadesMedidaService _unidadesMedidaService;

        public UnidadesMedidaController()
        {
            _unidadesMedidaService = new UnidadesMedidaService();
        }

        #region Obtener Datos
        [HttpGet]
        [Route("GetAllUnidadesMedida")]

        public IHttpActionResult GetAllUMedida()
        {
            var result = _unidadesMedidaService.GetAllUMedida();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetUMedidaById/{Id}")]

        public IHttpActionResult GetUMedidaById(int id)
        {
            var result = _unidadesMedidaService.GetUMedidaById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Unidad de Medida no Encontrada");
            }
        }
        #endregion

        #region Insertar Unidad de Medida

        [HttpPost]
        [Route("CreateUnidadMedida")]

        public IHttpActionResult CreateUMedida(UnidadesMedidaNombre umedidanom)
        {
            if (umedidanom == null)
            {
                return BadRequest("La Unidad de medida no debe contener Datos Vacios");
            }
            var result = _unidadesMedidaService.CreateUMedida(umedidanom);
            if (result)
            {
                return Ok($"La Unidad de Medida '{umedidanom.Nombre_Unidad}' Fue Agregada con Exito!");
            }
            else
            {
                return BadRequest($"La Unidad de Medida '{umedidanom.Nombre_Unidad}' No pudo ser agreagada.");
            }


        }

        #endregion

        #region Editar Cuenta
        [HttpPut]
        [Route("UpdateUMedidaBy/{id}")]
        public IHttpActionResult UpdateUMedida(int id, UnidadesMedidaNombre umedidanom)
        {
            if (umedidanom == null)
            {
                return BadRequest("Ocurrio un error al editar la Unidad de Medida");
            }

            var result = _unidadesMedidaService.UpdateUMedida(id, umedidanom);
            if (result)
            {
                return Ok("Unidad de Medida Actualizada con Exito!");
            }
            else
            {
                return BadRequest("Error al Actualizar la Unidad de Medida");
            }
        }
        #endregion

        #region Eliminar Cuenta

        [HttpDelete]
        [Route("DeleteUnidaddeMedida")]
        public IHttpActionResult DeleteUMedida(int id)
        {
            var result = _unidadesMedidaService.DeleteUMedida(id);
            if (result)
            {
                return Ok("Unidad de Medida Eliminada con exito");
            }
            else
            {
                return BadRequest("Error al eliminar la Unidad de Medida");
            }
        }

        #endregion
    }
}
