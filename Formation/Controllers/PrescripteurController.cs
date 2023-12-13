using Formation.Data.Entities;
using Formation.DTO.Prescripteur;
using Formation.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Formation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PrescripteurController : ControllerBase
    {
        public IPrescripteurService _svc;
        public ILogger<PrescripteurController> _logger;

        public PrescripteurController(IPrescripteurService svc, ILogger<PrescripteurController> logger)
        {
            _svc = svc;     //Appelle l'interface qui depuis le program.cs va créer une classe EchantillonService
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetPrescripteurDto>>> GetAll()
        {
            //try
            //{
            //    var bug = await _svc.buggingPrescripteur();
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, ex.Message);
            //    return Problem(detail: ex.Message, statusCode:StatusCodes.Status502BadGateway, title:"OSKOUUUUUR"); //Renvoie une exception rfc : objet standard qui récupère plusieurs infos sur l'exception
            //}
            var data = await _svc.buggingPrescripteur();
            return Ok(await _svc.GetAllAsync());
        }

        [HttpGet("{id:int}")]   //id:int ici car si c'est un string il le repèrera plus tôt donc plus optimisé
        public async Task<GetPrescripteurDto> GetPrescById(int id)
        {
            return await _svc.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<IEnumerable<GetPrescripteurDto>> post(PostPrescripteurDto p)
        {
            await _svc.PostAsync(p);
            return await _svc.GetAllAsync();
        }

        [HttpPut]
        public async Task<IEnumerable<GetPrescripteurDto>> put(PostPrescripteurDto p)
        {
            await _svc.PutAsync(p);
            return await _svc.GetAllAsync(); ;
        }
    }
}
