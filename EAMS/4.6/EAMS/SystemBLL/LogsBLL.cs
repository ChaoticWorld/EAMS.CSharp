using System;
using System.Collections.Generic;
using System.Net;

namespace SystemBLL
{
    public class LogsBLL:ISystemBLL
    {
        protected static SystemDB.dbLogs OpLog = new SystemDB.dbLogs();

        /// <summary>
        /// 验证字段值是否被使用,使用范围:注册,
        /// </summary>
        /// <param name="kv">单个键值对KeyValuePair<string,string></param>
        /// <returns></returns>
        public static bool Exist(string _key,string _value)
        {
            bool r = OpLog.select("it." + _key + " == " + _value) != null ? true : false;
            return r;
        }

        /// <summary>
        /// 记录列表
        /// </summary>
        /// <param name="strWhere">为string类型的条件字符串; "it.iUserID >= 0";it为委托变量不要改变</param>
        /// <returns></returns>
        public IEnumerable<Object> Lists(string strWhere)
        {
            if (!string.IsNullOrEmpty(strWhere))
            return (IEnumerable < SystemDB.Logs > )OpLog.select(strWhere);
            else
            return (IEnumerable < SystemDB.Logs > )OpLog.select();
        }

        /// <summary>
        /// 更新记录,返回更新记录数
        /// </summary>
        /// <param name="_u">类实体</param>
        /// <returns></returns>
        public int Update(object _u)
        {
            return OpLog.updata(_u); 
        }

        /// <summary>
        /// 返回指定id的用户实例
        /// </summary>
        /// <param name="_id">用户id</param>
        /// <returns></returns>
        public Object Single( int _id)
        {
            return (SystemDB.Logs)OpLog.single(_id);
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
            return OpLog.delete(_id);
        }

        /// <summary>
        /// 增加记录
        /// </summary>
        /// <param name="_o"></param>
        /// <returns></returns>
        public string Add(SystemDB.Logs _o)
        {
            string hostName = string.Empty;
            string ip4Str = string.Empty;
            string ip6Str = string.Empty;
            string ipStr = string.Empty;
            if (System.Web.HttpContext.Current == null)
            {//获取本地主机名和地址
                hostName = Dns.GetHostName();
                IPHostEntry ipe = Dns.GetHostEntry(hostName);

                foreach (IPAddress ip in ipe.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                     ip4Str = ip.ToString();                    
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                     ip6Str = ip.ToString(); 
                }
                ipStr = string.IsNullOrEmpty(ip4Str) ?
                    (string.IsNullOrEmpty(ip6Str) ? "" : ip6Str)
                    : ip4Str;
            }
            else
            {//获取Request主机名和地址
                hostName = System.Web.HttpContext.Current.Request.UserHostName;
                ipStr = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            _o.cIP = ipStr;
            _o.cWorkStation = hostName;
            _o.dLogDate = DateTime.Now;
            return OpLog.add(_o);
        }

    }
}

