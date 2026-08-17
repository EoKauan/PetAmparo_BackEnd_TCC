namespace PetAmparo.Domain.DTOs.Usuario
{
    public class UsuarioAdicionarDto
    {
        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;

        public string ConfirmarSenha { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        public string Municipio { get; set; } = string.Empty;

        public string Foto { get; set; } = string.Empty;

        public string? Bio { get; set; }
    }
}
