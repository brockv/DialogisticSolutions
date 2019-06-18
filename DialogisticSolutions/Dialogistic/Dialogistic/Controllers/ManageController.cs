using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Dialogistic.Models;
using System.IO;
using Amazon.S3;
using Amazon;
using Amazon.S3.Transfer;
using Amazon.Runtime;
using System.Configuration;
using Dialogistic.DAL;

namespace Dialogistic.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        #region Boring Stuff
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private DialogisticContext db = new DialogisticContext();

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region UpdateUserProfiles
        /// <summary>
        /// Loads the view for the user to update their information. Populates fields in the view
        /// with existing information.
        /// </summary>
        /// <returns>The view, with input fields populated with any existing information.</returns>
        [HttpGet]
        public async Task<ActionResult> UpdateUserProfile()
        {
            // Get the current user
            var currentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            // Get the account associated with this user
            UserProfile profile = db.UserProfiles.Where(x => x.UserID.Equals(currentUser.Id)).FirstOrDefault();

            // ONLY proceed if the value wasn't null (i.e., we found the user in the database)
            if (profile != null)
            {
                // Create an instance of the update model and populate the values
                UpdateUserProfileViewModel user = new UpdateUserProfileViewModel
                {
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    UserName = profile.UserName,
                    Email = profile.Email,
                    Avatar = profile.Avatar,
                    SiteTheme = profile.SiteTheme
                };

                // Load the view with the update model using the above information
                return View(user);
            }

            // We should never get here, but if we do, just load the view without the model information
            return View();
        }

        /// <summary>
        /// Updates the users information by overwriting existing information.
        /// </summary>
        /// <param name="model">The model used for updating this information.</param>
        /// <returns>Redirects back to the INdex page on success, or this view on failure -- with errors displayed.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUserProfile(UpdateUserProfileViewModel model, HttpPostedFileBase file)
        {
            // Find the profile associated with this user
            var profile = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            UserProfile userProfile = db.UserProfiles.Where(x => x.UserID.Equals(profile.Id)).FirstOrDefault();

            // If the model state is valid and there's a profile associated with this ID, proceed
            if (ModelState.IsValid && userProfile != null)
            {
                #region Profile Picture
                /////////////////////////////////////////////////////////////////////////
                //                                                                     //
                // This section handles uploading and storing a user's profile picture //
                //                                                                     //
                /////////////////////////////////////////////////////////////////////////

                // Only proceed if the file passed in isn't null and has content
                if (file != null && file.ContentLength > 0)
                {
                    // Don't allow files over 50KB
                    if (file.ContentLength > 5000000)
                    {
                        ViewBag.Message = "The size of the file should not exceed 5 MB";
                        return View();
                    }

                    // Limit the file types we allow for images
                    var supportedTypes = new[] { "jpg", "jpeg", "png", "gif", "bmp" };

                    // Make sure the uploaded image is one of those types
                    var fileExt = Path.GetExtension(file.FileName.ToLower()).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        ViewBag.Message = "Invalid file type. Only the following types (jpg, jpeg, png, gif, bmp) are supported.";
                        return View();
                    }

                    // Set the bucket, region identifier, and URL prefix
                    string bucketName = "dialogistic-uploads";
                    RegionEndpoint bucketRegion = RegionEndpoint.USWest2;
                    string prefix = "https://s3-us-west-2.amazonaws.com/dialogistic-uploads/images/";

                    // Instantiate a connection to the bucket via an S3 client, and assign the transfer utility
                    var credentials = new BasicAWSCredentials(ConfigurationManager.AppSettings["S3_ACCESS_KEY"], ConfigurationManager.AppSettings["S3_SECRET_KEY"]);
                    IAmazonS3 s3Client = new AmazonS3Client(credentials, bucketRegion);
                    TransferUtility fileTransferUtility = new TransferUtility(s3Client);

                    // Initialize the key and string to hold the complete URL in
                    string keyName = "images/" + file.FileName;
                    string avatarURL = null;
                    
                    // Attempt to upload the image to the S3 bucket
                    try
                    {
                        // Instantiate a file transfer request, setting the storage and ACL parameters
                        TransferUtilityUploadRequest fileTransferUtilityRequest = new
                        TransferUtilityUploadRequest
                        {
                            StorageClass = S3StorageClass.Standard, // Standard access is what we want for these images
                            CannedACL = S3CannedACL.PublicRead // Images must have public read access to view in the application
                        };
                        
                        // Convert the image to an input stream so we can upload it
                        MemoryStream fileToUpload = new MemoryStream();
                        file.InputStream.CopyTo(fileToUpload);

                        // Upload the image to the S3 bucket in the images directory
                        fileTransferUtility.Upload(fileToUpload, bucketName, keyName);                                                
                    }
                    catch (AmazonS3Exception amazonS3Exception) // Amazon S3 specific exceptions
                    {
                        Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", amazonS3Exception.Message);
                    }
                    catch (Exception e) // All other exceptions
                    {
                        Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                    }

                    // If the upload was successful, update the user's avatar
                    avatarURL = prefix + file.FileName;
                    userProfile.Avatar = avatarURL;

                    // Update the model's avatar as well -- it's overwritten as an empty string otherwise
                    model.Avatar = userProfile.Avatar;
                    ViewBag.Message = "Image uploaded successfully!";
                }
                // There wasn't a file that needed to be uploaded -- set the model avatar to the user's existing value
                else
                {
                    model.Avatar = userProfile.Avatar;
                }
                #endregion

                #region Profile Information
                /////////////////////////////////////////////////////////////////////////////
                //                                                                         //
                // This section handles updating and storing a user's personal information //
                //                                                                         //
                /////////////////////////////////////////////////////////////////////////////

                // Get the current user from the AspNetUsers table and update their
                ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;
                user.Email = model.Email;

                // Attempt to update the user's information
                var updateResult = await UserManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    // If the update succeeded, update their information in our UserProfiles table
                    userProfile.FullName = user.FirstName + " " + user.LastName;
                    db.Entry(userProfile).CurrentValues.SetValues(model);
                    db.SaveChanges();
                    
                    return View(model);                    
                }
                #endregion

                // Add any errors found to the model state for displaying
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            // There were issues with the model state-- return the view with any errors found above
            return View(model);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Loads the view to delete the currently logged in user's account.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Delete()
        {
            // Get the currently logged in user, and send it to the view
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            return View(user);
        }

        /// <summary>
        /// Deletes the currently logged in user.
        /// </summary>
        /// <returns>Redirects to the home page on success, and stays on this page on failure.</returns>
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed()
        {
            // Get the currently logged in user so that we can delete it
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            // Delete the user, redirecting to the home page on success
            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            // The user wasn't deleted -- stay on this page for now -- [TO-DO: DISPLAY AN ERROR MESSAGE]
            return View();
        }
        #endregion

        #region ManageLink
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
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

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}