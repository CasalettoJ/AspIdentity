namespace AngularandCSS.Data.Migrations
{
    using DevOne.Security.Cryptography.BCrypt;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AngularandCSS.Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AngularandCSS.Data.DataContext context)
        {
            //string seed = BCryptHelper.GenerateSalt();
            //context.Users.AddOrUpdate(new User() { Username = "Test User", Salt = seed, Password = BCryptHelper.HashPassword("password", seed), Message = "This user was created or updated in the seed method in Confiiguration." });
            //context.Users.AddOrUpdate(new User() { Username = "JakeC", Salt = seed, Password = BCryptHelper.HashPassword("password", seed), Message = "This user was also created or updated in the seed method in Confiiguration." });
            //context.SaveChanges();
        }
    }
}
