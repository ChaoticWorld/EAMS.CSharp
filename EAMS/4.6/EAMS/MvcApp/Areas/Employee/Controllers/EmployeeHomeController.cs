using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApp.Models;

namespace MvcApp.Areas.Employee.Controllers
{
    [AuthorizeEx(Roles="Employee")]
    public class EmployeeHomeController : Controller
    {
        //
        // GET: /Employee/Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
