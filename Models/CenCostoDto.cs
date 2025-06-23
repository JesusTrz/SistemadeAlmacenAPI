using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Models
{
    public class CenCostoDto
    {
        public int ID_CenCost { get; set; }
        public string Nombre_CenCost { get; set; }
        public string Descripcion_CenCost { get; set; }
    }

    public class CenCostoDatos
    {
        public string Nombre_CenCost { get; set; }
        public string Descripcion_CenCost { get; set; }
    }
}