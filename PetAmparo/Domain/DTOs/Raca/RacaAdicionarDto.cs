namespace PetAmparo.Domain.DTOs.Raca
{
    public class RacaAdicionarDto
    {
        public string Descricao { get; set; } = string.Empty;

        public Guid EspecieId { get; set; }
    }
}
