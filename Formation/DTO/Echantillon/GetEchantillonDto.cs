using System.Text.Json.Serialization;
using static Formation.Common.Enumerations;

public class GetEchantillonDto
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public DateTime DateDeCollecte { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TypeEchantillon Type { get; set; }
    public Dictionary<DateTime, StatutEchantillon> Historique { get; set; }
        = new Dictionary<DateTime, StatutEchantillon>();
}

