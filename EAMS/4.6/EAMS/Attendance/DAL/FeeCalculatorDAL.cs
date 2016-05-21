using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Attendance.Model;
using FluentData;

namespace Attendance.DAL
{
    public class FeeCalculatorDAL:DAL<FeeCalculatorModel>
    {
        public FeeCalculatorDAL() : base("Attendance_FeeCalculator")
        {
            setBaseQuery("select id,Unit,UnitFee,MaxFee,classId,dateEnum from [" + TableName + "] where 1 = 1 ");
        }

        protected override void Mapper(FeeCalculatorModel m, IDataReader row)
        {
            m.id = row.GetInt32("id");
            m.Unit = row.IsDBNull("Unit") ? new Nullable<int>() : row.GetInt32("Unit");
            m.UnitFee = row.IsDBNull("UnitFee") ? new Nullable<decimal>() : row.GetDecimal("UnitFee");
            m.MaxFee = row.IsDBNull("MaxFee") ? new Nullable<decimal>() : row.GetDecimal("MaxFee");
            m.classId = row.IsDBNull("classId") ? new Nullable<int>() : row.GetInt32("classId");
            m.dateEnum = row.GetString("dateEnum");
        }

        protected override string WhereStr(FeeCalculatorModel t)
        {
            wStr = new StringBuilder();
            if (t.id > 0)
                wStr.Append(" and id = "+t.id);
            if (t.classId > 0)
                wStr.Append(" and classId = "+t.classId);
            if (!string.IsNullOrEmpty(t.dateEnum))
                wStr.Append(" and dateEnum like '%"+t.dateEnum+"%'");
            return wStr.ToString();
        }

        public override long Create(FeeCalculatorModel t)
        {
            int r =
                Context.Insert(TableName, t).Column("Unit", t.Unit)
                .Column("UnitFee", t.UnitFee)
                .Column("MaxFee", t.MaxFee)
                .Column("classId", t.classId)
                .Column("dateEnum", t.dateEnum)
                .ExecuteReturnLastId<int>();
            return r;
        }

        public override int Delete(long id)
        {
            int r =
                Context.Delete(TableName).Where("id", id).Execute();
            return r;
        }

        public override List<FeeCalculatorModel> selects(FeeCalculatorModel t)
        {
            string sqlcmd = BaseQuery + WhereStr(t);
            var sels = Context.Sql(sqlcmd).QueryMany<FeeCalculatorModel>(Mapper);
            return sels;
        }

        public override FeeCalculatorModel Single(long id)
        {
            string sqlcmd = BaseQuery +"id = "+id;
            var sels = Context.Sql(sqlcmd).QuerySingle<FeeCalculatorModel>(Mapper);
            return sels;
        }

        public override FeeCalculatorModel Single(string code)
        {
            throw new Exception("不提供");
        }

        public override int Update(FeeCalculatorModel t)
        {
            int r = 0;
            r = Context.Update(TableName)
                .Column("Unit", t.Unit)
                .Column("UnitFee", t.UnitFee)
                .Column("MaxFee",t.MaxFee)
                .Column("classId",t.classId)
                .Column("dateEnum",t.dateEnum)            
                .Where("id", t.id)
                .Execute();
            return r;
        }
    }
}
