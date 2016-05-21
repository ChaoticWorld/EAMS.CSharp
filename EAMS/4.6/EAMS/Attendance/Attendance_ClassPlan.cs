using System;
using System.Collections.Generic;
namespace Attendance.Model
{  public partial class ClassPlanModel
    {
        public int autoid { get; set; }
        public Nullable<int> classId { get; set; }
        public int periodNo { get; set; }
        public string bTime { get; set; }
        public string eTime { get; set; }
        public string upTime { get; set; }
        public Nullable<System.DateTime> sdate { get; set; }
    
        public virtual Attendance_SchemeClass Attendance_SchemeClass { get; set; }
    }
}
