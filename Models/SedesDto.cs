﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemadeAlmacenAPI.Models
{
    public class SedesDto
    {
        public int ID_Sede { get; set; }
        public string Nombre_Sede { get; set; }
    }

    public class NombreSedeDto
    {
        public string Nombre_Sede { get; set; }
    }
}