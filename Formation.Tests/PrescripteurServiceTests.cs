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
                .UseInMemoryDatabase("db").Options;//prends une bdd qui sera stock�e dans la ram.
            ctx = new Data.ApplicationDbContext(options);
            var logger = Mock.Of<ILogger<PrescripteurService>>();//Cr�� un faux ILogger. Pas besoin de faire la m�thode qu'il ex�cute car cette m�thode renvoie un void
            var logger2 = new Mock<ILogger<PrescripteurService>>();//Meme chose que logger sauf que la on cr�� une fausse m�thode qui sera appel� dans la m�thode getall
            logger2.Setup(log => log.GetHashCode()).Returns(0);//Dans ce cas le mock est configur� pour pouvoir retourner 0 lorsqu'on appellera la m�thode GetHashCode
            svc = new PrescripteurService(ctx, logger);
        }

        [Fact(DisplayName = "GetAllAsync returns all entities")]//Le nom doit sp�cifier le nom de la m�thode test� ainsi que le r�sultat retourn�
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