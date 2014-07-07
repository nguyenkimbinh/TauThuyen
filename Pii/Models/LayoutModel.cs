using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pii.Models
{
    public class LayoutModel
    {
        private PiiContext Db;
        private HttpContextBase context;
        public LayoutModel(PiiContext _DbContext, HttpContextBase _HttpContext)
        {
            this.Db = _DbContext;
            this.context = _HttpContext;
        }
        public bool IsAuthenticated { get { return context.User.Identity.IsAuthenticated; } }
        public string Lang { get; set; }
        public string Warning { get; set; }
    }
}