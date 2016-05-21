using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemDB;

namespace SystemBLL
{
    public class GroupsBLL
    {
        protected static SystemDB.dbGroups OpGroup = new SystemDB.dbGroups();
        protected static dbUserGroupRefs OpUserGroupRefs = new dbUserGroupRefs();
        public string returnMsg { get { return OpGroup.lastMsg; } }
        /// <summary>
        /// 验证字段值是否被使用,使用范围:注册,
        /// </summary>
        /// <param name="kv">单个键值对KeyValuePair<string,string></param>
        /// <returns></returns>
        public static bool Exist(string _key,string _value)
        {
            bool r = OpGroup.select("it." + _key + " == " + _value) != null ? true : false;
            return r;
        }
        public static bool Exist(int _id)
        { return OpGroup.Exist(_id); }
        public static bool ExistUser(int uid) {
            bool r = false;
            var exist = OpUserGroupRefs.selectGroups(uid);
            if (exist == null || exist.Count() <= 0) r = false;
            else r = true;
            return r;
        }

        /// <summary>
        /// 记录列表
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns></returns>
        public IEnumerable<Group> Lists(string strWhere)
        {
            return (IEnumerable<SystemDB.Group>)OpGroup.select(strWhere);
        }
        public IEnumerable<Object> Lists()
        {
            return OpGroup.select();
        }

        /// <summary>
        /// 更新记录,返回更新记录数
        /// </summary>
        /// <param name="_u">用户类实体</param>
        /// <returns></returns>
        public int Update(object _u)
        {
            return OpGroup.updata(_u); 
        }

        /// <summary>
        /// 返回指定id的组实例
        /// </summary>
        /// <param name="_id">组id</param>
        /// <returns></returns>
        public Object Single(int _id)
        {
            SystemDB.Group g = (SystemDB.Group)OpGroup.single(_id);
            SystemDB.dbUser dbu = new dbUser();
            if (g.Users == null) g.Users = new List<SystemDB.User>();
            g.Users.Clear();
            User u;
            foreach (SystemDB.UserGroupRef urr in g.UserGroupRefs)
            {
                u = (User)dbu.single(urr.UserId.Value);
                u.isGroupAdmin = urr.isManager.Value;
                g.Users.Add(u);
            }
            return g;
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
            return OpGroup.delete(_id);
        }

        /// <summary>
        /// 增加记录,返回新增数据记录的主键值,失败返回string.Empty
        /// </summary>
        /// <param name="_o"></param>
        /// <returns></returns>
        public string Add(SystemDB.Group _o)
        {            
            return OpGroup.add(_o);
        }

        public List<Group> getGroupsWithEmpID(int emdID)
        {
            List<Group> r = null;
            IEnumerable<UserGroupRef> UserGroupRefs =
            OpUserGroupRefs.selectGroups(emdID);
            if (UserGroupRefs != null && UserGroupRefs.Count() > 0)
            {
                r = new List<Group>();
                foreach (UserGroupRef ugr in UserGroupRefs)
                {
                    if (!r.Exists(re => re.groupid == ugr.groupId))
                        r.Add(ugr.Group);
                }
            }
            return r;
        }
        public List<User> getUsersWithGroupID(int groupId)
        {
            List<User> r = new List<User>();
            UserBLL uBll = new UserBLL();
            IEnumerable<UserGroupRef> UserGroupRefs =
            OpUserGroupRefs.selectUsers(groupId);
            if (UserGroupRefs != null && UserGroupRefs.Count() > 0)
                foreach (UserGroupRef ugr in UserGroupRefs)
                {
                    if (!r.Exists(re => re.iUserId == ugr.UserId))
                        r.Add(uBll.Single(ugr.UserId.Value) as User);
                }
            return r;
        }
        public List<Group> getGroupsEmpIsAdmin(int empId)
        {
            List<Group> r = new List<Group>();
            IEnumerable<UserGroupRef> UserGroupRefs = OpUserGroupRefs.selectGroups(empId);
            foreach (UserGroupRef ugr in UserGroupRefs)
                if (ugr.isManager.Value && !r.Exists(e => e.groupid == ugr.groupId))
                    r.Add(ugr.Group);
            return r;
        }
    }
}

