using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using IDataAccess;
using FluentData;
using System.Data;

namespace report
{
    public class reportDataSourceAccess : abstractDataAccess
    {
        public DataTable DataTableSource { get { return _dt; } }
        private DataTable _dt;
        public reportDataSourceAccess() {
            Context = erpContextBase.Context;
        }

        public void setQueryCommand(string queryCmd) {
            setBaseQuery(queryCmd);
        }
        public void setYearDB(int yearDB) { //erp年度帐数据库
            Context = erpContextBase.getYearDB(yearDB);
        }
        public void getDataSource(string dateField = null, int year = -1,string personField = "",string personName = "",string OrderString = "")
        {
            _dt = new DataTable();
            int currYear = DateTime.Now.Year;
            string date = string.Empty;

            if (year < 0) year = currYear;
            while (year <= currYear)
            {
                setYearDB(year);
                if (!string.IsNullOrEmpty(dateField))
                    date = " and year(" + dateField + ") >= '" + year + "'";
                var ds = getDataSource(date,personName,personField, OrderString);
                if (ds != null && ds.Rows.Count > 0)
                {
                    if (_dt.Rows.Count <= 0) _dt = ds;
                    else foreach (DataRow dr in ds.Rows) _dt.Rows.Add(dr.ItemArray);
                }
                year++;
            }
        }

        public DataTable getDataSource(string date,string personName, string personField = "", string OrderString = "") {
            string QueryCmd = BaseQuery + date + customerAuthenWhereStr(personField,personName) + OrderString;
            DataTable dt = Context.Sql(QueryCmd).QuerySingle<DataTable>();
            return dt;
        }
        /// <summary>
        /// 客户权限查寻条件
        /// </summary>
        /// <param name="userName">员工名</param>
        /// <returns></returns>
        private string customerAuthenWhereStr(string authenField = null, string userName = null)
        {
            StringBuilder r = new StringBuilder();
            string authenSql = string.Empty;
            string erpUserID = string.Empty;
            List<string> authGroupIds = new List<string>();
            if (!string.IsNullOrEmpty(userName))
            {
                authenSql = "SELECT cUser_Id FROM UFSYSTEM.DBO.UA_USER WHERE CUSER_NAME = @pName";
                erpUserID = Context.Sql(authenSql).Parameter("pName", userName).QuerySingle<string>();
                if (string.IsNullOrEmpty(erpUserID)) {
                    r.Append(" and 1 = 2 ");
                    return r.ToString();
                }

                authenSql = "select cACCode from aa_holdauth where 1 = 1 and cBusObId = N'customer' And isUserGroup = 0 and cFuncID <> 'N' and cUserId = '" + erpUserID + "' ";
                authGroupIds = Context.Sql(authenSql).QueryMany<string>();
                if (authGroupIds!=null && authGroupIds.Count>0)
                {
                    string authGroupLike = string.Empty;
                    authenSql = string.Empty;
                    foreach (string id in authGroupIds)
                        authenSql += "select id from AA_AuthClass where cBusObId = N'customer' and cACCode like '" + id + "%' union ";
                    authenSql = authenSql.Remove(authenSql.Length - 6);
                    authenSql += " order by id ";
                    var authIDs = Context.Sql(authenSql).QueryMany<string>();
                    if (authIDs != null && authIDs.Count > 0)
                    {
                        r.Append(" and " + (string.IsNullOrEmpty(authenField) ? "iid" : authenField) + " in (");
                        foreach (string i in authIDs)
                        {
                            r.Append("'" + i + "',");
                        }
                        r.Remove(r.Length - 1, 1);
                        r.Append(") ");
                    }
                }
            }
            else { throw new Exception("未能获取用户名，请登录！"); }
            return r.ToString();
        }

        public string getErpAuthSQLWhere(string personName, string personField = "")
        {
            string r = string.Empty;
            string QueryCmd = string.Empty;
            QueryCmd = "select Department.cDepMemo as 'visit',cpersonCode FROM PERSON left join Department on Person.cDepCode = Department.cDepCode WHERE CPERSONNAME = '@personName'";//visit:visibleAll可访问所有数据，visiableSelf可访问自身数据，invisiable不可访问。
            List<string> visit = Context.Sql(QueryCmd).Parameter("personName",personName).QuerySingle<List<string>>();

            if (visit != null && visit.Count > 0)
                switch (visit[0])
                {
                    case "visibleAll":
                        r = "";
                        break;
                    case "visibleSelf":
                        r = " and " + personField + " = '" + visit[1] + "'";
                        break;
                    case "invisible":
                        r = " and 1 <> 1";
                        break;
                    default: break;
                }
            else r = "";// " and 1 <> 1";
            return r;
        }
        //public void arrangeDataSource(Report report) {
        //    List<string> groupFieldNames = new List<string>();
        //    groupFieldNames = report.groupFields.FindAll(f=>f.isDisplay)
        //        .Select(s => s.field).Select(ss=>ss.fieldName).ToList();
        //    DataColumnCollection dtColumns = DataTableSource.Columns;
        //    for (int r = 0; r < DataTableSource.Rows.Count; r++)
        //    {
        //        var rCell = DataTableSource.Rows[r].ItemArray;
        //        for (int c = 0; c < rCell.Length; c++)
        //        {
        //            if (rCell[c] == null || rCell[c].Equals(string.Empty))
        //            {
        //                if (groupFieldNames.Contains( dtColumns[c].ColumnName))
        //                {
        //                    if (c == 0) rCell[c] = "合计";
        //                    else rCell[c] = "小计";
        //                }
        //                break;
        //            }
        //        }
        //    }
        //}
    }
}
