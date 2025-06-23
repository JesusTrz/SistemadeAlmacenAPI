using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    public interface IProveedoresService
    {
        bool CreateProveedor(ProveedoresDatos datosProv);
        bool DeleteProveedor(int id);
        List<ProveedoresDto> GetAllProveedores();
        ProveedoresDto GetProveedoresById(int id);
        bool UpdateProveedor(int id, ProveedoresDatos datosProv);
    }
}
