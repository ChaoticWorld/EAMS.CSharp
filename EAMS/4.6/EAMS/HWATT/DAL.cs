using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace HWATT
{
    public class DALHWATTCard
    {
        private static HWATTEntities hwatt = new HWATTEntities();

        #region KQZ_Card
        /// <summary>
        /// 指定考勤记录是否存在.
        /// </summary>
        /// <param name="_autoid">编号</param>
        /// <returns></returns>
        public bool Exists(int _autoid)
        {
            bool r = false;
            try
            {
                var c = hwatt.KQZ_Card.FirstOrDefault(s => s.CardID == _autoid);
                r = true;
            }
            catch { r = false; }
            return r;
        }
        /// <summary>
        /// 获得编码为autoid的考勤记录实例.存在:返回实例,不存在:返回null
        /// </summary>
        /// <param name="_autoID">编号</param>
        /// <returns></returns>
        public KQZ_Card getInstance(int _autoid)
        {
            KQZ_Card r = hwatt.KQZ_Card.FirstOrDefault(s => s.CardID == _autoid);
            return r;
        }

        /// <summary>
        /// 获得考勤原始数据
        /// </summary>
        /// <param name="isBeginWork">上班(真)还是下班</param>
        /// <param name="_empName">员工姓名</param>
        /// <param name="_std">日期</param>
        /// <returns></returns>
        public DateTime getKQTime(bool isBeginWork, string _empName, DateTime _std)
        {
            DateTime r;
            try
            {
                var kqts = from c in hwatt.KQZ_Card
                           join e in hwatt.KQZ_Employee on c.EmployeeID equals e.EmployeeID
                           where e.EmployeeName == _empName
                           && EntityFunctions.DiffDays(c.CardTime, _std) == 0
                           select c;
                if (isBeginWork)//上班
                    r = kqts.Min(t => t.CardTime);
                else //下班
                    r = kqts.Max(t => t.CardTime);
            }
            catch { r = new DateTime(_std.Year, _std.Month, _std.Day, 0, 0, 0); }
            return r;
        }
        public DateTime getKQTime(bool isBeginWork, int _empID, DateTime _std)
        {
            DateTime r;
            try{
            var kqts = from c in hwatt.KQZ_Card
                       where c.EmployeeID == _empID 
                       && EntityFunctions.DiffDays(c.CardTime, _std) == 0
                       select c;
            if (isBeginWork)//上班
                r = kqts.Min(t => t.CardTime);
            else //下班
                r = kqts.Max(t => t.CardTime);
            }
            catch { r = new DateTime(_std.Year, _std.Month, _std.Day, 0, 0, 0); }
            return r;
        }

        /// <summary>
        /// 获得指定员工的首次考勤日期时间
        /// </summary>
        /// <param name="_empName">员工姓名</param>
        /// <returns></returns>
        public static DateTime getFirstDateTime(string _empName)
        {
            DateTime r;
            try
            {
                var cs = from c in hwatt.KQZ_Card
                         join e in hwatt.KQZ_Employee on c.EmployeeID equals e.EmployeeID
                         where e.EmployeeName == _empName
                         select c.CardTime;
                r = cs.Min();
            }
            catch { r = new DateTime(1, 1, 1); }
            return r;
        }
        public DateTime getLastDate(string _empName = null,int _empid = -1)
        {
            DateTime r = DateTime.MinValue;
            try
            {
                if (!string.IsNullOrEmpty(_empName))
                {
                    var cs = from c in hwatt.KQZ_Card
                             join e in hwatt.KQZ_Employee on c.EmployeeID equals e.EmployeeID
                             where e.EmployeeName == _empName
                             select c.CardTime;
                    r = cs.Max();
                }
                if (_empid > 0)
                {
                    var cs = from c in hwatt.KQZ_Card
                             join e in hwatt.KQZ_Employee on c.EmployeeID equals e.EmployeeID
                             where e.EmployeeID == _empid
                             select c.CardTime;
                    r = cs.Max();
                }
            }
            catch { r = DateTime.MinValue; }
            return r;
        }
        #endregion KQZ_Card
    }

    public class DALHWATTEmployee
    {
        private static HWATTEntities hwatt = new HWATTEntities();

        #region KQZ_Employee
        /// <summary>
        /// 员工是否存在.
        /// </summary>
        /// <param name="_autoid">编号</param>
        /// <returns></returns>
        public bool Exists(int _autoid)
        {
            bool r = false;
            try
            {
                var e = hwatt.KQZ_Employee.FirstOrDefault(s => s.EmployeeID == _autoid);
                r = true;
            }
            catch { r = false; }
            return r;
        }
        /// <summary>
        /// 获得员工实例.存在:返回实例,不存在:返回null
        /// </summary>
        /// <param name="_autoID">员工编号EmployeeID</param>
        /// <returns></returns>
        public KQZ_Employee getInstance(int _autoid)
        {
            KQZ_Employee r = hwatt.KQZ_Employee.FirstOrDefault(s => s.EmployeeID == _autoid);
            return r;
        }
        /// <summary>
        /// 考勤员工列表
        /// </summary>
        /// <returns></returns>
        public List<KQZ_Employee> Employees()
        {
            List<KQZ_Employee> r;
            //BrchID == 3,考勤部门=3
            r = hwatt.KQZ_Employee.Where(ew=>ew.BrchID==3).Distinct().ToList();
            return r;
        }
        /// <summary>
        /// 考勤员工列表,
        /// </summary>
        /// <param name="ee"></param>
        /// <returns></returns>
        public List<KQZ_Employee> Employees(int [] ee)
        {
            List<KQZ_Employee> r = new List<KQZ_Employee>();
            //BrchID == 3,考勤部门=3
            var rl = hwatt.KQZ_Employee.Where(ew => ew.BrchID == 3).Distinct().ToList();
            foreach (var rr in rl)
                if (ee.Contains((int)rr.EmployeeID)) r.Add(rr);
            return r;
        }
        public List<int> EmpIds() 
        {
            List<int> r = new List<int>();
            //BrchID == 3,考勤部门=3
            var es = hwatt.KQZ_Employee.Where(w => w.BrchID == 3).Distinct();
            r = es.Select(s => (int)s.EmployeeID).ToList<int>();
            return r;
        }
        public int getEID(string _eName)
        {
            int r = -1;
            try
            {
                r = (int)hwatt.KQZ_Employee.FirstOrDefault(e => e.EmployeeName == _eName).EmployeeID;
            }
            catch { r = -1; }
            return r;
        }
        #endregion KQZ_Employee
    }

}
