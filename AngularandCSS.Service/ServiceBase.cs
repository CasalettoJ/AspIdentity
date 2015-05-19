using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularandCSS.Data;

namespace AngularandCSS.Service
{
    public class ServiceBase
    {
        protected DataContext _dataContext;

        public ServiceBase(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
