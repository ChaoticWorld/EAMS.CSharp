using System;
using System.Collections.Generic;
using System.Reflection;

namespace Attendance.Model
{
    public partial class RecordModel
    {
        public RecordModel()
        { }

        public int autoid { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> classID { get; set; }
        public Nullable<int> periodNo { get; set; }
        public Nullable<System.DateTime> sDate { get; set; }
        public string bAttTimeStr { get; set; }
        public string eAttTimeStr { get; set; }
        public Nullable<int> bOffset { get; set; }
        public Nullable<int> eOffset { get; set; }
        public string dayName { get; set; }
        public Nullable<decimal> bOffsetFee { get; set; }
        public Nullable<decimal> eOffsetFee { get; set; }
        public Nullable<ClassPlanModel.dayState> dayType { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public ICalcAttrTime calcAttrTime { get { return getAttrTime(dayType.HasValue ? dayType.Value.ToString() : null); } }
        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<EventDeclaredModel> EventDeclareds { get { return getEvents(); } }

        private ICollection<EventDeclaredModel> getEvents()
        {
            List<EventDeclaredModel> r = new List<EventDeclaredModel>();
            DAL.EventDeclaredDAL edDal = new DAL.EventDeclaredDAL();
            r = edDal.selects(new EventDeclaredModel() { recordID = autoid });
            return r;
        }
        private ICalcAttrTime getAttrTime(string dayType = null)
        {
            if (string.IsNullOrEmpty(dayType))
                throw new InvalidOperationException("dayType日期类型属性未定义！");
            ICalcAttrTime attrTime = null;
            string clsName = "Attendance." + dayType + "AttrTime";
            Assembly Asse = Assembly.Load("Attendance");

            attrTime = (ICalcAttrTime)Asse.CreateInstance(clsName);
            return attrTime;
        }
    }
}
