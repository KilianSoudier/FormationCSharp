using Formation.Data;
using Formation.Data.Entities;
using Formation.DTO.Echantillon;
using Microsoft.EntityFrameworkCore;
using static Formation.Common.Enumerations;

namespace Formation.Services
{
    public interface IEchantillonService
    {
        Task<IEnumerable<GetEchantillonDto>> GetAllAsync();
        Task AddEchantillonAsync(PostEchantillonDto echantillon);
        Task PutEchantillonAsync(PostEchantillonDto echantillon);
    }

    public class EchantillonService : IEchantillonService
    {
        private readonly ApplicationDbContext dbContext;
        public EchantillonService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<GetEchantillonDto>> GetAllAsync()
        {
            var data =  await dbContext.Echantillons
                .Include(ech => ech.echantillonHistoriques)
                .AsNoTracking() //AsNoTracking Evite de tracker les modifications.
                .Where(e=>e.echantillonHistoriques.OrderBy(h=>h.Id).Last().Statut != StatutEchantillon.Detruit)
                .ToListAsync();
            var result = data.Select(d => new GetEchantillonDto
            {
                Id = d.Id,
                DateDeCollecte = d.DateDeCollecte,
                Nom = d.Nom,
                Historique = d.echantillonHistoriques.ToDictionary(
                    dbH => dbH.Date,
                    dbH => dbH.Statut                    
                    ),
                Type = d.Type,
            });
            return result;
        }
        public async Task AddEchantillonAsync(PostEchantillonDto echantillon)  //Task permet de signaler quand il a terminer (await) Void ne fait pas fonctionner le await
        {
            EchantillonEntity echantillonEntity = new EchantillonEntity()
            {
                Nom = echantillon.Nom,
                DateDeCollecte = echantillon.DateDeCollecte,
                Type = echantillon.Type
            };
            var histo = new EchantillonHistorique()
            {
                Date = DateTime.UtcNow,
                Statut = echantillon.Statut,
                Observations= echantillon.Observations
            };
            //var entry = dbContext.Echantillons.Add(echantillonEntity);    //Pas obligatoire, comme echantillon est dans l'historique il mets a jour les deux automatiquement
            dbContext.EchantillonHistoriques.Add(histo);

            await dbContext.SaveChangesAsync();
            //entry.Entity.Id;// récupère l'id de l'échantillon tout juste créé
        }

        public async Task PutEchantillonAsync(PostEchantillonDto echantillon)
        {
            EchantillonEntity? Echantillon = dbContext.Echantillons.FirstOrDefault(s => s.Id == echantillon.Id);
            if (Echantillon != null)
            {
                Echantillon.Type = echantillon.Type;
                Echantillon.DateDeCollecte = echantillon.DateDeCollecte;
                Echantillon.Nom = echantillon.Nom;

                var histo = new EchantillonHistorique()
                {
                    Date= DateTime.UtcNow,
                    EchantillonId=Echantillon.Id,
                    Statut = echantillon.Statut,
                    Observations = echantillon.Observations
                };
                dbContext.EchantillonHistoriques.Add(histo);
                
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
