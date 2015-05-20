using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AngularandCSS.Web.App_Start
{
    public static class AutofacConfig
    {
        public static void RegisterMVC()
        {
            var builder = new ContainerBuilder();
            var refAssemblies = System.Web.Compilation.BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();

            builder.RegisterAssemblyTypes(refAssemblies).InNamespace("AngularandCSS.Service").Where(r => r.Name.EndsWith("Service")).AsSelf().InstancePerRequest();
            builder.RegisterType(typeof(Data.DataContext)).InstancePerRequest();
            builder.RegisterControllers(typeof(Global).Assembly);
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.Register(c => new HttpContextWrapper(HttpContext.Current)).As<HttpContextBase>().InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public static System.Web.Http.Dependencies.IDependencyResolver RegisterWebAPI(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            var refAssemblies = System.Web.Compilation.BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();

            builder.RegisterAssemblyTypes(refAssemblies).InNamespace("AngularandCSS.Service").Where(r => r.Name.EndsWith("Service")).AsSelf().InstancePerRequest();
            builder.RegisterType(typeof(Data.DataContext)).InstancePerRequest();
            builder.RegisterControllers(typeof(Global).Assembly);
            builder.RegisterWebApiFilterProvider(config);
            builder.Register(c => new HttpContextWrapper(HttpContext.Current)).As<HttpContextBase>().InstancePerRequest();

            builder.RegisterApiControllers(typeof(AutofacConfig).Assembly);

            IContainer container = builder.Build();
            AutofacWebApiDependencyResolver resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;
            return resolver;
        }


    }
}