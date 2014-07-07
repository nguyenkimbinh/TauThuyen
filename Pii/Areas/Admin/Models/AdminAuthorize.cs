using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pii.Areas.Admin.Models
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        private string _url = "/AccountsManager/Home/Logon";
        public string RedirectUrl { get { return _url; } set { _url = value; } }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var returnUrl = filterContext.HttpContext.Request.Url.AbsolutePath;
            filterContext.Result = new RedirectResult(RedirectUrl + "?returnUrl=" + returnUrl);
        }
    }
}
