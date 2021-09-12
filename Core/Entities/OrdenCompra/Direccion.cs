﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrdenCompra
{
    public class Direccion
    {
        public Direccion()
        {

        }
        public Direccion(string calle, string ciudad, string departamento, string codigopostal, string pais)
        {
            Calle = calle;
            Ciudad = ciudad;
            Departamento = departamento;
            CodigoPostal = codigopostal;
            Pais = pais;
        }

        public int Id { get; set; }
        public string Calle { get; set; }
        public string Ciudad { get; set; }
        public string Departamento { get; set; }

        public string CodigoPostal { get; set; }

        public string Pais { get; set; }
    }
}