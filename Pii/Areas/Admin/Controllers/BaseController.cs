using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using Pii.Areas.Admin.Models;
using Pii.Models;
namespace Pii.Areas.Admin.Controllers
{
    public abstract class BaseController : Controller
    {
        internal List<string> StylePaths = new List<string>();
        internal List<string> ScriptPaths = new List<string>();
        internal LayoutAdminModel AdminModel;
        internal PiiContext DbContext = new PiiContext();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Layout
            AdminModel = new LayoutAdminModel();

            ViewBag.AdminModel = AdminModel;
            base.OnActionExecuting(filterContext);
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            StylePaths.Add("~/Areas/Admin/Content/Style.css");

            ScriptPaths.Add("~/Areas/Admin/Scripts/ckeditor/ckeditor.js");
            ScriptPaths.Add("~/Areas/Admin/Scripts/ckfinder/ckfinder_v1.js");
            ScriptPaths.Add("~/Areas/Admin/Scripts/jquery.validate.min.js");
            ScriptPaths.Add("~/Areas/Admin/Scripts/jquery.validate.unobtrusive.min.js");
            ScriptPaths.Add("~/Areas/Admin/Scripts/functions.js");

            BundleTable.Bundles.Add(new StyleBundle("~/css").Include(StylePaths.ToArray()));
            BundleTable.Bundles.Add(new ScriptBundle("~/js").Include(ScriptPaths.ToArray()));
            base.EndExecute(asyncResult);
        }
    }
}
