using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApp.Models;
using Webdiyer.WebControls.Mvc;
using strategyLib;
using UserBLL;
using UserInfo;
using Logs;
using VouchType;
using MvcApp.Areas.Common.Models;

namespace MvcApp.Areas.Employee.Controllers
{
    [AuthorizeEx(Roles = "Employee")]
    public class StrategyController : Controller
    {
        static LogsDataAccess logDA = new SysLogsDataAccess();
        userBLL userBll = new userBLL();
        static UserModel currUser;
        static strategyDAL strategyDal = new strategyDAL();
        
        public StrategyController()  {  }
        //
        // GET: /Employee/Strategy/

        public ActionResult InvStdRatePrice(int id = 1)
        {
            currUser = userBll.single(int.Parse(HttpContext.User.Identity.Name));
            logDA.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Strategy",
                cClass = "StrategyController",
                cFunction = "InvStdRatePrice",
                cReturn = "Open Strategy InvStdRatePrice Page!"
            });
            InvStdConvertDAL invConvert = new InvStdConvertDAL();
            //InvConvertBLL invConvert = new InvConvertBLL();
            var mod = invConvert.getList(null).OrderBy(o => o.invClsID).OrderBy(o => o.autoid);
            var model = mod.ToPagedList(id, 20);
            if (Request.IsAjaxRequest())
                return PartialView("InvStdList", model);
            return View(model);
        }

        [HttpPost]
        public ActionResult InvStdRatePrice(FormCollection f, int id = 1)
        {
            //搜索条件整理
            string invClass = null;
            if (f.AllKeys.Contains("invClass")) invClass = f["invClass"].ToString();
            InvClsStdConvertRate rate = new InvClsStdConvertRate() { invClsName = invClass };
            InvStdConvertDAL invConvert = new InvStdConvertDAL();

            var mod = invConvert.getList(rate).OrderBy(o => o.invClsID).OrderBy(o => o.autoid);
            var model = mod.ToPagedList(id, 20);
            if (Request.IsAjaxRequest())
                return PartialView("InvStdList", model);
            return View(model);
        }
        [HttpPost]
        public JsonResult rateSave(InvClsStdConvertRate rate,int id) {
            JsonResult r = new JsonResult() { MaxJsonLength = 1024 };
            InvStdConvertDAL rateDal = new InvStdConvertDAL();
            var _rate = rateDal.Retrieve(rate.autoid);
            if (_rate == null) r.Data = rateDal.Create(rate);
            else {  rateDal.Update(rate); r.Data = _rate.autoid;}
            return r;
        }
        [HttpPost]
        public JsonResult rateDelete(int id = -1)
        {
            JsonResult r = new JsonResult() { MaxJsonLength = 1024 };
            InvStdConvertDAL rateDal = new InvStdConvertDAL();
            r.Data = rateDal.Delete(id);
            return r;
        }

        public ActionResult Index(int id = 1)
        {
            currUser = userBll.single(int.Parse(HttpContext.User.Identity.Name));
            logDA.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Strategy",
                cClass = "StrategyController",
                cFunction = "Index",
                cReturn = "Open Strategy Default Page!"
            });
            var Strategies = strategyDal.getVWStrategys(null).OrderByDescending(o=>o.dEffDate);

            var model = Strategies.ToPagedList(id, 20);

            if (Request.IsAjaxRequest())
                return PartialView("List", model);
            return View(model);

        }
        [HttpPost]
        public ActionResult Index(FormCollection f, int id = 1)
        {
            bool isnull = true;
            //搜索条件整理
            vwStrategy m = new vwStrategy();
            if (f.AllKeys.Contains("DWName"))
            { m.cDWName = f["DWName"].ToString(); isnull = false; }
            if (f.AllKeys.Contains("DWContact"))
            { m.cDWContact = f["DWContact"].ToString(); isnull = false; }
            if (f.AllKeys.Contains("DWCode"))
            { m.cDWCode = f["DWCode"].ToString(); isnull = false; }
            if (f.AllKeys.Contains("DCName"))
            { m.cDCName = f["DCName"].ToString(); isnull = false; }
            if (f.AllKeys.Contains("VouchCode"))
            { m.cVouchCode = f["VouchCode"].ToString(); isnull = false; }
            if (f.AllKeys.Contains("SourceCode"))
            { m.cSourceCode = f["SourceCode"].ToString(); isnull = false; }
            if (f.AllKeys.Contains("dEffDate") && !string.IsNullOrEmpty(f["dEffDate"].ToString()))
            { m.dEffDate = DateTime.Parse(f["dEffDate"].ToString()); isnull = false; }
            if (f.AllKeys.Contains("dExpDate") && !string.IsNullOrEmpty(f["dExpDate"].ToString()))
            { m.dExpDate = DateTime.Parse(f["dExpDate"].ToString()); isnull = false; }

            if (f.AllKeys.Contains("invACode"))
            { m.cinvACode = f["invACode"].ToString(); isnull = false; }
            if (f.AllKeys.Contains("invAName"))
            { m.cinvAName = f["invAName"].ToString(); isnull = false; }
            if (f.AllKeys.Contains("invAStd"))
            { m.cinvAStd = f["invAStd"].ToString(); isnull = false; }

            if (f.AllKeys.Contains("invBCode"))
            { m.cinvBCode = f["invBCode"].ToString(); isnull = false; }
            if (f.AllKeys.Contains("invBName"))
            { m.cinvBName = f["invBName"].ToString(); isnull = false; }
            if (f.AllKeys.Contains("invBStd"))
            { m.cinvBStd = f["invBStd"].ToString(); isnull = false; }

            //var Strategies = strategyBll.getStrategies(sKey);
            var Strategies = strategyDal.getVWStrategys(isnull ? null : m);
            var model = Strategies.ToPagedList(id, 20);
            if (Request.IsAjaxRequest())
                return PartialView("List", model);
            return View(model);
        }

        public ActionResult Edit(int id = -1, string param = null)
        {
            ExtRef extRef = new ExtRef("u8");
            cStrategy strategy = new cStrategy();
            strategy.main = (Main) strategyDal.getStrategyMain(id);

            mainDAL mDal = new mainDAL();
            detailDAL dDal = new detailDAL();
            var model = new Models.strategyEditModel();
            if (id > 0)
            {
                model.Main = mDal.Retrieve(id);
                var ds = dDal.getList(new Detail() { ID = id });
                model.Details = ds.ToPagedList(1, 20);
            }
            else
            {
                model.Main = new Main()
                {
                    ID = -1,
                    dEffDate = DateTime.Now,
                    dExpDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1),
                    cModifier = currUser.cUserName
                };
                model.Details = null;
            }
            var listVouchType = VouchType.VouchType.Init();
            var listErpVouchType = listVouchType.Where(w => w.vouchClass == "ERP");
            TempData["listERP"] = new SelectList(listErpVouchType, "KEY", "vouchName", model.Main.cVouchType);
            //TempData["listERP"] = new SelectList(listErpVouchType, "vouchName", "vouchName", model.Main.cVouchType);
            var listEamsVouchType = listVouchType.Where(w => w.vouchClass == "EAMS");
            TempData["listEAMS"] = new SelectList(listEamsVouchType, "KEY", "vouchName", model.Main.cSourceType);
            //TempData["listEAMS"] = new SelectList(listEamsVouchType, "vouchName", "vouchName", model.Main.cSourceType);
            var dc = extRef.extDistrict.getList(string.Empty);
            TempData["listDC"] = new SelectList(dc, "dcName", "dcName", model.Main.cDCName);
            if (!string.IsNullOrEmpty(param) && param == "copy") { model.Main.ID = -1;
                foreach (var d in model.Details) { d.autoid = -1; d.ID = -1; }
            }
            if (Request.IsAjaxRequest())
                return PartialView("strategyDetails", model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, int pageIdx = 1)
        {
            mainDAL mDal = new mainDAL();
            detailDAL dDal = new detailDAL();
            var model = new Employee.Models.strategyEditModel();
            if (id > 0)
            {
                model.Main = mDal.Retrieve(id);
                var ds = dDal.getList(new Detail() { ID = id });
                model.Details = ds.ToPagedList(pageIdx, 20);
            }
            else
            {
                model.Main = new Main() { ID = -1 };
                model.Details = null;
            }
            var listVouchType = VouchType.VouchType.Init();
            var listErpVouchType = listVouchType.Where(w => w.vouchClass == "ERP").OrderBy(o => o.vouchOrder);
            //TempData["listERP"] = new SelectList(listErpVouchType, "KEY", "vouchName", model.Main.cVouchType);
            TempData["listERP"] = new SelectList(listErpVouchType, "vouchName", "vouchName", model.Main.cVouchType);

            var listEamsVouchType = listVouchType.Where(w => w.vouchClass == "EAMS").OrderBy(o => o.vouchOrder);
            //TempData["listEAMS"] = new SelectList(listEamsVouchType, "KEY", "vouchName", model.Main.cSourceType);
            TempData["listEAMS"] = new SelectList(listEamsVouchType, "vouchName", "vouchName", model.Main.cSourceType);
            if (Request.IsAjaxRequest())
                return PartialView("strategyDetails", model);
            return View(model);
        }

        [HttpPost]
        public JsonResult saveHead(Main m)
        {
            JsonResult r = new JsonResult() { MaxJsonLength = 1024 };
            mainDAL mDal = new mainDAL();
            long id = -1;
            bool isExist = false;
            isExist = mDal.Exists(m);
            if (isExist)
            { mDal.Update(m); id = m.ID; }
            else id = mDal.Create(m);
            r.Data = id;
            return r;
        }

        [HttpPost]
        public JsonResult saveDetail(Detail d)
        {
            JsonResult r = new JsonResult() { MaxJsonLength = 1024 };
            detailDAL dDal = new detailDAL();
            long id = -1;
            bool isExist = false;
            isExist = dDal.Exists(d);
            if (isExist)
            { dDal.Update(d); id = d.autoid; }
            else id = dDal.Create(d);
            r.Data = id;
            return r;
        }

        [HttpPost]
        public JsonResult deleteDetail(int id)
        {
            JsonResult r = new JsonResult() { MaxJsonLength = 1024 };
            detailDAL dDal = new detailDAL();
            int rd = dDal.Delete(id);
            r.Data = rd;
            return r;
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            JsonResult r = new JsonResult() { MaxJsonLength = 1024 ,Data = -1};
            mainDAL mDal = new mainDAL();
            detailDAL dDal = new detailDAL();
            long[] did = dDal.getList(new Detail() { autoid = -1, ID = id }).Select(s => s.autoid).ToArray();
            int rd = 0;
            foreach (long dautoid in did) rd += dDal.Delete(dautoid);
            if (rd == did.Length) {
                r.Data = mDal.Delete(id);
            }
            return r;
        }
        [HttpPost]
        //检查此策略对应单据是否合法
        public JsonResult checkErpVouch(int id)
        {
            JsonResult r = new JsonResult() { MaxJsonLength = 204800 };
            string validStr = strategyDal.isValid(id);
            var l = validStr.Length;
            r.Data = validStr;
            return r;
        }
    }
}
