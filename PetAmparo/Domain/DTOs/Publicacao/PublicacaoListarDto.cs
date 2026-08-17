namespace PetAmparo.Domain.DTOs.Publicacao
{
    public class PublicacaoListarDto
    {
        public Guid Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public Guid UsuarioId { get; set; }
        public string UsuarioNome { get; set; } = string.Empty;
        public string UsuarioFoto { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public DateTime Data { get; set; }

        public string Foto { get; set; } = string.Empty;

        public bool Like { get; set; }

        public int QuantidadeLike { get; set; }

        public List<Guid> LikeUsuario { get; set; } = [];
    }
}
