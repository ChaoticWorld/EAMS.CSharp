using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DataDB.ModelBase;
using System.Data.Entity;
using System.Data.SqlClient;
//处理存货信息
namespace DataDB.u8
{
    /// <summary>
    /// 方法操作
    /// 处理存货信息
    /// </summary>
    public partial class dbU8
    {
        /// <summary>
        /// 系统中是否存在存货
        /// </summary>
        /// <param name="code">存货编码</param>
        /// <returns></returns>
        public bool isExistsInventory(string code)
        {
            StringBuilder cmd = new StringBuilder();
            bool r = false;
            cmd.Clear();
            cmd.Append("Select top 1 [cInvCode] From [Inventory] where 1 = 1 ");
            cmd.Append(" AND [cInvCode] like '%" + code + "%' ");
            r = isExistSqlSelect(cmd.ToString());

            return r;
        }

        /// <summary>
        /// 返回存货列表，
        /// </summary>
        /// <param name="code">存货编码</param>
        /// <param name="name">存货名称</param>
        /// <param name="std" >存货型号</param>
        /// <returns></returns>
        public List<Inventory> getInventorys(string code = null, string name = null, string std = null, string ccode = null)
        {
            SqlDataReader sdr;
            List<Inventory> r = new List<Inventory>();
            StringBuilder invcodes = new StringBuilder();
            StringBuilder cmd = new StringBuilder();
            Inventory inv = new Inventory();

            cmd.Append(" select [cInvCode],[cInvName],[cInvStd],ComputationUnit.cComUnitName ,[cInvCCode] ");
            cmd.Append(" from [inventory] ");
            cmd.Append(" LEFT JOIN [ComputationUnit] on Inventory.cComUnitCode = ComputationUnit.cComunitCode ");
            cmd.Append(" where 1 = 1 ");
            if (!string.IsNullOrEmpty(code))
                cmd.Append(" and [cInvCode] like '%" + code + "%' ");
            if (!string.IsNullOrEmpty(name))
                cmd.Append(" and [cInvName] like '%" + name + "%' ");
            if (!string.IsNullOrEmpty(std))
                cmd.Append(" and [cInvStd] like '%" + std + "%' ");
            if (!string.IsNullOrEmpty(ccode))
                cmd.Append(" and [cInvCCode] like '" + ccode + "%'");
            SqlCommand sc = new SqlCommand() {CommandType = basesqlcmd.CommandType,Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString)}; sc.CommandText = cmd.ToString();
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            try
            {
                if (sc.Connection.State != System.Data.ConnectionState.Open)
                    sc.Connection.Open();
                sdr = sc.ExecuteReader();
                while (sdr.Read())
                {
                    inv = new Inventory()
                    {
                        cInvCode = sdr.GetValue(sdr.GetOrdinal("cInvCode")).ToString(),
                        cInvName = sdr.GetValue(sdr.GetOrdinal("cInvName")).ToString(),
                        cInvStd = sdr.GetValue(sdr.GetOrdinal("cInvStd")).ToString(),
                        cUnit = sdr.GetValue(sdr.GetOrdinal("cComUnitName")).ToString(),
                        cInvClass = sdr.GetValue(sdr.GetOrdinal("cInvCCode")).ToString()
                    };
                    inv.invClass = getInventoryClass(inv.cInvClass);
                    r.Add(inv);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            if (!sdr.IsClosed)  sdr.Close();
            if (sc.Connection.State != System.Data.ConnectionState.Closed) sc.Connection.Close();
            return r;
        }
        public Inventory getInventory(string code)
        {
            return getInventorys(code: code).First();
        }
        public List<InventoryClass> getInventoryClassList(List<string> CCodeList = null)
        {
            List<InventoryClass> r = new List<InventoryClass>();
            InventoryClass item = new InventoryClass();
            List<string> ClassLst = new List<string>();
            if (CCodeList != null) ClassLst = CCodeList;
            else  ClassLst = getInventorySubClassList();
            foreach (string ccode in ClassLst)
            {
                item = getInventoryClass(ccode);
                r.Add(item);
            }
            return r;
        }
        public InventoryClass getInventoryClass(string invCcode)
        {
            SqlDataReader sdr;
            InventoryClass r = new InventoryClass();
            StringBuilder cmd = new StringBuilder();
            int iInvCGrade = 0;
            string ccode = string.Empty;
            string cname = string.Empty;
            cmd.Append("select cInvCCode,cInvCName,iInvCGrade,bInvCEnd from InventoryClass ");
            cmd.Append(" where 1 = 1 ");
            cmd.Append(" and cInvCcode = '" + invCcode + "'");
            SqlCommand sc = new SqlCommand() {CommandType = basesqlcmd.CommandType,Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString)}; sc.CommandText = cmd.ToString();
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            try
            {
                if (sc.Connection.State != System.Data.ConnectionState.Open)
                    sc.Connection.Open();
                sdr = sc.ExecuteReader();
                while (sdr.Read())
                {
                    iInvCGrade = int.Parse(sdr.GetValue(sdr.GetOrdinal("iInvCGrade")).ToString());
                    ccode = sdr.GetValue(sdr.GetOrdinal("cInvCCode")).ToString();
                    cname = sdr.GetValue(sdr.GetOrdinal("cInvCName")).ToString();

                    r = new InventoryClass()
                    {
                        code = ccode,
                        name = cname,
                        upClass = upGradeCode("inventoryclass", ccode, iInvCGrade),
                        subClsCodes = getInventorySubClassList(ccode, iInvCGrade+1)
                    };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            if (!sdr.IsClosed)  sdr.Close();
            if (sc.Connection.State != System.Data.ConnectionState.Closed) sc.Connection.Close();
            
            return r;
        }
        public List<string> getInventorySubClassList(string code = null,int iInvCGrade = 1)
        {
            System.Data.SqlClient.SqlDataReader sdr;
            List<string> r = new List<string>();
            StringBuilder cmd = new StringBuilder();
            string ccode = string.Empty;
            string cname = string.Empty;
            cmd.Append("select cInvCCode,cInvCName,iInvCGrade,bInvCEnd from InventoryClass ");
            cmd.Append(" where 1 = 1 ");
            if (iInvCGrade > 1 && !string.IsNullOrEmpty(code))
                cmd.Append(" and cInvCCode like '" + code + "%' and iInvCGrade = " + iInvCGrade);
            else
                cmd.Append(" and iInvCGrade = 1");
            SqlCommand sc = new SqlCommand(cmd.ToString(),new SqlConnection(basesqlcmd.Connection.ConnectionString));
            try
            {
                   if (sc.Connection.State != System.Data.ConnectionState.Open)
                       sc.Connection.Open();
                    sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        iInvCGrade = int.Parse(sdr.GetValue(sdr.GetOrdinal("iInvCGrade")).ToString());
                        ccode = sdr.GetValue(sdr.GetOrdinal("cInvCCode")).ToString();
                        cname = sdr.GetValue(sdr.GetOrdinal("cInvCName")).ToString();

                        r.Add(ccode);
                    }
                }
            
            catch (Exception e)
            {
                throw e;
            }

            if (!sdr.IsClosed)  sdr.Close();
            if (sc.Connection.State != System.Data.ConnectionState.Closed) sc.Connection.Close();
            return r;
        }

        /// <summary>
        /// 编码规则，返回上级编码，否则返回string.Empty
        /// </summary>
        /// <param name="key">编码类型</param>
        /// <param name="Code">编码</param>
        /// <param name="currGrade">当前分类级别</param>
        /// <returns></returns>
        public string upGradeCode(string key,string Code,int currGrade) 
        {
            string r = string.Empty;
            string codingrule = GradeDefCode(key);// 当前编码规则
            //分析编码规则
            int endIdx = 0;
            int l = 0;
            foreach (char c in codingrule)
            {
                l=int.Parse(c.ToString());
                if (currGrade>1)
                    endIdx += l; 
                currGrade--;
            }
            r = Code.Substring(0, endIdx);
            return r;
        }
        private string GradeDefCode(string key)
        {
            SqlCommand sqlcmd = new SqlCommand() {CommandType = basesqlcmd.CommandType,Connection = new SqlConnection(basesqlcmd.Connection.ConnectionString)};
            if (sqlreader != null && !sqlreader.IsClosed) sqlreader.Close();
            /*LGRADE 最大级数 ,LGRADELEN 最长编码 ，iSerial 序号 ，GRADECLSNAME 中文说明 
            KEYWORD 编码表名 ， CODINGRULE 当前编码规则 */
            string r = string.Empty;
            StringBuilder cmd = new StringBuilder();
            string codingrule = string.Empty;
            cmd.Append("SELECT top 1 [keyword],[lgrade],[lgradelen],[codingrule],[iSerial],[Gradeclsname] ");
            cmd.Append(" FROM [GradeDef]");
            cmd.Append(" Where [keyword] = '" + key + "'");
            sqlcmd.CommandText = cmd.ToString();
            SqlCommand sc = new SqlCommand(cmd.ToString(), new SqlConnection(sqlcmd.Connection.ConnectionString));
            SqlDataReader sdr;
            try
            {
                if (sc.Connection.State != System.Data.ConnectionState.Open)
                    sc.Connection.Open();
                sdr = sc.ExecuteReader();
                while (sdr.Read())
                {
                    codingrule = sdr.GetValue(sdr.GetOrdinal("codingrule")).ToString();
                }
                if (!sdr.IsClosed)  sdr.Close();
                if (sc.Connection.State != System.Data.ConnectionState.Closed) sc.Connection.Close();
            }
            catch
            {
                throw;
            }
            if (!sdr.IsClosed) sdr.Close();
            if (sc.Connection.State != System.Data.ConnectionState.Closed) sc.Connection.Close();
            r = string.IsNullOrEmpty(codingrule) ? string.Empty : codingrule;
            return r;
        }

        public string getInvTopClass(string invcls)
        {
            string r = string.Empty;
            var cls = getInventoryClass(invcls);
            if (string.IsNullOrEmpty(cls.upClass)) r= invcls;
            else r = getInvTopClass(cls.upClass);
            return r;
        }
    }
}