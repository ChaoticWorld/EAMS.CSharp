using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApp.Models;

namespace MvcApp.Areas.Manager.Controllers
{
    [AuthorizeEx(Roles ="Admin")]
    public class HomeController : Controller
    {
        public HomeController() {
            ;
        }
        //
        // GET: /Manager/Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
