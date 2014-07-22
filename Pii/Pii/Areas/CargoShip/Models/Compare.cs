using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class Compare
    {
        [Key]
        public int CompareId { get; set; }
        [Display(Name="Dự Kiến")]
        public string PlanId { get; set; }
        [Display(Name="Trạng Thái")]
        public int StateId { get; set; }
        [ForeignKey("PlanId"), Display(Name = "Dự Kiến")]
        public virtual Plan Plan { get; set; }
        [ForeignKey("StateId"), Display(Name = "Trạng Thái")]
        public virtual State State { get; set; }
        
    }
}