using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Attendance.Model;
using Attendance.DAL;
using IAttendanceDevice;
using AttendanceDeviceFactory;
using Logs;
using Locks;

namespace Attendance
{
    public class AttendanceBLL
    {
        public IAttendanceDevice.IAttendanceDevice device { get; private set; }
            
        #region 获得计算所需数据设置
        SchemeModel currentScheme = null;
        List<EmployeeRefSchemeClassModel> empRefClasss = null;
        List<ClassPlanModel> currClassPlanModels = null;
        List<ICardTime> cardTimes = null;
        EmployeeRefSchemeClassModel currentERSC = null;
        List<FeeCalculatorModel> feeCalcs = null;
        UserBLL.userBLL uBll = new UserBLL.userBLL();
        UserInfo.UserModel currentUser = new UserInfo.UserModel();
        #endregion
        Logs.LogsDataAccess log;
        public AttendanceBLL(long userID) {
            device = AttendanceDeviceFactory.AttendanceDeviceFactory.Create();
            log = new Logs.AppLogsDataAccess("Attendance_Logs");
            currentUser = uBll.single(userID);
        }
       
        private EventDeclaredModel readyAbsence(RecordModel record)
        {   
            EventDeclaredModel r = new EventDeclaredModel();
            r.recordID = record.autoid;
            if (record.bOffset < 0) { 
                r.isBeginTime = true;
            r.FeeOther = record.bOffsetFee; }
            else
            {
                r.isBeginTime = false;
                r.FeeOther = record.eOffsetFee;
            }
            return r;
        }
        public void saveAbsenceWithMonth(int eid, DateTime datetimeMonth)
        {
            log.createLog(new LogModel()
            {
                cModule = "Attendance",
                cParams = "eid:" + eid.ToString() + ", Month:" + datetimeMonth.ToShortDateString() + "; ",
                cClass = "AttendanceBLL",
                cFunction = "saveAbsenceWithMonth",
                iUserID = currentUser.iUserId,
                cUserName = currentUser.cUserName
            });
            DateTime day = datetimeMonth.AddDays(1 - datetimeMonth.Day);
            DateTime endDay = datetimeMonth.AddMonths(1).AddDays(-1 * datetimeMonth.Day);
            EventDeclaredModel eventAbsence = null;
                RecordDAL rDal = new RecordDAL();
            List<RecordModel> records = rDal.search(eid, day, endDay);
            var recordAbsences = records.FindAll(f =>( f.bOffset < 0 || f.eOffset < 0) && f.dayType == ClassPlanModel.dayState.WorkDay);
            //recordAbsences缺勤记录，eventAbsences已存在的缺勤事件
            EventDeclaredDAL edDal = new EventDeclaredDAL();
            var eventAbsences = edDal.listWithRecordIDS(recordAbsences.Select(s => s.autoid).ToArray());
            if (eventAbsences != null && eventAbsences.Count > 0)
            recordAbsences.RemoveAll(r => eventAbsences.Exists(e => e.recordID == r.autoid));
            //recordAbsences无缺勤事件的缺勤记录
            foreach (RecordModel record in recordAbsences)
            {
                eventAbsence = readyAbsence(record);
                saveAbsence(eventAbsence);
                day.AddDays(1);
            }
        }
        public void saveAbsence(EventDeclaredModel absence)
        {
            EventDeclaredDAL edDal = new EventDeclaredDAL();
            if (absence.autoID > 0)
                edDal.Update(absence);
            else edDal.Create(absence);
        }
        /// <summary>
        /// 获得差异时间Ticks,返回单位分钟
        /// </summary>
        /// <param name="_b">开始时间</param>
        /// <param name="_e">结束时间</param>
        /// <returns></returns>
        internal static int difTime(DateTime _b, DateTime _e)
        {
            int r;
            r = (int)(Math.Floor(_e.TimeOfDay.TotalMinutes) - Math.Floor(_b.TimeOfDay.TotalMinutes));
            return r;
        }
        private FeeCalculatorModel getFeeCalc(List<FeeCalculatorModel> feeCalcs,string dateType, int classid)
        {
            FeeCalculatorModel.dayState dayState = (FeeCalculatorModel.dayState)Enum.Parse(typeof(FeeCalculatorModel.dayState), dateType);
            FeeCalculatorModel feeCalc = null;
            if (feeCalcs != null && feeCalcs.Count > 0)
                feeCalc = feeCalcs.FindAll(f => f.DatState == dayState).First();
            return feeCalc;
        }

        private int getClassPeriodNo(DateTime dt)
        {
            int pNo = -1;
            var span = dt.Subtract(currentERSC.EffDate.Value);
            var ps = currentScheme.periods.Value;
            pNo = (span.Days + currentERSC.periodNo ?? 0) % ps;
            return pNo;
        }
        public HolidayModel getHoliday(DateTime dt)
        { 
            HolidayModel r = new HolidayModel();
            HolidayDAL holidayDal = new HolidayDAL();
            var holidays = holidayDal.selects(new HolidayModel() { sDate = dt });
            if (holidays != null && holidays.Count > 0)
                r = holidays.First(); 
            else
                r = null;
            return r;
        }
        /// <summary>
        /// 读取指定员工班次列表，取从begin到end间的列表
        /// </summary>
        /// <param name="eid"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private List<EmployeeRefSchemeClassModel> getEmpRefSchemeClasss(int eid, DateTime begin, DateTime end)
        {//读取指定员工班次列表，取从begin到end间的列表
            List<EmployeeRefSchemeClassModel> r = null;
            EmployeeRefSchemeClassDAL erscDal = new EmployeeRefSchemeClassDAL();
            r = erscDal.selects(new EmployeeRefSchemeClassModel() { EmployeeID = eid });
            EmployeeRefSchemeClassModel b = null, e = null;
            int bidx = -1, eidx = -1;
            if (r != null && r.Count > 0)
            {
                if (r.Exists(exist=>exist.EffDate > begin))
                {
                    b = r.OrderBy(o => o.EffDate).First(f => f.EffDate > begin);
                    bidx = r.IndexOf(b);
                    if (bidx < 1) bidx = 0; else bidx--;
                }else { bidx = r.Count-1; }

                if (r.Exists(exist => exist.EffDate < end))
                {
                    e = r.OrderByDescending(o => o.EffDate).First(f => f.EffDate < end);
                    eidx = r.IndexOf(e);
                }else { throw new Exception("未设置考勤班次！"); }
                r = r.GetRange(bidx, eidx - bidx + 1);
                return r.OrderBy(o => o.EffDate).ToList();
            }
            else return null;
        }
        private ClassPlanModel.dayState getDateType(DateTime dt,out string dateType)
        {
            int period = getClassPeriodNo(dt);
            dateType = string.Empty;
            ClassPlanModel.dayState daystate;
            //假日
            var holiday = getHoliday(dt);
            if (holiday != null)
            {
                //是假日
                dateType = holiday.sName;
                daystate = ClassPlanModel.dayState.Holiday;
            }
            else
            {
                var currClassPlan = currClassPlanModels.First(f => f.periodNo == period);
                //不是假日
                if (currClassPlan.bTime.Equals(currClassPlan.eTime))
                { daystate = ClassPlanModel.dayState.DayOff;
                    dateType = daystate.ToString(); }
                else
                { daystate = ClassPlanModel.dayState.WorkDay;
                    dateType = daystate.ToString(); }
            }
            return daystate;
        }
        public int DoAtte(int eid, DateTime begin, DateTime end)
        {
            long lockid = -1;
            log.createLog(new LogModel()
            {
                cModule = "Attendance",
                cParams = "eid:" + eid.ToString() + ", Begin:" + begin.ToShortDateString()
                        + ", End:" + end.ToShortDateString() + "; ",
                cClass = "AttendanceBLL",
                cFunction="DoAtte", iUserID = currentUser.iUserId, cUserName=currentUser.cUserName
            });
            LockBLL lockBll = new LockBLL();
            var isLock = lockBll.isExistLock(new LockModel() { module = "Attendance", key = "DoAtte" });
            if (isLock) throw new Exception("考勤数据导入，独占操作中！请稍后再试。");
            else lockid = lockBll.Locking(module: "Attendance", key: "DoAtte", id: currentUser.iUserId.ToString(), locker: currentUser.cUserName);
            //获得指定日期内的考勤设置和设备记录
            RecordModel record = null;
            List<ICardTime> cardTimeDays = null;
            end = (end > DateTime.Now) ? DateTime.Now : end;
            AtteDateInit(eid, begin, end);
            if (empRefClasss.First().EffDate > begin)
                begin = empRefClasss.First().EffDate.Value;
            empRefClasss = empRefClasss.OrderByDescending(o => o.EffDate).ToList();
            FeeCalculatorDAL fcDal = new FeeCalculatorDAL();
            DateTime b = begin;
            int r = (int)Math.Floor(end.Subtract(begin).TotalDays);
            while (b <= end)
            {
                var ersc = empRefClasss.First(f => b >= f.EffDate);
                if (ersc != currentERSC)
                {
                    currentERSC = ersc;
                    currentScheme = currentERSC.SchemeClass.Scheme;
                    currClassPlanModels = currentERSC.SchemeClass.ClassPlans.ToList();
                    feeCalcs = fcDal.selects(new FeeCalculatorModel() { classId = currentERSC.classID });
                }
                if (cardTimes.Exists(e => e.cardTime.Date == b.Date))
                    cardTimeDays = cardTimes.FindAll(f => f.cardTime.Date == b.Date);
                else cardTimeDays = null;
                record = readyRecord(eid, b,cardTimeDays);
                save(record);

                b = b.AddDays(1);
            }
            lockBll.UnLocking(lockid);
            return r;
        }
        private void save(RecordModel record)
        {
            RecordDAL rDal = new RecordDAL();
            if (record.autoid > 0)
                rDal.Update(record);
            else rDal.Create(record);
        }
        private RecordModel readyRecord(int eid, DateTime dt, List<ICardTime> cardTimes)
        {
            RecordModel r = null;
            RecordDAL rDal = new RecordDAL();
            var rsels = rDal.selects(new RecordModel() { EmployeeID = eid, sDate = dt });
            if (rsels != null && rsels.Count > 0) r = rsels.First();
            else { r = null; }
            if (r == null) r = new RecordModel() { autoid =-1, EmployeeID = eid, sDate = dt, classID = currentERSC.classID };
            r.classID = (r.classID != currentERSC.classID)? currentERSC.classID : r.classID;
            r.periodNo = getClassPeriodNo(dt);
            ClassPlanModel cpModel = null;
            if (currClassPlanModels.Exists(e => e.sdate == dt))
                cpModel = currClassPlanModels.First(f => f.sdate == dt);
            if (cpModel == null)
                cpModel = currClassPlanModels.First(f => f.periodNo == r.periodNo);
            string dateType = string.Empty;
            r.dayType = getDateType(dt,out dateType);
            r.dayName = dateType;
            //FeeCalculatorModel feeCalc = getFeeCalc(feeCalcs,r.dayName, r.classID.Value);
            //periodNo,dateType,EmployeeID,sDate,classID,上面代码处理
            //bAttTimeStr,eAttTimeStr,bOffset,eOffset，下面代码处理
            r.calcAttrTime.calcAttrTime(r, cardTimes, cpModel, feeCalcs);

            return r;
        }
        private void AtteDateInit(int eid, DateTime begin, DateTime end)
        {
            //清理数据
            empRefClasss = null;
            currentScheme = null;
            currClassPlanModels = null;
            cardTimes = null;
            currentERSC = null;
            feeCalcs = null;
            //更新数据
            //员工关联班次
            empRefClasss = getEmpRefSchemeClasss(eid, begin, end);
            cardTimes = device.cardBll.getCardTimeDays(eid, begin, end);
        }
        public List<RecordModel> getRecords(int eid, DateTime begin, DateTime end)
        {
            List<RecordModel> records = null;
            RecordDAL rDal = new RecordDAL();
            records = rDal.search(eid, begin, end);
            return records.OrderBy(o=>o.sDate).ToList();
        }

        #region ForUI
        public int autoFillAttr(int eid)
        {
            DateTime begin, end;
            RecordDAL rDal = new RecordDAL();
            var rs = rDal.selects(new RecordModel() { EmployeeID = eid }, 10, " order by sDate desc ");
            if (rs != null && rs.Count > 0)
                begin = rs.OrderByDescending(o => o.sDate).First().sDate.Value;
            else begin = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            end = DateTime.Now;
            return DoAtte(eid, begin, end);
        }
        public int getEmpID(string EmpName)
        {
            int eid = -1;
            EmployeeRefSchemeClassDAL erscDal = new EmployeeRefSchemeClassDAL();
            var emps = erscDal.selects(new EmployeeRefSchemeClassModel() { EmployeeName = EmpName });
            if (emps != null && emps.Count > 0)
                eid = emps.OrderByDescending(o => o.isVaild).First().EmployeeID.Value;
            else { throw new InvalidOperationException(EmpName + " - 无此员工！"); }
            return eid;
        }
        public List<EmployeeRefSchemeClassModel> getAttrEmps() {
            EmployeeRefSchemeClassDAL erscDal = new EmployeeRefSchemeClassDAL();
            return erscDal.selects(null);
        }
        #endregion
    }
}
