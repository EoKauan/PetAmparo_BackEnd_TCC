using PetAmparo.Domain.Enumerators;

namespace PetAmparo.Domain.DTOs.Animal
{
    public class AnimalAtualizarDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public Guid EspecieId { get; set; }

        public Guid RacaId { get; set; }

        public int Idade { get; set; }

        public string Observacao { get; set; } = string.Empty;

        public EnumStatusAnimal Status { get; set; }

        public List<string> Fotos { get; set; } = [];
    }
}
