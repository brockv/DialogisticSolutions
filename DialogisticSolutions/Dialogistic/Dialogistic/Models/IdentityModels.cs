using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dialogistic.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        // Add the FirstName and LastName properties here in order to use them in the table
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here //

            // Add the FirstName field to the claims so we can use it other places
            //userIdentity.AddClaim(new Claim("FirstName", FirstName));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            // added this per brock's advice.
            Database.SetInitializer<IdentityDbContext>(null);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    /// <summary>
    /// Extension class for getting a logged in user's FirstName field.
    /// </summary>
    //public static class GenericPrincipalExtensions
    //{
    //    /// <summary>
    //    /// Helper method that gets an authenticated user's FirstName field via claims.
    //    /// </summary>
    //    /// <param name="user">The user to get the FirstName field from.</param>
    //    /// <returns>The value of the FirstName field, or an empty string.</returns>
    //    public static string GetFirstName(this IPrincipal user)
    //    {
    //        // Only proceed if the user is authenticated
    //        if (user.Identity.IsAuthenticated)
    //        {
    //            // Create a ClaimsIdentity in order to get access to the FirstName claim
    //            ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
    //            foreach (var claim in claimsIdentity.Claims)
    //            {
    //                // Check for the FirstName claim so we can grab the value
    //                if (claim.Type == "FirstName")
    //                {
    //                    // Return the value stored in FirstName
    //                    return claim.Value;
    //                }
                        
    //            }

    //            // If we got this far, something went wrong and we couldn't find the FirstName claim
    //            return "";
    //        }
    //        // The user isn't authenticated -- return an empty string
    //        else
    //        {
    //            return "";
    //        }                
    //    }
    //}
}