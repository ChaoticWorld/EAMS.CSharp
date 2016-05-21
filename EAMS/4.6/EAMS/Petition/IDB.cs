using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Petition
{
    interface IDB<T>
    {
        /// <summary>
        /// 返回列表
        /// </summary>
        /// <returns>返回列表</returns>
        IEnumerable<T> list();
        /// <summary>
        /// 新增数据，返回记录Id
        /// </summary>
        /// <param name="t">id为-1的新记录数据</param>
        /// <returns>返回新记录Id</returns>
        int Add(T t);
        /// <summary>
        /// 更新数据，返回更新数据的Id
        /// </summary>
        /// <param name="t">要更新的数据</param>
        /// <returns>返回更新数据的Id</returns>
        int Update(T t);
        /// <summary>
        /// 删除指定Id的数据记录
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
        /// <summary>
        /// 返回指定Id的单记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回指定Id的单记录</returns>
        T Signal(int id);
        /// <summary>
        /// 查寻参数对象中条件的数据
        /// </summary>
        /// <param name="t">包含条件的对象</param>
        /// <returns>返回查寻结果</returns>
        IEnumerable<T> Find(T t);
    }
}
