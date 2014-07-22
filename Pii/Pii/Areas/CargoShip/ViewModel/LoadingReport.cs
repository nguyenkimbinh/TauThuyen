using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.ViewModel
{
    public class LoadingReport
    {
        public string PortName { get; set; }
        public string Goods { get; set; }
        public DateTime? PortComeDate { get; set; }
        public string ComingCube { get; set; }
        public DateTime? PortComeTime { get; set; }
        public DateTime? LoadingTime { get; set; }
        public DateTime? LoadingComplTime { get; set; }
        public DateTime? LeavingTime { get; set; }
        public string LeavingCube { get; set; }
    }
}