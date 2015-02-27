using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularandCSS.Data
{
    public class DataContext: DbContext
    {
        public DataContext() : base("DataContext")
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
