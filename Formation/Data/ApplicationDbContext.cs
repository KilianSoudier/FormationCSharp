using Formation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using static Formation.Common.Enumerations;

namespace Formation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<EchantillonHistorique>()
            //    .HasKey(eh => new { eh.Id, eh.Date }); //Créé une clé composée par l'id et la date.

            //modelBuilder.Entity<EchantillonHistorique>()
            //    .Property(eh => eh.Statut)
            //    .HasColumnName("statut_echantillon");   //Renomme le champ par un nom custom.

            modelBuilder.Entity<EchantillonHistorique>()
                .Property(eh => eh.Statut)
                .HasConversion(
                    statut => statut.ToString(),      //Conversion vers la bdd en string
                    str => Enum.Parse<StatutEchantillon>(str)   //Renvoie vers le C# en StatutEchantillon
                );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<EchantillonEntity> Echantillons { get; set; }
        public DbSet<Laboratoire> Laboratoires { get; set; }
        public DbSet<Prescripteur> Prescripteurs { get; set; }
        public DbSet<EchantillonHistorique> EchantillonHistoriques { get; set; }
    }
}
