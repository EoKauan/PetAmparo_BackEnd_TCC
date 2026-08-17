using FluentValidation;

namespace PetAmparo.Domain.DTOs.Usuario
{
    public class UsuarioListarDtoValidator : AbstractValidator<UsuarioListarDto>
    {
        public UsuarioListarDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Usuário nâo existe");

            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O campo nome deve ser preenchido!")
                .MaximumLength(100).WithMessage("O campo nome deve possuir no máximo 100 caracteres");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("O campo email deve ser preenchido!")
                .MaximumLength(150).WithMessage("O campo email deve possuir no máximo 100 caracteres");

            RuleFor(p => p.Telefone)
                .NotEmpty().WithMessage("O campo telefone deve ser preenchido!")
                .MaximumLength(20).WithMessage("O campo telefone deve possuir no máximo 100 caracteres");

            RuleFor(p => p.Municipio)
                .NotEmpty().WithMessage("O campo município deve ser preenchido!")
                .MaximumLength(50).WithMessage("O campo município deve possuir no máximo 100 caracteres");

            RuleFor(p => p.Foto)
                .NotEmpty().WithMessage("O campo foto deve ser preenchido!");
        }
    }
}
