using System;
using System.Web.Mvc;
using MvcApp.Models;
using Logs;

namespace MvcApp.Areas.Manager.Controllers
{
    public class LogController : Controller
    {
        private LogsDataAccess logDA = new SysLogsDataAccess();

        //
        // GET: /Manager/Log/

        public ActionResult Index()
        {
            var r = logDA.selectLogs(null);
            return View(r);
        }

        //
        // GET: /Manager/Log/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }


        //
        // POST: /Manager/Log/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
