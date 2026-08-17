namespace PetAmparo.Domain.DTOs.Base;

public class BaseResponse(string mensagem)
{
    public string Message { get; } = mensagem;
}