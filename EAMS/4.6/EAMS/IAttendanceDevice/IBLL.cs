using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAttendanceDevice
{
    public interface IBLL
    {
        List<ICardTime> getCardTime(DateTime d, int eid);
        IEmployee getDevEmployee(int eid);
        List<IEmployee> getDevEmployees(int bid);
        List<ICardTime> getCardTimeDays(int eid, DateTime begin, DateTime end);
    }
}
