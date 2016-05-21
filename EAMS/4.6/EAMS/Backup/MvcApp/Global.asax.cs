using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Security.Principal;
using System.Web.Security;
using SystemBLL;
using System.Threading;

namespace MvcApp
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
        public MvcApplication()
        {
            AuthorizeRequest += new EventHandler(MvcApplication_AuthorizeRequest);
            //PostAuthenticateRequest += new EventHandler(MvcApplication_OnPostAuthenticateRequest);
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
        void MvcApplication_OnPostAuthenticateRequest(object sender, EventArgs e)
        {
        }

    }
}