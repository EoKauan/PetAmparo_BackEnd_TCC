using FluentValidation;

namespace PetAmparo.Domain.DTOs.Publicacao
{
    public class PublicacaoAtualizarDtoValidator : AbstractValidator<PublicacaoAtualizarDto>
    {
        public PublicacaoAtualizarDtoValidator()
        {
            RuleFor(p => p.Id)
               .NotEmpty().WithMessage("Publicação não existe");

            // Validação condicional: apenas valida se o campo foi informado
            When(p => !string.IsNullOrEmpty(p.Titulo), () =>
            {
                RuleFor(p => p.Titulo)
                    .MaximumLength(50).WithMessage("O campo título deve possuir no máximo 50 caracteres");
            });

            When(p => p.UsuarioId.HasValue, () =>
            {
                RuleFor(p => p.UsuarioId)
                    .NotEmpty().WithMessage("Usuário não encontrado!");
            });

            When(p => !string.IsNullOrEmpty(p.Descricao), () =>
            {
                RuleFor(p => p.Descricao)
                    .MaximumLength(150).WithMessage("O campo descrição deve possuir no máximo 150 caracteres");
            });
        }
    }
}
