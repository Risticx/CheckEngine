using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace WebProjekat.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        public DataContext Context { get; set; }
        private IUsersService usersService;
        private IAdminService adminsService;

        public HomeController(
            DataContext context,
            IUsersService usersService,
            IAdminService adminsService
        ) {
            Context = context;
            this.usersService = usersService;
            this.adminsService = adminsService;
        }

        #region Views
        [HttpGet("Login")]
        public IActionResult Login(string returnURL)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Index");
            }
            ViewBag.ReturnURL = returnURL;
            return View();
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin")]
        public IActionResult Admin()
        {
            return View("Admin");
        }

        [HttpGet("Denied")]
        public IActionResult Denied()
        {
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }
        [Authorize]
        [HttpGet("MojiPodaci")]
        public IActionResult MojiPodaci()
        {
            if (User.IsInRole("Admin"))
            {
                return Redirect("/Home/Admin");
            }
            return View();
        }
        [Authorize]
        [HttpGet("SvaIskustva")]
        public IActionResult SvaIskustva()
        {
            if (User.IsInRole("Admin"))
            {
                return Redirect("/Home/Admin");
            }
            return View();
        }
        [HttpGet("Iskustva")]
        public IActionResult Iskustva()
        {
            return View();
        }
        #endregion

        #region GET

        [Authorize]
        [HttpGet("getUsername")]
        public ActionResult getUsername()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(User.Identity.Name);
            }
            return BadRequest();
        }

        [Authorize]
        [Route("getUserInfo/{username}")]
        [HttpGet]
        public ActionResult getUserInfo(String username)
        {
            try
            {
                if (username == User.Identity.Name)
                {
                    var user = usersService.getUserInfo(username);
                    return Ok(user);
                }
                else
                {
                    return BadRequest("Denied");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region POST

        [HttpPost("Login")]
        public async Task<IActionResult> LoginCheck([FromQuery] string returnURL)
        {
            ViewBag.ReturnURL = returnURL;
            string username = HttpContext.Request.Form["username"].ToString();
            string password = HttpContext.Request.Form["password"].ToString();

            bool userExists = usersService.authUser(username, password);
            bool adminExists = adminsService.authAdmin(username, password);
            if (userExists)
            {
                var claims = new List<Claim>
                {
                    new Claim("username", username),
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.IsPersistent, "True"),
                    new Claim("session", Guid.NewGuid().ToString())
                };

                var ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var cp = new ClaimsPrincipal(ci);
                await HttpContext.SignInAsync(cp);
                return Redirect(returnURL);
            }
            else if(adminExists)
            {
                var claims = new List<Claim>
                {
                    new Claim("username", username),
                    new Claim(ClaimTypes.NameIdentifier, System.Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.IsPersistent, "True"),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var cp = new ClaimsPrincipal(ci);
                await HttpContext.SignInAsync(cp);
                return Redirect(returnURL);
            }
            else
            {
                ViewBag.Error = "Username or password invalid!";
                return View("Login");
            }
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Home/Index");
        }

        [Authorize(Roles = "Admin")]
        [Route("addAdmin/{username}/{password}/{ime}/{email}")]
        [HttpPost]
        public ActionResult addAdmin(String username, String password, String ime)
        {
            try
            {
                if (adminsService.isAdminAlreadyRegistered(username))
                {
                    return BadRequest("Admin je vec registrovan!");
                }

                if (usersService.addAdmin(username, password, ime))
                {
                    return Ok("Uspesno dodat admin!");
                }
                return BadRequest("Doslo je do neocekivane greske!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("Register/{username}/{password}/{ime}/{prezime}/{email}")]
        [HttpPost]
        public ActionResult Register(String username, String password, String ime, String prezime, String email)
        {
            try
            {
                if (usersService.isUserAlreadyRegistered(username, email))
                {
                    return BadRequest("Korisnik je vec registrovan!");
                }

                if (usersService.addUser(username, password, ime, prezime, email))
                {
                    return Ok("Uspesna registracija!");
                }

                return BadRequest("Doslo je do neocekivane greske!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region PUT
        [Authorize]
        [Route("editUserInfo/{ime}/{prezime}/{email}/{password}/{username}")]
        [HttpPut]
        public ActionResult editUserInfo(String ime, String prezime, String email, String password, String username)
        {
            try
            {
                if (username == User.Identity.Name)
                {
                    if (this.usersService.editUserInfo(ime, prezime, email, password, username))
                    {
                        return Ok("Korisnik uspesno izmenjen");

                    }
                }
                else
                {
                    return BadRequest();
                }
                    return BadRequest("Email zauzet!");
            }
            catch(Exception e) 
            {
                return BadRequest(e.Message);
            }
        }                           
        #endregion
    }
}