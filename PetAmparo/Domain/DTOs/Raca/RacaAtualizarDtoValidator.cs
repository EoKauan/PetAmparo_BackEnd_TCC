using FluentValidation;

namespace PetAmparo.Domain.DTOs.Raca
{
    public class RacaAtualizarDtoValidator : AbstractValidator<RacaAtualizarDto>
    {
        public RacaAtualizarDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Raça não existe");

            RuleFor(p => p.Descricao)
                .NotEmpty().WithMessage("O campo Descrição deve ser preenchido!")
                .MaximumLength(100).WithMessage("O campo descrição deve possuir no máximo 100 caracteres");

            RuleFor(p => p.EspecieId)
                .NotEmpty().WithMessage("O campo Espécie deve ser preenchido!");
        }
    }
}
