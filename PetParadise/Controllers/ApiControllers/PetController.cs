using PetParadise.Extras;
using PetParadise.Extras.Extensions.String;
using PetParadise.Models;
using PetParadise.Models.Body;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PetParadise.Controllers.ApiControllers
{
    public class PetController : ApiController
    {

        [Route("owner/pet")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPet(PetInformationModel petInfo) {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = identity.Claims.First(c => c.Type.Equals("userId")).Value;

                if (string.IsNullOrEmpty(petInfo.Name))
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        message = "Empty name."
                    });
                petInfo.Name = petInfo.Name.Trim().ToTitleCase();
                petInfo.Color = petInfo.Color.Trim().ToTitleCase();
                petInfo.Id = await new UID(IdSize.SHORT).GenerateIdAsync();

                using (MainDBEntities db = new MainDBEntities()) {
                    
                    var owner = db.owner_profile.Where(u => u.Id.Equals(userId)).First();
                    if (owner == null) return Content(HttpStatusCode.Forbidden, new {
                        message="Create owner profile first."
                    });
                    pet_profile pet = new pet_profile() {
                        Id = petInfo.Id,
                        OwnerId = userId,
                        Name = petInfo.Name,
                        Birthdate = petInfo.Birthdate,
                        CategoryId = petInfo.CategoryId,
                        BreedId = petInfo.BreedId,
                        Color = petInfo.Color
                    };
                    pet.owner_profile = owner;
                    db.pet_profile.Add(pet);
                    await db.SaveChangesAsync();
                }

                return Content(HttpStatusCode.Created, new {
                    petId = petInfo.Id
                });

            }
            catch (DbUpdateException e)
            {
                Debug.WriteLine(e.InnerException);
                return StatusCode(HttpStatusCode.BadRequest);
            }
            catch (DbEntityValidationException e)
            {
                var errs = e.EntityValidationErrors.ToList();
                string errorMessage = errs[0].ValidationErrors.ToList()[0].ErrorMessage;

                errs.ForEach(err =>
                {
                    var validationErrors = err.ValidationErrors.ToList();
                    validationErrors.ForEach(er =>
                    {
                        Debug.WriteLine($"property_name: {er.PropertyName}; errorMessage: {er.ErrorMessage}");
                    });
                });
                var errObj = new
                {
                    message = errorMessage,
                    code = HttpStatusCode.BadRequest,
                    stack = e.EntityValidationErrors.ToList()
                };
                return Content(HttpStatusCode.BadRequest, errObj);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return InternalServerError();
            }
        }

        [Route("owner/pets")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetPets() {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = identity.Claims.First(c => c.Type.Equals("userId")).Value;

                using (MainDBEntities db = new MainDBEntities()) {
                    var pet = db.owner_profile
                                .SingleOrDefault(o => o.Id.Equals(userId))
                                .pet_profile
                                .Select(p => new
                                {
                                    p.Id,
                                    p.OwnerId,
                                    p.Name
                                })
                                .ToList();
                    return Ok(pet);
                }
        }
            catch (DbUpdateException e)
            {
                Debug.WriteLine(e.InnerException);
                return StatusCode(HttpStatusCode.BadRequest);
    }
            catch (DbEntityValidationException e)
            {
                var errs = e.EntityValidationErrors.ToList();
    string errorMessage = errs[0].ValidationErrors.ToList()[0].ErrorMessage;

    errs.ForEach(err =>
                {
                    var validationErrors = err.ValidationErrors.ToList();
    validationErrors.ForEach(er =>
                    {
                        Debug.WriteLine($"property_name: {er.PropertyName}; errorMessage: {er.ErrorMessage}");
                    });
                });
                var errObj = new
                {
                    message = errorMessage,
                    code = HttpStatusCode.BadRequest,
                    stack = e.EntityValidationErrors.ToList()
                };
                return Content(HttpStatusCode.BadRequest, errObj);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException);
                Debug.WriteLine(e.StackTrace);
                return InternalServerError();
            }
        }
    }
}
