namespace PetAmparo.Domain.Entities
{
    public class PublicacaoLike
    {
        public Guid Id { get; set; }

        public Guid PublicacaoId { get; set; }

        public Guid UsuarioId { get; set; }

        public Publicacao Publicacao { get; set; } = null!;

        public Usuario Usuario { get; set; } = null!;
    }
}

