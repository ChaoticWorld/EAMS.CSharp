using System;
using System.Collections.Generic;
using OrganizationBase;
using UserInfo;

namespace UserBLL
{
    public class userBLL
    {
        private UserDataAccess uDA = new UserDataAccess();
        public userBLL()
        { }

        public int Register(UserModel m)
        {
            int uid = -1;
            string existMessage = string.Empty;
            if (isExist(m, out existMessage))
                throw new Exception("UserCode:" + m.cUserCode + "用户已存在!请修改参数" + existMessage + ",后重新注册.");
            else
                uid = (int)uDA.Create(m);
            return uid;
        }
        private bool isFieldExist(string PropertyName, string PropertyValue)
        {
            List<UserModel> us;
            UserModel um = new UserModel();
            var oType = um.GetType();
            var field = oType.GetProperty(PropertyName);
            field.SetValue(um, PropertyValue, null);

            us = uDA.selects(um);
            if (us != null && us.Count > 0)
                return true;
            else
                return false;
        }
        private bool isExist(UserModel m, out string message)
        {
            message = string.Empty;
            if (m.iUserId > 0 && uDA.Single(m.iUserId) != null)
            { message = "UserId"; return true; }
            if (!string.IsNullOrEmpty(m.cUserCode) && uDA.Single(m.cUserCode) != null)
            { message = "UserCode"; return true; }
            if (!string.IsNullOrEmpty(m.cUserName) && isFieldExist("cUserName", m.cUserName))
            { message = "UserName"; return true; }
            if (!string.IsNullOrEmpty(m.cUserEMail) && isFieldExist("cUserEMail", m.cUserEMail))
            { message = "UserEMail"; return true; }
            if (!string.IsNullOrEmpty(m.cUserMobile) && isFieldExist("cUserMobile", m.cUserMobile))
            { message = "UserMobile"; return true; }
            return false;
        }
        public UserModel Authentication(string id, string pw)
        {
            UserModel m = new UserModel();
            UserModel um = null;
            if (WebCommon.RegExp.IsEmail(id))
                m.cUserEMail = id;
            else if (WebCommon.RegExp.IsMobile(id))
                m.cUserMobile = id;
            else //if (WebCommon.RegExp.IsNumeric(id))
                //m.iUserId = int.Parse(id);
            //else 
                m.cUserCode = id;

            m.cUserPassWord = pw;
            var ums = uDA.selects(m);
            if (ums != null && ums.Count > 0)
                um = ums[0];
            return um;
        }
        public int update(UserModel m)
        { return uDA.Update(m); }
        /// <summary>
        /// 返回删除结果;-2:有关联不能删除,0:删除失败,>0:删除n记录
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public int delete(int id)
        {
            int r = 0;
            if (prepDelete(id)) r = -2;
            else
                r = uDA.Delete(id);
            return r;
        }
        private bool prepDelete(int id)
        {
            bool r = false;
            roleRefDataAccess rRefDA = new roleRefDataAccess();
            groupRefDataAccess gRefDA = new groupRefDataAccess();
            r |= rRefDA.getOrganization(id).Count > 0;
            r |= gRefDA.getOrganization(id).Count > 0;
            return r;
        }
        public IList<UserModel> selects(UserModel t)
        { return uDA.selects(t); }
        public IList<roleModel> getRoles(long userid)
        {
            roleDataAccess rDA = new roleDataAccess();
            return rDA.getOrganization(userid);
        }
        public IList<groupModel> getGroups(long userid)
        {
            groupDataAccess gDA = new groupDataAccess();
            return gDA.getOrganization(userid);
        }
        public string getStrRoles(long userid)
        {
            IList<roleModel> rms = getRoles(userid);
            string roles = string.Empty;
            foreach (roleModel r in rms)
                roles += (r.cRoleName.Trim()+",");
            roles = roles.Substring(0, roles.Length - 1);
            return roles;
        }
        public string getStrGroups(int userid)
        {
            IList<groupModel> gms = getGroups(userid);
            string groups = string.Empty;
            foreach (groupModel g in gms)
                groups += (g.groupName.Trim() + ",");
            return groups;
        }
        public UserModel single(long id)
        {
            UserModel u = uDA.Single(id);
            return u;
        }
        public static UserModel getUser(int id)
        {
            UserDataAccess userDA = new UserDataAccess();
            UserModel u = userDA.Single(id);
            return u;
        }
    }

}
