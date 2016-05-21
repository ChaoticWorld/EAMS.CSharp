using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs
{
    public class LogModel
    {
        public long iLogId { get; set; }
        public DateTime dLogDate { get; set; }
        public long iUserID { get; set; }
        public string cWorkStation { get; set; }
        public string cIP { get; set; }
        public string cBeforeValue { get; set; }
        public string cModule { get; set; }
        public string cFunction { get; set; }
        public string cParams { get; set; }
        public string cClass { get; set; }
        public string cReturn { get; set; }
        public string cException { get; set; }
        public string cUserName { get; set; }
    }
}
