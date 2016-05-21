using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceDevice;
using IAttendanceDevice;

namespace AttendanceDevice.HWATT
{
    public class Device : IAttendanceDevice.IAttendanceDevice
    {
        public IEmployee cardEmployee { get; set; }
        public ICardTime cardTime { get; set; }
        public IBLL cardBll { get; set; }
    }
}
