using System;

namespace Attendance
{
    public partial class vAttendanceModel
    {
        public int autoid { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> periodNo { get; set; }
        public Nullable<System.DateTime> sDate { get; set; }
        public string bAttTimeStr { get; set; }
        public string eAttTimeStr { get; set; }
        public Nullable<int> bOffset { get; set; }
        public Nullable<int> eOffset { get; set; }
        public Nullable<decimal> bOffsetFee { get; set; }
        public Nullable<decimal> eOffsetFee { get; set; }
    }
}
