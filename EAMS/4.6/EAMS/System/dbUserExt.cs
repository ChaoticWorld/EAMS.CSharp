using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using DBAccessBase;

namespace SystemDB
{
    public class dbUserExt : sysDB
    {
        private static string BaseQuery
            = @"SELECT [iUserId],[cUserAddress],[cUserPhone],[cUserIM],[cUserMasterPage] FROM [UserExt] ";
        public dbUserExt()
        { }
        ~dbUserExt()
        { }

        /// <summary>
        /// 新增数据,返回新增数据记录的主键值,失败返回string.Empty
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回新增数据记录的主键值,失败返回string.Empty</returns>
        public string add(object _u)
        {
            UserExt u = (UserExt)_u;
            appSystemEntity.UserExt.AddObject(u);
            try
            {
                appSystemEntity.SaveChanges(); 
                MasterKey = u.iUserId.ToString();
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
            UserExt r = new UserExt();
            try
            {
                r = appSystemEntity.UserExt.Single(s => s.iUserId == id);
            }
            catch { r = null; }
            return r;
        }

        /// <summary>
        /// 查询数据,返回IEnumerable
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns>返回IEnumerable可查询结果</returns>
        public IEnumerable<Object> select(string strWhere)
        {
            ObjectQuery<UserExt> selected;
            //以Context.ObjectQuery.Where(paramStr)方法实现
            //(参数)的写法:string paramStr = "it.iUserID >= 0";
            try
            { selected = appSystemEntity.UserExt.Where(strWhere); }
            catch { selected = null; }
            return selected;
        }
        public IEnumerable<Object> select()
        {
            return appSystemEntity.UserExt;
        }

        /// <summary>
        /// 更新数据,返回影响的记录数
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回影响的记录数</returns>
        public int updata(object u)
        {
            UserExt _u = (UserExt)u;
            int r = -1;
            var upd = appSystemEntity.UserExt.Single(s => s.iUserId == _u.iUserId);
            //upd = _u;
            upd.cUserAddress = _u.cUserAddress;
            upd.cUserIM = _u.cUserIM;
            upd.cUserMasterPage = _u.cUserMasterPage;
            upd.cUserPhone = _u.cUserPhone;

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

                var d = appSystemEntity.UserExt.Single(s => s.iUserId == id);
                appSystemEntity.DeleteObject(d);
                r += appSystemEntity.SaveChanges();
            
            Records = r;
            return r;
        }

                /// <summary>
        /// 对象是否存在
        /// </summary>
        /// <param name="_u">动作名称ActionName</param>
        /// <returns></returns>
        public bool Exist(int _u)
        {
            int u = (int)_u;
            bool r = false;
            try
            {
                var ra = appSystemEntity.UserExt.First(a => a.iUserId == u);
                r = true;
            }
            catch { r = false; }
            return r;
        }
    }    
}
