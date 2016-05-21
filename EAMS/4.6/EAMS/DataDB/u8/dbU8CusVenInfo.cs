using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DataDB.ModelBase;
using System.Data.Entity;
using System.Data.SqlClient;
//处理客户，客户联系人，供应商信息
namespace DataDB.u8
{
    /// <summary>
    /// 方法操作
    /// 处理客户，客户联系人，供应商信息
    /// </summary>
    public partial class dbU8
    {
        /// <summary>
        /// 依据单位编码，获得模型数据列表
        /// </summary>
        /// <param name="key">单位编码</param>
        /// <param name="cusList">客户列表</param>
        /// <param name="conList">联系人列表</param>
        /// <param name="venList">供应商列表</param>
        public void getModelWithSearchKey(string key, out List<Customer> cusList, out List<Contact> conList, out List<Vendor> venList)
        {
            cusList = null; conList = null; venList = null;
            Customer newCus = new Customer();
            List<Customer> con2cusList = new List<Customer>();
            Contact newCon = new Contact();

            if (key.Equals("_AllCustomers_"))
            {
                cusList = getCustomersAll();
                return;
            }
            else
            {               
                #region 客户联系人
                if (isExistsKey(phoneC: u8.phoneClass.CustomerContact, key: key))
                {
                    //获得指定来电号码的联系人列表
                    conList = getContacts(key: key);
                    if (conList != null)
                    {
                        foreach (Contact conItem in conList)
                        {
                            //获得此联系人对应的客户
                            con2cusList = getCustomers(code: conItem.DWCode);
                            if (null != con2cusList)
                            {
                                if (cusList == null) cusList = new List<Customer>();
                                foreach (Customer n in con2cusList)
                                    //客户列表中不存在，增加到客户列表
                                    if (!cusList.Exists(cl => cl.cCusCode == n.cCusCode))
                                        cusList.Add(n);
                            }
                        }
                    }
                }
                #endregion

                #region 客户
                if (isExistsKey(u8.phoneClass.Customer, key))
                {
                    //获得指定来电号码的客户列表
                    List<Customer> getCuss = getCustomers(key: key);
                    List<Contact> getCons = new List<Contact>();
                    if (getCuss != null)
                    {
                        foreach (Customer cusItem in getCuss)
                        {
                            newCus = cusItem;
                            //客户列表中不存在，增加到客户列表
                            if (null == cusList) cusList = new List<Customer>();
                            if (!cusList.Exists(ci => ci.cCusCode == newCus.cCusCode))
                                cusList.Add(newCus);
                            //获得此客户的联系人列表
                            getCons = getContacts(cuscode: newCus.cCusCode);
                            if (null != getCons)
                            {
                                if (null == conList) conList = new List<Contact>();
                                foreach (Contact conItem in getCons)
                                {
                                    //此客户联系人不存在，增加到联系人列表
                                    if (!conList.Exists(ci => ci.name == conItem.name))
                                        conList.Add(conItem);
                                }
                            }
                        }
                    }
                    return;
                }
                #endregion

                #region 供应商
                if (isExistsKey(u8.phoneClass.Vendor, key))
                {
                    //获得指定来电号码的供应商列表
                    venList = getVendors(key: key);
                    if (venList != null)
                    {
                        foreach (Vendor venItem in venList)
                        {
                            //供应商列表中不存在，增加到供应商列表
                            if (!venList.Exists(vi => vi.cVenCode == venItem.cVenCode))
                            {
                                venList.Add(venItem);
                                //将U8供应商联系人字段填充到供应商联系人列表
                                if (null == conList) conList = new List<Contact>();
                                conList.Add(new Contact { name = venItem.ContactName });
                            }
                        }
                    }
                    return;
                }
                #endregion
            }
        }

        /// <summary>
        /// 依据单位编码，获得模型数据列表
        /// </summary>
        /// <param name="code">单位编码</param>
        /// <param name="cusList">客户列表</param>
        /// <param name="conList">联系人列表</param>
        /// <param name="venList">供应商列表</param>
        public void getModelWithCode(string code,  out List<Customer> cusList, out List<Contact> conList, out List<Vendor> venList)
        {
            cusList = null; conList = null; venList = null;
            Customer newCus = new Customer();
            List<Customer> con2cusList = new List<Customer>();
            Contact newCon = new Contact();
            
            #region 客户
            if (isExistsKey(u8.phoneClass.Customer, code))
            {
                //获得指定来电号码的客户列表
                List<Customer> getCuss = getCustomers(code: code);
                List<Contact> getCons = new List<Contact>();

                if (getCuss != null)
                {
                    foreach (Customer cusItem in getCuss)
                    {
                        newCus = cusItem;
                        //客户列表中不存在，增加到客户列表
                        if (null == cusList) cusList = new List<Customer>();
                        if (!cusList.Exists(ci => ci.cCusCode == newCus.cCusCode))
                            cusList.Add(newCus);
                        //获得此客户的联系人列表
                        getCons = getContacts(cuscode: newCus.cCusCode);
                        if (null != getCons)
                        {
                            if (null == conList) conList = new List<Contact>();
                            foreach (Contact conItem in getCons)
                            {
                                //此客户联系人不存在，增加到联系人列表
                                if (!conList.Exists(ci => ci.name == conItem.name))
                                    conList.Add(conItem);
                            }
                        }
                    }
                }
                else return;
            }
            #endregion
           
            #region 客户联系人
            if (isExistsKey(phoneC: u8.phoneClass.CustomerContact, key:code))
            {   
                //获得指定来电号码的联系人列表
                conList = getContacts(cuscode: code);
                if (conList != null)
                foreach (Contact conItem in conList)
                {
                    //获得此联系人对应的客户
                    con2cusList = getCustomers(code: conItem.DWCode);
                    if (null != con2cusList)
                    {
                        if (cusList == null) cusList = new List<Customer>();
                        foreach (Customer n in con2cusList)
                            //客户列表中不存在，增加到客户列表
                            if (!cusList.Exists(ci => ci.cCusCode == n.cCusCode))
                                cusList.Add(n);
                    }
                }
            }
            #endregion
            
            #region 供应商
            if (isExistsKey(u8.phoneClass.Vendor, code))
            {
                //获得指定来电号码的供应商列表
                venList = getVendors(vencode: code);
                foreach (Vendor venItem in venList)
                {
                    //供应商列表中不存在，增加到供应商列表
                    if (!venList.Exists(vi => vi.cVenCode == venItem.cVenCode))
                    {
                        venList.Add(venItem);
                        //将U8供应商联系人字段填充到供应商联系人列表
                        if (null == conList) conList = new List<Contact>();
                        conList.Add(new Contact { name = venItem.ContactName });
                    }
                }
                return;
            }
            #endregion

        }
        /// <summary>
        /// 依据来电号码，获得模型数据列表
        /// </summary>
        /// <param name="_callid">来电号码</param>
        /// <param name="cusList">客户列表</param>
        /// <param name="conList">联系人列表</param>
        /// <param name="venList">供应商列表</param>
        public void getModelWithCallID(string _callid, out List<Customer> cusList, out List<Contact> conList, out List<Vendor> venList)
        {
            cusList = null;
            conList = null;
            venList = null;
            Customer newCus = new Customer();
            List<Customer> con2cusList = new List<Customer>();
            Contact newCon = new Contact();
            #region 客户联系人
            if (isExistsCallID(u8.phoneClass.CustomerContact, _callid))
            {
                //获得指定来电号码的联系人列表
                conList = getContacts(sphone: _callid);
                foreach (Contact conItem in conList)
                {
                    //获得此联系人对应的客户
                    con2cusList = getCustomers(code: conItem.DWCode);
                    if (null != con2cusList)
                    {
                        if (cusList == null) cusList = new List<Customer>();
                        foreach (Customer n in con2cusList)
                            //客户列表中不存在，增加到客户列表
                            if (!cusList.Exists(ci => ci.cCusCode == n.cCusCode))
                                cusList.Add(n);
                    }
                }
            }
            #endregion
            #region 客户
            if (isExistsCallID(u8.phoneClass.Customer, _callid))
            {
                //获得指定来电号码的客户列表
                List<Customer> getCuss = getCustomers(sphone: _callid);
                List<Contact> getCons = new List<Contact>();

                foreach (Customer cusItem in getCuss)
                {
                    newCus = cusItem;
                    //客户列表中不存在，增加到客户列表
                    if (null == cusList) cusList = new List<Customer>();
                    if (!cusList.Exists(ci => ci.cCusCode == newCus.cCusCode))
                        cusList.Add(newCus);
                    //获得此客户的联系人列表
                    getCons = getContacts(cuscode: newCus.cCusCode);
                    if (null != getCons)
                    {
                        if (null == conList) conList = new List<Contact>();
                        foreach (Contact conItem in getCons)
                        {
                            //此客户联系人不存在，增加到联系人列表
                            if (!conList.Exists(ci => ci.name == conItem.name))
                                conList.Add(conItem);
                        }
                    }
                }
                return;
            }
            #endregion
            //供应商联系人，U871不支持
            //if (isExistsCallID(u8.phoneClass.VendorContact, _callid)) { }
            #region 供应商
            if (isExistsCallID(u8.phoneClass.Vendor, _callid))
            {
                //获得指定来电号码的供应商列表
                venList = getVendors(sphone: _callid);
                foreach (Vendor venItem in venList)
                {
                    //供应商列表中不存在，增加到供应商列表
                    if (!venList.Exists(vi => vi.cVenCode == venItem.cVenCode))
                    {
                        venList.Add(venItem);
                        //将U8供应商联系人字段填充到供应商联系人列表
                        if (null == conList) conList = new List<Contact>();
                        conList.Add(new Contact { name = venItem.ContactName });
                    }
                }
                return;
            }
            #endregion
        }

        /// <summary>
        /// 系统中是否存在来电号码
        /// </summary>
        /// <param name="phoneC"></param>
        /// <param name="_name"></param>
        /// <returns></returns>
        public bool isExistsCallID(u8.phoneClass phoneC, string _callid)
        {
            StringBuilder cmd = new StringBuilder();
            bool r = false;
            switch (phoneC)
            {
                case u8.phoneClass.Customer:
                    cmd.Clear();
                    cmd.Append("Select top 1 [cCusCode] From [Customer] where 1 = 1 ");
                    cmd.Append(" AND ([cCusPhone] like '%" + _callid + "%' OR [cCusHand] like '%" + _callid + "%' OR [cCusDefine2] like '%" + _callid + "%' OR [cCusDefine3] like '%" + _callid + "%')");
                    r = isExistSqlSelect(cmd.ToString());
                    break;
                case u8.phoneClass.CustomerContact:
                    cmd.Clear();
                    cmd.Append("Select top 1 [cContactName] From [Crm_Contact] where 1 = 1 ");
                    cmd.Append(" and ( [cOfficePhone] like '%" + _callid + "%'");
                    cmd.Append(" or [cHomePhone] like '%" + _callid + "%'");
                    cmd.Append(" or [cMobilePhone] like '%" + _callid + "%')");
                    r = isExistSqlSelect(cmd.ToString());
                    break;
                case u8.phoneClass.Vendor:
                    cmd.Clear();
                    cmd.Append("Select top 1 [cVenCode] From [Vendor] where 1 = 1 ");
                    cmd.Append(" AND ([cVenPhone]='%" + _callid + "%' OR [cVenHand]='%" + _callid + "%')");
                    r = isExistSqlSelect(cmd.ToString());
                    break;
                case u8.phoneClass.VendorContact:
                    //u8 8.71版本无供应商联系人
                    cmd.Clear();
                    r = isExistSqlSelect(cmd.ToString());
                    break;
            }
            return r;
        }
        /// <summary>
        /// 系统中是否存在搜索关键字
        /// </summary>
        /// <param name="phoneC"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool isExistsKey(u8.phoneClass phoneC, string key)
        {
            StringBuilder cmd = new StringBuilder();
            bool r = false;
            switch (phoneC)
            {
                case u8.phoneClass.Customer:
                    cmd.Clear();
                    cmd.Append("Select top 1 [cCusCode] From [Customer] where 1 = 1 ");
                    cmd.Append(" AND ([cCusPhone] like '%" + key + "%' OR [cCusHand] like '%" + key + "%' OR [cCusDefine2] like '%" + key + "%' OR [cCusDefine3] like '%" + key + "%')");
                    cmd.Append(" OR ([cCusName] like '%" + key + "%' OR [cCusAbbName] like '%" + key + "%' )");
                    cmd.Append(" OR ([cCusAddress] like '%" + key + "%' OR [cCusOAddress] like '%" + key + "%' )");
                    cmd.Append(" OR ([cCusPerson] like '%" + key + "%' OR [cCusCode] like '%" + key + "%' )");
                    r = isExistSqlSelect(cmd.ToString());
                    break;
                case u8.phoneClass.CustomerContact:
                    cmd.Clear();
                    cmd.Append("Select top 1 [cContactName] From [Crm_Contact] where 1 = 1 ");
                    cmd.Append(" and ( [cOfficePhone] like '%" + key + "%' or [cHomePhone] like '%" + key + "%' or [cMobilePhone] like '%" + key + "%')");
                    cmd.Append(" or ( [cContactName] like '%" + key + "%' or [cWorkAddress] like '%" + key + "%' or [cHomeAddress] like '%" + key + "%')");
                    r = isExistSqlSelect(cmd.ToString());
                    break;
                case u8.phoneClass.Vendor:
                    cmd.Clear();
                    cmd.Append("Select top 1 [cVenCode] From [Vendor] where 1 = 1 ");
                    cmd.Append(" AND ([cVenPhone]='%" + key + "%' OR [cVenHand]='%" + key + "%')");
                    cmd.Append(" OR ([cVenName]='%" + key + "%' OR [cVenAbbName]='%" + key + "%' OR [cVenAddress]='%" + key + "%' OR [cVenPerson]='%" + key + "%')");
                    cmd.Append(" OR [cVenCode] like '%" + key + "%'");
                    r = isExistSqlSelect(cmd.ToString());
                    break;
                case u8.phoneClass.VendorContact:
                    //u8 8.71版本无供应商联系人
                    cmd.Clear();
                    r = isExistSqlSelect(cmd.ToString());
                    break;
            }
            return r;
        }

        /// <summary>
        /// 获得指定客户编码相关的客户编码[SQL in()]字符串
        /// </summary>
        /// <param name="cusCode">指定客户编码</param>
        /// <returns></returns>
        private string getRefCusCode(string cusCode)
        {
            StringBuilder r = new StringBuilder();
            List<string> arrcusCode= getCusCode(cusCode);
            if (null != arrcusCode || arrcusCode.Count>0 )
            {
                foreach (string i in arrcusCode)
                { r.Append("'" + i + "',"); }
                return r.ToString().Remove(r.Length - 1);
            }
            else
                return "''";
        }
        /// <summary>
        /// 获得相关客户编码数组
        /// </summary>
        /// <param name="isreturnHead">True:返回为上级单位</param>
        /// <param name="code"></param>
        /// <returns></returns>
        private List<string> getCusCode(string code)
        {
            SqlCommand sqlcmd = new SqlCommand()
            {
                CommandType = basesqlcmd.CommandType,
                Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString)
            };
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            //string rCode = string.Empty;
            string s = string.Empty;
            string h = string.Empty;
            List<string> codelist = new List<string>();
            List<string> codeHlist = new List<string>();
            StringBuilder cmd = new StringBuilder();
            sqlcmd.CommandText = string.Empty;
            cmd.Append("select cus.[cCusCode],cus.[cCusHeadCode] from [Customer] as cus ");
            cmd.Append(" LEFT JOIN [Customer] as cushead on cus.cCusHeadCode = cushead.cCusHeadCode ");
            cmd.Append(" where cushead.cCusCode = '" + code + "'");
            sqlcmd.CommandText = cmd.ToString();
            if (sqlcmd.Connection.State != System.Data.ConnectionState.Open)
                sqlcmd.Connection.Open();
            if (null != sqlreader && sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            sqlreader = sqlcmd.ExecuteReader();
            while (sqlreader.Read())
            {
                s = sqlreader.GetString(sqlreader.GetOrdinal("cCusCode"));
                h = sqlreader.GetString(sqlreader.GetOrdinal("cCusHeadCode")).ToString();

                if (!codelist.Exists(cs => cs == s))
                    codelist.Add(s);

                if (!codeHlist.Exists(cs => cs == h))
                    codeHlist.Add(h);
            }
            //加入查寻编码自身
            if (!codelist.Exists(cs => cs == code))
                codelist.Add(code);
            List<string> m = new List<string>();
            foreach (string codeitem in codeHlist)
            {
                if (!codelist.Exists(c => c == codeitem))
                {
                    m = getCusCode(codeitem);
                    codelist.AddRange(m);
                }
            }

            if (sqlreader != null && !sqlreader.IsClosed)
                sqlreader.Close();
            if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                sqlcmd.Connection.Close();

            return codelist;
        }

        /// <summary>
        /// 返回客户列表
        /// </summary>
        /// <param name="sphone">来电号码</param>
        /// <param name="code">客户编码数组</param>
        /// <returns></returns>
        public List<Customer> getCustomers(string sphone = null, string code = null,string key = null)
        {
            List<Customer> r = null;
            List<Customer> middle = new List<Customer>();
            r = getCustomerOnce(sphone: sphone, code: code, key: key);
            //判断是否是有总公司记录，并补全所有客户信息记录
            if (r!=null && !r.Exists(item => item.cCusCode == item.topCusCode))
            {
                foreach (Customer c in r)
                {
                    foreach (Customer ci in getCustomerOnce(code: c.topCusCode))
                        if (!r.Exists(citem => citem.cCusCode == ci.cCusCode))
                            middle.Add(ci);
                }
                r.AddRange(middle);
            }            
            return r;
        }
        private List<Customer> getCustomerOnce(string sphone = null, string code = null,string key = null)
        {
            SqlCommand sqlcmd = new SqlCommand() {CommandType = basesqlcmd.CommandType,Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString)};
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            List<Customer> r = null;//定义返回值
            Customer cus;
            sqlcmd.CommandText = string.Empty;
            StringBuilder cmd = new StringBuilder();
            string RefCuscode = string.Empty;
            cmd.Append(" SELECT [cCusCode],[cCusName],[cCusAbbName],[cCusAddress],[cCusOAddress]");
            cmd.Append(",[cDCName],[cCCName],[cCusHeadCode],[cCusPerson],[cCusPhone],[cCusHand],[cCusDefine2],[cCusDefine3]");
            cmd.Append(",[cPersonCode],[cPersonName],[cCusDefine6],[cCusDefine7],[cCusDefine8],[cCusDefine9]");
            cmd.Append(",[dCusDevDate],[cCusEmail] FROM [CRM_Customer_List] WHERE 1 = 1 ");
            if (!string.IsNullOrEmpty(sphone))
                cmd.Append(" AND ([cCusPhone] like '%" + sphone + "%' OR [cCusHand] like '%" + sphone + "%' OR [cCusDefine2] like '%" + sphone + "%' OR [cCusDefine3] like '%" + sphone + "%')");
            if (!string.IsNullOrEmpty(code))
            {
                RefCuscode = getRefCusCode(code);
                cmd.Append(" and [cCusCode] in (" + RefCuscode + ")");
            }
            if (!string.IsNullOrEmpty(key))
            {
                cmd.Append(" AND ([cCusPhone] like '%" + key + "%' OR [cCusHand] like '%" + key + "%' OR [cCusDefine2] like '%" + key + "%' OR [cCusDefine3] like '%" + key + "%')");
                cmd.Append(" OR ([cCusName] like '%" + key + "%' OR [cCusAbbName] like '%" + key + "%' )");
                cmd.Append(" OR ([cCusAddress] like '%" + key + "%' OR [cCusOAddress] like '%" + key + "%' )");
                cmd.Append(" OR ([cCusPerson] like '%" + key + "%' OR [cCusCode] like '%" + key + "%' )");
            }
                
            cmd.Append(" Order by [cCusCode]");

            sqlcmd.CommandText = cmd.ToString();
            try
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Open)
                    sqlcmd.Connection.Open();
                sqlreader = sqlcmd.ExecuteReader();
                while (sqlreader.Read())
                {
                    cus = new Customer()
                    {
                        cCusCode = sqlreader.GetValue(sqlreader.GetOrdinal("cCusCode")).ToString(),
                        cCusName = sqlreader.GetValue(sqlreader.GetOrdinal("cCusName")).ToString(),
                        cCusAbbName = sqlreader.GetValue(sqlreader.GetOrdinal("cCusAbbName")).ToString(),
                        cMobile = sqlreader.GetValue(sqlreader.GetOrdinal("cCusHand")).ToString(),
                        cAddress = sqlreader.GetValue(sqlreader.GetOrdinal("cCusAddress")).ToString(),
                        cPhone = sqlreader.GetValue(sqlreader.GetOrdinal("cCusPhone")).ToString(),
                        cShipAddress = sqlreader.GetValue(sqlreader.GetOrdinal("cCusOAddress")).ToString(),
                        District = sqlreader.GetValue(sqlreader.GetOrdinal("cDCName")).ToString(),
                        EMail = sqlreader.GetValue(sqlreader.GetOrdinal("cCusEmail")).ToString(),
                        Memo = sqlreader.GetValue(sqlreader.GetOrdinal("cCusDefine6")).ToString()
                            + " | " + sqlreader.GetValue(sqlreader.GetOrdinal("cCusDefine7")).ToString()
                            + " | " + sqlreader.GetValue(sqlreader.GetOrdinal("cCusDefine8")).ToString()
                            + " | " + sqlreader.GetValue(sqlreader.GetOrdinal("cCusDefine9")).ToString(),
                        topCusCode = sqlreader.GetValue(sqlreader.GetOrdinal("cCusHeadCode")).ToString(),
                        ContactName = sqlreader.GetValue(sqlreader.GetOrdinal("cCusPerson")).ToString(),
                        employeeName = sqlreader.GetValue(sqlreader.GetOrdinal("cPersonName")).ToString(),
                        cusClass = sqlreader.GetValue(sqlreader.GetOrdinal("cCCName")).ToString(),
                        CreateDate = DateTime.Parse(sqlreader.GetValue(sqlreader.GetOrdinal("dCusDevDate")).ToString()),
                        Employees = null,
                        topCus = null,
                        Contacts = null,
                    };
                    if (null == r) r = new List<Customer>();
                    if (!r.Exists(item => item.cCusCode == cus.cCusCode))
                        r.Add(cus);
                }
                if (sqlreader != null && !sqlreader.IsClosed)
                    sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                    sqlcmd.Connection.Close();
            }
            catch
            {
                if (sqlreader != null && !sqlreader.IsClosed)
                    sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                    sqlcmd.Connection.Close();
                throw;
            }
            return r;
        }
        private List<Customer> getCustomersAll()
        {
            List<Customer> rCusList = new List<Customer>();
            StringBuilder cmd = new StringBuilder();
            Customer cus = new Customer();
            cmd.Clear();
            cmd.Append(" SELECT [cCusCode],[cCusName],[cCusAbbName],[cCusAddress],[cCusOAddress]");
            cmd.Append(",[cDCName],[cCCName],[cCusHeadCode],[cCusPerson],[cCusPhone],[cCusHand],[cCusDefine2],[cCusDefine3]");
            cmd.Append(",[cPersonCode],[cPersonName],[cCusDefine6],[cCusDefine7],[cCusDefine8],[cCusDefine9]");
            cmd.Append(",[dCusDevDate],[cCusEmail] FROM [CRM_Customer_List] WHERE 1 = 1 ");
            cmd.Append(" Order by [cCusCode]");
            SqlCommand sqlcmd = new SqlCommand() {CommandType = basesqlcmd.CommandType,Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString)};
            if (sqlreader != null && sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            sqlcmd.CommandText = cmd.ToString();
            try
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Open)
                    sqlcmd.Connection.Open();
                sqlreader = sqlcmd.ExecuteReader();
                while (sqlreader.Read())
                {
                    cus = new Customer()
                    {
                        cCusCode = sqlreader.GetValue(sqlreader.GetOrdinal("cCusCode")).ToString(),
                        cCusName = sqlreader.GetValue(sqlreader.GetOrdinal("cCusName")).ToString(),
                        cCusAbbName = sqlreader.GetValue(sqlreader.GetOrdinal("cCusAbbName")).ToString(),
                        cMobile = sqlreader.GetValue(sqlreader.GetOrdinal("cCusHand")).ToString(),
                        cAddress = sqlreader.GetValue(sqlreader.GetOrdinal("cCusAddress")).ToString(),
                        cPhone = sqlreader.GetValue(sqlreader.GetOrdinal("cCusPhone")).ToString(),
                        cShipAddress = sqlreader.GetValue(sqlreader.GetOrdinal("cCusOAddress")).ToString(),
                        District = sqlreader.GetValue(sqlreader.GetOrdinal("cDCName")).ToString(),
                        EMail = sqlreader.GetValue(sqlreader.GetOrdinal("cCusEmail")).ToString(),
                        Memo = sqlreader.GetValue(sqlreader.GetOrdinal("cCusDefine6")).ToString()
                            + " | " + sqlreader.GetValue(sqlreader.GetOrdinal("cCusDefine7")).ToString()
                            + " | " + sqlreader.GetValue(sqlreader.GetOrdinal("cCusDefine8")).ToString()
                            + " | " + sqlreader.GetValue(sqlreader.GetOrdinal("cCusDefine9")).ToString(),
                        topCusCode = sqlreader.GetValue(sqlreader.GetOrdinal("cCusHeadCode")).ToString(),
                        ContactName = sqlreader.GetValue(sqlreader.GetOrdinal("cCusPerson")).ToString(),
                        employeeName = sqlreader.GetValue(sqlreader.GetOrdinal("cPersonName")).ToString(),
                        cusClass = sqlreader.GetValue(sqlreader.GetOrdinal("cCCName")).ToString(),
                        CreateDate = DateTime.Parse(sqlreader.GetValue(sqlreader.GetOrdinal("dCusDevDate")).ToString()),
                        Employees = null,
                        topCus = null,
                        Contacts = null,
                    };
                    if (null == rCusList) rCusList = new List<Customer>();
                    if (!rCusList.Exists(item => item.cCusCode == cus.cCusCode))
                        rCusList.Add(cus);
                }
                if (sqlreader != null && !sqlreader.IsClosed)
                    sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                    sqlcmd.Connection.Close();
            }
            catch
            {
                if (sqlreader != null && !sqlreader.IsClosed)
                    sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                    sqlcmd.Connection.Close();
                throw;
            }

            return rCusList;
        }

        public Customer getCustomer(string cuscode)
        {
            StringBuilder cmd = new StringBuilder();
            Customer cus = null;
            cmd.Clear();
            cmd.Append(" SELECT [cCusCode],[cCusName],[cCusAbbName],[cCusAddress],[cCusOAddress]");
            cmd.Append(",[cDCName],[cCCName],[cCusHeadCode],[cCusPerson],[cCusPhone],[cCusHand],[cCusDefine2],[cCusDefine3]");
            cmd.Append(",[cPersonCode],[cPersonName],[cCusDefine6],[cCusDefine7],[cCusDefine8],[cCusDefine9]");
            cmd.Append(",[dCusDevDate],[cCusEmail] FROM [CRM_Customer_List] ");
            cmd.Append(" WHERE 1 = 1 and [cCusCode] = '"+cuscode+"'");
            cmd.Append(" Order by [cCusCode]");
            SqlCommand sqlcmd = new SqlCommand() { CommandType = basesqlcmd.CommandType, Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString) };
            if (sqlreader != null && sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            sqlcmd.CommandText = cmd.ToString();
            try
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Open)
                    sqlcmd.Connection.Open();
                sqlreader = sqlcmd.ExecuteReader();
                while (sqlreader.Read())
                {
                    cus = new Customer()
                    {
                        cCusCode = sqlreader.GetValue(sqlreader.GetOrdinal("cCusCode")).ToString(),
                        cCusName = sqlreader.GetValue(sqlreader.GetOrdinal("cCusName")).ToString(),
                        cCusAbbName = sqlreader.GetValue(sqlreader.GetOrdinal("cCusAbbName")).ToString(),
                        cMobile = sqlreader.GetValue(sqlreader.GetOrdinal("cCusHand")).ToString(),
                        cAddress = sqlreader.GetValue(sqlreader.GetOrdinal("cCusAddress")).ToString(),
                        cPhone = sqlreader.GetValue(sqlreader.GetOrdinal("cCusPhone")).ToString(),
                        cShipAddress = sqlreader.GetValue(sqlreader.GetOrdinal("cCusOAddress")).ToString(),
                        District = sqlreader.GetValue(sqlreader.GetOrdinal("cDCName")).ToString(),
                        EMail = sqlreader.GetValue(sqlreader.GetOrdinal("cCusEmail")).ToString(),
                        Memo = sqlreader.GetValue(sqlreader.GetOrdinal("cCusDefine6")).ToString()
                            + " | " + sqlreader.GetValue(sqlreader.GetOrdinal("cCusDefine7")).ToString()
                            + " | " + sqlreader.GetValue(sqlreader.GetOrdinal("cCusDefine8")).ToString()
                            + " | " + sqlreader.GetValue(sqlreader.GetOrdinal("cCusDefine9")).ToString(),
                        topCusCode = sqlreader.GetValue(sqlreader.GetOrdinal("cCusHeadCode")).ToString(),
                        ContactName = sqlreader.GetValue(sqlreader.GetOrdinal("cCusPerson")).ToString(),
                        employeeName = sqlreader.GetValue(sqlreader.GetOrdinal("cPersonName")).ToString(),
                        cusClass = sqlreader.GetValue(sqlreader.GetOrdinal("cCCName")).ToString(),
                        CreateDate = DateTime.Parse(sqlreader.GetValue(sqlreader.GetOrdinal("dCusDevDate")).ToString()),
                        Employees = null,
                        topCus = null,
                        Contacts = null,
                    };
                }
                if (sqlreader != null && !sqlreader.IsClosed)
                    sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                    sqlcmd.Connection.Close();
            }
            catch
            {
                if (sqlreader != null && !sqlreader.IsClosed)
                    sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                    sqlcmd.Connection.Close();
                throw;
            }

            return cus;
        }


        /// <summary>
        /// 返回联系人列表
        /// </summary>
        /// <param name="cuscode">客户编码</param>
        /// <param name="sphone">来电号码</param>
        /// <returns></returns>
        public List<Contact> getContacts(string cuscode = null, string sphone = null, string name = null, string key = null)
        {
            
            List<Contact> r = null;
            SqlCommand sc = new SqlCommand() {CommandType = basesqlcmd.CommandType,Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString)};
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            Contact once = new Contact();
            StringBuilder cmd = new StringBuilder();
            cmd.Append(" SELECT [cCusCode],[cContactCode],[cContactName],[cMobilePhone],[cOfficePhone],[cHomePhone],[cEmail],[cMemo] FROM [Crm_Contact] ");
            cmd.Append(" where 1 = 1 ");
            if (!string.IsNullOrEmpty(sphone))
            {
                cmd.Append(" and ( [cOfficePhone] like '%" + sphone + "%'");
                cmd.Append(" or [cHomePhone] like '%" + sphone + "%'");
                cmd.Append(" or [cMobilePhone] like '%" + sphone + "%')");
            }
            if (!string.IsNullOrEmpty(cuscode))
                cmd.Append(" and [cCusCode] = '" + cuscode + "'");
            if (!string.IsNullOrEmpty(name))
                cmd.Append(" and [cContactName] like '%" + name + "%'");
            if (!string.IsNullOrEmpty(key))
            {
                cmd.Append(" and ( [cOfficePhone] like '%" + key + "%'");
                cmd.Append(" or [cHomePhone] like '%" + key + "%'");
                cmd.Append(" or [cMobilePhone] like '%" + key + "%'");
                cmd.Append(" or [cCusCode] = '" + key + "'");
                cmd.Append(" or [cContactName] like '%" + key + "%')");
            }
            sc.CommandText = cmd.ToString();
            try
            {
                //Context.Sql(cmd.ToString()).QueryMany<Contact>();
                if (sc.Connection.State != System.Data.ConnectionState.Open)
                    sc.Connection.Open();
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                sqlreader = sc.ExecuteReader();
                while (sqlreader.Read())
                {//[cCusCode],[cContactCode],[cContactName],[cMobilePhone],[cOfficePhone],[cHomePhone],[cEmail],[cMemo]
                    once = new Contact()
                    {
                        name = sqlreader.GetValue(sqlreader.GetOrdinal("cContactName")).ToString(),
                        code = sqlreader.GetValue(sqlreader.GetOrdinal("cContactCode")).ToString(),
                        DWCode = sqlreader.GetValue(sqlreader.GetOrdinal("cCusCode")).ToString(),
                        EMail = sqlreader.GetValue(sqlreader.GetOrdinal("cEmail")).ToString(),
                        mobile = sqlreader.GetValue(sqlreader.GetOrdinal("cMobilePhone")).ToString(),
                        phone = sqlreader.GetValue(sqlreader.GetOrdinal("cOfficePhone")).ToString()
                          + "," + sqlreader.GetValue(sqlreader.GetOrdinal("cHomePhone")).ToString(),
                        Memo = sqlreader.GetValue(sqlreader.GetOrdinal("cMemo")).ToString()
                    };
                    if (null == r) r = new List<Contact>();
                    if (!r.Exists(c => c.name == once.name))
                        r.Add(once);
                }
                if (sqlreader != null && !sqlreader.IsClosed)
                    sqlreader.Close();
                if (sc.Connection.State != System.Data.ConnectionState.Closed)
                    sc.Connection.Close();
            }
            catch (Exception e)
            {
                if (sqlreader != null && !sqlreader.IsClosed)
                    sqlreader.Close();
                if (sc.Connection.State != System.Data.ConnectionState.Closed)
                    sc.Connection.Close();
                throw e;
            }
            return r;
        }
        
        /// <summary>
        /// 返回供应商列表
        /// </summary>
        /// <param name="cuscode">供应商编码</param>
        /// <param name="sphone">来电号码</param>
        /// <returns></returns>
        public List<Vendor> getVendors(string vencode = null, string sphone = null,string key = null)
        {
            SqlCommand sqlcmd = new SqlCommand() {CommandType = basesqlcmd.CommandType,Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString)};
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            List<Vendor> r = null;
            sqlcmd.CommandText = string.Empty;
            Vendor once = new Vendor();
            StringBuilder cmd = new StringBuilder();
            cmd.Append(" SELECT [cVCName],[cVenCode],[cVenName],[cVenAddress],[cDCName],[dVenDevDate]");
            cmd.Append(" ,[cVenPerson],[cVenPhone],[cVenHand],[cVenEmail],[cMemo],[cPersonName],[cVenHeadCode] ");
            cmd.Append(" FROM [Vendor]");
            cmd.Append(" INNER JOIN [VendorClass] ON [Vendor].[cVCCode] = [VendorClass].[cVCCode]");
            cmd.Append(" INNER JOIN [DistrictClass] ON [Vendor].[cDCCode] = [DistrictClass].[cDCCode]");
            cmd.Append(" INNER JOIN [Person] on [Person].[cPersonCode] = [Vendor].[cVenPPerson]");
            cmd.Append(" WHERE 1 = 1");
            if (!string.IsNullOrEmpty(sphone))
            {
                cmd.Append(" and ( [cVenPhone] like '%" + sphone + "%'");
                cmd.Append(" or [cVenHand] like '%" + sphone + "%')");
            }
            if (!string.IsNullOrEmpty(vencode))
                cmd.Append(" and [cVenCode] = '" + vencode + "'");
            if (!string.IsNullOrEmpty(key))
            {
                cmd.Append(" OR ([cVenName]='%" + key + "%' OR [cVenAbbName]='%" + key + "%' OR [cVenAddress]='%" + key + "%' OR [cVenPerson]='%" + key + "%')");
                cmd.Append(" OR [cVenCode] like '%" + key + "%'");
            }
            sqlcmd.CommandText = cmd.ToString();
            try
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Open)
                    sqlcmd.Connection.Open();
                sqlreader = sqlcmd.ExecuteReader();
                while (sqlreader.Read())
                {//[cVCName],[cVenCode],[cVenName],[cVenAddress],[cDCName],[dVenDevDate]
                    //,[cVenPerson],[cVenPhone],[cVenHand],[cVenEmail],[cMemo],[cPersonName],[cVenHeadCode] 
                    once = new Vendor()
                    {
                        cVenName = sqlreader.GetValue(sqlreader.GetOrdinal("cVenName")).ToString(),
                        cVenCode = sqlreader.GetValue(sqlreader.GetOrdinal("cVenCode")).ToString(),
                        topVenCode = sqlreader.GetValue(sqlreader.GetOrdinal("cVenHeadCode")).ToString(),
                        EMail = sqlreader.GetValue(sqlreader.GetOrdinal("cVenEmail")).ToString(),
                        cMobile = sqlreader.GetValue(sqlreader.GetOrdinal("cVenHand")).ToString(),
                        cPhone = sqlreader.GetValue(sqlreader.GetOrdinal("cVenPhone")).ToString(),
                        Memo = sqlreader.GetValue(sqlreader.GetOrdinal("cMemo")).ToString(),
                        cAddress = sqlreader.GetValue(sqlreader.GetOrdinal("cVenAddress")).ToString(),
                        ContactName = sqlreader.GetValue(sqlreader.GetOrdinal("cVenPerson")).ToString(),
                        devDate = DateTime.Parse(sqlreader.GetValue(sqlreader.GetOrdinal("cVenDevDate")).ToString()),
                        District = sqlreader.GetValue(sqlreader.GetOrdinal("cDCName")).ToString(),
                        venClass = sqlreader.GetValue(sqlreader.GetOrdinal("cVCName")).ToString(),
                        employeeName = sqlreader.GetValue(sqlreader.GetOrdinal("cPersonName")).ToString()
                    };

                    if (null == r) r = new List<Vendor>();
                    if (!r.Exists(c => c.cVenCode == once.cVenCode))
                        r.Add(once);
                }
                if (sqlreader != null && !sqlreader.IsClosed)
                    sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                    sqlcmd.Connection.Close();
            }
            catch
            {
                if (sqlreader != null && !sqlreader.IsClosed)
                    sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                    sqlcmd.Connection.Close();
                throw;
            }
            return r;
        }

    }
}