using SummaryReport.Models;
using SummaryReport.Models.Containers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SummaryReport.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            try
            {
                using (MainDBEntities db = new MainDBEntities())
                {

                    var users = db.account_credential;

                    var ownersCount = users.Where(I => I.AccountTypeId == 1).Count();
                    var clinicsCount = users.Where(I => I.AccountTypeId == 2).Count();
                    var petsCounts = db.PetProfiles.Count();
                    var avePetCountPerOwner = db.owner_profile
                                        .Select(i => new
                                        {
                                            Id = i.Id,
                                            PetCount = i.pet_profile.Count()
                                        })
                                        .Select(i => i.PetCount)
                                        .Average();

                    var petAge = db.pet_profile
                                          .Select(i => i.Birthdate)
                                          .ToList();
                    int totalAge = 0;
                    petAge.ForEach(i =>
                    {
                        totalAge += CalculateAge(i);
                    });

                    double averagePetAge = (totalAge / (petsCounts));

                    ViewBag.OwnersCount = ownersCount;
                    ViewBag.ClinicsCount = clinicsCount;
                    ViewBag.PetsCount = petsCounts;
                    ViewBag.AvePetPerOwner = Math.Round(avePetCountPerOwner, 2);
                    ViewBag.AvePetAge = Math.Round(averagePetAge, 2);

                    ViewBag.RegisteredUsers = users.Count();
                }
                return View();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException);
                return View();
            }
        }

        private int CalculateAge(DateTime birthdate) {
            int yearDiff = (DateTime.UtcNow.Year - birthdate.Year);
            if (birthdate.Date > DateTime.UtcNow.AddYears(-yearDiff)) yearDiff--;
            return yearDiff;
        } 
    }
}