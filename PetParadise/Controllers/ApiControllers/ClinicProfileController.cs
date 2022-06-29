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
    public class ClinicProfileController : ApiController
    {

        [Authorize]
        [HttpPost]
        [Route("user/clinic")]
        public async Task<IHttpActionResult> AddClinicInformation(ClinicInformationModel clinicInput)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = identity.Claims.First(c => c.Type.Equals("userId")).Value;
                if (string.IsNullOrEmpty(clinicInput.FirstName))
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        message = "Empty First name."
                    });
                if (string.IsNullOrEmpty(clinicInput.LastName))
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        message = "Empty Last name."
                    });
                if (string.IsNullOrEmpty(clinicInput.Line) ||
                                   string.IsNullOrEmpty(clinicInput.Barangay) ||
                                   string.IsNullOrEmpty(clinicInput.City) ||
                                   string.IsNullOrEmpty(clinicInput.Country))
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        message = "Incomplete Address."
                    });

                if (string.IsNullOrEmpty(clinicInput.Contact))
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        message = "Empty Contact Number."
                    });


                clinicInput.ClinicName = clinicInput.ClinicName.Trim().ToTitleCase();
                clinicInput.FirstName = clinicInput.FirstName.Trim().ToTitleCase();
                clinicInput.MiddleName = clinicInput.MiddleName.Trim().ToTitleCase();
                clinicInput.LastName = clinicInput.LastName.Trim().ToTitleCase();

                clinicInput.Line = clinicInput.Line.Trim().ToUpper();
                clinicInput.Barangay = clinicInput.Barangay.Trim().ToUpper();
                clinicInput.City = clinicInput.City.Trim().ToUpper();
                clinicInput.Country = clinicInput.Country.Trim().ToUpper();

                clinicInput.Contact = clinicInput.Contact.Trim().ToTitleCase();

                using (MainDBEntities db = new MainDBEntities())
                {
                    clinic_contact contactNumber = new clinic_contact()
                    {
                        Id = await new UID(IdSize.SHORT).GenerateIdAsync(),
                        ClinicId = userId,
                        Contact = clinicInput.Contact
                    };

                    clinic_address address = new clinic_address()
                    {
                        Id = userId,
                        Line = clinicInput.Line,
                        Barangay = clinicInput.Barangay,
                        City = clinicInput.City,
                        Country = clinicInput.Country
                    };
                    clinic_profile profile = new clinic_profile()
                    {
                        Id = userId,
                        Name = clinicInput.ClinicName,
                        VetFirstName = clinicInput.FirstName,
                        VetMiddleName = clinicInput.MiddleName,
                        VetLastName = clinicInput.LastName,
                        clinic_address = address
                    };
                    profile.clinic_contact.Add(contactNumber);

                    db.clinic_profile.Add(profile);
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
        [Route("user/clinic")]
        public async Task<IHttpActionResult> UpdateClinic(ClinicInformationModel infoModel, [FromUri] string update)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = identity.Claims.First(c => c.Type.Equals("userId")).Value;

                using (MainDBEntities db = new MainDBEntities())
                {
                    /// updates specific areas using query update
                    switch (update)
                    {
                        /// Update basic information like name
                        /// in this case, we will update veterinarian's name
                        case "basic":
                            {
                                var profile = db.clinic_profile
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

                                profile.VetFirstName = infoModel.FirstName;
                                profile.VetMiddleName = infoModel.MiddleName;
                                profile.VetLastName = infoModel.LastName;

                                break;
                            }

                         /// Update the clinic's address 
                        case "address":
                            {
                                var address = db.clinic_address
                                    .Single(p => p.Id.Equals(userId));

                                if (address == null)
                                    return Content(HttpStatusCode.NotFound, new
                                    {
                                        message = "You haven't added one yet."
                                    });

                                infoModel.Line = infoModel.Line.Trim().ToUpper();
                                infoModel.Barangay = infoModel.Barangay.Trim().ToUpper();
                                infoModel.City = infoModel.City.Trim().ToUpper();
                                infoModel.Country = infoModel.Country.Trim().ToUpper();

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
                        /// Update the clinic contact number
                        case "contact":
                            {
                                var contact = db.clinic_contact
                                    .Single(p => p.ClinicId.Equals(userId));
                                
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
                        /// Update the clinic's email
                        case "email":
                            {
                                var credential = db.account_credential
                                    .Single(p => p.Id.Equals(userId));

                                if (credential == null) return BadRequest();
                                credential.Email = infoModel.Email;
                                break;
                            }
                        
                        /// update the clinic's password
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
                                    return Content(HttpStatusCode.BadRequest, new
                                    {
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