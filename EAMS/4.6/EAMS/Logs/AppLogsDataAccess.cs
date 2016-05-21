using DataAccess;

namespace Logs
{
    public class AppLogsDataAccess : LogsDataAccess
    {
        public AppLogsDataAccess(string logTableName) : base(logTableName)
        {
            Context = eamsAppDataContextBase.getContext();
        }
    }
}
