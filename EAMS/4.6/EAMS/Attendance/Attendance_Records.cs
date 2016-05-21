using System;
using System.Collections.Generic;

namespace Attendance
{
    
    public partial class RecordModel
    {
        public RecordModel()
        {
            //this.Attendance_EventDeclared = new HashSet<Attendance_EventDeclared>();
        }
    
        public int autoid { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> classID { get; set; }
        public Nullable<int> periodNo { get; set; }
        public Nullable<System.DateTime> sDate { get; set; }
        public string bAttTimeStr { get; set; }
        public string eAttTimeStr { get; set; }
        public Nullable<int> bOffset { get; set; }
        public Nullable<int> eOffset { get; set; }
        public string dateType { get; set; }
        public Nullable<decimal> bOffsetFee { get; set; }
        public Nullable<decimal> eOffsetFee { get; set; }
    
        public virtual ICollection<EventDeclaredModel> Attendance_EventDeclared { get; set; }
    }
}
