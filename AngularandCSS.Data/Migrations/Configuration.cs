namespace AngularandCSS.Data.Migrations
{
    using DevOne.Security.Cryptography.BCrypt;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<AngularandCSS.Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AngularandCSS.Data.DataContext context)
        {
            //string seed = BCryptHelper.GenerateSalt();
            //context.Users.AddOrUpdate(new User() { UserName = "Test User", SecurityStamp = seed, PasswordHash = BCryptHelper.HashPassword("password", seed), Message = "This user was created or updated in the seed method in Configuration." });
            //context.Users.AddOrUpdate(new User() { UserName = "JakeC", SecurityStamp = seed, PasswordHash = BCryptHelper.HashPassword("password", seed), Message = "This user was also created or updated in the seed method in Configuration." });
            //context.Roles.AddOrUpdate(new IdentityRole() { Name = "User" });
            //context.Roles.AddOrUpdate(new IdentityRole() { Name = "Admin" });
            //context.Roles.AddOrUpdate(new IdentityRole() { Name = "Moderator" });
            //context.Roles.AddOrUpdate(new IdentityRole() { Name = "Other" });

            context.SaveChanges();
        }
    }
}
