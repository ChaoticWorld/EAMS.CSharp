using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDataAccess;
using FluentData;


namespace OrganizationBase
{
    public class groupDataAccess :Organization<groupModel>
    {
        public groupDataAccess():base("Group")
        {
            setBaseQuery("select groupId,groupName,groupDescription from [" + TableName + "] where 1 = 1 ");
        }
        protected override string WhereStr(groupModel m)
        {
            wStr = new StringBuilder();
            if (m != null)
            {
                if (m.groupId > 0)
                    wStr.Append(" and groupId = " + m.groupId);
                if (!string.IsNullOrEmpty(m.groupName))
                    wStr.Append(" and groupName like '%" + m.groupName + "%'");
                if (!string.IsNullOrEmpty(m.groupDescription))
                    wStr.Append(" and groupDescription like '%" + m.groupDescription + "%'");
            }
            return wStr.ToString();
        }
        protected override void orgMapper(groupModel m,IDataReader row) {
            m.groupId = int.Parse(row.GetValue("groupId").ToString());
            m.groupName = row.GetString("groupName");
            m.groupDescription = row.GetString("groupDescription");
        }
        public override IList<groupModel> getOrganization(long userid)
        {
            List<groupModel> r = new List<groupModel>();
            IgetOrganization<groupRefModel> getOrgs = new groupRefDataAccess();
            IList<groupRefModel> OrgRefs = getOrgs.getOrganization(userid);
            foreach (groupRefModel OrgRef in OrgRefs)
            {
                r.Add(Single(OrgRef.groupId));
            }
            return r;
        }

        public override long Create(groupModel t)
        {
            long r =
            Context.Insert("[" + TableName + "]", t)
            //.Column("groupId", t.groupId)
            .Column("groupName", t.groupName)
            .Column("groupDescription", t.groupDescription)
            //.AutoMap(am => am.groupId)
            .ExecuteReturnLastId<int>();

            return r;
        }

        public override int Delete(long id)
        {
            int r =
            Context.Delete("["+TableName+"]").Where("groupId", id).Execute();
            return r;
        }

        public override int Update(groupModel t)
        {
            int r = -1;
            r = Context.Update("[" + TableName + "]", t)
                .Column("groupName", t.groupName)
                .Column("groupDescription", t.groupDescription)
                //.AutoMap(am => am.groupId)
                .Where(w => w.groupId).Execute();
            return r;
        }

        public override List<groupModel> selects(groupModel t)
        {
            string where = WhereStr(t);
            var r = Context.Sql(BaseQuery + where).QueryMany<groupModel>(orgMapper);
            return r;
        }
        
        public override groupModel Single(string code)
        {
            string where = " and groupName = '"+code+"'";
            var r = Context.Sql(BaseQuery + where).QuerySingle<groupModel>(orgMapper);
            return r;
        }

        public override groupModel Single(long id)
        {
            string where = " and groupId = '" + id + "'";
            var r = Context.Sql(BaseQuery + where).QuerySingle<groupModel>(orgMapper);
            return r;
        }

    }
}
