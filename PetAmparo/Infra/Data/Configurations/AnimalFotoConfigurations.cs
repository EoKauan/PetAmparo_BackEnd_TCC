using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAmparo.Domain.Entities;

namespace PetAmparo.Infra.Data.Configurations
{
    public class AnimalFotoConfigurations : IEntityTypeConfiguration<AnimalFoto>
    {
        public void Configure(EntityTypeBuilder<AnimalFoto> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.AnimalId)
                .IsRequired();
            
            builder.Property(p => p.Foto)
                .IsRequired();

            builder.ToTable("TB_AnimalFoto");
        }
    }
}
