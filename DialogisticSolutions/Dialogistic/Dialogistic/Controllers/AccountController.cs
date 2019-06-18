using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Dialogistic.Models;
using System.Text.RegularExpressions;
using Dialogistic.DAL;

namespace Dialogistic.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Setup

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private DialogisticContext db = new DialogisticContext();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
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

        #region AccountLogin
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // Prevent a logged in user from returning to the log in page
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Home");
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            // Set the return url
            ViewData["ReturnUrl"] = returnUrl;

            // If the model isn't in a valid state, immediately return the view
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if username/password pair match the currently logged in user, and update the security stamp if so
            var loggedInUser = await UserManager.FindAsync(model.Email, model.Password);
            if (loggedInUser != null)
            {
                // Update the security stamp
                await UserManager.UpdateSecurityStampAsync(loggedInUser.Id);
            }

            // Set a flag to check for email versus username
            bool isEmailAddress = false;

            // Check the model's Email input for an '@' symbol
            if (model.Email.IndexOf('@') > -1)
            {
                // Build a regular expression to validate the email format
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                       @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                          @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);

                // Compare the input to the regex, letting the user know if it's an invalid format
                if (!re.IsMatch(model.Email))
                {
                    ModelState.AddModelError("Email", "Email is not valid");
                }

                // Set the flag 
                isEmailAddress = true;
            }
            else
            { // No '@' was present, this must be a username

                // Build a regular expression to validate username format
                string emailRegex = @"^[a-zA-Z0-9]*$";
                Regex re = new Regex(emailRegex);

                // Compare the input to the regex, letting the user know if it's an invalid format
                if (!re.IsMatch(model.Email))
                {
                    ModelState.AddModelError("Email", "Username is not valid");
                }
            }

            // If the model is valid, try signing in with either the username or email
            if (ModelState.IsValid)
            {
                // Get the data from the model
                var userName = model.Email;

                // Check for an '@' again, to see if it's an email
                if (isEmailAddress)
                {
                    // Try to find the user by email address
                    var user = await UserManager.FindByEmailAsync(model.Email);

                    // If user is null, it means we didn't find a match in the table
                    if (user == null)
                    {
                        // Display invalid login attempt message and return the same view
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);

                        // If user ISN'T null, it means we found a match in the table
                    }
                    else
                    {
                        // Grab the user name from the model
                        userName = user.UserName;

                        // Now user have entered correct username and password.
                        // Time to change the security stamp
                        await UserManager.UpdateSecurityStampAsync(user.Id);
                    }
                }

                // Sign in, using either the email or username as determined above
                var result = await SignInManager.PasswordSignInAsync(userName, model.Password, model.RememberMe, shouldLockout: false);

                // Switch case to handle the result of the above login attempt
                switch (result)
                {
                    case SignInStatus.Success:

                        return RedirectToAction("Index", "Home");

                    case SignInStatus.LockedOut:
                        return View("Lockout");

                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }

            return View(model);
        }
        #endregion

        #region VerifyCode
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);

                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }
        #endregion

        #region AccountRegistration
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/CompleteRegistration
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> CompleteRegistration(string userId, string code)
        {
            // Prevent a logged in user from returning to the registration page
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            // Prevent attempts to create duplicate user accounts
            var existingAccount = db.UserProfiles.Where(x => x.UserID == userId).FirstOrDefault();
            if (existingAccount != null)
            {
                return View("Error");
            }

            // If either of the parameters are missing, display an error
            if (code == null || userId == null)
            {
                return View("Error");
            }

            // Make sure there's a user associated with this ID in the AspIdentity tables
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Pass the model in with the userId and code for verification on completion
                CompleteRegistrationViewModel model = new CompleteRegistrationViewModel
                {
                    UserID = userId,
                    Code = code
                };

                // Return the view with the model
                return View(model);
            }

            // If we got this far, something went wrong
            return View("Error");
        }

        // POST: /Account/CompleteRegistration
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CompleteRegistration(CompleteRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Get the current user through their UserID
                var user = await UserManager.FindByIdAsync(model.UserID);

                // If the user isn't null, we found a match, so update the account
                if (user != null)
                {
                    // Update it with the values from the view model
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.EmailConfirmed = true;

                    // Assign the password to this user account
                    var addPassword = await UserManager.AddPasswordAsync(model.UserID, model.Password);
                    var result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        // Apply the changes
                        UserManager.Update(user);

                        // Create the entry for the UserProfile table
                        UserProfile newUser = new UserProfile
                        {
                            UserID = model.UserID,
                            FullName = model.FirstName + " " + model.LastName,
                            UserName = model.UserName,
                            Email = user.Email,
                            IsCaller = false,
                            CallsRemaining = 0,
                            DonationsRaised = 0
                        };

                        // Set all Standard users to be callers -- Admins will not be counted as callers for now
                        var userEntry = await UserManager.FindAsync(newUser.Email, model.Password);
                        var roles = await UserManager.GetRolesAsync(user.Id);
                        if (roles.Contains("Standard"))
                        {
                            newUser.IsCaller = true;
                        }

                        // Add them to the table and save the changes
                        db.UserProfiles.Add(newUser);
                        db.SaveChanges();

                        // Redirect to the login page (TO-DO: UPDATE THIS TO THE APPROPRIATE DASHBOARD BASED ON THE USER'S ROLE)
                        return RedirectToAction("Login", "Account");
                    }

                    // Display any errors
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                    // The password wasn't added successfully -- don't update the account and return the view
                    return View(model);
                }

                // There was no match -- just return the view
                return View(model);
            }

            // Invalid model state -- return the view
            return View(model);
        }
        #endregion

        #region ForgotPassword
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region LogOff
        // POST: /Account/LogOff
        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
        #endregion

        #region Theme
        public JsonResult GetTheme()
        {
            // Get the current User ID
            var ID = User.Identity.GetUserId();
            // Get the current user selected theme
            string theme = db.UserProfiles.Where(x => x.UserID.Equals(ID))
                                          .Select(x => x.SiteTheme).FirstOrDefault();

            // If theme is unset or null return standard theme
            if(theme == null)
            {
                theme = "Standard";
            }

            return Json(theme, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}