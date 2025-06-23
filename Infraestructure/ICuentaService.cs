using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    public interface ICuentaService
    {
        bool CreateCuenta(DatosCuenta cuenta);
        bool DeleteCuenta(int id);
        List<CuentaDto> GetAllCuenta();
        CuentaDto GetCuentasById(int id);
        bool UpdateCuenta(int id, DatosCuenta datosCuenta);
    }
}
