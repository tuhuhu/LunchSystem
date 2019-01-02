using System.Web.Mvc;

namespace SystemSetup.Areas.LunchRegister
{
    public class LunchRegisterAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "LunchRegister";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "LunchRegister_default",
                "LunchRegister/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
