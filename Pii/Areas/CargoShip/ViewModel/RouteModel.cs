using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pii.Areas.CargoShip.Models;

namespace Pii.Areas.CargoShip.ViewModel
{
    public class RouteModel
    {
        //public string Date { get; set; }
        //public string Time { get; set; }
        //public string PortName { get; set; }
        //public string Position { get; set; }
        //public string CategoryName { get; set; }
        //public string TaskName { get; set; }
        //public string StateName { get; set; }
        //public string Result { get; set; }
        //public string PlanId { get; set; }
        //public string PlanName { get; set; }
        //public string Plan { get; set; }
        public virtual ICollection<JourneySupervision> JourneySupervisions { get; set; }
        public virtual ICollection<JourneyPlan> JourneyPlans { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Result> Results { get; set; }
        public RouteModel()
        {
            JourneyPlans = new List<JourneyPlan>();
            JourneySupervisions = new List<JourneySupervision>();
            Notes = new List<Note>();
            Results = new List<Result>();
        }
    }
}