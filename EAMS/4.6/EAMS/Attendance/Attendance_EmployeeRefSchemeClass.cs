using System;

namespace Attendance.Model
{ 
    public partial class EmployeeRefSchemeClassModel
    {
        public int autoid { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> classID { get; set; }
        public Nullable<System.DateTime> EffDate { get; set; }
        public Nullable<int> periodNo { get; set; }
        public Nullable<bool> isVaild { get; set; }
        public string EmployeeName { get; set; }
    }
}
