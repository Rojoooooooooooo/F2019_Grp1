using PetParadise.Controllers.ApiControllers;
using PetParadise.Extras;
using PetParadise.Extras.Extensions.JwtSecurity;
using PetParadise.Models;
using PetParadise.Models.Body;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
            ViewBag.Title = "Welcome";
            ViewBag.EnableSearchMenu = false;
            ViewBag.EnableUserMenu = false;

            var session = Request.Cookies["session_token"] != null ? Request.Cookies["session_token"].Value : "";
            if (!string.IsNullOrEmpty(session)) {
                var token = new JwtToken(session, new SessionManager().CreateValidationParameters(SessionType.SESSION));

                if (token.Value != null)
                {
                    var payload = await token.GetPayloadAsync();

                    using (MainDBEntities db = new MainDBEntities()) {
                        bool hasProfile = await db.owner_profile
                                        .AnyAsync(u => u.Id.Equals(payload.UserId)) ||
                                        await db.clinic_profile.AnyAsync(u => u.Id.Equals(payload.UserId));
                        if (!hasProfile)
                            return RedirectToAction("CreateProfile", "Account");

                        if (payload.AccountTypeId == 1)
                        {
                            Response.Redirect("/owner/" + payload.UserId);
                        }
                        else if (payload.AccountTypeId == 2)
                        {
                            Response.Redirect("/clinic/" + payload.UserId);
                        }
                    }
                }    
            } 
            return View();
        }

        // GET: Home/Signup
        public ActionResult Signup()
        {
            ViewBag.Title = "Signup";
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

        [Route("pet/{id}")]
        public async Task<ActionResult> Feeds(string id)
        {
            ViewBag.Title = "Feeds";
            ViewBag.EnableSearchMenu = true;
            ViewBag.EnableUserMenu = false;

            var session = Request.Cookies["session_token"] != null ? Request.Cookies["session_token"].Value : "";
            if (string.IsNullOrEmpty(session)) return RedirectToAction("Index", "Home");

            var token = new JwtToken(session, new SessionManager().CreateValidationParameters(SessionType.SESSION));

            if (string.IsNullOrEmpty(token.Value))
                return RedirectToAction("Index", "Home");


            var payload = await token.GetPayloadAsync();
            using (MainDBEntities db = new MainDBEntities())
            {
                var pet = await db.PetProfiles.SingleAsync(i => i.Id.Equals(id) && i.OwnerId.Equals(payload.UserId));
                if (pet == null) return RedirectToAction("Index", "Home");


                PetProfile profile = new PetProfile()
                {
                    Id = pet.Id,
                    OwnerId = pet.OwnerId,
                    Name = pet.Name,
                    Birthdate = pet.Birthdate,
                    Color = pet.Color,
                    Line = pet.Line,
                    Barangay = pet.Barangay,
                    City = pet.City,
                    Country = pet.Country,
                    Followers = pet.Followers,
                    Following = pet.Following,
                    Category = pet.Category,
                    Breed = pet.Breed,
                    ContactId = pet.ContactId,
                    Contact = pet.Contact
                };

                ViewBag.PetId = pet.Id;
                ViewData["profile"] = profile;

            }
            return View();
        }


        [Route("profile/{id}")]
        public new ActionResult Profile(string id)
        {
            
            try
            {
                ViewBag.EnableSearchMenu = true;

                string sessionToken = HttpContext.Request.Cookies["session_token"].Value;

                JwtToken token = new JwtToken(sessionToken, new SessionManager().CreateValidationParameters(SessionType.SESSION));
                var payload = token.GetPayload();

                ViewBag.Username = payload.Username;
               

                using (MainDBEntities db = new MainDBEntities())
                {
                    var pet = db.PetProfiles.Single(i => i.Id.Equals(id));
                    if (pet == null) return RedirectToAction("Index", "Home");

                    string userPetId = Request.Cookies["pet_id"] != null ? Request.Cookies["pet_id"].Value : "";

                    bool isUser = db.pet_profile
                                    .Single(i => i.Id.Equals(pet.Id)).OwnerId.Equals(payload.UserId) && pet.Id.Equals(userPetId);

                    ViewBag.Title = pet.Name;

                    PetProfile profile = new PetProfile()
                    {
                        Id = pet.Id,
                        OwnerId = pet.OwnerId,
                        Name = pet.Name,
                        Birthdate = pet.Birthdate,
                        Color = pet.Color,
                        Line = pet.Line,
                        Barangay = pet.Barangay,
                        City = pet.City,
                        Country = pet.Country,
                        Followers = pet.Followers,
                        Following = pet.Following,
                        Category = pet.Category,
                        Breed = pet.Breed,
                        ContactId = pet.ContactId,
                        Contact = pet.Contact
                    };

                    ViewBag.Following = "";
                    ViewBag.PetId = userPetId;
                    ViewBag.IsUser = isUser;

                    if (!isUser) {
                        var userFollows = db.followings
                                                .Any(i => i.FollowingId.Equals(pet.Id) &&
                                                        i.FollowerId.Equals(userPetId));
                        var profileFollows = db.followings
                                                .Any(i => i.FollowingId.Equals(userPetId) &&
                                                        i.FollowerId.Equals(pet.Id));

                        var bothFollows = userFollows && profileFollows;

                        if (bothFollows)
                        {
                            ViewBag.Following = "both";
                        }
                        else {
                           ViewBag.Following = userFollows ? "user" : "none";
                        }
                        
                    }

                    ViewBag.EnableUserMenu = isUser;
                    ViewData["profile"] = profile;
                    return View();
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("post/{postId}")]
        public ActionResult Post(string postId, string petid)
        {
            try
            {
                ViewBag.EnableSearchMenu = true;
                ViewBag.EnableUserMenu = false;

                string sessionToken = HttpContext.Request.Cookies["session_token"].Value;

                JwtToken token = new JwtToken(sessionToken, new SessionManager().CreateValidationParameters(SessionType.SESSION));

                var payload = token.GetPayload();


                ViewBag.Username = payload.Username;
                ViewBag.IsUser = false;

                using (MainDBEntities db = new MainDBEntities())
                {
                    ViewBag.PetName = db.pet_profile.Single(i => i.Id.Equals(petid) && i.OwnerId.Equals(payload.UserId)).Name;
                    var post = db.profile_post
                                    .Select(i => new PostModel()
                                    {
                                        Id = i.Id,
                                        ProfileId = i.ProfileId,
                                        Name = i.pet_profile.Name,
                                        Content = i.PostContent,
                                        CreatedAt = i.PostCreationDate,
                                        LikesCount = i.profile_post_like.Count(),
                                        Liked = i.profile_post_like.Any(j => j.ProfileId.Equals(petid)),
                                        CommentsCount = i.profile_post_comment.Count(),
                                        Comments = i.profile_post_comment.Select(j => new CommentModel()
                                        {
                                            Id = j.Id,
                                            Name = j.pet_profile.Name,
                                            ProfileId = j.ProfileId,
                                            PostId = j.PostId,
                                            Content = j.CommentContent,
                                            CreatedAt = j.CommentCreationDate
                                        })
                                        .OrderBy(j=>j.CreatedAt)
                                        .ToList()
                                    })
                                    .Single(i => i.Id.Equals(postId));
                    if (post == null)
                        return View("Index");


                    ViewData["post"] = post;
                    ViewBag.PetId = petid;
                }

                return View();
            }
            catch (Exception)
            {
                return View("Index");
            }
        }

        [Route("search/{query}")]
        public ActionResult Search(string query)
        {
            try
            {
                // decode query from base64
                byte[] queryData = Convert.FromBase64String(query);
                string decodedQuery = Encoding.UTF8.GetString(queryData);

                ViewBag.EnableSearchMenu = true;
                ViewBag.EnableUserMenu = false;
                ViewBag.Query = decodedQuery;
                ViewBag.Title = "Search results for: " + decodedQuery;
                using (MainDBEntities db = new MainDBEntities()) {
                    string userPetId = Request.Cookies["pet_id"] != null ? Request.Cookies["pet_id"].Value : "";
                    var pets = db.pet_profile.Select(i => new SearchModel() {
                        Id = i.Id,
                        Name = i.Name,
                        Type = 1
                    })
                    .Where(i => i.Name.StartsWith(decodedQuery)).ToArray();

                    var clinic = db.clinic_profile.Select(i => new SearchModel() {
                        Id = i.Id,
                        Name = i.Name,
                        Type = 2
                    })
                    .Where(i => i.Name.StartsWith(decodedQuery)).ToArray();

                    SearchModel[] result = pets.Concat(clinic).ToArray();

                    ViewData["result"] = result;
                    ViewBag.PetId = userPetId;
                    return View();

                }
            }
            catch (Exception)
            {

                return View("Index");
            }
        }

        [Route("nearby/clinics")]
        public ActionResult NearbyClinics()
        {
            ViewBag.EnableSearchMenu = true;
            ViewBag.EnableUserMenu = false;
            string userPetId = Request.Cookies["pet_id"] != null ? Request.Cookies["pet_id"].Value : "";
            ViewBag.PetId = userPetId;

            return View();
        }
    }
}