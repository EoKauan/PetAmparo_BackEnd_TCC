using FluentValidation;

namespace PetAmparo.Domain.DTOs.Usuario
{
    public class UsuarioAtualizarDtoValidator : AbstractValidator<UsuarioAtualizarDto>
    {
        public UsuarioAtualizarDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Usuário não existe");

            // Validação condicional: apenas valida se o campo foi informado
            When(p => !string.IsNullOrEmpty(p.Nome), () =>
            {
                RuleFor(p => p.Nome)
                    .MaximumLength(100).WithMessage("O campo nome deve possuir no máximo 100 caracteres");
            });

            When(p => !string.IsNullOrEmpty(p.Email), () =>
            {
                RuleFor(p => p.Email)
                    .MaximumLength(150).WithMessage("O campo email deve possuir no máximo 150 caracteres");
            });

            // Validação condicional para senha: se informada, deve ser validada
            When(p => !string.IsNullOrEmpty(p.Senha), () =>
            {
                RuleFor(p => p.Senha)
                    .MaximumLength(100).WithMessage("O campo senha deve possuir no máximo 100 caracteres");

                RuleFor(p => p.ConfirmaSenha)
                    .NotEmpty().WithMessage("O campo de confirmar senha deve ser preenchido quando a senha for informada!")
                    .Equal(p => p.Senha).WithMessage("As senhas não coincidem!")
                    .MaximumLength(100).WithMessage("O campo confirma senha deve possuir no máximo 100 caracteres");
            });

            When(p => !string.IsNullOrEmpty(p.Telefone), () =>
            {
                RuleFor(p => p.Telefone)
                    .MaximumLength(20).WithMessage("O campo telefone deve possuir no máximo 20 caracteres");
            });

            When(p => !string.IsNullOrEmpty(p.Municipio), () =>
            {
                RuleFor(p => p.Municipio)
                    .MaximumLength(50).WithMessage("O campo município deve possuir no máximo 50 caracteres");
            });

            When(p => !string.IsNullOrEmpty(p.Bio), () =>
            {
                RuleFor(p => p.Bio)
                    .MaximumLength(500).WithMessage("O campo bio deve possuir no máximo 500 caracteres");
            });
        }
    }
}
