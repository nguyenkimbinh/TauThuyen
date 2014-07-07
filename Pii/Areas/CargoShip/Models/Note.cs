using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }
        public int ShipId { get; set; }
        public string TripId { get; set; }
        public string NoteContent { get; set; }
        private DateTime? createdDate;
        public DateTime? CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }
        public virtual Ship Ship { get; set; }
        public virtual Journey Journey { get; set; }
        private string date;
        [NotMapped]
        public string Date
        {
            get { return CreatedDate == null ? "" : CreatedDate.Value.ToString("dd/MM/yyyy"); }
            set
            {
                date = value;
                if (value != null)
                {
                    try
                    {
                        createdDate = DateTime.ParseExact(value, "dd/MM/yyyy", null);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        public DateTime? UpdateTime { get; set; }
        public string Updator { get; set; }
    }
}