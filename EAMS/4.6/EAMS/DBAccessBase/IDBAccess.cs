using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccessBase
{
    public interface IDBAccess
    {
        /// <summary>
        /// 更新数据,返回影响的记录数
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回影响的记录数</returns>
        int updata(object o);

        /// <summary>
        /// 删除数据,返回影响的记录数
        /// </summary>
        /// <param name="List<int> ids">为删除主键id列表</param>
        /// <returns>返回影响的记录数</returns>
        int delete(int id);

        /// <summary>
        /// 新增数据,返回新增数据记录的主键值
        /// </summary>
        /// <param name="o">更新数据Object,数据对象</param>
        /// <returns>返回新增数据记录的主键值</returns>
        string add(object o);

        /// <summary>
        /// 查询数据,返回IEnumerable
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串;</param>
        /// <returns>返回IEnumerable可查询结果</returns>
        IEnumerable<Object> select(string strWhere);

        /// <summary>
        /// 查询单一结果,返回单一object
        /// </summary>
        /// <param name="id">查询主键id</param>
        /// <returns>返回单一object</returns>
        Object single(int id);

        /// <summary>
        /// 验证对象是否存在
        /// </summary>
        /// <param name="kv">实例对象</param>
        /// <returns></returns>
        bool Exist(object _o);
    }
}
