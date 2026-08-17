namespace PetAmparo.Domain.DTOs.Raca
{
    public class RacaAtualizarDto
    {
        public Guid Id { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public Guid EspecieId { get; set; }
    }
}
