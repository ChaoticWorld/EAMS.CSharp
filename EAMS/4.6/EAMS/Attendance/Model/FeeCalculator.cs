using System;

namespace Attendance.Model
{
    public partial class FeeCalculatorModel
    {
        public int id { get; set; }
        public Nullable<int> Unit { get; set; }
        public Nullable<decimal> UnitFee { get; set; }
        public Nullable<decimal> MaxFee { get; set; }
        public Nullable<int> classId { get; set; }
        public string dateEnum { get; set; }
    }
}
