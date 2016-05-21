using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MvcApp.Models;
using MvcApp.Areas.Employee.Models;
using report;
using Webdiyer.WebControls.Mvc;
using System.Data;
using System.Text;

namespace MvcApp.Areas.Employee.Controllers
{
    [AuthorizeEx(Roles = "Admin,Employee")]
    public class ReportController : Controller
    {
        private static UserInfo.UserModel currUser;
        private ReportBLL rBll = new ReportBLL();
        private DataTable _dt = new DataTable();
        private static List<Rpt> Rpts = new List<Models.Rpt>();
        private Rpt Rpt = new Rpt();
        private static int PageNums = 20;
        public ReportController()
        { }
        ~ReportController() {
            Rpts.RemoveAll(e => e.userID == currUser.iUserId);
        }
        private void getCurrUser()
        {
            if (currUser == null || currUser.iUserId.ToString() != HttpContext.User.Identity.Name)
                currUser = UserBLL.userBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
        }
        private void getCurrUserRpt(long id)
        {

            getCurrUser();
            Rpt = Rpts.Find(f => f.userID == currUser.iUserId && f.rptID == id);
            if (Rpt == null)
            {
                Rpt = new Rpt() { userID = currUser.iUserId, rptID = id, report = rBll.getReport(id) };
                Rpts.Add(Rpt);
            }
        }
        // GET: Employee/Report
        public ActionResult Index()
        {
            getCurrUser();
            reportViewModel rm = new reportViewModel();
            var models = rm.getReports(currUser.iUserId);
            long uid = currUser.iUserId;
            UserBLL.userBLL uBll = new UserBLL.userBLL();
            TempData["isAdmin"] = uBll.getStrRoles(uid).Contains("Admin");
            return View(models);
        }
        #region View
        public ActionResult View(long id)
        {
            var _rpt = rBll.getReport(id);
            switch (_rpt.Class.ClassName)
            {
                case "明细表":
                    return new RedirectResult("/Employee/report/MXView/" + id.ToString());
                case "汇总表":
                    return new RedirectResult("/Employee/report/pivotView/" + id.ToString());
                default:
                    return null;
            }

        }
        public ActionResult MXView(long id)
        {
            getCurrUserRpt(id);
            try
            {
                submitFilters(id);//设置Rpts[id]
                
                ViewData["pages"] = (int)decimal.Ceiling((decimal)Rpt.dataTable.Rows.Count / (decimal)PageNums);
                ViewData["records"] = Rpt.dataTable.Rows.Count;
            }
            catch (Exception e)
            {
                ViewData["pages"] = 0;
                ViewData["records"] = "[0 |error:" + e.Message + " 数据为空，请修改过滤条件；或者联系管理员！]";
            }
            ViewData["pageNums"] = PageNums;
            ViewData["rptID"] = id;
            TempData["rptTitle"] = Rpt.report.Title;
            ViewData["DataTime"] = Rpt.dataTime.ToString("yyyy-MM-dd HH:mm:ss");
            return View();
        }
        public ActionResult ViewList(int id, int param = 1)
        {
            DataTable dtview;
            getCurrUserRpt(id);
            if (Rpt.dataTable.Rows.Count > 0)
                dtview = getCurrPage(Rpt.dataTable, PageNums, param - 1);
            else dtview = new DataTable();
            return View(dtview);
        }
        private DataTable getCurrPage(DataTable dt, int pageNums, int idx)
        {
            DataTable _r = new DataTable();
            _r = dt.Clone();
            int recordMax = dt.Rows.Count;
            int pageRecordMax = Math.Min((idx + 1) * pageNums, recordMax);
            int pageRecordMin = idx * pageNums;
            for (int i = pageRecordMin; i < pageRecordMax; i++)
                _r.Rows.Add(dt.Rows[i].ItemArray);
            return _r;
        }
        [HttpPost]
        public int setPageNums(int id)
        {
            PageNums = id;
            return PageNums;
        }
        private void getViewBag()
        {
            ViewBag.modules = null;
            reportModuleDataAccess rmda = new reportModuleDataAccess();
            ViewBag.modules = rmda.selects(null);
            ViewBag.classs = null;
            reportClassDataAccess rcda = new reportClassDataAccess();
            ViewBag.classs = rcda.selects(null);
        }
        #endregion
        #region Permissions
        [AuthorizeEx(Roles = "Admin")]
        public ActionResult reportPermissions()
        {
            UserBLL.userBLL uBll = new UserBLL.userBLL();

            ViewData["rptLists"] = rBll.getReports();
            ViewData["userLists"] = uBll.selects(new UserInfo.UserModel()).ToList();
            var Model = rBll.getPermissions(-1);
            foreach (Permission p in Model)
            {
                p.userName = uBll.single(p.UserID).cUserName;
                p.reportName = rBll.getReport(p.reportID).Name;
            }
            return View(Model);
        }
        [HttpPost]
        public JsonResult savePermission(long id, List<Permission> Permissions)
        {
            JsonResult r = new JsonResult();
            long[] rc = new long[Permissions.Count];
            if (Permissions != null && Permissions.Count > 0)
            {
                foreach (Permission p in Permissions)
                {
                    if (p.AutoID < 0)
                        rc[p.index] = rBll.reportPermissionDA.Create(p);
                    else
                    { rBll.reportPermissionDA.Update(p); rc[p.index] = p.AutoID; }
                }
            }
            r.Data = rc;
            return r;
        }
        [HttpPost]
        public JsonResult deletePermission(long id)
        {
            JsonResult r = new JsonResult();
            r.Data = rBll.deletePermission(id);
            return r;
        }
        #endregion
        #region Filter
        public ActionResult ViewFilter(long id, bool isDefault = false)
        {
            getCurrUser();
            var fields = rBll.getFields(id);
            if (fields != null && fields.Count > 0)
                fields.RemoveAll(r => r.isErpEmployeeVisit);
            ViewData["Fields"] = fields;
            List<FilterField> filters;
            if (isDefault)
                filters = rBll.getFilter(id, -999);
            else
            {
                filters = rBll.getFilter(id, currUser.iUserId);
                if (filters == null || filters.Count < 1) filters = rBll.getFilter(id, -999);
            }
            return View(filters);
        }
        [HttpPost]
        public JsonResult submitFilters(long id, bool isDefault = false)
        {
            var r = getMXData(id, isDefault);
            return r;
        }
        private DataTable getMXDataTable(long id, bool ispreview)
        {
            StringBuilder filterBuilder = new StringBuilder();

            getCurrUser();
            List<FilterField> filters;
            if (ispreview)
                filters = rBll.getFilter(id, -999);
            else
                filters = rBll.getFilter(id, currUser.iUserId);
            string sqlCmd = string.Empty;
            DateTime dyear = DateTime.Now;
            DateTime outd = DateTime.MinValue;
            FilterField fdate = null;
            foreach (FilterField ff in filters)
            {
                filterBuilder.Append(ff.getFilterString());
                if (DateTime.TryParse(ff.Value, out outd) && ff.comparisonOperator.Contains(">"))
                { fdate = ff; dyear = outd; }
            }
            string OrderString = string.Empty;
            DataTable dt = null;
            try
            {
                var rpt = rBll.getReport(id);
                sqlCmd = (ispreview ? "select top 50" + rpt.QueryBase.Substring(6) : rpt.QueryBase) + filterBuilder.ToString();

                var erpVField = rpt.Fields.Find(f => f.isErpEmployeeVisit);
                string erpVisitField = erpVField == null ? "" : erpVField.fieldName;
                OrderString = getOrderString(id);
                if (fdate != null)
                { dt = rBll.getDataSource(rpt.reportID, sqlCmd, fdate.fieldName, dyear.Year, erpVisitField, currUser.cUserName, OrderString); }
                else dt = rBll.getDataSource(rpt.reportID, sqlCmd, null, -1, erpVisitField, currUser.cUserName, OrderString);
            }
            catch (Exception e) { throw new Exception(sqlCmd + OrderString + "\n" + e.Message); }
            return dt;
        }
        private JsonResult getMXData(long id, bool ispreview = false)
        {
            getCurrUserRpt(id);
            //提交，先清空报表
            if (Rpts.Exists(e => e.userID == currUser.iUserId && e.rptID == id))
            {
                Rpts.RemoveAll(rm => rm.userID == currUser.iUserId && rm.rptID == id);
            }
            Rpt = new Rpt() { rptID = id, userID = currUser.iUserId, report = rBll.getReport(id) };

            JsonResult r = new JsonResult() { MaxJsonLength = int.MaxValue };
            string[] rd = new string[2];

            DataTable dt = new DataTable();
            
            try
            {
                dt = getMXDataTable(id, ispreview);

                if (dt != null && dt.Rows.Count > 0)
                {
                    Rpt.dataTime = DateTime.Now;
                    Rpt.dataTable = dt;
                    rd[0] = "true";
                }
                else
                {
                    rd[0] = "false";
                    rd[1] = "数据空，请检查条件！";
                }
                Rpts.Add(Rpt);
            }
            catch (Exception e) { rd[0] = "error"; rd[1] = e.Message; }
            r.Data = rd;
            return r;
        }
        [HttpPost]
        public JsonResult saveFilter(long id, List<FilterField> Filters)
        {
            JsonResult r = new JsonResult() { MaxJsonLength = int.MaxValue };
            long[] rd = new long[Filters.Count];
            long rid = -1;
            if (Filters != null && Filters.Count > 0)
            {
                getCurrUser();
                foreach (FilterField ff in Filters)
                {
                    ff.UserID = ff.isDefault ? -999 : currUser.iUserId;
                    if (ff.filterID <= 0)
                    {
                        rid = rBll.reportFilterDA.Create(ff);
                        rd[ff.filterIndex] = rid;
                    }
                    else
                    {
                        rBll.reportFilterDA.Update(ff);
                        rd[ff.filterIndex] = ff.filterID;
                    }
                }
            }
            r.Data = rd;
            return r;
        }
        [HttpPost]
        public void deleteFilter(long id)
        {
            reportFilterDataAccess rfda = new reportFilterDataAccess();
            rfda.Delete(id);
        }
        #endregion
        #region Order
        public ActionResult ViewOrder(long id, bool isDefault = false)
        {
            getCurrUser();
            var fields = rBll.getFields(id);
            if (fields != null && fields.Count > 0)
                fields.RemoveAll(r => r.isErpEmployeeVisit);
            ViewData["Fields"] = fields;
            List<OrderField> orders;
            if (isDefault)
                orders = rBll.getOrderField(id, -999);
            else
                orders = rBll.getOrderField(id, currUser.iUserId);
            return View(orders);
        }
        [HttpPost]
        public JsonResult saveOrder(long id, List<OrderField> Orders)
        {
            JsonResult r = new JsonResult() { MaxJsonLength = int.MaxValue };
            long[] rd = new long[Orders.Count];
            int idx = 0;
            if (Orders != null && Orders.Count > 0)
            {
                getCurrUser();
                foreach (OrderField of in Orders)
                {
                    of.UserID = of.isDefault ? -999 : currUser.iUserId;
                    rd[idx++] = rBll.saveOrderField(id, of);
                }
            }
            r.Data = rd;
            return r;
        }
        [HttpPost]
        public void deleteOrder(long id)
        {
            rBll.deleteOrderField(id);
        }
        private string getOrderString(long id)
        {
            StringBuilder r = new StringBuilder();
            getCurrUser();
            var OrderFields = rBll.getOrderField(id, currUser.iUserId);
            //拼接Order语句
            if (OrderFields != null && OrderFields.Count > 0)
            {
                r.Append(" order by ");
                for (int i = 0; i < OrderFields.Count; i++)
                    r.Append(OrderFields[i].orderString + ",");
                r.Remove(r.Length - 1, 1);
            }
            return r.ToString();
        }
        #endregion
        #region EditReport
        [AuthorizeEx(Roles = "Admin")]
        public ActionResult Edit(int id = -1)
        {
            getCurrUser();
            Report _rpt = new Report(); ;

            if (id > 0)
            { _rpt = rBll.getReport(id); }
            getViewBag();
            return View(_rpt);
        }

        [AuthorizeEx(Roles = "Admin")]
        [HttpPost]
        public JsonResult save(long id, Report _rpt)
        {

            JsonResult r = new JsonResult() { MaxJsonLength = 1024 };
            r.Data = rBll.saveReport(_rpt);
            return r;
        }
        #region EditReportField
        public ActionResult getFieldTable(long id)
        {
            getCurrUser();
            Report _rpt = new Report();

            _rpt = rBll.getReport(id);
            return View(_rpt != null ? _rpt.Fields.OrderBy(o => o.fieldIndex).ToList() : null);
        }
        [HttpPost]
        public JsonResult saveField(Field f)
        {
            getCurrUser();
            JsonResult r = new JsonResult();

            r.Data = rBll.saveField(f);
            return r;
        }
        [HttpPost]
        public JsonResult deleteField(long fID)
        {
            getCurrUser();
            JsonResult r = new JsonResult();

            r.Data = rBll.deleteField(fID);
            return r;
        }
        #endregion
        #region EditReportOrderField
        public ActionResult getOrderFields(int id)
        {
            getCurrUser();
            long uid = currUser.iUserId;
            UserBLL.userBLL uBll = new UserBLL.userBLL();
            uid = uBll.getStrRoles(uid).Contains("Admin") ? -999 : uid;
            var OrderFields = rBll.getOrderField(id, uid);
            return View(OrderFields);
        }
        [HttpPost]
        public JsonResult saveOrderField(int rptID, OrderField of)
        {
            getCurrUser();
            JsonResult r = new JsonResult();

            r.Data = rBll.saveOrderField(rptID, of);
            return r;
        }
        [HttpPost]
        public JsonResult deleteOrderField(OrderField of)
        {
            getCurrUser();
            JsonResult r = new JsonResult();

            r.Data = rBll.deleteOrderField(of.orderID);
            return r;
        }
        #endregion
        #endregion
        #region Pivot
        public ActionResult pivotView(long id, bool isPreview = false)
        {
            ViewBag.isDefault = isPreview;
            List<PivotView> pvs = new List<PivotView>();
            getCurrUserRpt(id);
            if (isPreview)
                pvs = rBll.getPivotView(id, -999);
            else
                pvs = rBll.getPivotView(id, currUser.iUserId);

            ViewData["rptID"] = id;
            if (!ViewBag.isDefault) TempData["rptTitle"] = Rpt.report.Title;
            else TempData["rptTitle"] = "新报表";
            DataTable mx = null;
            
            if (Rpt.dataTable.Rows.Count<=0) getMXData(id, isPreview);
           
            mx = Rpt.dataTable;
            ViewData["records"] = mx.Rows.Count;
            ViewData["DataTime"] = Rpt.dataTime.ToString("yyyy-MM-dd HH:mm:ss");
            string strJson = //Json(mx,"JSON",JsonRequestBehavior.AllowGet);
                Newtonsoft.Json.JsonConvert.SerializeObject(mx);
            ViewData["dataSource"] = strJson;
            return View(pvs);
        }
        [HttpPost]
        public JsonResult deletePivotParam(long id)
        {
            JsonResult r = new JsonResult();
            rBll.deletePivotView(id);
            return r;
        }
        [HttpPost]
        public JsonResult savePivotParam(long id, PivotView pv)
        {
            JsonResult r = new JsonResult();
            getCurrUser(); 
            if (pv.isDefault) pv.userID = -999;
            else pv.userID = currUser.iUserId;
            r.Data = rBll.savePivotView(id, pv);
            return r;
        }
        #endregion
    }
}