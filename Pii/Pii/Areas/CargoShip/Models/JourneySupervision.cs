using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class JourneySupervision
    {
        [Key]
        public int Id { get; set; }
        public int ShipId { get; set; }
        public string TripId { get; set; }
        public int TaskId { get; set; }
        public int? PortId { get; set; }
        public DateTime? CreatedDate { get; set;}
        private string time;
        [NotMapped]
        public string Time
        {
            get { return CreatedDate.HasValue ? CreatedDate.Value.ToString("HH:mm") : time; }
            set
            {
                time = value;
            }
        }
        private string date;
        [NotMapped]
        public string Date
        {
            get { return CreatedDate.HasValue ? CreatedDate.Value.ToString("dd/MM/yyyy") : date; }
            set
            {
                date = value;
                if (value != null)
                {
                    CreatedDate = DateTime.ParseExact(value + (time != null ? time : "00:00"), "dd/MM/yyyyHH:mm", null);
                }
            }
        }
        public int? StateId { get; set; }
        public string Position { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]*", ErrorMessage = "*"), DefaultValue(0)]
        public Single ArrangedWeight { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]*", ErrorMessage = "*")]
        public Single RestWeight { get; set; }
        public virtual Ship Ship { get; set; }
        public virtual Port Port { get; set; }
        public virtual Task Task { get; set; }
        public virtual State State { get; set; }
        public virtual Journey Journey { get; set; }
        public string Note { get; set; }
        public string CategoryName { get; set; }
        public string Updator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string NationName { get; set; }
    }
}