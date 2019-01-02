using System.Web.Mvc;

namespace SystemSetup.Areas.Information
{
    public class InformationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Information";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Information_default",
                "Information/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "SystemSetup.Areas.Information.Controllers" }
            );
        }
    }
}
