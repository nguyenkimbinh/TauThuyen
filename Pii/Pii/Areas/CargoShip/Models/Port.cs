using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class Port
    {
        [Key]
        public int PortId { get; set; }
        [Required(ErrorMessage = "*Vui lòng nhập tên cảng.")]
        public string PortName { get; set; }
        [Required(ErrorMessage="*Vui lòng chọn mã quốc gia.")]
        public string NationId { get; set; }

        public virtual Nation Nation { get; set; }
        
        public virtual ICollection<Journey> Journeys { get; set; }
        public virtual ICollection<JourneyPlan> JourneyPlans { get; set; }
        public virtual ICollection<JourneySupervision> JourneySupervisions { get; set; }
        public virtual ICollection<GoodsMakingPlace> GoodsMakingPlaces { get; set; }
        
    }
}