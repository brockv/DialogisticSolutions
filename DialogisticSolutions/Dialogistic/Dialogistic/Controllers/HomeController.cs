using System.Linq;
using System.Web.Mvc;
using Dialogistic.DAL;
using Dialogistic.Models;
using Microsoft.AspNet.Identity;

/// <summary>
/// Namespace for our HomeController.
/// </summary>
namespace Dialogistic.Controllers
{
    /// <summary>
    /// Our HomeController class derived off of Controller.
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        #region Setup
        private DialogisticContext db = new DialogisticContext();

        // Not used - simply redirects the user to the appropriate dashboard
        [AllowAnonymous]
        public ActionResult Index()
        {
            // Redirect the user based on the role they are in
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Administrator") || User.IsInRole("SuperAdmin"))
                {
                    return RedirectToAction("Dashboard", "Admins");
                }

                return RedirectToAction("Dashboard", "StandardUsers");
            }

            // If they aren't authenticated and somehow got this far, send them to the login page
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Loads the site overlay into the page.
        /// </summary>
        /// <returns>The overlay used across the site.</returns>
        [ChildActionOnly]
        public PartialViewResult _SiteOverlay()
        {
            // Determine who the current user is so we can load their information into the overlay
            var currentUserID = User.Identity.GetUserId();
            if (currentUserID != null)
            {
                UserProfile user = db.UserProfiles.Where(x => x.UserID.Equals(currentUserID)).FirstOrDefault();

                return PartialView(user);
            }

            // Something went wrong - return the overlay without the user info
            return PartialView("_SiteOverlay");
        }
        #endregion

        //[Authorize(Roles = "Administrator")]
        public ActionResult StyleGuide()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        //[Authorize(Roles = "Administrator")]
        public ActionResult PentestingReport()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

    }
}