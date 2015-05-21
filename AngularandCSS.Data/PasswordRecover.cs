using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularandCSS.Data
{
    public class PasswordRecover
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string RecoveryToken { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
