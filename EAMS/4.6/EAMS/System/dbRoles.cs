using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using DBAccessBase;

namespace SystemDB
{
    public partial class Role
    {
        public List<User> Users { get; set; }
    }
    public class dbRoles : sysDB, IDBAccess
    {
        public string lastMsg { get; private set; }
        private static string BaseQuery = @"SELECT [iRoleId],[cRoleName],[cRoleDescription] FROM [Roles] ";
        public dbRoles()
        { lastMsg = "数据处理错误"; }
        ~dbRoles()
        { }

        /// <summary>
        /// 新增数据,返回新增数据记录的主键值,失败返回string.Empty
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回新增数据记录的主键值,失败返回string.Empty</returns>
        public string add(object _u)
        {
            Role u = (Role)_u;
            appSystemEntity.Roles.AddObject(u);
            try
            {
                appSystemEntity.SaveChanges();
                MasterKey = u.iRoleId.ToString();
            }
            catch (System.Data.OptimisticConcurrencyException e)
            {
                MasterKey = string.Empty;
                lastMsg = "新数据保存失败。"+e.Message;
            }
            return MasterKey;
        }


        /// <summary>
        /// 查询单一结果,返回单一object
        /// </summary>
        /// <param name="id">查询主键id</param>
        /// <returns>返回单一object</returns>
        public Object single(int id)
        {
            Role r = new Role();
            r = appSystemEntity.Roles.Single(s => s.iRoleId == id);
            return r;
        }
        public Object single(string name)
        {
            Role r = new Role();
            r = appSystemEntity.Roles.Single(s => s.cRoleName == name);
            return r;
        }
        /// <summary>
        /// 查询数据,返回IEnumerable
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns>返回IEnumerable可查询结果</returns>
        public IEnumerable<Object> select(string strWhere)
        {
            ObjectQuery<Role> selected;
            //以Context.ObjectQuery.Where(paramStr)方法实现
            //(参数)的写法:string paramStr = "it.iUserID >= 0"
            if (!string.IsNullOrEmpty(strWhere))
                selected = appSystemEntity.Roles.Where(strWhere);
            else
                selected = appSystemEntity.Roles.Where("1=1");
            return selected;
        }
        /// <summary>
        /// 返回所有对象
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Object> select()
        { return appSystemEntity.Roles.Where("1=1"); }

        /// <summary>
        /// 更新数据,返回影响的记录数
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回影响的记录数</returns>
        public int updata(object u)
        {
            Role _u = (Role)u;
            int r = -1;
            var upd = appSystemEntity.Roles.Single(s => s.iRoleId == _u.iRoleId);
            //upd = _u;
            upd.cRoleName = _u.cRoleName;
            upd.cRoleDescription = _u.cRoleDescription;
            try
            {
                r = appSystemEntity.SaveChanges();
                Records = r;
                lastMsg = "更新记录数：" + Records.ToString();
            }
            catch (System.Data.OptimisticConcurrencyException e)
            {
                Records = -1;
                lastMsg = "更新失败。" + e.Message;
            }
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

            try
            {
                var d = appSystemEntity.Roles.Single(s => s.iRoleId == id);
                appSystemEntity.DeleteObject(d);
                r += appSystemEntity.SaveChanges();

                Records = r;
                lastMsg = "删除记录数："+Records.ToString();
            }
            catch (System.Data.OptimisticConcurrencyException e)
            {
                Records = -1;
                lastMsg = "删除数据失败。" + e.Message;
            }
            return r;
        }
        /// <summary>
        /// 对象是否存在
        /// </summary>
        /// <param name="_u">动作名称RoleName</param>
        /// <returns></returns>
        public bool Exist(object _u)
        {
            string u = (string)_u;
            bool r = false;
            try
            {
                var ra = appSystemEntity.Roles.First(a => a.cRoleName == u);
                r = true;
            }
            catch { r = false; }
            return r;
        }

    }

}
