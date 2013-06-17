using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NavigationRoutes;

namespace VacationVillaManager
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapNavigationRoute("HomeNav", "Home", "", new { controller = "Home", action = "Index" });
            routes.MapNavigationRoute("HousesNav", "Houses", "Houses", new { controller = "Houses", action = "Index" });
            routes.MapNavigationRoute("BookingsNav", "Bookings", "Bookings", new { controller = "Bookings", action = "Index" });
            routes.MapNavigationRoute("InquiriesNav", "Inquiries", "Inquiries", new { controller = "Inquiries", action = "Index" });
            routes.MapNavigationRoute("SpecialsNav", "Specials", "Specials", new { controller = "Specials", action = "Index" });
            routes.MapNavigationRoute("ReportsNav", "Reports", "Reports", new { controller = "Reports", action = "Create" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}