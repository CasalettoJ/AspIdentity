using AngularandCSS.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularandCSS.Service
{
    public class UserService : ServiceBase
    {
        private UserManager<User> _userManager { get; set; }
        private RoleManager<IdentityRole> _roleManager { get; set; }

        public UserService( DataContext dataContext) : base(dataContext)
        {
            _userManager = new UserManager<User>(new UserStore<User>(dataContext));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dataContext));
        }
    }
}
