namespace PetAmparo.Domain.DTOs.AnimalFoto
{
    public class AnimalFotoAtualizarDto
    {
        public Guid Id { get; set; }

        public Guid AnimalId { get; set; }

        public string Foto { get; set; }
    }
}
