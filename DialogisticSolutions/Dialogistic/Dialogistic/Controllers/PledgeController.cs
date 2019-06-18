using Dialogistic.DAL;
using Dialogistic.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Twilio;
using Twilio.AspNet.Common;
using Twilio.AspNet.Mvc;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.Types;

namespace Stripe.Controllers
{
    public class PledgeController : TwilioController
    {
        #region Setup
        private DialogisticContext db = new DialogisticContext();

        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }

        /// <summary>
        /// Starting Pledge controller
        /// </summary>
        public PledgeController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext));
        }

        // GET: Pledge
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region SMS

        /// <summary>
        /// Sends an SMS message to a given Constituent's mobile number, if it exists.
        /// </summary>
        /// <param name="id">The id to use when looking up the Constituent.</param>
        /// <returns></returns>
        public ActionResult SendSMS(int? id)
        {
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber("15039830280");
            var from = new PhoneNumber(ConfigurationManager.AppSettings["TwilioPhoneNumber"]);

            var message = MessageResource.Create(
                to: to,
                from: from,
                body: "Test outgoing SMS.");

            return Content(message.Sid);
        }

        public ActionResult ReceiveSMS(SmsRequest request)
        {
            var response = new MessagingResponse();
            if (request.Body.ToUpper() == "DONATE")
            {
                response.Message("Please use the following secure link to make your donation: " +
                    "" +
                    "https://www.wou.edu/foundation/give-to-wou/");

                return TwiML(response);
            }

            
            response.Message("Unrecognized command. Please text 'DONATE' to place a donation.");

            return TwiML(response);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult MobileDonation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> MobileDonation(ChargeModel model)
        {
            var result = await MakeStripeCharge(model);
            if (result.Succeeded)
            {
                ViewBag.Message = $"The pledge of {model.PledgeAmount:C} was processed successfully.  Thank you!";
            }
            else
            {
                ViewBag.ErrorMessage = $"There was a problem processing your pledge";
            }
            return View();
        }

        #endregion

        #region PrintPledge
        public ActionResult Print()
        {

            List<PledgeVM> Pledges = db.Gifts.Where(p => p.GiftType.Equals("Pledge") && p.Printed.Equals(false))
                                                  .Select(c => new PledgeVM
                                                  {
                                                      Name = c.Constituent.PrimaryAddressee,
                                                      PAddress = c.Constituent.PreferredAddressLine1 + " " +
                                                                 c.Constituent.PreferredAddressLine2 + " " +
                                                                 c.Constituent.PreferredAddressLine3,
                                                      City = c.Constituent.PreferredCity,
                                                      State = c.Constituent.PreferredState,
                                                      Zip = c.Constituent.PreferredZIP,
                                                      PledgeAmmount = c.GiftAmount,
                                                      CallerName = db.UserProfiles.Where(x => x.UserID.Equals(c.CallLog.CallerID))
                                                                                  .Select(y => y.FullName).FirstOrDefault().ToString()
                                                  })
                                                  .OrderBy(x => x.Name)
                                                  .ToList();
            return View(Pledges);
        }

        /// <summary>
        /// Sets all current pledges in database to PRINTED status
        /// </summary>
        /// <returns>Redirects to Print page</returns>
        public ActionResult ClearPledges()
        {
            var pledges = db.Gifts.Where(p => p.GiftType.Equals("Pledge"));

            foreach(Gift p in pledges)
            {
                p.Printed = true;
            }
            db.SaveChanges();

            return RedirectToAction("Print");
        }
        #endregion

        #region Charge
        [HttpPost]
        public async Task<ActionResult> Charge(ChargeModel model, CallLog calls)
        {
            var result = await MakeStripeCharge(model);
            if (result.Succeeded)
            {

                var currentPledge = model.PledgeAmount;

                // constituent id passed
                int constituentID = Convert.ToInt16(Session["constituentID"].ToString());

                // returns entire constituent
                Constituent constituent = db.Constituents.SingleOrDefault(x => x.ConstituentID == constituentID);

                // Get current constituent ID and set as a string
                int callId = Convert.ToInt16(Session["constituentID"].ToString());

                // Get current caller to send to new CallDetails
                var currentUser = UserManager.FindById(User.Identity.GetUserId());
                var userProfile = db.UserProfiles.Where(x => x.UserID == currentUser.Id).FirstOrDefault();
                var caller = userProfile.UserID;
                
                if (constituent != null)
                {
                    // New call details with information
                    CallLog callInfo = new CallLog
                    {
                        ConstituentID = constituentID,
                        CallAnswered = true,
                        LineAvailable = true,
                        DateOfCall = DateTime.Now,
                        CallerID = caller,
                        CallOutcome = "Credit Card Donation",
                    };

                    // Create a new Gift entry
                    Gift giftInfo = new Gift
                    {
                        ConstituentID = constituentID,
                        CallID = callInfo.CallID,
                        GiftAmount = currentPledge,
                        GiftType = "Credit Card Donation",
                        GiftRecipient = "General",
                    };

                    // Set constituent status to retained
                    constituent.DonationStatus = "Retained";

                    db.CallLogs.Add(callInfo);
                    db.Gifts.Add(giftInfo);
                    
                    db.SaveChanges();
                
                    var callDetails = db.CallLogs.Where(x => x.ConstituentID == constituentID).ToList();
                    var gift = db.Gifts.Where(x => x.ConstituentID == constituentID).ToList();
                    
                    if (callDetails != null && callDetails.Count > 0)
                    {
                        CallLog callDetail = callDetails.FirstOrDefault(x => x.DateOfCall.ToShortDateString() == DateTime.Now.ToShortDateString());
                        if (callDetail == null)
                        {
                            gift[0].GiftAmount = model.PledgeAmount;
                            db.SaveChanges();
                        }
                    }
                }                
                
                db.SaveChanges();
                ViewBag.Message = $"The pledge of {model.PledgeAmount:C} was processed successfully.  Thank you!\n An email receipt has been sent to the email associated with {model.Name:C}";
            }
            else
            {
                ViewBag.ErrorMessage = $"There was a problem processing your pledge";
            }
            return View("Index");
        }

        /// <summary>
        /// Runs task of making the charge
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
        #endregion
    }

    internal class StripeResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}