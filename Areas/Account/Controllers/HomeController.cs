using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResourcePlanning.Models;
using ResourcePlanning.Utility;
using ResourcePlanning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ResourcePlanning.Areas.Account.Controllers
{

    [Area("Account")]
    [Authorize]
    public class HomeController : Controller
    {

        private readonly UserManager<RPUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RPSignInManager _signInManager;
        private readonly IUserService _userservice;

        private ISession _session;
        public ISession Session { get { return _session; } }

        private BO busObject = new BO();


        public HomeController(RPSignInManager signInManager,            
            UserManager<RPUser> userManager, IUserService userservice, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userservice = userservice;
            _session = httpContextAccessor.HttpContext.Session;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;

            //returnurl = returnurl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {

                    // =============================================== 
                    var cmicuser = busObject.GetCmiCUser(model.Email);

                    string usercompany = cmicuser.UserCompany;

                    if (usercompany == "0100" || usercompany == "1500")
                    {
                        _session.SetString("_SessionKey_Company", "0100");
                    } 
                    else {
                        _session.SetString("_SessionKey_Company", usercompany);
                    }

                    // ==================================================
                    


                    if (returnurl != null)
                    {
                        return LocalRedirect(returnurl);
                    }
                    else
                    {

                        return Redirect("/Admin/Home/Index");
                        
                    }

                }
            
                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Account/Home/Login");
        }
    }
}
