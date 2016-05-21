using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locks
{
    public class LockBLL
    {
        private LockModel lockItem = null;
        private LockDataAccess lockDal = new LockDataAccess();


        /// <summary>
        /// 加锁,返回锁ID
        /// </summary>
        /// <param name="module">模块名</param>
        /// <param name="key">关键字</param>
        /// <param name="id">相关ID</param>
        /// <param name="locker">加锁人</param>
        /// <returns></returns>
        public long Locking(string module, string key, string id, string locker)
        {
            lockItem = new LockModel()
            {
                module = module,
                key = key,
                id = id,
                locker = locker
            };
            return Locking(lockItem);
        }
        public long Locking(LockModel lockItem)
        {
            long lockId;
            lockId = lockDal.createLock(lockItem);
            return lockId;
        }
        /// <summary>
        /// 解锁,返回解锁数量
        /// </summary>
        /// <param name="searchKeys">查寻锁条件</param>
        /// <returns>解锁数量</returns>
        public int UnLocking(LockModel searchKeys)
        {
            int count = 0;
            IEnumerable<LockModel> delItems = find(searchKeys);
            if (delItems != null && delItems.Count() > 0)
                foreach (LockModel Item in delItems)
                {
                    count += lockDal.deleteLock(Item.autoId);
                }
            return count;
        }
        public int UnLocking(long lockid)
        {
            return lockDal.deleteLock(lockid);
        }
        /// <summary>
        /// 是否存在锁
        /// </summary>
        /// <param name="searchKeys">查寻锁条件</param>
        /// <returns></returns>
        public bool isExistLock(LockModel searchKeys)
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
        public IEnumerable<LockModel> find(LockModel searchKeys)
        {
            return lockDal.select(searchKeys);
        }
    }
}
