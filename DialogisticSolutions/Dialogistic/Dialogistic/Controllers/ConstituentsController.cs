using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dialogistic.Abstract;
using Dialogistic.Concrete;
using Dialogistic.DAL;
using Dialogistic.Models;
using LINQtoCSV;
using Microsoft.AspNet.Identity;



namespace Dialogistic.Controllers
{
    
    [Authorize(Roles = "Administrator, Standard")]
    public class ConstituentsController : Controller
    {
        #region Setup
        private IUnitOfWork unitOfWork;

        public ConstituentsController()
        {
            unitOfWork = new UnitOfWork(new DialogisticContext());
        }

        public ConstituentsController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Index
        // GET: Constituents
        public ViewResult Index()
        {
            // Get all entries in the Constituents table
            List<Constituent> constituents = new List<Constituent>();
            var currentUser = User.Identity.GetUserId();
            UserProfile standardUser = unitOfWork.Profiles.Find(x => x.UserID == currentUser).First();

            // this checks if the user is a caller and should thereby view only their own constituents
            if (standardUser.IsCaller)
            {
                // gets constitutents that are on the user's call list only.
                var allCallAssignments = unitOfWork.Assignments.GetCallAssignments();
                foreach (var assignment in allCallAssignments)
                {
                    if (assignment.CallerID == standardUser.UserID)
                    {
                        constituents.Add(assignment.Constituent);
                    }
                }
            }
            else // This occurs if the user is not a caller i.e. an admin and can view all constituents.
            {
                // gets all constituents.
                constituents = unitOfWork.Constituents.GetConstituents().ToList();
            }

            // Determine each Constituent's donation status
            foreach (var item in constituents)
            {
                item.DonationStatus = CheckStatus(item);
            }           

            // set view flag for indicating no error has occured.
            ViewBag.flag = "Success";
          
            return View(constituents);
        }
        #endregion

        #region CreateNew
        public ActionResult Create()
        {
            return View();
        }

        // GET: Constituent/New
        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Exclude = "ConstituentID")] Constituent constituent)
        {            
            if (ModelState.IsValid)
            {
                // Determine what the current highest ID is, then increment by one
                Constituent[] allConstituents = unitOfWork.Constituents.GetConstituents().OrderByDescending(x => x.ConstituentID).ToArray();
                int newConstituentID = allConstituents[0].ConstituentID + 1;

                // Use that number as the ID for this Constituent
                constituent.ConstituentID = newConstituentID;

                // Calculate their latitude and longitude
                Geolocation geo = new Geolocation();
                geo.GetGeocode(constituent.getFullAddress());
                constituent.Latitude = Convert.ToDecimal(geo.Latitude);
                constituent.Longitude = Convert.ToDecimal(geo.Longitude);

                unitOfWork.Constituents.Add(constituent);
                unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            return View(constituent);
        }

        [HttpPost]
        public ActionResult CreateConstituent(Constituent constituent)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Constituents.Add(constituent);
                unitOfWork.Complete();

                return RedirectToAction("Dashboard", "Admins");
            }

            return View(constituent);
        }
        #endregion

        #region DeleteConstituent
        [HttpPost]
        public bool Delete(int id)
        {
            // If this constituent is in the call assignments, remove them from there before deleting
            CallAssignment callAssignment = unitOfWork.Assignments.Get(id);
            if (callAssignment != null)
            {
                unitOfWork.Assignments.Remove(callAssignment);

                // Remove the constituent from the database
                Constituent constituent = unitOfWork.Constituents.Find(x => x.ConstituentID == id).FirstOrDefault();
                if (constituent != null)
                {
                    unitOfWork.Constituents.Remove(constituent);
                    unitOfWork.Complete();
                }
                return true;
            }
            else
            {
                // Remove the constituent from the database
                Constituent constituent = unitOfWork.Constituents.Find(x => x.ConstituentID == id).FirstOrDefault();
                if (constituent != null)
                {
                    unitOfWork.Constituents.Remove(constituent);
                    unitOfWork.Complete();
                }
                return true;
            }
        }
        #endregion

        #region Update
        [HttpGet]
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Constituent constituent = db.Constituents.Find(id);
            Constituent constituent = unitOfWork.Constituents.Find(x => x.ConstituentID == id).FirstOrDefault();
            if (constituent == null)
            {
                return HttpNotFound();
            }

            return View(constituent);
        }

        [HttpPost]
        public ActionResult UpdateConstituent(Constituent model)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Entry(model).State = EntityState.Modified;
                unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Constituents");
        }
        #endregion

        #region ConstituentChanges
        /// <summary>
        /// Attempts to find a Constituent associated with the given id, and loads their information
        /// into the view if one is found.
        /// </summary>
        /// <param name="id">The id of the Constituent to look up.</param>
        /// <returns>The update view if a match is found, else an appropriate error form.</returns>
        [HttpGet]
        public ActionResult ProposeConstituentChanges(int? id)
        {
            // If the id parameter is null, give a "bad request" error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // The parameter wasn't null -- try and find the associated Constituent in the proposed changes table first
            ProposedConstituentsChanges changes = unitOfWork.Changes.Find(x => x.ConstituentID == id).FirstOrDefault();
            if (changes == null)
            {
                // Nothing in the proposed changes table yet -- look them up in the main Constituents table
                Constituent constituent = unitOfWork.Constituents.Find(x => x.ConstituentID == id).FirstOrDefault();

                // If there wasn't a match with this id, give a "not found" error
                if (constituent == null)
                {
                    return HttpNotFound();
                }

                // Found a match in the main Constituents table -- return the view with the Constituent
                return PartialView("_ProposeChanges", constituent);
            }

            // If we got this far, we found a match in the proposed changes table -- copy values over to send to the view
            var existingConstituent = new Constituent
            {
                ConstituentID = changes.ConstituentID,
                PrimaryAddressee = changes.PrimaryAddressee,
                PreferredAddressLine1 = changes.PreferredAddressLine1,
                PreferredAddressLine2 = changes.PreferredAddressLine2,
                PreferredAddressLine3 = changes.PreferredAddressLine3,
                PreferredCity = changes.PreferredCity,
                PreferredState = changes.PreferredState,
                PreferredZIP = changes.PreferredZIP,
                PhoneNumber = changes.PhoneNumber,
                MobilePhoneNumber = changes.MobilePhoneNumber,
                AlternatePhoneNumber = changes.AlternatePhoneNumber,
                Deceased = changes.Deceased,
                DonationStatus = changes.DonationStatus,
                UniversityRelationship = changes.UniversityRelationship,
                CallPriority = changes.CallPriority
            };

            // Return the view with the Constituent
            return PartialView("_ProposeChanges", existingConstituent);
        }

        /// <summary>
        /// Creates an entry in the ProposedConstituentsChanges table, allowing non-administrators
        /// to submit changes to existing Constituents. Changes are reviewed and either accepted or
        /// denied by administrators in another controller / method.
        /// </summary>
        /// <param name="model">The Constituent to submit changes for.</param>
        /// <returns>The Constituents Index page on success, else the same view with any error messages.</returns>
        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult ProposeConstituentChanges(Constituent model)
        {
            // Check if the model state is valid before trying to do anything with it
            if (ModelState.IsValid)
            {
                // Check if this Constituent exists in the proposed changes table already
                var existingEntry = unitOfWork.Changes.Find(x => x.ConstituentID == model.ConstituentID).FirstOrDefault();

                // If there isn't an existing entry, create a new one for the table
                if (existingEntry == null)
                {
                    // If it's in a valid state, create a new entry in the proposed changes table
                    var proposedChanges = new ProposedConstituentsChanges
                    {
                        ConstituentID = model.ConstituentID,
                        PrimaryAddressee = model.PrimaryAddressee,
                        PreferredAddressLine1 = model.PreferredAddressLine1,
                        PreferredAddressLine2 = model.PreferredAddressLine2,
                        PreferredAddressLine3 = model.PreferredAddressLine3,
                        PreferredCity = model.PreferredCity,
                        PreferredState = model.PreferredState,
                        PreferredZIP = model.PreferredZIP,
                        PhoneNumber = model.PhoneNumber,
                        MobilePhoneNumber = model.MobilePhoneNumber,
                        AlternatePhoneNumber = model.AlternatePhoneNumber,
                        Deceased = model.Deceased,
                        DonationStatus = model.DonationStatus,
                        UniversityRelationship = model.UniversityRelationship,
                        CallPriority = model.CallPriority
                    };

                    unitOfWork.Changes.Add(proposedChanges);
                    unitOfWork.Complete();
                }
                // An entry existed already -- overwrite the values with the new changes
                else
                {
                    unitOfWork.Entry(existingEntry).CurrentValues.SetValues(model);
                    unitOfWork.Complete();
                }
            }

            // If we got this far, something went wrong -- return the view with the model
            return PartialView("_ProposeChanges", model);
        }
        #endregion

        #region CheckStatus
        public string CheckStatus(Constituent constituent)
        {
            string status;
            int timeLapsed;
            DateTime current = DateTime.Now;

            if (constituent.DonationStatus == null)
            {
                var logs = unitOfWork.Logs.GetCallLogs().Where(x => x.ConstituentID == constituent.ConstituentID);

                if (logs != null && logs.Count() > 0)
                {
                    var lastGift = logs.Last();
                    DateTime lastGiftDate = (DateTime)lastGift.DateOfCall;
                    timeLapsed = ((current.Year - lastGiftDate.Year) * 12) + current.Month - lastGiftDate.Month;

                    if (timeLapsed >= 12 && timeLapsed <= 15)
                    {
                        status = "At Risk";
                    }
                    else if (timeLapsed > 15 && timeLapsed <= 24)
                    {
                        status = "Lapsing";
                    }
                    else if (timeLapsed > 24 && timeLapsed <= 60)
                    {
                        status = "Lapsed";
                    }
                    else if (timeLapsed > 60)
                    {
                        status = "Lost";
                    }
                    else
                    {
                        status = "Retained";
                    }
                }
                else
                {
                    status = "Never Donor";
                }
            }
            else
            {
                status = constituent.DonationStatus;
            }

            return status;
        }
        #endregion

        #region ImportCSV
        /// <summary>
        /// Imports the selected file and loads its contents into a data class.
        /// The field information in the data class is then transferred to the 
        /// Constitutents database table as new constituents.
        /// </summary>
        /// <param name="file">takes the "posted" csv file</param>
        /// <returns>redirects the user back to the constituents index page.</returns>
        [HttpPost]
        public JsonResult Import(HttpPostedFileBase file)
        {
            // Only proceed if the file isn't null and has content
            if (file != null && file.ContentLength > 0)
            {
                // Restrict accepted file types to csv and CSV
                var supportedTypes = new[] { "csv", "CSV" };

                // Reject the file if it doesn't match the required type defined above
                var fileExt = Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt))
                {
                    ViewBag.Message = "Invalid file type. Please select a CSV file.";

                    // Redirects the user back to the Constituents index page
                    return Json(new { Success = false, Message = "Error: Invalid file type. Please select a CSV file." });
                }

                // Set up the requirements for reading in the CSV file
                CsvFileDescription csvFileDescription = new CsvFileDescription
                {
                    SeparatorChar = ',',
                    FirstLineHasColumnNames = true
                };

                try
                {
                    // Read the file into a stream reader, and then convert each entry into our Constituent model
                    CsvContext csvContext = new CsvContext();
                    StreamReader streamReader = new StreamReader(file.InputStream);
                    IEnumerable<Constituent> importList = csvContext.Read<Constituent>(streamReader, csvFileDescription);

                    // This loop iterates through each item in the importList object
                    // and either updates an existing record, or creates a new one
                    Geolocation geo = new Geolocation();
                    foreach (var constituent in importList)
                    {
                        // Check for an existing record with this ID
                        Constituent original = unitOfWork.Constituents.Find(x => x.ConstituentID == constituent.ConstituentID).FirstOrDefault();
                        if (original != null)
                        {
                            // If we found one, update the values //

                            // Calculate their latitude and longitude if the primary address changed
                            if (original.PreferredAddressLine1 != constituent.PreferredAddressLine1)
                            {
                                geo.GetGeocode(constituent.getFullAddress());
                                original.Latitude = Convert.ToDecimal(geo.Latitude);
                                original.Longitude = Convert.ToDecimal(geo.Longitude);
                            }

                            // Calculate their donation status
                            original.DonationStatus = CheckStatus(constituent);

                            // Add them to the database with the updated values
                            unitOfWork.Entry(original).CurrentValues.SetValues(constituent);
                        }
                        else
                        {
                            // Otherwise, create a new entry //

                            // Grab the ID
                            constituent.ConstituentID = constituent.ConstituentID;

                            // Calculate their latitude and longitude
                            geo.GetGeocode(constituent.getFullAddress());
                            constituent.Latitude = Convert.ToDecimal(geo.Latitude);
                            constituent.Longitude = Convert.ToDecimal(geo.Longitude);

                            // Calculate their donation status
                            constituent.DonationStatus = CheckStatus(constituent);

                            // Add them to the database
                            unitOfWork.Constituents.Add(constituent);
                        }
                    }
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

                // Saves the changes to the database and additions to the Constituents table and set the success message
                //unitOfWork.Complete();
                return Json(new { Success = true, Message = "CSV File Was Successfully Imported!" });
            }

            // If we got this far, something went wrong -- set the error message
            return Json(new { Success = false, Message = "Error: Import failed!" });
        }
        #endregion
    }
}
