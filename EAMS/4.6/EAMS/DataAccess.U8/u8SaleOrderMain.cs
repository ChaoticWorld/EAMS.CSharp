using System;
using System.Collections.Generic;
using System.Text;
using DataModel;
using FluentData;
using IDataAccess;

namespace DataAccess.U8
{
    public class u8SaleOrderMain : u8<VouchMain>
    {
        private VouchMain _main;
        private List<VouchMain> _mains;
        private string whereStr(VouchMain searchKey)
        {
            StringBuilder wStr = new StringBuilder();
            if (string.IsNullOrEmpty(searchKey.vouchCode))
                wStr.Append(" and cSOCode like '%"+searchKey.vouchCode+"%'");
            if (searchKey.Mid > 0)
                wStr.Append(" and id = '"+searchKey.Mid+"'");
            if (System.Data.SqlTypes.SqlDateTime.MinValue <= searchKey.vouchDate
                && System.Data.SqlTypes.SqlDateTime.MaxValue >= searchKey.vouchDate)
                wStr.Append(" and dDate = '" + searchKey.vouchDate.ToString("yyyy-MM-dd") + "'");
            if (searchKey.corporatio != null)
            {
                if (string.IsNullOrEmpty(searchKey.corporatio.Code))
                    wStr.Append(" and cCusCode = '" + searchKey.corporatio.Code + "'");
                if (string.IsNullOrEmpty(searchKey.corporatio.Name))
                    wStr.Append(" and cCusName like '%" + searchKey.corporatio.Name + "%'");
            }
            if (string.IsNullOrEmpty(searchKey.cSender))
                wStr.Append("");//送货人，用友单据里没有此项，没想好怎么处理
            if (string.IsNullOrEmpty(searchKey.cShipAddress))
                wStr.Append(" and cCusOAddress like '%"+searchKey.cShipAddress+"%'");
            if (string.IsNullOrEmpty(searchKey.vouchContact))
                wStr.Append(" and ccusperson like '%"+searchKey.vouchContact+"%'");
            if (string.IsNullOrEmpty(searchKey.Verifier))
                wStr.Append(" and cVerifier like '%"+searchKey.Verifier+"%'");
            if (string.IsNullOrEmpty(searchKey.Maker))
                wStr.Append(" and cMaker like '%"+searchKey.Maker+"%'");
            if (string.IsNullOrEmpty(searchKey.Memo))
                wStr.Append(" and cMemo like '%"+searchKey.Memo+"%'");
            return wStr.ToString();
        }
        private string headSqlCmd()
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append("select id, csocode, ccusCode, dDate ");
            sqlcmd.Append(" , dCreateSystime, dmodifySystime, cModifier ");
            sqlcmd.Append(" , cCusOAddress, cMaker, cVerifier, cMemo, ccusperson ");
            sqlcmd.Append(" from SO_SOMain where 1 = 1 ");
            return sqlcmd.ToString();
        }
        private void vmMapper(VouchMain vm,IDataReader row)
        {
            vm.vouchCode = row.GetString("cSoCode");
            vm.Mid = Convert.ToInt32(row.GetValue("id"));
            vm.vouchDate = row.GetDateTime("dDate");
            vm.corporatio = new u8Customer().getSingle(row.GetString("ccusCode"));
            vm.cShipAddress = row.GetString("cCusOAddress");
            vm.ModifyDate = row.GetDateTime("dmodifySystime");
            vm.ModifyMan = row.GetString("cModifier");
            vm.CreateDate = row.GetDateTime("dCreateSystime");
            vm.CreateMan = row.GetString("cMaker");
            vm.Maker = row.GetString("cMaker");
            vm.Verifier = row.GetString("cVerifier");
            vm.vouchContact = row.GetString("ccusperson");
            vm.Memo = row.GetString("cMemo");
        }
        public u8SaleOrderMain() {
            Fields = new List<string>() { "id", "csocode", "ccusCode", "dDate", "dCreateSystime", "dmodifySystime", "cModifier ", "cCusOAddress", "cMaker", "cVerifier", "cMemo", "ccusperson" };
        }
        public override List<VouchMain> getList(VouchMain searchKey)
        {
            return getList(whereStr(searchKey));
        }

        public override List<VouchMain> getList(string whereStr)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(whereStr);
            _mains = Context.Sql(sqlcmd.ToString()).QueryMany<VouchMain>(vmMapper);
            return _mains;
        }

        public override VouchMain getSingle(string code)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(" and csocode = '"+code+"'");
            _main = Context.Sql(sqlcmd.ToString()).QuerySingle<VouchMain>(vmMapper);
            return _main;
        }

        public override void setField(string field, string val, string whereStr)
        {
            //only Modify define
            sqlcmd = new StringBuilder();
            sqlcmd.Append("Update SO_SOMain");
            sqlcmd.Append(" set " + field + " = '" + val + "'");
            sqlcmd.Append(" where 1 = 1 ");
            sqlcmd.Append(whereStr);
            Context.Sql(sqlcmd.ToString()).Execute();
        }

        public override void setField(string field, string val, VouchMain searchKey)
        {
            //only Modify define
            setField(field, val, whereStr(searchKey));
        }
        public override object getField(string field, VouchMain searchKey)
        {
            object r = null;
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" select " + field + " from SO_SOMain where 1 = 1 ");
            sqlcmd.Append(whereStr(searchKey));
            r = Context.Sql(sqlcmd.ToString()).QuerySingle<dynamic>();
            return r;
        }
    }
}
