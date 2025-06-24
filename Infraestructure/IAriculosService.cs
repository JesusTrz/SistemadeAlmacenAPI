using SistemadeAlmacenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlmacenAPI.Infraestructure
{
    public interface IAriculosService
    {
        bool CreateArticulo(ArticulosDatos articulo);
        bool DeleteArticulos(int id);
        List<ArticuloDto> GetAllArticulos();
        ArticuloDto GetArticuloById(int id);
        bool UpdateArticulos(int id, ArticulosDatos articulosDatos);
    }
}
