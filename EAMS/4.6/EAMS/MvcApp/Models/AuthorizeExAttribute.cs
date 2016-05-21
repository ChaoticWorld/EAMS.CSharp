using System;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Models
{
    public class AuthorizeExAttribute : AuthorizeAttribute
    {
        private static string authorBeforeUrl = string.Empty;
        public static string AuthorBeforeReturnURL { get { return authorBeforeUrl; } }
        private static bool isPass = false;
        public static bool Pass { get { return isPass; } }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool r = base.AuthorizeCore(httpContext);
            isPass = r;
            if (r)
                authorBeforeUrl = httpContext.Request.Url.AbsolutePath;
            return r;
        }
    }
}