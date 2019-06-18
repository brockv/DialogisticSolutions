using Dialogistic.DAL;
using Dialogistic.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Stripe;
using System.Configuration;

[assembly: OwinStartupAttribute(typeof(Dialogistic.Startup))]
namespace Dialogistic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
            StripeConfiguration.SetApiKey("SecretKey");
        }


        /// <summary>
        /// Creates the "Administrator" and "Standard" user account roles, and a single super user.
        /// </summary>
        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            DialogisticContext db = new DialogisticContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Create the "Super Admin" role and a default Super Admin user
            if (!roleManager.RoleExists("SuperAdmin"))
            {
                // Create the "SuperAdmin" role
                var role = new IdentityRole
                {
                    Name = "SuperAdmin"
                };
                roleManager.Create(role);

                // Create the "master" user who will maintain the website
                var user = new ApplicationUser
                {
                    FirstName = "Admin",
                    LastName = "",
                    UserName = ConfigurationManager.AppSettings["SUPER_ADMIN_USERNAME"],
                    Email = "",
                    EmailConfirmed = true,
                    LockoutEnabled = true
                };                

                // Set the password and attempt to create the user
                string userPWD = ConfigurationManager.AppSettings["SUPER_ADMIN_PASSWORD"];
                var defaultUser = UserManager.Create(user, userPWD);

                // Add the "master" user account to the Administrator role
                if (defaultUser.Succeeded)
                {
                    var defaultAdmin = UserManager.AddToRole(user.Id, "SuperAdmin");
                }
            }



            // Create the "Administrator" role
            if (!roleManager.RoleExists("Administrator"))
            {
                // Create the "Administrator" role   
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                roleManager.Create(role);
            }


            //  Create the "Standard User" role    
            if (!roleManager.RoleExists("Standard"))
            {
                var role = new IdentityRole
                {
                    Name = "Standard"
                };
                roleManager.Create(role);
            }

            // inserted test code for creating users here.
            

        }
    }
}
