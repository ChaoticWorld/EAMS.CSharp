using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDataAccess;
using FluentData;


namespace OrganizationBase
{
    public class groupRefDataAccess :Organization<groupRefModel>
    {
        public groupRefDataAccess():base("UserGroupRef")
        {
            setBaseQuery(" select autoid,groupId,UserId,isManager from [" + TableName + "] where 1 = 1");
        }
        protected override string WhereStr(groupRefModel m)
        {
            wStr = new StringBuilder();
            if (m != null)
            {
                if (m.autoid > 0)
                    wStr.Append(" and autoid = " + m.autoid);
                if (m.groupId > 0)
                    wStr.Append(" and groupId = " + m.groupId);
                if (m.UserId>0)
                    wStr.Append(" and UserId = " + m.UserId);
                if (m.isManager)
                    wStr.Append(" and isManager = " + 1 );
            }
            return wStr.ToString();
        }
        protected override void orgMapper(groupRefModel m,IDataReader row)
        {
            m.autoid = int.Parse(row.GetValue("autoid").ToString());
            m.groupId = int.Parse(row.GetValue("groupId").ToString());
            m.UserId = int.Parse(row.GetValue("UserId").ToString());
            m.isManager = row.GetBoolean("isManager");
        }

        public override IList<groupRefModel> getOrganization(long userid)
        {
            List<groupRefModel> r = new List<groupRefModel>();
            groupRefModel k = new groupRefModel() { UserId = userid };
            r = selects(k);
            return r;
        }
        public override long Create(groupRefModel t)
        {
            long r =
            Context.Insert(TableName, t)
            .Column("groupId", t.groupId)
            .Column("UserId", t.UserId)
            .Column("isManager", t.isManager ? 1 : 0)
            .ExecuteReturnLastId<int>();
            return r;
        }

        public override int Delete(long id)
        {
            int r =
            Context.Delete(TableName).Where("autoid", id).Execute();
            return r;
        }
        public int DeleteWithGroupID(int groupID)
        {
            int r =
                Context.Delete(TableName).Where("groupId", groupID).Execute();
            return r;
        }

        public override int Update(groupRefModel t)
        {
            int r = -1;
            r = Context.Update(TableName, t).AutoMap(am => am.autoid)
                .Where(w=>w.autoid).Execute();
            return r;
        }

        public override List<groupRefModel> selects(groupRefModel t)
        {
            string where = WhereStr(t);
            var r = Context.Sql(BaseQuery + where).QueryMany<groupRefModel>(orgMapper);
            return r;
        }
        
        public override groupRefModel Single(string code)
        {
            throw new Exception("不提供关键字查询");
        }

        public override groupRefModel Single(long id)
        {
            string where = " and autoid = '" + id + "'";
            var r = Context.Sql(BaseQuery + where).QuerySingle<groupRefModel>(orgMapper);
            return r;
        }

    }
}
