using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Attendance.Model;
using FluentData;

namespace Attendance.DAL
{
    public class EventDeclaredDAL:DAL<EventDeclaredModel>
    {
        public EventDeclaredDAL() : base("Attendance_EventDeclared")
        {
            setBaseQuery("select autoID,EventDescription,FeeCar,FeeMeals,FeeOther,isCar,Other,Memo,checkMan,Manager,recordID,isBeginTime from ["+TableName+"] where 1 = 1 ");
        }

        protected override void Mapper(EventDeclaredModel m, IDataReader row)
        {
            m.autoID = row.GetInt32("autoID");
            m.recordID = row.GetInt32("recordID");
            m.FeeCar = row.IsDBNull("FeeCar") ? new Nullable<decimal>() : row.GetDecimal("FeeCar");
            m.FeeMeals = row.IsDBNull("FeeMeals") ? new Nullable<decimal>() : row.GetDecimal("FeeMeals");
            m.FeeOther = row.IsDBNull("FeeOther") ? new Nullable<decimal>() : row.GetDecimal("FeeOther");
            m.isCar = row.IsDBNull("isCar") ? new Nullable<bool>() : row.GetBoolean("isCar");
            m.isBeginTime = row.IsDBNull("isBeginTime") ? false : row.GetBoolean("isBeginTime");
            m.checkMan = row.GetString("checkMan");
            m.EventDescription = row.GetString("EventDescription");
            m.Manager = row.GetString("Manager");
            m.Memo = row.GetString("Memo");
            m.Other = row.GetString("Other");
        }

        protected override string WhereStr(EventDeclaredModel t)
        {
            wStr = new StringBuilder();
            if (t.autoID > 0)
                wStr.Append(" and autoID = "+t.autoID);
            if (!string.IsNullOrEmpty(t.checkMan))
                wStr.Append(" and checkMan like '%" + t.checkMan + "%'");
            if (!string.IsNullOrEmpty(t.EventDescription))
                wStr.Append(" and EventDescription like '%" + t.EventDescription + "%'");
            if (!string.IsNullOrEmpty(t.Manager))
                wStr.Append(" and Manager like '%" + t.Manager + "%'");
            if (!string.IsNullOrEmpty(t.Memo))
                wStr.Append(" and Memo like '%" + t.Memo + "%'");
            if (!string.IsNullOrEmpty(t.Other))
                wStr.Append(" and Other like '%" + t.Other + "%'");
            if (t.recordID > 0)
                wStr.Append(" and recordID = " + t.recordID);
            return wStr.ToString();
        }

        public override long Create(EventDeclaredModel t)
        {
                long r = -1;
            try
            {
                r =
                Context.Insert(TableName, t)
                .Column("EventDescription", t.EventDescription)
                .Column("FeeCar", t.FeeCar)
                .Column("FeeMeals", t.FeeMeals)
                .Column("FeeOther", t.FeeOther)
                .Column("isCar", t.isCar)
                .Column("Other", t.Other)
                .Column("Memo", t.Memo)
                .Column("checkMan", t.checkMan)
                .Column("Manager", t.Manager)
                .Column("recordID", t.recordID)
                .Column("isBeginTime", t.isBeginTime)
                .ExecuteReturnLastId<long>();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return r;
        }

        public override int Delete(long id)
        {
            int r =
                Context.Delete(TableName).Where("autoID", id).Execute();
            return r;
        }

        public override List<EventDeclaredModel> selects(EventDeclaredModel t)
        {
            string sqlcmd = BaseQuery + WhereStr(t);
            var sels = Context.Sql(sqlcmd).QueryMany<EventDeclaredModel>(Mapper);
            return sels;
        }
        public List<EventDeclaredModel> listWithRecordIDS(int[] recordids)
        {
            string recordidstr = string.Empty;
            if (recordids != null && recordids.Count() > 0)
            {
                foreach (int id in recordids) recordidstr += ("'" + id.ToString() + "',");
                recordidstr = recordidstr.Substring(0, recordidstr.Length - 1);
                string sqlcmd = BaseQuery
                    + " and recordid in (" + recordidstr + ")";
                var sels = Context.Sql(sqlcmd).QueryMany<EventDeclaredModel>(Mapper);
                return sels;
            }
            else return null;
        }
        public override EventDeclaredModel Single(long id)
        {
            string sqlcmd = BaseQuery + " and autoID = "+id;
            var sels = Context.Sql(sqlcmd).QuerySingle<EventDeclaredModel>(Mapper);
            return sels;
        }

        public override EventDeclaredModel Single(string code)
        {
            throw new Exception("不提供");
        }

        public override int Update(EventDeclaredModel t)
        {
            int r = 0;
            r = Context.Update(TableName)
                .Column("EventDescription", t.EventDescription)
                .Column("FeeCar", t.FeeCar)
                .Column("FeeMeals", t.FeeMeals)
                .Column("FeeOther", t.FeeOther)
                .Column("isCar", t.isCar)
                .Column("Other", t.Other)
                .Column("Memo", t.Memo)
                .Column("checkMan", t.checkMan)
                .Column("Manager", t.Manager)
                .Column("recordID", t.recordID)
                .Column("isBeginTime", t.isBeginTime)
                .Where("autoid", t.autoID)
                .Execute();
            return r;
        }
    }
}
