using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IDatabase _database;

        public ShoppingCartRepository(IConnectionMultiplexer redis) {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteShoppingCartAsync(string CartId)
        {
           return await _database.KeyDeleteAsync(CartId);
        }

        public async Task<ShoppingCart> GetShoppingCartAsync(string CartId)
        {
            var data=await _database.StringGetAsync(CartId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(data);
        }

        public async Task<ShoppingCart> UpdateShoppingCartAsync(ShoppingCart Cart)
        {
           var status= await _database.StringSetAsync(Cart.Id, JsonSerializer.Serialize(Cart), TimeSpan.FromDays(30));
            if (!status) return null;
            return await GetShoppingCartAsync(Cart.Id);

        }
    }
}
