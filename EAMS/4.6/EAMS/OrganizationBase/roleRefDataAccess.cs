
using System;
using System.Collections.Generic;
using System.Text;
using FluentData;


namespace OrganizationBase
{
    public class roleRefDataAccess : Organization<roleRefModel>
    {
        public roleRefDataAccess() : base("UserRoleRef")
        {
            setBaseQuery(" select iURRefId,iRoleId,iUserId from [" + TableName + "] where 1 = 1 ");
        }
        protected override string WhereStr(roleRefModel m)
        {
            wStr = new StringBuilder();
            if (m != null)
            {
                if (m.iURRefId > 0)
                    wStr.Append(" and iURRefId = " + m.iURRefId);
                if (m.iRoleId > 0)
                    wStr.Append(" and iRoleId = " + m.iRoleId);
                if (m.iUserId > 0)
                    wStr.Append(" and iUserId = " + m.iUserId);
            }
            return wStr.ToString();
        }
        protected override void orgMapper(roleRefModel m, IDataReader row)
        {
            m.iRoleId = int.Parse(row.GetValue("iRoleId").ToString());
            m.iURRefId = int.Parse(row.GetValue("iURRefId").ToString());
            m.iUserId = int.Parse(row.GetValue("iUserId").ToString());
        }

        public override IList<roleRefModel> getOrganization(long userid)
        {
            List<roleRefModel> r = new List<roleRefModel>();
            roleRefModel k = new roleRefModel() { iUserId = userid };
            r = selects(k);
            return r;
        }
        public override long Create(roleRefModel t)
        {
            long r = Context.Insert(TableName, t)
                .Column("iRoleId", t.iRoleId)
                .Column("iUserId", t.iUserId)
                .ExecuteReturnLastId<int>();
            return r;
        }

        public int DeleteWithRoleID(long id)
        {
            int r =
            Context.Delete(TableName).Where("iRoleId", id).Execute();
            return r;
        }

        public override int Delete(long id)
        {
            int r =
            Context.Delete(TableName).Where("iURRefId", id).Execute();
            return r;
        }

        public override int Update(roleRefModel t)
        {
            int r = -1;
            r = Context.Update(TableName, t)
                .Column("iRoleId", t.iRoleId)
                .Column("iUserId", t.iUserId)
                .Where(w => w.iURRefId).Execute();
            return r;
        }

        public override List<roleRefModel> selects(roleRefModel t)
        {
            string where = WhereStr(t);
            Context.IgnoreIfAutoMapFails(true);
            var r = Context.Sql(BaseQuery + where).QueryMany<roleRefModel>();// (orgMapper);
            return r;
        }
        public List<roleRefModel> selects(string cmd)
        {
            List<roleRefModel> r = new List<roleRefModel>();
            if (string.IsNullOrEmpty(cmd)) r = null;
            else
                r = Context.Sql(cmd).QueryMany<roleRefModel>();

            return r;

        }

        public override roleRefModel Single(string code)
        {
            throw new Exception("不提供关键字查询");
        }

        public override roleRefModel Single(long id)
        {
            string where = " and iURRefId = '" + id + "'";
            var r = Context.Sql(BaseQuery + where).QuerySingle<roleRefModel>(orgMapper);
            return r;
        }

    }
}


