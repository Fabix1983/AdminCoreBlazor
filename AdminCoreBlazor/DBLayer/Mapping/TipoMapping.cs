using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminCoreBlazorClass;

namespace DBLayer.Mapping
{
    internal class TipoMapping : IEntityTypeConfiguration<Tipo>
    {
        public void Configure(EntityTypeBuilder<Tipo> builder)
        {
            builder.ToTable("Tipo").HasKey(x => x.Id);
            builder.HasIndex(x => x.TipoAttivita);
            builder.HasIndex(e => e.ColoreHTML);
            builder.Property(e => e.Tipologia);
            builder.Property(e => e.ColoreAzione);
        }
    }
}
