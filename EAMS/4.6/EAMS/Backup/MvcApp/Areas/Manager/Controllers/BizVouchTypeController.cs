using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataDB.ModelBase;
namespace MvcApp.Areas.Manager.Controllers
{
    [SystemBLL.AuthorizeEx(Roles="Admin")]
    public class BizVouchTypeController : Controller
    {
        //
        // GET: /Manager/BizVouchType/

        public ActionResult Index()
        {
            VouchType vtBll = new DataDB.ModelBase.VouchType();
            var vtList = vtBll.getList(new DataDB.ModelBase.VouchType());
            
            return View(vtList);
        }
        [HttpPost]
        public JsonResult saveVouchType(VouchType vt) {
            JsonResult r = new JsonResult() {
                Data = -1, MaxJsonLength = 102400 };

            VouchType vtBll = new DataDB.ModelBase.VouchType();
            long id = vt.id;
            if (vt.id > 0) vtBll.Update(vt);
            else id = vtBll.Create(vt);
            r.Data = (int)id;
            return r;
        
        }
        [HttpPost]
        public JsonResult deleteVouchType(int id)
        {
            JsonResult r = new JsonResult() {
                Data = -1, MaxJsonLength = 102400 };
            int rn = -1;
            VouchType vtBll = new DataDB.ModelBase.VouchType();
            if (id > 0) rn = vtBll.Delete(id);
            r.Data = rn;
            return r;
        }
    }
}
