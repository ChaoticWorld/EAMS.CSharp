using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Attendance.Model
{
    public class CardTimeModel
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime firstTime { get; set; }
        public DateTime endTime { get; set; }
        public DateTime cardDate { get; set; }
    }
}
