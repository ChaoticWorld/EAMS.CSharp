using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserInfo;
namespace OrganizationBase
{
    public class roleModel : ICloneable
    {
        public int iRoleId { get; set; }
        public string cRoleName { get; set; }
        public string cRoleDescription { get; set; }
        public IList<UserModel> Users { get { return setRef(); } }// set; }
        private IList<UserModel> _users;

        public IList<UserModel> setRef()
        {
            roleRefDataAccess rrDA = new roleRefDataAccess();
            var refs = rrDA.selects(new roleRefModel() { iRoleId = this.iRoleId });
            UserDataAccess uDA = new UserDataAccess();
            if (null != refs && refs.Count > 0)
            {
                this._users = new List<UserModel>();
                foreach (roleRefModel rr in refs)
                    this._users.Add(uDA.Single(rr.iUserId));
            }
            return _users;
        }

        public object Clone()
        {
            roleModel rm = new roleModel()
            {
                iRoleId = this.iRoleId,
                cRoleDescription = this.cRoleDescription,
                cRoleName = this.cRoleName
            };
            return rm;
        }
    }
}
