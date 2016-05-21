using System;
using System.Collections.Generic;
using System.Text;
using DataModel;
using FluentData;
using IDataAccess;

namespace DataAccess.U8
{
    public class u8CurrentStock : u8<CurrentStock>
    {
        private CurrentStock _currStock = new CurrentStock();
        private List<CurrentStock> _currStocks = new List<CurrentStock>();

        public override List<CurrentStock> getList(CurrentStock searchKey)
        {
            return getList(whereStr(searchKey));
        }
        private string whereStr(CurrentStock searchKey)
        {
            StringBuilder wStr = new StringBuilder();
            wStr.Append(" and iquantity <> 0 ");
            if (null != searchKey.inventory && string.IsNullOrEmpty(searchKey.inventory.InvCode))
                wStr.Append(" and cInvCode = '" + searchKey.inventory.InvCode + "'");
            if (null != searchKey.wareHouse && string.IsNullOrEmpty(searchKey.wareHouse.whCode))
                wStr.Append(" and cWhCode = '" + searchKey.wareHouse.whCode + "'");
            return wStr.ToString();
        }
        public u8CurrentStock() { Fields = new List<string>() { "cWhCode"," cInvCode"," iQuantity"," iNum" }; }
        public override List<CurrentStock> getList(string whereStr)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(whereStr);
            _currStocks = Context.Sql(sqlcmd.ToString()).QueryMany<CurrentStock>(currStockMapper);
            return _currStocks;
        }

        public override void setField(string field, string val, CurrentStock searchKey)
        {//库存修改不提供
         //setField(field, val, whereStr(searchKey)); 
        }
        public override void setField(string field, string val, string whereStr)
        {//库存修改不提供
            ;
        }
        public  CurrentStock getSingle(string whcode, string invcode)
        {
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(" and cInvCode = '" + invcode + "'");
            sqlcmd.Append(" and cWhCode = '" + whcode + "'");
            _currStock = Context.Sql(sqlcmd.ToString()).QuerySingle<CurrentStock>(currStockMapper);
            return new CurrentStock();// _currStock;
        }
        /// <summary>
        /// 返回当前库存
        /// </summary>
        /// <param name="code">whcode,invcode</param>
        /// <returns></returns>
        public override CurrentStock getSingle(string code)
        {
            string[] whinv = code.Split(',');
            if (whinv != null)
                return getSingle(whinv[0], whinv[1]);
            else return null;
        }

        private string headSqlCmd()
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append("select cWhCode,cInvCode,iQuantity,iNum");
            sqlcmd.Append(" from CurrentStock where 1 = 1 ");
            return sqlcmd.ToString();
        }
        private void currStockMapper(CurrentStock cs, IDataReader row)
        {
            u8Warehouse u8wh = new u8Warehouse();
            Warehouse w = u8wh.getSingle(row.GetString("cWhCode"));

            u8Inventory u8Inv = new u8Inventory();
            Inventory inv = u8Inv.getSingle(row.GetString("cInvCode"));
            Quantity q = new Quantity() { iQuantity = Convert.ToDouble(row.GetValue("iQuantity")), iNum = Convert.ToDouble(row.GetValue("iNum")) };
            cs.inventory = inv;
            cs.wareHouse = w;
            cs.quantity = q;
        }
        public override object getField(string field, CurrentStock searchKey)
        {
            object r = null;
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" select " + field + " from CurrentStock where 1 = 1 ");
            sqlcmd.Append(whereStr(searchKey));
            r = Context.Sql(sqlcmd.ToString()).QuerySingle<dynamic>();
            return r;
        }
    }
}
