namespace PetAmparo.Domain.Entities
{
    public class AnimalFoto
    {
        public Guid Id { get; set; }

        public Guid AnimalId { get; set; }

        public string Foto { get; set; } = string.Empty;

        public Animal Animal { get; set; } = null!;
    }
}
