using System;
using System.Collections.Generic;
using System.Text;
using DataModel;
using FluentData;
using IDataAccess;

namespace DataAccess.U8
{
    public class u8Customer : u8<Customer>
    {
        private Customer _customer = new Customer();
        private List<Customer> _customers = new List<Customer>();
        public u8Customer() : base()
        {
            Fields = new List<string>() { "ccuscode", "ccusname", "ccusOAddress ", "ccusphone", "ccushand", "ccusfax  ", "dcusDevDate", "cCreatePerson", "dModifyDate", "cModifyPerson" };
        }
        private string whereStr(Customer searchKey)
        {
            if (searchKey == null) return string.Empty;
            StringBuilder wStr = new StringBuilder();
            if (!string.IsNullOrEmpty(searchKey.Code))
                wStr.Append(" and ccuscode like '%" + searchKey.Code + "%' ");
            if (!string.IsNullOrEmpty(searchKey.Name))
                wStr.Append(" and ccusname like '%" + searchKey.Name + "%' ");
            if (searchKey.corpCls != null && !string.IsNullOrEmpty(searchKey.corpCls.corpClsCode))
                wStr.Append(" and ccccode = '" + searchKey.corpCls.corpClsCode + "' ");
            if (searchKey.contactInfos != null)
            {
                var contactInfo = searchKey.contactInfos.Find(f => f.isdefault);
                if (contactInfo != null)
                {
                    if (!string.IsNullOrEmpty(contactInfo.shipAddress))
                        wStr.Append(" and ccusOAddress like '%" + contactInfo.shipAddress + "%' ");
                    if (!string.IsNullOrEmpty(contactInfo.Address))
                        wStr.Append(" and ccusAddress like '%" + contactInfo.Address + "%' ");
                    if (!string.IsNullOrEmpty(contactInfo.phone))
                        wStr.Append(" and ccusPhone like '%" + contactInfo.phone + "%' ");
                    if (!string.IsNullOrEmpty(contactInfo.mobile))
                        wStr.Append(" and ccusHand like '%" + contactInfo.mobile + "%' ");
                    if (!string.IsNullOrEmpty(contactInfo.Email))
                        wStr.Append(" and ccusEmail like '%" + contactInfo.Email + "%' ");
                }
            }
            if (null != searchKey.district)
                wStr.Append(" and cdccode like '" + searchKey.district.dcCode + "' ");
            if (null != searchKey.contacts)
            {
                var contactMan = searchKey.contacts.Find(f => f.isDefault).Name;
                if (!string.IsNullOrEmpty(contactMan))
                    wStr.Append(" and ccusperson like '%" + contactMan + "%' ");
            }
            return wStr.ToString();
        }
        public override List<Customer> getList(Customer searchKey)
        {
            return getList(whereStr(searchKey));
        }

        public override List<Customer> getList(string whereStr)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(whereStr);
            sqlcmd.Append(AuthenWhereStr(null));
            _customers = Context.Sql(sqlcmd.ToString()).QueryMany<Customer>(CusMapper);
            return _customers;
        }
        /// <summary>
        /// 客户权限查寻条件
        /// </summary>
        /// <param name="userName">员工名</param>
        /// <returns></returns>
        private string AuthenWhereStr(string userName=null) {
            StringBuilder r = new StringBuilder();
            string authenSql = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                authenSql = "select cACCode from aa_holdauth where 1 = 1 and cBusObId = N'customer' And isUserGroup = 0 and cFuncID <> 'N' and cUserId in ( "
                    + "SELECT cUser_Id FROM UFSYSTEM.DBO.UA_USER WHERE CUSER_NAME like '@pName')";
                var authGroupId = Context.Sql(authenSql).Parameter("pName",userName).QuerySingle<string>();
                authenSql = "select id from AA_AuthClass where cBusObId = N'customer' and cACCode like '"+authGroupId+"%' order by cACCode ";
                var authIDs = Context.Sql(authenSql).QueryMany<string>();
                if (authIDs != null && authIDs.Count > 0)
                {
                    r.Append(" and iid in (");
                    foreach (string i in authIDs)
                    {
                        r.Append("'" + i + "',");
                    }
                    r.Remove(r.Length - 1, 1);
                    r.Append(") ");
                }
            }
            return r.ToString();
        }
        public override void setField(string field, string val, Customer searchKey)
        {
            setField(field, val, whereStr(searchKey));
        }
        public override void setField(string field, string val, string whereStr)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" update Vendor ");
            sqlcmd.Append(" set " + field + "='" + val + "'");
            sqlcmd.Append(" where 1 = 1 ");
            sqlcmd.Append(whereStr);
            Context.Sql(sqlcmd.ToString()).Execute();
        }

        public override Customer getSingle(string code)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(" and cCusCode = '" + code + "'");
            _customer = Context.Sql(sqlcmd.ToString()).QuerySingle<Customer>(CusMapper);
            return _customer;
        }
        private string headSqlCmd()
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append("select ccuscode,ccusname,ccusOAddress");
            sqlcmd.Append(" ,ccusphone,ccushand,ccusfax ");//其它自定义项
            sqlcmd.Append(" ,dcusDevDate,cCreatePerson,dModifyDate,cModifyPerson ");
            sqlcmd.Append(" ,cDCCode,cCCCode,ccusperson");
            sqlcmd.Append(" from customer where 1 = 1 ");
            return sqlcmd.ToString();
        }
        private void CusMapper(Customer cus, IDataReader row)
        {
            cus.Code = row.GetString("ccusCode");
            cus.Name = row.GetString("ccusName");
            cus.contactInfos = 
                (cus.contactInfos == null ? new List<ContactInfoBase>() : cus.contactInfos);
            cus.contactInfos.Add(new ContactInfoBase()
            {
                shipAddress = row.GetString("ccusOAddress"),
                isdefault = true,
                phone = row.GetString("ccusphone"),
                mobile = row.GetString("ccushand")
            });
            cus.corpCreateDay = row.GetDateTime("dCusDevDate");
            cus.CreateDate = row.GetDateTime("dCusDevDate");
            cus.CreateMan = row.GetString("cCreatePerson");
            cus.ModifyDate = row.GetDateTime("dModifyDate");
            cus.ModifyMan = row.GetString("cModifyPerson");

            cus.district = new u8District().getSingle(row.GetString("cDCCode"));
            cus.contacts = new List<Contact>();
            cus.contacts.Add(new Contact() { Name = row.GetString("ccusperson"), isDefault = true });
            //cus.corpCls = new corporatioClass() { corpClsCode = row.GetString("cCCCode") };
            //其它自定义项
            }
        public override object getField(string field, Customer searchKey)
        {
            object r = null;
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" select " + field + " from Customer where 1 = 1 ");
            sqlcmd.Append(whereStr(searchKey));
            r = Context.Sql(sqlcmd.ToString()).QuerySingle<dynamic>();
            return r;
        }
    }
}
