//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SistemadeAlmacenAPI.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Inventario
    {
        public int ID_Inventario { get; set; }
        public Nullable<int> ID_Sede { get; set; }
        public Nullable<int> ID_Articulo { get; set; }
        public Nullable<int> Stock_Actual { get; set; }
        public Nullable<int> Stock_Minimo { get; set; }
        public Nullable<int> Stock_Maximo { get; set; }
        public string Ubicacion { get; set; }
        public Nullable<decimal> Costo_Promedio { get; set; }
        public Nullable<decimal> Saldo { get; set; }
        public Nullable<decimal> Ultimo_Costo { get; set; }
        public Nullable<System.DateTime> Ultima_Compra { get; set; }
    
        public virtual Articulo Articulo { get; set; }
        public virtual Sedes Sedes { get; set; }
    }
}
