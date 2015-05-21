using AngularandCSS.Data;
using AngularandCSS.Service;
using AngularandCSS.Service.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AngularandCSS.Web.Controllers.Api
{
    
    public class UserController : ApiController
    {
        private DataContext db = new DataContext();
        private HttpResponseMessage _response { get; set; }

        private IAuthenticationManager AuthenticationManager { get { return Request.GetOwinContext().Authentication; } }

        private UserService _userService { get; set; }

        public UserController(UserService UserService)
        {
            _userService = UserService;
        }

        [Authorize]
        // GET api/<controller>
        public IEnumerable<User> Get()
        {
            return db.Users.OrderBy(a => a.UserName).ToList();
        }

        [Authorize]
        // GET api/<controller>/5
        public User Get(string id)
        {
            return db.Users.Where(a => a.Id == id).SingleOrDefault();
        }

        [Route("api/login")]
        public async Task<HttpResponseMessage> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Data.User user = await _userService.GetUserFromViewModel(model);
                if (user != null)
                {
                    if (await _userService.SignIn(user, false, AuthenticationManager))
                    {
                        return _response = Request.CreateResponse(HttpStatusCode.OK);
                    }
                    return _response = Request.CreateResponse(HttpStatusCode.Forbidden, "This account requires activation.  A new activation email has been sent.");
                }
                return _response = Request.CreateResponse(HttpStatusCode.NotFound, "Invalid username / password given.");
            }
            return _response = Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [Route("api/logout")]
        [HttpGet]
        [Authorize]
        public async Task<HttpResponseMessage> Logout()
        {
            await _userService.SignOut(AuthenticationManager);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Route("api/register")]
        public async Task<HttpResponseMessage> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                RegistrationResultViewModel result = await _userService.Register(model);
                if (result.Result.Succeeded)
                {
                    //await _userService.SignIn(result.User, false, AuthenticationManager);
                    return _response = Request.CreateResponse(HttpStatusCode.OK, "User " + model.UserName + " created successfully.  An activation email has been sent to " + model.Email + ".");
                }
                return _response = Request.CreateResponse(HttpStatusCode.NotFound, result.Result.Errors);
            }
            return _response = Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [Route("api/recover")]
        [HttpGet]
        public async Task<HttpResponseMessage> Recover(string email)
        {
            if(!string.IsNullOrEmpty(email))
            {
                Data.User user = await _userService.GetUserFromEmail(email);
                if (user != null)
                {
                    await _userService.SendPasswordRecoveryEmail(user);
                    return _response = Request.CreateResponse(HttpStatusCode.OK, "Recovery email sent.  The request will expire in 1 day.");
                }
                return _response = Request.CreateResponse(HttpStatusCode.NotFound, "No user with the given email exists.");
            }
            return _response = Request.CreateResponse(HttpStatusCode.BadRequest, "No valid email given.");
        }

        [Route("api/recover/request")]
        [HttpGet]
        public async Task<HttpResponseMessage> RecoverRequest(string password, string userID, string recoveryToken)
        {
            if(!string.IsNullOrEmpty(password))
            {
                IdentityResult result = await _userService.CheckPasswordValidity(password);
                if (result.Succeeded)
                {
                    if (await _userService.SetUserPassword(userID, password, recoveryToken))
                    {
                        return _response = Request.CreateResponse(HttpStatusCode.OK, "Your password has been reset.");
                    }
                    return _response = Request.CreateResponse(HttpStatusCode.Forbidden, "Your password could not be reset.");
                }
                return _response = Request.CreateResponse(HttpStatusCode.BadRequest, result.Errors);
            }
            return _response = Request.CreateResponse(HttpStatusCode.BadRequest, "No password given.");
        }

        //[Route("api/delete")]
        //[HttpPost]
        //public async Task<HttpResponseMessage> DeleteUser([FromBody] LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Data.User user = await _userService.GetUserFromViewModel(model);
        //        if(user != null)
        //        {
        //            bool deleted = await _userService.DeleteUser(user);
        //            if(deleted)
        //            {
        //                return new HttpResponseMessage(HttpStatusCode.OK)
        //                {
        //                    Content = new StringContent("User " + model.UserName + " deleted successfully.")
        //                };
        //            }
        //            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
        //            {
        //                Content = new StringContent("User " + model.UserName + " could not be deleted.")
        //            };
        //        }
        //        return new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent("Invalid username / password given.")
        //        };
        //    }
        //    return new HttpResponseMessage(HttpStatusCode.BadRequest)
        //    {
        //        Content = new StringContent("Invalid username / password given.")
        //    };
        //}
    }
}