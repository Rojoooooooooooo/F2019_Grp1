using PetParadise.Extras;
using PetParadise.Extras.Extensions.String;
using PetParadise.Models;
using PetParadise.Models.Body;

using System;
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

namespace PetParadise.Controllers
{
    public class OwnerProfileController : ApiController
    {
        [Authorize]
        [HttpPost]
        [Route("user/owner")]
        public async Task<IHttpActionResult> AddOwnerInformation(OwnerInformationModel ownerInput)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = identity.Claims.First(c => c.Type.Equals("userId")).Value;

                using (MainDBEntities db = new MainDBEntities())
                {
                    owner_contact contactNumber = new owner_contact()
                    {
                        Id = await new UID(IdSize.SHORT).GenerateIdAsync(),
                        UserId = userId,
                        Contact = ownerInput.Contact
                    };

                    owner_address address = new owner_address()
                    {
                        Id = userId,
                        Line = ownerInput.Line,
                        Barangay = ownerInput.Barangay,
                        City = ownerInput.City,
                        Country = ownerInput.Country
                    };
                    owner_profile profile = new owner_profile()
                    {
                        Id = userId,
                        FirstName = ownerInput.FirstName,
                        MiddleName = ownerInput.MiddleName,
                        LastName = ownerInput.LastName,
                        owner_address = address
                    };
                    profile.owner_contact.Add(contactNumber);
                    db.owner_profile.Add(profile);
                    
                    await db.SaveChangesAsync();
                    
                }
                return Content(HttpStatusCode.Created, "Profile created!");
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


        [Authorize]
        [HttpPut]
        [Route("user/owner")]
        public async Task<IHttpActionResult> UpdateOwner(OwnerInformationModel infoModel, string update)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = identity.Claims.First(c => c.Type.Equals("userId")).Value;

                using (MainDBEntities db = new MainDBEntities())
                {
                    switch (update)
                    {
                        /// updates the basic information
                        /// in this case, the owner's full name
                        case "basic":
                            {
                                var profile = db.owner_profile
                                            .Single(p => p.Id.Equals(userId));

                                if (profile == null) return Content(HttpStatusCode.NotFound, new
                                {
                                    message = "You haven't added one yet."
                                });

                                infoModel.FirstName = infoModel.FirstName.Trim().ToTitleCase();
                                infoModel.MiddleName = infoModel.MiddleName.Trim().ToTitleCase();
                                infoModel.LastName = infoModel.LastName.Trim().ToTitleCase();

                                if (string.IsNullOrEmpty(infoModel.FirstName))
                                    return Content(HttpStatusCode.BadRequest, new
                                    {
                                        message = "Empty First name."
                                    });
                                if (string.IsNullOrEmpty(infoModel.LastName))
                                    return Content(HttpStatusCode.BadRequest, new
                                    {
                                        message = "Empty Last name."
                                    });

                                profile.FirstName = infoModel.FirstName;
                                profile.MiddleName = infoModel.MiddleName;
                                profile.LastName = infoModel.LastName;

                                break;
                            }
                        
                        /// Update the owner's address
                        case "address":
                            {
                                var address = db.owner_address
                                    .Single(p => p.Id.Equals(userId));

                                if (address == null)
                                    return Content(HttpStatusCode.NotFound, new
                                    {
                                        message = "You haven't added one yet."
                                    });

                                infoModel.Line = infoModel.Line.Trim().ToTitleCase();
                                infoModel.Barangay = infoModel.Barangay.Trim().ToTitleCase();
                                infoModel.City = infoModel.City.Trim().ToTitleCase();
                                infoModel.Country = infoModel.Country.Trim().ToTitleCase();

                                if (string.IsNullOrEmpty(infoModel.Line) ||
                                    string.IsNullOrEmpty(infoModel.Barangay) ||
                                    string.IsNullOrEmpty(infoModel.City) ||
                                    string.IsNullOrEmpty(infoModel.Country))
                                        return Content(HttpStatusCode.BadRequest, new
                                        {
                                            message = "Incomplete Address."
                                        });

                                address.Line = infoModel.Line;
                                address.Barangay = infoModel.Barangay;
                                address.City = infoModel.City;
                                address.Country = infoModel.Country;

                                break;
                            }
                        /// update owner's contact number
                        case "contact":
                            {
                                var contact = db.owner_contact
                                    .Single(p => p.UserId.Equals(userId));

                                if (contact == null) return BadRequest();

                                infoModel.Contact = infoModel.Contact.Trim();

                                if (string.IsNullOrEmpty(infoModel.Contact))
                                    return Content(HttpStatusCode.BadRequest, new
                                    {
                                        message = "Empty Contact Number."
                                    });

                                contact.Contact = infoModel.Contact;

                                break;
                            }
                        /// update owner's email
                        case "email":
                            {
                                var credential = db.account_credential
                                    .Single(p => p.Id.Equals(userId));

                                if (credential == null) return BadRequest();
                                credential.Email = infoModel.Email;
                                break;
                            }
                        /// update owner's password
                        case "password":
                            {
                                var credential = db.account_credential
                                    .Single(p => p.Id.Equals(userId));

                                if (credential == null) return Content(HttpStatusCode.NotFound, new
                                {
                                    message = "You haven't added one yet."
                                });

                                // check if current password if correct
                                bool isMatched = await PasswordManager.IsMatchedAsync(infoModel.CurrentPassword, credential.Password);

                                if (!isMatched)
                                    return Content(HttpStatusCode.BadRequest, new {
                                        message = "Password is invalid."
                                    });

                                credential.Password = await PasswordManager.HashAsync(infoModel.NewPassword);
                                break;
                            }
                        default:
                            return NotFound();
                    }

                    await db.SaveChangesAsync();

                }
                return Content(HttpStatusCode.OK, "Updated!");
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
