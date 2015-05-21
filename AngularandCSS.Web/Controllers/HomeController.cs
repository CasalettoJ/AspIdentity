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
        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }

        private UserService _userService { get; set; }

        public HomeController(UserService UserService)
        {
            _userService = UserService;
        }


        // GET: Home
        public ActionResult Index()
        {
            if(TempData["Activation"] != null)
            {
                ViewBag.Activation = TempData["Activation"].ToString();
            }
            return View();
        }

        [Authorize]
        public ActionResult SecretPage()
        {
            return View();
        }

        public async Task<ActionResult> Activate(string userID, string confirmationToken)
        {
            if(await _userService.ConfirmEmail(userID, confirmationToken))
            {
                TempData["Activation"] = "Activation was successful.  You may now log in.";
            }
            else
            {
                TempData["Activation"] = "Activation was unsuccessful.  Attempt a login to send a new activation email.";
            }
            return RedirectToAction("Index");
        }
    }
}