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
        // GET: Home
        public ActionResult Index()
        {
            if(TempData["Activation"] != null)
            {
                ViewBag.SystemMessage = TempData["Activation"].ToString();
            }
            if (TempData["Recovery"] != null)
            {
                ViewBag.SystemMessage = TempData["Recovery"].ToString();
            }
            return View();
        }

        [Authorize]
        public ActionResult SecretPage()
        {
            return View();
        }

    }
}