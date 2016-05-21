using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Entity;
using HWATT;

namespace Attendance
{
    public class AttendanceHolidayBLL
    {
        private AppDataAttendanceEntities attendance = new AppDataAttendanceEntities();
        
        public bool Exists(int id)
        {
            bool r = false;
            try
            {
                var hd = attendance.Attendance_Holiday.FirstOrDefault(h => h.autoid == id);
                r = true;
            }
            catch { r = false; }
            return r;
        }
        public　Attendance_Holiday Exists(string sdate)
        {
            Attendance_Holiday r;
            try
            {
                DateTime d = DateTime.Parse(sdate);
                r = attendance.Attendance_Holiday.FirstOrDefault(h => h.sDate == d);
            }
            catch { r = null; }
            return r;
        }

        public bool isHoliday(DateTime sDate)
        {
            bool r = false;
            try
            {
                attendance.Attendance_Holiday.First(h => h.sDate == sDate);
                r = true;
            }
            catch { r = false; }
            return r;
        }

        public int Add(Attendance_Holiday ah)
        {
            int r = -1;
            attendance.Attendance_Holiday.Add(ah);
            attendance.SaveChanges();
            r = ah.autoid;
            return r;
        }
        public int update(Attendance_Holiday ah)
        {
            int r = -1;
            var u = attendance.Attendance_Holiday.First(h => h.autoid == ah.autoid);
            u.iYear = ah.iYear;
            u.sDate = ah.sDate;
            u.sDescription = ah.sDescription;
            u.sName = ah.sName;
            
            r = attendance.SaveChanges();
            return r;
        }
        public int delete(int _autoid)
        {
            int r = -1;
            var d = attendance.Attendance_Holiday.Single(h => h.autoid == _autoid);
            attendance.Attendance_Holiday.Remove(d);
            r = attendance.SaveChanges();
            return r;
        }
        public Attendance_Holiday single(int autoid)
        {
            try
            {
                var r = attendance.Attendance_Holiday.Single(h => h.autoid == autoid);
                return r;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<Attendance_Holiday> filter(int year,DateTime dt = default(DateTime))
        {
            IEnumerable<Attendance_Holiday> r;
            IQueryable<Attendance_Holiday> f = attendance.Attendance_Holiday;
            if (year >DateTime.MinValue.Year)
                f = f.Where(ff => ff.iYear == year);
            if (dt != default(DateTime))
                f = f.Where(ff => ff.sDate == dt);
            r = f;
            return r;
        }
    }
    public class AttendanceSchemeBLL
    {
        private AppDataAttendanceEntities attendance = new AppDataAttendanceEntities();
        public bool Exists(int id)
        {
            bool r = false;
            try { attendance.Attendance_Scheme.FirstOrDefault(s => s.schemeID == id); r = true; }
            catch { r = false; }
            return r;
        }
        public int Add(Attendance_Scheme atts)
        {
            int r = -1;
            attendance.Attendance_Scheme.Add(atts);
            attendance.SaveChanges();
            r = atts.schemeID;
            return r;
        }
        public int update(Attendance_Scheme atts)
        {
            int r = -1;
            var u = attendance.Attendance_Scheme.Single(s => s.schemeID == atts.schemeID);
            u.schemeName = atts.schemeName;
            u.schemeDescription = atts.schemeDescription;
            u.periods = atts.periods;
            u.classs = atts.classs;
            
            r = attendance.SaveChanges();
            return r;
        }
        public int delete(int id)
        {
            int r = -1;
            var d = attendance.Attendance_Scheme.Single(s => s.schemeID == id);
            attendance.Attendance_Scheme.Remove(d);
            r = attendance.SaveChanges();
            return r;
        }
        public Attendance_Scheme single(int id)
        {
            try
            {
                var r = attendance.Attendance_Scheme.Single(s => s.schemeID == id);
                return r;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<Attendance_Scheme> filter()
        {
            IEnumerable<Attendance_Scheme> r;
            IQueryable<Attendance_Scheme> f = attendance.Attendance_Scheme;
            //if (参数条件) f = f.Where(ff=>ff.参数字段==参数值);
            r = f;
            return r;
        }
    }

    public class AttendanceSchemeClassBLL
    {
        private AppDataAttendanceEntities attendance = new AppDataAttendanceEntities();
        public bool Exists(int id)
        {
            bool r = false;
            try
            {
                attendance.Attendance_SchemeClass.FirstOrDefault(asc => asc.classId == id);
                r = true;
            }
            catch { r = false; }
            return r;
        }
        public int Add(Attendance_SchemeClass asc)
        {
            int r = -1;
            attendance.Attendance_SchemeClass.Add(asc);
            attendance.SaveChanges();
            r = asc.classId;
            return r;
        }
        public int update(Attendance_SchemeClass asc)
        {
            int r = -1;
            var u = attendance.Attendance_SchemeClass.Single(s => s.classId == asc.classId);
            u.classId = asc.classId;
            u.className = asc.className;
            u.schemeId = asc.schemeId;

            r = attendance.SaveChanges();
            return r;
        }
        public int delete(int id)
        {
            int r = -1;
            var d = attendance.Attendance_SchemeClass.Single(asc => asc.classId == id);
            attendance.Attendance_SchemeClass.Remove(d);
            r = attendance.SaveChanges();
            return r;
        }
        public Attendance_SchemeClass single(int id)
        {
            try
            {
                var r = attendance.Attendance_SchemeClass.Single(s => s.classId == id);
                return r;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<Attendance_SchemeClass> filter( int _classId = -1,int _schemeId = -1)
        {
            IEnumerable<Attendance_SchemeClass> r;
            IQueryable<Attendance_SchemeClass> f = attendance.Attendance_SchemeClass;
            //if (参数条件) f = f.Where(ff=>ff.参数字段==参数值);
            if (_classId > 0)
                f = f.Where(fw => fw.classId == _classId);
            if (_schemeId > 0)
                f = f.Where(fw => fw.schemeId == _schemeId);
            r = f;
            return r;
        }
    }
    public class AttendanceClassPlanBLL
    {
        private AppDataAttendanceEntities attendance = new AppDataAttendanceEntities();
        public bool Exists(int id)
        {
            bool r = false;
            try
            {
                attendance.Attendance_ClassPlan.FirstOrDefault(acp=>acp.autoid == id);
                r = true;
            }
            catch { r = false; }
            return r;
        }
        public Attendance_ClassPlan Exists(int classid, int period,string sdate)
        {
            Attendance_ClassPlan r;
            DateTime d = default(DateTime);
            try
            {
                if (!string.IsNullOrEmpty(sdate) && DateTime.TryParse(sdate, out d))
                    r = attendance.Attendance_ClassPlan.FirstOrDefault(acp => acp.classId == classid && acp.periodNo < 0 && acp.sdate == d);
                else
                    r = attendance.Attendance_ClassPlan.FirstOrDefault(acp => acp.classId == classid && acp.periodNo == period);
            }
            catch { r = null; }
            return r;
        }

        public int Add(Attendance_ClassPlan acp)
        {
            int r = -1;
            attendance.Attendance_ClassPlan.Add(acp);
            attendance.SaveChanges();
            r = acp.autoid;
            return r;
        }
        public int update(Attendance_ClassPlan acp)
        {
            int r = -1;
            var u = attendance.Attendance_ClassPlan.Single(s => s.autoid == acp.autoid);
            u.autoid = acp.autoid;
            u.upTime = acp.upTime;
            u.periodNo = acp.periodNo;
            u.eTime = acp.eTime;
            u.bTime = acp.bTime;
            u.classId = acp.classId;
            u.sdate = acp.sdate;

            r = attendance.SaveChanges();
            return r;
        }
        public int delete(int id)
        {
            int r = -1;
            var d = attendance.Attendance_ClassPlan.Single(acp=>acp.autoid==id);
            attendance.Attendance_ClassPlan.Remove(d);
            r = attendance.SaveChanges();
            return r;
        }
        public bool isWeekend(Attendance_ClassPlan acp)
        {
            bool r = false;
            r = acp.sdate.Value.DayOfWeek == DayOfWeek.Sunday;            
            return r;
        }
        public bool isDayOff(Attendance_ClassPlan acp)
        {
            bool r = false;
            r = acp.bTime == acp.eTime;
            return r;
        }
        public Attendance_ClassPlan single(int id)
        {
            try
            {
                var r = attendance.Attendance_ClassPlan.Single(s => s.autoid == id);
                setDayState(r);
                return r;
            }
            catch
            {
                return null;
            }
        }
        private void setDayState(Attendance_ClassPlan acp)
        {
            if (acp.bTime == acp.eTime)
                acp.setDayState(Attendance_ClassPlan.dayState.DayOff);
            else
                acp.setDayState(Attendance_ClassPlan.dayState.WorkDay);
        }
        public IEnumerable<Attendance_ClassPlan> filter(int autoid = -1,int _classId = -1,int _period = -1,DateTime sdate = default(DateTime))
        {
            IEnumerable<Attendance_ClassPlan> r;
            IQueryable<Attendance_ClassPlan> f = attendance.Attendance_ClassPlan;
            //if (参数条件) f = f.Where(ff=>ff.参数字段==参数值);
            if (autoid > 0)
                f = f.Where(fw => fw.autoid == autoid);
            if (_classId > 0)
                f = f.Where(fw => fw.classId == _classId);
            if (_period >= 0 && sdate == default(DateTime))
                f = f.Where(fw => fw.periodNo == _period);
            if (sdate != default(DateTime))
                f = f.Where(fw => fw.periodNo < 0 && fw.sdate == sdate);
            r = f;
            return r;
        }
    }
    public class AttendanceEmployeeRefSchemeClassBLL
    {
        private AppDataAttendanceEntities attendance = new AppDataAttendanceEntities();
        public bool Exists(int id)
        {
            bool r = false;
            try
            {
                attendance.Attendance_EmployeeRefSchemeClass.FirstOrDefault(f=>f.autoid == id);                
                r = true;
            }
            catch { r = false; }
            return r;
        }
        public int Add(Attendance_EmployeeRefSchemeClass m)
        {
            int r = -1;
            m.isVaild = true;
            attendance.Attendance_EmployeeRefSchemeClass.Add(m);
            attendance.SaveChanges();
            //批量更改有效标识
            var us = attendance.Attendance_EmployeeRefSchemeClass
                .Where(w => w.EmployeeID == m.EmployeeID && w.autoid != m.autoid);
            //var ul = us.ToList();
            if (us.Count() > 0)
            {
                foreach (var u in us)
                    u.isVaild = false;
                //us.Update(ud => new Attendance_EmployeeRefSchemeClass { isVaild = false });
                attendance.SaveChanges();
            }
            r = m.autoid;
            return r;
        }
        public int update(Attendance_EmployeeRefSchemeClass m)
        {
            int r = -1;
            var u = attendance.Attendance_EmployeeRefSchemeClass.Single(s => s.autoid == m.autoid);
            u.EmployeeID = m.EmployeeID;
            u.EffDate = m.EffDate;
            u.classID = m.classID;
            u.UserID = m.UserID;
            u.isVaild = true;
            u.EmployeeName = m.EmployeeName;
            r = attendance.SaveChanges();
            //批量更改有效标识
            var us = attendance.Attendance_EmployeeRefSchemeClass.Where(w => w.EmployeeID == m.EmployeeID && w.autoid != m.autoid);
            if (us.Count() > 0)
            {
                foreach (var uu in us)
                    uu.isVaild = false;
                //us.Update(ud => new Attendance_EmployeeRefSchemeClass { isVaild = false });
                attendance.SaveChanges();
            }
            return r;
        }
        public int delete(int id)
        {
            int r = -1;
            var del = attendance.Attendance_EmployeeRefSchemeClass.Single(ds => ds.autoid == id);
            attendance.Attendance_EmployeeRefSchemeClass.Remove(del);
            r = attendance.SaveChanges();
            if (del.isVaild.Value)
            {
                var vailds = attendance.Attendance_EmployeeRefSchemeClass
                    .Where(w => w.EmployeeID == del.EmployeeID);
                if (vailds == null && vailds.Count() >0)
                {
                    var vaild = vailds.OrderByDescending(o => o.EffDate).First();
                    vaild.isVaild = true;
                    attendance.SaveChanges();
                }
            }
            return r;
        }
        public Attendance_EmployeeRefSchemeClass single(int id)
        {
            try
            {
                var r = attendance.Attendance_EmployeeRefSchemeClass.Single(s => s.autoid == id);
                return r;
            }
            catch
            {
                return null;
            }
        }
        public IQueryable<Attendance_EmployeeRefSchemeClass> filter(int _empId = -1,string _empName = null,int classId = -1,int userId = -1,bool ? isVaild = null)
        {
            IQueryable<Attendance_EmployeeRefSchemeClass> f = attendance.Attendance_EmployeeRefSchemeClass;
            //if (参数条件) f = f.Where(ff=>ff.参数字段==参数值);
            if (!string.IsNullOrEmpty(_empName)) f = f.Where(fw => fw.EmployeeName == _empName);
            if (_empId > 0) {
                f = f.Where(fw=>fw.EmployeeID == _empId);
            }
            if (classId > 0)
            { f = f.Where(fw=>fw.classID == classId); }
            if (userId > 0)
            { f = f.Where(fw => fw.UserID == userId); }
            if (isVaild.HasValue)
            { f = f.Where(fw => fw.isVaild == isVaild); }
            return f;
        }
        public Attendance_EmployeeRefSchemeClass getInstanceEmpID(int empID)
        {
            Attendance_EmployeeRefSchemeClass r;
            try
            {
                r = filter(_empId:empID).Where(w => w.isVaild == true).OrderByDescending(o => o.EffDate).First();
            }
            catch { r = null; }
            return r;
        }
        public Attendance_EmployeeRefSchemeClass getInstanceUserID(int userID)
        {
            Attendance_EmployeeRefSchemeClass r;
            try
            {
                r = filter(userId:userID).Where(w => w.isVaild == true).OrderByDescending(o => o.EffDate).First();
            }
            catch { r = null;
            throw new ArgumentNullException("无指定用户号的考勤规则关联", new Exception("无指定用户号的考勤规则关联"));
            }
            return r;
        }
        public Attendance_EmployeeRefSchemeClass getInstanceEmpName(string empName)
        {
            Attendance_EmployeeRefSchemeClass r;
            try
            {
                r = filter(_empName: empName).Where(w => w.isVaild == true).OrderByDescending(o => o.EffDate).First();
            }
            catch { r = null; }
            return r;
        }
        /// <summary>
        /// 获得指定员工日期的有效Class
        /// </summary>
        /// <param name="_empId"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int getClassId(int _empId,DateTime dt)
        {
            int classId = -1;
            var f = attendance.Attendance_EmployeeRefSchemeClass.Where(e=>e.EmployeeID == _empId);
            f=f.Where(fw=>dt>=fw.EffDate).OrderByDescending(fo=>fo.EffDate);
            var fr = f.ToList();
            classId=f.FirstOrDefault().classID.Value;
            return classId;
        }
        /// <summary>
        /// 返回员工id列表List_int
        /// </summary>
        /// <returns></returns>
        public List<int> getEmpIds()
        {
            var eq = from e in attendance.Attendance_EmployeeRefSchemeClass
                     where e.UserID.HasValue && e.UserID.Value>0
                     select new { id = e.EmployeeID, name = e.EmployeeName, userid = e.UserID };
            List<int> r = new List<int>();
            foreach (var ed in eq.Distinct())
                r.Add(ed.id.Value);
            return r;
        }
        public void setIsVaild(int _uid)
        {
            var ersc = attendance.Attendance_EmployeeRefSchemeClass.Single(s=>s.UserID == _uid);
            ersc.isVaild = false;
            attendance.SaveChanges();
        }
    }
    public class AttendanceRecordsBLL
    {
        DALHWATTEmployee dalHwattEmp = new DALHWATTEmployee();
        private AppDataAttendanceEntities attendance = new AppDataAttendanceEntities();
        public bool Exists(int id)
        {
            bool r = false;
            try
            {
                //Exists
                attendance.Attendance_Records.FirstOrDefault(f => f.autoid == id);
                r = true;
            }
            catch { r = false; }
            return r;
        }
        public int Add(RecordModel m)
        {
            int r = -1;
            
            attendance.Attendance_Records.Add(m);
            r = attendance.SaveChanges();
            return r;
        }
        public int update(RecordModel m)
        {
            int r = -1;
            var u = attendance.Attendance_Records.Single(s => s.autoid == m.autoid);
            u.bAttTimeStr = m.bAttTimeStr;
            u.bOffset = m.bOffset;
            u.eAttTimeStr = m.eAttTimeStr;
            u.EmployeeID = m.EmployeeID;
            u.eOffset = m.eOffset;
            u.periodNo = m.periodNo;
            u.classID = m.classID;
            u.sDate = m.sDate;
            u.dateType = m.dateType;
            u.bOffsetFee = m.bOffsetFee;
            u.eOffsetFee = m.eOffsetFee;

            r = attendance.SaveChanges();
            return r;
        }
        public int delete(int id)
        {
            int r = -1;
            var del = attendance.Attendance_Records.Single(d=>d.autoid==id);
            attendance.Attendance_Records.Remove(del);
            r = attendance.SaveChanges();
            return r;
        }
        public RecordModel single(int id)
        {
            try
            {
                var r = attendance.Attendance_Records.Single(s => s.autoid == id);
                return r;
            }
            catch
            {
                return null;
            }
        }
        public IQueryable<RecordModel> filter(DateTime s,DateTime e,string _emp = null)
        {
            IQueryable<RecordModel> f = attendance.Attendance_Records;
            
            //if (参数条件) f = f.Where(ff=>ff.参数字段==参数值);
            //指定员工已记录考勤列表,按考勤日期倒序
            if (!string.IsNullOrEmpty(_emp))
            {                
                int _empid;
                if (!int.TryParse(_emp,out _empid))
                    _empid = dalHwattEmp.getEID(_emp);                
                if (_empid > 0)
                    f = f.Where(fw => fw.EmployeeID == _empid).OrderByDescending(fo => fo.sDate);
            }
            if (s != null)
            {
                s = (s <= System.Data.SqlTypes.SqlDateTime.MinValue.Value) ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : s;
                f = f.Where(fw => fw.sDate >= s);
            }

            if (e != null)
            {
                e = (e >= System.Data.SqlTypes.SqlDateTime.MaxValue.Value) ? System.Data.SqlTypes.SqlDateTime.MaxValue.Value : e;
                f = f.Where(fw => fw.sDate <= e);
            }
            return f;
        }
        public RecordModel getLastRecord(string _emp)
        {
            RecordModel r;
            try
            {
                r = filter(DateTime.MinValue, DateTime.MaxValue, _emp: _emp).First();
            }
            catch { r = null; }
            return r;
        }
        /// <summary>
        /// 获取指定员工,指定方案计划,最后考勤记录的 周期名
        /// </summary>
        /// <param name="_emp"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public int getPeriodNo(int _emp, Attendance_SchemeClass asc)
        {
            AttendanceSchemeBLL asBll = new AttendanceSchemeBLL();
            AttendanceEmployeeRefSchemeClassBLL aerscBll = new AttendanceEmployeeRefSchemeClassBLL();
            Attendance_EmployeeRefSchemeClass aersc = aerscBll.getInstanceEmpID(_emp);

            RecordModel attR = getLastRecord(_emp.ToString());
            Attendance_Scheme attS = asBll.single(asc.schemeId.Value);
            int r = (attR == null) ? aersc.periodNo.Value : attR.periodNo.Value + 1;
            r %= attS.periods.Value;
            return r;
        }
        /// <summary>
        /// 获取指定员工最后考勤记录的 周期名
        /// </summary>
        /// <param name="_emp"></param>
        /// <returns></returns>
        public int getPeriodNo(int _emp)
        {
            AttendanceSchemeClassBLL ascBll = new AttendanceSchemeClassBLL();
            AttendanceEmployeeRefSchemeClassBLL aerscBll = new AttendanceEmployeeRefSchemeClassBLL();
            Attendance_EmployeeRefSchemeClass aersc = aerscBll.getInstanceEmpID(_emp);
            RecordModel attR = getLastRecord(_emp.ToString());
            Attendance_SchemeClass acp = (attR != null) ?
                ascBll.single(attR.classID.Value)
                : ascBll.single(aersc.classID.Value);
            return getPeriodNo(_emp, acp);
        }



        internal int Save(RecordModel onceRecord)
        {
            //throw new NotImplementedException();
            int r = -1;
            if (onceRecord.autoid > 0)
                r = update(onceRecord);
            else r = Add(onceRecord);
            return r;
        }
    }
    public class AttendanceEventDeclaredBLL
    {
        private AppDataAttendanceEntities attendance = new AppDataAttendanceEntities();
        public bool Exists(int id)
        {
            bool r = false;
            try
            {
                //Exists
                attendance.Attendance_EventDeclared.First(f=>f.autoID == id);
                r = true;
            }
            catch { r = false; }
            return r;
        }
        public int Add( Attendance_EventDeclared m)
        {
            int r = -1;
            attendance.Attendance_EventDeclared.Add(m);
            attendance.SaveChanges();
            r = m.autoID;
            return r;
        }
        public int update(Attendance_EventDeclared m)
        {
            int r = -1;
            var u = attendance.Attendance_EventDeclared.Single(s=>s.autoID == m.autoID);
            u.checkMan = m.checkMan;
            u.EventDescription = m.EventDescription;
            u.FeeCar = m.FeeCar;
            u.FeeMeals = m.FeeMeals;
            u.FeeOther = m.FeeOther;
            u.isCar = m.isCar;
            u.Manager = m.Manager;
            u.Memo = m.Memo;
            u.Other = m.Other ;
            u.recordID = m.recordID;
            u.isBeginTime = m.isBeginTime;
            r = attendance.SaveChanges();
            return r;
        }
        public int delete(int id)
        {
            int r = -1;
            var del = attendance.Attendance_EventDeclared.Single(d => d.autoID == id);
            attendance.Attendance_EventDeclared.Remove(del);
            r = attendance.SaveChanges();

            return r;
        }
        public Attendance_EventDeclared single(int id)
        {
            try
            {
                var r = attendance.Attendance_EventDeclared.Single(s => s.autoID == id);
                return r;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<Attendance_EventDeclared> filter(int recordId = -1,bool? isBegin = null)
        {
            IEnumerable<Attendance_EventDeclared> r;
            IQueryable<Attendance_EventDeclared> f = attendance.Attendance_EventDeclared;
            //if (参数条件) f = f.Where(ff=>ff.参数字段==参数值);
            if (recordId > 0)
                f = f.Where(fw => fw.recordID == recordId);
            if (isBegin.HasValue)
                f = f.Where(fw => fw.isBeginTime == isBegin);
            r = f;
            return r;
        }
    }

    public class AttendanceVRecordListBLL
    {
        private AppDataAttendanceEntities attendance = new AppDataAttendanceEntities();
        private DALHWATTEmployee dalEmployee = new DALHWATTEmployee();
        public IEnumerable<v_Attendance> filter(int? year,int month,string emp)
        {
            IEnumerable<v_Attendance> r;
            IQueryable<v_Attendance> f = attendance.v_Attendance;
            //if (参数条件) f = f.Where(ff=>ff.参数字段==参数值);
            if (!string.IsNullOrEmpty(emp))
            {
                int empid;
                if (!int.TryParse(emp, out empid))
                    empid = dalEmployee.getEID(emp);
                f = f.Where(fw => fw.EmployeeID == empid);
            }
            if (year.HasValue) f = f.Where(fw => fw.sDate.Value.Year == year.Value);
            else f = f.Where(fw => fw.sDate.Value.Year == DateTime.Now.Year);
            if (1 <= month && month <= 12)
                f = f.Where(fw=>fw.sDate.Value.Month == month);
            r = f;
            return r;
        }
        public IEnumerable<v_Attendance> getRecordListWithEmpMonth(int? year,int month, string emp = null)
        {
            if (1 <= month && month <= 12 && !string.IsNullOrEmpty(emp))
            {
                var f = filter(year, month, emp);
                foreach (v_Attendance a in f)
                {
                    var b = a;
                    RefEvent(ref b);
                }
                return f;
            }
            else return null;
        }
        public void RefEvent(ref v_Attendance record ) 
        {
            AttendanceVEventListBLL attEventBll = new AttendanceVEventListBLL();
            record.EventList = attEventBll.getEvent(record.autoid).ToList();
        }
    }
    public class AttendanceVEventListBLL
    {
        private AppDataAttendanceEntities attendance = new AppDataAttendanceEntities();

        public IEnumerable<vAttendanceEventModel> filter(int? id = -1, string isBeginTime = null ,string empid = null,int Year = -1,int Month = -1,bool? hasOver = null)
        {
            IEnumerable<vAttendanceEventModel> r;
            IQueryable<vAttendanceEventModel> f = attendance.v_AttendanceEvent;
            //if (参数条件) f = f.Where(ff=>ff.参数字段==参数值);
            if (id.HasValue && id >-1 )
                f = f.Where(fw=>fw.recordID == id);
            bool isbt;
            if (!string.IsNullOrEmpty(isBeginTime) && bool.TryParse(isBeginTime, out isbt))
                f = f.Where(fw => fw.isBeginTime == isbt);
            if (!string.IsNullOrEmpty(empid))
            {
                int eid;if (int.TryParse(empid,out eid))
                f = f.Where(fw => fw.EmployeeID == eid);
            }
            if (Year >0)
            { f=f.Where(fw=>fw.sDate.Value.Year == Year);}
            if (Month >0)
            { f=f.Where(fw=>fw.sDate.Value.Month == Month);}
            if (hasOver.HasValue )
                if (hasOver.Value)//考勤类型:加班,缺勤
                { f = f.Where(fw => (fw.isBeginTime && fw.bOffset > 0) || (!fw.isBeginTime && fw.eOffset > 0)); }
                else
                { f = f.Where(fw => (fw.isBeginTime && fw.bOffset < 0) || (!fw.isBeginTime && fw.eOffset < 0)); }
            var t =f.ToList();
            r = f;
            return r;
        }
        public IEnumerable<vAttendanceEventModel> getEvent(int recordId)
        {
            if (recordId > -1 )
                return filter(recordId);
            else return null;
        }
    }

    public class AttendanceFeeCalculatorBLL {
        public AttendanceFeeCalculatorBLL() {
            Attendance_FeeCalculator.initialize();
        }
        //计算加班费
        //加班费规则:一天一次，0>时长>=30 5元/次；30>时长>=60 10元/次；60>时长 15元/次；周日20元/次；
        private AppDataAttendanceEntities attendance = new AppDataAttendanceEntities();
        public IEnumerable<Attendance_FeeCalculator> filter(int classId = -1) {
            var f = attendance.Attendance_FeeCalculator.AsQueryable();
            if (classId > 0)
            f = f.Where(w => w.classId == classId);
            var r = f;
            return r.AsEnumerable();
        }
        public int add(Attendance_FeeCalculator afc) {
            int r = -1;
            attendance.Attendance_FeeCalculator.Add(afc);
            r = attendance.SaveChanges();
            return r;
        }
        public int update(Attendance_FeeCalculator  afc) {
            int r = -1;
            var upd = attendance.Attendance_FeeCalculator.First(afcf => afcf.id == afc.id);
            upd.classId = afc.classId;
            upd.dateEnum = afc.dateEnum;
            upd.Unit = afc.Unit;
            upd.UnitFee = afc.UnitFee;
            upd.MaxFee = afc.MaxFee;

            r = attendance.SaveChanges();
            return r;
        }
        public int delete(Attendance_FeeCalculator afc)
        {
            int r = -1;
            var d = attendance.Attendance_FeeCalculator.Single(s => s.id == afc.id);
            attendance.Attendance_FeeCalculator.Remove(d);
            r = attendance.SaveChanges();
            return r;
        }
        public decimal fee(RecordModel ar,bool isBeginTime) {
            decimal r = 0;
            AttendanceHolidayBLL ahBll = new AttendanceHolidayBLL();
            AttendanceClassPlanBLL acpBll = new AttendanceClassPlanBLL();
            Attendance_FeeCalculator feeCalculator = new Attendance_FeeCalculator();
            var fees = attendance.Attendance_FeeCalculator.Where(w=>w.classId == ar.classID);
            //判断日期是Weekend,Holiday,Workday;
            //time>0为Over,time<0为Absence;
            switch((Attendance_FeeCalculator.dayState)Enum.Parse(typeof(Attendance_FeeCalculator.dayState),ar.dateType))
            {
                case Attendance_FeeCalculator.dayState.Holiday:
                    feeCalculator = fees.Where(fw=>fw.dateEnum =="Holiday").First();
                    if (isBeginTime) r = 0;
                    else
                    {
                        r = (decimal)Math.Ceiling(ar.eOffset.Value / (feeCalculator.Unit.Value + 0.00));
                        r *= feeCalculator.UnitFee.Value;
                        r = (r > feeCalculator.MaxFee.Value) ? feeCalculator.MaxFee.Value : r;
                    }             
                    break;
                case Attendance_FeeCalculator.dayState.DayOff:
                    feeCalculator = fees.Where(fw => fw.dateEnum == "DayOff").First();
                    if (isBeginTime) r = 0;
                    else
                    {
                        r = (decimal)Math.Ceiling(ar.eOffset.Value / (feeCalculator.Unit.Value + 0.00));
                        r *= feeCalculator.UnitFee.Value;
                        r = (r > feeCalculator.MaxFee.Value) ? feeCalculator.MaxFee.Value : r;
                    }
                    break;
                default:
                    if (isBeginTime)
                    { //上班
                        if (ar.bOffset > 0)//加班
                        {
                            var overs = fees.Where(fw => fw.dateEnum == "Over");
                            var over = (overs.Count() > 0) ? overs.First() : null;
                            feeCalculator = over;
                        }
                        else //缺勤
                        {
                            if (ar.bOffset > -300)//1440=60分钟*5小时,一天缺勤分钟最大值
                            {
                                var Absences = fees.Where(fw => fw.dateEnum == "Absence");
                                var Absence = (Absences.Count() > 0) ? Absences.First() : null;
                                feeCalculator = Absence;
                            }
                            else //请假
                            {
                                var Leaves = fees.Where(fw => fw.dateEnum == "Leave");
                                var Leave = (Leaves.Count() > 0) ? Leaves.First() : null;
                                feeCalculator = Leave;
                            }
                        }
                        if (feeCalculator != null)
                        {
                            r = (decimal)Math.Ceiling(ar.bOffset.Value / (feeCalculator.Unit.Value + 0.0));
                            r *= feeCalculator.UnitFee.Value;
                            if (r > 0)
                                r = (r > feeCalculator.MaxFee.Value) ? feeCalculator.MaxFee.Value : r;
                            else
                                r = (r > feeCalculator.MaxFee.Value) ? r : feeCalculator.MaxFee.Value;
                        }
                        else r = 0;
                    }
                    else
                    { //下班
                        if (ar.eOffset > 0)//加班
                        {
                            var overs = fees.Where(fw => fw.dateEnum == "Over");
                            var over = (overs.Count() > 0) ? overs.First() : null;
                            feeCalculator = over;
                        }
                        else //缺勤
                        {
                            if (ar.eOffset > -300)//1440=60分钟*5小时,一天缺勤分钟最大值
                            {
                                var Absences = fees.Where(fw => fw.dateEnum == "Absence");
                                var Absence = (Absences.Count() > 0) ? Absences.First() : null;
                                feeCalculator = Absence;
                            }
                            else //请假
                            {
                                var Leaves = fees.Where(fw => fw.dateEnum == "Leave");
                                var Leave = (Leaves.Count() > 0) ? Leaves.First() : null;
                                feeCalculator = Leave;
                            }
                        }
                        if (feeCalculator != null)
                        {
                            r = (decimal)Math.Ceiling(ar.eOffset.Value / (feeCalculator.Unit.Value + 0.00));
                            r *= feeCalculator.UnitFee.Value;
                            if (r > 0)
                                r = (r > feeCalculator.MaxFee.Value) ? feeCalculator.MaxFee.Value : r;
                            else
                                r = (r > feeCalculator.MaxFee.Value) ? r : feeCalculator.MaxFee.Value;
                        }
                        else r = 0;
                    }
                    break;
                    
            }
            return r;
        }
    }

    public class prevAttendanceBLL
    {
        private static DALHWATTCard dalCard = new DALHWATTCard();
        private static DALHWATTEmployee dalEmployee = new DALHWATTEmployee();
        /// <summary>
        /// 获得差异时间Ticks
        /// </summary>
        /// <param name="_b">开始时间</param>
        /// <param name="_e">结束时间</param>
        /// <returns></returns>
        private static int difTime(DateTime _b, DateTime _e)
        {
            int r;
            r = (int)(Math.Floor(_e.TimeOfDay.TotalMinutes) - Math.Floor(_b.TimeOfDay.TotalMinutes));
            return r;
        }
        /// <summary>
        /// 获取考勤系统中的考勤时间
        /// </summary>
        /// <param name="isBeginTime">上班:true;下班:false</param>
        /// <param name="_empName">员工姓名</param>
        /// <param name="_std">标准时间,为取得日期</param>
        /// <returns>考期时间</returns>
        private static DateTime GetKQTime(bool isBeginTime, string _empName, DateTime _std)
        {
            DateTime r;
            r = dalCard.getKQTime(isBeginTime, _empName, _std);
            return r;
        }
        private static DateTime GetKQTime(bool isBeginTime, int _empID, DateTime _std)
        {
            DateTime r;
            r = dalCard.getKQTime(isBeginTime, _empID, _std);
            return r;
        }
        /// <summary>
        /// 获取考勤系统的员工号
        /// </summary>
        /// <param name="_name"></param>
        /// <returns></returns>
        private static int? getKQEmploreeID(string _name)
        {
            int i;
            i = dalEmployee.getEID(_name);
            return i;
        }
        
        /// <summary>
        /// 预处理,如果指定员工，处理指定员工考勤记录；否则处理所有员工考勤记录
        /// </summary>
        /// <param name="empName"></param>
        public int prepRecord(string empName = null, string Operator = "system")
        {
            int r = 0;
            if (!string.IsNullOrEmpty(empName))//如果指定员工，处理指定员工考勤记录；否则处理所有员工考勤记录
            { r = DoRecords(empName, Operator); }
            else
            {//否则处理所有员工考勤记录
                foreach (KQZ_Employee e in dalEmployee.Employees())
                    r += DoRecords(e.EmployeeName, Operator);
            }
            return r;
        }
        public int DoRecords(string empName,string Operator)
        {
            #region 定义变量
            AttendanceRecordsBLL attRecordsBll = new AttendanceRecordsBLL();
            RecordModel lstRecord, onceRecord;
            AttendanceSchemeClassBLL attSchemeClassBll = new AttendanceSchemeClassBLL();
            AttendanceEmployeeRefSchemeClassBLL attEmpRefSchClassBll = new AttendanceEmployeeRefSchemeClassBLL();
            if (attEmpRefSchClassBll.filter(dalEmployee.getEID(empName)).Count() <= 0) return 0;
            Attendance_EmployeeRefSchemeClass attCurrERSC = attEmpRefSchClassBll.getInstanceEmpID(dalEmployee.getEID(empName));
            Attendance_SchemeClass prevSchemeClass;
            Attendance_SchemeClass currSchemeClass;
            #endregion
            int r = 0;

            SystemBLL.LockBLL lockBll = new SystemBLL.LockBLL();
            SystemDB.Lock dblock = new SystemDB.Lock() { module = "Attendance", key = "DoRecords", id = empName, locker = Operator };
            if (lockBll.isExistLock(dblock))
                return -1;
            dblock.autoId = lockBll.Locking(dblock);//加锁
            DateTime attEndDay = dalCard.getLastDate(empName);
            DateTime lastRecordDate = DateTime.MinValue;
            #region 取最后一次考勤记录
            lstRecord = attRecordsBll.getLastRecord(empName);
            if (lstRecord == null)
            {//最后一次考勤记录为空,此员工为新入职员工
                prevSchemeClass = null;
                currSchemeClass = attSchemeClassBll.single(attCurrERSC.classID.Value);
                lastRecordDate = new DateTime(2014, 4, 1);//系统启用日期
                if (lastRecordDate < attCurrERSC.EffDate) lastRecordDate = attCurrERSC.EffDate.Value;
            }
            else
            {
                prevSchemeClass = attSchemeClassBll.single(lstRecord.classID.Value);
                currSchemeClass = attSchemeClassBll.single(attCurrERSC.classID.Value);

                lastRecordDate = (lstRecord.sDate.HasValue) ?
                    new DateTime(lstRecord.sDate.Value.Year, lstRecord.sDate.Value.Month, lstRecord.sDate.Value.Day)
                    : new DateTime(2014, 4, 1);
            }
            #endregion
            if (currSchemeClass != null && prevSchemeClass != null && prevSchemeClass.classId == currSchemeClass.classId)
            {
                #region 最后一次考勤记录的计划号与当前配置的计划号一致

                //更新最后一天的考勤
                onceRecord = attRecordsBll.single(lstRecord.autoid);
                DoOnce(ref onceRecord, lstRecord.EmployeeID.Value, lastRecordDate);
                r += attRecordsBll.update(onceRecord);

                lastRecordDate = lastRecordDate.AddDays(1);
                while (lastRecordDate < attEndDay)
                {
                    onceRecord = new RecordModel()
                    {
                        EmployeeID = lstRecord.EmployeeID.Value,
                        sDate = lastRecordDate,
                        classID = currSchemeClass.classId
                    };

                    DoOnce(ref onceRecord, onceRecord.EmployeeID.Value, lastRecordDate);

                    r += attRecordsBll.Add(onceRecord);
                    lastRecordDate = lastRecordDate.AddDays(1);
                }
                #endregion
            }
            else
            {
                #region 最后一次考勤记录的计划号与当前配置的计划号不致,考勤设置更新了.

                if (lstRecord != null)
                {
                    #region 最后一次考勤记录不为空
                    if (lstRecord.sDate.HasValue)
                    {
                        if (lstRecord.sDate.Value > attCurrERSC.EffDate.Value)
                        {
                            #region 如果最后考勤日期I>当前方案计划日期L，则更新当前方案计划日期之前的考勤记录，且添加当前方案计划日期之后的考勤记录，
                            //更新当前方案计划日期之前的考勤记录，
                            IEnumerable<RecordModel> RecordList = attRecordsBll.filter(s: attCurrERSC.EffDate.Value, e: lstRecord.sDate.Value, _emp: empName);
                            foreach (RecordModel one in RecordList)
                            {
                                var o = one;
                                o.periodNo = null;
                                o.classID = attCurrERSC.classID;
                                DoOnce(ref o, one.EmployeeID.Value, one.sDate.Value);
                                r += attRecordsBll.update(o);
                            }

                            //添加当前方案计划日期之后的考勤记录，
                            lastRecordDate = lstRecord.sDate.Value.AddDays(1);
                            while (lastRecordDate <= attEndDay)
                            {
                                onceRecord = new RecordModel()
                                {
                                    EmployeeID = lstRecord.EmployeeID.Value,
                                    sDate = lastRecordDate,
                                    classID = currSchemeClass.classId
                                };

                                DoOnce(ref onceRecord, onceRecord.EmployeeID.Value, lastRecordDate);

                                r += attRecordsBll.Add(onceRecord);
                                lastRecordDate = lastRecordDate.AddDays(1);
                            }
                            #endregion
                        }
                        else
                        {
                            #region 否则分三部分处理考勤记录，1更新最后一天的考勤，2新增最新员工方案计划修改前日期的考勤记录，3.新增最新员工方案计划修改后日期的考勤记录
                            //1更新最后一天的考勤
                            onceRecord = attRecordsBll.single(lstRecord.autoid);
                            DoOnce(ref onceRecord, lstRecord.EmployeeID.Value, lastRecordDate);
                            r += attRecordsBll.update(onceRecord);

                            //2新增[最新员工方案计划]修改前日期的考勤记录
                            lastRecordDate = lastRecordDate.AddDays(1);
                            while (lastRecordDate < attCurrERSC.EffDate.Value)
                            {
                                onceRecord = new RecordModel()
                                {
                                    EmployeeID = lstRecord.EmployeeID.Value,
                                    sDate = lastRecordDate,
                                    classID = lstRecord.classID
                                    //periodNo = attCurrERSC.periodNo
                                };

                                DoOnce(ref onceRecord, onceRecord.EmployeeID.Value, lastRecordDate);

                                r += attRecordsBll.Add(onceRecord);
                                lastRecordDate = lastRecordDate.AddDays(1);
                            }
                            //3新增[最新员工方案计划]修改后日期的考勤记录
                            while (lastRecordDate <= attEndDay)
                            {
                                onceRecord = new RecordModel()
                                {
                                    EmployeeID = lstRecord.EmployeeID.Value,
                                    sDate = lastRecordDate,
                                    classID = currSchemeClass.classId
                                };

                                DoOnce(ref onceRecord, onceRecord.EmployeeID.Value, lastRecordDate);

                                r += attRecordsBll.Add(onceRecord);
                                lastRecordDate = lastRecordDate.AddDays(1);
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 新员工,无最后一次考勤记录
                    //i = i.AddDays(1);
                    while (lastRecordDate < attEndDay)
                    {
                        onceRecord = new RecordModel()
                        {
                            EmployeeID = dalEmployee.getEID(empName),
                            sDate = lastRecordDate,
                            classID = currSchemeClass.classId
                        };
                        //多线程加锁
                        DoOnce(ref onceRecord, onceRecord.EmployeeID.Value, lastRecordDate);

                        r += attRecordsBll.Add(onceRecord);
                        lastRecordDate = lastRecordDate.AddDays(1);
                    }
                    #endregion
                }
                #endregion
            }

            lockBll.UnLocking(dblock);//解锁
            return r;
        }

        public int doDay(int empid, DateTime doDate, string Operator)
        {
            #region 定义变量
            AttendanceRecordsBLL attRecordsBll = new AttendanceRecordsBLL();
            RecordModel  onceRecord;
            AttendanceEmployeeRefSchemeClassBLL attEmpRefSchClassBll = new AttendanceEmployeeRefSchemeClassBLL();
            int r = 0;
            #endregion

            SystemBLL.LockBLL lockBll = new SystemBLL.LockBLL();
            SystemDB.Lock dblock = new SystemDB.Lock() { module = "Attendance", key = "DoRecords", id = empid.ToString(), locker = Operator };
            if (lockBll.isExistLock(dblock))
                return -1;
            dblock.autoId = lockBll.Locking(dblock);//加锁
            lock (dblock)
            {
                #region 取指定日期员工的考勤记录
                //员工指定日期考勤记录
                var qr = attRecordsBll.filter(doDate, doDate, empid.ToString());
                if (qr.DefaultIfEmpty(null) == null) onceRecord = null;
                else onceRecord = qr.FirstOrDefault();
                #endregion

                if (onceRecord != null)
                {
                    onceRecord.classID
                        = attEmpRefSchClassBll.getClassId(onceRecord.EmployeeID.Value, onceRecord.sDate.Value);
                }
                else
                {
                    onceRecord = new RecordModel()
                    {
                        EmployeeID = empid,
                        sDate = doDate,
                        classID = attEmpRefSchClassBll.getClassId(empid, doDate)
                    };
                }

                DoOnce(ref onceRecord, empid, doDate);
                r = attRecordsBll.Save(onceRecord);
            }
            lockBll.UnLocking(dblock);//解锁
            return r;
 
        }
        /// <summary>
        /// 处理指定日期i的考勤记录
        /// </summary>
        /// <param name="once">当前记录</param>
        /// <param name="empID">员工号</param>
        /// <param name="i">指定日期</param>
        private void DoOnce(ref RecordModel once, int empID, DateTime i)
        {
            AttendanceRecordsBLL attRecordsBll = new AttendanceRecordsBLL();
            AttendanceClassPlanBLL attClassPlanBll = new AttendanceClassPlanBLL();
            Attendance_ClassPlan acp = new Attendance_ClassPlan();
            DateTime attOnTime, attOffTime, sdOnTime, sdOffTime;//考勤机时间，标准时间
            string[] ssTime;//标准时间字符串
            int difMinutes;
            attOnTime = GetKQTime(true, empID, i);
            //其它项

            once.sDate = (once.sDate.HasValue) ? once.sDate : new DateTime(attOnTime.Ticks - attOnTime.TimeOfDay.Ticks + 1);
            once.periodNo = (once.periodNo.HasValue) ? once.periodNo : attRecordsBll.getPeriodNo(empID);
            once.EmployeeID = (once.EmployeeID.HasValue) ? once.EmployeeID : empID;

            acp = attClassPlanBll.filter(_classId: once.classID.Value, _period: once.periodNo.Value).First();
            //考期机项
            //上班
            attOnTime = GetKQTime(true, empID, i);
            ssTime = acp.bTime.Split(':');
            sdOnTime = new DateTime(i.Year, i.Month, i.Day, int.Parse(ssTime[0]), int.Parse(ssTime[1]), 0);
            once.bAttTimeStr = attOnTime.ToShortTimeString();

            difMinutes = (Math.Floor(attOnTime.TimeOfDay.TotalSeconds)==0)?-9999:difTime(attOnTime, sdOnTime);
            once.bOffset = difMinutes;
            //下班
            attOffTime = GetKQTime(false, empID, i);
            ssTime = acp.eTime.Split(':');
            sdOffTime = new DateTime(i.Year, i.Month, i.Day, int.Parse(ssTime[0]), int.Parse(ssTime[1]), 0);
            once.eAttTimeStr = attOffTime.ToShortTimeString();
            difMinutes = difTime(sdOffTime, attOffTime);
            once.eOffset = (Math.Floor(attOnTime.TimeOfDay.TotalSeconds) == 0) ? -9999 : difMinutes;
            //节假日和周末
            bool isHoliday = IsHoliday(once);
            if (isHoliday || acp.bTime == acp.eTime)
            {
                once.bOffset = isHoliday ? int.MaxValue : int.MinValue;//Min-周末:Max-节日
                once.eOffset = (int)Math.Ceiling(new DateTime(attOffTime.Ticks - attOnTime.Ticks).TimeOfDay.TotalMinutes);
            }

            //工作日期类型
            if (isHoliday) once.dateType = "Holiday";
            else if (acp.bTime == acp.eTime) once.dateType = "DayOff";
            else once.dateType = "WorkDay";

            //调班
            var acps = attClassPlanBll.filter(_classId: once.classID.Value, sdate: once.sDate.Value);
            if (acps.Count() > 0)
            {
                once.dateType = "WorkDay";
                acp = acps.First();
                ssTime = acp.bTime.Split(':');
                sdOnTime = new DateTime(i.Year, i.Month, i.Day, int.Parse(ssTime[0]), int.Parse(ssTime[1]), 0);
                difMinutes = difTime(attOnTime, sdOnTime);
                once.bOffset = difMinutes;
                
                ssTime = acp.eTime.Split(':');
                sdOffTime = new DateTime(i.Year, i.Month, i.Day, int.Parse(ssTime[0]), int.Parse(ssTime[1]), 0);
                difMinutes = difTime(sdOffTime, attOffTime);
                once.eOffset = difMinutes;
            }

            //考勤费用
            AttendanceFeeCalculatorBLL afcBll = new AttendanceFeeCalculatorBLL();
            once.bOffsetFee = afcBll.fee(once, true);
            once.eOffsetFee = afcBll.fee(once, false);
        }
        /// <summary>
        /// 是否为法定节假日
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        public bool IsHoliday(RecordModel ar)
        {
            bool r = true ;
            AttendanceHolidayBLL ahBll = new AttendanceHolidayBLL();
            AttendanceClassPlanBLL acpBll = new AttendanceClassPlanBLL();
            r &= ahBll.isHoliday(ar.sDate.Value);
            //Attendance_ClassPlan acp = acpBll.filter(_classId: ar.classID.Value, _period: ar.periodNo.Value).First();
            //r |= (acp.bTime == acp.eTime);
            return r;
        }

        public void AddAbsence(Attendance_EventDeclared absence)
        {
            AttendanceEventDeclaredBLL aedBll = new AttendanceEventDeclaredBLL();
            aedBll.Add(absence);
        }
        public void AddAbsenceWithMonth(string empId, DateTime datetimeMonth, string Operator)
        {
            SystemBLL.LockBLL lockBll = new SystemBLL.LockBLL();
            SystemDB.Lock dblock = new SystemDB.Lock() { module = "Attendance", key = "Absence",id = empId,locker = Operator };
            if (lockBll.isExistLock(dblock))
                return;
            dblock.autoId = lockBll.Locking(dblock);//加锁
            DateTime s, e;
            s = new DateTime(datetimeMonth.Year, datetimeMonth.Month, 1);
            e = s.AddMonths(1).AddDays(-1);
            if (DateTime.Now.DayOfYear <= e.DayOfYear) e = DateTime.Now.AddDays(-1);
            AttendanceRecordsBLL attRecordsBll = new AttendanceRecordsBLL();
            IEnumerable<RecordModel> attList;
            attList = attRecordsBll.filter(s, e, _emp:empId);
            Attendance_EventDeclared absence = new Attendance_EventDeclared();
            AttendanceEventDeclaredBLL aedBll = new AttendanceEventDeclaredBLL();
            IEnumerable<Attendance_EventDeclared> aedFilterResult = null;
            foreach (RecordModel ar in attList)
            {
                if (ar.bOffset == -9999 && ar.bOffset == ar.eOffset)
                {
                    aedFilterResult = aedBll.filter(recordId: ar.autoid, isBegin: true);
                    if (aedFilterResult == null || aedFilterResult.Count() <= 0)
                    {
                        absence = new Attendance_EventDeclared()
                        {
                            isBeginTime = true,
                            recordID = ar.autoid,
                        };
                        AddAbsence(absence);
                    }
                    aedFilterResult = null;
                    continue;
                }

                if (0 > ar.bOffset && ar.bOffset > int.MinValue)
                {
                    aedFilterResult = aedBll.filter(recordId: ar.autoid, isBegin: true);
                    if (aedFilterResult == null || aedFilterResult.Count() <= 0)
                    {
                        absence = new Attendance_EventDeclared()
                        {
                            isBeginTime = true,
                            recordID = ar.autoid,
                        };
                        AddAbsence(absence);
                    }
                    aedFilterResult = null;
                }
                if (0 > ar.eOffset && ar.eOffset > int.MinValue)
                {
                    aedFilterResult = aedBll.filter(recordId: ar.autoid, isBegin: false);
                    if (aedFilterResult == null || aedFilterResult.Count() <= 0)
                    {
                        absence = new Attendance_EventDeclared()
                            {
                                isBeginTime = false,
                                recordID = ar.autoid,
                            };
                        AddAbsence(absence);
                    }
                    aedFilterResult = null;
                }
            }
            lockBll.UnLocking(dblock);//解锁
        }
    }
}
