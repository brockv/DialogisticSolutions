using Dialogistic.DAL;
using Dialogistic.Models;
using LINQtoCSV;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dialogistic.Controllers
{
    public class CSVController : Controller
    {
        #region CSVImporter
        // Create a new database object for the constituents table
        private DialogisticContext db = new DialogisticContext();

        /// <summary>
        /// Displays the import page where users in the "Administrator" or "SuperAdmin" roles
        /// can import csv files.
        /// </summary>
        /// <returns>the standard view</returns>
        [HttpGet]
        [Authorize(Roles = "Administrator, SuperAdmin")] // added SuperAdmin role.
        public ActionResult Import()
        {
            return View();
        }

        /// <summary>
        /// Imports the selected file and loads its contents into a data class.
        /// The field information in the data class is then transferred to the 
        /// Constitutents database table as new constituents.
        /// </summary>
        /// <param name="file">takes the "posted" csv file</param>
        /// <returns>redirects the user to the constituents index page.</returns>
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
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
                    return View();
                }

                // Set up the requirements for reading in the CSV file
                CsvFileDescription csvFileDescription = new CsvFileDescription
                {
                    SeparatorChar = ',',
                    FirstLineHasColumnNames = true
                };

                // Read the file into a stream reader, and then convert each entry into our Constituent model
                CsvContext csvContext = new CsvContext();
                StreamReader streamReader = new StreamReader(file.InputStream);
                IEnumerable<Constituent> importList = csvContext.Read<Constituent>(streamReader, csvFileDescription);

                // This loop iterates through each item in the importList object
                // and either updates an existing record, or creates a new one
                foreach (var constituent in importList)
                {
                    // Check for an existing record with this ID
                    Constituent original = db.Constituents.Find(constituent.ConstituentID);
                    if (original != null)
                    {
                        // If we found one, update the values
                        db.Entry(original).CurrentValues.SetValues(constituent);
                    }
                    else
                    {
                        // Otherwise, create a new entry
                        constituent.ConstituentID = constituent.ConstituentID;
                        db.Constituents.Add(constituent);
                    }
                }

                // Saves the changes to the database and additions to the Constituents table
                db.SaveChanges();

                // Redirects the user to the Constituents index page
                return RedirectToAction("Index", "Constituents");
            }

            // If we got this far, something went wrong -- return the view
            return View();
        }
        #endregion
    }
}