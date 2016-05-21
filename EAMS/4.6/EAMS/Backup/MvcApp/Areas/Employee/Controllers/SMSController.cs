using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemBLL;

namespace MvcApp.Areas.Employee.Controllers
{
    public class SMSController : Controller
    {
        //
        // GET: /Employee/SMS/

        [AuthorizeEx(Roles="Admin")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
