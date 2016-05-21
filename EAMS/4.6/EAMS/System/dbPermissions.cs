
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.Objects;
    using DBAccessBase;

namespace SystemDB
{
    public partial class Permission
    {
        /// <summary>
        /// 权限词典,Key-Function;Value-Action
        /// </summary>
        public Dictionary<int, int> FuncActionDict { get; set; }
        public enum PType { User, Role };
        public PType IDType
        {
            get { return (PType)Enum.Parse(typeof(PType), cType); }
            set { cType = Enum.GetName(typeof(PType), value); }
        }
    }
    public class dbPermissions : sysDB
    {
        public dbPermissions()
        {}
        ~dbPermissions()
        { }

        /// <summary>
        /// 权限是否存在,参数:Id对象Id,cLevel对象类型
        /// </summary>
        /// <param name="id">对象Id:iUserId,iRoleId</param>
        /// <param name="cType">对象类型:User,Role</param>
        /// <returns></returns>
        public bool Exist(int id,Permission.PType cType)
        {
            bool r = false;
            try
            {
                var once = appSystemEntity.Permissions.First(i => i.iId == id && i.cType == cType.ToString());
                r = true;
            }
            catch { r = false; }
            return r;
        }
        /// <summary>
        /// 保存数据,
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回新增数据记录的主键值,失败返回string.Empty</returns>
        public void save(Permission _p)
        {
            appSystemEntity.Permissions.AddObject(_p);
            appSystemEntity.SaveChanges();
        }


        /// <summary>
        /// 查询指定对象Id的结果集,返回Permission/IEnumerable_Permission_
        /// </summary>
        /// <param name="id">查询对象id</param>
        /// <param name="_t">查询对象类型</param>
        /// <returns>返回单一object</returns>
        public IEnumerable<Permission> single(int id,Permission.PType _t)
        {
            IEnumerable<Permission> r = null ;
            switch (_t)
            {
                case Permission.PType.Role:
                    var rs = appSystemEntity.Permissions
                        .Where(a => a.cType == Permission.PType.Role.ToString())
                        .Where(a1 => a1.iId == id);
                    r = rs;
                    break;
                case Permission.PType.User:
                    var us = appSystemEntity.Permissions
                        .Where(a => a.cType == Permission.PType.User.ToString())
                        .Where(a1 => a1.iId == id);
                    r = us;
                    break;
            }
            return r;
        }
        public Permission single(int id)
        {
            Permission r = new Permission() { iPermissionId = id };
            return r;
        }

        /// <summary>
        /// 按参数查询数据,返回IEnumerable
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns>返回IEnumerable可查询结果</returns>
        public IEnumerable<Object> select(int id,Permission.PType _t)
        {
            IEnumerable<Permission> selected = null;
            switch (_t)
            {
                case Permission.PType.Role:
                    var rs = appSystemEntity.Permissions.Where(a => a.cType == _t.ToString()).Where(a1 => a1.iId == id);
                    selected = rs;
                    break;
                case Permission.PType.User:
                    var us =appSystemEntity.Permissions.Where(a=>a.cType == _t.ToString()).Where(a1=>a1.iId == id);
                    selected = us;
                    break;
            }
            return (IEnumerable<Permission>) selected;
        }
        public IEnumerable<Object> select()
        {
            return appSystemEntity.Permissions;
        }

        /// <summary>
        /// 删除数据,返回影响的记录数
        /// </summary>
        /// <param name="int id">为删除主键id列表</param>
        /// <returns>返回影响的记录数</returns>
        public int delete(int id,Permission.PType _t)
        {
            int r = 0;
            var d = appSystemEntity.Permissions.Where(s => s.iId == id).Where(s1=>s1.cType == _t.ToString());
            appSystemEntity.DeleteObject(d);
            try { r = appSystemEntity.SaveChanges(); }
            catch { r = 0; }
            return r;
        }
    }

}


