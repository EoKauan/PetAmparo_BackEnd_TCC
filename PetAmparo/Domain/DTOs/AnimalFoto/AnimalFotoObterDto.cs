namespace PetAmparo.Domain.DTOs.AnimalFoto
{
    public class AnimalFotoObterDto
    {
        public Guid Id { get; set; }

        public Guid AnimalId { get; set; }

        public string Foto { get; set; }

        public AnimalFotoObterDto Animal { get; set; }
    }
}
