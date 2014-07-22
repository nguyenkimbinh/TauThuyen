using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using Pii.Models;
namespace Pii.Controllers
{
    public abstract class BaseController : Controller
    {
        internal List<string> StylePaths = new List<string>();
        internal List<string> ScriptPaths = new List<string>();
        internal LayoutModel LayoutModel;
        internal PiiContext DbContext = new PiiContext();
        internal string Lang;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Languages:
            SetLanguageParam(filterContext);

            // Layout
            LayoutModel = new LayoutModel(DbContext, filterContext.HttpContext);
            try
            {
                LayoutModel.Warning = DbContext.Variables.FirstOrDefault().Attention;

            }
            catch (Exception)
            {

                LayoutModel.Warning = @"Ghi chú: Các thông tin cập nhật dựa trên các báo cáo từ tàu/ đại lý v..v.
                Có thể mang tính đơn phương, dự đoán chưa được công nhận bởi các bên liên quan. Để biết thêm thông
tin vui lòng liên hệ P.KTTV để có được Statement of Fleets của tàu tại các cảng có chữ ký/ được
công nhận bởi các bên liên quan.";
            }
            ViewBag.LayoutModel = LayoutModel;
            base.OnActionExecuting(filterContext);
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            StylePaths.Add("~/Content/Site.css");
            ScriptPaths.Add("~/Scripts/jquery-1.9.1.js");
            ScriptPaths.Add("~/Scripts/jquery.cycle.all.js");
            ScriptPaths.Add("~/Scripts/jquery.easing.1.3.js");
            ScriptPaths.Add("~/Scripts/jquery.watermark.js");
            ScriptPaths.Add("~/Scripts/stickytooltip.js");
            BundleTable.Bundles.Add(new StyleBundle("~/css").Include(StylePaths.ToArray()));
            BundleTable.Bundles.Add(new ScriptBundle("~/js").Include(ScriptPaths.ToArray()));
            base.EndExecute(asyncResult);
        }

        // hàm set ngôn ngữ
        private void SetLanguageParam(ActionExecutingContext filterContext, string defaultLangCode = "vi")
        {
            if (filterContext.RouteData.Values["lang"] != null &&
                !string.IsNullOrWhiteSpace(filterContext.RouteData.Values["lang"].ToString()))
            {
                // set the culture from the route data (url)
                var lang = filterContext.RouteData.Values["lang"].ToString();
                Lang = lang;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
            }
            else
            {
                // load the culture info from the cookie
                var cookie = filterContext.HttpContext.Request.Cookies["Pii.CurrentUICulture"];
                var langHeader = string.Empty;
                if (cookie != null)
                {
                    // set the culture by the cookie content
                    langHeader = cookie.Value;
                    Lang = langHeader;
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(langHeader);
                }
                else
                {
                    // set the culture by the location if not speicified
                    langHeader = String.IsNullOrWhiteSpace(defaultLangCode) ? filterContext.HttpContext.Request.UserLanguages[0] : defaultLangCode;
                    Lang = langHeader;
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(langHeader);
                }
                // set the lang value into route data
                filterContext.RouteData.Values["lang"] = langHeader;
            }
            HttpCookie _cookie = new HttpCookie("Pii.CurrentUICulture", Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
            _cookie.Expires = DateTime.Now.AddYears(1);
            filterContext.HttpContext.Response.SetCookie(_cookie);
        }
    }
}
