using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAttendanceDevice
{
    public interface IEmployee
    {
        int EmployeeId { get; set; }
        string EmployeeName { get; set; }
        string EmployeeCode { get; set; }
        int BrchID { get; set; }
        string BrchName { get; set; }               
    }
}
