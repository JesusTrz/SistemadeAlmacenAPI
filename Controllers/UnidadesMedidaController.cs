using SistemadeAlmacenAPI.Infraestructure;
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
    }
}
