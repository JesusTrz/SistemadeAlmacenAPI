using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    public interface IMovimientosService
    {
        bool CreateMovimiento(MovimientosDatos movimientos);
        bool DeleteMovimiento(int id);
        List<MovimientosDto> GetAllMovimientos();
        MovimientosDto GetMovimientosById(int id);
        bool UpdateMovimiento(int id, MovimientosDatos movimiento);
    }
}
