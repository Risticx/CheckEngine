using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;

namespace WebProjekat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExperienceController : Controller
    {
        public DataContext Context { get; set; }
        private IExperienceService experiencesService;

        public ExperienceController(
            DataContext context,
            IExperienceService experiencesService
        ) {
            Context = context;
            this.experiencesService = experiencesService;
        }

        #region HttpGet
        [Route("GetSomeExperiences")]
        [HttpGet]
        public async Task<ActionResult> GetSomeExperiences()
        {
            try
            {
                return await this.experiencesService.GetSomeExperiences();
            }
                
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("experienceExist/{link}")]
        [HttpGet]
        public ActionResult experienceExist(String link)
        {
            try
            {
                if(experiencesService.experienceExist(link)) {
                    return Ok("Postoji");
                }
                else {
                    return BadRequest("Ne postoji");
                }
            }

                
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("experienceExistMadeAndYear/{marka}/{godiste}")]
        [HttpGet]
        public ActionResult experienceExist(String marka, int godiste)
        {
            try
            {
                if(experiencesService.experienceExist(marka, godiste)) {
                    return Ok("Postoji");
                }
                else {
                    return BadRequest("Ne postoji");
                }
            }

                
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("getExperiences/{link}")]
        [HttpGet]
        public ActionResult getExperiences(String link)
        { 
            try 
            {
                var iskustva = experiencesService.getExperiences(link);
                if(iskustva != null)
                    return Ok(iskustva);
                else
                    return BadRequest("Greska");
            }
            catch(Exception e) 
            {
                return BadRequest(e.Message);
            }
        }

        [Route("checkExperience/{link}/{username}")]
        [HttpGet]
        public ActionResult checkExperience(String link, String username)
        { 
            try 
            {
                var iskustvo = experiencesService.checkExperience(link, username);
                if(iskustvo == true)
                    return Ok("U redu");
                else
                    return BadRequest("Vec ste dodali iskustvo za ovo vozilo");
            }
            catch(Exception e) 
            {
                return BadRequest(e.Message);
            }
        }
        
        [Route("getExperiencesOfUser/{username}")]
        [HttpGet]
        public ActionResult getExperiencesOfUser(String username)
        { 
            try 
            {

                var experiences = this.experiencesService.getExperiencesOfUser(username);
                if(experiences != null) {
                    return Ok(experiences);
                }

                else {
                    return BadRequest("Doslo je do neocekivane greske!");
                }

               
            }
            catch(Exception e) 
            {
                return BadRequest(e.Message);
            }
        }
        [Route("getExperiencseByMadeAndYear/{marka}/{godiste}")]
        [HttpGet]
        public ActionResult getExperiencseByMadeAndYear(String marka, int godiste)
        { 
            try 
            {
                var experiences = this.experiencesService.getExperiencseByMadeAndYear(marka, godiste);
                if(experiences != null) {
                    return Ok(experiences);
                }

                else {
                    return BadRequest("Doslo je do neocekivane greske!");
                }
            }
            catch(Exception e) 
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
        #region HttpPost
        [Authorize]
        [Route("addExperience/{link}/{marka}/{godiste}/{opis}/{username}")]
        [HttpPost]
        public ActionResult addExperience(String link, String marka, int godiste, String opis, String username)
        { 
            try 
            {
                if (username == User.Identity.Name)
                {
                    if (experiencesService.addExperience(link, marka, godiste, opis, username))
                    {
                        return Ok("Uspesno dodato iskustvo");
                    }
                    else
                    {
                        return BadRequest("Doslo je do neocekivane greske!");
                    }
                }
                else
                {
                    return BadRequest("Denied");
                }

               
            }
            catch(Exception e) 
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
        #region HttpDelete
        [Authorize]
        [Route("deleteExperience/{id}/{username}")]
        [HttpDelete]
        public ActionResult deleteExperience(int id, String username) 
        {
            try
            {
                if (username == User.Identity.Name)
                {
                    if (experiencesService.deleteExperience(id))
                        return Ok("Uspesno obrisano");
                    else
                        return BadRequest("Doslo je do neocekivane greske!");
                }
                return BadRequest("Denied");
            }
            catch(Exception e) 
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    }
}