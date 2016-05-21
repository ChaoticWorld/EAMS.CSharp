using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Attendance.Model
{
    public partial class vAttendanceModel
    {
        private List<vAttendanceEventModel> _eventList = new List<vAttendanceEventModel>();
        [Newtonsoft.Json.JsonIgnore]
        public List<vAttendanceEventModel> EventList { get { return _eventList; } set { _eventList = value; } }
    }
    public partial class ClassPlanModel
    {
        public enum dayState {
            WorkDay,
            Holiday,
            DayOff            
        }
        private dayState _daystate;
        public dayState DayState { get { return _daystate; } }
        public void setDayState(dayState ds) {
            _daystate = ds;
        }
    }
    public partial class FeeCalculatorModel
    {
        public FeeCalculatorModel()
        { initialize(); }
        private static Dictionary<string, string> langDayState = new Dictionary<string, string>();
        public static Dictionary<string, string> cnDayState { get { return langDayState; } set { langDayState = value; } }
        public static void initialize() {
            cnDayState.Clear();
            cnDayState.Add("Holiday","节假日");
            cnDayState.Add("DayOff","休息日");
            cnDayState.Add("WorkDay","工作日");
            cnDayState.Add("Absence","缺勤");
            cnDayState.Add("Over", "加班");
            cnDayState.Add("Leave", "请假");
        }
        public enum dayState
        {
            Holiday,
            DayOff,
            WorkDay,
            Absence,
            Over,
            Leave
        }
        private dayState _daystate;
        public dayState DatState { get { return _daystate; } }
        public void setDayState(dayState ds)
        {
            _daystate = ds;
        }
    }    
}
