using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }
        [Required(ErrorMessage = "*Vui lòng nhập tên công việc.")]
        public string TaskName { get; set; }
        public virtual ICollection<JourneySupervision> JourneySupervisions { get; set; }
    }
}