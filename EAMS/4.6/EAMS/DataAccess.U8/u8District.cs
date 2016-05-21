using System;
using System.Collections.Generic;
using System.Text;
using DataModel;
using FluentData;
using IDataAccess;

namespace DataAccess.U8
{
    public class u8District : u8<District>
    {
        public u8District getInstance() {
            return new u8District();
        }
        private District _district = new District();
        private List<District> _districts = new List<District>();
        private string whereStr(District searchKey)
        {
            StringBuilder wStr = new StringBuilder();
            if (!string.IsNullOrEmpty(searchKey.dcCode))
                wStr.Append(" and cDCCode like '" + searchKey.dcCode + "%'");
            if (!string.IsNullOrEmpty(searchKey.dcName))
                wStr.Append(" and cDCName like '%" + searchKey.dcName + "%'");
            if (0 < searchKey.iGrade)
                wStr.Append(" and iDCGrade = " + searchKey.iGrade);
            if (searchKey.isEnd)
                wStr.Append(" and bDCEnd = '" + (searchKey.isEnd ? "1" : "0") + "'");
            return string.Empty;
        }
        private string headSqlCmd()
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append("select cDCCode,cDCName,iDCGrade,bDCEnd from districtClass");
            sqlcmd.Append(" where 1 = 1 ");
            return sqlcmd.ToString();
            //return string.Empty;
        }
        private void districtMapper(District dist, IDataReader row)
        {
            dist.dcCode = row.GetString("cDCCode");
            dist.dcName = row.GetString("cDCName");
            dist.iGrade = Convert.ToInt32(row.GetValue("iDCGrade"));
            dist.isEnd = row.GetBoolean("bDCEnd");
            dist.upDCCode = dist.iGrade > 1 ? dist.dcCode.Substring(0,2*(dist.iGrade-1)) : string.Empty;//Substring()中的2为2编码级别，可以从用友编码规则中读取。
            dist.upDC = getSingle(dist.upDCCode);
            
        }
        public u8District() { Fields = new List<string>() { "cDCCode"," cDCName"," iDCGrade"," bDCEnd" }; }
        public override List<District> getList(District searchKey)
        {
            return getList(whereStr(searchKey));
        }

        public override List<District> getList(string whereStr)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(whereStr);
            _districts = Context.Sql(sqlcmd.ToString()).QueryMany<District>(districtMapper);
            return _districts;
        }

        public override District getSingle(string code)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(" and cDCCode = '" + code + "'");
            _district = Context.Sql(sqlcmd.ToString()).QuerySingle<District>(districtMapper);
            return _district;
        }

        public override void setField(string field, string val, string whereStr)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" Update districtClass ");
            sqlcmd.Append(" set " + field + " = '" + val + "'");
            sqlcmd.Append(" where 1 = 1 ");
            sqlcmd.Append(whereStr);
            Context.Sql(sqlcmd.ToString()).Execute();
        }

        public override void setField(string field, string val, District searchKey)
        {
            setField(field,val,whereStr(searchKey));
        }
        public override object getField(string field, District searchKey)
        {
            object r = null;
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" select " + field + " from DistrictClass where 1 = 1 ");
            sqlcmd.Append(whereStr(searchKey));
            r = Context.Sql(sqlcmd.ToString()).QuerySingle<dynamic>();
            return r;
        }
    }
}