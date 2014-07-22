using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.ViewModel
{
    public class ReportViewModel
    {
        public LoadingReport OnLoadReport { get; set; }
        public LoadingReport UnloadReport { get; set; }
        public UnusualReport UnsualReport { get; set; }
    }
}