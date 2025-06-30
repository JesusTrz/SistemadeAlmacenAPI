using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Models
{
    public class EntradasDto
    {
        public Nullable<int> ID_Movimiento { get; set; }
        public Nullable<int> ID_Proveedores { get; set; }
        public Nullable<int> ID_Sede { get; set; }
        public string Comentarios { get; set; }
        public List<DestallesEntradasDto> Detalles { get; set; }
    }

    public class DestallesEntradasDto
    {
        public Nullable<int> ID_Articulo { get; set; }
        public Nullable<int> Cantidad { get; set; }
        public Nullable<decimal> Precio_Unitario { get; set; }
    }
}