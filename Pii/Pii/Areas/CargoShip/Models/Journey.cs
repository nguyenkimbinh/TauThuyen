using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pii.Areas.CargoShip.Models
{
    public class Journey
    {
        [Key]
        public int JourneyId { get; set; }
        [Display(Name="Tàu")]
        public int ShipId { get; set; }
        [Display(Name="Mã Chuyến")]
        public string TripId { get; set; }
        [Display(Name="Số Hợp Đồng")]
        public string ContractNumber { get; set; }
        public Nullable<int> StartPortId { get; set; }
        [Display(Name="Ngày Bắt Đầu")]
        public DateTime? StartDate { get; set; }
        [Display(Name="Khối Lượng")]
        public Single Weight { get; set; }
        [Display(Name="Ngày Kết Thúc")]
        public DateTime? EndDate { get; set; }
        public string StartNationName { get; set; }
        public string StopNationName { get; set; }
        [Display(Name="Cảng")]
        [ForeignKey("StartPortId")]
        public virtual Port Port { get; set; }
        [Display(Name="Tàu")]
        public virtual Ship Ship { get; set; }
        [Display(Name="Nơi Làm Hàng")]
        public virtual GoodsMakingPlace GoodMakingPlace { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Shipment> Shipments { get; set; }
        public virtual ICollection<Result> Results { get; set; }
        public virtual ICollection<JourneyPlan> JourneyPlans { get; set; }
        public virtual ICollection<JourneySupervision> JourneySupervisions { get; set; }
        
    }
}