using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class Reson
    {
        [Key]
        public int ResonId { get; set; }
        [Required(ErrorMessage = "*Vui lòng nhập lý do.")]
        public string ResonName { get; set; }
        public virtual ICollection<Result> Results { get; set; }

    }
}