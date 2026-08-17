using PetAmparo.Domain.DTOs.Especie;
using PetAmparo.Domain.DTOs.Raca;
using PetAmparo.Domain.DTOs.Usuario;
using PetAmparo.Domain.Enumerators;

namespace PetAmparo.Domain.DTOs.Animal
{
    public class AnimalListarDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public int Idade { get; set; }

        public string Observacao { get; set; } = string.Empty;

        public EnumStatusAnimal Status { get; set; }

        public Guid EspecieId { get; set; } = Guid.Empty;
        public EspecieListarDto? Especie { get; set; }

        public Guid UsuarioId { get; set; } = Guid.Empty;

        public UsuarioListarDto? Usuario { get; set; }

        public Guid RacaId { get; set; } = Guid.Empty;

        public RacaListarDto? Raca { get; set; }

        public List<string> Fotos { get; set; } = [];
    }
}
