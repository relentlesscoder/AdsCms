using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AdCms.Web.Windsor;

namespace AdCms.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            WindsorControllerFactory controllerFactory = new WindsorControllerFactory(WindsorContainerFactory.Instance);

            // use windsor controller factory
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //ToDo: register global filters, for future implementation
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //ToDo: for future implementation
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            WindsorContainerFactory.Instance.Dispose();
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Item", // Route name
                "{controller}/{id}/{title}", // URL with parameters
                new { action = "Index", id = @"\d+" }
            );

            routes.MapRoute(
                "Action", // Route name
                "{controller}/{action}" // URL with parameters
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }
    }
}