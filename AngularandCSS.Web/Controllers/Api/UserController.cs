using AngularandCSS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AngularandCSS.Web.Controllers.Api
{
    public class UserController : ApiController
    {
        private DataContext db = new DataContext();
        // GET api/<controller>
        public IEnumerable<User> Get()
        {
            return db.Users.OrderBy(a => a.UserName);
        }

        // GET api/<controller>/5
        public User Get(string id)
        {
            return db.Users.Where(a => a.Id == id).SingleOrDefault();
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        } 

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}