using Formation.DTO.Echantillon;
using Formation.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Numerics;

namespace Formation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EchantillonController : ControllerBase
    {
        public IEchantillonService _svc;
        private readonly ILogger<EchantillonController> _logger;

        public EchantillonController(IEchantillonService svc, ILogger<EchantillonController> logger)
        {
            _svc = svc;     //Appelle l'interface qui depuis le program.cs va créer une classe EchantillonService
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetEchantillonDto>>> GetEchantillon()    //IEnumerable moins gourmand en ressource que les lists mais pas modifiable
        {
            var echantillons = await _svc.GetAllAsync();
            _logger.LogInformation($"chargement de { echantillons?.Count() ?? 0 } échantillons.");// le premier ? ne fera pas le .Count si il est null et le ?? renverra 0 si il est null
            
            if(echantillons is null)    //Plus rapide que == null
            {
                return NotFound();
            }
            return Ok(echantillons);
        }

        [HttpPost]
        public async Task<IEnumerable<GetEchantillonDto>> PostEchantillon(PostEchantillonDto echantillon)
        {
            await _svc.AddEchantillonAsync(echantillon);
            return await _svc.GetAllAsync();
        }

        [HttpPut]
        public async Task<IEnumerable<GetEchantillonDto>> PutEchantillon(PostEchantillonDto echantillon)
        {
            await _svc.PutEchantillonAsync(echantillon);
            return await _svc.GetAllAsync();
        }
    }
}
