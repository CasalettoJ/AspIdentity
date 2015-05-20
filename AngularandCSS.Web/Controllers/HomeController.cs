using AngularandCSS.Service;
using AngularandCSS.Service.ViewModels;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AngularandCSS.Web.Controllers
{
    public class HomeController : Controller
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private UserService _userService { get; set; }

        public HomeController(UserService UserService)
        {
            _userService = UserService;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult SecretPage()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                RegistrationResultViewModel result = await _userService.Register(model);
                if (result.Result.Succeeded)
                {
                    await _userService.SignIn(result.User, false, AuthenticationManager);
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await _userService.SignOut(AuthenticationManager);
            return RedirectToAction("Index");
        }

    }
}