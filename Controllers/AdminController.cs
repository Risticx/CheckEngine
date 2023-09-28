using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace Aplikacija.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase {
        public DataContext Context { get; set; }
        private IUsersService usersService;
        private IAdminService adminsService;

        public AdminController(
            DataContext context,
            IUsersService usersService,
            IAdminService adminsService
        ) {
            Context = context;
            this.usersService = usersService;
            this.adminsService = adminsService;
        }

        #region HttpGet

        #endregion
        #region HttpPut
        [Authorize(Roles = "Admin")]
        [Route("archiveExperience/{link}/{username}")]
        [HttpPut]
        public ActionResult archiveExperience(String link, String username)
        { 
            try 
            {
                if(this.adminsService.archiveExperience(link, username)) {
                    return Ok("Uspesno arhivirano");
                }
                return BadRequest("Doslo je do neocekivane greske!");
            }
            catch(Exception e) 
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [Route("unArchiveExperience/{link}/{username}")]
        [HttpPut]
        public ActionResult unArchiveExperience(String link, String username)
        { 
            try 
            {
                if(this.adminsService.unArchiveExperience(link, username)) {
                    return Ok("Uspesno arhivirano");
                }
                return BadRequest("Doslo je do neocekivane greske!");
            }
            catch(Exception e) 
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
        #region HttpDelete
        [Authorize(Roles = "Admin")]
        [Route("deleteUser/{username}")]
        [HttpDelete]
        public ActionResult deleteUser(String username) 
        {
            try
            {
                if(this.adminsService.deleteUser(username))
                    return Ok("Uspesno obrisano");
                else
                    return BadRequest("Doslo je do neocekivane greske!");
            }
            catch(Exception e) 
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    }
}
