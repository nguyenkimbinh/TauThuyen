using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using System.Web.Routing;
using Pii.Areas.Accounts.Models.ViewModels;
using Pii.Areas.Accounts.Models;
using Pii.Areas.Accounts.Enums;

namespace Pii.Areas.Accounts.Controllers
{
  
    public class HomeController : BaseController
    {
        public ActionResult InitialUsers()
        {
           // return null;
           var roles = new string[] { "manager", "administrators" };
            foreach (var item in roles) if (!Roles.RoleExists(item)) Roles.CreateRole(item);

            var users = new List<string[]> { new string[] { "admin", "adminvitc", "administrators", "wisky549@yahoo.com" } };
            foreach (var item in users)
            {
                if (!WebSecurity.UserExists(item[0]))
                {
                    WebSecurity.CreateUserAndAccount(item[0], item[1], new { Email = item[3] });
                    Roles.AddUserToRole(item[0], item[2]);
                }
            }
            return null;
        }                                                                                                                         

        #region Login - Logoff
        public ActionResult Login()
        {
            //InitialUsers();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model)
        {
            if (ModelState.IsValid && Membership.ValidateUser(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                return Redirect("/");
            }
            else ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng!.");
            return View(model);
        }

        [Authorize]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return RedirectToAction("Login", "Home", new { Area="Accounts"});
        }
        [Authorize(Roles="administrators")]
        public ActionResult Register()
        {
            ViewBag.roles = new SelectList(Roles.GetAllRoles().ToList());
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    string[] a = Roles.GetAllRoles();
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    Roles.AddUserToRole(model.UserName , model.Role);
                    return RedirectToAction("ViewAll", "Manage");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

       

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        // Lay hinh capcha:
        public ActionResult Image(string d)
        {
            var builder = new XCaptcha.ImageBuilder();
            var rd = new XCaptcha.RandomTextGenerator();
            var result = builder.Create(rd.Create(5, false));
            if (Session["xcaptchapii"] == null) Session.Add("xcaptchapii", result.Solution);
            else Session["xcaptchapii"] = result.Solution;
            return new FileContentResult(result.Image, result.ContentType);
        }
        #endregion
    }
}
