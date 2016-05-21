using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemDB;

namespace SystemBLL
{
    public class LockBLL
    {
        Lock lockItem = new Lock();
        SystemDB.AppSystemEntities entity = new AppSystemEntities();

        /// <summary>
        /// 加锁,返回锁ID
        /// </summary>
        /// <param name="module">模块名</param>
        /// <param name="key">关键字</param>
        /// <param name="id">相关ID</param>
        /// <param name="locker">加锁人</param>
        /// <returns></returns>
        public int Locking(string module, string key, string id, string locker)
        {
            lockItem = new Lock()
            {
                module = module,
                key = key,
                id = id,
                locker = locker
            };
            return Locking(lockItem);
        }
        public int Locking(Lock lockItem)
        {
            int lockId;
            //lockId = sysDB.Context.Insert<Lock>("Lock", lockItem)
            //                        .AutoMap(x => x.autoId)
            //                        .ExecuteReturnLastId<int>();
            entity.Locks.AddObject(lockItem);
            entity.SaveChanges();
            lockId = lockItem.autoId;
            return lockId;
        }
        /// <summary>
        /// 解锁,返回解锁数量
        /// </summary>
        /// <param name="searchKeys">查寻锁条件</param>
        /// <returns>解锁数量</returns>
        public int UnLocking(Lock searchKeys)
        {
            int count = 0;
            IEnumerable<Lock> delItems = find(searchKeys);
            if (delItems != null && delItems.Count() >0)
            foreach (Lock Item in delItems)
            {
                //count += sysDB.Context.Delete("Lock").Where("autoid", Item.autoId).Execute();
                entity.Locks.DeleteObject(Item);
            }
            count += entity.SaveChanges();
            return count;
        }
        /// <summary>
        /// 是否存在锁
        /// </summary>
        /// <param name="searchKeys">查寻锁条件</param>
        /// <returns></returns>
        public bool isExistLock(Lock searchKeys)
        {
            bool r = false;
            r = find(searchKeys).Count() > 0;            
            return r;
        }
        /// <summary>
        /// 查找锁
        /// </summary>
        /// <param name="searchKeys">查寻条件</param>
        /// <returns></returns>
        public IEnumerable<Lock> find(Lock searchKeys)
        {
            string sqlcmd = "select * from Lock where 1 = 1 ";
            var locks = entity.Locks.AsQueryable();
            if (searchKeys.autoId > 0)
            {
                sqlcmd += " and autoId = '" + searchKeys.autoId + "' ";
                locks = locks.Where(dw => dw.autoId == searchKeys.autoId);
            } if (!string.IsNullOrEmpty(searchKeys.module))
            {
                sqlcmd += " and [module] = '" + searchKeys.module + "' ";
                locks = locks.Where(dw => dw.module == searchKeys.module);
            }
            if (!string.IsNullOrEmpty(searchKeys.key))
            {
                sqlcmd += " and [key] = '" + searchKeys.key + "' ";
                locks = locks.Where(dw => dw.key == searchKeys.key);
            }
            //if (!string.IsNullOrEmpty(searchKeys.locker))
            //    locks = locks.Where(dw => dw.locker == searchKeys.locker);
            if (searchKeys.id != null)
            {
                sqlcmd += " and id = '" + searchKeys.id + "'";
                locks = locks.Where(dw => dw.id == searchKeys.id);
            }
            //List<Lock> enums = new List<Lock>();
            //enums = sysDB.Context.Sql(sqlcmd).QueryMany<Lock>();

            return locks.AsEnumerable();
            //return enums;
        }
    }
}
