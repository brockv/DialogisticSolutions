using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Configuration;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.IO;
using Dialogistic.DAL;

namespace Dialogistic.Models
{
    public class Geolocation
    {
        private DialogisticContext db = new DialogisticContext();
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public void GetGeocode(string address)
        {
            var urlAddress = address.Replace(" ", "+");
            // Get the API key from the setting file
            string Key = ConfigurationManager.AppSettings["MQ_KEY"];
            string siteReq = "https://www.mapquestapi.com/geocoding/v1/address?key=" + Key + "&location=" + address + "&thumbMaps=false";
        
            var req = WebRequest.Create(siteReq);
            req.ContentType = "application/json; charset=utf-8";
            var resp = (HttpWebResponse)req.GetResponse();

            string text;

            using (var stream = new StreamReader(resp.GetResponseStream()))
            {
                text = stream.ReadToEnd();
                stream.Close();
            }

            Latitude = JObject.Parse(text)["results"][0]["locations"][0]["latLng"]["lat"].ToString();
            Longitude = JObject.Parse(text)["results"][0]["locations"][0]["latLng"]["lng"].ToString();
        }

    }
}