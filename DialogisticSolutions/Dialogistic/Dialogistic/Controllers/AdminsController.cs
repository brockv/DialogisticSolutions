using Dialogistic.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Dialogistic.DAL;
using Dialogistic.Abstract;
using Dialogistic.Concrete;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Dialogistic.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminsController : Controller
    {
        #region Setup
        private IUnitOfWork unitOfWork;

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public AdminsController()
        {
            unitOfWork = new UnitOfWork(new DialogisticContext());
        }

        public AdminsController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
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
        /// Loads the dashboard for admins, grabbing important information to display.
        /// </summary>
        /// <returns>The dashboard view if the user is authenticated, and the login page if not.</returns>
        [HttpGet]
        public ActionResult Dashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Get the current user
                var currentUser = User.Identity.GetUserId();
                ViewBag.UserProfile = unitOfWork.Profiles.Find(x => x.UserID.Equals(currentUser));

                // Grab all of the calls still needing to be made
                ViewBag.TotalRemainingCalls = unitOfWork.Assignments.GetCallAssignments().Count();

                // Grab the total donations raised by all callers                
                ViewBag.TotalDonations = unitOfWork.Profiles.GetUserProfiles().Sum(x => x.DonationsRaised);
                return View();

            }

            // If we got this far, the user isn't authenticated - send them to the login screen
            return RedirectToAction("Login", "Account");
        }

        public ActionResult HelpPage()
        {
            return View();
        }

        /// <summary>
        /// Map the constituant address. If the first constituent has a lat and long DONT run all constituents through the API. 
        /// Otherwise run through API to establish initial lat and long.
        /// </summary>
        /// <returns>json object list of Latitudes and Longitudes</returns>
        public JsonResult MapAddresses()
        {
            Geolocation geo = new Geolocation();
            var constituents = unitOfWork.Constituents.GetConstituents();
            List<string[]> locations = new List<string[]> { };

            if (constituents.FirstOrDefault().Latitude != null && constituents.FirstOrDefault().Longitude != null)
            {
                foreach (var item in constituents)
                {                   
                    locations.Add(item.getLocation());
                }
            }

            else
            {
                foreach(var item in constituents)
                {
                    // Pass the address string to the API
                    geo.GetGeocode(item.getFullAddress());

                    // Assign resulting values to the constituents column
                    item.Latitude = System.Convert.ToDecimal(geo.Latitude);
                    item.Longitude = System.Convert.ToDecimal(geo.Longitude);

                    // Pass the values to the jsonobject
                    locations.Add(item.getLocation());
                }
            }

            unitOfWork.Complete();

            return Json(locations, JsonRequestBehavior.AllowGet);
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

        /// <summary>
        /// Loads the overlay with the current user's information displayed.
        /// </summary>
        /// <returns>The site overlay partial view.</returns>
        [ChildActionOnly]
        public PartialViewResult _SiteOverlay()
        {
            // Grab the current user
            var currentUserID = User.Identity.GetUserId();
            if (currentUserID != null)
            {
                // Grab their profile so we can display their information
                UserProfile user = unitOfWork.Profiles.Find(x => x.UserID.Equals(currentUserID)).FirstOrDefault();

                // Return the partial view with the user profile
                return PartialView(user);
            }

            // If we got this far, we couldn't find a user associated with their ID - return the view without it
            return PartialView();
        }

        // POST: /Admins/LogOff
        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        #region GenerateCalls
        /// <summary>
        /// Generates a list of call assignments of constituents associated with callers.
        /// Higher priority constituents are assigned to callers with higher accumulated donation success.
        /// The prioritization involves weighing multiple factors related to constitutent's identity and donation history.
        /// </summary>
        /// <returns>A list of CallAssignments -- Will be empty if there are no Constituents that need to be contacted</returns>
        public ActionResult GenerateCallList()
        {
            // Remove any existing Call Assignments before proceeding
            List<CallAssignment> existingAssignments = unitOfWork.Assignments.GetCallAssignments().ToList();
            if (existingAssignments != null && existingAssignments.Count() > 0)
            {
                unitOfWork.Assignments.RemoveRange(existingAssignments);
            }

            // Get a list of processed Constituents with updated priority levels
            List<Constituent> constituents = GetConstituents();

            // stores the total number of constitutents in the created list.
            int numberOfConstituents = constituents.Count;

            // Only build the call assigment list if there are Constituents that need to be contacted
            if (constituents.Count != 0)
            {
                // Create an empty list of Call Assignments
                List<CallAssignment> callList = new List<CallAssignment>();

                // Generate a list of callers from users designated as such sorted in descending order by total donations raised over time.
                List<UserProfile> callers = unitOfWork.Profiles.Find(x => x.IsCaller).OrderByDescending(s => s.DonationsRaised).ToList();

                // store the number of callers in the list.
                int numberOfCallers = callers.Count;

                if (numberOfCallers <= 0)
                {
                    return View();
                }

                // determine the number of constituents to assign to each caller on an equitable basis.
                int constituentsPerCaller = numberOfConstituents / numberOfCallers;


                // order list in descending order according to CallPriority.
                constituents = constituents.OrderByDescending(s => s.CallPriority).ToList();

                // variable to keep track of number constituents assigned to a caller so far.
                int assignedCallTotal;

                // variable to keep track of how far into the constituents list we are independent of the assignments to callers.
                int constituentListIndex = 0;

                // keeps track of how deep we are into the constituent list for a given caller.
                int index;

                // This loop assigns to each caller an even number of constituents such that better callers get higher priority constituents.
                foreach (var person in callers)
                {
                    assignedCallTotal = 0;
                    index = 0;

                    // Loop through the list of Constituents, assigning a constituent to the current caller until no more can be added.                
                    foreach (var item in constituents)
                    {
                        // This checks of the caller can still be assigned constituents from further down the list.
                        if (assignedCallTotal < constituentsPerCaller && constituentListIndex == index)
                        {                            
                            // matches up the caller's id with the given constituent's id.
                            callList.Add(new CallAssignment() { CallerID = person.UserID, ConstituentID = item.ConstituentID });

                            // increments total to prevent over assigning constituents to one caller.
                            assignedCallTotal = assignedCallTotal + 1;

                            // increments the List index to keep track of how far into the constituents list the iterations have traversed.
                            constituentListIndex = constituentListIndex + 1;
                        }

                        // increments counter to keep track how far into list one has gotten for this caller.
                        index = index + 1;
                    }
                }

                // Add the assignments to the database, and save the changes
                unitOfWork.Assignments.AddRange(callList);
                unitOfWork.Complete();

                return RedirectToAction("ManageCallList", "Admins", callList);
            }

            // Return the list -- this will be an empty list if there were no Constituents that needed to be contacted
            return RedirectToAction("ManageCallList", "Admins");
        }

        /// <summary>
        /// This is a helper method for getting and returning a list of constituents.
        /// This involves processing the constituent's and given each a calculated donation status and call priority level.
        /// </summary>
        /// <returns>A list of prioritized and classified constituents to be called.</returns>
        public List<Constituent> GetConstituents()
        {
            // Set the threshold for determining if a Constituent needs to be added to the call assignments - (from a year ago and into the past)
            DateTime contactThreshold = DateTime.Now.AddDays(-365);

            /* -------- START OF UPDATED SECTION -------- */

            // Get a list of all the Constituents currently in the database that are NOT deceased
            List<Constituent> constituents = unitOfWork.Constituents.GetAll().Where(x => x.Deceased == false).ToList();

            // Determine who doesn't need to be called -- based on last contact date and if they are deceased
            List<Constituent> toRemove = new List<Constituent>();
            foreach (var entry in constituents)
            {
                // Grab all the call logs for this Constituent, ordered by date in descending order
                var logs = entry.CallLogs.OrderByDescending(x => x.DateOfCall).ToArray();
                if (logs != null && logs.Count() > 0)
                {
                    if (logs[0].DateOfCall > contactThreshold) // Have they been contacted recently?
                    {
                        toRemove.Add(entry);
                    }
                }
            }

            // Remove Constituents that don't need to be called
            foreach (var entry in toRemove)
            {
                constituents.Remove(entry);
            }

            /* -------- END OF UPDATED SECTION -------- */

            // Order the list in ascending order according to gift amounts
            constituents.OrderByDescending(s => s.Gifts);

            // obtains conunt of constituents which also provides a ranking via the constitents having been sorted in ascending order.
            int countOfConstituents = constituents.Count;

            try
            {
                // Prioritize list of constituents according to donation status.
                // Loop through the list of Constituents, assigning an appropriate donation status and call priority to each according to their donation history.                
                foreach (var item in constituents)
                {
                    // Get last date of call
                    var _lastContacted = item.CallLogs.OrderByDescending(x => x.DateOfCall).Select(c => c.DateOfCall).FirstOrDefault();
                    // Get the last gift date
                    var _lastGiftDate = item.Gifts.OrderByDescending(x => x.GiftID).Select(x => x.CallLog.DateOfCall).FirstOrDefault();
                    // Get the next to last gift date
                    var _nextToLastGiftDate = item.Gifts.OrderByDescending(x => x.GiftID).Select(x => x.CallLog.DateOfCall).ElementAtOrDefault(1);

                    // obtain the calculated donation status and call priority for a given consitituent.
                    var result = GiveConstituentRanking(_lastContacted, _nextToLastGiftDate, _lastGiftDate, item.UniversityRelationship, countOfConstituents);

                    // assign the resultant donation status to the constituent
                    item.DonationStatus = result.Item1;

                    // assign the resultant call priority level to the constituent
                    item.CallPriority = result.Item2;

                    // decrements the count of the constituents and thus the rank of the next constituent examined
                    countOfConstituents = countOfConstituents - 1;
                }
            
                // Saves the changes to the database and additions to the Constituents table
                unitOfWork.Complete();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return constituents;
        }
        #endregion

        #region ConstituentRankings
        /// <summary>
        /// This is another helper method utilized to determining the ranking and status of a constituent
        /// based on the their donation history, relationship to the university, and relative ranking in the current list being examined.
        /// </summary>
        /// <param name="contactTime"> This comes from last contacted field of a constituent.</param>
        /// <param name="secondToLastGiftTime">This comes from the NextToLastGiftDate field of a constituent.</param>
        /// <param name="giftTime">This comes from the LastGiftDate field of a constituent </param>
        /// <param name="relationship">This comes from the University relationship field of a constituent.</param>
        /// <param name="rankingAscending">This comes from relative ranking for the currently examined list of constituents</param>
        /// <returns>This returns a tuple containing the calculated donation status and call priority level.</returns>
        public Tuple<string, int> GiveConstituentRanking(DateTime? contactTime, DateTime? secondToLastGiftTime, DateTime? giftTime, string relationship, int rankingAscending)
        {
            // initialize string to empty.
            string constituentStatus = "";

            // intialize default constituent ranking as zero.
            int constituentRanking = 0;

            /////////////// first assign priority points based on contact times and donation history//////////////////

            DateTime contactThreshold;

            // At Risk
            // Set the threshold for determining if a Constituent is at Risk - (1 year ago from today)
            contactThreshold = DateTime.Now.AddDays(-365);
            if (giftTime <= contactThreshold && giftTime != null)
            {
                constituentStatus = "At Risk";
                constituentRanking = 5;
            }

            // Lapsing
            // Set the threshold for determining if a Constituent is Lapsing - (2 years ago from today)
            contactThreshold = DateTime.Now.AddDays(-365 * 2);
            if (giftTime <= contactThreshold && giftTime != null)
            {
                constituentStatus = "Lapsing";
                constituentRanking = 3;
            }

            // Lapsed
            // Set the threshold for determining if a Constituent is Lapsed - (3 years ago from today)
            contactThreshold = DateTime.Now.AddDays(-365 * 3);
            if (giftTime <= contactThreshold && giftTime != null)
            {
                constituentStatus = "Lapsed";
                constituentRanking = 2;
            }

            // Lost
            // Set the threshold for determining if a Constituent is Lost - (5 years ago from today)
            contactThreshold = DateTime.Now.AddDays(-365 * 5);
            if (contactTime <= contactThreshold && giftTime != null)
            {
                constituentStatus = "Lost";
                constituentRanking = 1;
            }

            // Recaptured
            // Set the threshold for determining if a Constituent has been recaptured - (1 and half years between donations)
            //To get the amount of days between two gift dates.  
            if (giftTime != null && secondToLastGiftTime != null)
            {
                int daysDiff = ((TimeSpan)(giftTime - secondToLastGiftTime)).Days;
                contactThreshold = DateTime.Now.AddDays(-365 * 1);
                if (daysDiff > 365 * 1.25 && giftTime >= contactThreshold)
                {
                    constituentStatus = "Recaptured";
                    constituentRanking = 4;
                }
            }

            // Retained
            // Set the threshold for determining if a Constituent has been retained - (less than 1 year between donations)
            //To get the amount of days between two gift dates.  
            if (giftTime != null && secondToLastGiftTime != null)
            {
                int daysDiff2 = ((TimeSpan)(giftTime - secondToLastGiftTime)).Days;
                contactThreshold = DateTime.Now.AddDays(-365 * 1);
                if (daysDiff2 < 365 * 1.25 && giftTime >= contactThreshold)
                {
                    constituentStatus = "Retained";
                    constituentRanking = 7;
                }
            }

            // Acquired
            // Set the threshold for determining if a Constituent has been acquired - (first gift ever within the last year.)
            int daysDiff3 = ((TimeSpan)(DateTime.Now - giftTime)).Days;
            if (secondToLastGiftTime == null && giftTime != null && daysDiff3 < 365)
            {
                constituentStatus = "Acquired";
                constituentRanking = 6;

            }

            // Never Donors
            // Determining if a Constituent has never donated - (no donations at all)
            if (secondToLastGiftTime == null && giftTime == null)
            {
                constituentStatus = "Never Donors";
                constituentRanking = 0;
            }

            ///////////////// Now assign additional priority points based on constituent's relation to the university.
            if (relationship != null)
            {
                // checks for university relation of constituent and assigns appropriate value.
                if (relationship.Equals("Student"))
                {
                    constituentRanking = constituentRanking + 1;
                }
                else if (relationship.Equals("Parent"))
                {
                    constituentRanking = constituentRanking + 2;
                }
                else if (relationship.Equals("Alumni"))
                {
                    constituentRanking = constituentRanking + 3;
                }
                else if (relationship.Equals("Corporation"))
                {
                    constituentRanking = constituentRanking + 4;
                }
                else
                {
                    // we add no additional priority points
                }
            }

            // Now assign additional priority points based on constituent's lifetime donations ranking compared to others
            // in the current list being examined.
            constituentRanking = constituentRanking + rankingAscending;

            return Tuple.Create(constituentStatus, constituentRanking);

        }
        #endregion

        /// <summary>
        /// Loads the view where the SuperAdmin can manage which roles all other users are in.
        /// </summary>
        /// <returns>The view loaded with the current list of users and their roles.</returns>
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ManageAllUsers()
        {
            #region Get all users
            // Get the SuperAdmin account -- only the SuperAdmin can access this page, so get them from the session
            var superAdmin = UserManager.FindById(User.Identity.GetUserId());

            // Build a list of all the current users in the system, minus the SuperAdmin
            List<UserProfile> users = unitOfWork.Profiles.Find(x => x.UserID != superAdmin.Id).ToList();

            // Convert to the model used in the view
            List<ChangeUserRoleModel> usersToChange = new List<ChangeUserRoleModel>();
            foreach (var user in users)
            {
                // Get the required information from the AspNet tables
                var currentUserFromSystem = UserManager.FindById(user.UserID);
                var roleName = UserManager.GetRoles(currentUserFromSystem.Id).FirstOrDefault();

                // Convert the information to the correct model for the view
                ChangeUserRoleModel currentUserToChange = new ChangeUserRoleModel
                {
                    User = user,
                    Role = roleName
                };

                // Add the current user to the list
                usersToChange.Add(currentUserToChange);
            }
            #endregion

            #region SelectList for roles
            // Set up a SelectList containing the available roles a user can have
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var role in RoleManager.Roles)
            {
                // Exclude the SuperAdmin role, as there should only be one
                if (role.Name != "SuperAdmin")
                {
                    list.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
                }
            }

            // Store the list to be used in the view
            ViewBag.Roles = list;
            #endregion

            // Return the view
            return View(usersToChange);
        }

        /// <summary>
        /// Updates the provided Call Assignment with the new Caller.
        /// </summary>
        /// <param name="model">The Call Assignment to update.</param>
        /// <returns>The Manage Call List view on success, and the Dashboard on failure.</returns>
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ManageAllUsers(ChangeUserRoleModel model)
        {
            // Make sure the model is in a valid state before we do anything with it
            if (ModelState.IsValid)
            {
                // Look up the given user and remove their current role
                var givenUser = UserManager.FindById(model.User.UserID);

                if (givenUser != null)
                {
                    #region Changing from "StandardUser" to "Administrator"
                    /* If we're changing this user from a "StandardUser" to an "Administrator", we need to remove their
                       Call Assignments, if any exist */

                    // Look up the given user in the UserProfiles table
                    var currentUser = unitOfWork.Profiles.Find(x => x.ProfileID == model.User.ProfileID).FirstOrDefault();
                    if (currentUser != null && model.Role == "Administrator")
                    {
                        // Check if this user is a Caller, remove their Call Assignments from the table
                        if (currentUser.IsCaller)
                        {
                            // Build a list of all the current Call Assignments
                            List<CallAssignment> assignments = unitOfWork.Assignments.Find(x => x.CallerID == currentUser.UserID).ToList();
                            if (assignments != null)
                            {
                                // Remove all Call Assignments assigned to this user
                                foreach (var entry in assignments)
                                {
                                    unitOfWork.Assignments.Remove(entry);
                                }
                            }

                            // Set the IsCaller flag to false
                            currentUser.IsCaller = false;
                        }
                    }
                    #endregion
                    #region Changing from "Administrator" to "StandardUser"
                    else
                    {
                        // Set the IsCaller flag to true
                        currentUser.IsCaller = true;
                    }
                    #endregion
                }

                #region Update the role
                // Remove the user's current role
                string roleName = UserManager.GetRoles(givenUser.Id).FirstOrDefault();
                UserManager.RemoveFromRole(givenUser.Id, roleName);

                // Add the new role to the user
                UserManager.AddToRole(givenUser.Id, model.Role);

                // Save the changes
                UserManager.Update(givenUser);
                #endregion

                // Save the changes to the database
                unitOfWork.Complete();

                // Regenerate the Call List since we are adding / removing a Caller
                GenerateCallList();

                // Return this view
                return RedirectToAction("ManageAllUsers");
            }

            // If we got this far, the model state wasn't valid - redirect to the Dashboard
            return RedirectToAction("Dashboard", "Admins");
        }

        #region CallerAssignments
        /// <summary>
        /// Gets all of the Call Assignments for a Caller associated with the given callerID.
        /// </summary>
        /// <param name="callerID">The ID of the Caller to look up.</param>
        /// <returns>An array of Constituent IDs associated with Call Assignments assigned to the Caller in question.</returns>
        [HttpPost]
        public JsonResult GetCallersAssignments(string callerID)
        {
            // User the callerID to find the Caller in the UserProfiles table
            var caller = unitOfWork.Profiles.Find(x => x.UserID == callerID).FirstOrDefault();

            // Find all Call Assignments currently assigned to this Caller
            var assignments = unitOfWork.Assignments.Find(x => x.CallerID == caller.UserID).ToArray();

            // Build an array of the assignments Constituent's IDs to pass back
            int[] constituentIDS = new int[assignments.Count()];
            for (var i = 0; i < assignments.Count(); i++)
            {
                constituentIDS[i] = assignments[i].ConstituentID;
            }

            // Return the array to the JavaScript function
            return Json(constituentIDS, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updates the Call Assignments assigned to a given user by replacing the existing
        /// CallerID with their own.
        /// </summary>
        /// <param name="model">The model used for updating Call Assignments.</param>
        /// <returns>An empty JSON object. Not significant.</returns>
        [HttpPost]
        public JsonResult UpdateCallerAssignments(BlockCallAssignmentsModel model)
        {
            // Ensure the model is in a valid state before doing anything with it
            if (ModelState.IsValid)
            {
                // Iterate over each entry of the array of Constituent IDs
                foreach (var constituentID in model.Constituents)
                {
                    // Get the Call Assignment associated with the current constituentID by ConstituentID
                    CallAssignment assignmentToUpdate = unitOfWork.Assignments.Find(x => x.ConstituentID == constituentID).FirstOrDefault(); //assignments.Where(x => x.ConstituentID == constituentID).FirstOrDefault();

                    // Quick compare of the current CallerID -- not sure if this is beneficial at all
                    if (assignmentToUpdate.CallerID != model.CallerID)
                    {
                        // Remove the entry associated with this Call Assignment -- necessary to avoid constraint issues
                        unitOfWork.Assignments.Remove(assignmentToUpdate);

                        // Create a new CallAssignment, updating the CallerID to the new one -- other values stay the same
                        CallAssignment updatedCallAssignment = new CallAssignment()
                        {
                            CallerID = model.CallerID,
                            Constituent = unitOfWork.Constituents.Find(x => x.ConstituentID == constituentID).FirstOrDefault(),
                            ConstituentID = constituentID
                        };

                        // Add the updated version of this Call Assignment back into the table
                        unitOfWork.Assignments.Add(updatedCallAssignment);
                    }
                }

                // Save the changes
                unitOfWork.Complete();
            }

            // Return an empty JSON object -- we don't care about what's returned, only that *something* is returned
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Loads the view where admins can manage which Callers are calling which Constituents.
        /// </summary>
        /// <returns>The view loaded with the current list of Call Assignments</returns>
        [HttpGet]
        public ActionResult ManageCallList()
        {
            // Get all of the current Call Assignments
            CallAssignment[] assignments = unitOfWork.Assignments.GetCallAssignments().ToArray();

            // Get all of the users designated as Callers
            var callers = unitOfWork.Profiles.GetUserProfiles().ToArray();

            // Assign the appropriate Caller's name to their respective Call Assignments
            for (var i = 0; i < assignments.Count(); i++)
            {
                // Compare each Caller to the current index of assignments
                for (var k = 0; k < callers.Count(); k++)
                {
                    // If the CallerID matches the UserID, assign their name to the assignment
                    if (assignments[i].CallerID == callers[k].UserID)
                    {
                        assignments[i].CallerName = callers[k].FullName;
                    }
                }
            }

            // Grab all the Callers to be used in a dropdown in the view
            ViewBag.Callers = unitOfWork.Profiles.Find(x => x.IsCaller == true).ToList();

            // Sort the array (in ascending order) so that it appears in the same order every time
            var sorted = assignments.OrderBy(x => x.ConstituentID).ToList();

            // Return the view with the current list of Call Assignments
            return View(sorted);
        }     

        /// <summary>
        /// Loads the view where admins can manage Callers rolls.
        /// </summary>
        /// <returns>The view loaded with the current list of Callers</returns>
        public ActionResult ManageCallers()
        {
            // Grab all of the Users designated as Callers
            var names = unitOfWork.Profiles.Find(x => x.IsCaller).ToList();

            // Return the view with the current list of Call Assignments
            return View(names);
        }

        /// <summary>
        /// Update a callers roll to admin and change their userprofile to no longer be a caller.
        /// </summary>
        /// <param name="userID">the ID of the user that is getting updated</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ManageCallers(string userID)
        {
            // Look up the user in the UserProfiles table
            var newAdmin = unitOfWork.Profiles.Find(x => x.UserID.Equals(userID)).FirstOrDefault();

            // Set this user's IsCaller designation to false as they are now an admin
            // TO-DO: Remove Call Assignments from this user when they are removed as a Caller
            newAdmin.IsCaller = false;
           
            // Remove the user's current role
            string roleName = UserManager.GetRoles(userID).FirstOrDefault();
            UserManager.RemoveFromRole(userID, roleName);

            // Add the 'Administrator' role to the user
            UserManager.AddToRole(userID, "Administrator");

            // Look up this user in the AspNet tables and save the changes to them
            var user = UserManager.FindById(userID);
            UserManager.Update(user);
           
            // Regenerate the Call List since we are removing a Caller
            GenerateCallList();

            // Save the changes to the database
            unitOfWork.Complete();

            // Get a list of all the current Callers and return the view
            var names = unitOfWork.Profiles.Find(x => x.IsCaller).ToList();
            
            return View(names);
        }
        #endregion

        #region ProposedChanges
        /// <summary>
        /// Loads a view where admins can review changes that have been submitted by standard users for review,
        /// and either approve or deny them.
        /// </summary>
        /// <returns>The view containing a list of any changes to review.</returns>
        [HttpGet]
        public ActionResult ReviewProposedConstituentChanges()
        {
            // Get a list of all the entries in both the Constituents and ProposedConstituentChanges tables
            ReviewChangesModel reviewChanges = new ReviewChangesModel
            {
                Constituents = new List<Constituent>(),
                ConstituentChanges = new List<ProposedConstituentsChanges>()
            };

            // Create a list of all entries in the table of proposed changes
            List<ProposedConstituentsChanges> proposedChanges = unitOfWork.Changes.GetProposedChanges().ToList();

            //List<Constituent> constituents = new List<Constituent>();

            // Only proceed if there are entries in the table
            if (proposedChanges != null)
            {
                // Grab the original entry for each Constituent
                foreach (var changed in proposedChanges)
                {
                    Constituent original = unitOfWork.Constituents.Find(x => x.ConstituentID == changed.ConstituentID).FirstOrDefault();
                    //constituents.Add(selectedConstituent);
                    reviewChanges.Constituents.Add(original);
                    reviewChanges.ConstituentChanges.Add(changed);
                }

                // Send the list to the view
                return View(reviewChanges);
            }

            // Send the list to the view
            return View();
        }

        /// <summary>
        /// Approves changes that have been made to a given Constituent, saving the changes to the master table
        /// of Constituents.
        /// </summary>
        /// <param name="id">The ID of the Constituent that is being updated.</param>
        /// <returns>The view containing all proposed changes on success, otherwise the 'Error' view.</returns>
        [HttpGet]
        public ActionResult ApproveConstituentChanges(int? id)
        {
            // Make sure the model is in a valid state before trying to do anything with it
            if (id != null)
            {
                // Find the entry for this Constituent in the master Constituent table
                Constituent original = unitOfWork.Constituents.Find(x => x.ConstituentID == id).FirstOrDefault();

                // Find the entry for this Constituent in the proposed changes table
                ProposedConstituentsChanges proposedChanges = unitOfWork.Changes.Find(x => x.ConstituentID == id).FirstOrDefault();

                // If we found an entry, update the values                
                if (original != null)
                {
                    // Calculate their latitude and longitude if the primary address changed
                    if (original.PreferredAddressLine1 != proposedChanges.PreferredAddressLine1)
                    {
                        Geolocation geo = new Geolocation();
                        geo.GetGeocode(proposedChanges.getFullAddress());
                        original.Latitude = Convert.ToDecimal(geo.Latitude);
                        original.Longitude = Convert.ToDecimal(geo.Longitude);
                    }
                    
                    // Update the original values with the changes
                    unitOfWork.Entry(original).CurrentValues.SetValues(proposedChanges);                    

                    // Remove the entry associated with this Constituent from the proposed changes table
                    unitOfWork.Changes.Remove(proposedChanges);

                    // Save the changes to the database
                    unitOfWork.Complete();

                    // Success - return the view containing proposed changes
                    return RedirectToAction("ReviewProposedConstituentChanges");
                }
            }

            // If we get this far, something went wrong -- return the 'Error' view
            return View("Error");
        }

        /// <summary>
        /// Denies changes that have been made to the given Constituent, deleting the entry in the
        /// ProposedConstituentChanges table without making any changes to the master Constituents table.
        /// </summary>
        /// <param name="id">The ID of the Constituent whose changes are being denied.</param>
        /// <returns>The view containing all proposed changes on success, otherwise the 'Error' view.</returns>
        [HttpGet]
        public ActionResult DenyConstituentChanges(int? id)
        {
            // Find the entry associated with this Constituent in the proposed changes table
            ProposedConstituentsChanges proposedChanges = unitOfWork.Changes.Find(x => x.ConstituentID == id).FirstOrDefault();

            // If we found an entry, delete it
            if (proposedChanges != null)
            {
                // Remove the entry and save the database
                unitOfWork.Changes.Remove(proposedChanges);
                unitOfWork.Complete();

                // Success - return the view containing proposed changes
                return RedirectToAction("ReviewProposedConstituentChanges");
            }

            // If we get this far, something went wrong -- return the 'Error' view
            return View("Error");
        }
        #endregion
    }
}