using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logs;

namespace UserInfo
{
    public class UserLogs: LogsDataAccess
    {
        public UserLogs() : base("Logs")
        { Context = DataAccess.eamsAppSystemContextBase.Context;  }
    }
}
