using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    public interface ISalidaService
    {
        bool ActualizarSalidasyDetalles(int idSalida, GetSalidasDto dto);
        List<GetSalidasDto> ObtenerSalidasporSede(int idSede);
        bool RegistrarSalidasyDetalles(SalidaDto salidaDto);
    }
}
