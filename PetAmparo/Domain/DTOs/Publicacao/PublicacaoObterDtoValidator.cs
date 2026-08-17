using FluentValidation;

namespace PetAmparo.Domain.DTOs.Publicacao
{
    public class PublicacaoObterDtoValidator : AbstractValidator<PublicacaoObterDto>
    {
        public PublicacaoObterDtoValidator()
        {
            RuleFor(p => p.Id)
               .NotEmpty().WithMessage("Usuário não existe");
        }
    }
}
