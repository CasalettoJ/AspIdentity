using AngularandCSS.Data;
using AngularandCSS.Service;
using AngularandCSS.Service.ViewModels;
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
                User user = await _userService.GetUserFromViewModel(model);
                if (user != null)
                {
                    if (await _userService.SignIn(user, false, AuthenticationManager))
                    {
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                    return new HttpResponseMessage(HttpStatusCode.Forbidden);
                }
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [Route("api/logout")]
        [HttpGet]
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
                    await _userService.SignIn(result.User, false, AuthenticationManager);
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [Route("api/delete")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteUser([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Data.User user = await _userService.GetUserFromViewModel(model);
                if(user != null)
                {
                    bool deleted = await _userService.DeleteUser(user);
                    if(deleted)
                    {
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        //// POST api/<controller>
        //public async Task<HttpResponseMessage> Post([FromBody] RegisterViewModel model)
        //{
        //} 

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
            //  if (ModelState.IsValid)
            //{
            //    RegistrationResultViewModel result = await _userService.Register(model);
            //    if(result.Result.Succeeded)
            //    {
            //        //await _userService.SignIn(result.User, false, AuthenticationManager);
            //        return new HttpResponseMessage(HttpStatusCode.OK);
            //    }
            //    else
            //    {
            //        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            //        response.Content = new StringContent("Failed to create an account: " + result.Result.Errors.ToString());
            //        return response;
            //    }
            //}
            //else
            //{
            //    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            //    return response;
    }
}