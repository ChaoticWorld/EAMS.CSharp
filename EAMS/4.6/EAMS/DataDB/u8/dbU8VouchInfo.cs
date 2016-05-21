using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataDB.ModelBase;
using System.Data.SqlClient;


//处理销售,入库单据信息
namespace DataDB.u8
{
    /// <summary>
    /// 扩展Model
    /// 销售,入库单据
    /// </summary>
    public partial class u8
    {
    }
    /// <summary>
    /// 方法操作
    /// 处理销售,入库单据信息
    /// </summary>
    public partial class dbU8
    {
        static IVouch vouch;
        private bool exists(string cmd)
        {
            bool r = false;
            SqlCommand sqlcmd = new SqlCommand() { CommandType = basesqlcmd.CommandType, Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString) };
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();

            sqlcmd.CommandText = string.Empty;
            sqlcmd.CommandText = cmd;
            try
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Open)
                    sqlcmd.Connection.Open();
                r = (sqlcmd.ExecuteScalar() != null);
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                    sqlcmd.Connection.Close();
            }
            catch
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                    sqlcmd.Connection.Close();
                throw;
            }
            return r;
        }
        public bool existsDispatchDLCode(string dlcode)
        {
            bool r = false;
            StringBuilder cmd = new StringBuilder();
            cmd.Append("select top 1 cdlcode from Dispatchlist where cdlcode = '" + dlcode + "'");
            
            r = exists(cmd.ToString());
            return r;
        }
        private object getFieldValue(string cmd)
        {
            object r = null;
            SqlCommand sqlcmd = new SqlCommand() { CommandType = basesqlcmd.CommandType, Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString) };
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();

            sqlcmd.CommandText = string.Empty;
            sqlcmd.CommandText = cmd;
            try
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Open)
                    sqlcmd.Connection.Open();
                r = sqlcmd.ExecuteScalar();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                    sqlcmd.Connection.Close();
            }
            catch
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                    sqlcmd.Connection.Close();
                throw;
            }
            return r;
        }
        private bool update(string cmd)
        {
            bool r = false;
            SqlCommand sqlcmd = new SqlCommand() { CommandType = basesqlcmd.CommandType, Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString) };
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();

            sqlcmd.CommandText = string.Empty;
            sqlcmd.CommandText = cmd;
            try
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Open)
                    sqlcmd.Connection.Open();
                r = sqlcmd.ExecuteNonQuery() > 0;
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                    sqlcmd.Connection.Close();
            }
            catch
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                    sqlcmd.Connection.Close();
                throw;
            }
            return r;
        }
        public object getDispatchMainField(string field, string dlcode) {
            
            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT TOP 1 "+field);
            cmd.Append(" FROM sales_fhd_t where 1 = 1 and cdlcode = '" + dlcode + "'");
            return getFieldValue(cmd.ToString());
        }
        public bool updateDispatchField(string field, string value,string dlcode)
        {
            bool r = false;
            StringBuilder cmd = new StringBuilder();
            cmd.Append("update dispatchlist set " + field + " = '" + value + "' where cdlcode = '" + dlcode + "'");
            r = update(cmd.ToString());
            return r;
        }
        public bool existsDispatch(string cusCode)
        {
            bool r = false;
            StringBuilder cmd = new StringBuilder();
            string CusCodes = getRefCusCode(cusCode);
            cmd.Append("select top 1 cdlcode from Dispatchlist where ccuscode in ( " + CusCodes + ")");
            r = exists(cmd.ToString());
            return r;
        }

        public Dispatch getDispatch(string dlcode)
        {
            Dispatch r = new Dispatch();
            r.Main = getDispatchMain(dlcode);
            r.Details = getDispatchDetail(r.Main.id);
            double je = 0;
            foreach (DispatchDetail dd in r.Details)
                je += dd.iSum;
            r.Main.Je = (decimal)je;
            return r;
        }
        public List<Dispatch> getDispatchsNoTax(DateTime s, DateTime f)
        {
            List<Dispatch> r = new List<Dispatch>();
            SqlCommand sqlcmd = new SqlCommand() { CommandType = basesqlcmd.CommandType, Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString) };
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            DispatchMain m = null;
            Dispatch d = new Dispatch();
            sqlcmd.CommandText = string.Empty;
            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT  DLID, cDLCode, cCusCode, cVouchType, dDate, cShipAddress, cMemo, cMaker, cVerifier ");
            cmd.Append(" FROM DispatchList where 1 = 1 and cdefine15 is null and dDate >= '" + s.ToString("yyyy-MM-dd") + "' and dDate <= '" + f.ToString("yyyy-MM-dd") + "'");
            sqlcmd.CommandText = cmd.ToString();
            try
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Open) sqlcmd.Connection.Open();
                sqlreader = sqlcmd.ExecuteReader();
                while (sqlreader.Read())
                { // DLID, cDLCode, cVouchType, dDate, cShipAddress, cMemo, cMaker, cVerifier
                    d = new Dispatch();
                    m = new DispatchMain()
                    {
                        id = sqlreader.GetValue(sqlreader.GetOrdinal("DLID")).ToString(),
                        Code = sqlreader.GetValue(sqlreader.GetOrdinal("cDLCode")).ToString(),
                        cShipAddress = sqlreader.GetValue(sqlreader.GetOrdinal("cShipAddress")).ToString(),
                        dDate = sqlreader.GetDateTime(sqlreader.GetOrdinal("dDate")),
                        DWCode = sqlreader.GetValue(sqlreader.GetOrdinal("cCusCode")).ToString(),
                        Maker = sqlreader.GetValue(sqlreader.GetOrdinal("cMaker")).ToString(),
                        Verifier = sqlreader.GetValue(sqlreader.GetOrdinal("cVerifier")).ToString(),
                        Memo = sqlreader.GetValue(sqlreader.GetOrdinal("cMemo")).ToString()
                    };
                    d.Main = m;
                    r.Add(d);
                }
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed) sqlcmd.Connection.Close();
            }
            catch
            {
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed) sqlcmd.Connection.Close();
                throw;
            }
            foreach (Dispatch dd in r)
            {
                dd.Details = getDispatchDetail(dd.Main.id);
                dd.Main.customer = getCustomer(dd.Main.DWCode);
            }
            return r;
        }

        private DispatchMain getDispatchMain(string dlcode){
            SqlCommand sqlcmd = new SqlCommand() { CommandType = basesqlcmd.CommandType, Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString) };
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            //List<DispatchMain> r = new List<DispatchMain>();
            DispatchMain m = null;
            sqlcmd.CommandText = string.Empty;
            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT top 1 DLID, cDLCode, cCusCode, cVouchType, dDate, cShipAddress, cMemo, cMaker, cVerifier ");
            cmd.Append(" FROM DispatchList where 1 = 1 and ( cdlcode = '" + dlcode + "' or cdlcode like '%" + dlcode + "')");
            sqlcmd.CommandText = cmd.ToString();
            try
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Open) sqlcmd.Connection.Open();
                sqlreader = sqlcmd.ExecuteReader();
                while (sqlreader.Read())
                { // DLID, cDLCode, cVouchType, dDate, cShipAddress, cMemo, cMaker, cVerifier
                    m = new DispatchMain()
                    {
                        id = sqlreader.GetValue(sqlreader.GetOrdinal("DLID")).ToString(),
                        Code = sqlreader.GetValue(sqlreader.GetOrdinal("cDLCode")).ToString(),
                        cShipAddress = sqlreader.GetValue(sqlreader.GetOrdinal("cShipAddress")).ToString(),
                        dDate = sqlreader.GetDateTime(sqlreader.GetOrdinal("dDate")),
                        DWCode = sqlreader.GetValue(sqlreader.GetOrdinal("cCusCode")).ToString(),
                        Maker = sqlreader.GetValue(sqlreader.GetOrdinal("cMaker")).ToString(),
                        Verifier = sqlreader.GetValue(sqlreader.GetOrdinal("cVerifier")).ToString(),
                        Memo = sqlreader.GetValue(sqlreader.GetOrdinal("cMemo")).ToString()
                    };
                    //r.Main = m;
                }
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed) sqlcmd.Connection.Close();
            }
            catch
            {
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed) sqlcmd.Connection.Close();
                throw;
            }
            m.customer = getCustomer(m.DWCode);
            return m;
        }
        public List<Dispatch> getDispatch(string cusCode, int lastnum)
        {
            SqlCommand sqlcmd = new SqlCommand() {CommandType = basesqlcmd.CommandType,Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString)};
            //if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            List<Dispatch> r = new List<Dispatch>();
            List<Dispatch> rOnce = new List<Dispatch>();
            r = getDispatchOnce(cusCode:cusCode,lastnum:lastnum);
            //查询上年数据
            if (r.Count < lastnum)
            {
                //获得上年数据连接
                if (null != getPrevYearConn())
                {
                    if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                        sqlcmd.Connection.Close();
                    sqlcmd.Connection = new SqlConnection(getPrevYearConn().ConnectionString);
                    Context = new FluentData.DbContext().ConnectionString(sqlcmd.Connection.ConnectionString, new FluentData.SqlServerProvider());
                    rOnce = getDispatchOnce(cusCode, lastnum - r.Count);
                    //还原当前年度数据库连接
                    if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                        sqlcmd.Connection.Close();
                    sqlcmd.Connection = new SqlConnection(getCurrYearConn().ConnectionString);
                    Context = new FluentData.DbContext().ConnectionString(sqlcmd.Connection.ConnectionString, new FluentData.SqlServerProvider());

                }
                if (rOnce.Count > 0)
                    r.AddRange(rOnce);
            }

            return r;
        }
        private List<Dispatch> getDispatchOnce(string cusCode, int lastnum)
        {
            List<Dispatch> r = new List<Dispatch>();
            Dispatch d;
            foreach (DispatchMain dm in getDispatchMain(cusCode, lastnum))
            {
                d = new Dispatch();
                d.Main=dm;
                if (!r.Exists(item => item.Main.id == d.Main.id))
                {
                    d.Details = getDispatchDetail(d.Main.id);
                    r.Add(d);
                }                
            }
            return r;
        }
        
        private List<DispatchMain> getDispatchMain(string cusCode, int lastnum)
        {
            SqlCommand sqlcmd = new SqlCommand() {CommandType = basesqlcmd.CommandType,Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString)};
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            List<DispatchMain> r = new List<DispatchMain>();
            DispatchMain m = new DispatchMain();
            sqlcmd.CommandText = string.Empty;
            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT DLID, cDLCode, cCusCode, cVouchType, dDate, cShipAddress, cMemo, cMaker, cVerifier ");
            cmd.Append(" FROM DispatchList where 1 = 1 and dlid in ( ");
            cmd.Append(" select top " + lastnum.ToString() + " dlid from DispatchList ");
            cmd.Append(" where 1 = 1 and cCusCode in (" + getRefCusCode(cusCode) + ")");
            cmd.Append(" order by dDate desc  )");
            sqlcmd.CommandText = cmd.ToString();
            try {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Open) sqlcmd.Connection.Open();
                sqlreader = sqlcmd.ExecuteReader();
                while (sqlreader.Read())
                { // DLID, cDLCode, cVouchType, dDate, cShipAddress, cMemo, cMaker, cVerifier
                    m = new DispatchMain()
                    {
                        id = sqlreader.GetValue(sqlreader.GetOrdinal("DLID")).ToString(),
                        Code = sqlreader.GetValue(sqlreader.GetOrdinal("cDLCode")).ToString(),
                        cShipAddress = sqlreader.GetValue(sqlreader.GetOrdinal("cShipAddress")).ToString(),
                        dDate = sqlreader.GetDateTime(sqlreader.GetOrdinal("dDate")),
                        DWCode = sqlreader.GetValue(sqlreader.GetOrdinal("cCusCode")).ToString(),
                        Maker = sqlreader.GetValue(sqlreader.GetOrdinal("cMaker")).ToString(),
                        Verifier = sqlreader.GetValue(sqlreader.GetOrdinal("cVerifier")).ToString(),
                        Memo = sqlreader.GetValue(sqlreader.GetOrdinal("cMemo")).ToString()
                    };
                    if (!r.Exists(item => item.id == m.id)) r.Add(m);
                }
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed) sqlcmd.Connection.Close();
            }
            catch
            {
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed) sqlcmd.Connection.Close();
                throw;
            }
            return r;
        }
        private List<Detail> getDispatchDetail(string mainId)
        {
            SqlCommand sqlcmd = new SqlCommand() {CommandType = basesqlcmd.CommandType,Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString)};
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            List<Detail> r = new List<Detail>();
            DispatchDetail d = new DispatchDetail();
            sqlcmd.CommandText = string.Empty;
            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT DispatchLists.AutoID, DispatchLists.cInvCode, Inventory.cInvName, Inventory.cInvStd, ");
            cmd.Append(" ComputationUnit.cComUnitName, isnull(DispatchLists.iQuantity,0) as iQuantity, isnull(DispatchLists.iNum,0) as iNum, ");
            cmd.Append(" isnull(DispatchLists.iTaxUnitPrice,0) as iTaxUnitPrice, isnull(DispatchLists.iSum,0) as iSum, DispatchLists.cMemo FROM  DispatchLists ");
            cmd.Append(" INNER JOIN  Inventory ON  DispatchLists.cInvCode =  Inventory.cInvCode ");
            cmd.Append(" INNER JOIN  ComputationUnit ON  Inventory.cComUnitCode = ComputationUnit.cComunitCode ");
            cmd.Append(" where 1 = 1 and DispatchLists.DLID = '"+mainId+"'");
            sqlcmd.CommandText = cmd.ToString();
            try
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Open) sqlcmd.Connection.Open();
                sqlreader = sqlcmd.ExecuteReader();
                while (sqlreader.Read())
                { /*DispatchLists.AutoID, DispatchLists.cInvCode, Inventory.cInvName, Inventory.cInvStd,
 ComputationUnit.cComUnitName, DispatchLists.iQuantity, DispatchLists.iNum,
 DispatchLists.iTaxUnitPrice, DispatchLists.iSum, DispatchLists.cMemo*/
                    d = new DispatchDetail()
                    {
                        autoid = sqlreader.GetValue(sqlreader.GetOrdinal("AutoID")).ToString(),
                        //inventory = getInventory(sqlreader.GetValue(sqlreader.GetOrdinal("cInvCode")).ToString()),
                        
                        inventory = new Inventory()
                        {
                            cInvCode = sqlreader.GetValue(sqlreader.GetOrdinal("cInvCode")).ToString()
                            //cInvName = sqlreader.GetValue(sqlreader.GetOrdinal("cInvName")).ToString(),
                            //cInvStd = sqlreader.GetValue(sqlreader.GetOrdinal("cInvStd")).ToString(),
                            //cUnit = sqlreader.GetValue(sqlreader.GetOrdinal("cComUnitName")).ToString(),
                            //cInvClass = sqlreader.GetValue(sqlreader.GetOrdinal("cInvCode")).ToString()
                        },
                        quantity = new Quantity()
                        {
                            iQuantity = double.Parse(sqlreader.GetValue(sqlreader.GetOrdinal("iQuantity")).ToString()),
                            iNum = double.Parse(sqlreader.GetValue(sqlreader.GetOrdinal("iNum")).ToString())
                        },
                        iPrice =   double.Parse(sqlreader.GetValue(sqlreader.GetOrdinal("iTaxUnitPrice")).ToString()),
                        iSum = double.Parse(sqlreader.GetValue(sqlreader.GetOrdinal("iSum")).ToString()),
                        Memo = sqlreader.GetValue(sqlreader.GetOrdinal("cMemo")).ToString()
                    };
                    if (!r.Exists(item => item.autoid == d.autoid)) r.Add(d);
                }
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed) sqlcmd.Connection.Close();
                foreach (DispatchDetail dd in r)
                    dd.inventory = getInventory(dd.inventory.cInvCode); //.inventory.invClass = getInventoryClass(dd.inventory.cInvClass);
            }
            catch (Exception e)
            {
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed) sqlcmd.Connection.Close();
                throw e;
            }
            return r;
        }

        public List<DispatchMain> filterDispatchMain(DateTime s, DateTime e, string Area, string cusCode) {
            List<DispatchMain> r = new List<DispatchMain>();
            List<string> codes = filterDispatchMainCode(s, e, Area, cusCode);
            foreach (string c in codes)
                r.Add(getDispatchMain(c));
            return r;
        }
        public List<string> filterDispatchMainCode(DateTime s, DateTime e, string Area, string cusCode)
        {
            List<string> r = new List<string>();
            string sqlcmd = "select cdlcode from DispatchList as dl ";
            sqlcmd += " LEFT JOIN Customer as cus on cus.ccuscode = dl.cCusCode ";
            sqlcmd += " LEFT JOIN DistrictClass as dc on cus.cDCCode = dc.cDCCode ";
            sqlcmd += " where 1 = 1 ";
            sqlcmd += string.IsNullOrEmpty(Area) ? "" : " and (dc.cDCName = '" + Area + "' or dc.cDCName is null) ";
            sqlcmd += " and dl.dDate >= '"+s.ToString("yyyy-MM-dd")+"' and dl.dDate <= '"+e.ToString("yyyy-MM-dd")+"' ";
            sqlcmd += string.IsNullOrEmpty(cusCode) ? "" : " and dl.ccuscode = '" + cusCode + "' ";
            sqlcmd += " order by ddate desc ";

            r = Context.Sql(sqlcmd).QueryMany<string>();

            return r;
        }
        public bool existsSaleOrder(string cusCode)
        {
            bool r = false;
            StringBuilder cmd = new StringBuilder();
            string CusCodes = getRefCusCode(cusCode);
            cmd.Append("select top 1 cSOcode from SO_SOMain where ccuscode in ( " + CusCodes + ")");
            r = exists(cmd.ToString());
            return r;
        }

        public List<string> filterSaleOrderMainCode(DateTime s, DateTime e, string Area, string cusCode)
        {
            List<string> r;
            string sqlcmd = "select csocode from SO_SOMain as som ";
            sqlcmd += " LEFT JOIN Customer as cus on cus.ccuscode = som.cCusCode ";
            sqlcmd += " LEFT JOIN DistrictClass as dc on cus.cDCCode = dc.cDCCode ";
            sqlcmd += " where 1 = 1 ";
            sqlcmd += string.IsNullOrEmpty(Area) ? "" : " and (dc.cDCName = '" + Area + "' or dc.cDCName is null) ";
            sqlcmd += " and som.dDate >= '" + s.ToString("yyyy-MM-dd") + "' and som.dDate <= '" + e.ToString("yyyy-MM-dd") + "' ";
            sqlcmd += string.IsNullOrEmpty(cusCode) ? "" : " and som.ccuscode = '" + cusCode + "' ";
            sqlcmd += " order by ddate desc ";
            //var cmd = Context.Sql(sqlcmd);
            r = Context.Sql(sqlcmd).QueryMany<string>();
            return r;
        }
        public SaleOrder getSaleOrder(string soCode)
        {
            SaleOrder r = new SaleOrder();
            r.Main = getSaleOrderMain(soCode);
            r.Details = getSaleOrderDetail(r.Main.id);
            double je = 0;
            foreach (SaleOrderDetail dd in r.Details)
                je += dd.iSum;
            r.Main.Je = (decimal)je;
            return r;
        }
        public List<SaleOrder> getSaleOrder(string cusCode, int lastnum)
        {
            SqlCommand sqlcmd = new SqlCommand() {CommandType = basesqlcmd.CommandType,Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString)};
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            List<SaleOrder> r = new List<SaleOrder>();
            List<SaleOrder> rOnce = new List<SaleOrder>();
            r = getSaleOrderOnce(cusCode: cusCode, lastnum: lastnum);
            //查询上年数据
            if (r.Count < lastnum)
            {
                //获得上年数据连接
                if (null != getPrevYearConn())
                {
                    if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                        sqlcmd.Connection.Close();
                    sqlcmd.Connection = new SqlConnection(getPrevYearConn().ConnectionString); 
                    Context = new FluentData.DbContext().ConnectionString(sqlcmd.Connection.ConnectionString, new FluentData.SqlServerProvider());

                    rOnce = getSaleOrderOnce(cusCode, lastnum - r.Count);
                    //还原当前年度数据库连接
                    if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed)
                        sqlcmd.Connection.Close();
                    sqlcmd.Connection = new SqlConnection(getCurrYearConn().ConnectionString);
                    Context = new FluentData.DbContext().ConnectionString(sqlcmd.Connection.ConnectionString, new FluentData.SqlServerProvider());

                }
                if (rOnce.Count > 0)
                    r.AddRange(rOnce);
            }

            return r;
        }
        private List<SaleOrder> getSaleOrderOnce(string cusCode, int lastnum)
        {
            List<SaleOrder> r = new List<SaleOrder>();
            SaleOrder d;
            foreach (SaleOrderMain dm in getSaleOrderMain(cusCode, lastnum))
            {
                d = new SaleOrder();
                d.Main = dm;
                if (!r.Exists(item => item.Main.id == d.Main.id))
                {
                    d.Details = getSaleOrderDetail(d.Main.id);
                    r.Add(d);
                }
            }
            return r;
        }
        public object getSaleOrderMainField(string field, string code)
        {
            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT TOP 1 " + field);
            cmd.Append(" FROM SaleOrderQ where 1 = 1 and cSOCode = '" + code + "'");
            return getFieldValue(cmd.ToString());
        }
        private SaleOrderMain getSaleOrderMain(string soCode)
        {
            SqlCommand sqlcmd = new SqlCommand() { CommandType = basesqlcmd.CommandType, Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString) };
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            SaleOrderMain m = null;
            sqlcmd.CommandText = string.Empty;
            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT top 1 ID,cSOCode,cCusCode,dDate,cCusOAddress,cMemo,cMaker,cVerifier ");
            cmd.Append(" FROM SO_SOMain where 1 = 1 and (cSoCode = '" + soCode + "' or cSoCode like '%" + soCode + "')");
            sqlcmd.CommandText = cmd.ToString();
            try
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Open) sqlcmd.Connection.Open();
                sqlreader = sqlcmd.ExecuteReader();
                while (sqlreader.Read())
                { //ID,cSOCode,cCusCode,dDate,cCusOAddress,cMemo,cMaker,cVerifier
                    m = new SaleOrderMain()
                    {
                        id = sqlreader.GetValue(sqlreader.GetOrdinal("ID")).ToString(),
                        Code = sqlreader.GetValue(sqlreader.GetOrdinal("cSoCode")).ToString(),
                        cShipAddress = sqlreader.GetValue(sqlreader.GetOrdinal("cCusOAddress")).ToString(),
                        dDate = sqlreader.GetDateTime(sqlreader.GetOrdinal("dDate")),
                        DWCode = sqlreader.GetValue(sqlreader.GetOrdinal("cCusCode")).ToString(),
                        Maker = sqlreader.GetValue(sqlreader.GetOrdinal("cMaker")).ToString(),
                        Verifier = sqlreader.GetValue(sqlreader.GetOrdinal("cVerifier")).ToString(),
                        Memo = sqlreader.GetValue(sqlreader.GetOrdinal("cMemo")).ToString()
                    };
                    //r.Main = m;
                }
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed) sqlcmd.Connection.Close();
            }
            catch
            {
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed) sqlcmd.Connection.Close();
                throw;
            }
            m.customer = getCustomer(m.DWCode);
            return m;
        }

        private List<SaleOrderMain> getSaleOrderMain(string cusCode, int lastnum)
        {
            SqlCommand sqlcmd = new SqlCommand() {CommandType = basesqlcmd.CommandType,Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString)};
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            List<SaleOrderMain> r = new List<SaleOrderMain>();
            SaleOrderMain m = new SaleOrderMain();
            sqlcmd.CommandText = string.Empty;
            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT ID,cSOCode,cCusCode,dDate,cMemo,cMaker,cVerifier ");
            cmd.Append(" FROM SO_SOMain where 1 = 1 and id in ( ");
            cmd.Append(" select top " + lastnum.ToString() + " id from SO_SOMain ");
            cmd.Append(" where 1 = 1 and cCusCode in (" + getRefCusCode(cusCode) + ")");
            cmd.Append(" order by dDate desc  )");
            sqlcmd.CommandText = cmd.ToString();
            try
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Open) sqlcmd.Connection.Open();
                sqlreader = sqlcmd.ExecuteReader();
                while (sqlreader.Read())
                { // DLID, cDLCode, cVouchType, dDate, cShipAddress, cMemo, cMaker, cVerifier
                    m = new SaleOrderMain()
                    {
                        id = sqlreader.GetValue(sqlreader.GetOrdinal("ID")).ToString(),
                        Code = sqlreader.GetValue(sqlreader.GetOrdinal("cSOCode")).ToString(),
                        dDate = sqlreader.GetDateTime(sqlreader.GetOrdinal("dDate")),
                        DWCode = sqlreader.GetValue(sqlreader.GetOrdinal("cCusCode")).ToString(),
                        Maker = sqlreader.GetValue(sqlreader.GetOrdinal("cMaker")).ToString(),
                        Verifier = sqlreader.GetValue(sqlreader.GetOrdinal("cVerifier")).ToString(),
                        Memo = sqlreader.GetValue(sqlreader.GetOrdinal("cMemo")).ToString()
                    };
                    if (!r.Exists(item => item.id == m.id)) r.Add(m);
                }
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed) sqlcmd.Connection.Close();
            }
            catch
            {
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed) sqlcmd.Connection.Close();
                throw;
            }
            return r;
        }
        private List<Detail> getSaleOrderDetail(string mainId)
        {
            SqlCommand sqlcmd = new SqlCommand() { CommandType = basesqlcmd.CommandType, Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString) };
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            List<Detail> r = new List<Detail>();
            SaleOrderDetail d = new SaleOrderDetail();
            sqlcmd.CommandText = string.Empty;
            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT SO_SODetails.AutoID, SO_SODetails.cInvCode, Inventory.cInvName, Inventory.cInvStd, ");
            cmd.Append(" ComputationUnit.cComUnitName, isnull(SO_SODetails.iQuantity,0) as iQuantity, isnull(SO_SODetails.iNum,0) as iNum, ");
            cmd.Append(" isnull(SO_SODetails.iTaxUnitPrice,0) as iTaxUnitPrice, isnull(SO_SODetails.iSum,0) as iSum, SO_SODetails.cMemo FROM  SO_SODetails ");
            cmd.Append(" INNER JOIN  Inventory ON  SO_SODetails.cInvCode =  Inventory.cInvCode ");
            cmd.Append(" INNER JOIN  ComputationUnit ON  Inventory.cComUnitCode = ComputationUnit.cComunitCode ");
            cmd.Append(" where 1 = 1 and SO_SODetails.ID = '" + mainId + "'");
            sqlcmd.CommandText = cmd.ToString();
            try
            {
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Open) sqlcmd.Connection.Open();
                sqlreader = sqlcmd.ExecuteReader();
                while (sqlreader.Read())
                { /*SaleOrderLists.AutoID, SaleOrderLists.cInvCode, Inventory.cInvName, Inventory.cInvStd,
 ComputationUnit.cComUnitName, SaleOrderLists.iQuantity, SaleOrderLists.iNum,
 SaleOrderLists.iTaxUnitPrice, SaleOrderLists.iSum, SaleOrderLists.cMemo*/
                    d = new SaleOrderDetail()
                    {
                        autoid = sqlreader.GetValue(sqlreader.GetOrdinal("AutoID")).ToString(),
                        inventory = new Inventory()
                        {
                            cInvCode = sqlreader.GetValue(sqlreader.GetOrdinal("cInvCode")).ToString(),
                            cInvName = sqlreader.GetValue(sqlreader.GetOrdinal("cInvName")).ToString(),
                            cInvStd = sqlreader.GetValue(sqlreader.GetOrdinal("cInvStd")).ToString(),
                            cUnit = sqlreader.GetValue(sqlreader.GetOrdinal("cComUnitName")).ToString()
                        },
                        quantity = new Quantity()
                        {
                            iQuantity = double.Parse(sqlreader.GetValue(sqlreader.GetOrdinal("iQuantity")).ToString()),
                            iNum = double.Parse(sqlreader.GetValue(sqlreader.GetOrdinal("iNum")).ToString())
                        },
                        iPrice = double.Parse(sqlreader.GetValue(sqlreader.GetOrdinal("iTaxUnitPrice")).ToString()),
                        iSum = double.Parse(sqlreader.GetValue(sqlreader.GetOrdinal("iSum")).ToString()),
                        Memo = sqlreader.GetValue(sqlreader.GetOrdinal("cMemo")).ToString()
                    };
                    if (!r.Exists(item => item.autoid == d.autoid)) r.Add(d);
                }
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed) sqlcmd.Connection.Close();
            }
            catch
            {
                if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
                if (sqlcmd.Connection.State != System.Data.ConnectionState.Closed) sqlcmd.Connection.Close();
                throw;
            }
            return r;
        }

        public bool updateSaleOrderField(string field, string value, string code)
        {
            bool r = false;
            StringBuilder cmd = new StringBuilder();
            cmd.Append("update so_somain set " + field + " = '" + value + "' where csocode = '" + code + "'");
            r = update(cmd.ToString());
            return r;
        }
    }
}
