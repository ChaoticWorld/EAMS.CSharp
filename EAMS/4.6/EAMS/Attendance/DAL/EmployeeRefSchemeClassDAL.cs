using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Attendance.Model;
using FluentData;

namespace Attendance.DAL
{
    public class EmployeeRefSchemeClassDAL : DAL<EmployeeRefSchemeClassModel>
    {
        public EmployeeRefSchemeClassDAL() : base("Attendance_EmployeeRefSchemeClass")
        {
            setBaseQuery("select autoid,UserID,EmployeeID,classID,EffDate,periodNo,isVaild,EmployeeName from ["+TableName+"] where 1 = 1 ");
        }

        protected override void Mapper(EmployeeRefSchemeClassModel m, IDataReader row)
        {
            m.autoid = row.GetInt32("autoid");
            m.classID = row.GetInt32("classID");
            m.EmployeeID = row.GetInt32("EmployeeID");
            m.UserID = row.GetInt32("UserID");
            m.periodNo = row.GetInt32("periodNo");
            m.EffDate = row.IsDBNull("EffDate") ? new Nullable<DateTime>() : row.GetDateTime("EffDate");
            m.isVaild = row.GetBoolean("isVaild");
            m.EmployeeName = row.GetString("EmployeeName");
        }

        protected override string WhereStr(EmployeeRefSchemeClassModel t)
        {
            wStr = new StringBuilder();
            if (t != null)
            {
                if (t.autoid > 0)
                    wStr.Append(" and autoid = " + t.autoid);
                if (t.UserID.HasValue && t.UserID > 0)
                    wStr.Append(" and UserID = " + t.UserID);
                if (t.EmployeeID.HasValue && t.EmployeeID > 0)
                    wStr.Append(" and EmployeeID = " + t.EmployeeID);
                if (t.classID.HasValue && t.classID > 0)
                    wStr.Append(" and classID = " + t.classID);
                if (t.periodNo.HasValue && t.periodNo > 0)
                    wStr.Append(" and periodNo = " + t.periodNo);
                if (t.isVaild.HasValue && t.isVaild.Value)
                    wStr.Append(" and isVaild = " + (t.isVaild.Value ? 1 : 0));
                if (t.EffDate.HasValue)
                    wStr.Append(" and month(EffDate) = " + t.EffDate.Value.Month);
                if (!string.IsNullOrEmpty(t.EmployeeName))
                    wStr.Append(" and EmployeeName like '%" + t.EmployeeName + "%'");
            }
            return wStr.ToString();
        }
        public override long Create(EmployeeRefSchemeClassModel t)
        {
            int r =
                Context.Insert(TableName, t)
                .Column("UserID", t.UserID)
                .Column("EmployeeID", t.EmployeeID)
                .Column("classID", t.classID)
                .Column("periodNo", t.periodNo)
                .Column("EffDate", t.EffDate)
                .Column("isVaild", t.isVaild)
                .Column("EmployeeName", t.EmployeeName)
                .ExecuteReturnLastId<int>();
            return r;
        }

        public override int Delete(long id)
        {
            int r =
                Context.Delete(TableName).Where("autoid", id).Execute();
            return r;
        }

        public List<EmployeeRefSchemeClassModel> search(int eid, DateTime begin, DateTime end)
        {
            string sqlcmd = BaseQuery
                + " and EmployeeID = " + eid
                + " and EffDate > '" + begin.AddDays(-1).ToString("yyyy-MM-dd") + "'"
                + " and EffDate < '" + end.AddDays(1).ToString("yyyy-MM-dd") + "'";
            var sels = Context.Sql(sqlcmd).QueryMany<EmployeeRefSchemeClassModel>(Mapper);
            return sels;
        }
        public override List<EmployeeRefSchemeClassModel> selects(EmployeeRefSchemeClassModel t)
        {
            string sqlcmd = BaseQuery + WhereStr(t) + " order by autoid,UserID,EmployeeID ";
            var sels = Context.Sql(sqlcmd).QueryMany<EmployeeRefSchemeClassModel>(Mapper);
            return sels;
        }

        public override EmployeeRefSchemeClassModel Single(long id)
        {
            string sqlcmd = BaseQuery + "autoid" + id;
            var sels = Context.Sql(sqlcmd).QuerySingle
                <EmployeeRefSchemeClassModel>(Mapper);
            return sels;
        }

        public override EmployeeRefSchemeClassModel Single(string code)
        {
            throw new Exception("不提供");
        }

        public override int Update(EmployeeRefSchemeClassModel t)
        {//autoid,UserID,EmployeeID,classID,EffDate,periodNo,isVaild,EmployeeName
            int r =
                Context.Update(TableName)
                .Column("UserID", t.UserID)
                .Column("EmployeeID", t.EmployeeID)
                .Column("classID", t.classID)
                .Column("periodNo", t.periodNo)
                .Column("EffDate", t.EffDate)
                .Column("isVaild", t.isVaild)
                .Column("EmployeeName", t.EmployeeName)
                .Where("autoid", t.autoid)
                .Execute();
            return r;
        }
    }
}
