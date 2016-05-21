using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locks
{
    public class LockModel
    {
        public long autoId { get; set; }
        public string module { get; set; }
        public string locker { get; set; }
        public string key { get; set; }
        public string id { get; set; }
        public DateTime createDate { get; set; }

    }
}
