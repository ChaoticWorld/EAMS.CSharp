using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Areas.Common.Controllers
{
    public class RefCustomerController : Controller
    {
        /// <summary>
        /// 客户信息以Json返回
        /// </summary>
        /// <param name="id">客户编码</param>
        /// <returns></returns>
        public ActionResult cusInfoJson(string id)
        {
            CallInfo.CallDWInfo ci = new CallInfo.CallDWInfo(code: id);
            DataDB.ModelBase.Customer c = new DataDB.ModelBase.Customer();
            try
            {
                c = ci.CallDWModel.cusInfo.First(item => item.cCusCode.ToLower().Equals(id.ToLower()));
                var rcus = Json(c, "application/json", JsonRequestBehavior.AllowGet);
                return rcus;
            }
            catch { return new JsonResult(); }
        }
        public ActionResult searchGo(string id)
        {
            CallInfo.CallDWInfo ci;
            string searchKey = id;

            //模糊查询;电话，联系人，单位编码，单位名称,地址
            if (string.IsNullOrEmpty(id))
                ci = new CallInfo.CallDWInfo(key: "_AllCustomers_");
            else
                ci = new CallInfo.CallDWInfo(key: searchKey);
            //filter(ref ci, "客户经理");
            //是否有结果
            ViewData["hasResult"] = false;

            if (!ci.CallDWModel.IsNull && ci.CallDWModel.cusInfo != null && ci.CallDWModel.cusInfo.Count > 0)
                ViewData["hasResult"] = true;
            return View(ci.CallDWModel.cusInfo);
        }


    }
}
