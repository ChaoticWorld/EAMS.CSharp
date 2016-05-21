using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAttendanceDevice;
using IDataAccess;
using DataAccess;
using FluentData;

namespace AttendanceDevice.HWATT.DAL
{
    public class EmployeeDAL : DAL<Employee>
    {
        public EmployeeDAL()
        {
            Context = new FluentData.DbContext().ConnectionStringName("AttendanceDevice", new SqlServerProvider());
            setBaseQuery("select e.EmployeeID, e.EmployeeName, e.EmployeeCode, brch.BrchID, brch.BrchName from KQZ_Employee e left join KQZ_Brch brch on e.BrchID = brch.BrchID where 1 = 1 ");
            Fields = new List<string>() { "e.EmployeeID", "e.EmployeeName", "e.EmployeeCode", "brch.BrchID", "brch.BrchName" };
        }
        private void Mapper(Employee m, IDataReader row)
        {
            m.BrchID = int.Parse(row.GetValue("BrchID").ToString());
            m.EmployeeId = int.Parse(row.GetValue("EmployeeID").ToString());
            m.BrchName = row.GetString("BrchName");
            m.EmployeeCode = row.GetString("EmployeeCode");
            m.EmployeeName = row.GetString("EmployeeName");
        }
        private string WhereStr(Employee key) {
            wStr = new StringBuilder();
            if (key != null)
            {
                if (key.EmployeeId > 0)
                    wStr.Append(" and e.EmployeeID = " + key.EmployeeId);
                if (key.BrchID > 0)
                    wStr.Append(" and brch.BrchID = " + key.BrchID);
                if (!string.IsNullOrEmpty(key.BrchName))
                    wStr.Append(" and brch.BrchName like '%" + key.BrchName + "%'");
                if (!string.IsNullOrEmpty(key.EmployeeCode))
                    wStr.Append(" and e.EmployeeCode like '%" + key.EmployeeCode + "%'");
                if (!string.IsNullOrEmpty(key.EmployeeName))
                    wStr.Append(" and e.EmployeeName like '%" + key.EmployeeName + "%'");
            }
            return wStr.ToString();
        }

        public override List<Employee> getList(Employee searchKey)
        {
            List<Employee> r = new List<Employee>();
            string sqlcmd = BaseQuery + WhereStr(searchKey);
            r = Context.Sql(sqlcmd).QueryMany<Employee>(Mapper);
            return r;
        }

        public override object getField(string field, Employee searchKey)
        {
            string sqlcmd = "select [" + field + "]  from KQZ_Employee e left join KQZ_Brch brch on e.BrchID = brch.BrchID where 1 = 1 " + WhereStr(searchKey);
            object r = Context.Sql(sqlcmd).QuerySingle<object>();
            return r;
        }

        public override List<Employee> getList(string code)
        {
            throw new Exception("不提供");
        }

        public override Employee getSingle(string code)
        {
            Employee r = new Employee();
            string sqlcmd = BaseQuery + " and EmployeeID = " + code;
            r = Context.Sql(sqlcmd).QuerySingle<Employee>();
            return r;
        }

        public override void setField(string field, string val, string whereStr)
        {
            throw new Exception("不提供");
        }

        public override void setField(string field, string val, Employee searchKey)
        {
            throw new Exception("不提供");
        }

    }
}
