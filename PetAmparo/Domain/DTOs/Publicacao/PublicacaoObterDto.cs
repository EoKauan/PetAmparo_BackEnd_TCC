using PetAmparo.Domain.DTOs.Usuario;

namespace PetAmparo.Domain.DTOs.Publicacao
{
    public class PublicacaoObterDto
    {
        public Guid Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public DateTime Data { get; set; }

        public string Foto { get; set; } = string.Empty;

        public bool Like { get; set; }

        public int QuantidadeLike { get; set; }

        public List<Guid> LikeUsuario { get; set; } = [];

        public UsuarioObterDto Usuario { get; set; }
        
    }
}
