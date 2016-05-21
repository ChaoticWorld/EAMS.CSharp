using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemBLL
{
    public class ActionsBLL:ISystemBLL
    {
        protected static SystemDB.dbActions OpAction = new SystemDB.dbActions();

        /// <summary>
        /// 验证字段值是否被使用,使用范围:注册,
        /// </summary>
        /// <param name="kv">单个键值对KeyValuePair<string,string></param>
        /// <returns></returns>
        public static bool Exist(string _key,string _value)
        {
            bool r = OpAction.select("it." + _key + " == " + _value) != null ? true : false;
            return r;
        }
        public static bool Exist(int _id)
        { return OpAction.Exist(_id); }

        /// <summary>
        /// 记录列表
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns></returns>
        public IEnumerable<Object> Lists(string strWhere)
        {
            return (IEnumerable < SystemDB.Actions > )OpAction.select(strWhere);
        }

        /// <summary>
        /// 更新记录,返回更新记录数
        /// </summary>
        /// <param name="_u">用户类实体</param>
        /// <returns></returns>
        public int Update(object _u)
        {
            return OpAction.updata(_u); 
        }

        /// <summary>
        /// 返回指定id的用户实例
        /// </summary>
        /// <param name="_id">用户id</param>
        /// <returns></returns>
        public Object Single( int _id)
        {
            return (SystemDB.Actions)OpAction.single(_id);
        }

        /// <summary>
        /// 删除指定id的用户实例
        /// </summary>
        /// <param name="_id">用户id列表</param>
        /// <returns></returns>
        public int Delete(List<int> _l)
        {
            int r = 0;
            foreach (int i in _l)
                r += Delete(i);
            return r;
        }
        public int Delete(int _id)
        {
            return OpAction.delete(_id);
        }

        /// <summary>
        /// 增加记录
        /// </summary>
        /// <param name="_o"></param>
        /// <returns></returns>
        public string Add(SystemDB.Actions _o)
        {
            return OpAction.add(_o);
        }

    }
}

