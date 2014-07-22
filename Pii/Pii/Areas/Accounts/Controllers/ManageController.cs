using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Pii.Areas.Accounts.Controllers
{
    //[Authorize(Roles = "administrators")]
    public class ManageController : BaseController
    {
        #region Search User
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string UserName)
        {
            var user = DbContext.UserProfiles.SingleOrDefault(m => m.UserName == UserName);
            if (user == null)
            {
                ViewBag.Message = "Not found user";
                ViewBag.UserName = UserName;
                return View(UserName);
            }
            return RedirectToAction("View", new { Id = user.UserId });
        }
        public ActionResult ViewAll()
        {
            var users = DbContext.UserProfiles.ToList();
            return View(users);
        }
      
        public ActionResult View(int Id)
        {
            var user = DbContext.UserProfiles.Find(Id);
            return View(user);
        }
        #endregion

        [HttpPost]
        public ActionResult Delete(int UserId)
        {
            var user = DbContext.UserProfiles.Find(UserId);
            if (user.GetRole().ToString() != "Administrators")
            {
                if (Roles.GetRolesForUser(user.UserName).Count()>0)
                {
                    Roles.RemoveUserFromRoles(user.UserName, Roles.GetRolesForUser(user.UserName));
                }
                DbContext.UserProfiles.Remove(user);
                DbContext.SaveChanges();
            }
            return RedirectToAction("ViewAll","Manage");
        }

        #region Reset Password For User
        public ActionResult ResetPasswordForUser()
        {
            return View();
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            DbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}
