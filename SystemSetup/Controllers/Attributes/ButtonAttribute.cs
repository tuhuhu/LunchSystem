using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iEnterAsia.iseiQ.Controllers.Attributes
{
    public class ButtonAttribute : ActionMethodSelectorAttribute
    {
        public string ButtonName { get; private set; }

        public ButtonAttribute(string buttonName)
        {
            this.ButtonName = buttonName;
        }

        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            return (controllerContext.Controller.ValueProvider.GetValue(this.ButtonName) !=  null);
        }

    }
}