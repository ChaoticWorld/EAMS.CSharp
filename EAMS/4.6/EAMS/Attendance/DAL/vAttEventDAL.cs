using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using FluentData;
using DataAccess;

namespace Attendance.DAL
{
    public class vEventDAL:abstractDataAccess
    {
        public vEventDAL()
        {
            Context = eamsAppDataContextBase.Context;
            TableName = "v_AttendanceEvent";
            setBaseQuery("select autoID,EventDescription,FeeCar,FeeMeals,FeeOther,isCar,Other,Memo,checkMan,Manager,recordID,isBeginTime,periodNo,bAttTimeStr,eAttTimeStr,bOffset,eOffset,sDate,EmployeeID,bTime,eTime from [" + TableName + "] where 1 = 1 ");
        }
        public List<vAttendanceEventModel> searchAbsence(int eid, DateTime begin, DateTime end)
        {
            wStr = new StringBuilder();
            wStr.Append(" and (bOffset < 0 or eOffset < 0)");
            wStr.Append(" and EmployeeID="+eid);
            wStr.Append(" and sDate >= '"+begin.ToString("yyyy-MM-dd")+"'");
            wStr.Append(" and sDate <= '" + end.ToString("yyyy-MM-dd") + "'");
            string sqlcmd = BaseQuery + wStr.ToString();
            List<vAttendanceEventModel> r = Context.Sql(sqlcmd).QueryMany<vAttendanceEventModel>();
            return r;
        }
        public List<vAttendanceEventModel> searchOver(int eid, DateTime begin, DateTime end)
        {
            wStr = new StringBuilder();
            wStr.Append(" and (bOffset > 0 or eOffset > 0)");
            wStr.Append(" and EmployeeID=" + eid);
            wStr.Append(" and sDate >= '" + begin.ToString("yyyy-MM-dd") + "'");
            wStr.Append(" and sDate <= '" + end.ToString("yyyy-MM-dd") + "'");
            string sqlcmd = BaseQuery + wStr.ToString();
            List<vAttendanceEventModel> r = Context.Sql(sqlcmd).QueryMany<vAttendanceEventModel>();
            return r;
        }
    }
}
