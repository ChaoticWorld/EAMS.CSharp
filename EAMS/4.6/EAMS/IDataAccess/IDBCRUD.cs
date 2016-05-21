using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface IdbCRUD<T>
    {
        //CREATE,RETRIEVE,UPDATE,DELETE:增加,读取,更新,删除
        /// <summary>
        /// 增加记录
        /// </summary>
        /// <param name="t">增加记录实例</param>
        /// <returns>新记录的ID</returns>
        long Create(T t);
        T Single(long id);
        T Single(string key);
        int Update(T t);
        int Delete(long id);
        List<T> selects(T t);
    }

}
