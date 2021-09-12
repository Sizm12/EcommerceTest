using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrdenCompra
{
    public enum OrderStatus
    {
        [EnumMember(Value ="Pendiente")]
        Pendiente,
        [EnumMember(Value = "El Pago fue Recibido")]
        PagoRecibido,
        [EnumMember(Value = "El Pago no se Efectuo")]
        PagoFallo
    }
}
