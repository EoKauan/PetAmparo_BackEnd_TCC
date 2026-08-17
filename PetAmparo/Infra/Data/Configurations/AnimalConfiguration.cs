using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAmparo.Domain.Entities;

namespace PetAmparo.Infra.Data.Configurations
{
    public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
    {
        public void Configure(EntityTypeBuilder<Animal> builder)
        {

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.EspecieId)
                .IsRequired();

            builder.Property(p => p.RacaId)
               .IsRequired();

            builder.Property(p => p.Idade)
                .IsRequired();

            builder.Property(p => p.Observacao)
               .HasMaxLength(200)
               .IsRequired();

            builder.Property(p => p.Status)
               .IsRequired();

            builder.Property(p => p.UsuarioId)
               .IsRequired();

            builder.ToTable("TB_Animal");
        }
    }
}
