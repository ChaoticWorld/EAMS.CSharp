using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemBLL;

namespace MvcApp.Areas.Employee.Controllers
{
    [AuthorizeEx(Roles="Employee")]
    public class PurchaseController : Controller
    {
        //
        // GET: /Employee/Purchase/

        public ActionResult Index()
        {
            //菜单
            return View();
        }

    }
}
