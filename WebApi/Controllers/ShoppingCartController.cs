using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class ShoppingCartController : BaseApiController
    {
        private readonly IShoppingCartRepository _ShoppingCart;
        public ShoppingCartController(IShoppingCartRepository ShoppingCart)
        {
            _ShoppingCart = ShoppingCart;
        }

        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetCartByID(string id)
        {
            var Cart = await _ShoppingCart.GetShoppingCartAsync(id);
            return Ok(Cart ?? new ShoppingCart(id));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>>UpdateShoppingCart(ShoppingCart ParamsCart)
        {
            var Cart=await _ShoppingCart.UpdateShoppingCartAsync(ParamsCart);
            return Ok(Cart);
        }

        [HttpDelete]
        public async Task DeleteShoppingCart(string id)
        {
            await _ShoppingCart.DeleteShoppingCartAsync(id);
        }
    }
}
