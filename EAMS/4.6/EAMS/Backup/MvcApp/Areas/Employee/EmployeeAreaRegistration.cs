using System.Web.Mvc;

namespace MvcApp.Areas.Employee
{
    public class EmployeeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Employee";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Employee_default",
                "Employee/{controller}/{action}/{id}/{param}",
                new { action = "Index", id = UrlParameter.Optional, param = UrlParameter.Optional }
            );
        }
    }
}
