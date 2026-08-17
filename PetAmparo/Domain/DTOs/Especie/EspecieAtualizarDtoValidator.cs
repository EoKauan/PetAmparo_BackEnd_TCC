using FluentValidation;

namespace PetAmparo.Domain.DTOs.Especie
{
    public class EspecieAtualizarDtoValidator : AbstractValidator<EspecieAtualizarDto>
    {
        public EspecieAtualizarDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Espécie não existe");

            RuleFor(p => p.Descricao)
                .NotEmpty().WithMessage("O campo descrição deve ser preenchido!")
                .MaximumLength(100).WithMessage("O campo descrição deve possuir no máximo 100 caracteres");
        }
    }
}

