using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemBLL
{
    interface ISystemBLL
    {        
        /// <summary>
        /// 更新信息
        /// </summary>
        /// <returns></returns>
        int Update(object _o);
        
        /// <summary>
        /// 删除记录,关键字列表指定的记录,返回删除记录数
        /// </summary>
        /// <returns></returns>
        int Delete(int _id);
        int Delete(List<int> _l);
        
        /// <summary>
        /// 记录列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Object> Lists(string StrWhere);
        
        /// <summary>
        /// 返回单个记录
        /// </summary>
        /// <returns></returns>
        object Single( int _id);

    }
}
