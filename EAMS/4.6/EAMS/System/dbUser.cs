using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using DBAccessBase;
using System.ComponentModel.DataAnnotations;


namespace SystemDB
{
    public partial class User
    {
        public User() { this.cUserCode = "[None]"; Roles = new List<Role>(); }
        public List<Role> Roles { get; set; }
        /// <summary>
        /// 角色名,多角色以逗号[,]分隔
        /// 认证角色使用,待测试
        /// </summary>
        public string RoleNames { get; set; }
    }
    public class dbUser : sysDB, IDBAccess
    {
        private static string BaseQuery
            = @"select [iUserId],[cUserCode],[cUserName],[cUserPassWord],[cUserEMail],[cUserMobile] from [User] where 1 = 1 ";

        public dbUser()
            : base()
        { }
        ~dbUser()
        { }

        public  int Authentication(User _u)
        {
            int r = -1;
            User ru = new User();
            try
            {
                if (!string.IsNullOrEmpty(_u.cUserCode) && _u.cUserCode != "[None]") //用户编码登录
                    ru = appSystemEntity.User.Single(r1 => (r1.cUserPassWord == _u.cUserPassWord
                             && r1.cUserCode == _u.cUserCode));
                else if (!string.IsNullOrEmpty(_u.cUserEMail) && WebCommon.RegExp.IsEmail(_u.cUserEMail)) //EMail登录
                    ru = appSystemEntity.User.Single(r1 => (r1.cUserPassWord == _u.cUserPassWord
                             && r1.cUserEMail == _u.cUserEMail));
                else if (!string.IsNullOrEmpty(_u.cUserMobile) && WebCommon.RegExp.IsMobile(_u.cUserMobile))//手机号登录
                    ru = appSystemEntity.User.Single(r1 => (r1.cUserPassWord == _u.cUserPassWord
                             && r1.cUserMobile == _u.cUserMobile));
                if (ru.iUserId > 0) r = ru.iUserId;
            }
            catch
            { r = -1;  }
            return r;
        }
        /// <summary>
        /// 新增数据,返回新增数据记录的主键值,失败返回string.Empty
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回新增数据记录的主键值,失败返回string.Empty</returns>
        public string add(object _u)
        {
            User u = (User)_u;
            appSystemEntity.User.AddObject(u);
            try
            {
                appSystemEntity.SaveChanges();
                MasterKey = u.iUserId.ToString();
            }
            catch(Exception e) { 
                MasterKey = string.Empty;
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
            User r = new User();
            StringBuilder rolenames = new StringBuilder();
            //if (appSystemEntity.Connection.State != System.Data.ConnectionState.Closed)
            //    appSystemEntity.Connection.Close();
            if (appSystemEntity.Connection.State != System.Data.ConnectionState.Open)
            { appSystemEntity.Connection.Close(); appSystemEntity.Connection.Open(); }
            r = appSystemEntity.User.Single(s => s.iUserId == id);
            var u2r = r.UserRoleRefs.Where(i => i.iUserId == id);
            //var t = u2r.ToList();
            foreach (UserRoleRef j in u2r)
            {
                if (!r.Roles.Exists(re => re.iRoleId == j.iRoleId))
                {
                    r.Roles.Add(j.Role);
                    rolenames.Append(j.Role.cRoleName + ",");
                }
            }
            if (rolenames.Length > 0)
            r.RoleNames = rolenames.ToString().Remove(rolenames.Length - 1);
            return r;
        }

        /// <summary>
        /// 查询数据,返回IEnumerable
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns>返回IEnumerable可查询结果</returns>
        public IEnumerable<Object> select(string strWhere)
        {
            //以Context.ObjectQuery.Where(paramStr)方法实现
            //(参数)的写法:string paramStr = "it.iUserID >= 0";
            ObjectQuery<User> selected;
            if (!string.IsNullOrEmpty(strWhere))
                selected = appSystemEntity.User.Where(strWhere);
            else
                selected = appSystemEntity.User.Where("1=1");
            return selected;
        }

        /// <summary>
        /// 更新数据,返回影响的记录数
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回影响的记录数</returns>
        public int updata(object u)
        {
            User _u = (User)u;
            int r = -1;
            var upd = appSystemEntity.User.Single(s => s.iUserId == _u.iUserId);
            //upd = _u;
            upd.cUserCode = string.IsNullOrEmpty(_u.cUserCode) ? upd.cUserCode : _u.cUserCode;
            upd.cUserEMail = string.IsNullOrEmpty(_u.cUserEMail) ? upd.cUserEMail : _u.cUserEMail;
            upd.cUserMobile = string.IsNullOrEmpty(_u.cUserMobile) ? upd.cUserMobile : _u.cUserMobile;
            upd.cUserName = string.IsNullOrEmpty(_u.cUserName) ? upd.cUserName : _u.cUserName;
            upd.cUserPassWord = string.IsNullOrEmpty(_u.cUserPassWord) ? upd.cUserPassWord : _u.cUserPassWord;

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

            var d = appSystemEntity.User.Single(s => s.iUserId == id);
            appSystemEntity.DeleteObject(d);
            try
            {
                r += appSystemEntity.SaveChanges();
            }
            catch (Exception e) {
                var em = e.Message;
            }
            Records = r;
            return r;
        }

        /// <summary>
        /// 验证指定字段的值是否存在,使用范围:注册,
        /// </summary>
        /// <param name="kv">单个键值对KeyValuePair[string,string]</param>
        /// <returns></returns>
        public bool Exist(KeyValuePair<string, string> _kv)
        {
            bool r = false;
            string QueryString = " and [" + _kv.Key + "] = '" + _kv.Value + "'";
            string Query = BaseQuery + QueryString;
            //以ExecuteStoryQuery方法实现查寻
            ObjectResult<User> Qr = appSystemEntity.ExecuteStoreQuery<User>(Query);
            try { r = Qr.Count() > 0 ? true : false; }
            catch { r = false; }
            return r;
        }
        public bool Exist(object _u)
        {
            User u = (User)_u;
            bool r = Exist(new KeyValuePair<string, string>("iUserId", u.iUserId.ToString()));
            r = r || Exist(new KeyValuePair<string, string>("cUserCode", u.cUserCode.ToString()));
            r = r || Exist(new KeyValuePair<string, string>("cUserEMail", u.cUserEMail.ToString()));
            r = r || Exist(new KeyValuePair<string, string>("cUserMobile", u.cUserMobile.ToString()));
            return r;
        }
    }

}
