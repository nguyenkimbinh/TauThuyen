using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class Shipment
    {
        [Key]
        public int ShipmentId { get; set; }
        public int ShipId { get; set; }
        public string TripId { get; set; }
        public string ContractNumber { get; set; }
        public int? CategoryId { get; set; }
        public Single Weight { get; set; }
        public virtual Ship Ship { get; set; }
        //[ForeignKey("TripId")]
        public virtual Journey Journey { get; set; }
        public virtual GoodsMakingPlace GoodsMakingPlace { get; set; }
        public virtual Category Category { get; set; }
    }
}