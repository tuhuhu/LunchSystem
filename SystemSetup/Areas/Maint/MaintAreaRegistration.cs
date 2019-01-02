using System.Web.Mvc;

namespace SystemSetup.Areas.Maint
{
    public class MaintAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Maint";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Maint_default",
                "Maint/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
