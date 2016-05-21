using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentData;
namespace DataDB
{
    public class dbAppData
    {
        public dbAppData() { }
        ~dbAppData() { }

    }
    public class DAL
    {
        protected static IDbContext Context =
           new DbContext().ConnectionStringName("StrategyDB", new SqlServerProvider());
    }
}
