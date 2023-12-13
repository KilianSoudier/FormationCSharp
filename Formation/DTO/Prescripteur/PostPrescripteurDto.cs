using Formation.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Formation.DTO.Prescripteur
{
    public class PostPrescripteurDto
    {
        public int Id { get; set; }
        public string Denomination { get; set; }
        public string? Adresse { get; set; }
    }
}
