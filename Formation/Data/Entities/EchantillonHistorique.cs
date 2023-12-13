using System.ComponentModel.DataAnnotations;
using static Formation.Common.Enumerations;

namespace Formation.Data.Entities
{
    public class EchantillonHistorique
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public EchantillonEntity Echantillon { get; set; }
        public int EchantillonId { get; set; }
        public StatutEchantillon Statut { get; set; }
        public string? Observations { get; set; }
        public Laboratoire Laboratoire { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
