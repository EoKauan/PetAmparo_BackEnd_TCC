using FluentValidation;


namespace PetAmparo.Domain.DTOs.AnimalFoto
{ 
    public class AnimalFotoAdicionarDtoValidator : AbstractValidator<AnimalFotoAdicionarDto>
    {

        public AnimalFotoAdicionarDtoValidator()
        {

            RuleFor(p => p.AnimalId)
                .NotEmpty().WithMessage("O campo AnimalId deve ser preenchido!");

            RuleFor(p => p.Foto)
                .NotEmpty().WithMessage("Uma foto deve ser adicionada!");
        }
    }
}
