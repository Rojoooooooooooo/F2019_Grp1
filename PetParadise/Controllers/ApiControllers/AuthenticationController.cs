using PetParadise.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using PetParadise.Extras.Extensions.JwtSecurity;
using System.Threading.Tasks;
using System.Web.Http;
using PetParadise.Models;
using PetParadise.Models.Body;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Diagnostics;
using PetParadise.Extras.Extensions.HttpRequestHeaders;
using System.Data.Entity;
using PetParadise.Extras.Extensions.Models;
using PetParadise.Extras.Error;

namespace PetParadise.Controllers.ApiControllers
{   
    public class AuthenticationController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("auth/signup")]
        public async Task<IHttpActionResult> CreateAccount(account_credential newAcc)
        {
            string hashedPassword = "", sessionId = "",
                sessionToken = "";
            SessionManager session = null;
            login_sessions loginSession = null;
            UID uid = new UID(IdSize.SHORT);

            try
            {
                using (MainDBEntities db = new MainDBEntities())
                {
                    bool userExists = await db.account_credential
                                    .AnyAsync(u => u.Username.Equals(newAcc.Username) || u.Email.Equals(newAcc.Email));

                    if (userExists) return new HttpErrorContent(Request, 
                        HttpStatusCode.BadRequest, 
                        Extras.Error.HttpError.UserExists);

                    string accountId = await uid.GenerateIdAsync();
                    newAcc.Id = accountId;
                    hashedPassword = await PasswordManager.HashAsync(newAcc.Password);
                    newAcc.Password = hashedPassword;

                    session = new SessionManager(newAcc.Id, newAcc.Username, newAcc.AccountTypeId);

                    sessionId = await uid.GenerateIdAsync();
                    sessionToken = await session.TokenHandlerAsync(SessionType.SESSION);

                    loginSession = new login_sessions()
                    {
                        Id = sessionId, // unique id
                        AccountId = newAcc.Id, // use user id as foreign key
                        Token = sessionToken,
                        ExpiresAt = DateTime.Now.AddDays(30)
                    };


                    newAcc.login_sessions.Add(loginSession);
                    db.account_credential.Add(newAcc);
                    await db.SaveChangesAsync();

                    var response = Request.CreateResponse(HttpStatusCode.Created, new
                    {
                        userId = newAcc.Id,
                        accountTypeId = newAcc.AccountTypeId,
                        session = loginSession.Token
                    });

                    var cookie = new CookieHeaderValue("session_token", sessionToken);
                    cookie.Domain = Request.RequestUri.Host;
                    cookie.Path = "/";
                    cookie.HttpOnly = true;
                    cookie.Expires = Expiration.MONTH;
                    cookie.Path += "; SameSite=Strict"; // add samesite constraint
                    response.Headers.AddCookies(new CookieHeaderValue[] { cookie });


                    return ResponseMessage(response);
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
                Debug.WriteLine(e.StackTrace);
                return InternalServerError();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("auth/login")]
        public async Task<IHttpActionResult> Login(LoginModel userLogin)
        {
            login_sessions loginSession = null;
            account_credential user = null;
            string sessionId = "", sessionToken = "";
            SessionManager session = null;
            UID uid = new UID(IdSize.SHORT);

            try
            {
                using (MainDBEntities db = new MainDBEntities())
                {
                    user = await db.account_credential
                             .FirstOrDefaultAsync(u => u.Username.Equals(userLogin.Username));


                    if (user == null)
                        return new HttpErrorContent(Request, 
                            HttpStatusCode.BadRequest, 
                            Extras.Error.HttpError.LoginAuthError);

                    bool hasProfile = await db.owner_profile
                                        .AnyAsync(u => u.Id.Equals(user.Id));

                    // if password doesn't match
                    if (!await PasswordManager.IsMatchedAsync(userLogin.Password, user.Password))
                        return new HttpErrorContent(Request,
                            HttpStatusCode.BadRequest, 
                            Extras.Error.HttpError.LoginAuthError);



                    // generate new token 
                    session = new SessionManager(user.Id, user.Username, user.AccountTypeId);
                    sessionId = await uid.GenerateIdAsync();
                    sessionToken = await session.TokenHandlerAsync(SessionType.SESSION);

                    loginSession = new login_sessions()
                    {
                        Id = sessionId, // unique id
                        AccountId = user.Id, // use user id as foreign key
                        Token = sessionToken,
                        ExpiresAt = DateTime.Now.AddDays(30)
                    };



                    user.login_sessions.Add(loginSession);
                    await db.SaveChangesAsync();

                    var response = Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        userId = user.Id,
                        accountTypeId = user.AccountTypeId,
                        HasProfile = hasProfile,
                        session = loginSession.Token
                    });

                    var cookie = new CookieHeaderValue("session_token", sessionToken)
                    {
                        Domain = Request.RequestUri.Host,
                        Path = "/; SameSite=Strict",
                        Expires = Expiration.MONTH,
                        HttpOnly = true,
                        Secure = true
                    };

                    response.Headers.AddCookies(new CookieHeaderValue[] { cookie });

                    return ResponseMessage(response);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return InternalServerError();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("auth/logout")]
        public IHttpActionResult Logout()
        {
            try
            {
                var headers = Request.Headers;
                if (!headers.HasSessionTokenHeader())
                    return Ok("Where is your session header?"); // return anyway

                JwtToken token = new JwtToken(headers.GetSessionToken(),
                                                new SessionManager().CreateValidationParameters(SessionType.SESSION));

                if (token == null)
                    return Ok("Invalid session."); // logout anyway

                PayloadModel payload = token.GetPayload();

                // delete session token if exists
                using (MainDBEntities db = new MainDBEntities())
                {
                    // get session from db
                    login_sessions activeSession = db.login_sessions
                                           .Single(s => s.Token.Equals(token.Value));
                    if (activeSession == null)
                        return Ok("Session not found in database.");

                    db.login_sessions.Remove(activeSession);
                    db.SaveChanges();
                }

                var response = Request.CreateResponse(HttpStatusCode.OK, "If you reached here, your session is now removed from the database.");

                var cookie = new CookieHeaderValue("session_token", "")
                {
                    Domain = Request.RequestUri.Host,
                    Path = "/; SameSite=Strict",
                    HttpOnly = true,
                    Expires = new DateTime(1960, 01, 01),
                    Secure = true
                };

                response.Headers.AddCookies(new CookieHeaderValue[] { cookie });

                return ResponseMessage(response);
            }
            catch (Exception)
            {
                return Ok("Your session is either invalid or something went wrong in our server.");
            }
        }

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("auth/refresh")]
        //public async Task<IHttpActionResult> GetAccessToken()
        //{
        //    try
        //    {
        //        var headers = Request.Headers;
        //        // check if session header exists
        //        if (!headers.HasSessionTokenHeader())
        //            return new HttpErrorContent(Request,
        //                HttpStatusCode.Forbidden,
        //                Extras.Error.HttpError.InvalidSession);

        //        // check if session is valid
        //        JwtToken token = new JwtToken(headers.GetSessionToken(),
        //                                new SessionManager().CreateValidationParameters(SessionType.SESSION));
        //        if (token == null)
        //            return new HttpErrorContent(Request,
        //                HttpStatusCode.Forbidden,
        //                Extras.Error.HttpError.InvalidSession);

        //        // check if session is in database
        //        using (MainDBEntities db = new MainDBEntities())
        //        {
        //            var session = db.login_sessions
        //                            .Where(s => s.Token.Equals(token.Value))
        //                            .First();

        //            if (session == null)
        //                return new HttpErrorContent(Request,
        //                HttpStatusCode.Forbidden,
        //                Extras.Error.HttpError.InvalidSession);

        //        }

        //        // reuse session userId and username for access token generation
        //        PayloadModel payload = await token.GetPayloadAsync();

        //        string sessionToken = await new SessionManager(payload.UserId, payload.Username)
        //                                .TokenHandlerAsync(SessionType.SESSION);


        //        var response = Request.CreateResponse(HttpStatusCode.OK, "Session token is granted.");

        //        var cookie = new CookieHeaderValue("session_token", sessionToken)
        //        {
        //            Domain = Request.RequestUri.Host,
        //            Path = "/; SameSite=Strict",
        //            HttpOnly = true,
        //            Expires = Expiration.MONTH,
        //            Secure = true
        //        };
        //        response.Headers.AddCookies(new CookieHeaderValue[] { cookie });

        //        return ResponseMessage(response);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e.InnerException);
        //        return InternalServerError();
        //    }
        //}


        //[AllowAnonymous]
        //[HttpPost]
        //[Route("dev/refresh")]
        //public async Task<IHttpActionResult> GetAccessTokenDevMode()
        //{
        //    try
        //    {
        //        var headers = Request.Headers;
        //        // check if session header exists
                
        //        if (!headers.HasSessionTokenHeader())
        //            return new HttpErrorContent(Request,
        //                HttpStatusCode.Forbidden,
        //                Extras.Error.HttpError.InvalidSession);

        //        // check if session is valid
        //        JwtToken token = new JwtToken(headers.GetSessionToken(),
        //                                new SessionManager().CreateValidationParameters(SessionType.SESSION));

        //        if (string.IsNullOrEmpty(token.Value))
        //            return new HttpErrorContent(Request,
        //                HttpStatusCode.Forbidden,
        //                Extras.Error.HttpError.InvalidSession);

        //        // check if session is in database
        //        using (MainDBEntities db = new MainDBEntities())
        //        {
        //            var session = db.login_sessions
        //                            .Where(s => s.Token.Equals(token.Value))
        //                            .First();

        //            if (session == null)
        //                return new HttpErrorContent(Request,
        //                HttpStatusCode.Forbidden,
        //                Extras.Error.HttpError.InvalidSession);
        //        }

        //        // reuse session userId and username for access token generation
        //        PayloadModel payload = await token.GetPayloadAsync();

        //        string sessionToken = await new SessionManager(payload.UserId, payload.Username)
        //                                .TokenHandlerAsync(SessionType.SESSION);


        //        var response = Request.CreateResponse(HttpStatusCode.OK, new
        //        {
        //            token = sessionToken
        //        });


        //        return ResponseMessage(response);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e.InnerException);
        //        return InternalServerError();
        //    }
        //}

        [Authorize]
        [HttpPost]
        [Route("auth/archive")]
        public async Task<IHttpActionResult> ArchiveAccount(LoginModel confirmLogin)
        {
            try
            {
                /// 1.) fetch data from db that matches id from token
                /// 2.) compare passwords
                /// 3.) if matches, archive account
                /// 4.) set account to archived
                /// 
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = identity.Claims.First(c => c.Type.Equals("userId")).Value;

                using (MainDBEntities db = new MainDBEntities())
                {
                    var userAcc = await db.account_credential.FirstAsync(u => u.Id.Equals(userId));
                    if (userAcc == null) return BadRequest();

                    bool isMatch = await PasswordManager.IsMatchedAsync(
                        confirmLogin.Password, userAcc.Password);

                    if (!isMatch) return BadRequest();

                    userAcc.ArchiveAccount();

                    await db.SaveChangesAsync();
                }

                return Ok();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return InternalServerError();
            }
        }


        [Route("auth/verify")]
        [HttpPost]
        public IHttpActionResult VerifySession()
        {
            try
            {
                var headers = Request.Headers;
                // check if session header exists
                if (!headers.HasSessionTokenHeader())
                    return new HttpErrorContent(Request,
                        HttpStatusCode.Forbidden,
                        Extras.Error.HttpError.InvalidSession);

                // check if session is valid
                JwtToken token = new JwtToken(headers.GetSessionToken(),
                                        new SessionManager().CreateValidationParameters(SessionType.SESSION));
                if (token.Value == null)
                    return new HttpErrorContent(Request,
                        HttpStatusCode.Forbidden,
                        Extras.Error.HttpError.InvalidSession);

                // check if session is in database
                using (MainDBEntities db = new MainDBEntities())
                {
                    var session = db.login_sessions
                                    .Where(s => s.Token.Equals(token.Value))
                                    .First();

                    if (session == null)
                        return new HttpErrorContent(Request,
                        HttpStatusCode.Forbidden,
                        Extras.Error.HttpError.InvalidSession);

                }

                return Content(HttpStatusCode.OK, new { authenticity = 100 });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return InternalServerError();
            }
        }
    }
}
