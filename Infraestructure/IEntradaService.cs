using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    public interface IEntradaService
    {
        bool ActualizarEntradayDetalles(int idEntrada, GetEntradasDto dto);
        List<GetEntradasDto> ObtenerEntradasporSede(int idSede);
        bool RegistrarEntradayDetalles(EntradasDto entradasdto);
    }
}
