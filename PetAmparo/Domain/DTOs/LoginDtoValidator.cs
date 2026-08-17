using FluentValidation;

namespace PetAmparo.Domain.DTOs
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("O email precisa ser preenchido!");

            RuleFor(p => p.Senha)
                .NotEmpty().WithMessage("A senha precisa ser preenchida");
        }
    }
}
