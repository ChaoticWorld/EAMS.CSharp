using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Attendance.Model;
using FluentData;

namespace Attendance.DAL
{
    public class SchemeDAL : DAL<SchemeModel>
    {
        public SchemeDAL():base("Attendance_Scheme")
        {
            setBaseQuery("select schemeID,schemeName,schemeDescription,periods,classs from ["+TableName+"] where 1 = 1 ");
        }
        protected override void Mapper(SchemeModel m, IDataReader row)
        {
            m.schemeID = row.GetInt32("schemeID");
            m.schemeName = row.GetString("schemeName");
            m.schemeDescription = row.GetString("schemeDescription");
            m.periods = row.GetInt32("periods");
            m.classs = row.GetInt32("classs");
        }

        protected override string WhereStr(SchemeModel t)
        {
            wStr = new StringBuilder();
            if (t != null)
            {
                if (t.schemeID > 0)
                    wStr.Append(" and schemeID = " + t.schemeID);
                if (t.periods > 0)
                    wStr.Append(" and periods = " + t.periods);
                if (t.classs > 0)
                    wStr.Append(" and classs = " + t.classs);
                if (!string.IsNullOrEmpty(t.schemeName))
                    wStr.Append(" and schemeName like '%" + t.schemeName + "%'");
                if (!string.IsNullOrEmpty(t.schemeDescription))
                    wStr.Append(" and schemeDescription like '%" + t.schemeDescription + "%'");
            }
            return wStr.ToString();
        }
        public override long Create(SchemeModel t)
        {
            int r =
            Context.Insert(TableName, t).AutoMap(a => a.schemeID).Execute();
            return r;
        }

        public override int Delete(long id)
        {
            int r =
                Context.Delete(TableName).Where("schemeID", id).Execute();
            return r;
        }

        public override List<SchemeModel> selects(SchemeModel t)
        {
            string sqlcmd = BaseQuery + WhereStr(t);
            var sels = Context.Sql(sqlcmd).QueryMany<SchemeModel>(Mapper);
            return sels;
        }

        public override SchemeModel Single(long id)
        {
            string sqlcmd = BaseQuery + " and schemeID = " + id.ToString();
            var sels = Context.Sql(sqlcmd).QuerySingle<SchemeModel>(Mapper);
            return sels;
        }

        public override SchemeModel Single(string code)
        {
            string sqlcmd = BaseQuery + " and schemeName = " + code;
            var sels = Context.Sql(sqlcmd).QuerySingle<SchemeModel>(Mapper);
            return sels;
        }

        public override int Update(SchemeModel t)
        {
            int r =
                Context.Update(TableName, t)
                .Column("schemeName", t.schemeName)
                .Column("schemeDescription", t.schemeDescription)
                .Column("periods", t.periods)
                .Column("classs",t.classs)
                .Where("schemeID", t.schemeID)
                .Execute();
            return r;
        }

    }
}
