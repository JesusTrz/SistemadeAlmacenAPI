using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    public interface ILineaService
    {
        bool CreateLinea(LineaDatos linea);
        bool DeleteLinea(int id);
        List<LineaDto> GetAllLinea();
        LineaDto GetLineaById(int id);
        bool UpdateLinea(int id, LineaDatos datosLinea);
    }
}
