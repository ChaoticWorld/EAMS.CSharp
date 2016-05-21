using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using IDataAccess;
using System.Data;

namespace report
{
    public class ReportBLL
    {
        private static DataTable _dataTable = new DataTable();
        public DataTable ReportDataTable { get { return _dataTable; } private set { _dataTable = value; } }
        public DataAccess<Report> reportDA = new reportDataAccess();
        public reportFieldDataAccess reportFieldDA = new reportFieldDataAccess();
        public reportFilterDataAccess reportFilterDA = new reportFilterDataAccess();
        public reportPermissionDataAccess reportPermissionDA = new reportPermissionDataAccess();
        public v_ReportPermissionDataAccess v_ReportPermissonDA = new v_ReportPermissionDataAccess();
        public reportOrderFieldDataAccess reportOrderDA = new reportOrderFieldDataAccess();
        public reportPivotViewDataAccess reportPivotViewDA = new reportPivotViewDataAccess();
        public reportDataSourceAccess reportDSDA = new reportDataSourceAccess();
        #region OrderField
        public List<OrderField> getOrderField(long reportID, long userID = -999)
        {
            List<OrderField> r = new List<OrderField>();
            r = reportOrderDA.selects(new OrderField() { reportID = reportID, UserID = userID });
            if (r.Count <= 0)
                r = reportOrderDA.selects(new OrderField() { reportID = reportID,  UserID = -999 });
            return r; }
        public long saveOrderField(long reportID, OrderField of)
        {
            long r = -1;
            if (of.orderID >= 0) { r = of.orderID; reportOrderDA.Update(of);  }
            else r = reportOrderDA.Create(of);
            return r;
        }
        public int deleteOrderField(long ofid)
        {
            int r = -1;
            r = reportOrderDA.Delete(ofid);
            return r;
        }
        #endregion
        #region PivotView
        public List<PivotView> getPivotView(long reportID, long userID = -999)
        {
            List<PivotView> r = new List<PivotView>();
            r = reportPivotViewDA.selects(new PivotView() { reportID = reportID, userID = userID });
            if (r.Count <= 0)
                r = reportPivotViewDA.selects(new PivotView() { reportID = reportID, userID = -999 });            
            return r;
        }
        public int savePivotView(long reportID,PivotView pv)
        {
            int r = -1;
            if (pv.autoid >0)
               r =  reportPivotViewDA.Update(pv);
            else r = (int)reportPivotViewDA.Create(pv);
            return r;
        }
        public int deletePivotView(long pvid)
        {
            int r = -1;
            r = reportPivotViewDA.Delete(pvid);
            return r;
        }
        #endregion
        #region Filter

        public List<FilterField> getFilter(long reportID,long userID = -999)
        {
            List<FilterField> r = null;
            if (reportID > 0)
                r = reportFilterDA.selects(new FilterField() { reportID = reportID,UserID = userID });
            return r;
        }
        public long saveFilter(FilterField _ff)
        {
            long r = -1;
            if (_ff.filterID>0)
                r = reportFilterDA.Update(_ff);
            else
                r = reportFilterDA.Create(_ff);
            return r;
        }
        public int deleteFilter(long filterID)
        {
            int r = -1;
            r = reportFilterDA.Delete(filterID);
            return r;
        }
        #endregion
        #region Report
        public List<Report> getReportsWithUserID(long UserID, string key = null)
        {
            List<Report> r = null;
            if (!string.IsNullOrEmpty(key))
                r = v_ReportPermissonDA.selects(new v_ReportPermission() { UserID = UserID, Name = key }).ConvertAll(c => (Report)c);
            else
                r = v_ReportPermissonDA.selects(new v_ReportPermission() { UserID = UserID }).ConvertAll(c => (Report)c);
            return r;
        }
        public List<Report> getReports(string key = null)
        {
            List<Report> r = null;
            if (!string.IsNullOrEmpty(key))
                r = reportDA.selects(new Report() { Name = key });
            else
                r = reportDA.selects(null);
            return r;
        }
        public Report getReport(long id)
        {
            Report r = null;
            if (id > 0)
                r = reportDA.Single(id);
            return r;
        }
        public long saveReport(Report _report)
        {
            long r = -1;
            if (_report.reportID > 0)
            { r = reportDA.Update(_report);
                if (r > 0) r = _report.reportID;
                else r = -1;
            }
            else
                r = reportDA.Create(_report);
            return r;
        }
        public int deleteReport(long reportID)
        {
            int r = -1;
            r = reportDA.Delete(reportID);
            return r;
        }
        #endregion
        #region Field
        public List<Field> getFields(long reportID)
        {
            List<Field> r = null;
            if (reportID > 0)
                r = reportFieldDA.selects(new Field() { reportID = reportID });
            return r;
        }
        public long saveField(Field f)
        {
            long r = -1;
            if (f.fieldID < 0) r = reportFieldDA.Create(f);
            else r = reportFieldDA.Update(f);
            return r;
        }
        public int deleteField(long fieldID)
        {
            int r = -1;
            r = reportFieldDA.Delete(fieldID);
            return r;
        }

        #endregion
        #region Permission
        public List<Permission> getPermissions(long permissionID = -1)
        {
            List<Permission> r = new List<Permission>();
            if (permissionID < 0)
                r.AddRange(reportPermissionDA.selects(null));
            else r.Add(reportPermissionDA.Single(permissionID));
            return r;
        }
        public long savePermission(Permission _rp)
        {
            long r = -1;
            if (_rp.AutoID > 0)
                r = reportPermissionDA.Update(_rp);
            else
                r = reportPermissionDA.Create(_rp);
            return r;
        }
        public int deletePermission(long permissionID)
        {
            int r = -1;
            r = reportPermissionDA.Delete(permissionID);
            return r;
        }
        #endregion
            #region getDataSource
        public DataTable getDataSource(long reportID, string QueryCmd = null, string dateField = null, int dyear = -1) {
            return getDataSource(reportID, QueryCmd, dateField, dyear);
        }
        public DataTable getDataSource(long reportID, string QueryCmd = null, string dateField = null, int dyear = -1, string personField = "", string personName = "", string OrderString = "")
        {
            DataTable r = new DataTable();
            string cmd = (!string.IsNullOrEmpty(QueryCmd)) ? QueryCmd : "";
            reportDSDA.setQueryCommand(cmd);
            reportDSDA.getDataSource(dateField, dyear, personField, personName, OrderString);
            r = reportDSDA.DataTableSource;
            if (r == null) return r;
            var fields = getFields(reportID);
            int idx = 0;
            foreach (DataColumn dc in r.Columns) dc.Caption = null;
            foreach (Field f in fields)
            {
                idx = r.Columns.IndexOf(f.fieldName);
                if (idx>=0)
                r.Columns[idx].Caption = (f.isDisplay) ? f.fieldTitle : null;
            }
            var forColumns = new DataColumn[r.Columns.Count];
            r.Columns.CopyTo(forColumns,0);
            foreach (DataColumn dc in forColumns)
            { if (string.IsNullOrEmpty(dc.Caption)) r.Columns.Remove(dc); }
           
            return r;
        }
        #endregion
    }
}
