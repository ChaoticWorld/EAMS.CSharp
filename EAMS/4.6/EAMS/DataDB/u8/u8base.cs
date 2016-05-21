using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DataDB.ModelBase;
using System.Data.Entity;
using System.Data.SqlClient;
using FluentData;

namespace DataDB.u8
{
    /// <summary>
    /// dbU8基本部分
    /// </summary>
    [Serializable]
    public partial class dbU8
    {
        private SqlCommand basesqlcmd = new SqlCommand();
        private SqlDataReader sqlreader;
        public IDbContext Context;
        public dbU8()
        {
            //正式工作时，将缺省连接getDefaultConn改为当前年度连接getCurrYearConn
            basesqlcmd.Connection = new SqlConnection(getCurrYearConn().ConnectionString);
            basesqlcmd.CommandType = System.Data.CommandType.Text;      
            Context = new FluentData.DbContext().ConnectionString(
                (new SqlConnection(getCurrYearConn().ConnectionString)).ConnectionString
                , new SqlServerProvider());

        }
        ~dbU8() { if (sqlreader != null) sqlreader.Dispose(); basesqlcmd.Dispose(); }

        private SqlConnectionStringBuilder getDefaultConn()
        {
            SqlConnectionStringBuilder r = new SqlConnectionStringBuilder();
            r.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ErpConn"].ConnectionString;
            return r;
        }
        private SqlConnectionStringBuilder getCurrYearConn()
        {
            string initialCatalog;
            SqlConnectionStringBuilder r = new SqlConnectionStringBuilder();
            r = getDefaultConn();
            initialCatalog = r.InitialCatalog;
            initialCatalog = initialCatalog.Replace(initialCatalog.Substring(initialCatalog.Length - 4, 4), DateTime.Now.Year.ToString());
            r.InitialCatalog = initialCatalog;
            r.ConnectTimeout = 5;
            if (!canSqlConn(r.ConnectionString))
                r = null;
            return r;
        }
        private SqlConnectionStringBuilder getPrevYearConn()
        {
            string initialCatalog;
            SqlConnectionStringBuilder r = new SqlConnectionStringBuilder();
            r = getDefaultConn();
            initialCatalog = r.InitialCatalog;
            initialCatalog = initialCatalog.Replace(initialCatalog.Substring(initialCatalog.Length - 4, 4), (DateTime.Now.Year - 1).ToString());
            r.InitialCatalog = initialCatalog;
            r.ConnectTimeout = 5;
            if (!canSqlConn(r.ConnectionString))
                r = null;
            return r;
        }

        public bool canSqlConn(string strConn)
        {
            bool r = false;
            SqlConnection t = new SqlConnection(strConn);
            try
            {
                if (t.State != System.Data.ConnectionState.Open)
                    t.Open();
                r = true;
                t.Close();
            }
            catch 
            {
                if (t.State != System.Data.ConnectionState.Closed)
                    t.Close();
                r = false;
            }
            return r;
        }

        /// <summary>
        /// 设置当前数据库链接为前当年度帐
        /// </summary>
        public void setCurrYearConn()
        { basesqlcmd.Connection = new SqlConnection(getCurrYearConn().ConnectionString); }
        /// <summary>
        /// 设置当前数据库链接为上年年度帐
        /// </summary>
        public void setPrevYearConn()
        { basesqlcmd.Connection = new SqlConnection(getPrevYearConn().ConnectionString); }

        /// <summary>
        /// SQL Select 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool isExistSqlSelect(string sqlStr)
        {
            bool r = false;
            dynamic qs = Context.Sql(sqlStr).QuerySingle<dynamic>();
            if (qs == null) r = false; else r = true;
            SqlCommand sc = new SqlCommand()
            {
                CommandType = basesqlcmd.CommandType,
                Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString)
            };
            if (sqlreader != null && sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            sc.CommandText = sqlStr;
            try
            {
                if (sc.Connection.State != System.Data.ConnectionState.Open)
                    sc.Connection.Open();
                if (sc.ExecuteScalar() != null)
                    r = true;
                if (sc.Connection.State != System.Data.ConnectionState.Closed)
                    sc.Connection.Close();
            }
            catch (SqlException e)
            {
                if (sc.Connection.State != System.Data.ConnectionState.Closed)
                    sc.Connection.Close();
                throw new Exception("数据库连接错误！", e);
            }
            return r;
        }
    }

    /// <summary>
    /// 扩展Model
    /// 客户，客户联系人，供应商
    /// </summary>
    [Serializable]
    public partial class u8
    {
        /// <summary>
        /// 电话类别
        /// </summary>
        public enum phoneClass
        {
            Customer = 0,
            CustomerContact = 1,
            Vendor = 2,
            VendorContact = 3
        }
        public static phoneClass PhoneClass { get; set; }
    }

}
