using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Pii.Areas.Admin.Models
{
    public class UpdatePage
    {
        public long NewsId { get; set; }
        public string Title { get; set; }

        [AllowHtml]
        [CKEditor(CKEditorMode.Full)]
        public string Content { get; set; }
    }
}