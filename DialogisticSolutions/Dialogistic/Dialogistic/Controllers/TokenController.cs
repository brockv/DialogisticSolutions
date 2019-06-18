using System;
using System.Web.Mvc;
using Dialogistic.Domain.Twilio;

namespace Dialogistic.Controllers
{
    public class TokenController : Controller
    {
        // Initialize credentials used for placing requests to Twilio
        private readonly ICredentials _credentials;

        /// <summary>
        /// Default, 0-argument constructor.
        /// </summary>
        public TokenController() : this(new Credentials()) { }

        /// <summary>
        /// 1-argument constructor that initializes the credentials.
        /// </summary>
        /// <param name="credentials">Twilio credentials to be initialized.</param>
        public TokenController(ICredentials credentials)
        {
            _credentials = credentials;
        }

        /// <summary>
        /// Generates a token to be used for Twilio calls.
        /// </summary>
        /// <param name="page">Where Twilio sends the call request.</param>
        /// <returns>The call format, as determined by Twilio.</returns>
        public JsonResult Generate(string page)
        {
            var token = new Capability(_credentials).Generate(InferRole(page));
            return Json(new { token }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Used to determine which role the individual placing the call is in.
        /// </summary>
        /// <param name="page">Where Twilio sends the call request.</param>
        /// <returns>The role of the individual placing the call.</returns>
        private static string InferRole(string page)
        {
            return page.Equals("/Voice", StringComparison.InvariantCultureIgnoreCase)
                ? "caller" : "constituent";
        }
    }
}