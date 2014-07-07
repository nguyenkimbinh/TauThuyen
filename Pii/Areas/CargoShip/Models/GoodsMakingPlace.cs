using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class GoodsMakingPlace
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public int Id { get; set; }
        [Key,Column(Order=1), Display(Name="Số Dự Án")]
        public string ContractNumber { get; set; }
        [Key, Column(Order = 2), Display(Name="Quốc Gia")]
        public string NationId { get; set; }
        [Display(Name="Cảng")]
        public int? PortId { get; set; }
        [Display(Name="Ghi Chú")]
        public string Note { get; set; }
        [Display(Name="Loại")]
        public bool Kind { get; set; }
        public virtual ICollection<Journey> Journeys { get; set; }
        [ForeignKey("PortId"), Display(Name = "Cảng")]
        public virtual Port Port { get; set; }
        [ForeignKey("NationId"), Display(Name = "Quốc Gia")]
        public virtual Nation Nation { get; set; }
        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}