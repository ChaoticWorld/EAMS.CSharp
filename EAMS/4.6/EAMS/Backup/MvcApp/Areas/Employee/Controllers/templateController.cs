using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemBLL;
using Webdiyer.WebControls.Mvc;

namespace MvcApp.Areas.Employee.Controllers
{
        //
        // GET: /Employee/template/
        //模板控制器
        
    [AuthorizeEx(Roles = "Employee")]
    public class templateController : Controller
    {
        static LogsBLL logbll = new LogsBLL();
        SystemDB.User currUser;
        //静态业务操作对象,请更改:
        //static strategyLib.strategyBLL strategyBll = new strategyLib.strategyBLL();

        public ActionResult Index(int id = 1)
        {
            //查询结果,默认为全部
            List<object> list = new List<object>();
            //分页模型
            var model = list.ToPagedList(id, 20);
            if (Request.IsAjaxRequest())
                return PartialView("List", model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(FormCollection f, int id = 1)
        {
            //key为查询条件
            object key = new object();

            //查询并返回结果:
            List<object> list = new List<object>();

            int sizePage;
            if (f.AllKeys.Contains("sizePage"))
                int.TryParse(f["sizePage"].ToString(), out sizePage);
            else sizePage = 20;
            var model = list.ToPagedList(id, sizePage);
            if (Request.IsAjaxRequest())
                return PartialView("List", model);
            return View(model);
        }
    }
}
