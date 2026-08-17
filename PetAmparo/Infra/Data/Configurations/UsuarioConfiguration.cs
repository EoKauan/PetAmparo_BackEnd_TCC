using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAmparo.Domain.Entities;

namespace PetAmparo.Infra.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Email)
               .HasMaxLength(150)
               .IsRequired();

            builder.Property(p => p.Senha)
               .HasMaxLength(100)
               .IsRequired();

            builder.Property(p => p.Telefone)
               .HasMaxLength(20)
               .IsRequired();

            builder.Property(p => p.Municipio
            )
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Foto)
               .IsRequired(false);

            builder.Property(p => p.Administrador)
                .IsRequired()
                .HasDefaultValue(false);

            builder.ToTable("TB_Usuario");
        }
    }
}
