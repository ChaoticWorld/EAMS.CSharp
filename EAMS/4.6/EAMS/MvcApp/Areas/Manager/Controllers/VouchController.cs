using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VouchType;
using Webdiyer.WebControls.Mvc;
using MvcApp.Models;

namespace MvcApp.Areas.Manager.Controllers
{[AuthorizeEx(Roles ="Employee,Admin")]
    public class VouchController : Controller
    {
        // GET: Manager/VouchType
        public ActionResult Index(int id = 1)
        {
            VouchType.VouchType vt = new VouchType.VouchType();
            List<VouchType.VouchType> vouchTypes = new List<VouchType.VouchType>();
            vouchTypes = VouchType.VouchType.Init().OrderBy(o => o.vouchClass).ThenBy(o => o.vouchType).ThenBy(o => o.vouchOrder).ToList();
            var model = vouchTypes.ToPagedList(id, 20);
            var clsSelect = vouchTypes.Select(s => s.vouchClass);
            clsSelect = clsSelect.Union(clsSelect);
            TempData["vtClass"] = new SelectList(clsSelect, clsSelect.First());
            var typeSelect = vouchTypes.Select(s => s.vouchType);
            typeSelect = typeSelect.Union(typeSelect);
            TempData["vtType"] = new SelectList(typeSelect, typeSelect.First());
            if (Request.IsAjaxRequest())
                return PartialView("List", model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormCollection f, int id)
        {
            VouchType.VouchType vt = new VouchType.VouchType();
            if (f.AllKeys.Contains("vtClass"))
                vt.vouchClass = f["vtClass"].ToString();
            if (f.AllKeys.Contains("vtType"))
                vt.vouchType = f["vtType"].ToString();
            List<VouchType.VouchType> vouchTypes = new List<VouchType.VouchType>();
            vouchTypes = vt.getList(vt);
            var model = vouchTypes.ToPagedList(id, 20);

            if (Request.IsAjaxRequest())
                return PartialView("List", model);
            return View(model);
        }
        [AuthorizeEx(Roles = "Admin")]
        [HttpPost]
        public JsonResult Save(int id,VouchType.VouchType vt) {
            JsonResult r = new JsonResult() { };
            long newid = -1;
            if (vt.id < 0)
                newid = vt.Create(vt);
            else newid = vt.Update(vt);
            r.Data = newid;
            return r;
        }
        [AuthorizeEx(Roles = "Admin")]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            JsonResult r = new JsonResult() { };
            VouchType.VouchType vt = new VouchType.VouchType();
            var rd = vt.Delete(id);
            r.Data = rd;
            return r;
        }
        [HttpPost]
        public JsonResult getTypeWithClass(string id)
        {
            JsonResult r = new JsonResult() { MaxJsonLength = 4000 };
            VouchType.VouchType vt = new VouchType.VouchType();
            var list = vt.getList(new VouchType.VouchType() { vouchClass = id }).Select(s=>s.vouchType);
            list = list.Union(list);
            r.Data = list;
            return r;
        }

        [AuthorizeEx(Roles = "Employee")]
        [HttpPost]
        public JsonResult getVouchOrder(VouchType.VouchType vt, int id = -1)
        {
            JsonResult r = new JsonResult();
            if (null != vt)
            {
                vt = vt.getList(vt).First();
                r.Data = vt.vouchOrder;
            }
            return r;
        }
    }
}