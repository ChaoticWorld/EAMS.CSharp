using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface IDbAccess<T> : IAccess<T>
    {
        List<string> Fields { get; }
        List<T> getList(string whereStr);
        T getSingle(string code);
        List<T> getList(T searchKey);
        void setField(string field, string val, T searchKey);
        void setField(string field, string val, string whereStr);
        object getField(string field, T searchKey);
        string LinkExpression(string lnkKey, string lnkExpression);
        string LinkField(string lnkKey, string[] lnkFields);
        string Comparison(string compstring, string compKey, string compValue);
        //IList<T> pager();分页   
    }
    public interface IXmlAccess<T> { }
    public interface IJsonAccess<T> { }

    public interface IAccess<T> { }
    public interface IMultiTableQuery{
        /// <summary>
        /// 查指定日期内，指定地区，指定客户编码的发货单号
        /// </summary>
        /// <param name="begin">开始日期</param>
        /// <param name="end">结果日期</param>
        /// <param name="cDCName">地区名称</param>
        /// <param name="cDWCode">客户编码</param>
        /// <returns>List<string> 发货单号</returns>
        List<string> getDispatchMainCodesWithScope(DateTime begin, DateTime end, string cDCName, string cDWCode);
        /// <summary>
        /// 查指定日期内，指定地区，指定客户编码的订单单号
        /// </summary>
        /// <param name="begin">开始日期</param>
        /// <param name="end">结果日期</param>
        /// <param name="cDCName">地区名称</param>
        /// <param name="cDWCode">客户编码</param>
        /// <returns>List<string> 订单号</returns>
        List<string> getSODetailMainCodesWithScope(DateTime begin, DateTime end, string cDCName, string cDWCode);
    }
}
