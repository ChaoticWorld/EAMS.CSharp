using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAttendanceDevice
{
    public interface IAttendanceDevice
    {
        ICardTime cardTime { get; set; }
        IEmployee cardEmployee { get; set; }
        IBLL cardBll { get; set; }
    }
}
