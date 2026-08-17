using PetAmparo.Domain.DTOs.Especie;

namespace PetAmparo.Domain.DTOs.Raca
{
    public class RacaObterDto
    {
        public Guid Id { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public Guid EspecieId { get; set; }

        public EspecieObterDto? Especie { get; set; }
    }
}
