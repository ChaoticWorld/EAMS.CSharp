using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDataAccess;
using DataAccess;
using FluentData;

namespace AttendanceDevice.HWATT.DAL
{
    public class CardTimeDAL:DAL<CardTime>
    {
        public CardTimeDAL()
        {
            setBaseQuery("select card.cardID, card.employeeID, card.CardTime "
              + " from kqz_card card where 1 = 1 ");
            Fields = new List<string>() { "card.cardID", "card.employeeID", "card.CardTime" };
        }

        private string WhereStr(CardTime searchKey)
        {
            wStr = new StringBuilder();
            if (searchKey != null)
            {
                if (searchKey.cardID > 0)
                    wStr.Append(" and cardID = " + searchKey.cardID);
                if (searchKey.EmployeeId > 0)
                    wStr.Append(" and EmployeeId = " + searchKey.EmployeeId);
                if (searchKey.cardTime != null)
                {
                    wStr.Append(" and year(cardTime) = " + searchKey.cardTime.Year);
                    wStr.Append(" and month(cardTime) = " + searchKey.cardTime.Month);
                    wStr.Append(" and day(cardTime) = " + searchKey.cardTime.Day);
                }
            }
            return wStr.ToString();
        }

        public override List<CardTime> getList(CardTime searchKey)
        {
            List<CardTime> r = new List<CardTime>();
            string sqlcmd = BaseQuery + WhereStr(searchKey);
            r = Context.Sql(sqlcmd).QueryMany<CardTime>();
            return r;
        }
        public List<CardTime> search(int eid, DateTime begin, DateTime end)
        {
            List<CardTime> r = new List<CardTime>();
            string sqlcmd = BaseQuery
                + " and card.employeeID = " + eid.ToString()
                + " and card.CardTime > '" + begin.AddDays(-1).ToString("yyyy-MM-dd") + "'"
                + " and card.CardTime < '" + end.AddDays(1).ToString("yyyy-MM-dd") + "'";
            r = Context.Sql(sqlcmd).QueryMany<CardTime>();
            return r;
        }

        public override object getField(string field, CardTime searchKey)
        {
            string sqlcmd = "select [" + field + "] from kqz_card where 1 = 1" + WhereStr(searchKey);
            object r = Context.Sql(sqlcmd).QuerySingle<object>();
            return r;
        }

        public override List<CardTime> getList(string code)
        {
            List<CardTime> r = new List<CardTime>();
            int uid = -1;
            DateTime yearmonth = new DateTime();
            string[] codes = code.Split(',');
            if (int.TryParse(codes[0], out uid) && DateTime.TryParse(codes[1], out yearmonth))
            {
                string sqlcmd = BaseQuery
                    + " and EmployeeId = " + uid
                    + " and year(cardTime) = " + yearmonth.Year
                    + " and month(cardTime) = " + yearmonth.Month;
                r = Context.Sql(sqlcmd).QueryMany<CardTime>();
            }
            return r;
        }

        public override CardTime getSingle(string code)
        {
            CardTime r = new CardTime();
            string sqlcmd = BaseQuery + " and cardID = " + code;
            r = Context.Sql(sqlcmd).QuerySingle<CardTime>();
            return r;
        }

        public override void setField(string field, string val, string whereStr)
        {
            string sqlcmd = "update kqz_card set [" + field + "] = '" + val + "' where 1 = 1 " + whereStr;
            Context.Sql(sqlcmd).Execute();
        }

        public override void setField(string field, string val, CardTime searchKey)
        {
            setField(field, val, WhereStr(searchKey));
        }
 
    }
}
