using IDataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.U8
{
    public class u8MultiTableQuery :u8Base,IMultiTableQuery
    {
        public string userName { get; set; }
        /// <summary>
        /// 客户权限查寻条件
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private string AuthenWhereStr()
        {
            StringBuilder r = new StringBuilder();
            string authenSql = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                authenSql = "select cACCode from aa_holdauth where 1 = 1 and cBusObId = N'customer' And isUserGroup = 0 and cFuncID <> 'N' and cUserId in ( "
                    + "SELECT cUser_Id FROM UFSYSTEM.DBO.UA_USER WHERE CUSER_NAME like '@pName')";
                var authGroupId = Context.Sql(authenSql).Parameter("pName", userName).QuerySingle<string>();
                authenSql = "select id from AA_AuthClass where cBusObId = N'customer' and cACCode like '" + authGroupId + "%' order by cACCode ";
                var authIDs = Context.Sql(authenSql).QueryMany<string>();
                if (authIDs != null && authIDs.Count > 0)
                {
                    r.Append("");
                    foreach (string i in authIDs)
                    {
                        r.Append("'" + i + "',");
                    }
                    r.Remove(r.Length - 1, 1);
                }
            }
            return r.ToString();
        }

        /// <summary>
        /// 查指定日期内，指定地区，指定客户编码的发货单号
        /// </summary>
        /// <param name="begin">开始日期</param>
        /// <param name="end">结果日期</param>
        /// <param name="cDCName">地区名称</param>
        /// <param name="cDWCode">客户编码</param>
        /// <returns>List<string> 发货单号</returns>
        public List<string> getDispatchMainCodesWithScope(DateTime begin, DateTime end, string cDCName, string cDWCode)
        {
            List<string> r = new List<string>();
            sqlcmd = new System.Text.StringBuilder();
            sqlcmd.Append(" select cdlcode from dispatchlist as dl left join Customer as cus on dl.ccuscode = cus.ccuscode left join DistrictClass as dc on cus.cDCcode = dc.cdccode  where 1 = 1 ");
            sqlcmd.Append( string.IsNullOrEmpty(cDCName) ? "" : " and (dc.cDCName = '" + cDCName + "' or dc.cDCName is null) ");
            sqlcmd.Append(" and dl.dDate >= '" + begin.ToString("yyyy-MM-dd") + "' and dl.dDate <= '" + end.ToString("yyyy-MM-dd") + "' ");
            sqlcmd.Append(!string.IsNullOrEmpty(cDWCode) ? " and dl.ccuscode = '" + cDWCode + "' " : " and cus.iid in (" + AuthenWhereStr() + ")");
            sqlcmd.Append(" order by ddate desc ");

            r = Context.Sql(sqlcmd.ToString()).QueryMany<string>();
            return r;
        }
        /// <summary>
        /// 查指定日期内，指定地区，指定客户编码的订单单号
        /// </summary>
        /// <param name="begin">开始日期</param>
        /// <param name="end">结果日期</param>
        /// <param name="cDCName">地区名称</param>
        /// <param name="cDWCode">客户编码</param>
        /// <returns>List<string> 订单号</returns>
        public List<string> getSODetailMainCodesWithScope(DateTime begin, DateTime end, string cDCName, string cDWCode)  {
            List<string> r = new List<string>();
            sqlcmd = new System.Text.StringBuilder();
            sqlcmd.Append(" select csocode from SO_SOMain as som left join Customer as cus on som.ccuscode = cus.ccuscode left join DistrictClass as dc on cus.cDCcode = dc.cdccode  where 1 = 1  ");
            sqlcmd.Append(string.IsNullOrEmpty(cDCName) ? "" : " and (dc.cDCName = '" + cDCName + "' or dc.cDCName is null) ");
            sqlcmd.Append(" and som.dDate >= '" + begin.ToString("yyyy-MM-dd") + "' and som.dDate <= '" + end.ToString("yyyy-MM-dd") + "' ");
            sqlcmd.Append(string.IsNullOrEmpty(cDWCode) ? " and som.ccuscode = '" + cDWCode + "' " : " and cus.iid in (" + AuthenWhereStr() + ")");
            sqlcmd.Append(" order by ddate desc ");

            r = Context.Sql(sqlcmd.ToString()).QueryMany<string>();
            return r;
        }
}
}
