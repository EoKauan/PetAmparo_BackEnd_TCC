using FluentValidation;

namespace PetAmparo.Domain.DTOs.Animal
{
    public class AnimalObterDtoValidator : AbstractValidator<AnimalObterDto>
    {
        public AnimalObterDtoValidator()
        {
            RuleFor(p => p.Id)
              .NotEmpty().WithMessage("Usuário não existe");
        }
    }
}
