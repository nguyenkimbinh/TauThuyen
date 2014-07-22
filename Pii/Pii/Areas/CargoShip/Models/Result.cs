using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class Result
    {
        [Key]
        public int ResultId { get; set; }
        public int ShipId { get; set; }
        public string PlanId { get; set; }
        public DateTime? PlanDate { get; set; }
        public Nullable<int> StateId { get; set; }
        public DateTime? JourneyDate { get; set; }
        public string TripId { get; set; }
        public string Remark { get; set; }
        public int? ResonId { get; set; }
        public int? PortId { get; set; }
        public virtual Ship Ship { get; set; }
        public virtual Journey Journey { get; set; }
        public virtual Plan Plan { get; set; }
        public virtual State State { get; set; }
        public virtual Reson Reson { get; set; }
        public virtual Port Port { get; set; }
    }
}