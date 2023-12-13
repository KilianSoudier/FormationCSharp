
using Formation.Data;
using Formation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Formation.Services
{
    public class BackgroundJob : BackgroundService
    {
        private ApplicationDbContext? dbContext;
        private readonly IServiceProvider _serviceProvider;
        public BackgroundJob(IServiceProvider provider) {
            _serviceProvider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IEnumerable<Prescripteur> denominationPre;  //On doit faire une variable en dehors du using, le context existe uniquement dans le using.
            using (var scope = _serviceProvider.CreateScope())
            {
                dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                denominationPre = await dbContext.Prescripteurs.ToListAsync();
                //Requete et récupération des données
            }; //Créé un scope pour aller chercher le dbcontext et le ferme à la fin du using
            while(!stoppingToken.IsCancellationRequested)    //Jeton d'annulation équivalent a un while true
            {
                Console.WriteLine(denominationPre);
                await Task.Delay(1000);
            }

        }
    }
}
