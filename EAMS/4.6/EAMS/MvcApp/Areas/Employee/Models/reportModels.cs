using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using report;
namespace MvcApp.Areas.Employee.Models
{
    public class reportViewModel
    {
        private Dictionary<Module, Dictionary<Class, List<Report>>> _ViewModel;
        public Dictionary<Module, Dictionary<Class, List<Report>>> ViewModel { get { return _ViewModel; } set { _ViewModel = value; } }
        public reportViewModel()
        { _ViewModel = new Dictionary<Module, Dictionary<Class, List<Report>>>(); }
        ~reportViewModel() { _ViewModel = null; }
        public reportViewModel getReports(long userID = -1)
        {
            //_ViewModel = new Dictionary<Module, Dictionary<Class, List<Report>>>();
            ReportBLL reportBll = new ReportBLL();
            var userreports = reportBll.getReportsWithUserID(userID);
            if (userreports != null && userreports.Count > 0)
            {
                foreach (Report ri in userreports)
                {
                    if (_ViewModel.Keys != null && !_ViewModel.Keys.ToList().Exists(e => e.ID == ri.moduleID))
                        _ViewModel.Add(ri.Module, new Dictionary<Class, List<Report>>());
                    var keymod = _ViewModel.First(modf => modf.Key.ID == ri.Module.ID);
                    if (keymod.Value.Keys != null && !keymod.Value.Keys.ToList().Exists(e => e.ID == ri.clsID))
                        keymod.Value.Add(ri.Class, new List<Report>());
                    var keycls = keymod.Value.First(clsf => clsf.Key.ID == ri.Class.ID);
                    if (keycls.Value != null && !keycls.Value.Exists(e => e.reportID == ri.reportID))
                        keycls.Value.Add(ri);
                }
            }
            return this;
        }
    }
    public class Rpt {
        public long rptID { get; set; }
        public long userID { get; set; }
        public Report report { get; set; }
        public DataTable dataTable { get; set; } = new DataTable();
        public DateTime dataTime { get; set; }
    }
}