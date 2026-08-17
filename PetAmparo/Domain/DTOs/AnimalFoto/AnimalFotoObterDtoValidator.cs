using FluentValidation;

namespace PetAmparo.Domain.DTOs.AnimalFoto
{
    public class AnimalFotoObterDtoValidator : AbstractValidator<AnimalFotoAdicionarDto>
    {
        public AnimalFotoObterDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Usuário não existe");
        }
    }
}
