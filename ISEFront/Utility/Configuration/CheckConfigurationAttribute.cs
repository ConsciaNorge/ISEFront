using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace ISEFront.Utility.Configuration
{
    public class CheckConfigurationAttribute : ActionFilterAttribute
    {
        public string Controller { get; set; }
        public string Action { get; set; }

        public CheckConfigurationAttribute(string controller, string action)
        {
            Controller = controller;
            Action = action;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                ComponentSpace.SAML2.SAMLController.Initialize();
            }
            catch (ComponentSpace.SAML2.Exceptions.SAMLConfigurationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == Controller &&
                    filterContext.ActionDescriptor.ActionName == Action)
                    return;

                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new {
                            controller = "Dashboard",
                            action = "FirstRun"
                        }
                    )
                );
            }
        }
    }
}