using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Attendance;
using Attendance.Model;
using Attendance.DAL;
using MvcApp.Models;
using UserBLL;
using UserInfo;
using Logs;
using OrganizationBase;


namespace MvcApp.Areas.Employee.Controllers
{
    [AuthorizeEx(Roles = "Employee")]
    public class AttendanceController : Controller
    {
        static LogsDataAccess log;
        static userBLL userBll = new userBLL();
        static UserModel currUser;
        static int currEmpId = -1;
        static object locker = new object();
        static AttendanceBLL attrBll = null;
        static IEnumerable<RecordModel> attList;
        static string currUserRoleNames = string.Empty;
        
        public AttendanceController() {
            log = new Logs.AppLogsDataAccess("Attendance_Logs");
        }
        private void getCurrUser()
        {
            int userid = int.Parse(System.Web.HttpContext.Current.User.Identity.Name);
            if (currUser == null || currUser.iUserId != userid)
            {
                currUser = userBll.single(userid);
            }
        }
        private void Load()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                getCurrUser();
                attrBll = new AttendanceBLL(currUser.iUserId);
                currEmpId = attrBll.getEmpID(currUser.cUserName);
                currUserRoleNames = userBll.getStrRoles(currUser.iUserId);
            }
        }

        #region ver:20151020
        public int ImportAtts()
        {
            int r = 0;
            var attrEmps = attrBll.getAttrEmps().Select(s => s.EmployeeID);
            foreach (int eid in attrEmps) r += ImportAtts(eid);
            return r;
        }
        public int ImportAtts(int eid)
        {
            int r = 0;
            r = attrBll.autoFillAttr(eid);
            return r;
        }
        public int ImportAtts(int eid, DateTime begin, DateTime end)
        {
            int r = attrBll.DoAtte(eid, begin, end);
            return r;
        }
       
        [HttpPost]
        public ContentResult reImportAtts(FormCollection f,int id)
        {
            bool bb = true;
            int r = -1;
            DateTime b;
            bb = id > 0;
            bb &= f.AllKeys.Contains("begin");
            string beginstr = f["begin"].ToString();
            bb &= DateTime.TryParse(beginstr, out b);
            if (bb)
            {
                b = b.AddDays(1 - b.Day);
                DateTime e = b.AddMonths(1).AddDays(-1);
                r = ImportAtts(id, b, e);
            }
            else r = ImportAtts();
            return new ContentResult() { Content = r.ToString() };
        }
        private SelectList getEmpRefList(int currEid)
        {
            SelectList r = null;
            groupBLL groupBll = new groupBLL();
            groupRefBLL groupRefBll = new groupRefBLL();
            //考勤员工参照
            var cardEmps = attrBll.device.cardBll.getDevEmployees(3);
            List<IAttendanceDevice.IEmployee> cardEmployees = null;
            if (userBll.getStrRoles(currUser.iUserId).Contains("AttendanceManager"))//如果角色是考勤管理员，则Employees所有员工
                r = new SelectList(cardEmps, "EmployeeID", "EmployeeName", currEid);
            else
            {// 算法：判断是否是组内管理员，是employees为组内所有人，否Employees为自己

                List<groupModel> Groups = new List<groupModel>();
                var grpRefs = groupRefBll.select(new groupRefModel() { isManager = true });
                groupRefModel groupRef = null;
                if (grpRefs.Exists(e => e.UserId == currUser.iUserId))
                    groupRef = grpRefs.First(f => f.UserId == currUser.iUserId);
                if (groupRef != null)
                {
                    int empid = -1;
                    foreach (UserModel um in groupBll.single(groupRef.groupId).Users)
                    {
                        empid = attrBll.getEmpID(um.cUserName);
                        if (!cardEmployees.Exists(ee => ee.EmployeeId == empid))
                            cardEmployees.Add(attrBll.device.cardBll.getDevEmployee(empid));
                    }
                    r = new SelectList(cardEmployees, "EmployeeID", "EmployeeName", currEid);
                }
                else //当前员工自己
                {
                    var es = new List<IAttendanceDevice.IEmployee>();
                    es.Add(attrBll.device.cardBll.getDevEmployee(currEid));
                    r = new SelectList(es, "EmployeeID", "EmployeeName", currEid);
                }
            }
            return r;
        }
        public ActionResult Index()
        {
            Load();
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "Index",
                cReturn = "Open Attendance Default Page!"
            });
            TempData["updateAtts"] = ImportAtts(currEmpId);
            DateTime dn = DateTime.Now;
            if (dn.Day < 8)//当前日期小于8号，取上月考勤
            { dn = dn.AddMonths(-1); }
            ViewBag.date = dn;
            ViewBag.Employees = getEmpRefList(currEmpId);
            ViewBag.currUserRoleNames = currUserRoleNames;
            attList = attrBll.getRecords(currEmpId, dn.AddDays(1 - dn.Day), dn.AddMonths(1).AddDays(-1 * dn.Day));
            if (Request.IsAjaxRequest())
                return PartialView("AttendanceList", attList);
            return View(attList);
        }
        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            //Load();
            int y = (f.AllKeys.Contains("Year")) ? int.Parse(f["Year"]) : DateTime.Now.Year;
            int m = (f.AllKeys.Contains("Month")) ? int.Parse(f["Month"]) : DateTime.Now.Month;
            int selEmpId = (f.AllKeys.Contains("selEmpId") ? int.Parse(f["selEmpId"]) : -1);
            DateTime b = new DateTime(y, m, 1);
            DateTime e = b.AddMonths(1).AddDays(-1);
            attList = attrBll.getRecords(selEmpId, b, e);

            ViewBag.date = new DateTime(y, m, 1);
            ViewBag.Employees = getEmpRefList(selEmpId);
            ViewBag.currUserRoleNames = currUserRoleNames;
            var isr = Request.IsAjaxRequest();
            //if (Request.IsAjaxRequest())
                //return PartialView("AttendanceList",attList);
            return View(attList);
        }
        #endregion 

        //public JsonResult getEmployees()
        //{
        //    JsonResult rJson = new JsonResult();
        //    rJson.MaxJsonLength = int.MaxValue;
        //    rJson.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            
        //    DALHWATTEmployee dalEmpBll = new DALHWATTEmployee();
        //    //系统用户表里角色为Employee的员工;离职员工将角色收回;
        //    //roleBLL roleBll = new roleBLL();
        //    //IEnumerable<int> Users = roleBll.getUsersWithRole("Employee").Select(s => s.iUserId);
        //    //IList<UserModel> Users = roleBll.single("Employee").Users;
        //    //考勤用户配置表里的员工.
        //    AttendanceEmployeeRefSchemeClassBLL aeBll = new AttendanceEmployeeRefSchemeClassBLL();
        //    var ae = aeBll.filter().Where(w => w.isVaild == true);
        //    IEnumerable<int> ael = ae.Select(aes => aes.EmployeeID.Value).Distinct();

        //    //考勤机数据表里的员工
        //    IEnumerable<int> HWemps = dalEmpBll.EmpIds();
           
        //    //交集
        //    var rd = HWemps.Intersect(ael).OrderBy(o => o);
        //    rJson.Data = rd;
        //    currUser = userBll.single(int.Parse(HttpContext.User.Identity.Name));
        //    log.createLog(new LogModel()
        //    {
        //        iUserID = currUser.iUserId,
        //        cUserName = currUser.cUserName,
        //        cModule = "Attendance",
        //        cClass = "AttendanceController",
        //        cFunction="getEmployee",
        //        cReturn= Newtonsoft.Json.JsonConvert.SerializeObject(rd)
        //    });
        //    return rJson;
        //}
        //public JsonResult getEmployeeAttDays(int id) 
        //{
        //    JsonResult rJson = new JsonResult();
        //    rJson.MaxJsonLength = int.MaxValue;
        //    rJson.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    AttendanceRecordsBLL attRecordListBll = new AttendanceRecordsBLL();
        //    RecordModel attRecord = attRecordListBll.getLastRecord(id.ToString());
        //    //List<DateTime> ldt = new List<DateTime>();
        //    List<string> ldt = new List<string>();
        //    //指定员工计划生效日期
        //    Attendance_EmployeeRefSchemeClass attCurrERSC;
        //    AttendanceEmployeeRefSchemeClassBLL attEmpRefSchClassBll = new AttendanceEmployeeRefSchemeClassBLL();
        //    attCurrERSC = attEmpRefSchClassBll.getInstanceEmpID(id); 
        //    if (attCurrERSC == null) attCurrERSC=new Attendance_EmployeeRefSchemeClass(){ EffDate=DateTime.MaxValue};
        //    //生成要计算的日期列表
        //    DateTime start = (attRecord!=null)?attRecord.sDate.Value:
        //        attCurrERSC.EffDate.Value;
        //    DateTime end = DateTime.Now;
        //    while (start <= end) 
        //    {
        //        ldt.Add(start.ToShortDateString());
        //        start = start.AddDays(1);
        //    }

        //    rJson.Data = ldt;
        //    log.createLog(new LogModel()
        //    {
        //        iUserID = currUser.iUserId,
        //        cUserName = currUser.cUserName,
        //        cModule = "Attendance",
        //        cClass = "AttendanceController",
        //        cFunction = "getEmployeeAttDays",
        //        cParams = id.ToString(),
        //        cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(ldt)
        //    });
        //    return rJson;
        //}
        //[HttpPost]
        //public JsonResult doDay(FormCollection f)
        //{
        //    JsonResult rJson = new JsonResult();
        //    rJson.MaxJsonLength = int.MaxValue;
        //    rJson.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    int empid = int.Parse(f["empid"]);
        //    DateTime dt = DateTime.Parse(f["date"]);
        //    //处理指定员工,日期的考勤导入.
        //    AttendanceBLL aBll = new AttendanceBLL();
        //    string Operator = currEmp;
            
        //    lock (locker) { 
        //        rJson.Data = aBll.doDay(empid, dt, Operator);
        //    }
        //    log.createLog(new LogModel()
        //    {
        //        iUserID = currUser.iUserId,
        //        cUserName = currUser.cUserName,
        //        cModule = "Attendance",
        //        cClass = "AttendanceController",
        //        cFunction = "doDay",
        //        cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { empid,dt}),
        //        cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(rJson.Data)
        //    });
        //    return rJson;
        //}


        #region Event OK
        public ActionResult editEvent(int id,string param)
        {
            int recordId = id;
            EventDeclaredDAL edDal = new EventDeclaredDAL();
            RecordDAL rDal = new RecordDAL();
            RecordModel ar = new RecordModel();
            
            ar = rDal.Single(id);
            ViewBag.record = ar;
            bool isOnTime = false;
            ViewBag.isOnTime = bool.TryParse(param, out isOnTime) ? isOnTime : false;
            ViewBag.currUserRoleNames = currUserRoleNames;
            //指定recordId的事件列表
            IEnumerable<EventDeclaredModel> aeList = edDal.selects(new EventDeclaredModel() { recordID = recordId, isBeginTime = isOnTime });
            return View(aeList);
        }

        [HttpPost]
        public ActionResult editEvent(int id, FormCollection f)
        { //SELECT autoID,EventDescription,FeeCar,FeeMeals,FeeOther,isCar,Other,Memo,chechMan,Manager,recordID,isBeginTime FROM Attendance_EventDeclared
          //data: { id: id, description:description, fee:fee, memo:memo,check:check,isOnTime:isOnTime,recordId:recordId, Operation: operation },

            EventDeclaredModel edModel = new EventDeclaredModel();
            edModel.autoID = f.AllKeys.Contains("id") ? int.Parse(f["id"]) : -1;
            edModel.EventDescription = f.AllKeys.Contains("description") ? f["description"] : "无";
            decimal fee = -1;
            if (f.AllKeys.Contains("fee") && decimal.TryParse(f["fee"], out fee))
                edModel.FeeOther = fee;
            else edModel.FeeOther = 0;
            edModel.Memo = f.AllKeys.Contains("memo") ? f["memo"] : "";
            edModel.checkMan = f.AllKeys.Contains("check") ? f["check"] : null;
            bool isot = false;
            if (f.AllKeys.Contains("isOnTime") && bool.TryParse(f["isOnTime"], out isot))
                edModel.isBeginTime = isot;
            else edModel.isBeginTime = false;
            int rid = -1;
            if (f.AllKeys.Contains("recordId") && int.TryParse(f["recordId"], out rid))
                edModel.recordID = rid;
            else edModel.recordID = -1;

            string Operation = f["Operation"].ToString();
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "editEvent",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(edModel),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject("GO -> " + Operation)
            });
            switch (Operation)
            {
                case "SaveRow":
                    return saveEvent(edModel, false);
                case "DeleteRow":
                    return deleteEvent(edModel, false);
                case "SaveALL":
                    return saveEvent(edModel, true);
                case "DeleteALL":
                    return deleteEvent(edModel, true);
                default:
                    return View();
            }
        }
        public ContentResult saveEvent(EventDeclaredModel edModel, bool isMulti)
        {
            int id = edModel.autoID;
            EventDeclaredDAL edDal = new EventDeclaredDAL();
            bool r = false;

            if (id < 0)
            {//新增
                r = edDal.Create(edModel) > 0;
            }
            else
            {//更新 
                r = edDal.Update(edModel) > 0;
            }
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "saveEvent",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { edModel, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "保存成功!" };
            else return new ContentResult() { Content = isMulti ? "" : "保存失败" };
        }
        public ContentResult deleteEvent(EventDeclaredModel edModel, bool isMulti)
        {
            EventDeclaredDAL edDal = new EventDeclaredDAL();
            bool r = true;
            r &= (edDal.Delete(edModel.autoID) > 0);
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "deleteEvent",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { edModel, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "删除成功！" };
            else
                return new ContentResult() { Content = isMulti ? "" : "删除失败" };
        }
        #endregion

        #region EventList
        public ActionResult eventOverList(int id, string param)
        {
            vEventDAL vEventDal = new vEventDAL();
            ViewBag.isChecker = userBll.getStrRoles(currUser.iUserId).Contains("attsChecker");
            ViewBag.AttendanceMan = attrBll.device.cardBll.getDevEmployee(id).EmployeeName;
            List<vAttendanceEventModel> vaEventList = null;
            DateTime d = new DateTime();
            if (!string.IsNullOrEmpty(param) && DateTime.TryParse(param, out d))
            {
                DateTime b = d.AddDays(1 - d.Day);
                DateTime e = b.AddMonths(1).AddDays(-1);
                vaEventList = vEventDal.searchOver(id, b, e);
                ViewBag.YearMonth = d.Year + "-" + d.Month;
            }
            else
            {
                vaEventList = null;
                //rList = aelBll.filter(empid: id); ViewBag.YearMonth = "全部";
            }
            vaEventList = (vaEventList != null) ?
                vaEventList.OrderBy(aeo => aeo.sDate).ToList() : null;
            return View(vaEventList);
        }
        public ActionResult eventAbsenceList(int id, string param)
        {
            vEventDAL vEventDal = new vEventDAL();
            ViewBag.isChecker = userBll.getStrRoles(currUser.iUserId).Contains("attsChecker");
            ViewBag.AttendanceMan = attrBll.device.cardBll.getDevEmployee(id).EmployeeName;
            List<vAttendanceEventModel> vaEventList = null;
            DateTime d = new DateTime();
            if (!string.IsNullOrEmpty(param) && DateTime.TryParse(param, out d))
            {
                attrBll.saveAbsenceWithMonth(id, d);//增加所有缺勤记录到缺勤事件
                DateTime b = d.AddDays(1 - d.Day);
                DateTime e = b.AddMonths(1).AddDays(-1);
                vaEventList = vEventDal.searchAbsence(id, b, e);
                ViewBag.YearMonth = d.Year + "-" + d.Month;
            }
            else
            {
                vaEventList = null;
                //rList = aelBll.filter(empid: id); ViewBag.YearMonth = "全部";
            }
            vaEventList = (vaEventList != null) ?
                vaEventList.OrderBy(aeo => aeo.sDate).ToList() : null;
            return View(vaEventList);
        }

        [HttpPost]
        public ContentResult saveChecker(int id, string chkMan)
        {
            int r = 0;
            EventDeclaredDAL edDal = new EventDeclaredDAL();
            EventDeclaredModel edModel = edDal.Single(id);
            if (edModel.checkMan != chkMan)
            {
                edModel.checkMan = chkMan;
                r = edDal.Update(edModel);
            }
            else
                r = 0;
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "saveChecker",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { id,chkMan}),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            return new ContentResult() { Content = r.ToString() };
        }
        #endregion 

        #region Manager
        [AuthorizeEx(Roles="AttendanceManager")]
        public ActionResult ManagerPage()
        {
            Load();
            return View();
        }

        private SelectList getUsersOfEmpRole()
        {
            //用户参照
            //系统用户表里角色为Employee的员工;离职员工将角色收回;
            roleBLL roleBll = new roleBLL();
            IEnumerable<UserModel> Users = roleBll.single("Employee").Users;
            getCurrUser();
            SelectList slUser = new SelectList(Users, "iUserId", "cUserName", currUser.iUserId);
            return slUser;
        }

        #region EmpRefSchemePlan OK
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ActionResult editEmpRefSchemeClass(string id)
        {
           List<EmployeeRefSchemeClassModel> erscList;
            EmployeeRefSchemeClassDAL erscDal = new EmployeeRefSchemeClassDAL();
            if (attrBll == null) Load();
            //用户参照
            ViewBag.Users = getUsersOfEmpRole();
            //考勤机员工参照
            ViewBag.Employees = new SelectList(attrBll.device.cardBll.getDevEmployees(3),"EmployeeID","EmployeeName",currEmpId);

            //班次计划参照
            SchemeClassDAL scDal = new SchemeClassDAL();
            List<SchemeClassModel> scList = scDal.selects(null);
            foreach (SchemeClassModel sc in scList)
            { sc.className = sc.Scheme.schemeName+":"+sc.className; }

            SelectList slClass = new SelectList(scList, "classId", "className");
            ViewBag.SchemeClass = slClass;
            IAttendanceDevice.IEmployee cardEmp = null;
            //分析id
            if (string.IsNullOrEmpty(id))
            {
                //所有员工，所有班次计划
                erscList = erscDal.selects(null);
            }
            else 
            {
                //id的字符串形式：{emp|员工名,emp|员工id} 或者 {plan|planId}
                string[] ids = id.Split('|');
                switch (ids[0])
                {
                    case "emp":
                        int empid = -1;
                        if (int.TryParse(ids[1], out empid))
                        {
                            cardEmp = attrBll.device.cardBll.getDevEmployee(empid);
                            if (cardEmp != null)
                            {
                                erscList = erscDal.selects(new EmployeeRefSchemeClassModel() { EmployeeID = cardEmp.EmployeeId });
                                ViewBag.Employee = cardEmp;
                            }
                            else { erscList = null; TempData["errMsg"] += (ids[1]+" - 无此员工！"); }
                        }
                        else
                        {
                            empid = attrBll.getEmpID(ids[1]);
                            if (empid > 0)
                            {
                                cardEmp = attrBll.device.cardBll.getDevEmployee(empid);
                                if (cardEmp != null)
                                    erscList = erscDal.selects(new EmployeeRefSchemeClassModel() { EmployeeID = cardEmp.EmployeeId });
                                else { erscList = null; TempData["errMsg"] += (ids[1] + " - 无此员工！"); }
                            }
                            else { erscList = null; TempData["errMsg"] += (ids[1] + " - 无此员工！"); }
                        }
                        break;
                    case "plan":
                        int planid = -1;
                        if (int.TryParse(ids[1], out planid) && planid > 0)
                        {
                            erscList = erscDal.selects(new EmployeeRefSchemeClassModel() { classID = planid});
                            ViewBag.currPlan = scDal.Single(planid);
                            ViewBag.scheme = scDal.Single(planid).Scheme;
                        }
                        else { erscList = null; TempData["errMsg"] += "方案班次计划号无效！"; }
                        break;
                    default:
                        erscList = null;
                        break;
                }
            }
            ViewBag.currUserRoleNames = currUserRoleNames;
            return View(erscList);
        }

        [AuthorizeEx(Roles = "AttendanceManager")]
        [HttpPost]
        public ActionResult editEmpRefSchemeClass(FormCollection f, int id)
        {
            //SELECT autoid,UserID,EmployeeID,classID,effDate,periodNo,isVaild FROM Attendance_EmployeeRefSchemeClass

            EmployeeRefSchemeClassModel aersc = new EmployeeRefSchemeClassModel();
            aersc.autoid = id;
            aersc.EmployeeID = int.Parse(f["EmployeeID"]);
            aersc.EmployeeName = f["EmployeeName"];
            aersc.EffDate = DateTime.Parse(f["effDate"]);
            aersc.periodNo = int.Parse(f["periodNo"]);
            aersc.classID = int.Parse(f["classID"]);
            aersc.UserID = int.Parse(f["UserID"]);

            string Operation = f["Operation"].ToString();
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "editEmpRefSchemeClass",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(aersc),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject("GO -> " + Operation)
            });
            switch (Operation)
            {
                case "SaveRow":
                    return saveEmpRefSchemeClass(aersc, false);
                case "DeleteRow":
                    return deleteEmpRefSchemeClass(aersc, false);
                case "SaveALL":
                    return saveEmpRefSchemeClass(aersc, true);
                case "DeleteALL":
                    return deleteEmpRefSchemeClass(aersc, true);
                default:
                    return View();
            }
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult saveEmpRefSchemeClass(EmployeeRefSchemeClassModel aersc, bool isMulti)
        {
            int id = aersc.autoid;
             EmployeeRefSchemeClassDAL erscDal = new EmployeeRefSchemeClassDAL();
            bool r = false;

            if (id < 0)
            {//新增
                r = erscDal.Create(aersc) > 0;
            }
            else
            {//更新 
                r = erscDal.Update(aersc) > 0;
            }
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "saveEmpRefSchemeClass",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { aersc, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "保存成功!" };
            else return new ContentResult() { Content = isMulti ? "" : "保存失败" };
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult deleteEmpRefSchemeClass(EmployeeRefSchemeClassModel aersc, bool isMulti)
        {
            EmployeeRefSchemeClassDAL erscDal = new EmployeeRefSchemeClassDAL(); 
            bool r = true;
            r &= (erscDal.Delete(aersc.autoid) > 0);

            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "deleteClassPlan",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { aersc, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "删除成功！" };
            else
                return new ContentResult() { Content = isMulti ? "" : "删除失败" };
        }

        #endregion

        #region ClassPlan OK
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ActionResult editClassPlan(int id = -1)
        {
            SchemeClassDAL scDal = new SchemeClassDAL();
            SchemeClassModel scModel = scDal.Single(id); 
            ViewBag.schemeClass = scModel;
            ViewBag.scheme = scModel.Scheme;
            ViewBag.currUserRoleNames = currUserRoleNames;
            ClassPlanDAL cpDal = new ClassPlanDAL();
            List<ClassPlanModel> cpList;
            cpList = scModel.ClassPlans; 
            return View(cpList);
        }

        [AuthorizeEx(Roles = "AttendanceManager")]
        [HttpPost]
        public ActionResult editClassPlan(FormCollection f, int id)
        {
            //SELECT SchemeClassID,schemeID,classes,periodNo,bTime,eTime,upTime,sdate FROM SchemeClass
            ClassPlanModel scModel = new ClassPlanModel();
            scModel.autoid = id;
            scModel.classId = int.Parse(f["classId"]);
            scModel.bTime = f["bTime"];
            scModel.eTime = f["eTime"];
            scModel.periodNo = int.Parse(f["periodNo"]);
            scModel.upTime = f["upTime"];
            scModel.sdate = null;
            DateTime d = default(DateTime);
            if (f.AllKeys.Contains("sDate") && DateTime.TryParse(f["sDate"], out d))
                scModel.sdate = d;
            string Operation = f["Operation"].ToString();
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "editClassPlan",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(scModel),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject("GO -> "+Operation)
            });
            switch (Operation)
            {
                case "SaveRow":
                    return saveClassPlan(scModel, false);
                case "DeleteRow":
                    return deleteClassPlan(scModel, false);
                case "SaveALL":
                    return saveClassPlan(scModel, true);
                case "DeleteALL":
                    return deleteClassPlan(scModel, true);
                default:
                    return View();
            }
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult saveClassPlan(ClassPlanModel acp, bool isMulti)
        {
            int id = acp.autoid;
            ClassPlanDAL cpDal = new ClassPlanDAL();
            bool r = false;

            if (id < 0)
            {//新增
                r = cpDal.Create(acp) > 0;
            }
            else
            {//更新 
                r = cpDal.Update(acp) > 0;
            }
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "saveClassPlan",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { acp,isMulti}),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "本班周期：" + acp.periodNo + " 保存成功!" };
            else return new ContentResult() { Content = isMulti ? "" : "保存失败" };
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult deleteClassPlan(ClassPlanModel acp, bool isMulti)
        {
            ClassPlanDAL cpDall = new ClassPlanDAL();
            bool r = true;
            r &= cpDall.Delete(acp.autoid) > 0;
            
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "deleteClassPlan",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new{acp,isMulti}),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "本班周期：" + acp.periodNo + " 删除成功！" };
            else
                return new ContentResult() { Content = isMulti ? "" : "删除失败,记录不存在或有向下关联的数据" };
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public bool isExistClassPeriod(int id,int classid,int period,string sdate)
        {
            bool r = false;
            ClassPlanDAL cpDal = new ClassPlanDAL();
            DateTime d;
            if (!DateTime.TryParse(sdate, out d)) {throw new InvalidCastException("日期参数不合法！,function=isExistClassPeriod"); }
            var a = cpDal.selects(new ClassPlanModel() { classId=classid, periodNo = period, sdate = d });//返回存在的实例
            if (a != null&&a.Exists(e=>e.autoid == id))
                r = true;
            return r;
        }
        #endregion

        #region SchemeClass OK
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ActionResult editSchemeClass(int id = -1)
        {
            SchemeDAL schemeDal = new SchemeDAL();
            ViewBag.currUserRoleNames = currUserRoleNames;
            SchemeModel scheme = schemeDal.Single(id);
            ViewBag.scheme = scheme;
            return View(scheme.SchemeClasss);
        }

        [AuthorizeEx(Roles = "AttendanceManager")]
        [HttpPost]
        public ActionResult editSchemeClass(FormCollection f, int id)
        {
            //SELECT classId,schemeId,className FROM Attendance_SchemeClass
            SchemeClassModel scModel = new SchemeClassModel();
            scModel.classId = id;
            scModel.schemeId = int.Parse(f["schemeId"]);
            scModel.className =  f["className"];

            string Operation = f["Operation"].ToString();
            switch (Operation)
            {
                case "SaveRow":
                    return saveSchemeClass(scModel, false);
                case "DeleteRow":
                    return deleteSchemeClass(scModel, false);
                case "SaveALL":
                    return saveSchemeClass(scModel, true);
                case "DeleteALL":
                    return deleteSchemeClass(scModel, true);
                default:
                    return View();
            }
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult saveSchemeClass(SchemeClassModel asc, bool isMulti)
        {
            int id = asc.classId;
            SchemeClassDAL scDal = new SchemeClassDAL();
            bool r = false;

            if (id < 0)
            {//新增
                r = scDal.Create(asc) > 0;
            }
            else
            {//更新 
                r = scDal.Update(asc) > 0;
            }
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "saveSchemeClass",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { asc, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "班次：" + asc.className + " 保存成功!" };
            else return new ContentResult() { Content = isMulti ? "" : "保存失败" };
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult deleteSchemeClass(SchemeClassModel asc, bool isMulti)
        {
            EmployeeRefSchemeClassDAL erscDal = new EmployeeRefSchemeClassDAL();
            SchemeClassDAL scDal = new SchemeClassDAL();
            bool r = true;
            if (erscDal.selects(new EmployeeRefSchemeClassModel() {classID = asc.classId }).Count() <= 0)
                r &= scDal.Delete(asc.classId) > 0;
            else r = false;
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "deleteClassPlan",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { asc, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "班次：" + asc.className + " 删除成功！" };
            else
                return new ContentResult() { Content = isMulti ? "" : "删除失败,记录不存在或有向下关联的数据" };
        }
        #endregion

        #region Scheme OK
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ActionResult editScheme()
        {
            SchemeDAL schemeDal = new SchemeDAL();
            List<SchemeModel> schemeList;
            ViewBag.currUserRoleNames = currUserRoleNames;
            schemeList = schemeDal.selects(null);
            return View(schemeList);
        }

        [AuthorizeEx(Roles = "AttendanceManager")]
        [HttpPost]
        public ActionResult editScheme(FormCollection f, int id)
        {
            //SELECT schemeID,schemeName,schemeDescription,periods,classs FROM Attendance_Scheme
            SchemeModel schemeModel = new SchemeModel();
            schemeModel.schemeID = id;
            schemeModel.periods = int.Parse(f["periods"]);
            schemeModel.classs = int.Parse(f["classs"]);
            schemeModel.schemeName = f["name"];
            schemeModel.schemeDescription = f["description"];

            string Operation = f["Operation"].ToString();
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "editScheme",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(schemeModel),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject("GO -> " + Operation)
            });
            switch (Operation)
            {
                case "SaveRow":
                    return saveScheme(schemeModel, false);
                case "DeleteRow":
                    return deleteScheme(schemeModel, false);
                case "SaveALL":
                    return saveScheme(schemeModel, true);
                case "DeleteALL":
                    return deleteScheme(schemeModel, true);
                default:
                    return View();
            }
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult saveScheme(SchemeModel schemeModel, bool isMulti)
        {
            int id = schemeModel.schemeID;
            SchemeDAL schemeDall = new SchemeDAL();
            bool r = false;

            if (id < 0)
            {//新增
                r = schemeDall.Create(schemeModel) > 0;
            }
            else
            {//更新 
                r = schemeDall.Update(schemeModel) > 0;
            }
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "saveScheme",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { schemeModel, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : schemeModel.schemeName + " 保存成功!" };
            else return new ContentResult() { Content = isMulti ? "" : "保存失败" };
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult deleteScheme(SchemeModel schemeModel, bool isMulti)
        {
            SchemeDAL schemeDal = new SchemeDAL();
            bool r = true;
            if (schemeModel.SchemeClasss != null && schemeModel.SchemeClasss.Count() <= 0)
                r &= schemeDal.Delete(schemeModel.schemeID) > 0;
            else r = false;
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "deleteClassPlan",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { schemeModel, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : schemeModel.schemeName + " 删除成功！" };
            else
                return new ContentResult() { Content = isMulti ? "" : "删除失败,记录不存在或有向下关联的数据" };
        }
        #endregion

        #region Holiday OK
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ActionResult editHoliday()
        {
            HolidayDAL hDal = new HolidayDAL();
            List<HolidayModel> ahList;
            ahList = hDal.selects(new HolidayModel() { iYear = DateTime.Now.Year });
            ViewBag.currUserRoleNames = currUserRoleNames;
            if (ahList.Count() <= 0) return View();
            return View(ahList); 
        }

        [AuthorizeEx(Roles = "AttendanceManager")]
        [HttpPost]
        public ActionResult editHoliday(FormCollection f,int id)
        {
            HolidayModel hModel = new HolidayModel();
            hModel.autoid = id;
            hModel.sDate = DateTime.Parse(f["date"]);
            hModel.iYear = hModel.sDate.Value.Year;
            hModel.sName = f["holiday"];
            hModel.sDescription = f["holidayDescription"];

            string Operation = f["Operation"].ToString();
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "editHoliday",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(hModel),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject("GO -> " + Operation)
            });
            switch (Operation)
            {
                case "SaveRow":
                    return saveHoliday(hModel,false);
                case "DeleteRow":
                    return deleteHoliday(hModel,false);
                case "SaveALL":
                    return saveHoliday(hModel,true);
                case "DeleteALL":
                    return deleteHoliday(hModel,true);
                default:
                    return View();
            }
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult saveHoliday(HolidayModel ah, bool isMulti)
        {
            int id = ah.autoid;
            HolidayDAL hDal = new HolidayDAL();
            bool r = false;
            
            if (id < 0)
            {//新增
                r = hDal.Create(ah) > 0;
            }
            else
            {//更新 
                r = hDal.Update(ah) > 0;
            }
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "saveHoliday",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { ah, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti?"": ah.sName + " 保存成功!" };
            else return new ContentResult() { Content = isMulti ? "" : "保存失败" };
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult deleteHoliday(HolidayModel ah, bool isMulti)
        {
            HolidayDAL hDal = new HolidayDAL();
            bool r = false;
            r = (hDal.Delete(ah.autoid) > 0);
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "deleteClassPlan",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { ah, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : ah.sName + " 删除成功！" };
            else
                return new ContentResult() { Content = isMulti ? "" : "删除失败" };
        }
        [AuthorizeEx(Roles = "AttendanceManager")]        
        public JsonResult isExistHoliday(int id,string sdate)
        {
            HolidayDAL hDal = new HolidayDAL();
            bool r = false;
            DateTime d;
            if (!DateTime.TryParse(sdate, out d))
                throw new InvalidCastException("参数日期格式不正确！,function=isExistHoliday");
            var a = hDal.selects(new HolidayModel() { sDate =  d});
            r = (a != null && a.Count > 0);
            return new JsonResult() { Data = r?"True":"False", JsonRequestBehavior = JsonRequestBehavior.AllowGet  };
        }
        #endregion

        #region FeeCalculator
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ActionResult editFeeCalculator(int id)
        {
            SchemeClassDAL scDal = new SchemeClassDAL();
            SchemeClassModel scModel = scDal.Single(id);
            ViewBag.schemeClass = scModel;
            ViewBag.scheme = scModel.Scheme;
            ViewBag.currUserRoleNames = currUserRoleNames;
             FeeCalculatorDAL fcDal = new FeeCalculatorDAL();
            //ViewBag.dateEnum = new SelectList(Enum.GetNames(typeof(Attendance_FeeCalculator.dayState)).AsEnumerable());
            ViewBag.dateEnum = new SelectList(FeeCalculatorModel.cnDayState.AsEnumerable(),"Key","Value");
            List<FeeCalculatorModel> fcList;
            fcList = fcDal.selects(new FeeCalculatorModel() { classId = id });
            return View(fcList);
        }
        [HttpPost]
        public ActionResult editFeeCalculator(FormCollection f ,int id) 
        {
            //SELECT id,Unit,UnitFee,MaxFee,classId,dateEnum FROM Attendance_FeeCalculator
            FeeCalculatorModel fcModel = new FeeCalculatorModel();
            fcModel.id = id;
            fcModel.classId = f.AllKeys.Contains("classId")?int.Parse(f["classId"]):-1;
            fcModel.dateEnum = f["dateEnum"] ;
            fcModel.Unit = f.AllKeys.Contains("Unit") ? int.Parse(f["Unit"]) : 1;
            fcModel.UnitFee = f.AllKeys.Contains("UnitFee") ? decimal.Parse(f["UnitFee"]) : 0;
            fcModel.MaxFee = f.AllKeys.Contains("MaxFee") ? decimal.Parse(f["MaxFee"]) : 0;

            string Operation = f["operation"];
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "editFeeCalculator",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(fcModel),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject("GO -> " + Operation)
            });
            switch (Operation)
            {
                case "SaveRow":
                    return saveFeeCalculator(fcModel, false);
                case "DeleteRow":
                    return deleteFeeCalculator(fcModel, false);
                case "SaveALL":
                    return saveFeeCalculator(fcModel, true);
                case "DeleteALL":
                    return deleteFeeCalculator(fcModel, true);
                default:
                    return View();
            }

            return View();
        }
        public ActionResult saveFeeCalculator(FeeCalculatorModel afc, bool isMulti) 
        {

            int id = afc.id;
            FeeCalculatorDAL fcDal = new FeeCalculatorDAL();
            bool r = false;

            if (id < 0)
            {//新增
                r = fcDal.Create(afc) > 0;
            }
            else
            {//更新 
                r = fcDal.Update(afc) > 0;
            }
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "saveFeeCalculator",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { afc, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "本班：" + afc.dateEnum + " 保存成功!" };
            else return new ContentResult() { Content = isMulti ? "" : "保存失败" };
        }
        public ActionResult deleteFeeCalculator(FeeCalculatorModel afc, bool isMulti)
        {
            FeeCalculatorDAL fcDal = new FeeCalculatorDAL();
            bool r = true;
            r &= fcDal.Delete(afc.id) > 0;
            log.createLog(new LogModel()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "deleteClassPlan",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { afc, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "本班：" + afc.dateEnum + " 删除成功！" };
            else
                return new ContentResult() { Content = isMulti ? "" : "删除失败,记录不存在或有向下关联的数据" };
       
        }
        #endregion

        #endregion
    }
}
