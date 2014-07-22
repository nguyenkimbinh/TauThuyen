namespace Pii.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<Pii.Models.PiiContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Pii.Models.PiiContext context)
        {
            //var roles = new string[] { "1", "2", "administrators" };
            //foreach (var item in roles) if (!Roles.RoleExists(item)) Roles.CreateRole(item);

            //var users = new List<string[]> { new string[] { "admin", "hoangtrum", "administrators", "wisky549@yahoo.com" } };
            //foreach (var item in users)
            //{
            //    if (!WebSecurity.UserExists(item[0]))
            //    {
            //        WebSecurity.CreateUserAndAccount(item[0], item[1], new { Email = item[3] });
            //        Roles.AddUserToRole(item[0], item[2]);
            //    }
            //}
        }
    }
}
