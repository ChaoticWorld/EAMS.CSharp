using System;
using System.Collections.Generic;
using System.Text;
using DataModel;
using FluentData;
using IDataAccess;

namespace DataAccess.U8
{
    public class u8Unit : u8Base, IDbAccess<UnitBase>
    {
        private UnitBase _unit = new UnitBase();
        private List<UnitBase> _units = new List<UnitBase>();

        private string whereStr(UnitBase searchKey)
        {
            StringBuilder wStr = new StringBuilder();
            if (string.IsNullOrEmpty(searchKey.UnitCode))
                wStr.Append(" and cComunitcode = '" + searchKey.UnitCode + "'");
            if (string.IsNullOrEmpty(searchKey.UnitName))
                wStr.Append(" and cComUnitName like '%" + searchKey.UnitName + "%'");
            return wStr.ToString();
        }
        private string headSqlcmd()
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append("select cComunitCode,cComUnitName,iChangRate,bMainUnit ");
            sqlcmd.Append(" from ComputationUnit where 1 = 1 ");
            return sqlcmd.ToString();
        }
        private void UnitMapper(UnitBase u, IDataReader row)
        {
            u.UnitCode = row.GetString("cComUnitCode");
            u.UnitName = row.GetString("cComUnitName");
            u.rate = Convert.ToDouble(row.GetDecimal("iChangRate"));
            u.isDefault = row.GetBoolean("bMainUnit");
        }
        public u8Unit() {
            Fields = new List<string>() { "cComunitCode"," cComUnitName"," iChangRate"," bMainUnit" }; }
        public List<UnitBase> getList(UnitBase searchKey)
        {
            return getList(whereStr(searchKey));
        }

        public List<UnitBase> getList(string whereStr)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlcmd());
            sqlcmd.Append(whereStr);
            _units = Context.Sql(sqlcmd.ToString()).QueryMany<UnitBase>(UnitMapper);
            return _units;
        }

        public UnitBase getSingle(string code)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlcmd());
            sqlcmd.Append(" and cComUnitCode = '" + code + "'");
            _unit = Context.Sql(sqlcmd.ToString()).QuerySingle<UnitBase>(UnitMapper);
            return _unit;
        }

        public void setField(string field, string val, UnitBase searchKey)
        {//计量单位修改不提供
         //setField(field, val, whereStr(searchKey)); 
        }
        public void setField(string field, string val, string whereStr)
        {//计量单位修改不提供
            ;
        }
        public object getField(string field, UnitBase searchKey)
        {
            object r = null;
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" select " + field + " from ComputationUnit where 1 = 1 ");
            sqlcmd.Append(whereStr(searchKey));
            r = Context.Sql(sqlcmd.ToString()).QuerySingle<dynamic>();
            return r;
        }
    }
}
