using System.ComponentModel.DataAnnotations;

namespace Formation.Data.Entities
{
    public class Prescripteur
    {
        public int Id { get; set; }
        [StringLength(200)]// == [MinLength(0), MaxLength(200)]
        public string Denomination { get; set; }
        [StringLength(200)]
        public string? Adresse { get; set; }
        public List<EchantillonEntity> Echantillons { get; set; } = new List<EchantillonEntity>();
    }
}
