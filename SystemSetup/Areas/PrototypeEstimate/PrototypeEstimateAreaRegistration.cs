using System.Web.Mvc;

namespace iEnterAsia.iseiQ.Areas.PrototypeEstimate
{
    public class PrototypeEstimateAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PrototypeEstimate";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PrototypeEstimate_default",
                "PrototypeEstimate/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
