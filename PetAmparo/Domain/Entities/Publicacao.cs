namespace PetAmparo.Domain.Entities
{
    public class Publicacao
    {
        public Guid Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public DateTime Data {  get; set; }

        public string? Foto { get; set; }

        //public int QuantidadeLike { get; set; }

        public Guid UsuarioId { get; set; }

        public Usuario Usuario { get; set; } = null!;

        //public List<PublicacaoLike> PublicacaoLikes { get; set; } = [];
    }
}
