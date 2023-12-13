using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Formation.Common.Enumerations;

namespace Formation.Data.Entities
{
    public class EchantillonEntity
    {
        //Pas besoin de [Key] dans les cas suivants : ID id Id iD echantillonEntityid
        public int Id { get; set; }
        [StringLength(50)]
        public string Nom { get; set; }
        public DateTime DateDeCollecte { get; set; } = DateTime.UtcNow;
        public TypeEchantillon Type { get; set; }
        //public StatutEchantillon Statut { get; set; }
        public Prescripteur Prescripteur { get; set; }
        public int PrescripteurId { get; set; }
        public Laboratoire? Laboratoire { get; set; }
        public int? LaboratoireId { get; set; }
        public List<EchantillonHistorique> echantillonHistoriques { get; set; } = new List<EchantillonHistorique>();
        
    }
}
