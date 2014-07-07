using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class Variable
    {
        [Key]
        public int Id { get; set; }
        public string Attention { get; set; }
    }
}