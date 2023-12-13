using Formation.Data;
using Formation.Data.Entities;
using Formation.DTO.Prescripteur;
using Microsoft.EntityFrameworkCore;
using static Formation.Common.Enumerations;

namespace Formation.Services
{
    public interface IPrescripteurService
    {
        Task<GetPrescripteurDto> buggingPrescripteur();
        Task<IEnumerable<GetPrescripteurDto>> GetAllAsync();
        Task<GetPrescripteurDto?> GetByIdAsync(int id);
        Task PostAsync(PostPrescripteurDto p);
        Task PutAsync(PostPrescripteurDto p);
    }
    public class PrescripteurService : IPrescripteurService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<PrescripteurService> logger;

        public PrescripteurService(ApplicationDbContext dbContext, ILogger<PrescripteurService> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<GetPrescripteurDto> buggingPrescripteur()
        {
            await Task.Delay(1500);
            throw new ApplicationException("Ca bugue ici");
        }

        public async Task<IEnumerable<GetPrescripteurDto>> GetAllAsync()    
        {
            var data = await dbContext.Prescripteurs
                .AsNoTracking() //AsNoTracking Evite de tracker les modifications.
                .ToListAsync();
            var result = data.Select(p => new GetPrescripteurDto
            {
                Id = p.Id,
                Denomination= p.Denomination,
                Adresse= p.Adresse
            });
            logger.LogInformation("GettAllEntities");
            return result;
        }

        public async Task<GetPrescripteurDto?> GetByIdAsync(int id)
        {
            var data = await dbContext.Prescripteurs
                .AsNoTracking() //AsNoTracking Evite de tracker les modifications.
                .FirstOrDefaultAsync(pre => pre.Id == id);
            GetPrescripteurDto result = default;
            if (data != null)
            {
                result = new GetPrescripteurDto
                {
                    Id = data.Id,
                    Denomination = data.Denomination,
                    Adresse = data.Adresse
                };
            }
            return result;
        }

        public async Task PostAsync(PostPrescripteurDto p)
        {
            Prescripteur pre = new Prescripteur()
            {
                Adresse=p.Adresse,
                Denomination = p.Denomination
            };
            await dbContext.Prescripteurs.AddAsync(pre);
            await dbContext.SaveChangesAsync();
        }

        public async Task PutAsync(PostPrescripteurDto p)
        {
            Prescripteur? pre = await dbContext.Prescripteurs.FirstOrDefaultAsync(s=>s.Id == p.Id);
            if (pre != null)
            {
                pre.Denomination = p.Denomination;
                pre.Adresse = p.Adresse;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
