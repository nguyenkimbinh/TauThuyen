using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pii.Areas.CargoShip.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pii.Areas.CargoShip.ViewModel
{
    public class HomeViewModel
    {
        public string ShipName { get; set; }
        public DateTime? StartDate { get; set; }
        public string TripId { get; set; }
        public int ShipId { get; set; }
        public string FromTo { get; set; }
        public string CurPosAndState { get; set; }
    }
}