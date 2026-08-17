using FluentValidation;

namespace PetAmparo.Domain.DTOs.Animal
{
    public class AnimalListarDtoValidator : AbstractValidator<AnimalListarDto>
    {
        public AnimalListarDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Usuário não existe");

            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O campo nome deve ser preenchido!")
                .MaximumLength(100).WithMessage("O campo nome deve possuir no máximo 100 caracteres");

            RuleFor(p => p.RacaId)
                .NotEmpty().WithMessage("O campo raça deve ser preenchido!");

            RuleFor(p => p.Idade)
                .NotEmpty().WithMessage("O campo de idade deve ser preenchido!");

            RuleFor(p => p.Observacao)
                .NotEmpty().WithMessage("O campo observação deve ser preenchido!")
                .MaximumLength(200).WithMessage("O campo observação deve possuir no máximo 100 caracteres");

            RuleFor(p => p.Status)
                .NotEmpty().WithMessage("O campo status deve ser preenchido!");

            RuleFor(p => p.UsuarioId)
                .NotEmpty().WithMessage("Usuário(dono) não encontrado");
        }
    }
}
