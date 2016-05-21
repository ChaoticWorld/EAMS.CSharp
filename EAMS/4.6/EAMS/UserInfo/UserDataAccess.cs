using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using IDataAccess;
using FluentData;
using Logs;

namespace UserInfo
{
    public class UserDataAccess:abstractDataAccess,IdbCRUD<UserModel>
    {
        private static UserLogs uLog = new UserLogs();
        public UserDataAccess() 
        {
            TableName = "UserInfo";
            Context = eamsAppSystemContextBase.getContext();
            setBaseQuery(" select iUserId,cUserCode,cUserName,cUserPassWord,cUserEMail,cUserMobile from [" + TableName + "] where 1 = 1 ");
        }
        protected string WhereStr(UserModel m)
        {
            wStr = new StringBuilder();
            if (m != null)
            {
                if (m.iUserId > 0)
                    wStr.Append(" and iUserId = " + m.iUserId.ToString());
                if (!string.IsNullOrEmpty(m.cUserCode))
                    wStr.Append(" and cUserCode like '%" + m.cUserCode + "%'");
                if (!string.IsNullOrEmpty(m.cUserName))
                    wStr.Append(" and cUserName like '%" + m.cUserName + "%'");
                if (!string.IsNullOrEmpty(m.cUserEMail))
                    wStr.Append(" and cUserEMail like '%" + m.cUserEMail + "%'");
                if (!string.IsNullOrEmpty(m.cUserMobile))
                    wStr.Append(" and cUserMobile like '%" + m.cUserMobile + "%'");
                if (!string.IsNullOrEmpty(m.cUserPassWord))
                    wStr.Append(" and cUserPassWord like '%" + m.cUserPassWord + "%'");
            }
            //日期范围条件未处理
            return wStr.ToString();
        }
        protected void UserMapper(UserModel m, IDataReader row)
        {
            int userid;
            if (int.TryParse(row.GetValue("iUserID").ToString(), out userid))
                m.iUserId = userid;
            m.cUserCode = row.GetString("cUserCode");
            m.cUserName = row.GetString("cUserName");
            m.cUserEMail = row.GetString("cUserEMail");
            m.cUserMobile = row.GetString("cUserMobile");
            m.cUserPassWord = row.GetString("cUserPassWord");
        }
        private void log(string message, string function)
        {
            uLog.createLog(new Logs.LogModel()
            {
                cModule = this.GetType().Name,
                cFunction = function,
                cParams = message
            });
        }
        private void log(UserModel m, string function)
        {
            LogModel logModel = new Logs.LogModel();
            logModel.cModule = "UserInfo";
            logModel.cFunction = function;
            if (null != m)
                logModel.cParams = "UserModel:" + m.cUserCode + ";" + m.cUserName + ";" + m.cUserEMail + ";" + m.cUserMobile;
            else logModel.cParams = "UserModel is Null!";
            uLog.createLog(logModel);
        }
        private long getMaxID() {
            long r = -1;
            string cmd = "select max(iUserId) from ["+TableName+"] where 1 = 1";
            r = Context.Sql(cmd).QuerySingle<long>();
            return r;
        }
        public long Create(UserModel um)
        {
            um.iUserId = getMaxID() + 1;

            Context.Insert<UserModel>(TableName, um)
            .Column("cUserCode", um.cUserCode)
            .Column("cUserEMail", um.cUserEMail)
            .Column("cUserMobile", um.cUserMobile)
            .Column("cUserName", um.cUserName)
            .Column("cUserPassWord", um.cUserPassWord)
            .Column("iUserId", um.iUserId)
            .Execute();
            log(um, "Create");
            return um.iUserId;
        }

        public int Delete(long id)
        {
            int r =
            Context.Delete(TableName).Where("iUserId", id).Execute();
            log(id.ToString(), "Delete");
            return r;
        }

        public List<UserModel> selects(UserModel t)
        {
            List<UserModel> ms = new List<UserModel>();
            string sqlcmd = BaseQuery + WhereStr(t);
            ms = Context.Sql(sqlcmd).QueryMany<UserModel>(UserMapper);
            log(t, "selects");
            return ms;
        }

        public UserModel Single(string code)
        {
            UserModel m = new UserModel();
            string sqlcmd = BaseQuery + WhereStr(new UserModel() { cUserName = code });
            m = Context.Sql(sqlcmd).QuerySingle<UserModel>(UserMapper);
            log(code, "Single");
            return m;
        }

        public UserModel Single(long id)
        {
            UserModel m = new UserModel();
            string sqlcmd = BaseQuery + WhereStr(new UserModel() { iUserId = id });
            m = Context.Sql(sqlcmd).QuerySingle<UserModel>(UserMapper);
            log(id.ToString(), "Single");
            return m;
        }

        public int Update(UserModel m)
        {
            int r = -1;
            r = Context.Update(TableName, m).AutoMap(am => am.iUserId)
                .Where(w => w.iUserId).Execute();
            log(m, "Update");
            return r;
        }
        
    }
}
