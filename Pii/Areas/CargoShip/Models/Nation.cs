using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class Nation
    {
        [Key]
        public string NationId { get; set; }
        [Required(ErrorMessage = "*Vui lòng nhập tên quốc gia.")]
        public string NationName { get; set; }

        public virtual ICollection<Port> Ports { get; set; }
        public virtual ICollection<GoodsMakingPlace> GoodsMakingPlaces { get; set; }
    }
}