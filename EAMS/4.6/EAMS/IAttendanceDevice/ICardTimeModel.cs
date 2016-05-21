using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAttendanceDevice
{
    public interface ICardTime
    {
        int cardID { get; set; }
        int EmployeeId { get; set; }
        DateTime cardTime { get; set; }

        IEmployee EmployeeInfo { get; }
    }
}
