using PetParadise.Extras;
using PetParadise.Extras.Error;
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

namespace PetParadise.Controllers.ApiControllers
{
    public class FeedController : ApiController
    {
        // get posts according to country
        // get posts according to city
        // get posts according to barangay
        [Authorize]
        [Route("posts")]
        [HttpGet]
        public IHttpActionResult getPosts(string filter)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = identity.Claims.First(c => c.Type.Equals("userId")).Value;

                using (MainDBEntities db = new MainDBEntities())
                {
                    var userAddress = db.owner_address
                                         .Single((a) => a.Id.Equals(userId));
                    string basis = "";
                    switch (filter)
                    {
                        case "country":
                            {
                                basis = userAddress.Country;
                                var s = db.owner_profile
                                        .Where(owner => owner.owner_address.Country.Equals(basis))
                                        .SelectMany(i => i.pet_profile)
                                        .SelectMany(i => i.profile_post)
                                        .Select(i => new PostModel()
                                        {
                                            Id = i.Id,
                                            Content = i.PostContent,
                                            ProfileId = i.ProfileId,
                                            CreatedAt = i.PostCreationDate
                                        })
                                        .OrderBy(i => i.CreatedAt)
                                        .ToList();

                                return Ok(s);
                            }

                        case "barangay":
                            {
                                basis = userAddress.Barangay;
                                var s = db.owner_profile
                                        .Where(owner => owner.owner_address.Barangay.Equals(basis))
                                        .SelectMany(i => i.pet_profile)
                                        .SelectMany(i => i.profile_post)
                                        .Select(i => new PostModel()
                                        {
                                            Id = i.Id,
                                            Content = i.PostContent,
                                            ProfileId = i.ProfileId,
                                            CreatedAt = i.PostCreationDate
                                        })
                                        .OrderBy(i => i.CreatedAt)
                                        .ToList();

                                return Ok(s);
                            }
                        case "city":
                            {
                                basis = userAddress.City;
                                var s = db.owner_profile
                                        .Where(owner => owner.owner_address.City.Equals(basis))
                                        .SelectMany(i => i.pet_profile)
                                        .SelectMany(i => i.profile_post)
                                        .Select(i => new PostModel()
                                        {
                                            Id = i.Id,
                                            Content = i.PostContent,
                                            ProfileId = i.ProfileId,
                                            CreatedAt = i.PostCreationDate
                                        })
                                        .OrderBy(i => i.CreatedAt)
                                        .ToList();

                                return Ok(s);
                            }
                        default:
                            {
                                basis = userAddress.Country;
                                var s = db.owner_profile
                                        .Where(owner => owner.owner_address.Country.Equals(basis))
                                        .SelectMany(i => i.pet_profile)
                                        .SelectMany(i => i.profile_post)
                                        .Select(i => new PostModel()
                                        {
                                            Id = i.Id,
                                            Content = i.PostContent,
                                            ProfileId = i.ProfileId,
                                            CreatedAt = i.PostCreationDate,
                                            LikesCount = i.profile_post_like.Count(),
                                            CommentsCount = i.profile_post_comment.Count()
                                        })
                                        .OrderBy(i => i.CreatedAt)
                                        .ToList();

                                return Ok(s);
                            }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return InternalServerError();
            }
        }


        // add post
        [Authorize]
        [Route("post/add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPost(PostModel post) {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = identity.Claims.First(c => c.Type.Equals("userId")).Value;
                
                // check if post is from its owner
                using(MainDBEntities db = new MainDBEntities())
                {
                    var profile = db.owner_profile
                                    .Single(u => userId.Equals(u.Id))
                                    .pet_profile
                                    .Single(p => p.Id.Equals(post.ProfileId));

                    if(profile == null) return new HttpErrorContent(Request,
                        HttpStatusCode.BadRequest,
                        Extras.Error.HttpError.InvalidSession);


                    var postId = await new UID(IdSize.SHORT).GenerateIdAsync();
                    var profilePost = new profile_post() {
                        Id = postId,
                        PostContent = post.Content,
                        ProfileId = post.ProfileId,
                        PostCreationDate = DateTime.UtcNow
                    };

                    profile.profile_post.Add(profilePost);
                    await db.SaveChangesAsync();

                    var returnPost = db.profile_post
                                    .Select(p => new
                                    {
                                        p.Id,
                                        p.PostContent,
                                        p.PostCreationDate
                                    })
                                    .Single(p => p.Id.Equals(profilePost.Id));
                    return Ok(returnPost);                
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

        // remove post

        // get post (id)
        [Route("post/")]
        [HttpGet]
        public IHttpActionResult GetPost(string id) {
            try
            {
                //ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                //var userId = identity.Claims.First(c => c.Type.Equals("userId")).Value;
                using (MainDBEntities db = new MainDBEntities())
                {
                    var post = db.profile_post
                                .Select(i => new {
                                    i.Id,
                                    i.ProfileId,
                                    i.pet_profile.Name,
                                    i.PostContent,
                                    i.PostCreationDate,
                                    LikesCount = i.profile_post_like.Count(),
                                    CommentsCount = i.profile_post_comment.Count(),
                                    Comments = i.profile_post_comment.Select(j => new {
                                        j.Id,
                                        j.pet_profile.Name,
                                        j.ProfileId,
                                        j.PostId,
                                        j.CommentContent
                                    })
                                })
                                .Single(i => i.Id.Equals(id));

                  return Ok(post);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // comment in a post (id)
        [Authorize]
        [Route("post/comment")]
        [HttpPost]
        public async Task<IHttpActionResult> AddComment(PostModel comment) {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = identity.Claims.First(c => c.Type.Equals("userId")).Value;

                // check if comment is from its owner
                using (MainDBEntities db = new MainDBEntities())
                {
                    var profile = db.owner_profile
                                    .Single(u => userId.Equals(u.Id))
                                    .pet_profile
                                    .Single(p => p.Id.Equals(comment.ProfileId));

                    if (profile == null) return new HttpErrorContent(Request,
                         HttpStatusCode.BadRequest,
                         Extras.Error.HttpError.InvalidSession);

                    // get the post
                    var post = db.profile_post.Single(p => p.Id.Equals(comment.Id));

                    if(post == null) return new HttpErrorContent(Request,
                         HttpStatusCode.BadRequest,
                         Extras.Error.HttpError.InvalidSession);

                    var commentId = await new UID(IdSize.SHORT).GenerateIdAsync();
                    var postComment = new profile_post_comment()
                    {
                        Id = commentId,
                        ProfileId = profile.Id,
                        PostId = comment.Id,
                        CommentContent = comment.Content,
                        CommentCreationDate = comment.CreatedAt
                    };

                    post.profile_post_comment.Add(postComment);
                    await db.SaveChangesAsync();

                    var returnComment = db.profile_post_comment
                                    .Select(p => new
                                    {
                                        p.Id,
                                        p.pet_profile.Name,
                                        p.PostId,
                                        p.ProfileId,
                                        p.CommentContent,
                                        p.CommentCreationDate
                                    })
                                    .Single(p => p.Id.Equals(postComment.Id));
                    return Ok(returnComment);
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


        // like a post
        [Authorize]
        [Route("post/like")]
        [HttpPost]
        public async Task<IHttpActionResult> Like(string id, string petid)
        {
            try
            {
                using (MainDBEntities db = new MainDBEntities())
                {
                    var isValid = db.pet_profile.Any(i => i.Id.Equals(petid));

                    if (!isValid) return new HttpErrorContent(Request,
                        HttpStatusCode.BadRequest, Extras.Error.HttpError.InvalidSession);

                    var post = db.profile_post.Single(i => i.Id.Equals(id));

                    if (post == null) return new HttpErrorContent(Request,
                         HttpStatusCode.BadRequest, Extras.Error.HttpError.InvalidSession);

                    profile_post_like newLike = new profile_post_like()
                    {
                        Id = await new UID(IdSize.SHORT).GenerateIdAsync(),
                        PostId = id,
                        ProfileId = petid
                    };

                    post.profile_post_like.Add(newLike);
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
                Debug.WriteLine(e.StackTrace);
                return InternalServerError();
            }
        }

        [Authorize]
        [Route("post/unlike")]
        [HttpPost]
        public async Task<IHttpActionResult> Unlike(string id, string petid)
        {
            try
            {
                using (MainDBEntities db = new MainDBEntities())
                {
                    var postLikes = db.profile_post_like
                                        .Single(p => p.PostId.Equals(id) && p.ProfileId.Equals(petid));
                    if(postLikes == null) return new HttpErrorContent(Request,
                         HttpStatusCode.BadRequest, Extras.Error.HttpError.InvalidSession);
                    db.profile_post_like.Remove(postLikes);
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
                Debug.WriteLine(e.StackTrace);
                return InternalServerError();
            }
        }
    }
}
