using Dialogistic.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dialogistic.Controllers
{
    // Only let users in the "Administrator" and "SuperAdmin" roles create other users
    [Authorize(Roles = "Administrator, SuperAdmin")]
    public class UserController : Controller
    {
        #region Set up
        ApplicationUserManager _userManager;
        ApplicationRoleManager _roleManager;

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
        #endregion

        #region CreateUser
        /// <summary>
        /// Displays the new account creation form where users in the "Administrator" role
        /// can create new user accounts.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
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

            // Store the list to be used in the view, and return the view
            ViewBag.Roles = list;
            return View();
        }

        /// <summary>
        /// Allows users in the "Administrator" role to create new user accounts using a valid
        /// email address, while also specifying which role the account should be created in.
        /// </summary>
        /// <param name="model">The model used for account creation.</param>
        /// <returns>Returns this view if there are errors, otherwise returns the "CreationConfirmation" view.</returns>
        [HttpPost]
        public async Task<ActionResult> Create(CreateUserModel model)
        {
            // If the model is valid, proceed
            if (ModelState.IsValid)
            {
                // Create a new instance of an ApplicationUser, using the email provided
                var user = new ApplicationUser { FirstName = "", LastName = "", UserName = model.Email, Email = model.Email };

                // Create the user *without* the password -- this allows the user to set 
                // their own password when completing their registration
                var result = await UserManager.CreateAsync(user);

                // If the user account was created successfully, send the email with the activation link
                if (result.Succeeded)
                {
                    // Immediately add the user to the selected role
                    result = await UserManager.AddToRoleAsync(user.Id, model.Role);

                    // Send the email and redirect to the creation confirmation page
                    await SendActivationMail(user);
                    return RedirectToAction("CreateConfirmation");
                }

                // Display any errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

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

            // Store the list to be used in the view, and return the view
            ViewBag.Roles = list;
            return View(model);
        }
        #endregion

        #region Activate
        /// <summary>
        /// Sends a link to the supplied user's email address that allows them to finsh
        /// activating their account.
        /// </summary>
        /// <param name="user">The user account to send the email to.</param>
        /// <returns></returns>
        private async Task SendActivationMail(ApplicationUser user)
        {
            // Instantiate an instance of IdentityMessage
            IdentityMessage message = new IdentityMessage();

            // Generate an access code for this email message
            string unformattedCode = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id); //await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            unformattedCode = Regex.Replace(unformattedCode, @"[+//]", "");
            unformattedCode = Regex.Unescape(unformattedCode);
            string code = unformattedCode.Substring(0, 64);

            // Use the protocol param to force the creation of an absolut url
            var callbackUrl = Url.Action("CompleteRegistration", "Account", new { userId = user.Id, code }, protocol: Request.Url.Scheme);

            // The body of the email message
            string body = @"<h4>Welcome to the organization!</h4>
                            <p>To complete your registration, please <a href=""" + callbackUrl + @""">activate</a> your account.</p>
                                <p>You must complete your account registration within 24 hours from receving this mail.</p>
                                    <p>This is an automated email. Please do not reply to it, as we will not see it.</p>";

            // Set the message details
            message.Subject = "Account Registration";
            message.Body = body;
            message.Destination = user.Email;

            // Send the email message
            await UserManager.EmailService.SendAsync(message);
        }

        /// <summary>
        /// Displays the account creation confirmation view.
        /// </summary>
        /// <returns>Returns the view.</returns>
        [HttpGet]
        public ActionResult CreateConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Used to display form errors in the view.
        /// </summary>
        /// <param name="result">IdentiyResult containing any errors from the form.</param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion
    }
}