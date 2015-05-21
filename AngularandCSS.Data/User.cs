using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularandCSS.Data
{
    public class User : IdentityUser
    {
        public string Message { get; set; }
        public bool CustomEmailConfirmation { get; set; }
    }
}
