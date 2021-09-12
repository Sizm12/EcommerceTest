using Core.Entities.OrdenCompra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOrdenCompraServices
    {
        Task<OrdenCompra> AddOrdenCompraAsync(string compradoEmail, int tipoEnvio, string carritoId, Direccion direccion);
        Task<IReadOnlyList<OrdenCompra>> GetOrdenCompraByUserAsync(string email);
        Task<OrdenCompra> GetOrdenCompraByID(int id, string email);
        Task<IReadOnlyList<Envio>> GetEnvios();
    }
}
