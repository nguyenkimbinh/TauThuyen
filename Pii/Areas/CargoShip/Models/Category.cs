using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "*Vui lòng nhập tên hàng."), Display(Name="Loại Hàng")]
        public string CategoryName { get; set; }
        public virtual ICollection<Shipment> Shipments { get; set; }

    }
}