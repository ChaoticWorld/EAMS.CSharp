using System;
using System.Text;
using FluentData;

namespace DataAccess
{
    public sealed class erpContextBase 
    {
        private volatile static IDbContext _context = null;
        private static object syncRoot = new Object();
        private erpContextBase() { }
        public static IDbContext Context { get { return getContext("ErpConn"); } }
        public static IDbContext getContext(string ConnName = "ErpConn")
        {
            if (_context == null)
            {
                lock (syncRoot)
                {
                    if (_context == null)
                        _context = new DbContext().ConnectionStringName(ConnName, new SqlServerProvider());
                }
            }
            return _context;
        }
        /// <summary>
        /// 执行条件:已经生成DBContext;
        /// 目标:调整ERP年度账的数据库名,并返回调整后的DBContext
        /// </summary>
        /// <param name="y">要增加的年度数-1,0,1</param>
        /// <returns></returns>
        public static IDbContext getAddYearContext(int y)
        {
            IDbContext Context = null;
            System.Data.IDbConnection conn = null;
            string connStr;
            string connDataBase;
            int year = 0;
            if (_context != null)
            {
                conn = _context.Data.Connection;
                connStr = conn.ConnectionString;
                if (int.TryParse(conn.Database.Substring(conn.Database.Length - 4, 4), out year))
                {
                    year += y;
                    connDataBase = conn.Database.Substring(0, conn.Database.Length - 4) + year.ToString();
                    if (year > System.DateTime.Now.Year) { throw new Exception("年度异常,请检查!"); }
                    connStr = connStr.Replace(conn.Database, connDataBase);
                    Context = new DbContext().ConnectionString(connStr, new FluentData.SqlServerProvider());
                }
                else { throw new Exception("年度异常,请检查!"); }
            }
            else { getContext("ErpConn");getAddYearContext(y); }
            return Context;
        }
        /// <summary>
        /// 执行条件:已经生成DBContext;
        /// 目标:并返回指定年度的DBContext
        /// </summary>
        /// <param name="y">yearDB指定年度：2010|2015</param>
        /// <returns></returns>
        public static IDbContext getYearDB(int yearDB)
        { //erp年度帐数据库

            IDbContext Context = null;
            System.Data.SqlClient.SqlConnectionStringBuilder scsb = new System.Data.SqlClient.SqlConnectionStringBuilder(erpContextBase.Context.Data.ConnectionString);
            scsb.InitialCatalog = scsb.InitialCatalog.Substring(0, scsb.InitialCatalog.Length - 4) + yearDB.ToString();
            if (yearDB > DateTime.Now.Year) throw new Exception("设置年度不能大于当前所年度！");
            Context = new DbContext().ConnectionString(scsb.ConnectionString, new SqlServerProvider());
            return Context;
        }
    }
    public sealed class eamsAppDataContextBase 
    {
        private volatile static IDbContext _context = null;
        private static object syncRoot = new Object();
        private eamsAppDataContextBase() { }
        public static IDbContext Context { get { return getContext("eamsAppDataConn"); } }
        public static IDbContext getContext(string ConnName = "eamsAppDataConn")
        {
            if (_context == null)
            {
                lock (syncRoot)
                {
                    if (_context == null)
                        _context = new DbContext().ConnectionStringName(ConnName, new SqlServerProvider());
                }
            }
            return _context;
        }
    }
    public sealed class eamsAppSystemContextBase 
    {
        private volatile static IDbContext _context = null;
        private static object syncRoot = new Object();
        private eamsAppSystemContextBase() { }
        public static IDbContext Context { get { return getContext("eamsAppSystemConn"); } }
        public static IDbContext getContext(string ConnName = "eamsAppSystemConn")
        {
            if (_context == null)
            {
                lock (syncRoot)
                {
                    if (_context == null)
                        _context = new DbContext().ConnectionStringName(ConnName, new SqlServerProvider());
                }
            }
            return _context;
        }
    }
}
