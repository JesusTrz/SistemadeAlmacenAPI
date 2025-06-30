using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    public interface IInventarioService
    {
        bool ActualizarInventario(int idInv, InventarioDatos datos);
        bool AgregarArticuloaInventario(AgregarArticuloaInventario agArtInv);
        bool AgregarStockAInventario(int idSede, int idArticulo, int cantidadAgregada);
        List<InventarioArticulos> ObtenerArticulosaInventario(int idSede);
        List<ExpandoObject> ObtenerInventarioFiltrado(InventarioFiltro filtros);
    }
}
