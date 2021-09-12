using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;

namespace WebApi.DTO
{
    public class OrdenCompraDTO
    {
        public string CarritoCompraId { get; set; }
        public int TipoEnvio { get; set; }
        public DireccionDto DireccionEnvio { get; set; }
    }
}
