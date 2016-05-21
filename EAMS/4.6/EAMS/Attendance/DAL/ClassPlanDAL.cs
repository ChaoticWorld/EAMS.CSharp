using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Attendance.Model;
using FluentData;

namespace Attendance.DAL
{
    public class ClassPlanDAL : DAL<ClassPlanModel>
    {
        public ClassPlanDAL():base("Attendance_ClassPlan")
        { setBaseQuery("select classId,periodNo,bTime,eTime,upTime,autoid,sdate from ["+TableName+"] where 1 = 1 "); }

        protected override void Mapper(ClassPlanModel m, IDataReader row)
        {
            m.autoid = row.GetInt32("autoid");
            m.classId = row.GetInt32("classId");
            m.periodNo = row.GetInt32("periodNo");
            m.bTime = row.GetString("bTime");
            m.eTime = row.GetString("eTime");
            m.upTime = row.GetString("upTime");
            m.sdate = row.IsDBNull("sdate") ? new Nullable<DateTime>() : row.GetDateTime("sdate");
        }

        protected override string WhereStr(ClassPlanModel t)
        {
            wStr = new StringBuilder();
            if (t.autoid > 0)
                wStr.Append(" and autoid = " + t.autoid);
            if (t.classId.HasValue && t.classId > 0)
                wStr.Append(" and classId = " + t.classId);
            if (t.periodNo > 0)
                wStr.Append(" and periodNo =" + t.periodNo);
            if (t.sdate.HasValue)
                wStr.Append(" and month(sdate) = " + t.sdate.Value.Month);
            if (!string.IsNullOrEmpty(t.bTime))
                wStr.Append(" and bTime like '%" + t.bTime + "%'");
            if (!string.IsNullOrEmpty(t.eTime))
                wStr.Append(" and eTime like '%" + t.eTime + "%'");
            if (!string.IsNullOrEmpty(t.upTime))
                wStr.Append(" and upTime like '%" + t.upTime + "%'");
            return wStr.ToString();
        }
        public override long Create(ClassPlanModel t)
        {
            long r =
                Context.Insert(TableName, t)
                .Column("classId", t.classId)
                .Column("periodNo", t.periodNo)
                .Column("sdate", t.sdate)
                .Column("bTime", t.bTime)
                .Column("eTime", t.eTime)
                .Column("upTime", t.upTime)
                .ExecuteReturnLastId<long>();
            return r;
        }

        public override int Delete(long id)
        {
            int r =
                Context.Delete(TableName).Where("autoid", id).Execute();
            return r;
        }

        public override List<ClassPlanModel> selects(ClassPlanModel t)
        {
            string sqlcmd = BaseQuery + WhereStr(t);
            var sels = Context.Sql(sqlcmd).QueryMany<ClassPlanModel>(Mapper);
            return sels;
        }

        public override ClassPlanModel Single(long id)
        {
            string sqlcmd = BaseQuery + " and autoid = "+id;
            var sels = Context.Sql(sqlcmd).QuerySingle<ClassPlanModel>(Mapper);
            return sels;
        }

        public override ClassPlanModel Single(string code)
        {
            throw new Exception("不提供");
        }

        public override int Update(ClassPlanModel t)
        {//classId,periodNo,bTime,eTime,upTime,autoid,sdate 
            int r =
                Context.Update(TableName)
                .Column("classId", t.classId)
                .Column("periodNo", t.periodNo)
                .Column("sdate", t.sdate)
                .Column("bTime", t.bTime)
                .Column("eTime", t.eTime)
                .Column("upTime", t.upTime)
                .Where("autoid", t.autoid)
                .Execute();
            return r;
        }
    }
}
