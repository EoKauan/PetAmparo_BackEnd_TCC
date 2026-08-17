using PetAmparo.Domain.Enumerators;

namespace PetAmparo.Domain.Entities
{
    public class Animal
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public int Idade { get; set; }

        public string Observacao { get; set; } = string.Empty;

        public Guid EspecieId { get; set; }

        public Guid RacaId { get; set; }

        public Guid UsuarioId { get; set; }

        public EnumStatusAnimal Status { get; set; }

        public List<AnimalFoto> AnimalFoto { get; set; } = [];

        public Usuario Usuario { get; set; } = null!;

        public Especie Especie { get; set; } = null!;

        public Raca Raca { get; set; } = null!;
    }
}
