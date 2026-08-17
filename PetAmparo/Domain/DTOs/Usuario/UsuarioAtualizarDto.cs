namespace PetAmparo.Domain.DTOs.Usuario
{
    public class UsuarioAtualizarDto
    {
        public Guid Id { get; set; }

        public string? Nome { get; set; }

        public string? Email { get; set; }

        public string? Senha { get; set; }

        public string? ConfirmaSenha { get; set; }

        public string? Telefone { get; set; }

        public string? Municipio { get; set; }

        public string? Foto { get; set; }

        public string? Bio { get; set; }

        public bool? Administrador { get; set; }
    }
}
