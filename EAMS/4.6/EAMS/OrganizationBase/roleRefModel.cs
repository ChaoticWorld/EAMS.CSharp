using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrganizationBase
{
    public class roleRefModel:ICloneable
    {
        public long iURRefId { get; set; }
        public int iRoleId { get; set; }
        public long iUserId { get; set; }

        public object Clone()
        {
            roleRefModel rrm = new roleRefModel()
            {
                iRoleId = this.iRoleId,
                iURRefId = this.iURRefId,
                iUserId = this.iUserId
            };
            return rrm;
        }
    }
}
