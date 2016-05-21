using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemBLL;
using Attendance;
using HWATT;

namespace MvcApp.Areas.Employee.Controllers
{
    [AuthorizeEx(Roles="Employee")]
    public class AttendanceController : Controller
    {
        static LogsBLL logbll = new LogsBLL();
        SystemDB.User currUser;
        string currEmp;
        static object locker = new object();
        AttendanceVRecordListBLL attRecordListBll = new AttendanceVRecordListBLL();
        IEnumerable<v_Attendance> attList;

        public int prevImportAtts(bool isAll) 
        {
            int r = 0;
            currEmp = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name)).cUserName;
            AttendanceBLL aBll = new AttendanceBLL();
            if (!isAll) r = aBll.prepRecord(currEmp, Operator:currEmp);
            else r = aBll.prepRecord(null,Operator:currEmp);
            return r;
        }
        public JsonResult getEmployees()
        {
            JsonResult rJson = new JsonResult();
            rJson.MaxJsonLength = int.MaxValue;
            rJson.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            
            DALHWATTEmployee dalEmpBll = new DALHWATTEmployee();
            //系统用户表里角色为Employee的员工;离职员工将角色收回;
            RolesBLL rBll = new RolesBLL();
            IEnumerable<int> Users = rBll.getUsersWithRole("Employee").Select(s => s.iUserId);

            //考勤用户配置表里的员工.
            AttendanceEmployeeRefSchemeClassBLL aeBll = new AttendanceEmployeeRefSchemeClassBLL();
            var ae = aeBll.filter().Where(w => w.isVaild == true);
            IEnumerable<int> ael = ae.Select(aes => aes.EmployeeID.Value).Distinct();

            //考勤机数据表里的员工
            IEnumerable<int> HWemps = dalEmpBll.EmpIds();
           
            //交集
            var rd = HWemps.Intersect(ael).OrderBy(o => o);
            rJson.Data = rd;
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction="getEmployee",
                cReturn= Newtonsoft.Json.JsonConvert.SerializeObject(rd)
            });
            return rJson;
        }

        public JsonResult getEmployeeAttDays(int id) 
        {
            JsonResult rJson = new JsonResult();
            rJson.MaxJsonLength = int.MaxValue;
            rJson.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            AttendanceRecordsBLL attRecordListBll = new AttendanceRecordsBLL();
            Attendance_Records attRecord = attRecordListBll.getLastRecord(id.ToString());
            //List<DateTime> ldt = new List<DateTime>();
            List<string> ldt = new List<string>();
            //指定员工计划生效日期
            Attendance_EmployeeRefSchemeClass attCurrERSC;
            AttendanceEmployeeRefSchemeClassBLL attEmpRefSchClassBll = new AttendanceEmployeeRefSchemeClassBLL();
            attCurrERSC = attEmpRefSchClassBll.getInstanceEmpID(id); 
            if (attCurrERSC == null) attCurrERSC=new Attendance_EmployeeRefSchemeClass(){ EffDate=DateTime.MaxValue};
            //生成要计算的日期列表
            DateTime start = (attRecord!=null)?attRecord.sDate.Value:
                attCurrERSC.EffDate.Value;
            DateTime end = DateTime.Now;
            while (start <= end) 
            {
                ldt.Add(start.ToShortDateString());
                start = start.AddDays(1);
            }

            rJson.Data = ldt;
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "getEmployeeAttDays",
                cParams = id.ToString(),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(ldt)
            });
            return rJson;
        }

        [HttpPost]
        public JsonResult doDay(FormCollection f)
        {
            JsonResult rJson = new JsonResult();
            rJson.MaxJsonLength = int.MaxValue;
            rJson.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            int empid = int.Parse(f["empid"]);
            DateTime dt = DateTime.Parse(f["date"]);
            //处理指定员工,日期的考勤导入.
            AttendanceBLL aBll = new AttendanceBLL();
            currEmp = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name)).cUserName;
            string Operator = currEmp;
            
            lock (locker) { 
                rJson.Data = aBll.doDay(empid, dt, Operator);
            }
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "doDay",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { empid,dt}),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(rJson.Data)
            });
            return rJson;
        }
        public ActionResult Index()
        {
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "Index",
                cReturn = "Open Attendance Default Page!"
            });
            TempData["updateAtts"] = prevImportAtts(isAll:false);
            DateTime dn = DateTime.Now;
            if (dn.Day < 8)//当前日期小于8号，取上月考勤
            { dn = dn.AddMonths(-1); }
            ViewBag.date = dn;
            //改进-抽象-员工参照:考勤机里的员工和用户交集
            //考勤员工参照
            DALHWATTEmployee dalEmpBll = new DALHWATTEmployee();
            AttendanceEmployeeRefSchemeClassBLL aerscBll = new AttendanceEmployeeRefSchemeClassBLL();
            List<KQZ_Employee> Employees = new List<KQZ_Employee>();
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            
            if (currUser.RoleNames.Contains("AttendanceManager"))//如果角色是考勤管理员，则Employees所有员工
                ViewBag.Employees = new SelectList(dalEmpBll.Employees(), "EmployeeID", "EmployeeName", dalEmpBll.getEID(currEmp));
            else {// 算法：判断是否是组内管理员，是employees为组内所有人，否Employees为自己
                GroupsBLL gBll = new GroupsBLL();
                List<SystemDB.Group> Groups = gBll.getGroupsEmpIsAdmin(currUser.iUserId);

                if (Groups != null && Groups.Count > 0)
                {
                    foreach (SystemDB.Group g in Groups)
                        foreach(SystemDB.UserGroupRef u in g.UserGroupRefs)                        
                            if (!Employees.Exists(ee => ee.EmployeeID == aerscBll.getInstanceUserID(u.UserId.Value).EmployeeID))
                                Employees.Add(dalEmpBll.getInstance(aerscBll.getInstanceUserID(u.UserId.Value).EmployeeID.Value));
                    ViewBag.Employees = new SelectList(Employees, "EmployeeID", "EmployeeName", dalEmpBll.getEID(currEmp));
                }
                else //当前员工自己
                    ViewBag.Employees = new SelectList(dalEmpBll.Employees().Where(w => w.EmployeeName == currUser.cUserName), "EmployeeID", "EmployeeName", dalEmpBll.getEID(currEmp));
            }
            attList = attRecordListBll.getRecordListWithEmpMonth(dn.Year, dn.Month, currEmp);
            if (Request.IsAjaxRequest())
                return PartialView("AttendanceList", attList);
            return View(attList);
        }
        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            currEmp = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name)).cUserName;
            int y = (f.AllKeys.Contains("Year")) ? int.Parse(f["Year"]) : DateTime.Now.Year;
            int m = (f.AllKeys.Contains("Month")) ? int.Parse(f["Month"]) : DateTime.Now.Month;
            currEmp = (f.AllKeys.Contains("selEmpId") && !string.IsNullOrEmpty(f["selEmpId"])) ? f["selEmpId"] : currEmp;
            attList = attRecordListBll.getRecordListWithEmpMonth(y, m, currEmp);
           
            if (Request.IsAjaxRequest())
                return PartialView("AttendanceList",attList);
            return View();
        }

        #region Event OK
        public ActionResult editEvent(int id,string param)
        {
            int recordId = id;
            AttendanceEventDeclaredBLL aeBll = new AttendanceEventDeclaredBLL();
            AttendanceRecordsBLL arBll = new AttendanceRecordsBLL();
            Attendance_Records ar = new Attendance_Records();
            ar = arBll.single(id);
            ViewBag.record = ar;
            bool isOnTime = false;
            ViewBag.isOnTime = bool.TryParse(param, out isOnTime) ? isOnTime : false;
            
            //指定recordId的事件列表
            IEnumerable<Attendance_EventDeclared> aeList = aeBll.filter(recordId: recordId,isBegin:isOnTime);
            return View(aeList);
        }

        [HttpPost]
        public ActionResult editEvent(int id, FormCollection f)
        { //SELECT autoID,EventDescription,FeeCar,FeeMeals,FeeOther,isCar,Other,Memo,chechMan,Manager,recordID,isBeginTime FROM Attendance_EventDeclared
            //data: { id: id, description:description, fee:fee, memo:memo,check:check,isOnTime:isOnTime,recordId:recordId, Operation: operation },
               
            Attendance_EventDeclared aed = new Attendance_EventDeclared();
            aed.autoID = f.AllKeys.Contains("id") ? int.Parse(f["id"]) : -1;
            aed.EventDescription = f.AllKeys.Contains("description") ? f["description"] : "无";
            decimal fee = -1;
            if (f.AllKeys.Contains("fee") && decimal.TryParse(f["fee"], out fee))
                aed.FeeOther = fee;
            else aed.FeeOther = 0;
            aed.Memo = f.AllKeys.Contains("memo") ? f["memo"] : "";
            aed.checkMan = f.AllKeys.Contains("check") ? f["check"] : null;
            bool isot = false;
            if (f.AllKeys.Contains("isOnTime") && bool.TryParse(f["isOnTime"], out isot))
                aed.isBeginTime = isot;
            else aed.isBeginTime = false;
            int rid = -1;
            if (f.AllKeys.Contains("recordId") && int.TryParse(f["recordId"], out rid))
                aed.recordID = rid;
            else aed.recordID = -1;

            string Operation = f["Operation"].ToString();
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "editEvent",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(aed),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject("GO -> " + Operation)
            });
            switch (Operation)
            {
                case "SaveRow":
                    return saveEvent(aed, false);
                case "DeleteRow":
                    return deleteEvent(aed, false);
                case "SaveALL":
                    return saveEvent(aed, true);
                case "DeleteALL":
                    return deleteEvent(aed, true);
                default:
                    return View();
            }
        }
        public ContentResult saveEvent(Attendance_EventDeclared aed, bool isMulti)
        {
            int id = aed.autoID;
            AttendanceEventDeclaredBLL aedBll = new AttendanceEventDeclaredBLL();
            bool r = false;

            if (id < 0)
            {//新增
                r = aedBll.Add(aed) > 0;
            }
            else
            {//更新 
                r = aedBll.update(aed) > 0;
            }
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "saveEvent",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { aed, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "保存成功!" };
            else return new ContentResult() { Content = isMulti ? "" : "保存失败" };
        }
        public ContentResult deleteEvent(Attendance_EventDeclared aed, bool isMulti)
        {
            AttendanceEventDeclaredBLL aedBll = new AttendanceEventDeclaredBLL();
            bool r = true;
            r &= (aedBll.delete(aed.autoID) > 0);
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "deleteEvent",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { aed, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : "删除成功！" };
            else
                return new ContentResult() { Content = isMulti ? "" : "删除失败" };
        }
        #endregion

        #region EventList
        public ActionResult eventOverList(string id, string param)
        {
            DALHWATTEmployee dalEmp = new DALHWATTEmployee();
            AttendanceVEventListBLL aelBll = new AttendanceVEventListBLL();
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            ViewBag.isChecker = currUser.RoleNames.Contains("attsChecker");
            ViewBag.AttendanceMan = dalEmp.getInstance(int.Parse(id)).EmployeeName;
            IEnumerable<v_AttendanceEvent> aeList = null;
            DateTime d = new DateTime();
            if (!string.IsNullOrEmpty(param) && DateTime.TryParse(param, out d))
            {
                aeList = aelBll.filter(empid: id, Year: d.Year, Month: d.Month, hasOver: true);
                ViewBag.YearMonth = d.Year + "-" + d.Month;
            }
            else
            { aeList = aelBll.filter(empid: id); ViewBag.YearMonth = "全部"; }

            return View(aeList.OrderBy(aeo => aeo.sDate));
        }
        public ActionResult eventAbsenceList(string id, string param)
        {            
            DALHWATTEmployee dalEmp = new DALHWATTEmployee();
            AttendanceVEventListBLL aelBll = new AttendanceVEventListBLL();
            AttendanceBLL aBll = new AttendanceBLL();
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            ViewBag.isChecker = currUser.RoleNames.Contains("attsChecker");
            ViewBag.AttendanceMan = dalEmp.getInstance(int.Parse(id)).EmployeeName;
            IEnumerable<v_AttendanceEvent> aeList = null;
            DateTime d = new DateTime();
            if (!string.IsNullOrEmpty(param) && DateTime.TryParse(param, out d))
            {
                aBll.AddAbsenceWithMonth(id, d,currUser.cUserName);//增加所有缺勤记录到缺勤事件
                aeList = aelBll.filter(empid: id, Year: d.Year, Month: d.Month, hasOver: false);
                ViewBag.YearMonth = d.Year + "-" + d.Month;
            }
            else
            { aeList = aelBll.filter(empid: id); ViewBag.YearMonth = "全部"; }

            return View(aeList.OrderBy(aeo=>aeo.sDate));
        }

        [HttpPost]
        public ContentResult saveChecker(int id, string chkMan)
        {
            int r = 0;
            AttendanceEventDeclaredBLL aedBll = new AttendanceEventDeclaredBLL();
            Attendance_EventDeclared aed = aedBll.single(id);
            if (aed.checkMan != chkMan)
            {
                aed.checkMan = chkMan;
                r = aedBll.update(aed);
            }
            else
                r = 0;
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
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
            return View();
        }

        #region EmpRefSchemePlan OK
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ActionResult editEmpRefSchemeClass(string id)
        {
            IEnumerable<Attendance_EmployeeRefSchemeClass> aerscList;
            AttendanceEmployeeRefSchemeClassBLL aerspBll = new AttendanceEmployeeRefSchemeClassBLL();
            //用户参照
            //系统用户表里角色为Employee的员工;离职员工将角色收回;
            RolesBLL rBll = new RolesBLL();
            IEnumerable<SystemDB.User> Users = rBll.getUsersWithRole("Employee");
            
            //SystemBLL.UserBLL userBll= new UserBLL();
            //var usl = userBll.Lists("").Where(uw => !string.IsNullOrEmpty(uw.RoleNames) && uw.RoleNames.Contains("Employee"));
            
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            SelectList slUser = new SelectList(Users, "iUserId", "cUserName", currUser.iUserId);
            ViewBag.Users = slUser;

            //考勤员工参照
            DALHWATTEmployee dalEmpBll = new DALHWATTEmployee();
            ViewBag.Employees = new SelectList(dalEmpBll.Employees(),"EmployeeID","EmployeeName",dalEmpBll.getEID(currEmp));

            //班次计划参照
            AttendanceSchemeClassBLL ascBll = new AttendanceSchemeClassBLL();            
            AttendanceSchemeBLL asBll = new AttendanceSchemeBLL();
            IEnumerable<Attendance_SchemeClass> ascList = ascBll.filter();
            foreach (Attendance_SchemeClass sc in ascList)
            {
                sc.className = sc.Attendance_Scheme.schemeName+":"+sc.className;
            }
            SelectList slClass = new SelectList(ascBll.filter(), "classId", "className");
            ViewBag.SchemeClass = slClass;
            
            //分析id
            if (string.IsNullOrEmpty(id))
            {
                //所有员工，所有班次计划
                aerscList = aerspBll.filter();
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
                            if (dalEmpBll.Exists(empid))
                            {
                                aerscList = aerspBll.filter(_empId: empid);
                                ViewBag.Employee = dalEmpBll.getInstance(empid);
                            }
                            else { aerscList = null; TempData["errMsg"] += "无此员工！"; }
                        }
                        else {
                            empid = dalEmpBll.getEID(ids[1]);
                            if (empid > 0 && dalEmpBll.Exists(empid))
                                aerscList = aerspBll.filter(_empId: empid);
                            else { aerscList = null; TempData["errMsg"] += "无此员工！"; }
                        }
                        break;
                    case "plan":
                        int planid = -1;
                        if (int.TryParse(ids[1], out planid) && planid > 0)
                        {
                            aerscList = aerspBll.filter(classId: planid);
                            ViewBag.currPlan = ascBll.single(planid);
                            ViewBag.scheme = asBll.single(ascBll.single(planid).schemeId.Value);
                        }
                        else { aerscList = null; TempData["errMsg"] += "方案班次计划号无效！"; }
                        break;
                    default:
                        aerscList = null;
                        break;
                }
            }
            //计算在职员工的考勤配置列表
            List<Attendance_EmployeeRefSchemeClass> r = new List<Attendance_EmployeeRefSchemeClass>();
            var aL = aerscList.ToList();//所有员工的考勤配置列表
            //var ul = Users.Select(u=>u.iUserId);//在职员工IEnumerable<iUserId>
            //var tmpul = ul.ToList();
            //foreach (var a in aL)
            //{
            //    if (ul.Contains(a.UserID.Value))
            //    {
            //        var ud = aL.Single(rs => rs.UserID.Value == a.UserID.Value);
            //        r.Add(ud);
            //    }
            //}
            r = aL;
            return View(r);
        }

        [AuthorizeEx(Roles = "AttendanceManager")]
        [HttpPost]
        public ActionResult editEmpRefSchemeClass(FormCollection f, int id)
        {
            //SELECT autoid,UserID,EmployeeID,classID,effDate,periodNo,isVaild FROM Attendance_EmployeeRefSchemeClass

            Attendance_EmployeeRefSchemeClass aersc = new Attendance_EmployeeRefSchemeClass();
            aersc.autoid = id;
            aersc.EmployeeID = int.Parse(f["EmployeeID"]);
            aersc.EmployeeName = f["EmployeeName"];
            aersc.EffDate = DateTime.Parse(f["effDate"]);
            aersc.periodNo = int.Parse(f["periodNo"]);
            aersc.classID = int.Parse(f["classID"]);
            aersc.UserID = int.Parse(f["UserID"]);

            string Operation = f["Operation"].ToString();
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
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
        public ContentResult saveEmpRefSchemeClass(Attendance_EmployeeRefSchemeClass aersc, bool isMulti)
        {
            int id = aersc.autoid;
            AttendanceEmployeeRefSchemeClassBLL aerspBll = new AttendanceEmployeeRefSchemeClassBLL();
            bool r = false;

            if (id < 0)
            {//新增
                r = aerspBll.Add(aersc) > 0;
            }
            else
            {//更新 
                r = aerspBll.update(aersc) > 0;
            }
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
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
        public ContentResult deleteEmpRefSchemeClass(Attendance_EmployeeRefSchemeClass aersc, bool isMulti)
        {
            AttendanceEmployeeRefSchemeClassBLL aerscBll = new AttendanceEmployeeRefSchemeClassBLL();
            bool r = true;
            r &= (aerscBll.delete(aersc.autoid) > 0);
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
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
            AttendanceSchemeClassBLL ascBll = new AttendanceSchemeClassBLL();
            ViewBag.schemeClass = ascBll.single(id);
            AttendanceSchemeBLL asBll = new AttendanceSchemeBLL();
            ViewBag.scheme = asBll.single(ascBll.single(id).schemeId.Value);
            AttendanceClassPlanBLL acpBll = new AttendanceClassPlanBLL();
            IEnumerable<Attendance_ClassPlan> acpList;
            acpList = acpBll.filter(_classId: id);
            //var t = acpList.ToList();
            return View(acpList);
        }

        [AuthorizeEx(Roles = "AttendanceManager")]
        [HttpPost]
        public ActionResult editClassPlan(FormCollection f, int id)
        {
            //SELECT SchemeClassID,schemeID,classes,periodNo,bTime,eTime,upTime,sdate FROM SchemeClass
            Attendance_ClassPlan asc = new Attendance_ClassPlan();
            asc.autoid = id;
            asc.classId = int.Parse(f["classId"]);
            asc.bTime = f["bTime"];
            asc.eTime = f["eTime"];
            asc.periodNo = int.Parse(f["periodNo"]);
            asc.upTime = f["upTime"];
            asc.sdate = null;
            DateTime d = default(DateTime);
            if (f.AllKeys.Contains("sDate") && DateTime.TryParse(f["sDate"], out d))
                asc.sdate = d;
            string Operation = f["Operation"].ToString();
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "editClassPlan",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(asc),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject("GO -> "+Operation)
            });
            switch (Operation)
            {
                case "SaveRow":
                    return saveClassPlan(asc, false);
                case "DeleteRow":
                    return deleteClassPlan(asc, false);
                case "SaveALL":
                    return saveClassPlan(asc, true);
                case "DeleteALL":
                    return deleteClassPlan(asc, true);
                default:
                    return View();
            }
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult saveClassPlan(Attendance_ClassPlan acp, bool isMulti)
        {
            int id = acp.autoid;
            AttendanceClassPlanBLL acpBll = new AttendanceClassPlanBLL();
            bool r = false;

            if (id < 0)
            {//新增
                r = acpBll.Add(acp) > 0;
            }
            else
            {//更新 
                r = acpBll.update(acp) > 0;
            }
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
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
        public ContentResult deleteClassPlan(Attendance_ClassPlan acp, bool isMulti)
        {
            AttendanceClassPlanBLL acpBll = new AttendanceClassPlanBLL();
            bool r = true;
            r &= acpBll.delete(acp.classId.Value) > 0;

            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
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
            AttendanceClassPlanBLL acpBll = new AttendanceClassPlanBLL();
            var a = acpBll.Exists(classid, period,sdate);//返回存在的实例
            if (a != null) r = a.autoid != id;
            return r;
        }
        #endregion

        #region SchemeClass OK
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ActionResult editSchemeClass(int id = -1)
        {
            AttendanceSchemeBLL asBll = new AttendanceSchemeBLL();
            ViewBag.scheme = asBll.single(id);
            AttendanceSchemeClassBLL ascBll = new AttendanceSchemeClassBLL();
            IEnumerable<Attendance_SchemeClass> ascList;
            ascList = ascBll.filter(_schemeId: id);
            return View(ascList);
        }

        [AuthorizeEx(Roles = "AttendanceManager")]
        [HttpPost]
        public ActionResult editSchemeClass(FormCollection f, int id)
        {
            //SELECT classId,schemeId,className FROM Attendance_SchemeClass
            Attendance_SchemeClass asc = new Attendance_SchemeClass();
            asc.classId = id;
            asc.schemeId = int.Parse(f["schemeId"]);
            asc.className =  f["className"];

            string Operation = f["Operation"].ToString();
            switch (Operation)
            {
                case "SaveRow":
                    return saveSchemeClass(asc, false);
                case "DeleteRow":
                    return deleteSchemeClass(asc, false);
                case "SaveALL":
                    return saveSchemeClass(asc, true);
                case "DeleteALL":
                    return deleteSchemeClass(asc, true);
                default:
                    return View();
            }
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult saveSchemeClass(Attendance_SchemeClass asc, bool isMulti)
        {
            int id = asc.classId;
            AttendanceSchemeClassBLL ascBll = new AttendanceSchemeClassBLL();
            bool r = false;

            if (id < 0)
            {//新增
                r = ascBll.Add(asc) > 0;
            }
            else
            {//更新 
                r = ascBll.update(asc) > 0;
            }
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
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
        public ContentResult deleteSchemeClass(Attendance_SchemeClass asc, bool isMulti)
        {
            AttendanceEmployeeRefSchemeClassBLL aersp = new AttendanceEmployeeRefSchemeClassBLL();
            AttendanceSchemeClassBLL aspBll = new AttendanceSchemeClassBLL();
            bool r = true;
            r &= aersp.filter(classId:asc.classId).Count() <= 0;
            r &= aspBll.delete(asc.classId) > 0;
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
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
            AttendanceSchemeBLL asBll = new AttendanceSchemeBLL();
            IEnumerable<Attendance_Scheme> asList;
            asList = asBll.filter();
            return View(asList); 
        }

        [AuthorizeEx(Roles = "AttendanceManager")]
        [HttpPost]
        public ActionResult editScheme(FormCollection f, int id)
        {
            //SELECT schemeID,schemeName,schemeDescription,periods,classs FROM Attendance_Scheme
            Attendance_Scheme atts = new Attendance_Scheme();
            atts.schemeID = id;
            atts.periods = int.Parse(f["periods"]);
            atts.classs = int.Parse(f["classs"]);
            atts.schemeName = f["name"];
            atts.schemeDescription = f["description"];

            string Operation = f["Operation"].ToString();
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "editScheme",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(atts),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject("GO -> " + Operation)
            });
            switch (Operation)
            {
                case "SaveRow":
                    return saveScheme(atts, false);
                case "DeleteRow":
                    return deleteScheme(atts, false);
                case "SaveALL":
                    return saveScheme(atts, true);
                case "DeleteALL":
                    return deleteScheme(atts, true);
                default:
                    return View();
            }
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult saveScheme(Attendance_Scheme atts, bool isMulti)
        {
            int id = atts.schemeID;
            AttendanceSchemeBLL asBll = new AttendanceSchemeBLL();
            bool r = false;

            if (id < 0)
            {//新增
                r = asBll.Add(atts) > 0;
            }
            else
            {//更新 
                r = asBll.update(atts) > 0;
            }
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "saveScheme",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { atts, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : atts.schemeName + " 保存成功!" };
            else return new ContentResult() { Content = isMulti ? "" : "保存失败" };
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult deleteScheme(Attendance_Scheme atts, bool isMulti)
        {
            AttendanceSchemeBLL asBll = new AttendanceSchemeBLL();
            AttendanceSchemeClassBLL ascBll = new AttendanceSchemeClassBLL();
            bool r = true;
            r &= ascBll.filter(atts.schemeID).Count() <= 0;
            r &= asBll.delete(atts.schemeID) > 0;
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "deleteClassPlan",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(new { atts, isMulti }),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject(r)
            });
            if (r)
                return new ContentResult() { Content = isMulti ? "" : atts.schemeName + " 删除成功！" };
            else
                return new ContentResult() { Content = isMulti ? "" : "删除失败,记录不存在或有向下关联的数据" };
        }
        #endregion

        #region Holiday OK
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ActionResult editHoliday()
        {
            AttendanceHolidayBLL ahBll = new AttendanceHolidayBLL();
            IEnumerable<Attendance_Holiday> ahList;
            ahList = ahBll.filter(DateTime.Now.Year);
            if (ahList.Count() <= 0) return View();
            return View(ahList); 
        }

        [AuthorizeEx(Roles = "AttendanceManager")]
        [HttpPost]
        public ActionResult editHoliday(FormCollection f,int id)
        {
            Attendance_Holiday ah = new Attendance_Holiday();
            ah.autoid = id;
            ah.sDate = DateTime.Parse(f["date"]);
            ah.iYear = ah.sDate.Value.Year;
            ah.sName = f["holiday"];
            ah.sDescription = f["holidayDescription"];

            string Operation = f["Operation"].ToString();
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "editHoliday",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(ah),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject("GO -> " + Operation)
            });
            switch (Operation)
            {
                case "SaveRow":
                    return saveHoliday(ah,false);
                case "DeleteRow":
                    return deleteHoliday(ah,false);
                case "SaveALL":
                    return saveHoliday(ah,true);
                case "DeleteALL":
                    return deleteHoliday(ah,true);
                default:
                    return View();
            }
        }
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ContentResult saveHoliday(Attendance_Holiday ah, bool isMulti)
        {
            int id = ah.autoid;
            AttendanceHolidayBLL ahBll = new AttendanceHolidayBLL();
            bool r = false;
            
            if (id < 0)
            {//新增
                r = ahBll.Add(ah) > 0;
            }
            else
            {//更新 
                r = ahBll.update(ah) > 0;
            }
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
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
        public ContentResult deleteHoliday(Attendance_Holiday ah, bool isMulti)
        {
            AttendanceHolidayBLL ahBll = new AttendanceHolidayBLL();
            bool r = false;
            r = (ahBll.delete(ah.autoid) > 0);
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
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
        public bool isExistHoliday(int id,string sdate)
        {
            AttendanceHolidayBLL ahBll = new AttendanceHolidayBLL();
            bool r = false;
            var a = ahBll.Exists(sdate);
            if (a != null) r = a.autoid != id;
            return r;
        }
        #endregion

        #region FeeCalculator Test
        [AuthorizeEx(Roles = "AttendanceManager")]
        public ActionResult editFeeCalculator(int id)
        {
            AttendanceSchemeClassBLL ascBll = new AttendanceSchemeClassBLL();
            ViewBag.schemeClass = ascBll.single(id);
            AttendanceSchemeBLL asBll = new AttendanceSchemeBLL();
            ViewBag.scheme = asBll.single(ascBll.single(id).schemeId.Value);
            AttendanceFeeCalculatorBLL afcBll = new AttendanceFeeCalculatorBLL();
            //ViewBag.dateEnum = new SelectList(Enum.GetNames(typeof(Attendance_FeeCalculator.dayState)).AsEnumerable());
            ViewBag.dateEnum = new SelectList(Attendance_FeeCalculator.cnDayState.AsEnumerable(),"Key","Value");
            IEnumerable<Attendance_FeeCalculator> afcs;
            afcs = afcBll.filter(classId: id);
            return View(afcs);
        }
        [HttpPost]
        public ActionResult editFeeCalculator(FormCollection f ,int id) 
        {
            //SELECT id,Unit,UnitFee,MaxFee,classId,dateEnum FROM Attendance_FeeCalculator
            Attendance_FeeCalculator afc = new Attendance_FeeCalculator();
            afc.id = id;
            afc.classId = f.AllKeys.Contains("classId")?int.Parse(f["classId"]):-1;
            afc.dateEnum = f["dateEnum"] ;
            afc.Unit = f.AllKeys.Contains("Unit") ? int.Parse(f["Unit"]) : 1;
            afc.UnitFee = f.AllKeys.Contains("UnitFee") ? decimal.Parse(f["UnitFee"]) : 0;
            afc.MaxFee = f.AllKeys.Contains("MaxFee") ? decimal.Parse(f["MaxFee"]) : 0;

            string Operation = f["operation"];
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
            {
                iUserID = currUser.iUserId,
                cUserName = currUser.cUserName,
                cModule = "Attendance",
                cClass = "AttendanceController",
                cFunction = "editFeeCalculator",
                cParams = Newtonsoft.Json.JsonConvert.SerializeObject(afc),
                cReturn = Newtonsoft.Json.JsonConvert.SerializeObject("GO -> " + Operation)
            });
            switch (Operation)
            {
                case "SaveRow":
                    return saveFeeCalculator(afc, false);
                case "DeleteRow":
                    return deleteFeeCalculator(afc, false);
                case "SaveALL":
                    return saveFeeCalculator(afc, true);
                case "DeleteALL":
                    return deleteFeeCalculator(afc, true);
                default:
                    return View();
            }

            return View();
        }
        public ActionResult saveFeeCalculator(Attendance_FeeCalculator afc, bool isMulti) 
        {

            int id = afc.id;
            AttendanceFeeCalculatorBLL afcBll = new AttendanceFeeCalculatorBLL();
            bool r = false;

            if (id < 0)
            {//新增
                r = afcBll.add(afc) > 0;
            }
            else
            {//更新 
                r = afcBll.update(afc) > 0;
            }
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
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
        public ActionResult deleteFeeCalculator(Attendance_FeeCalculator afc, bool isMulti)
        {
            AttendanceFeeCalculatorBLL afcBll = new AttendanceFeeCalculatorBLL();
            bool r = true;
            r &= afcBll.delete(afc) > 0;
            currUser = UserBLL.getUser(int.Parse(HttpContext.User.Identity.Name));
            logbll.Add(new SystemDB.Logs()
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
