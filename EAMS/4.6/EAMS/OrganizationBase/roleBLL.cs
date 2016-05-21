using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrganizationBase
{
    public class roleBLL
    {
        roleDataAccess rDA = new roleDataAccess();

        public int create(roleModel m)
        { return (int)rDA.Create(m); }
        public int update(roleModel m)
        { return rDA.Update(m); }
        /// <summary>
        /// 返回删除结果;-2:有关联不能删除,0:删除失败,>0:删除n记录
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public int delete(int id)
        {
            int r = 0;
            if (preDelete(id))
                r = -2;
            else r = rDA.Delete(id);
            return r;
        }
        private bool preDelete(int id)
        {
            bool r = false;
            roleRefDataAccess gRefDA = new roleRefDataAccess();            
            r |= gRefDA.selects(new roleRefModel() { iRoleId = id }).Count > 0;
            return r;
        }
        public List<roleModel> select(roleModel t)
        { return rDA.selects(t); }
        public roleModel single(int id)
        { return rDA.Single(id); }
        public roleModel single(string key)
        { return rDA.Single(key); }
        public IList<roleModel> getOrganizations(int uid)
        {
            return rDA.getOrganization(uid);
        }
    }

    public class roleRefBLL
    {
        roleRefDataAccess rrDA = new roleRefDataAccess();

        public int create(roleRefModel m)
        { return (int)rrDA.Create(m); }
        public int Update(List<roleRefModel> ms)
        {
            if (null != ms && ms.Count > 0)
            {
                rrDA.DeleteWithRoleID(ms[0].iRoleId);
                foreach (roleRefModel rr in ms)
                    rrDA.Create(rr);
            }
            return ms.Count; }
        public int Update(roleRefModel rrm)
        {
            int r = -1;
            r = rrDA.Update(rrm);
            return r;
        }
        /// <summary>
        /// 返回删除结果;-2:有关联不能删除,0:删除失败,>0:删除n记录
        /// </summary>
        /// <param name="id">关联autoid</param>
        /// <returns></returns>
        public int delete(long id)
        {
            int r = 0;
            r = rrDA.Delete(id);
            return r;
        }
        public List<roleRefModel> select(roleRefModel t)
        { return rrDA.selects(t); }
        public roleRefModel single(int id)
        { return rrDA.Single(id); }
    }
}
