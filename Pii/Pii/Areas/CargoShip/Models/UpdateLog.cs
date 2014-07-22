using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class UpdateLog
    {
        [Key]
        public int Id { get; set; }
        public int ShipId { get; set; }
        public string TripId { get; set; }
        public string ContractNumber { get; set; }
        public string Category { get; set; }
        public string Weight { get; set; }
        public string StartPort { get; set; }
        public string StopPort { get; set; }
    }
}