using FluentValidation;

namespace PetAmparo.Domain.DTOs.Publicacao
{
    public class PublicacaoListarDtoValidator : AbstractValidator<PublicacaoListarDto>
    {
        public PublicacaoListarDtoValidator()
        {
            RuleFor(p => p.Id)
               .NotEmpty().WithMessage("Usuário nâo existe");

            RuleFor(p => p.Titulo)
                .NotEmpty().WithMessage("O campo título deve ser preenchido!")
                .MaximumLength(50).WithMessage("O campo título deve possuir no máximo 100 caracteres");

            RuleFor(p => p.UsuarioId)
                .NotEmpty().WithMessage("Usuário não encontrado!");

            RuleFor(p => p.Descricao)
                .NotEmpty().WithMessage("O campo Descrição deve ser preenchido!")
                .MaximumLength(150).WithMessage("O campo deve possuir no máximo 100 caracteres");

            RuleFor(p => p.Data)
                .NotEmpty().WithMessage("O campo data deve ser preenchido!");
        }
    }
}
