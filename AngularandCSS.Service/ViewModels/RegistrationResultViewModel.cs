using AngularandCSS.Data;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularandCSS.Service.ViewModels
{
    public class RegistrationResultViewModel
    {
        public IdentityResult Result { get; set; }
        public User User { get; set; }
    }
}
