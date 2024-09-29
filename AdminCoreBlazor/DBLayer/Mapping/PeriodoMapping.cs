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
    internal class PeriodoMapping : IEntityTypeConfiguration<Periodo>
    {
        public void Configure(EntityTypeBuilder<Periodo> builder)
        {
            builder.ToTable("Periodo").HasKey(x => x.Id);
            builder.HasIndex(x => x.Anno);
            builder.HasIndex(e => e.Mese);
            builder.Property(e => e.Descrizione);
        }
    }
}
