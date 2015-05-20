using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AngularandCSS.Web.StartUp))]
namespace AngularandCSS.Web
{
    public partial class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}