using DataAccess;

namespace Logs
{
    public class SysLogsDataAccess : LogsDataAccess
    {
        public SysLogsDataAccess() : base("Logs")
        {
            Context = eamsAppSystemContextBase.getContext();
        }
    }
}
