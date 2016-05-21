using System;
using System.Collections.Generic;
using System.Text;
using DataModel;
using FluentData;
using IDataAccess;

namespace DataAccess.U8
{
    public class u8Warehouse : u8<Warehouse>
    {
        private Warehouse _wareHouse;
        private List<Warehouse> _wareHouses;
        private string whereStr(Warehouse searchKey)
        {
            StringBuilder wStr = new StringBuilder();
            if (string.IsNullOrEmpty(searchKey.whCode))
                wStr.Append(" and cWhCode = '" + searchKey.whCode + "'");
            if (string.IsNullOrEmpty(searchKey.whName))
                wStr.Append(" and cWhName like '%" + searchKey.whName + "%'");
            return wStr.ToString();
        }
        private string headSqlCmd()
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append("select cWhCode,cWhName from warehouse where 1 = 1 ");
            return sqlcmd.ToString();
        }
        private void whMapper(Warehouse wh, IDataReader row)
        {
            wh.whCode = row.GetString("cWhCode");
            wh.whName = row.GetString("cWhName");
        }
        public u8Warehouse() { Fields = new List<string>() { "cWhCode", "cWhName" }; }
        public override List<Warehouse> getList(Warehouse searchKey)
        {
            return getList(whereStr(searchKey));
        }

        public override List<Warehouse> getList(string whereStr)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(whereStr);
            _wareHouses = Context.Sql(sqlcmd.ToString()).QueryMany<Warehouse>(whMapper);
            return _wareHouses;
        }

        public override Warehouse getSingle(string code)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(" and cWhCode = '" + code + "'");
            _wareHouse = Context.Sql(sqlcmd.ToString()).QuerySingle<Warehouse>(whMapper);
            return _wareHouse;
        }

        public override void setField(string field, string val, string whereStr)
        {
            //仓库档案修改不提供
        }

        public override void setField(string field, string val, Warehouse searchKey)
        {//仓库档案修改不提供
         //setField(field, val, whereStr(searchKey)); 
        }
        public override object getField(string field, Warehouse searchKey)
        {
            object r = null;
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" select " + field + " from Warehouse where 1 = 1 ");
            sqlcmd.Append(whereStr(searchKey));
            r = Context.Sql(sqlcmd.ToString()).QuerySingle<dynamic>();
            return r;
        }
    }
}
