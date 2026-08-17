namespace PetAmparo.Domain.DTOs.Publicacao
{
    public class PublicacaoAtualizarDto
    {
        public Guid Id { get; set; }

        public string? Titulo { get; set; }

        public Guid? UsuarioId { get; set; }

        public string? Descricao { get; set; }

        public DateTime? Data { get; set; }

        public string? Foto { get; set; }

        public bool? Like { get; set; }
    }
}
