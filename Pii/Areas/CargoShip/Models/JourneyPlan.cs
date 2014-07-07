using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class JourneyPlan
    {
        [Key]
        public int JourneyPlanId { get; set; }
        public int ShipId { get; set; }
        public string TripId { get; set; }
        public string PlanId { get; set; }
        [Required]
        public DateTime? PlanTime { get; set; }
        private string time;
        [NotMapped]
        public string Time
        {
            get { return PlanTime.HasValue ? PlanTime.Value.ToString("HH:mm") : time; }
            set
            {
                time = value;
            }
        }
        private string date;
        [NotMapped]
        public string Date
        {
            get { return PlanTime.HasValue ? PlanTime.Value.ToString("dd/MM/yyyy") : date; }
            set
            {
                date = value;
                if (value != null)
                {
                    PlanTime = DateTime.ParseExact(value + (time != null ? time : "00:00"), "dd/MM/yyyyHH:mm", null);
                }
            }
        }
        public int? PortId { get; set; }
        public virtual Plan Plan { get; set; }
        public virtual Port Port { get; set; }
        public virtual Ship Ship { get; set; }
        public string Updator { get; set; }
        public DateTime? UpdateTime { get; set; }
        //[ForeignKey("TripId")]
        public virtual Journey Journey { get; set; }
    }
}