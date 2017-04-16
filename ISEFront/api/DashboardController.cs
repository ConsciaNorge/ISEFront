using ISEFront.api.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;

namespace ISEFront.api
{
    public class DashboardController : ApiController
    {
        // TODO : Consider caching dashboard actions as a singleton to eliminate the need for scanning all classes
        // TODO : Consider optimizing the LINQ pattern below to require fewer iterations
        [HttpGet]
        [Route("api/dashboard/sidepanelitems")]
        public HttpResponseMessage SidePanelItems()
        {
            Assembly asm = Assembly.GetAssembly(typeof(MvcApplication));

            var controlleractionlist = asm.GetTypes()
                .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => m.DeclaringType.Name == "DashboardController" && !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                .Select(x => new
                {
                    Controller = x.DeclaringType.Name.Replace("Controller", ""),
                    Action = x.Name,
                    Dashboard = x.GetCustomAttributes()
                        .Where(a => a.GetType().Name == "DashboardAttribute")
                        .Select(a => new
                        {
                            Attribute = a as Utility.DashboardAttribute
                        })
                        .FirstOrDefault()
                })
                .Where(x => x.Dashboard != null)
                //.OrderBy(x => x.Action)
                .ToList()
                .Select(x => new DashboardSidePanelItemViewModel
                {
                    ActionUrl = Url.Link("Default", new {
                        Controller = x.Controller,
                        Action = x.Action
                    }),

                    Title = x.Dashboard.Attribute.Title
                })
                .ToList();

            var jsonText = JsonConvert.SerializeObject(controlleractionlist);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(jsonText, Encoding.UTF8, "application/json");

            return response;
        }
    }
}