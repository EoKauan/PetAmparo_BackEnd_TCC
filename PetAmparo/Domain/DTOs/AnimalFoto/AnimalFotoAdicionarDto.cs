namespace PetAmparo.Domain.DTOs.AnimalFoto { 

    public class AnimalFotoAdicionarDto
    {

        public Guid Id { get; set; }

        public Guid AnimalId { get; set; }

        public string Foto { get; set; }


    }
}