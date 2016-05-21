using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataDB.ModelBase
{
    interface IOperatorBase
    {
        string CreateMan { get; set; }
        string ModifyMan { get; set; }
        DateTime CreateDate { get; set; }
        DateTime ModifyDate { get; set; }
    }
}
