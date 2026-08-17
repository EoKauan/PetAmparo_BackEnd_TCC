using FluentValidation;

namespace PetAmparo.Domain.DTOs.Raca
{
    public class RacaListarDtoValidator : AbstractValidator<RacaListarDto>
    {
        public RacaListarDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Usuário não existe");

            RuleFor(p => p.Descricao)
                .NotEmpty().WithMessage("O campo Descrição deve ser preenchido!");
        }
    }
}
