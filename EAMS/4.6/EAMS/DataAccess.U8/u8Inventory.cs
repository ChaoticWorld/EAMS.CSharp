using System;
using System.Collections.Generic;
using System.Text;
using DataModel;
using FluentData;
using IDataAccess;


namespace DataAccess.U8
{
    public class u8Inventory : u8<Inventory>
    {
        private Inventory _inventory { get; set; }
        private List<Inventory> _inventorys { get; set; }

        private string whereStr(Inventory searchKey) {
            StringBuilder wStr = new StringBuilder();
            if (!string.IsNullOrEmpty(searchKey.InvCode))
                wStr.Append(" and cinvcode = '" + searchKey.InvCode + "'");
            if (!string.IsNullOrEmpty(searchKey.InvName))
                wStr.Append(" and cinvname like '%" + searchKey.InvName + "%'");
            if (!string.IsNullOrEmpty(searchKey.InvStd))
                wStr.Append(" and cinvstd like '%" + searchKey.InvStd + "%'");
            if (!string.IsNullOrEmpty(searchKey.cInvClassCode))
                wStr.Append(" and cinvccode = '"+searchKey.cInvClassCode+"'");
            return wStr.ToString();
        }
        private string headSqlCmd()
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" select cInvCode,cInvName,cInvStd,cInvCCode,cComUnitCode");
            sqlcmd.Append(" from Inventory where 1 = 1");
            return sqlcmd.ToString();
        }
        private void InvMapper(Inventory inv, IDataReader row)
        {
            inv.InvCode = row.GetString("cInvCode");
            inv.InvName = row.GetString("cInvName");
            inv.InvStd = row.GetString("cInvStd");
            inv.cInvClassCode = row.GetString("cInvCCode");
            inv.cUnitCode = row.GetString("cComUnitCode");

            inv.Units = (inv.Units == null ? new List<UnitBase>() : inv.Units);
            u8Unit u8unit = new u8Unit();
            var unit = u8unit.getSingle(inv.cUnitCode);
            if (unit != null)
            {
                inv.Units.Add(unit);
                inv.cInvClass = unit.UnitName;
            }
            inv.invClass = (inv.invClass == null ? new List<InventoryClass>() : inv.invClass);
            u8InventoryClass u8ic = new u8InventoryClass();
            var invcls = u8ic.getSingle(inv.cInvClassCode);
            if (null != invcls)
            {
                inv.cInvClass = invcls.invClsName;
                inv.invClass.Add(invcls);
            }
        }
        public u8Inventory() { Fields = new List<string>() { "cInvCode"," cInvName"," cInvStd"," cInvCCode"," cComUnitCode" }; }
        public override List<Inventory> getList(Inventory searchKey)
        {
            return getList(whereStr(searchKey));
        }

        public override List<Inventory> getList(string whereStr)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(whereStr);
            _inventorys = Context.Sql(sqlcmd.ToString()).QueryMany<Inventory>(InvMapper);
            return _inventorys;
        }

        public override Inventory getSingle(string code)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(" and cInvCode = '" + code + "'");

            _inventory = Context.Sql(sqlcmd.ToString()).QuerySingle<Inventory>(InvMapper);
            return _inventory;
        }

        public override void setField(string field, string val, string whereStr)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" Update Inventory ");
            sqlcmd.Append(" set " + field + "='" + val + "'");
            sqlcmd.Append(" where 1 = 1 ");
            sqlcmd.Append(whereStr);
            Context.Sql(sqlcmd.ToString()).Execute();
        }

        public override void setField(string field, string val, Inventory searchKey)
        {
            setField(field, val, whereStr(searchKey));
        }
        public override object getField(string field, Inventory searchKey)
        {
            object r = null;
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" select " + field + " from  Inventory where 1 = 1 ");
            sqlcmd.Append(whereStr(searchKey));
            r = Context.Sql(sqlcmd.ToString()).QuerySingle<dynamic>();
            return r;
        }
    }
}
