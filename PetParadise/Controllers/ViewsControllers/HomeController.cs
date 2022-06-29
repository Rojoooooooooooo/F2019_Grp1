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
            ViewBag.EnableSearchMenu = false;
            ViewBag.EnableUserMenu = false;

            var session = Request.Cookies["session_token"] != null ? Request.Cookies["session_token"].Value : "";
            if (!string.IsNullOrEmpty(session)) {
                var token = new JwtToken(session, new SessionManager().CreateValidationParameters(SessionType.SESSION));

                if (token.Value != null)
                {
                    var payload = await token.GetPayloadAsync();
                    if (payload.AccountTypeId == 1)
                    {
                        return RedirectToAction("PetDashboard", "Account");
                    }
                    else if (payload.AccountTypeId == 2) {
                        return View();
                        //return RedirectToAction("PetDashboard", "Account");
                    }
                }
            } 
            return View();
        }

        // GET: Home/Signup
        public ActionResult Signup()
        {
            ViewBag.EnableSearchMenu = false;
            ViewBag.EnableUserMenu = false;

            var session = Request.Cookies["session_token"] != null ? Request.Cookies["session_token"].Value : "";
            if (!string.IsNullOrEmpty(session))
            {
                var token = new JwtToken(session, new SessionManager().CreateValidationParameters(SessionType.SESSION));

                if (token.Value != null)
                {
                    var payload = token.GetPayload();
                    if (payload.AccountTypeId == 1)
                    {
                        return RedirectToAction("PetDashboard", "Account");
                    }
                    else if (payload.AccountTypeId == 2)
                    {
                        return View();
                        //return RedirectToAction("PetDashboard", "Account");
                    }
                }
            }
            return View();
        }

        public ActionResult Feeds(string uid)
        {
            ViewBag.EnableSearchMenu = true;
            ViewBag.EnableUserMenu = false;

            var session = Request.Cookies["session_token"] != null ? Request.Cookies["session_token"].Value : "";
            if (!string.IsNullOrEmpty(session))
            {
                var token = new JwtToken(session, new SessionManager().CreateValidationParameters(SessionType.SESSION));
                if (token.Value != null)
                {
                    var payload = token.GetPayload();
                    return View(payload);
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}