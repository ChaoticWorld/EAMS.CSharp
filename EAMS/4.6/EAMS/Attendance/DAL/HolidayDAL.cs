using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Attendance.Model;
using FluentData;

namespace Attendance.DAL
{
    public class HolidayDAL : DAL<HolidayModel>
    {
        public HolidayDAL() : base("Attendance_Holiday")
        {
            setBaseQuery("select autoid,iYear,sDate,sName,sDescription From[" + TableName + "] where 1 = 1 ");
        }

        protected override string WhereStr(HolidayModel t)
        {
            wStr = new StringBuilder();
            if (t.autoid > 0)
                wStr.Append(" and autoid = " + t.autoid);
            if (t.iYear > 0)
                wStr.Append(" and iYear = " + t.iYear);
            if (!string.IsNullOrEmpty(t.sName))
                wStr.Append(" and sName like '%" + t.sName + "%'");
            if (t.sDate.HasValue)
                wStr.Append(" and sDate = '" + t.sDate.Value.ToString("yyyy-MM-dd") + "'");
            if (!string.IsNullOrEmpty(t.sDescription))
                wStr.Append(" and sDescription like '%" + t.sDescription + "%'");            
            return wStr.ToString();
        }
        protected override void Mapper(HolidayModel m, IDataReader row)
        {
            m.autoid = row.GetInt32("autoid");
            m.iYear = row.GetInt32("iYear");
            m.sDate = row.GetDateTime("sDate");
            m.sName = row.GetString("sName");
            m.sDescription = row.GetString("sDescription");
        }

        public override long Create(HolidayModel t)
        {
            int r = Context
                .Insert<HolidayModel>(TableName, t)
                .AutoMap(a=>a.autoid)
                .ExecuteReturnLastId<int>();
            return r;
        }

        public override int Delete(long id)
        {
            int r =
            Context.Delete(TableName).Where("autoid", id).Execute();
            return r;
        }

        public override List<HolidayModel> selects(HolidayModel t)
        {
            string sqlcmd = BaseQuery + WhereStr(t);
            var sels = Context.Sql(sqlcmd).QueryMany<HolidayModel>(Mapper);
            return sels;
        }

        public override HolidayModel Single(long id)
        {
            string sqlcmd = BaseQuery + " and autoid = " + id;
            var single = Context.Sql(sqlcmd).QuerySingle<HolidayModel>(Mapper);
            return single;
        }

        public override HolidayModel Single(string key)
        {
            string sqlcmd = BaseQuery + " and sDate = " + key;
            var single = Context.Sql(sqlcmd).QuerySingle<HolidayModel>(Mapper);
            return single;
        }

        public override int Update(HolidayModel t)
        {
            int r = Context
                .Update(TableName, t)
                .AutoMap(a=>a.autoid)
                .Where(w=>w.autoid)
                .Execute();
            return r;
        }
    }
}
