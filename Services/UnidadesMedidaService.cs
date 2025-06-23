using SistemadeAlmacenAPI.Database;
using SistemadeAlmacenAPI.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Services
{
    public class UnidadesMedidaService : IUnidadesMedidaService
    {
        private readonly Sistema_de_Almacen_Entities _context;

        public UnidadesMedidaService()
        {
            _context = new Sistema_de_Almacen_Entities();
        }
    }
}