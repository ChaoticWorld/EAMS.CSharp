using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Principal;
using System.Web.Security;
using System.Web.Optimization;


namespace MvcApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        public MvcApplication()
        {
            AuthorizeRequest += new EventHandler(MvcApplication_AuthorizeRequest);
        }
        void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            var id = Context.User.Identity as FormsIdentity;
            if (id != null && id.IsAuthenticated)
            {
                var roles = id.Ticket.UserData.Split(',');
                Context.User = new GenericPrincipal(id, roles);
            }
        }
    }
}
