
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.Objects;
    using DBAccessBase;

namespace SystemDB
{
    public class dbUserGroupRefs : sysDB
    {
        private static string BaseQuery
            = @"SELECT [autoid],[groupId],[UserId],[isManager] FROM [UserGroupRef] where 1 = 1 ";

        public dbUserGroupRefs()
        {}
        ~dbUserGroupRefs()
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
            ObjectResult<UserGroupRef> Qr = appSystemEntity.ExecuteStoreQuery<UserGroupRef>(Query);
            try { r = Qr.Count() > 0 ? true : false; }
            catch { r = false; }
            return r;
        }
        /// <summary>
        /// 保存数据,
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回新增数据记录的主键值,失败返回string.Empty</returns>
        public void saveUser(int UserId, List<int> GroupIds)
        {
            UserGroupRef u;
            if (Exist("iUserId", UserId))
            {
                deleteGroups(UserId);
            }
            foreach (int gId in GroupIds)
            {
                u = new UserGroupRef() {  UserId = UserId, groupId = gId };
                appSystemEntity.UserGroupRefs.AddObject(u);
            }
            appSystemEntity.SaveChanges();
        }
        public void saveGroup(int GroupId, List<int> UserIds,int AdminId)
        {
            UserGroupRef u;
            if (Exist("groupId", GroupId))
            {
                deleteUsers(GroupId);
            }
            foreach (int UId in UserIds)
            {
                u = new UserGroupRef() { groupId = GroupId, UserId = UId, isManager = (UId == AdminId) };
                appSystemEntity.UserGroupRefs.AddObject(u);
            }
            appSystemEntity.SaveChanges();
        }


        /// <summary>
        /// 查询单一结果,返回单一object
        /// </summary>
        /// <param name="id">查询主键id</param>
        /// <returns>返回单一object</returns>
        public UserGroupRef single(int id)
        {
            UserGroupRef r = new UserGroupRef() { autoid = id };
            var ra = appSystemEntity.UserGroupRefs.Single(s => s.autoid == id);
            r = ra;
            return r;
        }

        /// <summary>
        /// 按参数查询数据,返回IEnumerable
        /// </summary>
        /// <param name="int id">编码</param>
        /// <returns>返回IEnumerable可查询结果</returns>
        public IEnumerable<UserGroupRef> selectUsers(int GroupId)
        {
            var selected = from q in appSystemEntity.UserGroupRefs
                           where q.groupId == GroupId
                           select q;
            if (selected == null) return null;
            else
                return selected.AsEnumerable<UserGroupRef>();
        }
        /// <summary>
        /// 返回用户所在的组列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IEnumerable<UserGroupRef> selectGroups(int UserId)
        {
            var selected = from q in appSystemEntity.UserGroupRefs
                           where q.UserId == UserId
                           select q;
            if (selected == null ) return null;
            else
                return selected.AsEnumerable<UserGroupRef>();
        }

        /// <summary>
        /// 删除数据,返回影响的记录数
        /// </summary>
        /// <param name="int id">为删除主键id列表</param>
        /// <returns>返回影响的记录数</returns>
        public int deleteUsers(int GroupId)
        {
            int r = 0;
            if (appSystemEntity.UserGroupRefs.Any(ru => ru.groupId == GroupId))
            {
                var d = appSystemEntity.UserGroupRefs.Where(s => s.groupId == GroupId);
                foreach (var i in d)
                    appSystemEntity.UserGroupRefs.DeleteObject(i);
                r = appSystemEntity.SaveChanges();
            }
            return r;
        }
        public int deleteGroups(int UserId)
        {
            int r = 0;
            var d = appSystemEntity.UserGroupRefs.Where(s => s.UserId == UserId);
            foreach (var i in d)
                appSystemEntity.DeleteObject(i);
            r = appSystemEntity.SaveChanges();
            return r;
        }
    }

}


