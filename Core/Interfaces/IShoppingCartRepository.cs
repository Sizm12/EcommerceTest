using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetShoppingCartAsync(string CartId);
        Task<ShoppingCart> UpdateShoppingCartAsync(ShoppingCart Cart);
        Task<bool> DeleteShoppingCartAsync(string CartId);
    }
}
