using FluentValidation;

namespace PetAmparo.Domain.DTOs.Usuario
{
    public class UsuarioAdicionarDtoValidator : AbstractValidator<UsuarioAdicionarDto>
    {
        public UsuarioAdicionarDtoValidator()
        {

            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O campo nome deve ser preenchido!")
                .MaximumLength(100).WithMessage("O campo nome deve possuir no máximo 100 caracteres");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("O campo email deve ser preenchido!")
                .MaximumLength(150).WithMessage("O campo email deve possuir no máximo 150 caracteres");

            RuleFor(p => p.Senha)
                .NotEmpty().WithMessage("O campo senha deve ser preenchido!")
                .MaximumLength(100).WithMessage("O campo deve possuir no máximo 100 caracteres");

            RuleFor(p => p.ConfirmarSenha)
                .NotEmpty().WithMessage("O campo de confirmar senha deve ser preenchido!")
                .Equal(p => p.Senha).WithMessage("As senha não coincidem!")
                .MaximumLength(100).WithMessage("O campo confirma senha deve possuir no máximo 100 caracteres");

            RuleFor(p => p.Telefone)
                .NotEmpty().WithMessage("O campo telefone deve ser preenchido!")
                .MaximumLength(20).WithMessage("O campo telefone deve possuir no máximo 20 caracteres");

            RuleFor(p => p.Municipio)
                .NotEmpty().WithMessage("O campo município deve ser preenchido!")
                .MaximumLength(50).WithMessage("O campo município deve possuir no máximo 50 caracteres");
            
            RuleFor(p => p.Bio)
                .MaximumLength(500).WithMessage("O campo bio deve possuir no máximo 500 caracteres");
        }

    }
}
