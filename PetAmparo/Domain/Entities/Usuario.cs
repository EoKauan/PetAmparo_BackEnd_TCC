using PetAmparo.Domain.Enumerators;

namespace PetAmparo.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        public string Municipio { get; set; } = string.Empty;

        public string Foto { get; set; } = string.Empty;

        public string? Bio { get; set; }

        public bool Administrador { get; set; }

        public Guid? ChaveResetSenha { get; set; }
    }
}
