namespace PetAmparo.Domain.Entities
{
    public class Raca
    {
        public Guid Id { get; set; }
        
        public string Descricao { get; set; } = string.Empty;

        public Guid EspecieId { get; set; }

        public Especie Especie { get; set; } = null!;
    }
}
