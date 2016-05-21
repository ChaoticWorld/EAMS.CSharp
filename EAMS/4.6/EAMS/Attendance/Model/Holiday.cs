using System;

namespace Attendance.Model
{   
    public partial class HolidayModel
    {
        public int autoid { get; set; }
        public int iYear { get; set; }
        public Nullable<System.DateTime> sDate { get; set; }
        public string sName { get; set; }
        public string sDescription { get; set; }
    }
}
