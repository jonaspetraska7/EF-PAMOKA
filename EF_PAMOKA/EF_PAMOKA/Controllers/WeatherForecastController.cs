using EF_PAMOKA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EF_PAMOKA.Controllers
{
    [ApiController]
    [Route("/")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly PavyzdinisDbContext _dbContext;

        public WeatherForecastController(PavyzdinisDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("/daiktai")]
        public List<Daiktas> VisiDaiktai()
        {
            return _dbContext.Daiktai.Where(x => x.Pavadinimas != "kazkas").ToList();
        }

        
        [HttpGet]
        [Route("/automobiliai")]
        [Authorize]
        public List<Automobilis> VisiAutomobiliai()
        {
            return _dbContext.Automobiliai.Where(x => x.Marke != "kazkas").ToList();
        }

        [HttpGet]
        [Route("/daiktai/{id}")]
        public Daiktas? Daiktas(int? id)
        {
            return _dbContext.Daiktai.Where(x => x.Id == id).FirstOrDefault();
        }


        [HttpGet]
        [Route("/savininkai")]
        public List<Savininkas> VisiSavininkai()
        {
            return _dbContext.Savininkai.Where(x => x.Vardas != "kazkas").ToList();
        }

        [HttpGet]
        [Route("/savininkai/{id}")]
        public Savininkas? VienasSavininkas(int? id)
        {
            return _dbContext.Savininkai.Where(x => x.Id == id).FirstOrDefault();
        }


        [HttpGet]
        [Route("/pridetiDaikta/{savininkoId}")]
        public void PridetiDaikta(int? savininkoId)
        {
            var savininkas = _dbContext.Savininkai.Where(x => x.Id == savininkoId).FirstOrDefault();
            _dbContext.Daiktai.Add(new Daiktas() { Pavadinimas = "Telefonas", SavininkoId = savininkas != null ? savininkas.Id : 1 });
            _dbContext.SaveChanges();
            _dbContext.Daiktai.FromSqlRaw("drop table;");
        }

        [HttpGet]
        [Route("/pridetiSavininka")]
        public void PridetiSavininka()
        {
            _dbContext.Savininkai.Add(new Savininkas() { Vardas = "Jonas" });
            _dbContext.SaveChanges();
        }

        [HttpDelete]
        [Route("/daiktas/{daiktoId}")]
        public void IstrintiDaikta(int? daiktoId)
        {
            var daiktas = _dbContext.Daiktai.Where(x => x.Id == daiktoId).FirstOrDefault();
            if(daiktas != null)
            {
                _dbContext.Daiktai.Remove(daiktas);
                _dbContext.SaveChanges();
            }
        }

        [HttpDelete]
        [Route("/savininkas/{savininkoId}")]
        public void IstrintiSavininka(int? savininkoId)
        {
            var savininkas = _dbContext.Savininkai.Where(x => x.Id == savininkoId).FirstOrDefault();
            if (savininkas != null)
            {
                _dbContext.Savininkai.Remove(savininkas);
                _dbContext.SaveChanges();
            }
        }







    }
}