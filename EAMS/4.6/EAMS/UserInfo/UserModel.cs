using System;

namespace UserInfo
{
    public class UserModel : ICloneable
    {
        public long iUserId { get; set; }
        public string cUserCode { get; set; }
        public string cUserName { get; set; }
        public string cUserPassWord { get; set; }
        public string cUserEMail { get; set; }
        public string cUserMobile { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
