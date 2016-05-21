using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VouchType
{
    interface IVouchType
    {
        int id { get; set; }
        string vouchName { get; set; }
        string Key { get; set; }
        string vouchClass { get; set; }
        int vouchOrder { get; set; }
        string vouchType { get; set; }

        VouchType Clone();
    }
}
