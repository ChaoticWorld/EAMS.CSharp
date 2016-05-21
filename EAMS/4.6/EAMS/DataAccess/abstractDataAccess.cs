using System.Text;
using FluentData;
using System.Collections.Generic;

namespace DataAccess
{
    public abstract class abstractDataAccess
    {
        protected string TableName { get; set; }
        protected string _sqlBase = string.Empty;
        protected string BaseQuery { get { return _sqlBase; } }
        protected void setBaseQuery(string _base)
        { _sqlBase = _base; }
        protected StringBuilder wStr;
        protected IDbContext Context;

        public List<string> Fields { get; protected set; }
        /// <summary>
        /// 生成比较表达式字符串，compKey comstring[=,>,...] compValue
        /// </summary>
        /// <param name="compstring">比较操作符(=,<,>,>=,like,<>)</param>
        /// <param name="compKey">key</param>
        /// <param name="compValue">value</param>
        /// <returns>key=value</returns>
        public string Comparison(string compstring, string compKey, string compValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" [" + compKey + "] " + compstring + " '" + compValue + "'");
            return sb.ToString();
        }
        /// <summary>
        /// 生成条件字符串，lnkKey[and,or...] (比较表达式 expression[key=value])
        /// </summary>
        /// <param name="lnkKey">and,or</param>
        /// <param name="lnkExpression">比较表达式Comparison()[key=value]</param>
        /// <returns>key[and,or] (expression[key=value])</returns>
        public string LinkExpression(string lnkKey, string lnkExpression)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" " + lnkKey + " (" + lnkExpression + ") ");
            return sb.ToString();
        }
        /// <summary>
        /// 生成字段列表字符串, lnkKey[group by, order by] field1,field2...
        /// </summary>
        /// <param name="lnkKey">group by |order by</param>
        /// <param name="lnkFields">Fields[fld1,fld2...fldN]</param>
        /// <returns>key[group by |order by ] Fields[fld1,fld2...fldN]</returns>
        public string LinkField(string lnkKey, string[] lnkFields)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" " + lnkKey + " ");
            for (int i = 0; i < lnkFields.Length; i++)
                sb.Append("[" + lnkFields[i] + "],");
            sb = sb.Remove(sb.Length - 2, 1);
            return sb.ToString();
        }
    }
}
