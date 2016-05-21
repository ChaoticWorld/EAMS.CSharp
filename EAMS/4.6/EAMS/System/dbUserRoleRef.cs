
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.Objects;
    using DBAccessBase;

namespace SystemDB
{
    public class dbUserRoleRefs : sysDB
    {
        private static string BaseQuery
            = @"SELECT [iRoleId],[iUserId],[iURRefId] FROM [UserRoleRef] where 1 = 1 ";

        public dbUserRoleRefs()
        {}
        ~dbUserRoleRefs()
        { }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="_u">Users.iUserId</param>
        /// <returns></returns>
        public bool Exist(string _key,int _id)
        {
            bool r = false;
            string QueryString = " and [" + _key + "] = '" + _id + "'";
            string Query = BaseQuery + QueryString;
            //以ExecuteStoryQuery方法实现查寻
            ObjectResult<UserRoleRef> Qr = appSystemEntity.ExecuteStoreQuery<UserRoleRef>(Query);
            try { r = Qr.Count() > 0 ? true : false; }
            catch { r = false; }
            return r;
        }
        /// <summary>
        /// 保存数据,
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回新增数据记录的主键值,失败返回string.Empty</returns>
        public void saveUser(int UserId, List<int> RoleIds)
        {
            UserRoleRef u;
            if (Exist("iUserId", UserId))
            {
                deleteRoles(UserId);
            }
            foreach (int RId in RoleIds)
            {
                u = new UserRoleRef() { iUserId = UserId, iRoleId = RId };
                appSystemEntity.UserRoleRefs.AddObject(u);
            }
            appSystemEntity.SaveChanges();
        }
        public void saveRole(int RoleId, List<int> UserIds)
        {
            UserRoleRef u;
            if (Exist("iRoleId", RoleId))
            {
                deleteUsers(RoleId);
            }
            foreach (int UId in UserIds)
            {
                u = new UserRoleRef() { iRoleId = RoleId, iUserId = UId };
                appSystemEntity.UserRoleRefs.AddObject(u);
            }
            appSystemEntity.SaveChanges();

        }


        /// <summary>
        /// 查询单一结果,返回单一object
        /// </summary>
        /// <param name="id">查询主键id</param>
        /// <returns>返回单一object</returns>
        public UserRoleRef single(int id)
        {
            UserRoleRef r = new UserRoleRef() { iURRefId = id };
            var ra = appSystemEntity.UserRoleRefs.Single(s => s.iURRefId == id);
            r = ra;
            return r;
        }

        /// <summary>
        /// 按参数查询数据,返回IEnumerable
        /// </summary>
        /// <param name="int id">编码</param>
        /// <returns>返回IEnumerable可查询结果</returns>
        public IEnumerable<Object> selectUsers(int RoleId)
        {
            var selected = from q in appSystemEntity.UserRoleRefs
                           where q.iRoleId == RoleId
                           select q;
            if (selected == null) return null;
            else
                return selected.AsEnumerable<UserRoleRef>();
        }
        public IEnumerable<Object> selectRoles(int UserId)
        {
            var selected = from q in appSystemEntity.UserRoleRefs
                           where q.iUserId == UserId
                           select q;
            if (selected == null) return null;
            else
                return selected.AsEnumerable<UserRoleRef>();
        }

        /// <summary>
        /// 删除数据,返回影响的记录数
        /// </summary>
        /// <param name="int id">为删除主键id列表</param>
        /// <returns>返回影响的记录数</returns>
        public int deleteUsers(int RoleId)
        {
            int r = 0;
            if (appSystemEntity.UserRoleRefs.Any(ru => ru.iRoleId == RoleId))
            {
                var d = appSystemEntity.UserRoleRefs.Where(s => s.iRoleId == RoleId);
                foreach (var i in d)
                    appSystemEntity.UserRoleRefs.DeleteObject(i);
                r = appSystemEntity.SaveChanges();
            }
            return r;
        }
        public int deleteRoles(int UserId)
        {
            int r = 0;
            var d = appSystemEntity.UserRoleRefs.Where(s => s.iUserId == UserId);
            foreach (var i in d)
                appSystemEntity.DeleteObject(i);
            r = appSystemEntity.SaveChanges();
            return r;
        }
    }

}


