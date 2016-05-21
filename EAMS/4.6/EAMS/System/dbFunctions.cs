using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBAccessBase;
using System.Data.Objects;

namespace SystemDB
{
    public class dbFunctions : sysDB, IDBAccess
    {
        private static string BaseQuery = @"SELECT [iFunctionId],[cFunctionLevel],[cFunctionName],[cFunctionDescription],[cFunctionCommandGo],[bLog] FROM [Functions] ";
        public dbFunctions()
        { }
        ~dbFunctions()
        { }

        /// <summary>
        /// 新增数据,返回新增数据记录的主键值,失败返回string.Empty
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回新增数据记录的主键值,失败返回string.Empty</returns>
        public string add(object _u)
        {
            Functions u = (Functions)_u;
            appSystemEntity.Functions.AddObject(u);
            try
            {
                appSystemEntity.SaveChanges();
                MasterKey = u.iFunctionId.ToString();
            }
            catch { MasterKey = string.Empty; }
            return MasterKey;
        }


        /// <summary>
        /// 查询单一结果,返回单一object
        /// </summary>
        /// <param name="id">查询主键id</param>
        /// <returns>返回单一object</returns>
        public Object single(int id)
        {
            Functions r = new Functions();
            r = appSystemEntity.Functions.Single(s => s.iFunctionId == id);
            return r;
        }
        public Object single(string name)
        {
            Functions r = new Functions();
            r = appSystemEntity.Functions.Single(s => s.cFunctionName == name);
            return r;
        }
        /// <summary>
        /// 查询数据,返回IEnumerable
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns>返回IEnumerable可查询结果</returns>
        public IEnumerable<Object> select(string strWhere)
        {
            ObjectQuery<Functions> selected;
            //以Context.ObjectQuery.Where(paramStr)方法实现
            //(参数)的写法:string paramStr = "it.iUserID >= 0";
            try { selected = appSystemEntity.Functions.Where(strWhere); }
            catch { selected = null; }
            return selected;
        }
        public IEnumerable<Object> select()
        { return appSystemEntity.Functions; }

        /// <summary>
        /// 更新数据,返回影响的记录数
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回影响的记录数</returns>
        public int updata(object u)
        {
            Functions _u = (Functions)u;
            int r = -1;
            var upd = appSystemEntity.Functions.Single(s => s.iFunctionId == _u.iFunctionId);
            //upd = _u;
            upd.bLog = _u.bLog;
            upd.cFunctionCommandGo = _u.cFunctionCommandGo;
            upd.cFunctionDescription = _u.cFunctionDescription;
            upd.cFunctionLevel = _u.cFunctionLevel;
            upd.cFunctionName = _u.cFunctionName;
            
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

            var d = appSystemEntity.Functions.Single(s => s.iFunctionId == id);
            appSystemEntity.DeleteObject(d);
            r += appSystemEntity.SaveChanges();

            Records = r;
            return r;
        }
        /// <summary>
        /// 对象是否存在
        /// </summary>
        /// <param name="_u">动作名称FunctionsName</param>
        /// <returns></returns>
        public bool Exist(object _u)
        {
            string u = (string)_u;
            bool r = false;
            try
            {
                var ra = appSystemEntity.Functions.First(a => a.cFunctionName == u);
                r = true;
            }
            catch { r = false; }
            return r;
        }

    }

}
