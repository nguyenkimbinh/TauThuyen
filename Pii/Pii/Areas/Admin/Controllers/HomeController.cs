using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PiiLibrary.Tools;
using Pii.Areas.Admin.Models;
namespace Pii.Areas.Admin.Controllers
{
    //[AdminAuthorize(Roles = "Administrators, Managers")]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FileManager()
        {
            return View();
        }
        public ActionResult Ckfinder()
        {
            return View();
        }

        [HttpPost]
        public JsonResult MakeSlug(string title)
        {
            string alias = PiiLibrary.Tools.Functions.MakeSlug(title);
            return Json(alias, JsonRequestBehavior.AllowGet);
        }
    }
}
