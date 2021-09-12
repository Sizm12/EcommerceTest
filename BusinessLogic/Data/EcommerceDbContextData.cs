using Core.Entities;
using Core.Entities.OrdenCompra;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public class EcommerceDbContextData
    {
        public static async Task CargarDataAsync(EcommerceDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Marca.Any())
                {
                    var marcaData = File.ReadAllText("../BusinessLogic/CargarData/marca.json");
                    var marcas = JsonSerializer.Deserialize<List<Marca>>(marcaData);
                    foreach(var marca in marcas)
                    {
                        context.Marca.Add(marca);
                    }

                    await context.SaveChangesAsync();
                }
                if (!context.Categoria.Any())
                {
                    var CategoriaData = File.ReadAllText("../BusinessLogic/CargarData/categoria.json");
                    var categorias = JsonSerializer.Deserialize<List<Categoria>>(CategoriaData);
                    foreach (var categoria in categorias)
                    {
                        context.Categoria.Add(categoria);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.Store.Any())
                {
                    var StoreData = File.ReadAllText("../BusinessLogic/CargarData/tienda.json");
                    var stores = JsonSerializer.Deserialize<List<Store>>(StoreData);
                    foreach(var store in stores)
                    {
                        context.Store.Add(store);
                    }
                }
                

                if (!context.Producto.Any())
                {
                    var ProductoData = File.ReadAllText("../BusinessLogic/CargarData/producto.json");
                    var productos = JsonSerializer.Deserialize<List<Producto>>(ProductoData);
                    foreach (var producto in productos)
                    {
                        context.Producto.Add(producto);
                    }

                    await context.SaveChangesAsync();
                }
                if (!context.TipoEnvios.Any())
                {
                    var TipoEnvioData = File.ReadAllText("../BusinessLogic/CargarData/tipoenvio.json");
                    var envios = JsonSerializer.Deserialize<List<Envio>>(TipoEnvioData);
                    foreach (var envio in envios)
                    {
                        context.TipoEnvios.Add(envio);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch(Exception e)
            {
                var logger = loggerFactory.CreateLogger<EcommerceDbContextData>();
                logger.LogError(e.Message);
            }
        }
    }
}
