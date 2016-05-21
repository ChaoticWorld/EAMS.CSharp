using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using IDataAccess;
using FluentData;

namespace report
{
    public class reportDataAccess : DataAccess<Report>
    {
        public reportDataAccess() 
        {
            TableName = "Reports";
            setBaseQuery("select [reportID], [Description], [Name], [Title], [QueryBase], [clsID], [moduleID]  from [" + TableName + "] where 1 = 1");
        }
        private string WhereStr(Report t)
        {
            wStr = new StringBuilder();
            if (t != null)
            {
                if (t.reportID > 0)
                    wStr.Append(" and [reportID] = '" + t.reportID + "'");
                if (!string.IsNullOrEmpty(t.Name))
                    wStr.Append(" and [Name] like '%" + t.Name + "%'");
                if (!string.IsNullOrEmpty(t.Title))
                    wStr.Append(" and [Title] like  '%" + t.Title + "%'");
                if (!string.IsNullOrEmpty(t.Description))
                    wStr.Append(" and [Description] like '%" + t.Description + "%'");
                if (t.clsID > 0)
                    wStr.Append(" and [clsID] = " + t.clsID);
                if (t.moduleID > 0)
                    wStr.Append(" and [moduleID] = " + t.moduleID);
            }
            return wStr.ToString();
        }
        private void Mapper(Report m, IDataReader row)
        {
            m.Name = row.GetString("Name");
            m.Description = row.GetString("Description");
            //m.filter = row.GetString("filter");
            //m.footer = row.GetString("footer");
            m.Title = row.GetString("Title");            
            m.QueryBase = row.GetString("QueryBase");
            m.reportID = (long)row.GetValue("reportID");
            m.clsID = (long)row.GetValue("clsID");
            m.moduleID = (long)row.GetValue("moduleID");
        }
        public override long Create(Report t)
        {
            long r = -1;
            r = Context.Insert(TableName, t)
                //.Column("css", t.css)
                .Column("Description", t.Description)
                //.Column("filter", t.filter)
                //.Column("footer", t.footer)
                .Column("Name", t.Name)
                .Column("Title", t.Title)
                .Column("QueryBase", t.QueryBase)
                .Column("moduleID", t.moduleID)
                .Column("clsID",t.clsID)
                .ExecuteReturnLastId<long>();
            return r;
        }
        public override Report Single(long id)
        {
            Report r = null;
            string cmd = BaseQuery + " and reportID = " + id.ToString();
            r = Context.Sql(cmd).QuerySingle<Report>(Mapper);
            reportFieldDataAccess rfda = new reportFieldDataAccess();
            r.Fields = rfda.selects(new Field() { reportID = r.reportID });
            return r;
        }
        public override Report Single(string key)
        {
            Report r = null;
            string cmd = BaseQuery + "and [Name] = " + key;
            r = Context.Sql(cmd).QuerySingle<Report>(Mapper);
            return r;
        }
        public override int Update(Report t) { int r = -1;
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Update(TableName,t)
                //.Column("css", t.css)
                .Column("Description", t.Description)
                //.Column("filter", t.filter)
                //.Column("footer", t.footer)
                .Column("Name", t.Name)
                .Column("Title", t.Title)
                .Column("QueryBase", t.QueryBase)
                .Column("moduleID", t.moduleID)
                .Column("clsID", t.clsID)
                .Where("reportID",t.reportID)
                .Execute();
            return r; }
        public override int Delete(long id)
        {
            int r = -1;
            r = Context.Delete(TableName).Where("reportID", id).Execute();
            return r;
        }
        public override List<Report> selects(Report t) { List<Report> r = new List<Report>();
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Sql(cmd).QueryMany<Report>(Mapper);
            return r;
        }
    }

    public class reportPivotViewDataAccess : DataAccess<PivotView>
    {
        public reportPivotViewDataAccess()
        {
            TableName = "report_PivotView";
            setBaseQuery("SELECT [autoid],[reportID],[userID],[pivotParams],[pivotName],[isLast] FROM [" + TableName + "] where 1 = 1 ");
        }
        public override long Create(PivotView t)
        {
            long r = -1;
            r = Context.Insert(TableName, t)
                .Column("userID", t.userID)
                .Column("pivotParams", t.pivotParams)
                .Column("reportID", t.reportID)
                .Column("pivotName",t.pivotName)
                .Column("isLast",t.isLast)
                .ExecuteReturnLastId<long>();
            return r;
        }
        public override int Delete(long id)
        {
            int r = -1;
            r = Context.Delete(TableName).Where("autoid", id).Execute();
            return r;
        }
        public override List<PivotView> selects(PivotView t)
        {
            List<PivotView> r = new List<PivotView>();
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Sql(cmd).QueryMany<PivotView>(Mapper);
            return r;
        }
        public override PivotView Single(string key)
        {
            //throw new Exception("不提供！");
            return null;
        }
        public override PivotView Single(long id)
        {
            PivotView r = null;
            string cmd = BaseQuery + "and [autoid] = " + id.ToString();
            r = Context.Sql(cmd).QuerySingle<PivotView>(Mapper);
            return r;
        }
        public override int Update(PivotView t)
        {
            int r = -1;
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Update(TableName, t)
                .Column("userID", t.userID)
                .Column("pivotParams", t.pivotParams)
                .Column("reportID", t.reportID)
                .Column("pivotName", t.pivotName)
                .Column("isLast", t.isLast)
                .Where("autoid", t.autoid)
                .Execute();
            return r;
        }
        private string WhereStr(PivotView t)
        {
            wStr = new StringBuilder();
            if (t != null)
            {
                if (t.autoid > 0)
                    wStr.Append(" and [autoid] = " + t.autoid);
                if (!string.IsNullOrEmpty(t.pivotParams))
                    wStr.Append(" and [pivotParams] like '%" + t.pivotParams + "%'");
                if (!string.IsNullOrEmpty(t.pivotName))
                    wStr.Append(" and [pivotName] like '%" + t.pivotName + "%'");
                if (t.isLast)
                    wStr.Append(" and [isLast] = 1");
                wStr.Append(" and [userID] = " + t.userID);
                wStr.Append(" and [reportID] = " + t.reportID);
            }
            return wStr.ToString();
        }
        private void Mapper(PivotView m, IDataReader row)
        {
            m.autoid = (long)row.GetValue("autoid");
            m.reportID= (long)row.GetValue("reportID");
            m.userID = (long)row.GetValue("userID");
            m.pivotParams = row.GetString("pivotParams");
            m.pivotName = row.GetString("pivotName");
            m.isLast = row.GetBoolean("isLast");
            m.isDefault = m.userID < 0;
        }
        }
    public class reportFieldDataAccess : DataAccess<Field>
    {
        public reportFieldDataAccess()
        {
            TableName = "report_Fields";
            setBaseQuery("select [fieldID],[reportID],[fieldIndex],[fieldTitle],[fieldName],[fieldNameCompute],[isDisplay],[isErpEmployeeVisit] from [" + TableName + "] where 1 = 1 ");
        }
        private string WhereStr(Field t)
        {
            wStr = new StringBuilder();
            if (t != null)
            {
                if (t.fieldID > 0)
                    wStr.Append(" and [fieldID] = '" + t.fieldID.ToString() + "'");
                if (t.reportID > 0)
                    wStr.Append(" and [reportID] = '" + t.reportID.ToString() + "'");
                if (!string.IsNullOrEmpty(t.fieldName))
                    wStr.Append(" and [fieldName] like '%" + t.fieldName + "%'");
                if (!string.IsNullOrEmpty(t.fieldNameCompute))
                    wStr.Append(" and [fieldNameCompute] like '%" + t.fieldNameCompute + "%'");
                if (!string.IsNullOrEmpty(t.fieldTitle))
                    wStr.Append(" and [fieldTitle] like  '%" + t.fieldTitle + "%'");
                if (t.fieldIndex > 0)
                    wStr.Append(" and [fieldIndex] = '" + t.fieldIndex + "'");
            }
            return wStr.ToString();
        }
        private void Mapper(Field m, IDataReader row)
        {
            m.fieldID = (long)row.GetValue("fieldID");
            m.reportID = (long)row.GetValue("reportID");
            m.fieldName = row.GetString("fieldName");
            m.fieldNameCompute = row.GetString("fieldNameCompute");
            m.isErpEmployeeVisit = row.GetBoolean("isErpEmployeeVisit");
            m.fieldTitle = row.GetString("fieldTitle");
            m.fieldIndex = (int)row.GetValue("fieldIndex");
            m.isDisplay = row.GetBoolean("isDisplay");
        }
        public override long Create(Field t)
        {
            long r = -1;
            r = Context.Insert(TableName, t)
                .Column("fieldName", t.fieldName)
                .Column("fieldNameCompute", t.fieldNameCompute)
                .Column("fieldTitle", t.fieldTitle)
                .Column("fieldIndex", t.fieldIndex)
                .Column("reportID", t.reportID)
                .Column("isDisplay",t.isDisplay)
                .Column("isErpEmployeeVisit", t.isErpEmployeeVisit)
                .ExecuteReturnLastId<long>();
            return r;
        }
        public override Field Single(long id)
        {
            Field r = null;
            string cmd = BaseQuery + "and fieldID = " + id.ToString();
            r = Context.Sql(cmd).QuerySingle<Field>(Mapper);
            return r;
        }
        public override Field Single(string key)
        {
            //throw new Exception("不提供！");
            return null;
        }
        public override int Update(Field t)
        {
            int r = -1;
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Update(TableName, t)
                .Column("fieldName", t.fieldName)
                .Column("fieldNameCompute", t.fieldNameCompute)
                .Column("fieldTitle", t.fieldTitle)
                .Column("fieldIndex", t.fieldIndex)
                .Column("reportID", t.reportID)
                .Column("isDisplay", t.isDisplay)
                .Column("isErpEmployeeVisit", t.isErpEmployeeVisit)
                .Where("fieldID", t.fieldID)
                .Execute();
            return r;
        }
        public override int Delete(long id)
        {
            int r = -1;
            r = Context.Delete(TableName).Where("fieldID", id).Execute();
            return r;
        }
        public override List<Field> selects(Field t)
        {
            List<Field> r = new List<Field>();
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Sql(cmd).QueryMany<Field>(Mapper);
            return r;
        }
    }
    public class reportOrderFieldDataAccess : DataAccess<OrderField>
    {
        public reportOrderFieldDataAccess()
        {
            TableName = "report_Order";
            setBaseQuery("SELECT [orderID],[reportID],[userID],[fieldID],[fieldName],[orderIndex],[orderString],[isAsc] FROM [" + TableName + "] where 1 = 1");
        }
        private string WhereStr(OrderField t)
        {
            wStr = new StringBuilder();
            if (t != null)
            {
                if (t.orderID > 0)
                    wStr.Append(" and [orderID] = '" + t.orderID.ToString() + "'");
                if (t.fieldID > 0)
                    wStr.Append(" and [fieldID] = '" + t.fieldID.ToString() + "'");
                if (t.orderIndex > 0)
                    wStr.Append(" and [orderIndex] = '" + t.orderIndex + "'");
                if (!string.IsNullOrEmpty(t.orderString))
                    wStr.Append(" and [orderString] like '%" + t.orderString + "%'");
                if (!string.IsNullOrEmpty(t.fieldName))
                    wStr.Append(" and [fieldName] like '%" + t.fieldName + "%'");
                //if (!string.IsNullOrEmpty(t.isAsc))
                //    wStr.Append(" and [isAsc] =  '" + t.isAsc + "'");
                wStr.Append(" and [reportID] = '" + t.reportID.ToString() + "'");
                wStr.Append(" and [userID] = '" + t.UserID.ToString() + "'");
            }
            return wStr.ToString();
        }
        private void Mapper(OrderField m, IDataReader row)
        {
            m.fieldID = (long)row.GetValue("fieldID");
            m.reportID = (long)row.GetValue("reportID");
            m.UserID = (long)row.GetValue("userID");
            m.orderID = (long)row.GetValue("orderID");
            m.orderIndex = (int)row.GetValue("orderIndex");
            m.orderString = row.GetString("orderString");
            m.isAsc = row.GetBoolean("isAsc");
            m.fieldName = row.GetString("fieldName");
        }
        public override long Create(OrderField t)
        {
            long r = -1;
            r = Context.Insert(TableName, t)
                .Column("reportID", t.reportID)
                .Column("userID", t.UserID)
                .Column("fieldID", t.fieldID)
                .Column("fieldName", t.fieldName)
                .Column("orderIndex", t.orderIndex)
                .Column("orderString",t.orderString)
                .Column("isAsc",t.isAsc)
                .ExecuteReturnLastId<long>();
            return r;
        }
        public override OrderField Single(long id)
        {
            OrderField r = null;
            string cmd = BaseQuery + "and orderID = " + id.ToString();
            r = Context.Sql(cmd).QuerySingle<OrderField>(Mapper);
            return r;
        }
        public override OrderField Single(string key)
        {
            return null;
        }
        public override int Update(OrderField t)
        {
            int r = -1;
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Update(TableName, t)
                .Column("reportID", t.reportID)
                .Column("userID", t.UserID)
                .Column("fieldID", t.fieldID)
                .Column("fieldName", t.fieldName)
                .Column("orderIndex", t.orderIndex)
                .Column("orderString", t.orderString)
                .Column("isAsc", t.isAsc)
                .Where("orderID", t.orderID)
                .Execute();
            return r;
        }
        public override int Delete(long id)
        {
            int r = -1;
            r = Context.Delete(TableName).Where("orderID", id).Execute();
            return r;
        }
        public override List<OrderField> selects(OrderField t)
        {
            List<OrderField> r = new List<OrderField>();
            string cmd = BaseQuery + WhereStr(t)+ " order by [reportID],[orderIndex],[orderID]";//order表查寻固定排序
            r = Context.Sql(cmd).QueryMany<OrderField>(Mapper);
            return r;
        }
    }
    public class reportFilterDataAccess : DataAccess<FilterField>
    {
        public reportFilterDataAccess()
        {
            TableName = "report_Filter";
            setBaseQuery("select [filterID],[reportID],[userID],[filterIndex],[fieldID],[value],[fieldName],[logicOperator],[comparisonOperator] from [" + TableName + "] where 1 = 1 ");
        }
        private string WhereStr(FilterField t)
        {
            wStr = new StringBuilder();
            if (t != null)
            {
                if (t.filterID > 0)
                    wStr.Append(" and [filterID] = '" + t.filterID.ToString() + "'");
                if (t.fieldID > 0)
                    wStr.Append(" and [fieldID] = '" + t.fieldID.ToString() + "'");
                if (t.filterIndex > 0)
                    wStr.Append(" and [filterIndex] = '" + t.filterIndex.ToString() + "'");
                if (t.fieldName != null)
                    wStr.Append(" and [fieldName] like '%" + t.fieldName + "%'");
                if (!string.IsNullOrEmpty(t.Value))
                    wStr.Append(" and [value] like '%" + t.Value + "%'");
                if (!string.IsNullOrEmpty(t.logicOperator))
                    wStr.Append(" and [logicOperator] like '%" + t.logicOperator.ToString() + "%'");
                if (!string.IsNullOrEmpty(t.comparisonOperator))
                    wStr.Append(" and [comparisonOperator] like '%" + t.comparisonOperator.ToString() + "%'");
                wStr.Append(" and [reportID] = '" + t.reportID.ToString() + "'");
                wStr.Append(" and [UserID] = '" + t.UserID.ToString() + "'");
            }
            return wStr.ToString();
        }
        private void Mapper(FilterField m, IDataReader row)
        {
            m.filterID = (long)row.GetValue("filterID");
            m.fieldID = (long)row.GetValue("fieldID");
            m.reportID = (long)row.GetValue("reportID");
            m.UserID = (long)row.GetValue("userID");
            m.filterIndex = (int)row.GetValue("filterIndex");
            m.Value = row.GetString("value");
            m.logicOperator =  row.GetString("logicOperator");
            m.comparisonOperator =  row.GetString("comparisonOperator");
            m.fieldName = row.GetString("fieldName");
            //reportFieldDataAccess fieldDA = new reportFieldDataAccess();
            //m.field = fieldDA.Single((int)m.fieldID);
        }
        public override long Create(FilterField t)
        {/*[filterID],[reportID],[userID],[filterIndex],[fieldID],[value1],[value2],[filterString],[logicOperator],[comparisonOperator]*/
            long r = -1;
            r = Context.Insert(TableName, t)
                .Column("reportID", t.reportID)
                .Column("userID", t.UserID)
                .Column("filterIndex", t.filterIndex)
                .Column("fieldID", t.fieldID)
                .Column("fieldName", t.fieldName)
                .Column("value", t.Value)
                .Column("logicOperator", t.logicOperator)
                .Column("comparisonOperator", t.comparisonOperator)
                .ExecuteReturnLastId<long>();
            return r;
        }
        public override FilterField Single(long id)
        {
            FilterField r = null;
            string cmd = BaseQuery + "and filterID = " + id.ToString();
            r = Context.Sql(cmd).QuerySingle<FilterField>(Mapper);
            return r;
        }
        public override FilterField Single(string key)
        {
            //throw new Exception("不提供！");
            return null;
        }
        public override int Update(FilterField t)
        {
            int r = -1;
            reportFieldDataAccess fieldDA = new reportFieldDataAccess();
            var fieldName = (t.fieldID > 0) ? fieldDA.Single((int)t.fieldID).fieldName : "";
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Update(TableName, t)
                .Column("reportID", t.reportID)
                .Column("userID", t.UserID)
                .Column("filterIndex", t.filterIndex)
                .Column("fieldID", t.fieldID)
                .Column("fieldName", t.fieldName)
                .Column("value", t.Value)
                .Column("logicOperator", t.logicOperator)
                .Column("comparisonOperator", t.comparisonOperator)
                .Where("filterID", t.filterID)
                .Execute();
            return r;
        }
        public override int Delete(long id)
        {
            int r = -1;
            r = Context.Delete(TableName).Where("filterID", id).Execute();
            return r;
        }
        public override List<FilterField> selects(FilterField t)
        {
            List<FilterField> r = new List<FilterField>();
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Sql(cmd).QueryMany<FilterField>(Mapper);
            return r;
        }
    }

    public class reportPermissionDataAccess : DataAccess<Permission>
    {
        public reportPermissionDataAccess()
        {
            TableName = "report_Permissions";
            setBaseQuery("select [AutoID],[reportID],[UserID] from [" + TableName + "] where 1 = 1 ");
        }
        private string WhereStr(Permission t)
        {
            wStr = new StringBuilder();
            if (t != null)
            {
                if (t.AutoID > 0)
                    wStr.Append(" and [AutoID] = '" + t.AutoID.ToString() + "'");
                if (t.reportID > 0)
                    wStr.Append(" and [reportID] = '" + t.reportID.ToString() + "'");
                if (t.UserID > 0)
                    wStr.Append(" and [IserID] = '" + t.UserID.ToString() + "'");
            }
            return wStr.ToString();
        }
        private void Mapper(Permission m, IDataReader row)
        {
            m.AutoID = (long)row.GetValue("AutoID");
            m.reportID = (long)row.GetValue("reportID");
            m.UserID = (long)row.GetValue("UserID");
        }
        public override long Create(Permission t) {
            long r = -1;
            r = Context.Insert(TableName, t)
                .Column("UserID", t.UserID)
                .Column("reportID", t.reportID)
                .ExecuteReturnLastId<long>();
            return r;
        }
        public override Permission Single(long id)
        {
            Permission r = null;
            string cmd = BaseQuery + "and AutoID = " + id.ToString();
            r = Context.Sql(cmd).QuerySingle<Permission>(Mapper);
            return r;
        }
        public override Permission Single(string key)
        {
            //throw new Exception("不提供！");
            return null;
        }
        public override int Update(Permission t)
        {
            int r = -1;
            r = Context.Update(TableName, t)
                .Column("UserID", t.UserID)
                .Column("reportID", t.reportID)
                .Where("AutoID", t.AutoID)
                .Execute();
            return r;
        }
        public override int Delete(long id)
        {
            int r = -1;
            r = Context.Delete(TableName).Where("AutoID", id).Execute();
            return r;
        }
        public override List<Permission> selects(Permission t)
        {
            List<Permission> r = new List<Permission>();
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Sql(cmd).QueryMany<Permission>(Mapper);
            return r;
        }
    }

    public class v_ReportPermissionDataAccess : DataAccess<v_ReportPermission>
    {
        public v_ReportPermissionDataAccess()
        {
            TableName = "v_reportPermissions";
            setBaseQuery("select [reportID], [Description],[Name], [Title],[QueryBase], [UserID],[clsID],[moduleID] from [" + TableName + "] where 1 = 1");
        }
        private string WhereStr(v_ReportPermission t)
        {
            wStr = new StringBuilder();
            if (t != null)
            {
                if (t.reportID > 0)
                    wStr.Append(" and [reportID] = '" + t.reportID + "'");
                if (!string.IsNullOrEmpty(t.Name))
                    wStr.Append(" and [Name] like '%" + t.Name + "%'");
                if (!string.IsNullOrEmpty(t.Title))
                    wStr.Append(" and [Title] like  '%" + t.Title + "%'");
                if (!string.IsNullOrEmpty(t.Description))
                    wStr.Append(" and [Description] like '%" + t.Description + "%'");
                if (t.UserID > 0)
                    wStr.Append(" and [UserID] = '" + t.UserID + "'");
                if (t.clsID > 0)
                    wStr.Append(" and [clsID] = '"+t.clsID + "'");
                if (t.moduleID > 0)
                    wStr.Append(" and [moduleID] = '" + t.moduleID + "'");
            }
            return wStr.ToString();
        }
        private void Mapper(v_ReportPermission m, IDataReader row)
        {
            m.reportID = (long)row.GetValue("reportID");
            m.Name = row.GetString("Name");
            m.Description = row.GetString("Description");
            m.Title = row.GetString("Title");
            m.QueryBase = row.GetString("QueryBase");
            m.UserID = (long)row.GetValue("UserID");
            m.clsID = (long)row.GetValue("clsID");
            m.moduleID = (long)row.GetValue("moduleID");
        }
        public override long Create(v_ReportPermission t)
        {
            long r = -1;
            r = Context.Insert(TableName, t)
                .Column("UserID", t.UserID)
                .Column("Description", t.Description)
                .Column("Name", t.Name)
                .Column("Title", t.Title)
                .Column("QueryBase", t.QueryBase)
                .Column("clsID", t.clsID)
                .Column("moduleID", t.moduleID)
                .ExecuteReturnLastId<long>();
            return r;
        }
        public override v_ReportPermission Single(long id)
        {
            v_ReportPermission r = null;
            string cmd = BaseQuery + "and reportID = " + id.ToString();
            r = Context.Sql(cmd).QuerySingle<v_ReportPermission>(Mapper);
            return r;
        }
        public override v_ReportPermission Single(string key)
        {
            v_ReportPermission r = null;
            string cmd = BaseQuery + "and [Name] = " + key;
            r = Context.Sql(cmd).QuerySingle<v_ReportPermission>(Mapper);
            return r;
        }
        public override int Update(v_ReportPermission t)
        {
            int r = -1;
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Update(TableName, t)
                .Column("UserID", t.UserID)
                .Column("Description", t.Description)
                .Column("Name", t.Name)
                .Column("Title", t.Title)
                .Column("QueryBase", t.QueryBase)
                .Column("clsID", t.clsID)
                .Column("moduleID", t.moduleID)
                .Where("reportID", t.reportID)
                .Execute();
            return r;
        }
        public override int Delete(long id)
        {
            int r = -1;
            r = Context.Delete(TableName).Where("reportID", id).Execute();
            return r;
        }
        public override List<v_ReportPermission> selects(v_ReportPermission t)
        {
            List<v_ReportPermission> r = new List<v_ReportPermission>();
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Sql(cmd).QueryMany<v_ReportPermission>(Mapper);
            return r;
        }
    }

    public class reportClassDataAccess : DataAccess<Class>
    {
        public reportClassDataAccess()
        {
            TableName = "report_Class";
            setBaseQuery("select [clsID],[clsName],[clsDescription] from [" + TableName + "] where 1 = 1 ");
        }
        private string WhereStr(Class t)
        {
            wStr = new StringBuilder();
            if (t != null)
            {
                if (t.ID > 0)
                    wStr.Append(" and [clsID] = '" + t.ID.ToString() + "'");
                if (!string.IsNullOrEmpty(t.ClassName))
                    wStr.Append(" and [clsName] like '%" + t.ClassName.ToString() + "%'");
                if (!string.IsNullOrEmpty(t.Description))
                    wStr.Append(" and [Description] like '%" + t.Description.ToString() + "%'");
            }
            return wStr.ToString();
        }
        private void Mapper(Class m, IDataReader row)
        {
            m.ID = (long)row.GetValue("clsID");
            m.ClassName = row.GetString("clsName");
            m.Description = row.GetString("clsDescription");
        }
        public override long Create(Class t)
        {
            long r = -1;
            r = Context.Insert(TableName, t)
                .Column("clsNameID", t.ClassName)
                .Column("Description", t.Description)
                .ExecuteReturnLastId<long>();
            return r;
        }
        public override Class Single(long id)
        {
            Class r = null;
            string cmd = BaseQuery + "and clsID = " + id.ToString();
            r = Context.Sql(cmd).QuerySingle<Class>(Mapper);
            return r;
        }
        public override Class Single(string key)
        {
            Class r = null;
            string cmd = BaseQuery + "and clsName = '" + key + "'";
            r = Context.Sql(cmd).QuerySingle<Class>(Mapper);
            return r;
        }
        public override int Update(Class t)
        {
            int r = -1;
            r = Context.Update(TableName, t)
                .Column("clsName", t.ClassName)
                .Column("clsDescription", t.Description)
                .Where("clsID", t.ID)
                .Execute();
            return r;
        }
        public override int Delete(long id)
        {
            int r = -1;
            r = Context.Delete(TableName).Where("clsID", id).Execute();
            return r;
        }
        public override List<Class> selects(Class t)
        {
            List<Class> r = new List<Class>();
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Sql(cmd).QueryMany<Class>(Mapper);
            return r;
        }
    }
    public class reportModuleDataAccess : DataAccess<Module>
    {
        public reportModuleDataAccess()
        {
            TableName = "report_Module";
            setBaseQuery("select [moduleID],[moduleName],[moduleDescription] from [" + TableName + "] where 1 = 1 ");
        }
        private string WhereStr(Module t)
        {
            wStr = new StringBuilder();
            if (t != null)
            {
                if (t.ID > 0)
                    wStr.Append(" and [moduleID] = '" + t.ID.ToString() + "'");
                if (!string.IsNullOrEmpty(t.Name))
                    wStr.Append(" and [moduleName] like '%" + t.Name.ToString() + "%'");
                if (!string.IsNullOrEmpty(t.Description))
                    wStr.Append(" and [Description] like '%" + t.Description.ToString() + "%'");
            }
            return wStr.ToString();
        }
        private void Mapper(Module m, IDataReader row)
        {
            m.ID = (long)row.GetValue("moduleID");
            m.Name = row.GetString("moduleName");
            m.Description = row.GetString("moduleDescription");
        }
        public override long Create(Module t)
        {
            long r = -1;
            r = Context.Insert(TableName, t)
                .Column("moduleNameID", t.Name)
                .Column("moduleDescription", t.Description)
                .ExecuteReturnLastId<long>();
            return r;
        }
        public override Module Single(long id)
        {
            Module r = null;
            string cmd = BaseQuery + "and moduleID = " + id.ToString();
            r = Context.Sql(cmd).QuerySingle<Module>(Mapper);
            return r;
        }
        public override Module Single(string key)
        {
            Module r = null;
            string cmd = BaseQuery + "and moduleName = '" + key + "'";
            r = Context.Sql(cmd).QuerySingle<Module>(Mapper);
            return r;
        }
        public override int Update(Module t)
        {
            int r = -1;
            r = Context.Update(TableName, t)
                .Column("moduleName", t.Name)
                .Column("moduleDescription", t.Description)
                .Where("moduleID", t.ID)
                .Execute();
            return r;
        }
        public override int Delete(long id)
        {
            int r = -1;
            r = Context.Delete(TableName).Where("moduleID", id).Execute();
            return r;
        }
        public override List<Module> selects(Module t)
        {
            List<Module> r = new List<Module>();
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Sql(cmd).QueryMany<Module>(Mapper);
            return r;
        }
    }

    /*
    public class reportGroupDataAccess : DataAccess<GroupField>
    {
        public reportGroupDataAccess()
        {
            TableName = "report_Group";
            Context = eamsAppDataContextBase.Context;
            setBaseQuery("select [groupID],[reportID],[userID],[groupIndex],[fieldID],[groupString],[isDisplay] from ["+TableName+"] where 1 = 1 ");
        }
        private string WhereStr(GroupField t)
        {
            wStr = new StringBuilder();
            if (t != null)
            {
                if (t.groupID > 0)
                    wStr.Append(" and [groupID] = '" + t.groupID.ToString() + "'");
                if (t.fieldID > 0)
                    wStr.Append(" and [fieldID] = '" + t.fieldID.ToString() + "'");
                if (t.reportID > 0)
                    wStr.Append(" and [reportID] = '" + t.reportID.ToString() + "'");
                if (t.UserID > 0)
                    wStr.Append(" and [UserID] = '" + t.UserID.ToString() + "'");
                if (t.groupIndex > 0)
                    wStr.Append(" and [groupIndex] = '" + t.groupIndex.ToString() + "'");
                if (!string.IsNullOrEmpty(t.groupString))
                    wStr.Append(" and [groupString] like '%" + t.groupString + "%'");
            }
            return wStr.ToString();
        }
        private void Mapper(GroupField m, IDataReader row)
        {
            m.groupID = (int)row.GetValue("groupID");
            m.fieldID = (int)row.GetValue("fieldID");
            m.reportID = (int)row.GetValue("reportID");
            m.UserID = (int)row.GetValue("userID");
            m.groupIndex = (int)row.GetValue("groupIndex");
            m.groupString = row.GetString("groupString");
            m.isDisplay = row.GetBoolean("isDisplay");
        }
        public override long Create(GroupField t)
        {
            long r = -1;
            r = Context.Insert(TableName, t)
                .Column("reportID", t.reportID)
                .Column("userID", t.UserID)
                .Column("groupIndex", t.groupIndex)
                .Column("fieldID", t.fieldID)
                .Column("isDisplay", t.isDisplay)
                .Column("groupString", t.groupString)
                .ExecuteReturnLastId<long>();
            return r;
        }
        public override GroupField Single(int id)
        {
            GroupField r = null;
            string cmd = BaseQuery + "and groupID = " + id.ToString();
            r = Context.Sql(cmd).QuerySingle<GroupField>(Mapper);
            return r;
        }
        public override GroupField Single(string key)
        {
            throw new Exception("不提供！");
            //GroupField r = null;
            //string cmd = BaseQuery + "and [Name] = " + key;
            //r = Context.Sql(cmd).QuerySingle<GroupField>(Mapper);
            return null;
        }
        public override int Update(GroupField t)
        {
            int r = -1;
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Update(TableName, t)
                .Column("reportID", t.reportID)
                .Column("userID", t.UserID)
                .Column("groupIndex", t.groupIndex)
                .Column("fieldID", t.fieldID)
                .Column("isDisplay", t.isDisplay)
                .Column("groupString", t.groupString)
                .Where("groupID", t.groupID)
                .Execute();
            return r;
        }
        public override int Delete(int id)
        {
            int r = -1;
            r = Context.Delete(TableName).Where("groupID", id).Execute();
            return r;
        }
        public override List<GroupField> selects(GroupField t)
        {
            List<GroupField> r = new List<GroupField>();
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Sql(cmd).QueryMany<GroupField>(Mapper);
            return r;
        }
    }

    public class reportOrderDataAccess : DataAccess<OrderField>
    { 
        public reportOrderDataAccess()
        {
            TableName = "report_Order";
            setBaseQuery("select [orderID],[reportID],[userID],[orderIndex],[fieldID],[orderString],[isAsc] from ["+TableName+"] where 1 = 1 ");
        }
        private string WhereStr(OrderField t)
        {
            wStr = new StringBuilder();
            if (t != null)
            {
                if (t.orderID > 0)
                    wStr.Append(" and [orderID] = '" + t.orderID.ToString() + "'");
                if (t.fieldID > 0)
                    wStr.Append(" and [fieldID] = '" + t.fieldID.ToString() + "'");
                if (t.reportID > 0)
                    wStr.Append(" and [reportID] = '" + t.reportID.ToString() + "'");
                if (t.UserID > 0)
                    wStr.Append(" and [UserID] = '" + t.UserID.ToString() + "'");
                if (t.orderIndex > 0)
                    wStr.Append(" and [orderIndex] = '" + t.orderIndex.ToString() + "'");
                if (!string.IsNullOrEmpty(t.orderString))
                    wStr.Append(" and [orderString] like '%" + t.orderString + "%'");
            }
            return wStr.ToString();
        }
        private void Mapper(OrderField m, IDataReader row)
        {
            m.orderID = (int)row.GetValue("orderID");
            m.fieldID = (int)row.GetValue("fieldID");
            m.reportID = (int)row.GetValue("reportID");
            m.UserID = (int)row.GetValue("userID");
            m.orderIndex = (int)row.GetValue("orderIndex");
            m.orderString = row.GetString("orderString");
            m.isAsc = row.GetBoolean("isAsc");
        }
        public override long Create(OrderField t)
        {
            long r = -1;
            r = Context.Insert(TableName, t)
                .Column("reportID", t.reportID)
                .Column("userID", t.UserID)
                .Column("orderIndex", t.orderIndex)
                .Column("fieldID", t.fieldID)
                .Column("isAsc", t.isAsc)
                .Column("orderString", t.orderString)
                .ExecuteReturnLastId<long>();
            return r;
        }
        public override OrderField Single(int id)
        {
            OrderField r = null;
            string cmd = BaseQuery + "and orderID = " + id.ToString();
            r = Context.Sql(cmd).QuerySingle<OrderField>(Mapper);
            return r;
        }
        public override OrderField Single(string key)
        {
            throw new Exception("不提供！");
            //OrderField r = null;
            //string cmd = BaseQuery + "and [Name] = " + key;
            //r = Context.Sql(cmd).QuerySingle<OrderField>(Mapper);
            return null;
        }
        public override int Update(OrderField t)
        {
            int r = -1;
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Update(TableName, t)
                .Column("reportID", t.reportID)
                .Column("userID", t.UserID)
                .Column("orderIndex", t.orderIndex)
                .Column("fieldID", t.fieldID)
                .Column("isAsc", t.isAsc)
                .Column("orderString", t.orderString)
                .Where("orderID", t.orderID)
                .Execute();
            return r;
        }
        public override int Delete(int id)
        {
            int r = -1;
            r = Context.Delete(TableName).Where("orderID", id).Execute();
            return r;
        }
        public override List<OrderField> selects(OrderField t)
        {
            List<OrderField> r = new List<OrderField>();
            string cmd = BaseQuery + WhereStr(t);
            r = Context.Sql(cmd).QueryMany<OrderField>(Mapper);
            return r;
        }
    }    
*/
}
