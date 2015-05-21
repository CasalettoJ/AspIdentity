using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularandCSS.Data
{
    public class EmailConfirmation
    {
        public int ID {get; set;}
        public string UserID { get; set; }
        public string ConfirmationToken { get; set; }
    }
}
