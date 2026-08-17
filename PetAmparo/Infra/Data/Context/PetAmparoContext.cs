using Microsoft.EntityFrameworkCore;
using PetAmparo.Domain.Entities;
using PetAmparo.Infra.Data.Configurations;

namespace PetAmparo.Infra.Data.Context
{
    public class PetAmparoContext : DbContext
    {
        public DbSet<Animal> AnimalSet { get; set; }

        public DbSet<Usuario> UsuarioSet { get; set; }

        public DbSet<Publicacao> PublicacaoSet { get; set; }

        public DbSet<Raca> RacaSet { get; set; }

        public DbSet<Especie> EspecieSet { get; set; }

        public DbSet<AnimalFoto> AnimalFotoSet { get; set; }

        public DbSet<PublicacaoLike> PublicacaoLikeSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new AnimalConfiguration());
            modelBuilder.ApplyConfiguration(new PublicacaoConfigurations());
            modelBuilder.ApplyConfiguration(new AnimalFotoConfigurations());
            modelBuilder.ApplyConfiguration(new RacaConfigurations());
            modelBuilder.ApplyConfiguration(new EspecieConfigurations());
            modelBuilder.ApplyConfiguration(new PublicacaoLikeConfigurations());
               
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string conexao = "server=mysql.tccnapratica.com.br;database=tccnapratica15;port=3306;uid=tccnapratica15;password=3H3tcc2025";
            optionsBuilder.UseMySql(conexao, ServerVersion.AutoDetect(conexao));

            base.OnConfiguring(optionsBuilder);
        }

    }
}
