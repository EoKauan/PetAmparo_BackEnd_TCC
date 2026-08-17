using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAmparo.Domain.Entities;

namespace PetAmparo.Infra.Data.Configurations
{
    public class RacaConfigurations : IEntityTypeConfiguration<Raca>
    {
        public void Configure(EntityTypeBuilder<Raca> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Descricao)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.EspecieId)
                .IsRequired();

            builder.HasOne(p => p.Especie)
                .WithMany()
                .HasForeignKey(p => p.EspecieId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("TB_Raca");            
        }
    }
}
