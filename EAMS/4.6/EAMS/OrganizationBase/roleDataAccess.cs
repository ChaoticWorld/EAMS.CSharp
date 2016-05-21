using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentData;

namespace OrganizationBase
{
    public class roleDataAccess : Organization<roleModel>
    {
        public roleDataAccess() : base("Roles")
        {
            setBaseQuery("select iRoleId,cRoleName,cRoleDescription from  [" + TableName + "] where 1 = 1");
        }
        protected override string WhereStr(roleModel m)
        {
            wStr = new StringBuilder();
            if (m != null)
            {
                if (m.iRoleId > 0)
                    wStr.Append(" and iRoleId = " + m.iRoleId);
                if (!string.IsNullOrEmpty(m.cRoleName))
                    wStr.Append(" and cRoleName like '%" + m.cRoleName + "%'");
                if (!string.IsNullOrEmpty(m.cRoleDescription))
                    wStr.Append(" and cRoleDescription like '%" + m.cRoleDescription + "%'");
            }
            return wStr.ToString();
        }
        protected override void orgMapper(roleModel m, IDataReader row)
        {
            m.iRoleId = int.Parse(row.GetValue("iRoleId").ToString());
            m.cRoleName = row.GetString("cRoleName");
            m.cRoleDescription = row.GetString("cRoleDescription");
        }
        public override IList<roleModel> getOrganization(long userid)
        {
            List<roleModel> r = new List<roleModel>();
            IgetOrganization<roleRefModel> getOrgs = new roleRefDataAccess();
            IList<roleRefModel> OrgRefs = getOrgs.getOrganization(userid);
            foreach (roleRefModel OrgRef in OrgRefs)
            {
                r.Add(Single(OrgRef.iRoleId));
            }
            return r;
        }

        public override long Create(roleModel t)
        {
            long r =
            Context.Insert<roleModel>(TableName, t)
                .Column("cRoleName", t.cRoleName)
                .Column("cRoleDescription", t.cRoleDescription)
            //.AutoMap(am => am.iRoleId)
            .ExecuteReturnLastId<int>();
            return r;
        }

        public override int Delete(long id)
        {
            int r =
            Context.Delete(TableName).Where("iRoleId", id).Execute();
            return r;
        }

        public override int Update(roleModel t)
        {
            int r = -1;
            r = Context.Update(TableName, t)
                .Column("cRoleName", t.cRoleName)
                .Column("cRoleDescription", t.cRoleDescription)
                .Where(w => w.iRoleId).Execute();
            return r;
        }

        public override List<roleModel> selects(roleModel t)
        {
            string where = WhereStr(t);
            var r = Context.Sql(BaseQuery + where).QueryMany<roleModel>(orgMapper);
            return r;
        }

        public override roleModel Single(string code)
        {
            string where = " and cRoleName = '" + code + "'";
            var r = Context.Sql(BaseQuery + where).QuerySingle<roleModel>(orgMapper);
            return r;
        }

        public override roleModel Single(long id)
        {
            string where = " and iRoleId = '" + id + "'";
            var r = Context.Sql(BaseQuery + where).QuerySingle<roleModel>(orgMapper);
            return r;
        }

    }

}
