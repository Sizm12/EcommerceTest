using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrdenCompra
{
    public class OrdenCompra : ClaseBase
    {
        public OrdenCompra()
        {

        }
        public OrdenCompra(string compradorEmail, Direccion direccionEnvio, Envio tipoEnvio, IReadOnlyList<OrderItem> orderItems, decimal subtotal)
        {
            CompradorEmail = compradorEmail;
            DireccionEnvio = direccionEnvio;
            TipoEnvio = tipoEnvio;
            OrderItems = orderItems;
            Subtotal = subtotal;
        }

        public string CompradorEmail { get; set; }
        public DateTimeOffset OrdenCompraFecha { get; set; } = DateTimeOffset.Now;
        public Direccion DireccionEnvio { get; set; }
        public Envio TipoEnvio { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pendiente;
        public string PagoIntentoId { get; set; }
        public decimal GetTotat()
        {
            return Subtotal + TipoEnvio.Precio;
        }
    }
}
