using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAmparo.Domain.Entities;

namespace PetAmparo.Infra.Data.Configurations
{
    public class PublicacaoLikeConfigurations : IEntityTypeConfiguration<PublicacaoLike>
    {
        public void Configure(EntityTypeBuilder<PublicacaoLike> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PublicacaoId)
                .IsRequired();

            builder.Property(p => p.UsuarioId)
                .IsRequired();

            // Garantir que um usuário só pode dar like uma vez por publicação
            builder.HasIndex(p => new { p.PublicacaoId, p.UsuarioId })
                .IsUnique();

            builder.ToTable("TB_PublicacaoLike");
        }
    }
}

