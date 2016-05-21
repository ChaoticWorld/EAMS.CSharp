using System;
using System.Collections.Generic;
using System.Text;
using DataModel;
using FluentData;
using IDataAccess;

namespace DataAccess.U8
{
    public class u8DispatchDetail :u8<VouchDetail>
    {
        private VouchDetail _detail;
        private List<VouchDetail> _details;

        private string whereStr(IVouchDetail searchKey)
        {
            StringBuilder wStr = new StringBuilder();
            if (searchKey.Mid > 0)
                wStr.Append(" and dlid = '"+searchKey.Mid+"'");
            if (searchKey.Did > 0)
                wStr.Append(" and idlsid = '" + searchKey.Did + "'");
            if (searchKey.autoid > 0)
                wStr.Append(" and autoid = '"+searchKey.autoid+"'");
            if (searchKey.inventory != null)
            {
                if (!string.IsNullOrEmpty(searchKey.inventory.InvCode))
                    wStr.Append(" and cInvCode = '" + searchKey.inventory.InvCode + "'");
                if (!string.IsNullOrEmpty(searchKey.inventory.InvName))
                    wStr.Append(" and cInvName like '%" + searchKey.inventory.InvName + "%'");
            }
            if (searchKey.warehouse != null)
            {
                if (!string.IsNullOrEmpty(searchKey.warehouse.whCode))
                    wStr.Append(" and cWhCode = '" + searchKey.warehouse.whCode + "'");
            }
            if (!string.IsNullOrEmpty(searchKey.Memo))
                wStr.Append(" and cMemo like '" + searchKey.Memo + "'");

            return wStr.ToString();
        }
        private string headSqlCmd() {
            sqlcmd = new StringBuilder();
            sqlcmd.Append("select  autoid,dlid,idlsid,cwhcode,cinvcode,iQuantity,iNum ");
            sqlcmd.Append(" , iTaxUnitPrice, iMoney, cMemo ");
            sqlcmd.Append(" from DispatchLists where 1 = 1 ");
            return sqlcmd.ToString();
        }
        private void vdMapper(VouchDetail vd, IDataReader row)
        {
            vd.autoid = Convert.ToInt64(row.GetValue("autoid"));
            vd.Mid = Convert.ToInt32(row.GetValue("dlid"));
            vd.Did = Convert.ToInt32(row.GetValue("idlsid"));
            vd.inventory = new u8Inventory().getSingle(row.GetString("cInvCode"));
            vd.warehouse = new u8Warehouse().getSingle(row.GetString("cwhcode"));
            vd.quantity = new Quantity()
            {
                iQuantity = Convert.ToDouble(row.GetValue("iQuantity")),
                iNum = Convert.ToDouble(row.GetValue("iNum"))
            };
            vd.iPrice = Convert.ToDouble(row.GetValue("iTaxUnitPrice"));
            vd.iSum = Convert.ToDouble(row.GetValue("iMoney"));
            vd.Memo = row.GetString("cMemo");
        }
        public u8DispatchDetail() {
            Fields = new List<string>() {  "autoid","dlid","idlsid","cwhcode","cinvcode","iQuantity","iNum "," iTaxUnitPrice"," iMoney"," cMemo" }; }
        public override List<VouchDetail> getList(VouchDetail searchKey)
        {
            return getList(whereStr(searchKey));
        }

        public override List<VouchDetail> getList(string whereStr)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(whereStr);
            _details = Context.Sql(sqlcmd.ToString()).QueryMany<VouchDetail>(vdMapper);
            return _details;
        }

        public override VouchDetail getSingle(string code)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(" and idlsid = '" + code + "'");
            _detail = Context.Sql(sqlcmd.ToString()).QuerySingle<VouchDetail>(vdMapper);
            return _detail;
        }

        public override void setField( string val, string whereStr, string field = "cMemo")
        {
            //only Modify Memo
            field = "cMemo";
            sqlcmd = new StringBuilder();
            sqlcmd.Append("Update DispatchLists");
            sqlcmd.Append(" set " + field + " = '" + val + "'");
            sqlcmd.Append(" where 1 = 1 ");
            sqlcmd.Append(whereStr);
            Context.Sql(sqlcmd.ToString()).Execute();
        }

        public override void setField(string field, string val, VouchDetail searchKey)
        {
            //only Modify Memo
            field = "cMemo";
            setField(field,val,whereStr(searchKey));
        }
        public override object getField(string field, VouchDetail searchKey)
        {
            object r = null;
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" select " + field + " from dispatchlists where 1 = 1 ");
            sqlcmd.Append(whereStr(searchKey));
            r = Context.Sql(sqlcmd.ToString()).QuerySingle<dynamic>();
            return r;
        }
    }
}
