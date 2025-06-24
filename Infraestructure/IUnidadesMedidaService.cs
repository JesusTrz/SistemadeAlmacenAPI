using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    public interface IUnidadesMedidaService
    {
        bool CreateUMedida(UnidadesMedidaNombre umedidanom);
        bool DeleteUMedida(int id);
        List<UnidadesMedidaDto> GetAllUMedida();
        UnidadesMedidaDto GetUMedidaById(int id);
        bool UpdateUMedida(int id, UnidadesMedidaNombre umedidanom);
    }
}
