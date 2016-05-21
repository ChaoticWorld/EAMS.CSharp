using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemDB;
using SystemBLL;

namespace MvcApp.Areas.Manager.Controllers
{
    public class LogController : Controller
    {
        private Logs log = new Logs();
        private LogsBLL logbll = new LogsBLL();

        //
        // GET: /Manager/Log/

        public ActionResult Index()
        {
            var r = logbll.Lists("");
            return View(r);
        }

        //
        // GET: /Manager/Log/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Manager/Log/Create

        public ActionResult Create()
        {
            log = new Logs();
            log.iUserID = 1;
            log.iActionId = 1;
            log.iFunctionId = 1;
            logbll.Add(log);
            return RedirectToAction("Index");
            //return View();
        }

        //
        // POST: /Manager/Log/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manager/Log/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Manager/Log/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manager/Log/Delete/5

        public ActionResult Delete(int id)
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
