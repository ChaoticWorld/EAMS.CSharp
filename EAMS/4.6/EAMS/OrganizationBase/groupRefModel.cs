using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrganizationBase
{
    public class groupRefModel : ICloneable
    {
        public long autoid { get; set; }
        public int groupId { get; set; }
        public long UserId { get; set; }
        public bool isManager { get; set; }

        public object Clone()
        {
            groupRefModel grm = new groupRefModel()
            {
                autoid = this.autoid,
                groupId = this.groupId,
                UserId = this.UserId,
                isManager = this.isManager
            };
            return grm;
        }
    }
}
