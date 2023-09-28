using System;
using System.Linq;
using Models;
using System.Web.Helpers;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Services {
    public interface IExperienceService {
        public bool addExperience(String link, String marka, int godiste, String opis, String username);
        public bool experienceExist(String link);
        public bool experienceExist(String marka, int godiste);
        public ActionResult getExperiences(String link);
        public bool checkExperience(String link, String username);
        public ActionResult getExperiencesOfUser(String username);
        public ActionResult getExperiencseByMadeAndYear(String marka, int godiste);
        public Task<ActionResult> GetSomeExperiences();
        public bool deleteExperience(int id);
    }
    public class ExperienceService : IExperienceService {
        public DataContext Context { get; set; }
        
        private IUsersService usersService;

        public ExperienceService(
            DataContext context,
            IUsersService usersService

        ) {
            Context = context;
            this.usersService = usersService;

        }
        
        #region GetExps
        public async Task<ActionResult> GetSomeExperiences() {

            var tabela = Context.Iskustva
                .Include(p => p.Vozilo);
            return new OkObjectResult( await tabela.OrderByDescending(p => p.ID).Where(p => p.Arhivirano == false).Take(4).Select(p => new {p.Vozilo.Link, p.Vozilo.Marka, p.Vozilo.Godiste, p.Opis, p.Korisnik.Username }).ToListAsync());
        }
        public ActionResult getExperiences(String link) {
            
            var experiences = Context.Iskustva.Where(p => p.Vozilo.Link == link).Where(p => p.Arhivirano == false).Select(p => new {p.Vozilo.Marka, p.Vozilo.Godiste, p.Opis, p.Vozilo.Link, p.Korisnik.Username}).ToList();
            if(experiences != null) {
                 return new JsonResult(experiences);
            }
            return new BadRequestObjectResult("Nema iskustva za to vozilo");
            
        }
        public bool experienceExist(String link) {
            var experience = Context.Iskustva.Where(p => p.Vozilo.Link == link).FirstOrDefault();
            if(experience != null) {
                return true;
            } 
            return false;
        }
        public bool experienceExist(String marka, int godiste) {
            var experience = Context.Iskustva.Where(p => p.Vozilo.Marka == marka).Where(p => p.Vozilo.Godiste == godiste).FirstOrDefault();
            if(experience != null) {
                return true;
            } 
            return false;
        }
        public ActionResult getExperiencesOfUser(String username) {

            var user = Context.RegistrovaniKorisnici.Where(p => p.Username == username).First();
            
            var tabela = Context.Iskustva.Where(p => p.Korisnik.ID == user.ID).Where(p => p.Arhivirano == false).Select(p => new { p.ID, p.Vozilo.Link, p.Vozilo.Marka, p.Vozilo.Godiste, p.Opis }).ToList();
            
            return new JsonResult(tabela);
        }
        public ActionResult getExperiencseByMadeAndYear(String marka, int godiste) {
            
            var experiences = Context.Iskustva.Where(p => p.Vozilo.Marka == marka).Where(p => p.Arhivirano == false).Where(p => p.Vozilo.Godiste == godiste).Select(p => new { p.Vozilo.Link, p.Vozilo.Marka, p.Vozilo.Godiste, p.Opis, p.Korisnik.Username}).ToList();
            if(experiences != null) {
                return new JsonResult(experiences);
            }
            return new BadRequestObjectResult("Nema iskustva za takvo vozilo!");
        }
        public bool checkExperience(String link, String username) {
        
        var user = this.usersService.getUserInfo(username);
        var experience = Context.Iskustva.Where(p => p.Vozilo.Link == link).Where(p => p.Korisnik.ID == user.ID).FirstOrDefault();
        if(experience != null) {
            return false;
        }
        return true;
        }
        #endregion
        #region PostExps
        public bool addExperience(String link, String marka, int godiste, String opis, String username) {

            var user = this.usersService.getUserInfo(username);

            if(user == null) {
                return false;
            }

            var vozilo_id = Context.Vozila.Where(p => p.Link == link).Select(p => p.ID).FirstOrDefault();
            var vozilo =  Context.Vozila.Find(vozilo_id);
            var vozilo1 = Context.Vozila.Where(p => p.Link == link).FirstOrDefault();

            if(vozilo == null) {

                Vozilo v = new Vozilo
                {
                    Link = link,
                    Marka = marka,
                    Godiste = godiste
                };
                Context.Vozila.Add(v);
                Context.SaveChanges();

                Iskustvo i = new Iskustvo
                {
                    Opis = opis,
                    Vozilo = v,
                    Korisnik = user,
                    Arhivirano = false
                };
                Context.Iskustva.Add(i);
                Context.SaveChanges();
                return true;
            }
            else {
                if(vozilo.Marka != marka)
                    return false;
                if(vozilo.Godiste != godiste)
                    return false;

                Iskustvo i = new Iskustvo
                {
                    Opis = opis,
                    Vozilo =  vozilo1,
                    Korisnik = user
                };
                Context.Iskustva.Add(i);
                Context.SaveChanges();
                return true;
            }
        }
        #endregion
        #region DeleteExps
        public bool deleteExperience(int id) {
            
            var experience = Context.Iskustva.Find(id);
            if(experience != null) {
                Context.Iskustva.Remove(experience);
                Context.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion
    }
}