using System;
using System.Collections.Generic;
using System.Text;
using DataModel;
using FluentData;
using IDataAccess;

namespace DataAccess.U8
{
    public class u8InventoryClass : u8<InventoryClass>
    {
        private InventoryClass _invCls = new InventoryClass();
        private List<InventoryClass> _invClss = new List<InventoryClass>();

        private string headSqlCmd()
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append("select cInvCCode, cInvCName, iInvCGrade, bInvCEnd ");
            sqlcmd.Append(" from InventoryClass where 1 = 1 ");
            return sqlcmd.ToString();
        }
        private void invClsMapper(InventoryClass invCls,IDataReader row)
        {
            invCls.invClsCode = row.GetString("cInvCCode");
            invCls.invClsName = row.GetString("cInvCName");
            invCls.iGrade = Convert.ToInt32(row.GetValue("iInvCGrade"));
            invCls.isEnd = row.GetBoolean("bInvCEnd");
            if (invCls.iGrade > 1) {
                invCls.upInvClsCode = invCls.invClsCode.Substring(0, 2 * (invCls.iGrade - 1));
                invCls.upInvCls = getSingle(invCls.upInvClsCode);
            }
        }

        private string whereStr(InventoryClass searchKey)
        {
            StringBuilder wStr = new StringBuilder();
            if (searchKey != null)
            {
                if (!string.IsNullOrEmpty(searchKey.invClsCode))
                    wStr.Append(" and cinvCCode = '" + searchKey.invClsCode + "'");
                if (!string.IsNullOrEmpty(searchKey.invClsName))
                    wStr.Append(" and cinvCName like '%" + searchKey.invClsName + "%'");
                if (!string.IsNullOrEmpty(searchKey.upInvClsCode))
                    wStr.Append(" and cinvCCode like '" + searchKey.upInvClsCode + "%'");
                if (searchKey.iGrade > 0)
                    wStr.Append(" and iInvCGrade = " + searchKey.iGrade);
                if (searchKey.isEnd.HasValue)
                    wStr.Append(" and bInvCEnd = '" + searchKey.isEnd.Value.ToString() + "'");
            }
            return wStr.ToString();
        }
        public u8InventoryClass() { Fields = new List<string>() { "cInvCCode"," cInvCName"," iInvCGrade"," bInvCEnd" }; }
        public override List<InventoryClass> getList(InventoryClass searchKey)
        {
            return getList(whereStr(searchKey));
        }

        public override List<InventoryClass> getList(string whereStr)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(whereStr);
            _invClss = Context.Sql(sqlcmd.ToString()).QueryMany<InventoryClass>(invClsMapper);
            return _invClss;
        }

        public override InventoryClass getSingle(string code)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(" and cinvCCode = '"+code+"'");
            _invCls = Context.Sql(sqlcmd.ToString()).QuerySingle<InventoryClass>(invClsMapper);
            return _invCls;
        }

        public override void setField(string field, string val, InventoryClass searchKey)
        {//存货分类修改不提供
         //setField(field, val, whereStr(searchKey)); 
        }
        public override void setField(string field, string val, string whereStr)
        {//存货分类修改不提供
            ;
        }

        public override object getField(string field, InventoryClass searchKey) {
            object r = null;
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" select " + field + " from InventoryClass where 1 = 1 ");
            sqlcmd.Append(whereStr(searchKey));
            r = Context.Sql(sqlcmd.ToString()).QuerySingle<dynamic>();
            return r;
        }
    }
}
