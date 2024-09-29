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
    internal class AttivitaMapping : IEntityTypeConfiguration<Attivita>
    {
        public void Configure(EntityTypeBuilder<Attivita> builder)
        {
            builder.ToTable("Attivita").HasKey(x => x.Id);
            builder.HasIndex(x => x.Giorno);
            builder.HasIndex(e => e.Dettagli);
            builder.Property(e => e.Costo);
            builder
                .HasOne(e => e.Periodo)
                .WithMany(s => s.Attivita)
                .HasForeignKey(e => e.RifPeriodo) 
                .IsRequired();
            builder
                .HasOne(e => e.Tipo)
                .WithMany(s => s.Attivita)
                .HasForeignKey(e => e.RifTipoAttivita)
                .IsRequired();
        }
    }
}
