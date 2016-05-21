using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Attendance.Model;
using FluentData;

namespace Attendance.DAL
{
    public class RecordDAL:DAL<RecordModel>
    {
        public RecordDAL() : base("Attendance_Records")
        {
            setBaseQuery("select autoid,EmployeeID,classID,periodNo,sDate,bAttTimeStr,eAttTimeStr,bOffset,eOffset,dateType,bOffsetFee,eOffsetFee from ["+TableName+"] where 1 = 1 ");
        }

        protected override void Mapper(RecordModel m, IDataReader row)
        {
            m.autoid = row.GetInt32("autoid");
            m.classID = row.GetInt32("classID");
            m.EmployeeID = row.GetInt32("EmployeeID");
            m.periodNo = row.GetInt32("periodNo");
            m.sDate = row.IsDBNull("sDate") ? new Nullable<DateTime>() : row.GetDateTime("sDate");
            m.eOffsetFee = row.IsDBNull("eOffsetFee") ? 0 : row.GetDecimal("eOffsetFee");
            m.eOffset = row.IsDBNull("eOffset") ? 0 : row.GetInt32("eOffset");
            m.eAttTimeStr = row.GetString("eAttTimeStr");
            m.dayName = row.GetString("dateType");
            m.bOffsetFee = row.IsDBNull("bOffsetFee") ? 0 : row.GetDecimal("bOffsetFee");
            m.bOffset = row.IsDBNull("bOffset") ? 0 : row.GetInt32("bOffset");
            m.bAttTimeStr = row.GetString("bAttTimeStr");
        }

        protected override string WhereStr(RecordModel t)
        {
            wStr = new StringBuilder();
            if (t.autoid > 0)
                wStr.Append(" and autoid = " + t.autoid);
            if (t.classID.HasValue && t.classID > 0)
                wStr.Append(" and classID = " + t.classID);
            if (t.EmployeeID.HasValue && t.EmployeeID > 0)
                wStr.Append(" and EmployeeID = " + t.EmployeeID);
            if (t.periodNo.HasValue && t.periodNo > 0)
                wStr.Append(" and periodNo = " + t.periodNo);
            if (t.sDate.HasValue)
                wStr.Append(" and sDate = '" + t.sDate.Value.ToString("yyyy-MM-dd")+"'");
            if (t.eOffsetFee.HasValue && t.eOffsetFee > 0)
                wStr.Append(" and eOffsetFee = " + t.eOffsetFee.Value);
            if (t.eOffset.HasValue && t.eOffset > 0)
                wStr.Append(" and eOffset = " + t.eOffset);
            if (!string.IsNullOrEmpty(t.eAttTimeStr))
                wStr.Append(" and eAttTimeStr like '%" + t.eAttTimeStr + "%'");
            if (!string.IsNullOrEmpty(t.dayName))
                wStr.Append(" and dateType like '%" + t.dayName + "%'");
            if (t.bOffsetFee.HasValue && t.bOffsetFee > 0)
                wStr.Append(" and bOffsetFee = " + t.bOffsetFee);
            if (t.bOffset.HasValue && t.bOffset > 0)
                wStr.Append(" and bOffset = " + t.bOffset);
            if (!string.IsNullOrEmpty(t.bAttTimeStr))
                wStr.Append(" and bAttTimeStr like '%" + t.bAttTimeStr + "%'");
            return wStr.ToString();
        }

        public override long Create(RecordModel t)
        {
            int r =
                Context.Insert(TableName, t)
                .Column("EmployeeID", t.EmployeeID)
                .Column("classID", t.classID)
                .Column("periodNo", t.periodNo)
                .Column("sDate", t.sDate)
                .Column("bAttTimeStr", t.bAttTimeStr)
                .Column("eAttTimeStr", t.eAttTimeStr)
                .Column("bOffset", t.bOffset)
                .Column("eOffset", t.eOffset)
                .Column("dateType", t.dayName)
                .Column("bOffsetFee", t.bOffsetFee)
                .Column("eOffsetFee", t.eOffsetFee)
                .ExecuteReturnLastId<int>();
            return r;
        }

        public override int Delete(long id)
        {
            int r =
                Context.Delete(TableName).Where("autoid", id).Execute();
            return r;
        }
        public List<RecordModel> selects(RecordModel t, int topNum,string orderby)
        {
            string sqlcmd = "select top " + topNum.ToString() + BaseQuery.Remove(0, 6) + WhereStr(t) + orderby;
            var sels = Context.Sql(sqlcmd).QueryMany<RecordModel>(Mapper);
            return sels;
        }
        public override List<RecordModel> selects(RecordModel t)
        {
            string sqlcmd = BaseQuery + WhereStr(t);
            var sels = Context.Sql(sqlcmd).QueryMany<RecordModel>(Mapper);
            return sels;
        }
        public List<RecordModel> search(int eid,DateTime begin,DateTime end)
        {
            List<RecordModel> r = null;
            string sqlcmd = BaseQuery
                + " and EmployeeID = " + eid
                + " and sDate > '" + begin.AddDays(-1).ToString("yyyy-MM-dd") + "'"
                + " and sDate < '" + end.AddDays(1).ToString("yyyy-MM-dd") + "'";
            r = Context.Sql(sqlcmd).QueryMany<RecordModel>(Mapper);
            return r;
        }
        public override RecordModel Single(long id)
        {
            string sqlcmd = BaseQuery + " and autoid = "+id;
            var sels = Context.Sql(sqlcmd).QuerySingle<RecordModel>(Mapper);
            return sels;
        }

        public override RecordModel Single(string code)
        {
            throw new Exception("不提供");
        }

        public override int Update(RecordModel t)
        {
            //autoid,EmployeeID,classID,periodNo,sDate,bAttTimeStr,eAttTimeStr,bOffset,eOffset,dateType,bOffsetFee,eOffsetFee 
            int r = 0;
            r = Context.Update(TableName)
                .Column("EmployeeID", t.EmployeeID)
                .Column("classID",t.classID)
                .Column("periodNo",t.periodNo)
                .Column("sDate",t.sDate)
                .Column("bAttTimeStr",t.bAttTimeStr)
                .Column("eAttTimeStr",t.eAttTimeStr)
                .Column("bOffset",t.bOffset)
                .Column("eOffset",t.eOffset)
                .Column("dateType",t.dayName)
                .Column("bOffsetFee",t.bOffsetFee)
                .Column("eOffsetFee",t.eOffsetFee)
                .Where("autoid",t.autoid)
                .Execute();

            return r;
        }
    }
}
