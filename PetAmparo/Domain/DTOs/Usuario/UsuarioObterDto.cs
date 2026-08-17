namespace PetAmparo.Domain.DTOs.Usuario
{
    public class UsuarioObterDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        public string Municipio { get; set; } = string.Empty;

        public string Foto { get; set; } = string.Empty;

        public string? Bio { get; set; }

        public bool Administrador { get; set; }

    }
}
