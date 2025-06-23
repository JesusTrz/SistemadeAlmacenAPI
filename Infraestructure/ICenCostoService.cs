using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    public interface ICenCostoService
    {
        bool CrearCentroCosto(CenCostoDatos costo);
        bool DeleteCentroCosto(int id);
        List<CenCostoDto> GetAllCentroCosto();
        CenCostoDto GetCentroCostoById(int id);
        bool UpdateCentroCosto(int id, CenCostoDatos datosCentro);
    }
}
