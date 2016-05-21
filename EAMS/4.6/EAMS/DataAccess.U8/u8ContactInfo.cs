using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using FluentData;
using IDataAccess;

namespace DataAccess.U8
{
    public class u8ContactInfo : u8<ContactInfoBase>
    {//未处理
        private ContactInfoBase _contactInfo = new ContactInfoBase();
        private List<ContactInfoBase> _contactInfos = new List<ContactInfoBase>();
        private string whereStr(ContactInfoBase searchKey)
        { return string.Empty; }
        private string headSqlCmd()
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append("select ccuscode,ccusname,ccusOAddress");
            sqlcmd.Append(" ,ccusphone,ccushand,ccusfax ");//其它自定义项
            sqlcmd.Append(" ,ccusDevDate,cCreatePerson,cModifyPerson,dModifyDate ");
            sqlcmd.Append(" ,cDCCode,cCCCode ");
            sqlcmd.Append(" from  where 1 = 1 ");
            //return sqlcmd.ToString();
            return string.Empty;
        }
        private void ContactInfoMapper(ContactInfoBase contactInfo, IDataReader row)
        { }
        public u8ContactInfo() { Fields = new List<string>() { }; }
        public override List<ContactInfoBase> getList(ContactInfoBase seachkey)
        {
            throw new NotImplementedException();
        }

        public override List<ContactInfoBase> getList(string code)
        {
            throw new NotImplementedException();
        }

        public override ContactInfoBase getSingle(string code)
        {
            throw new NotImplementedException();
        }

        public override void setField(string field, string val, string whereStr)
        {
            throw new NotImplementedException();
        }

        public override void setField(string field, string val, ContactInfoBase seachkey)
        {
            throw new NotImplementedException();
        }
        public override object getField(string field, ContactInfoBase searchKey)
        {
            object r = null;
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" select " + field + " from InventoryClass where 1 = 1 ");
            sqlcmd.Append(whereStr(searchKey));
            r = Context.Sql(sqlcmd.ToString()).QuerySingle<dynamic>();
            return r;
        }
    }
}
