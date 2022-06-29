using PetParadise.Extras;
using PetParadise.Extras.Extensions.JwtSecurity;
using PetParadise.Models;
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
            ViewBag.EnableSearchMenu = false;
            ViewBag.EnableUserMenu = false;
            var session = Request.Cookies["session_token"] != null ? Request.Cookies["session_token"].Value : "";

            if (!string.IsNullOrEmpty(session))
            {
                var token = new JwtToken(session, new SessionManager().CreateValidationParameters(SessionType.SESSION));

                if (token.Value != null)
                {
                    var payload = token.GetPayload();

                    // get account type
                    if (accountType == 1)
                    {
                        ViewBag.Title = "Build Owner Profile";
                        return View("CreateOwner");
                    }
                    else if (accountType == 2)
                    {
                        ViewBag.Title = "Build Clinic Profile";
                        return View("CreateClinic");
                    }
                    else return View("Index");
                }
            }

            return View("Index");
        }

        public ActionResult PetDashboard()
        {
            try
            {
                ViewBag.EnableSearchMenu = false;
                ViewBag.EnableUserMenu = true;

                string sessionToken = HttpContext.Request.Cookies["session_token"].Value;

                JwtToken token = new JwtToken(sessionToken, new SessionManager().CreateValidationParameters(SessionType.SESSION));
                var payload = token.GetPayload();
                string username = payload.Username;
                ViewBag.Username = username;

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public new ActionResult Profile()
        {
            ViewBag.EnableSearchMenu = false;
            ViewBag.EnableUserMenu = false;
            return View();
        }

        public ActionResult Clinic()
        {
            try
            {
                ViewBag.EnableSearchMenu = false;
                ViewBag.EnableUserMenu = true;

                string sessionToken = HttpContext.Request.Cookies["session_token"].Value;

                JwtToken token = new JwtToken(sessionToken, new SessionManager().CreateValidationParameters(SessionType.SESSION));
                var payload = token.GetPayload();
                string uid = payload.UserId;

                using (MainDBEntities db = new MainDBEntities())
                {

                }


                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
