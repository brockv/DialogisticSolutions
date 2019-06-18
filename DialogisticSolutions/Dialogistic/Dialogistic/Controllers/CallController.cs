using System.Web.Mvc;
using Dialogistic.Domain.Twilio;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace Dialogistic.Controllers
{
    public class CallController : TwilioController
    {
        #region Setup
        // Initialize credentials used for placing requests to Twilio
        private readonly ICredentials _credentials;

        /// <summary>
        /// Default, 0-argument constructor.
        /// </summary>
        public CallController() : this(new Credentials()) { }

        /// <summary>
        /// 1-argument constructor that initializes the credentials.
        /// </summary>
        /// <param name="credentials">Twilio credentials to be initialized.</param>
        public CallController(ICredentials credentials)
        {
            _credentials = credentials;
        }
        #endregion


        /// <summary>
        /// This creates the connection between Twilio and our application, and enables
        /// browser-to-phone calls to be placed.
        /// </summary>
        /// <param name="phoneNumber">The phone number to have Twilio call - the Constituent's number./// </param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Connect(string phoneNumber)
        {
            // Instantiate a new VoiceResponse
            var response = new VoiceResponse();

            // Initialze a Dial object with the phone number passed in with the credentials
            var dial = new Dial(callerId: _credentials.PhoneNumber);

            // If the phone number isn't null, set up the call to the Constituent
            if (phoneNumber != null)
            {
                dial.Number(phoneNumber);
            }

            // Append the Dial object to our response to Twilio
            response.Append(dial);

            // Return the TwiML built above to Twilio so the call can be placed
            return TwiML(response);
        }
    }
}