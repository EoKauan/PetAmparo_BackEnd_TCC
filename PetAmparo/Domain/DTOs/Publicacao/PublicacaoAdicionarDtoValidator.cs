using FluentValidation;

namespace PetAmparo.Domain.DTOs.Publicacao
{
    public class PublicacaoAdicionarDtoValidator : AbstractValidator<PublicacaoAdicionarDto>
    {
        public PublicacaoAdicionarDtoValidator()
        {
            RuleFor(p => p.Titulo)
                .NotEmpty().WithMessage("O campo título deve ser preenchido!")
                .MaximumLength(50).WithMessage("O campo título deve possuir no máximo 100 caracteres");
        }
    }
}
