using FluentValidation;

namespace PetAmparo.Domain.DTOs.Raca
{
    public class RacaObterDtoValidator : AbstractValidator<RacaObterDto>
    {
        public RacaObterDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Usuário não existe");
        }
    }
}
