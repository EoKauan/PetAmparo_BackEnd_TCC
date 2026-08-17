using FluentValidation;

namespace PetAmparo.Domain.DTOs.AnimalFoto
{
    public class AnimalFotoListarDtoValidator : AbstractValidator<AnimalFotoListarDto>
    {
        public AnimalFotoListarDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Usuário não existe");

            RuleFor(p => p.AnimalId)
                .NotEmpty().WithMessage("O campo AnimalId deve ser preenchido!");

            RuleFor(p => p.Foto)
                .NotEmpty().WithMessage("Uma foto deve ser adicionada!");
        }
    }
}
