using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pii.Models;
using Pii.Areas.CargoShip.Models;
using System.ComponentModel.DataAnnotations;

namespace Pii.Areas.CargoShip.ViewModel
{
    public class UpdateViewModel
    {
        [Required(ErrorMessage="*")]
        public string ContractNumber { get; set; }
        //[Required(ErrorMessage="*")]
        public virtual IList<int?> CategoryIds { get; set; }
        public virtual IList<Single?> Weights { get; set; }
        //[Required(ErrorMessage = "*")]
        public virtual IList<int?> StartPortIds { get; set; }
        //[Required(ErrorMessage = "*")]
        public virtual IList<int?> StopPortIds { get; set; }
        public virtual Ship Ship { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}"), Required(ErrorMessage = "*")]
        public DateTime? CurrentStartTime { get; set; }
        public string CurrentTripId { get; set; }
        public string StartNationId { get; set; }
        public string StopNationId { get; set; }
        public virtual JourneySupervision LoadGoodsJourney { get; set; }
        public virtual JourneySupervision UnloadGoodsJourney { get; set; }
        public virtual JourneySupervision JourneyInProgress { get; set; }
        public virtual JourneySupervision UnusualJourney { get; set; }
        public virtual JourneyPlan JourneyPlan { get; set; }
        public virtual Note Note { get; set; }
        public virtual ICollection<Port> AvailablePorts { get; set; }
        public virtual ICollection<Category> AvailableCategories { get; set; }
        public virtual ICollection<State> AvailableStates { get; set; }
        public virtual ICollection<Nation> AvailableNations { get; set; }
        public virtual ICollection<string> AvailableTripIds { get; set; }
        public virtual ICollection<Plan> AvailablePlans { get; set; }
        public int InsertOrUpdate { get; set; }
        public virtual ICollection<UpdateLog> Logs { get; set; }
        public byte IsLate { get; set; }
        public int? rStateId { get; set; }
        public string rPlanId { get; set; }
        public int rShipId { get; set; }
    }
}