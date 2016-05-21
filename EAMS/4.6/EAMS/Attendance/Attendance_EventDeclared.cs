using System;

namespace Attendance
{
    
    public partial class EventDeclaredModel
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
    
        public virtual Records Records { get; set; }
    }
}
