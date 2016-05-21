using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using DBAccessBase;

namespace SystemDB
{
    public class dbLogs : sysDB
    {
        private static string BaseQuery 
            = @"SELECT [iLogId],[dLogDate],[iUserID],[cWorkStation],[cIP],[cBeforeValue],
                [cModule],[cFunction],[cParams],[cClass],[cReturn],[cException],[cUserName] FROM [Logs] ";
        public dbLogs()
        { }
        ~dbLogs()
        { }

        /// <summary>
        /// 新增数据,返回新增数据记录的主键值,失败返回string.Empty
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回新增数据记录的主键值,失败返回string.Empty</returns>
        public string add(object _u)
        {
            Logs u = (Logs)_u;
            appSystemEntity.Logs.AddObject(u);
            try
            {
                appSystemEntity.SaveChanges();
                MasterKey = u.iLogId.ToString();
            }
            catch (Exception e )
            { MasterKey = string.Empty; }
            return MasterKey;
        }


        /// <summary>
        /// 查询单一结果,返回单一object
        /// </summary>
        /// <param name="id">查询主键id</param>
        /// <returns>返回单一object</returns>
        public Object single(int id)
        {
            Logs r = new Logs();
            r = appSystemEntity.Logs.Single(s => s.iLogId == id);
            return r;
        }

        /// <summary>
        /// 查询数据,返回IEnumerable
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns>返回IEnumerable可查询结果</returns>
        public IEnumerable<Object> select(string strWhere)
        {
            ObjectQuery<Logs> selected;
            //以Context.ObjectQuery.Where(paramStr)方法实现
            //(参数)的写法:string paramStr = "it.iUserID >= 0";
            try { selected = appSystemEntity.Logs.Where(strWhere); }
            catch { selected = null; }
            return selected;
        }
        public IEnumerable<Object> select()
        {
            if ( null !=appSystemEntity.Logs && appSystemEntity.Logs.Count() > 0)
                return appSystemEntity.Logs;
            else
                return null;
        }

        /// <summary>
        /// 更新数据,返回影响的记录数
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回影响的记录数</returns>
        public int updata(object u)
        {
            Logs _u = u as Logs;
            int r = -1;
            var upd = appSystemEntity.Logs.Single(s => s.iLogId == _u.iLogId);
            //upd = _u;
            upd.cIP = _u.cIP;
            upd.cLastValue = _u.cLastValue;
            upd.cWorkStation = _u.cWorkStation;
            upd.dLogDate = _u.dLogDate;
            upd.iActionId = _u.iActionId;
            upd.iFunctionId = _u.iFunctionId;
            upd.iUserID = _u.iUserID;
            
            r = appSystemEntity.SaveChanges();
            Records = r;
            return r;
        }

        /// <summary>
        /// 删除数据,返回影响的记录数
        /// </summary>
        /// <param name="int id">为删除主键id列表</param>
        /// <returns>返回影响的记录数</returns>
        public int delete(int id)
        {
            int r = 0;

            var d = appSystemEntity.Logs.Single(s => s.iLogId == id);
            appSystemEntity.DeleteObject(d);
            r += appSystemEntity.SaveChanges();

            Records = r;
            return r;
        }
    }

}
