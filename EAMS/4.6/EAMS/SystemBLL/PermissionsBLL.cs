using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemDB;

namespace SystemBLL
{
    public partial class PermissionsBLL
    {
        private static dbPermissions OpPermission = new dbPermissions();
        public PermissionsBLL()
        { }
        ~PermissionsBLL()
        { }

        /// <summary>
        /// 保存权限,参数为对象的权限列表
        /// </summary>
        /// <param name="_pl"></param>
        public void Save(IEnumerable<SystemDB.Permission> _pl)
        {
            Permission p = _pl.First();
            //delete
            Delete(p.iId, p.IDType);
            
            foreach (Permission ps in _pl)
                OpPermission.save(ps);
        }

        /// <summary>
        /// 删除指定对象的权限
        /// </summary>
        /// <param name="id">对象Id</param>
        /// <param name="_t">对象类型:Role,User</param>
        public void Delete(int id, Permission.PType _t)
        {
            OpPermission.delete(id, _t);
        }

        /// <summary>
        /// 获得指定对象IEnumerable_Permission_列表
        /// </summary>
        /// <param name="id">对象Id</param>
        /// <param name="_t">对象类型:Role,User</param>
        /// <returns></returns>
        public IEnumerable<Permission> Lists(int id, Permission.PType _t)
        {
            IEnumerable<Permission> r = null;
            r = (IEnumerable<Permission>)OpPermission.select(id, _t);
            return r;
        }
    }
}
