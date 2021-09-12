using Core.Entities;
using Core.Entities.OrdenCompra;
using Core.Interfaces;
using Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{
    public class OrdenCompraService : IOrdenCompraServices
    {
        
        private readonly IShoppingCartRepository _ShoppingCartRepository;
        private readonly IUnitofWork _unitofWork;

        public OrdenCompraService(IShoppingCartRepository shoppingCartRepository, IUnitofWork unitofWork)
        {
            _ShoppingCartRepository = shoppingCartRepository;
            _unitofWork = unitofWork;
        }

        public async Task<OrdenCompra> AddOrdenCompraAsync(string compradorEmail, int tipoEnvio, string carritoId, Core.Entities.OrdenCompra.Direccion direccion)
        {
            var ShoppingCart = await _ShoppingCartRepository.GetShoppingCartAsync(carritoId);
            var items = new List<OrderItem>();
            foreach(var item in ShoppingCart.Items)
            {
                var productItem = await _unitofWork.Repository<Producto>().GetByIdAsync(item.Id);

                var itemSort = new ProductoItemOrder(productItem.Id, productItem.Nombre, productItem.Imagen);
                var ordenItem = new OrderItem(itemSort, productItem.Precio, item.Cantidad);
                items.Add(ordenItem);
            }
            var TipoEnvioEntity =await _unitofWork.Repository<Envio>().GetByIdAsync(tipoEnvio);

            var Subtotal = items.Sum(item => item.Precio * item.Cantidad);
            var OrdenCompra = new OrdenCompra(compradorEmail, direccion, TipoEnvioEntity, items, Subtotal);
            _unitofWork.Repository<OrdenCompra>().AddEntity(OrdenCompra);
            var respone = await _unitofWork.Complete();
            if (respone <= 0)
            {
                return null;
            }
            await _ShoppingCartRepository.DeleteShoppingCartAsync(carritoId);
            return OrdenCompra;
        }

        public async Task<IReadOnlyList<Envio>> GetEnvios()
        {
            return await _unitofWork.Repository<Envio>().GetAllAsync();
        }

        public async Task<OrdenCompra> GetOrdenCompraByID(int id, string email)
        {
            var spec = new OrdenComprawithItemsSpecification(id, email);

            return await _unitofWork.Repository<OrdenCompra>().GetByIdWithSpec(spec);
        }

        public async Task<IReadOnlyList<OrdenCompra>> GetOrdenCompraByUserAsync(string email)
        {
            var spec = new OrdenComprawithItemsSpecification(email);

           return await _unitofWork.Repository<OrdenCompra>().GetAllWithSpec(spec);


        }
    }
}
