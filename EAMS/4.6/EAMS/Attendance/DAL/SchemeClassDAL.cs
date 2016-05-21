using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Attendance.Model;
using FluentData;

namespace Attendance.DAL
{
    public class SchemeClassDAL : DAL<SchemeClassModel>
    {
        public SchemeClassDAL():base("Attendance_SchemeClass")
        {
            setBaseQuery("select classId,schemeId,className from [" + TableName+"] where 1 = 1 ");
        }

        protected override void Mapper(SchemeClassModel m, IDataReader row)
        {
            m.classId = int.Parse(row.GetValue("classId").ToString());
            m.schemeId = int.Parse(row.GetValue("schemeId").ToString());
            m.className = row.GetString("className");
        }

        protected override string WhereStr(SchemeClassModel t)
        {
            wStr = new StringBuilder();
            if (t != null)
            {
                if (t.classId > 0)
                    wStr.Append(" and classId = " + t.classId);
                if (t.schemeId > 0)
                    wStr.Append(" and schemeId = " + t.schemeId);
                if (!string.IsNullOrEmpty(t.className))
                    wStr.Append(" and className like '%" + t.className + "%'");
            }
            return wStr.ToString();
        }
        public override long Create(SchemeClassModel t)
        {
            int r =
                Context.Insert(TableName, t).AutoMap(a => a.classId).Execute();
            return r;
        }

        public override int Delete(long id)
        {
            int r =
                Context.Delete(TableName).Where("classId", id).Execute();
            return r;
        }

        public override List<SchemeClassModel> selects(SchemeClassModel t)
        {
            string sqlcmd = BaseQuery + WhereStr(t);
            var sels = Context.Sql(sqlcmd).QueryMany<SchemeClassModel>(Mapper);
            return sels;            
        }

        public override SchemeClassModel Single(long id)
        {
            string sqlcmd = BaseQuery + " and classId = "+id;
            var sels = Context.Sql(sqlcmd).QuerySingle<SchemeClassModel>(Mapper);
            return sels;
        }

        public override SchemeClassModel Single(string code)
        {
            string sqlcmd = BaseQuery + " and className = " + code;
            var sels = Context.Sql(sqlcmd).QuerySingle<SchemeClassModel>(Mapper);
            return sels;
        }

        public override int Update(SchemeClassModel t)
        {
            int r =
                Context.Update(TableName, t)
                .Column("className", t.className)
                .Column("schemeId", t.schemeId)
                .Where("classId", t.classId)
                .Execute();
            return r;
        }
    }
}
