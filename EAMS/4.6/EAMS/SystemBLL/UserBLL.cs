using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemDB;
using System.Web;

namespace SystemBLL
{
    public class UserBLL
    {
        protected static SystemDB.dbUser OpUser = new SystemDB.dbUser();

        /// <summary>
        /// 认证
        /// </summary>
        /// <returns></returns>
        public static bool Authentication(SystemDB.User _u)
        {
            bool r = false;
            int AuthUserId = OpUser.Authentication(_u);
            if (AuthUserId>=0)
            {
                r = true;
                WebCommon.SessionState.Set("AuthUser", OpUser.single(AuthUserId));
            }
            return r;
        }

        /// <summary>
        /// 验证字段值是否被使用,使用范围:注册,
        /// </summary>
        /// <param name="kv">单个键值对KeyValuePair<string,string></param>
        /// <returns></returns>
        public static bool UserExist(string _key,string _value)
        {
            bool r = false;
            r = OpUser.Exist(new KeyValuePair<string, string>(_key, _value));
            return r;
        }

        /// <summary>
        /// 注册新用户,返回真注册成功,返回假注册用户已存在
        /// </summary>
        /// <param name="_u">注册基本信息</param>
        /// <returns></returns>
        public static bool Register(SystemDB.User _u)
        {
            bool r = false;
            //检查是否已存在
            if (UserExist("cUserCode", _u.cUserCode)
                ||UserExist("cUserEMail", _u.cUserEMail)
                ||UserExist("cUserMobile", _u.cUserMobile)
                )
                r = false;
            else
                //新增用户
                r = !OpUser.add(_u).Equals(string.Empty);
            return r;
        }

        /// <summary>
        /// 记录列表
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns></returns>
        public IEnumerable<User> Lists(string strWhere)
        {
            IEnumerable<SystemDB.User> r = (IEnumerable<SystemDB.User>)OpUser.select(strWhere);
            Role role = new Role();
            RolesBLL rbll = new RolesBLL();
            StringBuilder roleNames = new StringBuilder();
            //设置用户角色
            foreach (User u in r)
            {
                roleNames = new StringBuilder();
                foreach (UserRoleRef urr in u.UserRoleRefs)
                {
                    if (!u.Roles.Exists(re => re.iRoleId == urr.iRoleId))
                    {
                        role = new Role();
                        role = (Role)rbll.Single(urr.iRoleId);
                        u.Roles.Add(role);
                        roleNames.Append(role.cRoleName + ",");
                    }
                }
                if (roleNames.Length > 1)
                    u.RoleNames = roleNames.ToString(0, roleNames.Length - 1);
            }
            return r;
        }

        /// <summary>
        /// 更新记录,返回更新记录数
        /// </summary>
        /// <param name="_u">用户类实体</param>
        /// <returns></returns>
        public int Update(object _u)
        {
            return OpUser.updata(_u); 
        }
        public static SystemDB.User getUser(int id)
        {
            SystemDB.User u = new SystemDB.User();
            u = OpUser.single(id) as SystemDB.User;
            return u;
        }
        /// <summary>
        /// 返回指定用户名对应的编码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string getCodewithName(string name){
            List<int> codes = new List<int>();
            StringBuilder sb = new StringBuilder();
            var us = OpUser.select("it.iUserName == '" + name + "'");
            foreach (var u in us)
                if (!codes.Exists(i => ((User)u).iUserId == i))
                { codes.Add(((User)u).iUserId); sb.Append(((User)u).iUserId.ToString() + ","); }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }
        /// <summary>
        /// 返回指定id的用户实例
        /// </summary>
        /// <param name="_id">用户id</param>
        /// <returns></returns>
        public Object Single( int _id)
        {
            SystemDB.User r = (SystemDB.User)OpUser.single(_id);
            if (r.Roles == null) r.Roles = new List<Role>();
            //r.Roles = ListRoles(_id);
            r.Roles.Clear();
            foreach (UserRoleRef urr in r.UserRoleRefs)
            {
                r.Roles.Add(urr.Role);
            }
            return r;
        }
        public static bool CurrentUserInRole(string roleName)
        {
            string id = HttpContext.Current.User.Identity.Name;
            User current = (User)OpUser.single(int.Parse(id));
            return current.Roles.Exists(r => r.cRoleName == roleName);
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
            //检查用户记录关联
            if (!preDelete(_id))
                //删除记录
                return OpUser.delete(_id);
            else return -1;
        }
        /// <summary>
        /// 检查是否有关联数据,
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public bool preDelete(int _id)
        {
            bool r = false;
            bool exist = false;
            //用户关联记录是否存在
            exist = SystemBLL.UserExtBLL.Exist(_id);
            r = r || exist;
            //用户关联组
            exist = SystemBLL.GroupsBLL.ExistUser(_id);
            r = r || exist;
            //用户关联角色
            exist = SystemBLL.RolesBLL.ExistUser(_id);
            r = r || exist;
            return r;
        }
    }
}
