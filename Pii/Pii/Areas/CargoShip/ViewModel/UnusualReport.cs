using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.ViewModel
{
    public class UnusualReport
    {
        public string PortName { get; set; }
        public string Purpose { get; set; }
        public DateTime? ComingDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public DateTime? RunTime { get; set; }
    }
}