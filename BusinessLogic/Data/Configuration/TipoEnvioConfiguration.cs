using Core.Entities.OrdenCompra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data.Configuration
{
    public class TipoEnvioConfiguration : IEntityTypeConfiguration<Envio>
    {
        public void Configure(EntityTypeBuilder<Envio> builder)
        {
            builder.Property(t => t.Precio).HasColumnType("decimal(18,2");
        }
    }
}
