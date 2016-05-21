using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Areas.Manager.Controllers
{
    public class ManagerHomeController : Controller
    {
        //
        // GET: /Manager/Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
