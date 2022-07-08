using PetParadise.Extras;
using PetParadise.Extras.Error;
using PetParadise.Extras.Extensions.String;
using PetParadise.Models;
using PetParadise.Models.Body;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
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
                        error = new { message = "Empty name." }
                    });

                if (string.IsNullOrEmpty(petInfo.Color))
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        error = new { message = "Empty color." }
                    });
                if (petInfo.Birthdate == DateTime.MinValue)
                    return Content(HttpStatusCode.BadRequest, new {
                        error =  new { message = "Invalid date format" }
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
                Debug.WriteLine(e.InnerException);
                return InternalServerError();
            }
        }

        //[Route("owner/pet")]
        //[HttpGet]
        //[Authorize]
        //public IHttpActionResult GetPet(string petId)
        //{
        //    try
        //    {
        //        ClaimsIdentity identity = User.Identity as ClaimsIdentity;
        //        var userId = identity.Claims.First(c => c.Type.Equals("userId")).Value;

        //        using (MainDBEntities db = new MainDBEntities())
        //        {
        //            var pet = db.owner_profile
        //                        .SingleOrDefault(o => o.Id.Equals(userId))
        //                        .pet_profile
        //                        .Select(p => new
        //                        {
        //                            p.Id,
        //                            p.OwnerId,
        //                            p.Name,
        //                            p.BreedId,
        //                            p.CategoryId,
        //                            p.Color,
        //                            p.Birthdate
        //                        })
        //                        .Single(p => p.Id == petId);

        //            return Ok(pet);
        //        }
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        Debug.WriteLine(e.InnerException);
        //        return StatusCode(HttpStatusCode.BadRequest);
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        var errs = e.EntityValidationErrors.ToList();
        //        string errorMessage = errs[0].ValidationErrors.ToList()[0].ErrorMessage;

        //        errs.ForEach(err =>
        //        {
        //            var validationErrors = err.ValidationErrors.ToList();
        //            validationErrors.ForEach(er =>
        //            {
        //                Debug.WriteLine($"property_name: {er.PropertyName}; errorMessage: {er.ErrorMessage}");
        //            });
        //        });
        //        var errObj = new
        //        {
        //            message = errorMessage,
        //            code = HttpStatusCode.BadRequest,
        //            stack = e.EntityValidationErrors.ToList()
        //        };
        //        return Content(HttpStatusCode.BadRequest, errObj);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e.InnerException);
        //        Debug.WriteLine(e.StackTrace);
        //        return InternalServerError();
        //    }
        //}

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

        [Route("pet/categories")]
        [HttpGet]
        public IHttpActionResult GetCategories() {
            try
            {
                using (MainDBEntities db = new MainDBEntities())
                {
                    var categories = db.pet_category
                                    .Select(c=>new {
                                        c.Id,
                                        c.Category
                                    })
                                    .OrderBy(c=>c.Category)
                                    .ToList();

                    return Ok(categories);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);

                return InternalServerError();
            }

        }

        [Route("pet/{catId}/breed")]
        [HttpGet]
        public IHttpActionResult GetBreeds(int catId)
        {
            try
            {
                using (MainDBEntities db = new MainDBEntities())
                {
                    var breeds = db.pet_category
                                    .Single(p=>p.Id == catId)
                                    .pet_breeds
                                    .Select(c => new {
                                        c.Id,
                                        c.Breed
                                    })
                                    .OrderBy(c=>c.Breed)
                                    .ToList();

                    return Ok(breeds);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return InternalServerError();
            }
        }
        
        [Authorize]
        [Route("follow")]
        [HttpPost]
        public async Task<IHttpActionResult> Follow(string followingId, string followerId, string type)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = identity.Claims.First(c => c.Type.Equals("userId")).Value;

                using(MainDBEntities db = new MainDBEntities())
                {
                    var isValid = db.pet_profile
                                    .Where(i => i.OwnerId.Equals(userId))
                                    .Any(i => i.Id.Equals(followerId));
                    if(!isValid)
                        return new HttpErrorContent(Request,
                            HttpStatusCode.BadRequest,
                            Extras.Error.HttpError.InvalidSession);

                    bool followingExists = false;
                    bool followerExists = false;
                    switch (type) {
                        case "clinic":
                            {
                                followingExists = await db.clinic_profile.AnyAsync(i => i.Id.Equals(followingId));
                                followerExists = await db.pet_profile.AnyAsync(i => i.Id.Equals(followerId));
                                break;
                            }
                        case "pet":
                            {
                                followingExists = await db.pet_profile.AnyAsync(i => i.Id.Equals(followingId));
                                followerExists = await db.pet_profile.AnyAsync(i => i.Id.Equals(followerId));
                                break;
                            }
                        default:
                            followingExists = false;
                            break;
                    }

                    if (!followingExists || !followerExists) 
                        return new HttpErrorContent(Request, 
                            HttpStatusCode.BadRequest, 
                            Extras.Error.HttpError.InvalidSession);

                    var followed = await db.followings.AnyAsync(i => i.FollowerId.Equals(followerId) &&
                                                            i.FollowingId.Equals(followingId));
                    if (followed) {
                        var follow = await db.followings.SingleAsync(i => i.FollowerId.Equals(followerId) &&
                                                            i.FollowingId.Equals(followingId));
                        db.followings.Remove(follow);
                        await db.SaveChangesAsync();
                        return Ok();
                    }

                    else
                    {
                        string followId = await new UID(IdSize.SHORT).GenerateIdAsync();

                        db.followings.Add(new following()
                        {
                            Id = followId,
                            FollowingId = followingId,
                            FollowerId = followerId,
                            Type = type
                        });
                        await db.SaveChangesAsync();
                        return Ok();
                    }
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

        [Authorize]
        [Route("unfollow")]
        [HttpPost]
        public async Task<IHttpActionResult> Unfollow(string followingId, string followerId, string type)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = identity.Claims.First(c => c.Type.Equals("userId")).Value;

                using (MainDBEntities db = new MainDBEntities())
                {
                    var follow = db.followings
                                        .Single(i=>i.FollowingId.Equals(followingId) && i.FollowerId.Equals(followerId));
                    if (follow == null)
                        return new HttpErrorContent(Request,
                            HttpStatusCode.BadRequest,
                            Extras.Error.HttpError.InvalidSession);

                    db.followings.Remove(follow);
                    await db.SaveChangesAsync();

                    return Ok();
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


        [Route("pet/")]
        [HttpGet]
        public IHttpActionResult GetPet(string id)
        {
            try
            {
                using (MainDBEntities db = new MainDBEntities())
                {
                    var pet = db.PetProfiles
                                    .Where(i => i.Id.Equals(id))
                                    .ToList();
                    return Ok(pet);
                }
            }
            catch (DbUpdateException e)
            {
                Debug.WriteLine(e.InnerException);
                return Content(HttpStatusCode.BadRequest, new
                {
                    message = "Request cancelled."
                });
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

        [Authorize]
        [Route("pet/{id}/feedback")]
        [HttpPost]
        public async Task<IHttpActionResult> WriteFeedback(ReviewModel feedback, string id)
        {
            try
            {
                if (string.IsNullOrEmpty(feedback.Content))
                    return BadRequest();

                using (MainDBEntities db = new MainDBEntities())
                {
                    // check if pet id exists
                    var petExists = db.pet_profile.Any(i => id.Equals(i.Id));
                    if (!petExists)
                        return new HttpErrorContent(Request, HttpStatusCode.BadRequest, Extras.Error.HttpError.InvalidSession);

                    // check if clinic exists
                    var clinicExists = db.clinic_profile.Any(i => feedback.ClinicId.Equals(i.Id));
                    if(!clinicExists)
                        return new HttpErrorContent(Request, HttpStatusCode.BadRequest, Extras.Error.HttpError.InvalidSession);

                    string reviewId = await new UID(IdSize.SHORT).GenerateIdAsync();
                    clinic_review review = new clinic_review()
                    {
                        Id = reviewId,
                        ClinicId = feedback.ClinicId,
                        ReviewerId = id,
                        ReviewContent = feedback.Content,
                        Rating = feedback.Rating,
                        ReviewCreationDate = DateTime.UtcNow
                    };

                    db.clinic_review.Add(review);
                    await db.SaveChangesAsync();

                    return Ok();
                }
            }
            catch (DbUpdateException e)
            {
                Debug.WriteLine(e.InnerException);
                return Content(HttpStatusCode.BadRequest, new
                {
                    message = "Request cancelled."
                });
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
    }
}
