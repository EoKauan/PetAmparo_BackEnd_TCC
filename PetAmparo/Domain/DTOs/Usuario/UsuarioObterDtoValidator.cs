using FluentValidation;

namespace PetAmparo.Domain.DTOs.Usuario
{
    public class UsuarioObterDtoValidator : AbstractValidator<UsuarioObterDto>
    {
        public UsuarioObterDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Usuário nâo existe");
        }
    }
}
