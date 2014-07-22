using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }
        [Required(ErrorMessage = "*Vui lòng nhập tên trạng thái.")]
        public string StateName { get; set; }
        public virtual ICollection<JourneySupervision> JourneySupervisions { get; set; }
        public virtual ICollection<Compare> Compares { get; set; }
        public virtual ICollection<Result> Results { get; set; }

    }
}