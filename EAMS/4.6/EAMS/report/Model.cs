using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace report
{
    /// <summary>
    /// 报表，不可编辑，可过滤
    /// </summary>
    public class Report
    {
        public Report()
        {
            this.reportID = -1;
        }
        /// <summary>
        /// 序号
        /// </summary>
        public long reportID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 显示标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 查询命令
        /// </summary>
        public string QueryBase { get; set; }
        public string OtherQueryCmd { get; set; }
        public string OrderCmd { get; set; }
        public string GroupCmd { get; set; }
        public string QueryCmd { get { return QueryBase + getFilterCmd() + OtherQueryCmd + GroupCmd + OrderCmd; } }

        /// <summary>
        /// 字段
        /// </summary>
        public List<Field> Fields { get; set; }
        public List<OrderField> orderFields { get; set; }
        /// <summary>
        /// 过滤条件字段
        /// </summary>
        public List<FilterField> filters { get; set; }
        public string getFilterCmd()
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }
        private static DataTable _dataTable = new DataTable();
        public DataTable DataSource { get { return _dataTable; } private set { _dataTable = value; } }

        public long clsID { get; set; }
        public long moduleID { get; set; }
        public Class Class { get { return classGetter(); } }
        public Module Module { get { return moduleGetter(); } }
        private Class classGetter()
        {
            Class r = null;
            reportClassDataAccess reportClassDA = new reportClassDataAccess();
            if (clsID > 0)
                r = reportClassDA.Single(clsID);
            return r;
        }
        private Module moduleGetter()
        {
            Module r = null;
            reportModuleDataAccess reportModuleDA = new reportModuleDataAccess(); if (clsID > 0)
                r = reportModuleDA.Single(moduleID);
            return r;
        }
        /* 部分代码 参照
        /// <summary>
        /// 报表数据源
        /// </summary>
        public DataTable dtReport {
            get { return _dtReport; }
            set { _dtReport = value;
                page.countPage = (int)Math.Ceiling(Convert.ToDecimal(_dtReport.Rows.Count) / Convert.ToDecimal(page.numPage));
                page.currIndex = 0;
                pageStrip = new PageStrip(page.countPage);
            }
        }
        private DataTable _dtReport;
        private DataTable getPageDT(int pageIdx) {
            DataTable r = _dtReport.Clone();
            int b = -1,e = -1;
            b = pageIdx * page.numPage;
            e = b + page.numPage;
            for (int i = b; i < e; i++)
                r.Rows.Add(_dtReport.Rows[i]);
            return r;
        }
        /// <summary>
        /// 表体,由dtReport数据生成
        /// </summary>
        public string getBody(int pageIdx)
        {
            DataTable dtPage = new DataTable();
            if (pageIdx < 0)
                dtPage = dtReport;
            else
                dtPage = getPageDT(pageIdx);//获得指定页面的数据表

            StringBuilder sbReturn = new StringBuilder();
            StringBuilder sbRow = new StringBuilder();
            bool hasRow = true;
            if (dtReport != null && dtPage.Rows.Count > 0)
                for (int ridx = 0; ridx < dtPage.Rows.Count; ridx++)
                {
                    hasRow = true;
                    sbRow.Append("<tr>");
                    var rCells = dtPage.Rows[ridx];
                    for (int cidx = 0; cidx < rCells.ItemArray.Count(); cidx++)
                    {
                        if (rCells[cidx].Equals("合计") || rCells[cidx].Equals("小计"))
                        {
                            if (groupFields.Exists(e => e.groupIndex == cidx))
                                sbRow.Append("<td>" + rCells[cidx].ToString() + "</td>");
                            else { hasRow = false; break; }
                        }
                        else
                            sbRow.Append("<td>" + rCells[cidx].ToString() + "</td>");

                    }
                    sbRow.Append("</tr>");
                    if (hasRow) sbReturn.Append(sbRow);
                }
            return sbReturn.ToString();
        }
        /// <summary>
        /// 表脚，显示制表人，日期，页数
        /// </summary>
        public string footer { get; set; }
        /// <summary>
        /// CSS
        /// </summary>
        public string css { get; set; }
        /// <summary>
        /// 过滤字段列表
        /// </summary>
        public List<FilterField> filterFields { get; set; }
        /// <summary>
        /// 分组字段列表
        /// </summary>
        public List<GroupField> groupFields { get; set; }
        /// <summary>
        /// 排序字段列表
        /// </summary>
        public List<OrderField> orderFields { get; set; }
        /// <summary>
        /// 生成查询命令
        /// </summary>
        /// <returns></returns>
        private string generateQueryCommand() {
            StringBuilder sb = new StringBuilder();
            sb.Append(QueryCommand);
            #region Filter
            FilterField ff = new FilterField();
            if (filterFields != null && filterFields.Count > 0)
            {
                for (int f = 0; f < filterFields.Count; f++)
                {
                    ff = filterFields[f];
                    sb.Append(" and ");
                    sb.Append(ff.getFilterString());
                }
            }
            #endregion

            #region Group
            GroupField gf = new GroupField();
            if (groupFields != null && groupFields.Count > 0)
            {
                sb.Append("Group by ");
                for (int g = 0; g < groupFields.Count; g++)
                {
                    gf = groupFields[g];
                    sb.Append(gf.getGroupString()+",");
                }
            }
            #endregion

            #region Order
            OrderField of = new OrderField();
            if (orderFields != null && orderFields.Count > 0)
            {
                sb.Append("Order by ");
                for (int o = 0; o < orderFields.Count; o++)
                {
                    of = orderFields[o];
                    sb.Append(of.getOrderString() + ",");
                }
            }
            #endregion
            return sb.ToString();
        }
        public Page page { get; set; }
        public PageStrip pageStrip { get; private set; }
        */
    }
    /// <summary>
    /// 报表用户权限
    /// </summary>
    public class Permission
    {
        public long AutoID { get; set; }
        public long reportID { get; set; }
        public long UserID { get; set; }
        public string userName { get; set; }
        public string reportName { get; set; }
        public int index { get; set; }
    }
    public class Class
    {
        public long ID { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
    }
    public class Module
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class v_ReportPermission : Report
    {
        public v_ReportPermission()
        {

        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }
    }

    /// <summary>
    /// 字段类
    /// </summary>
    public class Field
    {
        /// <summary>
        /// 报表序号
        /// </summary>
        public long reportID { get; set; }
        /// <summary>
        /// 字段序号
        /// </summary>
        public long fieldID { get; set; }
        /// <summary>
        /// cell引索
        /// </summary>
        public int fieldIndex { get; set; }
        /// <summary>
        /// cell标题
        /// </summary>
        public string fieldTitle { get; set; } = string.Empty;
        /// <summary>
        /// 对应数据源结果字段名称
        /// </summary>
        public string fieldName { get; set; }
        /// <summary>
        /// 对应数据源参与计算的字段名称
        /// </summary>
        public string fieldNameCompute { get; set; }
        /// <summary>
        /// 字段显示类型名称
        /// </summary>
        public bool isErpEmployeeVisit { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool isDisplay { get; set; }
    }
    /// <summary>
    /// 操作符
    /// </summary>
    public static class Operator
    {
        public static string[] Comparison = { "=", ">", "<", ">=", "<=", "like", "in" };
        public static string[] Logic = { "and", "or", "not" };
    }
    /// <summary>
    /// 过滤条件字段
    /// </summary>
    public class FilterField
    {
        /// <summary>
        /// 字段
        /// </summary>    
        public Field field { get { return getField(); } }
        private Field getField()
        {
            reportFieldDataAccess rfda = new reportFieldDataAccess();
            return rfda.Single(fieldID);
        }
        /// <summary>
        ///  过滤序号
        /// </summary>
        public int filterIndex { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 报表ID
        /// </summary>
        public long reportID { get; set; }
        /// <summary>
        /// 字段ID
        /// </summary>
        public long fieldID { get; set; }
        /// <summary>
        /// 过滤字段名
        /// </summary>
        public string fieldName { get; set; }
        /// <summary>
        /// 过滤字段ID
        /// </summary>
        public long filterID { get; set; }
        /// <summary>
        /// 比较操作符
        /// </summary>
        public string comparisonOperator { get; set; }
        /// <summary>
        /// 逻辑操作符
        /// </summary>
        public string logicOperator { get; set; }
        /// <summary>
        /// 是否默认值
        /// </summary>
        public bool isDefault { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 生成过滤字符串
        /// </summary>
        /// <returns></returns>
        public string getFilterString()
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(fieldName) && string.IsNullOrEmpty(Value)) return sb.ToString();
            if (string.IsNullOrEmpty(logicOperator)) sb.Append(" and");
            else sb.Append(" " + logicOperator);

            sb.Append(" " + fieldName);
            sb.Append(" " + comparisonOperator);
            switch (comparisonOperator)
            {//"like":'%likestring%';无比较符：直接拼接Value;其它：'Value'
                case "":
                case " ":
                case null:
                    sb.Append(" " + Value);
                    break;
                case "like":
                    sb.Append(" '%" + Value + "%'");
                    break;
                default:
                    sb.Append(" '" + Value + "'");
                    break;

            }
            return sb.ToString();
        }
    }
    /// <summary>
    /// 排序字段
    /// </summary>
    public class OrderField
    {
        public string fieldName { get; set; }
        public Field field { get { return getField(); } }
        private Field getField()
        {
            reportFieldDataAccess rfda = new reportFieldDataAccess();
            return rfda.Single(fieldID);
        }
        /// <summary>
        /// 排序序号
        /// </summary>
        public long orderID { get; set; }
        /// <summary>
        /// order by 顺序号
        /// </summary>
        public int orderIndex { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 报表序号
        /// </summary>
        public long reportID { get; set; }
        /// <summary>
        /// 字段序号
        /// </summary>
        public long fieldID { get; set; }
        /// <summary>
        /// 升序(True)降序(False)
        /// </summary>
        public bool isAsc { get; set; }
        /// <summary>
        /// 是否默认值
        /// </summary>
        public bool isDefault { get; set; }
        /// <summary>
        /// 排序命令字符串
        /// </summary>
        public string orderString { get; set; }
        public string getOrderString()
        {
            string sb = string.Empty;
            if (field != null && !string.IsNullOrEmpty(field.fieldName))
                sb = " " + field.fieldName + " " + (isAsc ? "Asc" : "Desc");
            else
                sb = " " + this.orderString;
            return sb.ToString();
        }
    }

    public class PivotView{
        public long autoid { get; set; }
        public long reportID { get; set; }
        public long userID { get; set; }
        public string pivotParams { get; set; }
        public string pivotName { get; set; }
        public bool isLast { get; set; }
        public bool isDefault { get; set; }
    }
}
