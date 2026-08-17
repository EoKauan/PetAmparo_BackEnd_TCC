using PetAmparo.Domain.Enumerators;

namespace PetAmparo.Domain.DTOs.Animal
{
    public class AnimalAdicionarDto
    {
        public string Nome { get; set; } = string.Empty;

        public int Idade { get; set; }

        public Guid EspecieId { get; set; }

        public Guid RacaId { get; set; }

        public string Observacao { get; set; } = string.Empty;

        public List<string> Fotos { get; set; } = [];
    }
}
