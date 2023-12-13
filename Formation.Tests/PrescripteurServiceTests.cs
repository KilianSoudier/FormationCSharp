using Castle.Core.Logging;
using Formation.Data;
using Formation.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics;

namespace Formation.Tests
{
    public class PrescripteurServiceTests
    {
        private readonly IPrescripteurService svc;
        private readonly ApplicationDbContext ctx;
        public PrescripteurServiceTests()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("db").Options;//prends une bdd qui sera stockée dans la ram.
            ctx = new Data.ApplicationDbContext(options);
            var logger = Mock.Of<ILogger<PrescripteurService>>();//Créé un faux ILogger. Pas besoin de faire la méthode qu'il exécute car cette méthode renvoie un void
            var logger2 = new Mock<ILogger<PrescripteurService>>();//Meme chose que logger sauf que la on créé une fausse méthode qui sera appelé dans la méthode getall
            logger2.Setup(log => log.GetHashCode()).Returns(0);//Dans ce cas le mock est configuré pour pouvoir retourner 0 lorsqu'on appellera la méthode GetHashCode
            svc = new PrescripteurService(ctx, logger);
        }

        [Fact(DisplayName = "GetAllAsync returns all entities")]//Le nom doit spécifier le nom de la méthode testé ainsi que le résultat retourné
        public async Task Test1()
        {
            var entity = new Data.Entities.Prescripteur()
            {
                Id = 1,
                Adresse = "1",
                Denomination = "1",
            };
            //Arrange
            ctx.Prescripteurs.Add(entity);
            ctx.SaveChanges();
            //Act
            var result = await svc.GetAllAsync();

            //Assert
            Assert.Single(result);
            Assert.Equal(entity.Id, result.First().Id);
            Assert.Equal(entity.Adresse, result.First().Adresse);
            Assert.Equal(entity.Denomination, result.First().Denomination);
        }
    }
}