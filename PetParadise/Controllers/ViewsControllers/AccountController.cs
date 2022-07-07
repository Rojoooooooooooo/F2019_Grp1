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
        public ActionResult CreateProfile()
        {
            ViewBag.Title = "Create Profile";
            ViewBag.EnableSearchMenu = false;
            ViewBag.EnableUserMenu = false;
            var session = Request.Cookies["session_token"] != null ? Request.Cookies["session_token"].Value : "";

            if (!string.IsNullOrEmpty(session))
            {
                var token = new JwtToken(session, new SessionManager().CreateValidationParameters(SessionType.SESSION));

                if (token.Value != null)
                {
                    var payload = token.GetPayload();
                    var accountType = payload.AccountTypeId;

                    // get account type
                    if (accountType == 1)
                    {
                        ViewBag.Title = "Create Owner Profile";
                        return View("CreateOwner");
                    }
                    else if (accountType == 2)
                    {
                        ViewBag.Title = "Create Clinic Profile";
                        return View("CreateClinic");
                    }
                    else return View("Index");
                }
            }

            return View("Index");
        }

        [Route("owner/{id}")]
        public ActionResult PetDashboard()
        {
            try
            {
                ViewBag.Title = "Pets Dashboard";
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

        [Route("clinic/{id}")]
        public ActionResult Clinic(string id)
        {
            try
            {
                string sessionToken = HttpContext.Request.Cookies["session_token"].Value;
                
                JwtToken token = new JwtToken(sessionToken, new SessionManager().CreateValidationParameters(SessionType.SESSION));
                var payload = token.GetPayload();

                var isUser = payload.UserId.Equals(id);

                ViewBag.Username = payload.Username;


                using (MainDBEntities db = new MainDBEntities()) {
                    pet_profile pet = null;
                    if(payload.AccountTypeId == 1)
                    {
                        string petId = Request.Cookies["pet_id"] != null ? Request.Cookies["pet_id"].Value : "";
                        pet = db.pet_profile.Single(i => i.Id.Equals(petId));
                        ViewBag.PetId = petId;
                        ViewBag.PetName = pet.Name;
                    }
                        

                    var clinic = db.ClinicProfiles.Single(i => i.Id.Equals(id));
                    if (clinic == null) return RedirectToAction("Index", "Home");

                    ViewBag.Title = clinic.ClinicName; 
                    
                    ClinicProfile profile = new ClinicProfile()
                    {
                        Id = clinic.Id,
                        ClinicName = clinic.ClinicName,
                        VetFirstName = clinic.VetFirstName,
                        VetMiddleName = clinic.VetMiddleName,
                        VetLastName = clinic.VetLastName,
                        Line = clinic.Line,
                        Barangay = clinic.Barangay,
                        City = clinic.City,
                        Country = clinic.Country,
                        Contact = clinic.Contact,
                        Followers = clinic.Followers,
                        Rating = clinic.Rating
                    };


                    ViewBag.Following = "";

                    if (!isUser)
                    {
                        var userFollows = db.followings
                                                .Any(i => i.FollowingId.Equals(clinic.Id) &&
                                                        i.FollowerId.Equals(pet.Id));
                        var profileFollows = db.followings
                                                .Any(i => i.FollowingId.Equals(pet.Id) &&
                                                        i.FollowerId.Equals(clinic.Id));

                        var bothFollows = userFollows && profileFollows;

                        if (bothFollows)
                        {
                            ViewBag.Following = "both";
                        }
                        else
                        {
                            ViewBag.Following = userFollows ? "user" : "none";
                        }

                    }
                    ViewData["profile"] = profile;

                    ViewBag.EnableSearchMenu = !isUser;
                    ViewBag.EnableUserMenu = isUser;
                    ViewBag.IsUser = isUser;
                    
                    return View();
                }

                
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
