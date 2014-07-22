using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pii.Areas.CargoShip.Models;
namespace Pii.Areas.CargoShip.ViewModel
{
    public class DetailViewModel
    {
        public DetailViewModel()
        {
            Journeys = new List<Journey>();
            States = new List<State>();
            Tasks = new List<Task>();
            JourneySupervisions = new List<JourneySupervision>();
            Notes = new List<Note>();
            JourneyPlans = new List<JourneyPlan>();
            Ports = new List<Port>();
            CreatedDates = new List<DateTime?>();
        }
        public virtual List<DateTime?> CreatedDates { get; set; }
        public virtual List<Journey> Journeys { get; set; }
        public virtual List<JourneySupervision> JourneySupervisions { get; set; }
        public virtual List<JourneyPlan> JourneyPlans { get; set; }
        public virtual List<Note> Notes { get; set; }
        public virtual Ship Ship { get; set; }
        public string SelectedTripId { get; set; }
        public int? SelectedTaskId { get; set; }
        public int? SelectedPortId { get; set; }
        public DateTime? SelectedTime { get; set; }
        public int? SelectedStateId { get; set; }
        public virtual List<Task> Tasks { get; set; }
        public virtual List<State> States { get; set; }
        public virtual List<Port> Ports { get; set; }
        public virtual Result Result { get; set; }
        public virtual List<Result> Results { get; set; }
    }
}