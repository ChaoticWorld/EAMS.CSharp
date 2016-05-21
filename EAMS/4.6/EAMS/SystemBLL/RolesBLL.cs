using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemDB;

namespace SystemBLL
{
    public class RolesBLL
    {
        protected static SystemDB.dbRoles OpRole = new SystemDB.dbRoles();
        protected static SystemDB.dbUserRoleRefs OpUserRoleRefs = new dbUserRoleRefs();
        public string returnMsg { get { return OpRole.lastMsg; } }
        /// <summary>
        /// 验证字段值是否被使用,使用范围:注册,
        /// </summary>
        /// <param name="kv">单个键值对KeyValuePair<string,string></param>
        /// <returns></returns>
        public static bool Exist(string _key,string _value)
        {
            bool r = OpRole.select("it." + _key + " == " + _value) != null ? true : false;
            return r;
        }
        public static bool Exist(int _id)
        { return OpRole.Exist(_id); }
        public static bool ExistUser(int uid)
        {
            bool r = false;
            var exist = OpUserRoleRefs.selectRoles(uid);
            if (exist == null || exist.Count() <= 0) r = false;
            else r = true;
            return r;
        }

        /// <summary>
        /// 记录列表
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns></returns>
        public IEnumerable<Role> Lists(string strWhere)
        {
            return (IEnumerable<SystemDB.Role>)OpRole.select(strWhere);
        }
        public IEnumerable<Object> Lists()
        {
            return OpRole.select(); 
        }

        /// <summary>
        /// 更新记录,返回更新记录数
        /// </summary>
        /// <param name="_u">用户类实体</param>
        /// <returns></returns>
        public int Update(object _u)
        {
            return OpRole.updata(_u); 
        }

        /// <summary>
        /// 返回指定id的用户实例
        /// </summary>
        /// <param name="_id">用户id</param>
        /// <returns></returns>
        public Object Single(int _id)
        {
            SystemDB.Role r = (SystemDB.Role)OpRole.single(_id);
            if (r.Users == null) r.Users = new List<SystemDB.User>();
            r.Users.Clear();
            foreach (SystemDB.UserRoleRef urr in r.UserRoleRefs)
                r.Users.Add(urr.User);
            return r;
        }
        public IEnumerable<User> getUsersWithRole(string rName)
        {
            SystemDB.Role r = (SystemDB.Role)OpRole.single(rName);
            if (r.Users == null) r.Users = new List<SystemDB.User>();
            r.Users.Clear();
            foreach (SystemDB.UserRoleRef urr in r.UserRoleRefs)
                r.Users.Add(urr.User);
            return r.Users;
        }
        public List<Role> getRolesWithUser(int uid)
        {
            List<Role> r = null;
            IEnumerable<UserRoleRef> URRs = (IEnumerable<UserRoleRef>)OpUserRoleRefs.selectRoles(uid);

            if (URRs != null && URRs.Count() > 0)
            {
                r = new List<Role>();
                foreach (var urr in URRs)
                {
                    if (!r.Exists(re => re.iRoleId == urr.iRoleId))
                        r.Add(urr.Role);
                }
            }

            return r;
        }
        /// <summary>
        /// 删除指定id的用户实例
        /// </summary>
        /// <param name="_id">用户id列表</param>
        /// <returns></returns>
        public int Delete(List<int> _l)
        {
            int r = 0;
            foreach (int i in _l)
                r += Delete(i);
            return r;
        }
        public int Delete(int _id)
        {
            return OpRole.delete(_id);
        }

        /// <summary>
        /// 增加记录,返回新增数据记录的主键值,失败返回string.Empty
        /// </summary>
        /// <param name="_o"></param>
        /// <returns></returns>
        public string Add(SystemDB.Role _o)
        {            
            return OpRole.add(_o);
        }

        //public List<SystemDB.User> ListUsers(int RoleId)
        //{
        //    List<SystemDB.User> r = new List<SystemDB.User>();
        //    UserBLL OpUser = new UserBLL();
        //    SystemDB.dbUserRoleRefs Op = new SystemDB.dbUserRoleRefs();
        //    foreach (SystemDB.UserRoleRef i in Op.selectUsers(RoleId))
        //        r.Add((SystemDB.User)OpUser.Single(i.iUserId));
        //    return r;
        //}
    }
}

