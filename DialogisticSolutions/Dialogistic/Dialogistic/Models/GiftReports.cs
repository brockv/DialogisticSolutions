using Dialogistic.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Dialogistic.DAL;
using System.Diagnostics;

namespace Dialogistic.Models
{
    public class GiftReports
    {
        private DialogisticContext db = new DialogisticContext();

        public GiftReports()
        {
            GetConstituentReport(778);
        }

        /// <summary>
        /// Get the gift sum on a spicific constituent
        /// </summary>
        /// <param name="ID">ID of consituent you want</param>
        /// <returns></returns>

        public Chart GetConstituentReport(int ID)
        {
            Chart chart = new Chart();
            // Get the number of days in the current month
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            string[] labels = new string[12];

            chart.datasets = new List<Datasets>();

            var constituent = db.Gifts.Where(c => c.ConstituentID.Equals(ID));

            List<int> dataList = new List<int>();

            // Get each month
            for (int i = 1; i <=12; i++)
            {
                labels[i-1] = new DateTime(DateTime.Now.Year, i, 1).ToString("MMM", CultureInfo.InvariantCulture).ToUpper();

                decimal? getSum = constituent.Where(d => d.CallLog.DateOfCall.Month.Equals(i) &&
                                                         d.CallLog.DateOfCall.Year.Equals(DateTime.Now.Year))
                                             .Sum(x => x.GiftAmount);
                int total = Convert.ToInt32(getSum);
                // Add to list
                dataList.Add(total);
            }


            // Convert list to array
            int[] list = dataList.ToArray();
            chart.labels = labels;
            chart.datasets = CreateDataset("#ED6A5A", "rgba(237, 106, 90, 0.1)", list, "Donations in " + DateTime.Now.Year.ToString());

            return chart;
        }


        /// <summary>
        /// Get the gift sum of donations between two dates
        /// </summary>
        /// <param name="startDate">The starting date you want to include</param>
        /// <param name="endDate">The last date you want to include</param>
        /// <returns></returns>
        public int GetSumBetween(DateTime startDate, DateTime endDate)
        {
            decimal? dec = db.Gifts.Where(d => d.CallLog.DateOfCall >= startDate && d.CallLog.DateOfCall <= endDate).Sum(g => g.GiftAmount);
            int sum = Convert.ToInt32(dec);

            return sum;
        }
        /// <summary>
        /// Get the gift sum on a spicific date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public int GetSumEqualTo(DateTime date)
        {
            decimal? dec = db.Gifts.Where(d => d.CallLog.DateOfCall == date).Sum(g => g.GiftAmount);
            int sum = Convert.ToInt32(dec);

            return sum;
        }
        #region NeedsRefactoring
        /// <summary>
        /// Gets the current yearly gifts by month 
        /// </summary>
        /// <returns>Json object with months and sum of gifts during said months</returns>
        /// 
        public Chart YearlyReport()
        {
            Chart chart = new Chart();

            // Get the current year and subtract 5
            int year = DateTime.Now.Year - 5;
            List<string> yearLabel = new List<string>();

            // Fill dayLabel array with strings representing the date
            for (int i = year; i <= DateTime.Now.Year; i++)
                yearLabel.Add(i.ToString());


            chart.labels = yearLabel.ToArray();
            chart.datasets = new List<Datasets>();

            List<int> dataList = new List<int>();

            for (int i = year; i <= DateTime.Now.Year; i++)
            {
                // Get the value from the database
                decimal? getSum = db.Gifts
                                    .Where(x => x.CallLog.DateOfCall.Year.Equals(i))
                                    .Sum(x => x.GiftAmount);

                if (getSum == null)
                    getSum = 0;
                // Convert to int from decimal
                int total = Convert.ToInt32(getSum);
                // Add to list
                dataList.Add(total);
            }
            // Convert list to array
            int[] list = dataList.ToArray();

            chart.datasets = CreateDataset("#677D49", "rgba(103, 125, 73, 0.1)", list, DateTime.Now.Year.ToString());

            return chart;
        }

        /// <summary>
        /// Gets the current yearly gifts by month 
        /// </summary>
        /// <returns>Json object with months and sum of gifts during said months</returns>
        public Chart MonthlyReport()
        {
            Chart chart = new Chart();
            chart.labels = new string[] { "JAN", "FEB", "MAR", "ARL", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
            chart.datasets = new List<Datasets>();
           
            List<int> dataList = new List<int>();

            for (int i = 1; i <= 12; i++)
            {
                // Get the value from the database
                decimal? getSum = db.Gifts
                                    .Where(x => x.CallLog.DateOfCall.Month.Equals(i) &&
                                                x.CallLog.DateOfCall.Year.Equals(DateTime.Now.Year))
                                    .Sum(x => x.GiftAmount);

                if (getSum == null)
                    getSum = 0;
                // Convert to int from decimal
                int total = Convert.ToInt32(getSum);
                // Add to list
                dataList.Add(total);
            }
            // Convert list to array
            int[] list = dataList.ToArray();

            chart.datasets = CreateDataset("#ED6A5A", "rgba(237, 106, 90, 0.1)", list, DateTime.Now.Year.ToString());

            return chart;
        }

        /// <summary>
        /// Gets the current yearly gifts by month 
        /// </summary>
        /// <returns>Json object with months and sum of gifts during said months</returns>
        public Chart DailyReport()
        {
            Chart chart = new Chart();

            // Get the number of days in the current month
            int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            List<string> dayLabel = new List<string>();

            // Fill dayLabel array with strings representing the date
            for (int i = 1; i <= days; i++)
                dayLabel.Add(i.ToString());

            chart.labels = dayLabel.ToArray();
            chart.datasets = new List<Datasets>();

            List<int> dataList = new List<int>();

            for (int i = 1; i <= days; i++)
            {
                // Get the value from the database
                decimal? getSum = db.Gifts
                                    .Where(x => x.CallLog.DateOfCall.Day.Equals(i) &&
                                                x.CallLog.DateOfCall.Month.Equals(DateTime.Now.Month) &&
                                                x.CallLog.DateOfCall.Year.Equals(DateTime.Now.Year))
                                    .Sum(x => x.GiftAmount);

                if (getSum == null)
                    getSum = 0;
                // Convert to int from decimal
                int total = Convert.ToInt32(getSum);
                // Add to list
                dataList.Add(total);
            }
            // Convert list to array
            int[] list = dataList.ToArray();

            // Create label
            string setLabel = DateTime.Now.Month.ToString() + ", " + DateTime.Now.Year.ToString();

            // Add the dataset to the chart
            chart.datasets = CreateDataset("#9BC1BC", "rgba(155, 193, 188, 0.1)", list, setLabel);

            return chart;
        }
        #endregion

        /// <summary>
        /// Helper method for creating new datasets for the above charts. 
        /// </summary>
        /// <param name="borderColor">Color representation in string format for the border</param>
        /// <param name="backgroundColor">Color representation in string format for the background</param>
        /// <param name="data">The data input into the set</param>
        /// <param name="label">The label that will be displayed for the set</param>
        /// <returns></returns>
        private List<Datasets> CreateDataset(string borderColor, string backgroundColor, int[] data, string label)
        {
            List<Datasets> dataSet = new List<Datasets>();
            dataSet.Add(new Datasets()
            {
                label = label,
                data = data,
                borderColor = new string[] { borderColor },
                borderWidth = "1",
                backgroundColor = new string[] { backgroundColor }
            });
            return dataSet;
        }

    }
}