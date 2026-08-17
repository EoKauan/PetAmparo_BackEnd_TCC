using FluentValidation;

namespace PetAmparo.Domain.DTOs.Especie
{
    public class EspecieAdicionarDtoValidator : AbstractValidator<EspecieAdicionarDto>
    {
        public EspecieAdicionarDtoValidator()
        {
            RuleFor(p => p.Descricao)
                .NotEmpty().WithMessage("O campo descrição deve ser preenchido!")
                .MaximumLength(100).WithMessage("O campo descrição deve possuir no máximo 100 caracteres");
        }
    }
}

