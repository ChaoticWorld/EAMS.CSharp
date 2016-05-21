using System;
using System.Collections.Generic;
using System.Text;
using DataModel;
using FluentData;
using IDataAccess;
namespace DataAccess.U8
{
    public class u8Vendor : u8<Vendor>
    {
        public u8Vendor getInstance() {
            return new u8Vendor();
        }
        private Vendor _Vendor = new Vendor();
        private List<Vendor> _Vendors = new List<Vendor>();
        private string whereStr(Vendor searchKey)
        {
            if (searchKey == null) return string.Empty;
            StringBuilder wStr = new StringBuilder();
            if (!string.IsNullOrEmpty(searchKey.Code))
                wStr.Append(" and cvencode = '" + searchKey.Code + "'");
            if (!string.IsNullOrEmpty(searchKey.Name))
                wStr.Append(" and cvenname like '%" + searchKey.Name + "%'");
            if (searchKey.corpCls != null && !string.IsNullOrEmpty(searchKey.corpCls.corpClsCode))
                wStr.Append(" and cvccode = '" + searchKey.corpCls.corpClsCode + "'");
            var contactInfo = searchKey.contactInfos.Find(f => f.isdefault);
            if (contactInfo != null)
            {
                if (!string.IsNullOrEmpty(contactInfo.shipAddress))
                    wStr.Append(" and cvenAddress like '%" + contactInfo.shipAddress + "%'");
                if (!string.IsNullOrEmpty(contactInfo.Address))
                    wStr.Append(" and cvenAddress like '%" + contactInfo.Address + "%'");
                if (!string.IsNullOrEmpty(contactInfo.phone))
                    wStr.Append(" and cvenPhone like '%" + contactInfo.phone + "%'");
                if (!string.IsNullOrEmpty(contactInfo.mobile))
                    wStr.Append(" and cvenHand like '%" + contactInfo.mobile + "%'");
                if (!string.IsNullOrEmpty(contactInfo.Email))
                    wStr.Append(" and ccusEmail like '%" + contactInfo.Email + "%'");
            }
            if (null != searchKey.district)
                wStr.Append(" and cdccode like '" + searchKey.district.dcCode + "'");

            return wStr.ToString();
        }

        private string headSqlCmd()
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append("select cvencode, cvenname, cvenAddress ");
            sqlcmd.Append(" , cvenphone, cvenhand, cvenfax ");
            sqlcmd.Append(" , dvenDevDate, cCreatePerson, dModifyDate, cModifyPerson ");
            sqlcmd.Append(" , cDCCode, cVCCode ");
            sqlcmd.Append(" from Vendor where 1 = 1 ");
            return sqlcmd.ToString();
            //return string.Empty;
        }
        private void VendorMapper(Vendor ven, IDataReader row)
        {
            ven.Code = row.GetString("cvencode");
            ven.Name = row.GetString("cvenname");
            ven.contactInfos =
                (ven.contactInfos == null ? new List<ContactInfoBase>() : ven.contactInfos);
            ven.contactInfos.Add(new ContactInfoBase()
            {
                shipAddress = row.GetString("cvenAddress"),
                isdefault = true,
                phone = row.GetString("cvenphone"),
                mobile = row.GetString("cvenhand")
            });
            ven.corpCreateDay = row.GetDateTime("dVenDevDate");
            ven.CreateDate = row.GetDateTime("dVenDevDate");
            ven.CreateMan = row.GetString("cCreatePerson");
            ven.ModifyDate = row.GetDateTime("dModifyDate");
            ven.ModifyMan = row.GetString("cModifyPerson");
        }
        public u8Vendor() { Fields = new List<string>() { "cvencode"," cvenname"," cvenAddress"," cvenphone"," cvenhand"," cvenfax  "," dvenDevDate"," cCreatePerson"," dModifyDate"," cModifyPerson "," cDCCode"," cVCCode"}; }
        public override List<Vendor> getList(Vendor searchKey)
        {
            return getList(whereStr(searchKey));
        }

        public override List<Vendor> getList(string whereStr)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(whereStr);
            _Vendors = Context.Sql(sqlcmd.ToString()).QueryMany<Vendor>(VendorMapper);
            return _Vendors;
        }

        public override Vendor getSingle(string code)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(headSqlCmd());
            sqlcmd.Append(" and cvenCode = '" + code + "'");
            var _Vendor = Context.Sql(sqlcmd.ToString()).QuerySingle<Vendor>(VendorMapper);
            return _Vendor;
        }

        public override void setField(string field, string val, string whereStr)
        {
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" Update Vendor ");
            sqlcmd.Append(" set " + field + " = '" + val + "'");
            sqlcmd.Append(" where 1 = 1 ");
            sqlcmd.Append(whereStr);
            Context.Sql(sqlcmd.ToString()).Execute();
        }

        public override void setField(string field, string val, Vendor searchKey)
        {
            setField(field,val,whereStr(searchKey));
        }
        public override object getField(string field, Vendor searchKey)
        {
            object r = null;
            sqlcmd = new StringBuilder();
            sqlcmd.Append(" select " + field + " from Vendor where 1 = 1 ");
            sqlcmd.Append(whereStr(searchKey));
            r = Context.Sql(sqlcmd.ToString()).QuerySingle<dynamic>();
            return r;
        }
    }
}