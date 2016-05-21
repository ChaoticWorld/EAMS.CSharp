using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceDevice.HWATT.DAL;
using IAttendanceDevice;

namespace AttendanceDevice.HWATT
{
    public class BLL : IAttendanceDevice.IBLL
    {
        public BLL() { }
        CardTimeDAL ctDal = new CardTimeDAL();
        EmployeeDAL deDal = new EmployeeDAL();

        public List<ICardTime> getCardTime(DateTime d,int eid)
        {
            List<CardTime> r = new List<CardTime>();
            r = ctDal.getList(new CardTime() { EmployeeId = eid, cardTime = d });
            return r.ConvertAll(c=>(ICardTime)c);
        }
        public List<ICardTime> getCardTimeDays(int eid, DateTime begin, DateTime end)
        {
            List<CardTime> r = new List<CardTime>();
            r = ctDal.search(eid, begin, end);
            return r.ConvertAll(c => (ICardTime)c);
        }
        /// <summary>
        /// 返回bid部门号的员工,HWATT中考勤部门号为3
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        public List<IEmployee> getDevEmployees(int bid = 3)
        {
            List<Employee> Emps = null;
            Emps = deDal.getList(new Employee() { BrchID = bid });
            return Emps.ConvertAll(c => (IEmployee)c);
        }
        public IEmployee getDevEmployee(int eid)
        {
            Employee r = null;
            r = deDal.getSingle(eid.ToString());
            return r;
        }
    }
}
