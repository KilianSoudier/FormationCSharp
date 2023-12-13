using System.ComponentModel.DataAnnotations;

namespace Formation.Data.Entities
{
    public class Laboratoire
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Nom { get; set; }
        [StringLength(200, ErrorMessage = "Veuillez indiquer une adresse avec 200 charactères maximum")]
        public string? Adresse { get; set; }
        public List<EchantillonEntity> echantillons { get; set; } = new List<EchantillonEntity>();

    }
}
