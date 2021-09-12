using Core.Entities;
using Core.Entities.OrdenCompra;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options) { }
        public DbSet<Producto> Producto { get; set; }

        public DbSet<Categoria> Categoria { get; set; }

        public DbSet<Marca> Marca { get; set; }
        public DbSet<Store> Store { get; set; }

        public DbSet<OrdenCompra> OrdenCompras { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Envio> TipoEnvios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
