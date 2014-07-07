using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class Ship
    {
        public Ship()
        {
            State = true;
        }
        [Key]
        public int ShipId { get; set; }
        [Required(ErrorMessage="*Vui lòng nhập tên tàu.")]
        public string ShipName { get; set; }
        public bool State { get; set; }
        public virtual ICollection<Journey> Journeys { get; set; }
        public virtual ICollection<JourneyPlan> JourneyPlans { get; set; }
        public virtual ICollection<JourneySupervision> JourneySupervisions { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Shipment> Shipments { get; set; }
        public virtual ICollection<Result> Results { get; set; }
        
        
    }
}