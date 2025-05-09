using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using eUseControl.Web.App_Start;

namespace eUseControl.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
           AreaRegistration.RegisterAllAreas();
           RouteConfig.RegisterRoutes(RouteTable.Routes);

           BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();

            Response.Clear();
            Server.ClearError();
            Response.StatusCode = 404;

            Response.Redirect("~/Main/Error404?error=true");
        }
    }
}