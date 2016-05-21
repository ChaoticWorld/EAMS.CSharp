using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrganizationBase
{
    public class groupBLL
    {
        groupDataAccess gDA = new groupDataAccess();

        public int create(groupModel m)
        { return (int)gDA.Create(m); }
        public int update(groupModel m)
        { return gDA.Update(m); }
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
            else r = gDA.Delete(id);
            return r;
        }
        private bool preDelete(int id)
        {
            bool r = false;
            groupRefDataAccess gRefDA = new groupRefDataAccess();            
            r |= gRefDA.selects(new groupRefModel() { groupId = id }).Count > 0;
            return r;
        }
        public List<groupModel> select(groupModel t)
        { return gDA.selects(t); }
        public groupModel single(int id)
        { return gDA.Single(id); }
        public groupModel single(string key)
        { return gDA.Single(key); }
    }

    public class groupRefBLL
    {
        groupRefDataAccess grDA = new groupRefDataAccess();

        public int create(groupRefModel m)
        { return (int)grDA.Create(m); }
        public int Update(List<groupRefModel> ms)
        {
            if (null != ms && ms.Count > 0)
            {
                grDA.DeleteWithGroupID(ms[0].groupId);
                foreach (groupRefModel gr in ms)
                    grDA.Create(gr);
            }
            return ms.Count; }
        /// <summary>
        /// 返回删除结果;-2:有关联不能删除,0:删除失败,>0:删除n记录
        /// </summary>
        /// <param name="id">关联autoid</param>
        /// <returns></returns>
        public int delete(int id)
        {
            int r = 0;
            r = grDA.Delete(id);
            return r;
        }
        public List<groupRefModel> select(groupRefModel t)
        { return grDA.selects(t); }
        public groupRefModel single(int id)
        { return grDA.Single(id); }
    }
}
