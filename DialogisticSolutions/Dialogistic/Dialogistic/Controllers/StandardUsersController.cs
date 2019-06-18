using Dialogistic.DAL;
using Dialogistic.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System;
using System.Configuration;
using System.Net;
using SendGrid;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Web.Configuration;
using Dialogistic.Abstract;
using Dialogistic.Concrete;

namespace Dialogistic.Controllers
{
    [Authorize(Roles = "Standard")]
    public class StandardUsersController : Controller
    {
        #region Setup
        private ApplicationUserManager _userManager;

        private IUnitOfWork unitOfWork;

        public StandardUsersController()
        {
            unitOfWork = new UnitOfWork(new DialogisticContext());
        }

        public StandardUsersController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        #endregion

        /// <summary>
        /// Loads the Dashboard for Standard Users.
        /// </summary>
        /// <returns>The Dashboard view for Standard Users.</returns>
        [HttpGet]
        public ActionResult Dashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = User.Identity.GetUserId();
                UserProfile standardUser = unitOfWork.Profiles.Find(x => x.UserID == currentUser).First();
                standardUser.CallsRemaining = unitOfWork.Assignments.Find(x => x.CallerID == standardUser.UserID).Count();

                // this calculate the sum of the donations obtained by users (callers)
                // and stores total team donation amount in a ViewBag structure.
                ViewBag.GroupTotal = unitOfWork.Profiles.GetUserProfiles().Sum(x => x.DonationsRaised);

                return View(standardUser);
            }

            return RedirectToAction("Login", "Account");
        }


        public ActionResult HelpPage()
        {
            return View();
        }

        #region Reports
        /// <summary>
        /// Gets the current yearly gifts by month 
        /// </summary>
        /// <returns>Json object with months and sum of gifts during said months</returns>
        public JsonResult CurrentYearlyReport()
        {
            GiftReports report = new GiftReports();
            return Json(report.YearlyReport(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the current yearly gifts by month 
        /// </summary>
        /// <returns>Json object with months and sum of gifts during said months</returns>
        public JsonResult CurrentMonthlyReport()
        {
            GiftReports report = new GiftReports();
            return Json(report.MonthlyReport(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the current yearly gifts by month 
        /// </summary>
        /// <returns>Json object with months and sum of gifts during said months</returns>
        public JsonResult CurrentDailyReport()
        {
            GiftReports report = new GiftReports();
            return Json(report.DailyReport(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Calls
        /// <summary>
        /// Shows the list of Constituents a Caller is responsible for calling.
        /// </summary>
        /// <returns>The view with any call assignments assigned to a Caller.</returns>
        [HttpGet]
        public ActionResult ViewCallList()
        {
            var currentUser = User.Identity.GetUserId();
            UserProfile standardUser = unitOfWork.Profiles.Find(x => x.UserID == currentUser).First();
            List<CallAssignment> callAssignments = unitOfWork.Assignments.Find(x => x.CallerID == standardUser.UserID).ToList();

            return View(callAssignments);
        }

        /// <summary>
        /// Removes a Call Assigment associated with the given Constituent ID.
        /// </summary>
        /// <param name="id">The ID of the Constituent whos Call Assignment we want to remove</param>
        /// <returns>The view with the user's Call Assignments on success, Error view otherwise.</returns>
        public virtual ActionResult RemoveCallAssignment(int? id)
        {
            // Check if the id passed in is null
            if (id != null)
            {
                // Look up this Constituent using the given ID, and check if it's null
                Constituent constituent = unitOfWork.Constituents.Find(x => x.ConstituentID == id).FirstOrDefault();
                if (constituent != null)
                {
                    // Lookup the Call Assignment associated with this Constituent, and check if it's null
                    CallAssignment assignment = unitOfWork.Assignments.Find(x => x.ConstituentID == id).FirstOrDefault();
                    if (assignment != null)
                    {
                        // Remove the call assignment from the database, and save the changes
                        unitOfWork.Assignments.Remove(assignment);
                        unitOfWork.Complete();

                        return View("ViewCallList");
                    }
                }
            }

            return View("Error");
        }

        /// <summary>
        /// Looks up a Constituent using the given id.
        /// </summary>
        /// <param name="id">The id of the Constituent to look up.</param>
        public bool GetConstituentID(int? id)
        {
            // Check if the given id is null
            if (id != null)
            {
                // Attempt to look up the Constituent using the given id, and send the email if we find them
                Constituent constituent = unitOfWork.Constituents.Find(x => x.ConstituentID == id).FirstOrDefault();
                if (constituent != null)
                {
                    // Send a 'Thank you' email to the Constituent
                    var user = UserManager.FindById(User.Identity.GetUserId());
                    SendThankYouEmail(user, constituent);
                    return true;
                }                
            }
            return false;
        }
        #endregion

        #region FollowUpEmail
        /// <summary>
        /// Sends a 'thank you' email to the given Constituent.
        /// </summary>
        /// <param name="constituent">The Constituent to send the email to.</param>
        /// <returns>The task, whether it succeeded or failed.</returns>
        public virtual Task SendThankYouEmail(ApplicationUser user, Constituent constituent)
        {
            return Task.Run(() =>
            {
                if (constituent != null)
                {
                    var userProfile = unitOfWork.Profiles.Find(x => x.UserID == user.Id).FirstOrDefault();

                    string apiKey = WebConfigurationManager.AppSettings["SENDGRID_API_KEY"];
                    var client = new SendGridClient(apiKey);
                    var msg = new SendGridMessage()
                    {
                        From = new EmailAddress(userProfile.Email, userProfile.FullName),
                        Subject = "Thank you for your recent gift!",
                        PlainTextContent = "Thank you taking the time to speak with us over the phone, and for your recent gift!" +
                        "                                                       " +
                        "You did a good thing, and should feel good. Good? Good.",
                        HtmlContent = "<h4>Thank you taking the time to speak with us over the phone, and for your recent gift!</h4>" +
                        "<p>You did a good thing, and should feel good. Good? Good.</p>"
                    };

                    msg.AddTo(new EmailAddress("bvance17@mail.wou.edu", "Mr. Brock Vance"));
                    Task<Response> response = client.SendEmailAsync(msg);                    
                }
            });            
        }
        #endregion

        #region More Setup
        /// <summary>
        /// Loads the site overlay for Standard Users.
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult _SiteOverlay()
        {
            var currentUserID = User.Identity.GetUserId();
            if (currentUserID != null)
            {
                UserProfile user = unitOfWork.Profiles.Find(x => x.UserID.Equals(currentUserID)).FirstOrDefault();

                return PartialView(user);
            }

            return PartialView();
        }

        /// <summary>
        /// Allows Standard Users to log out of their account from the sidebar link.
        /// </summary>
        /// <returns>Sends the user to the log in view after logging them out.</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }
        #endregion
    }
}