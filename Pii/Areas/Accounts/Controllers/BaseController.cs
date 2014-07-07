using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pii.Filters;

namespace Pii.Areas.Accounts.Controllers
{
    [InitializeSimpleMembership]
    public abstract class BaseController : Pii.Controllers.BaseController
    {
        public BaseController()
        {
            StylePaths.Add("~/Areas/Accounts/Content/Style.css");
        }
    }
}
