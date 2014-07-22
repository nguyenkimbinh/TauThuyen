using System.Web.Mvc;

namespace Pii.Areas.CargoShip
{
    public class CargoShipAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CargoShip";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CargoShip_default",
                "{controller}/{action}",
                new {controller="Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
