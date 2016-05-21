using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using FluentData;
using System.Net;

namespace Logs
{
    public abstract class LogsDataAccess : abstractDataAccess
    {        
        public LogsDataAccess(string tblName) {
            TableName = tblName;
            setBaseQuery(@"SELECT [iLogId],[dLogDate],[iUserID],[cWorkStation],[cIP],[cBeforeValue],[cModule],[cFunction],[cParams],[cClass],[cReturn],[cException],[cUserName] FROM [" + TableName + "] where 1 = 1  ");
        }

        protected string WhereStr(LogModel m)
        {
            wStr = new StringBuilder();
            if (m != null)
            {
                if (m.iLogId > 0)
                    wStr.Append(" and iLogId = " + m.iLogId.ToString());
                if (m.iUserID > 0)
                    wStr.Append(" and iUserID = " + m.iUserID.ToString());
                if (!string.IsNullOrEmpty(m.cWorkStation))
                    wStr.Append(" and cWorkStation like '%" + m.cWorkStation + "%'");
                if (!string.IsNullOrEmpty(m.cUserName))
                    wStr.Append(" and cUserName like '%" + m.cUserName + "%'");
                if (!string.IsNullOrEmpty(m.cReturn))
                    wStr.Append(" and cReturn like '%" + m.cReturn + "%'");
                if (!string.IsNullOrEmpty(m.cParams))
                    wStr.Append(" and cparams like '%" + m.cParams + "%'");
                if (!string.IsNullOrEmpty(m.cModule))
                    wStr.Append(" and cModule like '%" + m.cModule + "%'");
                if (!string.IsNullOrEmpty(m.cIP))
                    wStr.Append(" and cIP like '%" + m.cIP + "%'");
                if (!string.IsNullOrEmpty(m.cFunction))
                    wStr.Append(" and cFunction like '%" + m.cFunction + "%'");
                if (!string.IsNullOrEmpty(m.cException))
                    wStr.Append(" and cException like '%" + m.cException + "%'");
                if (!string.IsNullOrEmpty(m.cClass))
                    wStr.Append(" and cClass like '%" + m.cClass + "%'");
                if (!string.IsNullOrEmpty(m.cBeforeValue))
                    wStr.Append(" and cBeforeValue like '%" + m.cBeforeValue + "%'");
            }
            //日期范围条件未处理
            return wStr.ToString();
        }
        protected void logMapper(LogModel m, IDataReader row)
        {

            m.iLogId = long.Parse(row.GetValue("iLogId").ToString());
            int userid;
            if (int.TryParse(row.IsDBNull("iUserID") ? "-999" : row.GetValue("iUserID").ToString(), out userid))
                m.iUserID = userid;
            else m.iUserID = -999;

            m.dLogDate = row.GetDateTime("dLogDate");
            //DateTime logDT;
            //if (DateTime.TryParse(row.GetString("dLogDate"), out logDT))
            //    m.dLogDate = logDT;
            m.cWorkStation = row.GetString("cWorkStation");
            m.cUserName = row.GetString("cUserName");
            m.cReturn = row.GetString("cReturn");
            m.cParams = row.GetString("cParams");
            m.cModule = row.GetString("cModule");
            m.cIP = row.GetString("cIP");
            m.cFunction = row.GetString("cFunction");
            m.cException = row.GetString("cException");
            m.cClass = row.GetString("cClass");
            m.cBeforeValue = row.GetString("cBeforeValue");
        }

        protected LogModel createLogModelPrep(LogModel lm)
        {
            string hostName = string.Empty;
            string ip4Str = string.Empty;
            string ip6Str = string.Empty;
            string ipStr = string.Empty;
            if (System.Web.HttpContext.Current == null)
            {//获取本地主机名和地址
                hostName = Dns.GetHostName();
                IPHostEntry ipe = Dns.GetHostEntry(hostName);

                foreach (IPAddress ip in ipe.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        ip4Str = ip.ToString();
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        ip6Str = ip.ToString();
                }
                ipStr = string.IsNullOrEmpty(ip4Str) ?
                    (string.IsNullOrEmpty(ip6Str) ? "" : ip6Str)
                    : ip4Str;
            }
            else
            {//获取Request主机名和地址
                hostName = System.Web.HttpContext.Current.Request.UserHostName;
                ipStr = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            lm.cIP = ipStr;
            lm.cWorkStation = hostName;
            lm.dLogDate = DateTime.Now;
            if (lm.iUserID < 1)
                lm.iUserID = -999;
            return lm;
        }

        public void createLog(LogModel m)
        {
            LogModel lm = createLogModelPrep(m);
            Context.Insert<LogModel>(TableName, lm).AutoMap(am => am.iLogId).ExecuteReturnLastId<int>();
        }

        public void deleteLog(int logId)
        {
            Context.Delete(TableName).Where("iLogId",logId).Execute();
        }

        public LogModel singleLog(int logId)
        {
            LogModel m = new LogModel();
            string sqlcmd = BaseQuery + WhereStr(new LogModel() { iLogId = logId });
            m = Context.Sql(sqlcmd).QuerySingle<LogModel>(logMapper);
            return m;
        }

        public IList<LogModel> selectLogs(LogModel m)
        {
            IList<LogModel> ms;
            string sqlcmd = BaseQuery + WhereStr(m);
            ms = Context.Sql(sqlcmd).QueryMany<LogModel>(logMapper);

            return ms;
        }
    }

}
