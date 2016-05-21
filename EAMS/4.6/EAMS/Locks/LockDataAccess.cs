using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using FluentData;

namespace Locks
{
    public class LockDataAccess : abstractDataAccess
    {
        public LockDataAccess(string tblName = "Lock")
        {
            Context = eamsAppSystemContextBase.Context;
            TableName = tblName;
            setBaseQuery("select autoId, [module],locker,[key],id,createDate from [" + TableName + "] where 1 = 1 ");
        }

        private string WhereStr(LockModel m)
        {
            wStr = new StringBuilder();
            if (m != null)
            {
                if (m.autoId > 0)
                    wStr.Append(" and autoId = " + m.autoId);
                if (!string.IsNullOrEmpty(m.id))
                    wStr.Append(" and id like '%" + m.id + "%'");
                if (!string.IsNullOrEmpty(m.key))
                    wStr.Append(" and [key] like '%" + m.key + "%'");
                if (!string.IsNullOrEmpty(m.locker))
                    wStr.Append(" and locker like '%" + m.locker + "%'");
                if (!string.IsNullOrEmpty(m.module))
                    wStr.Append(" and [module] like '%" + m.module + "%'");
            }
            return wStr.ToString();
        }
        private void Mapper(LockModel m, IDataReader row)
        {
            m.autoId = long.Parse(row.GetValue("autoId").ToString());
            m.createDate = row.GetDateTime("createDate");
            m.id = row.GetString("id");
            m.key = row.GetString("key");
            m.locker = row.GetString("locker");
            m.module = row.GetString("module");
        }
        public long createLock(LockModel m)
        {
            long r = -1;
            m.createDate = DateTime.Now;
            r =  Context.Insert(TableName, m).AutoMap(a=>a.autoId).ExecuteReturnLastId<long>();
            return r;
        }
        public int deleteLock(long autoid)
        {
            int r =
                Context.Delete(TableName).Where("autoId", autoid).Execute();
            return r;
        }
        public List<LockModel> select(LockModel m)
        {
            string sqlcmd = BaseQuery + WhereStr(m);
            var sels = Context.Sql(sqlcmd).QueryMany<LockModel>(Mapper);
            return sels;
        }
    }
}
