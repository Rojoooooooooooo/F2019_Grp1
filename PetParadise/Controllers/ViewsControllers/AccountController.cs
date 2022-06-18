using PetParadise.Extras;
using PetParadise.Extras.Extensions.JwtSecurity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace PetParadise.Controllers.ViewsControllers
{
    public class AccountController : Controller
    {

        // GET: Create
        public ActionResult CreateProfile(int accountType)
        {
            string accessToken = HttpContext.Request.Cookies["access_token"] != null ? HttpContext.Request.Cookies["access_token"].Value : "";
            JwtToken token = new JwtToken(accessToken, new SessionManager().CreateValidationParameters(SessionType.ACCESS));

            if (token.Value == null || token.Value == "")
                ViewBag.HasAccess = false;
            else ViewBag.HasAccess = true;

            // get account type
            if (accountType == 1) {
                ViewBag.Title = "Build Owner Profile";
                return View("CreateOwner");
            }
            else if (accountType == 2) {
                ViewBag.Title = "Build Clinic Profile";
                return View("CreateClinic");
             }
            else return View("Index");
        }

        public ActionResult PetDashboard() {
            return View();
        }
    }
}
