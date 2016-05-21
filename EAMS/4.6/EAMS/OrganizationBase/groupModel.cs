using System;
using System.Collections.Generic;
using UserInfo;

namespace OrganizationBase
{
    public class groupModel : ICloneable
    {
        public int groupId { get; set; }
        public string groupName { get; set; }
        public string groupDescription { get; set; }
        public long adminId { get; set; } = -1;
        public IList<UserModel> Users
        { get { return setRef(); } }
        private List<UserModel> _users;

        private IList<UserModel> setRef()
        {
            groupRefDataAccess grDA = new groupRefDataAccess();
            UserDataAccess uDA = new UserDataAccess();
            var refs = grDA.selects(new groupRefModel() { groupId = this.groupId });
            if (null != refs && refs.Count > 0)
            {
                _users = new List<UserModel>();
                foreach (groupRefModel gr in refs)
                {
                    if (gr.isManager) adminId = gr.UserId;
                    _users.Add(uDA.Single(gr.UserId));
                }
            }
            return _users;
        }

        public object Clone()
        {
            groupModel gm = new groupModel() {
                groupId = this.groupId,
                groupName = this.groupName,
                groupDescription = this.groupDescription,
                adminId = this.adminId
            };
            return gm;
        }
    }
}
