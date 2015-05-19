using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularandCSS.Service.ViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool RememberMe { get; set; }
        public string Password { get; set; }
    }
}
