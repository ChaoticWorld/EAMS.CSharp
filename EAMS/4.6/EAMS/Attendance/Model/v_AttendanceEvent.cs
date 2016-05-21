using System;

namespace Attendance.Model
{
    public partial class vAttendanceEventModel
    {
        public int autoID { get; set; }
        public string EventDescription { get; set; }
        public Nullable<decimal> FeeCar { get; set; }
        public Nullable<decimal> FeeMeals { get; set; }
        public Nullable<decimal> FeeOther { get; set; }
        public Nullable<bool> isCar { get; set; }
        public string Other { get; set; }
        public string Memo { get; set; }
        public string checkMan { get; set; }
        public string Manager { get; set; }
        public int recordID { get; set; }
        public bool isBeginTime { get; set; }
        public Nullable<int> periodNo { get; set; }
        public string bAttTimeStr { get; set; }
        public string eAttTimeStr { get; set; }
        public Nullable<int> bOffset { get; set; }
        public Nullable<int> eOffset { get; set; }
        public Nullable<System.DateTime> sDate { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public string bTime { get; set; }
        public string eTime { get; set; }
    }
}
