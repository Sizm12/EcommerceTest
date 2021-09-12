using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrdenCompra
{
    public class OrderItem : ClaseBase
    {
        public OrderItem()
        {
        }

        public OrderItem(ProductoItemOrder itemOrder, decimal precio, int cantidad)
        {
            ItemOrder = itemOrder;
            Precio = precio;
            Cantidad = cantidad;
        }
        public ProductoItemOrder ItemOrder { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
