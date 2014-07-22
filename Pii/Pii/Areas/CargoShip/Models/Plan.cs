using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class Plan
    {
        [Key]
        public string PlanId { get; set; }
        [Required(ErrorMessage = "*Vui lòng nhập dự kiến.")]
        public string PlanName { get; set; }
        public virtual ICollection<Compare> Compares { get; set; }
        public virtual ICollection<Result> Results { get; set; }
        
    }
}