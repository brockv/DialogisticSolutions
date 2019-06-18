using Dialogistic.Models;
using Stripe;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using Dialogistic.DAL;
using System.Linq;

namespace Dialogistic.Controllers
{
    public class VoiceController : Controller
    {
        #region Setup
        // Instantiate an instance of our DB context file
        DialogisticContext db = new DialogisticContext();

        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public VoiceController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext));
        }

        /// <summary>
        /// Builds a view for a given Constituent where a Caller can call them from.
        /// Also, checks if donation type has been specified and if so,
        /// updates the call details list with the new call detail info.
        /// </summary>
        /// <param name="id">The ID of the Constituent to build the view for.</param>
        /// <returns>The view if the there's a Constituent associated with the given ID, HttpNotFound otherwise.</returns>
        [HttpGet]
        public ActionResult Index(int? id)
        {            
            // If id is null, return to this user's Call List
            if (id == null)
            {
                return RedirectToAction("ViewCallList", "StandardUsers");
            }

            // It wasn't null, so try and find a Constituent associated with it
            Constituent constituent = db.Constituents.Find(id);
            Session["constituentID"] = id;

            // If we can't find a Constituent with this id, return an error
            if (constituent == null)
            {
                return View("Error");
            }
            
            // Build lists of Call Logs and Gifts to display in the view
            ViewBag.CallLog = db.CallLogs.Where(x => x.ConstituentID == id).OrderBy(x => x.DateOfCall).ToList();
            ViewBag.Gifts = db.Gifts.Where(x => x.ConstituentID == id).OrderBy(x => x.CallLog.DateOfCall).ToList();

            // Return the view with the Consituent's information
            return View(constituent);
        }
        #endregion

        #region CallAssignments
        /// <summary>
        /// Gets the next Constituent that the Caller is responsible for calling, and reloads the
        /// view with their information, as well as fills out details of the current call and saves
        /// them in the database.
        /// </summary>
        /// <param name="id">The id of the *current* Constituent</param>
        /// <param name="donationType">The type of donation made by the Constituent</param>
        /// <param name="callOutcome">The outcome of the call, i.e., whether it was answered or not.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNextCallAssignment(int? id, string donationType, string callOutcome)
        {
            // If the id passed in is null for any reason, return the user to their Call List
            if (id == null)
            {
                return RedirectToAction("ViewCallList", "StandardUsers");
            }

            // Check if user input has been given on potential donation type
            if (!string.IsNullOrEmpty(donationType) || !string.IsNullOrEmpty(callOutcome))
            {
                // Create a new  gift entry
                var update = new Gift
                {
                    Constituent = db.Constituents.Find(id),
                    CallLog = new CallLog()
                };

                // Get the currently logged in user
                var user = User.Identity.GetUserId();

                ////////////////////////////////////////////////////////
                //                                                    //
                // Fill in the relevant attributes of the call record //
                //                                                    //
                ////////////////////////////////////////////////////////
                
                // Set the Caller and Constituent IDs
                update.CallLog.CallerID = user;
                update.CallLog.ConstituentID = id.GetValueOrDefault();

                // Set the date of the call
                DateTime dateOnly = DateTime.Now;
                update.CallLog.DateOfCall = dateOnly.Date;

                // Set CallAnswered and LineAvailable
                update.CallLog.CallAnswered = true;
                update.CallLog.LineAvailable = true;

                // Assign the gift type
                if (!string.IsNullOrEmpty(donationType))
                {
                    update.GiftType = donationType;
                }
                else
                {
                    update.GiftType = null;
                }

                // Assign the call outcome
                if (!string.IsNullOrEmpty(callOutcome))
                {
                    update.CallLog.CallOutcome = callOutcome;
                }
                else
                {
                    update.CallLog.CallOutcome = null;
                }

                // Deal with GiftAmount and Gift recipient later

                // Add the Gift and CallLog to the database, then save the changes
                db.Gifts.Add(update);
                db.CallLogs.Add(update.CallLog);
                db.SaveChanges();   
            }
            // Find and remove the Call Assignment that was just completed, and save the changes
            var userID = User.Identity.GetUserId();

            // Attempt to grab the next Call Assignment
            var remainingAssignments = db.CallAssignments.Where(c => c.CallerID == userID).ToList();
            Constituent nextConstituent = new Constituent();

            // Make sure there's actually another Call Assignment
            if (remainingAssignments != null && remainingAssignments.Count > 0)
            {
                // Two or more Call Assignments remaining for this user
                if (remainingAssignments.Count >= 2)
                {
                    CallAssignment nextCall = remainingAssignments.ElementAt(1);
                    nextConstituent = nextCall.Constituent;
                }
                // Exactly one Call Assignment remaining for this user
                else if (remainingAssignments.Count == 1)
                {
                    CallAssignment nextCall = remainingAssignments.ElementAt(0);
                    nextConstituent = nextCall.Constituent;
                }
                // There are no more Call Assigments for this user -- redirect to their empty Call List view
                else
                {
                    return RedirectToAction("ViewCallList", "StandardUsers");
                }
            }
            // There are no more Call Assigments for this user -- redirect to their empty Call List view
            else
            {
                return RedirectToAction("ViewCallList", "StandardUsers");
            }

            // Build lists of Call Logs and Gifts to display in the view
            ViewBag.CallLog = db.CallLogs.Where(x => x.ConstituentID == id).OrderBy(x => x.DateOfCall).ToList();
            ViewBag.Gifts = db.Gifts.Where(x => x.ConstituentID == id).OrderBy(x => x.CallLog.DateOfCall).ToList();

            // Return to the view with the next Constituent loaded
            return View("Index", nextConstituent);
        }
        #endregion

        #region Reports
        public JsonResult GetConstituentReport(int ID)
        {
            GiftReports report = new GiftReports();
            return Json(report.GetConstituentReport(ID), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region TwilioInfo
        /// <summary>
        /// Gets the necessary information from the form in order to have Twilio call
        /// a Constituent.
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpPost]
        //public ActionResult Index(string to)
        public ContentResult Index(string to)
        {
            // If there's a caller ID setup, grab it here
            var callerId = ConfigurationManager.AppSettings["TwilioCallerId"];

            // Instantiate a new VoiceResponse
            var response = new VoiceResponse();

            // If 'to' isn't empty or null, this is an outgoing call -- this should always have something
            if (!string.IsNullOrEmpty(to))
            {
                // Build the Dial object for placing the call
                var dial = new Dial(callerId: callerId);

                /* Wrap the phone number or Constituent name in the appropriate TwiML verb
                   by checking if the number given has only digits and format symbols */
                if (Regex.IsMatch(to, "^[\\d\\+\\-\\(\\) ]+$"))
                {
                    dial.Number(to);
                }
                else
                {
                    dial.Client(to);
                }

                // Append the caller ID
                response.Append(dial);
            }
            // 'to' was empty or null -- this is an incoming call (SHOULD NEVER HAPPEN)
            else
            {
                // Default voice playback when an incoming call is placed -- not going to be used
                response.Say("Thanks for calling!");
            }

            // Send the information to Twilio
            return Content(response.ToString(), "text/xml");
        }
        #endregion

        #region StripeCharge
        [HttpPost]
        public async Task<ActionResult> Charge(ChargeModel model)
        {
            var result = await MakeStripeCharge(model);
            if (result.Succeeded)
            {
                var currentPledge = model.PledgeAmount.ToString().FirstOrDefault();

                ViewBag.Message = $"The pledge of {model.PledgeAmount:C} was processed successfully.  Thank you!";
            }
            else
            {
                ViewBag.ErrorMessage = $"There was a problem processing your pledge";
            }
            return View("Index");
        }

        private async Task<StripeResult> MakeStripeCharge(ChargeModel model)
        {
            var secretKey = ConfigurationManager.AppSettings["StripeSecretKey"];
            var chargeSvc = new ChargeService(secretKey);
            var options = new ChargeCreateOptions();
            options.Amount = Convert.ToInt64(model.PledgeAmount * 100m);
            options.SourceId = model.StripeToken;
            options.Currency = "usd";
            options.Description = "Pledge";
            var charge = await chargeSvc.CreateAsync(options);
            return new StripeResult
            {
                Succeeded = charge.Status == "succeeded"
            };
        }
    }
    #endregion

    internal class StripeResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
