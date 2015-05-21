using AngularandCSS.Service;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AngularandCSS.Web.Controllers
{
    public class UserController : Controller
    {
        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }

        private UserService _userService { get; set; }

        public UserController(UserService UserService)
        {
            _userService = UserService;
        }


        public async Task<ActionResult> Activate(string userID, string confirmationToken)
        {
            if (await _userService.ConfirmEmail(userID, confirmationToken))
            {
                TempData["Activation"] = "Activation was successful.  You may now log in.";
            }
            else
            {
                TempData["Activation"] = "Activation was unsuccessful.  Attempt a login to send a new activation email.";
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Recover(string userID, string recoveryToken)
        {
            if (await _userService.CheckRecoveryValidity(userID, recoveryToken))
            {
                return View();
            }
            TempData["Recovery"] = "The recovery link followed was invalid or expired.";
            return RedirectToAction("Index", "Home");
        }
    }
}