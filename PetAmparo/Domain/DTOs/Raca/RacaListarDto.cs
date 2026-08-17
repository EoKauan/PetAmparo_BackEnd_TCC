using PetAmparo.Domain.DTOs.Especie;

namespace PetAmparo.Domain.DTOs.Raca
{
    public class RacaListarDto
    {
        public Guid Id { get; set; }

        public string Descricao { get; set; } = string.Empty;
        public EspecieListarDto Especie { get; set; } = null!;
    }
}
