using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAttendanceDevice;


namespace AttendanceDevice.HWATT
{
    public class CardTime : ICardTime
    {
        public int cardID { get; set; }
        public int EmployeeId { get; set; }
        public DateTime cardTime { get; set; }
        
        public IEmployee EmployeeInfo { get; }
    }
}
