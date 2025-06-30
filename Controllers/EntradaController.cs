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
    public class EntradaController : ApiController
    {
        private readonly IEntradaService _entradaService;

        public EntradaController()
        {
            _entradaService = new EntradaService();
        }

        [HttpPost]
        [Route("RegistrarEntradayDetalles")]

        public IHttpActionResult RegistrarEntradayDetalles(EntradasDto entradasdto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = _entradaService.RegistrarEntradayDetalles(entradasdto);
                if (result) return Ok("La entrada fue registrada exitosamente!");
                else return BadRequest("Ocurrio un error al registrar la Entrada");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Ocurrió un error al registrar la entrada: " + ex.Message));
            }
        }
    }
}
