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
    [Authorize]
    public class UserController : ApiController
    {
        private DataContext db = new DataContext();

        private UserService _userService { get; set; }

        public UserController(UserService UserService)
        {
            _userService = UserService;
        }

        // GET api/<controller>
        public IEnumerable<User> Get()
        {
            return db.Users.OrderBy(a => a.UserName).ToList();
        }

        // GET api/<controller>/5
        public User Get(string id)
        {
            return db.Users.Where(a => a.Id == id).SingleOrDefault();
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
    }
}