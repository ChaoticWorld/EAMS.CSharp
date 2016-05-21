using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DBAccessBase
{
    public class DBAccessBase
    {
        public static string conn = ConfigurationManager.ConnectionStrings["AppSystemEntities"].ConnectionString;
        public static int Records = -1;
        public static string MasterKey = string.Empty;
        public DBAccessBase()
        { ;}
        ~DBAccessBase()
        { ;}
    }
}
