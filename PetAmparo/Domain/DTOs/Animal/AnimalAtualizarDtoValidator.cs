using FluentValidation;

namespace PetAmparo.Domain.DTOs.Animal
{
    public class AnimalAtualizarDtoValidator : AbstractValidator<AnimalAtualizarDto>
    {
        public AnimalAtualizarDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Usuário não existe");

            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O campo nome deve ser preenchido!")
                .MaximumLength(100).WithMessage("O campo nome deve possuir no máximo 100 caracteres");

            RuleFor(p => p.EspecieId)
                .NotEmpty().WithMessage("O campo espécie deve ser preenchido!");

            RuleFor(p => p.RacaId)
                .NotEmpty().WithMessage("O campo raça deve ser preenchido!");

            RuleFor(p => p.Idade)
                .NotEmpty().WithMessage("O campo idade deve ser preenchido!");

            RuleFor(p => p.Observacao)
                .NotEmpty().WithMessage("O campo observação deve ser preenchido!")
                .MaximumLength(200).WithMessage("O campo observação deve possuir no máximo 100 caracteres");

            RuleFor(p => p.Status)
                .NotEmpty().WithMessage("O campo status deve ser preenchido!");

            RuleFor(p => p.Fotos)
                .NotEmpty().WithMessage("Pelo menos uma foto deve ser adicionada!")
                .Must(fotos => fotos != null && fotos.Count >= 1 && fotos.Count <= 5)
                .WithMessage("O animal deve ter entre 1 e 5 fotos!")
                .ForEach(foto => foto.NotEmpty().WithMessage("Cada foto deve ser preenchida!"));
        }
    }
}
