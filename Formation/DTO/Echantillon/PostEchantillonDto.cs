using static Formation.Common.Enumerations;
using System.Text.Json.Serialization;

namespace Formation.DTO.Echantillon
{
    public class PostEchantillonDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public DateTime DateDeCollecte { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TypeEchantillon Type { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatutEchantillon Statut { get; set; }
        public string? Observations { get; set; }
    }
}
