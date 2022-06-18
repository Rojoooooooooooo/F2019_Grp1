using PetParadise.Controllers.ApiControllers;
using PetParadise.Extras;
using PetParadise.Extras.Extensions.JwtSecurity;
using PetParadise.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PetParadise.Controllers.ViewsControllers
{

    public class HomeController : Controller
    {
        // GET: Home
        public async Task<ActionResult> Index()
        {

            SessionManager sessionManager = new SessionManager("123456", "druloloy");
            string token = await sessionManager.TokenHandlerAsync(SessionType.SESSION);

            Session["session_token"] = token;
            return View();
        }

        // GET: Home/Signup
        public ActionResult Signup()
        {
            return View();
        }

        public ActionResult Feeds(string uid)
        {
            return View();
        }
    }
}