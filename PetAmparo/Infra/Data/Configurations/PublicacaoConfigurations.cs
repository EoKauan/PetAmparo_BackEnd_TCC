using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAmparo.Domain.Entities;

namespace PetAmparo.Infra.Data.Configurations
{
    public class PublicacaoConfigurations : IEntityTypeConfiguration<Publicacao>
    {
        public void Configure(EntityTypeBuilder<Publicacao> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Titulo)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Foto)
               .IsRequired(false);

            builder.Property(p => p.Data)
                .IsRequired();

            builder.Property(p => p.UsuarioId)                               
               .IsRequired();

            builder.ToTable("TB_Publicacao");
        }
    }
}
