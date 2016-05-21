using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAttendanceDevice;

namespace AttendanceDevice.HWATT
{
    public class Employee : IEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int BrchID { get; set; }
        public string BrchName { get; set; }
    }
}
