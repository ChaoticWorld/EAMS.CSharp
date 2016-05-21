using System;
using System.Collections.Generic;
using System.Linq;
using FluentData;

namespace VouchType
{
    public class VouchType : IVouchType
    {
        static IDbContext Context = DataAccess.eamsAppDataContextBase.Context;

        string sqlBase = "select id,vouchClass,vouchType,vouchName,vouchOrder,[KEY] from vouchType where 1 = 1 ";
        public int id { get; set; }
        public string vouchName { get; set; }
        public string Key { get; set; }
        public string vouchClass { get; set; }
        public int vouchOrder { get; set; }
        public string vouchType { get; set; }

        public VouchType Clone()
        {
            VouchType n = new VouchType()
            {
                id = this.id,
                vouchName = this.vouchName,
                Key = this.Key,
                vouchClass = this.vouchClass,
                vouchOrder = this.vouchOrder,
                vouchType = this.vouchType
            };
            return n;
        }
        public VouchType() { }
        public static List<VouchType> Init()
        {
            List<VouchType> list = new List<VouchType>();
            string sql = "select id,vouchClass,vouchType,vouchName,vouchOrder,[KEY] "
                + " from vouchType where 1 = 1 ";
            list = VouchType .Context.Sql(sql).QueryMany<VouchType>();
            return list;
        }

        public long Create(VouchType t)
        {
            long r = -1;
            r = Context.Insert("VouchType", t)
                .AutoMap(a => a.id)
                .ExecuteReturnLastId<long>();
            return r;
        }
        public VouchType Retrieve(int id)
        {
            VouchType r = new VouchType();
            string sql = sqlBase + " and id = " + id.ToString();
            r = Context.Sql(sql).QuerySingle<VouchType>();
            return r;
        }
        public VouchType Retrieve(string code)
        {
            VouchType r = new VouchType();
            string sql = sqlBase + " and Key = " + code;
            r = Context.Sql(sql).QuerySingle<VouchType>();
            return r;
        }
        public int Update(VouchType t)
        {
            int r = -1;
            r = Context.Update<VouchType>("VouchType", t)
                        .AutoMap(a => a.id)
                        .Where(w => w.id)
                        .Execute();
            return r;
        }
        public int Delete(int id)
        {
            int r = -1;
            r = Context.Delete("VouchType")
                        .Where("id", id)
                        .Execute();
            return r;
        }
        public List<VouchType> getList(VouchType t)
        {
            List<VouchType> r = new List<VouchType>();
            string sql = string.Empty;
            if (t.id > 0)
                sql += " and id = " + t.id.ToString();
            if (!string.IsNullOrEmpty(t.Key))
                sql += " and [Key] = '" + t.Key + "'";
            if (!string.IsNullOrEmpty(t.vouchClass))
                sql += " and vouchClass = '" + t.vouchClass + "'";
            if (!string.IsNullOrEmpty(t.vouchName))
                sql += " and vouchName = '" + t.vouchName + "'";
            if (!string.IsNullOrEmpty(t.vouchType))
                sql += " and vouchType = '" + t.vouchType + "'";
            if (t.vouchOrder > 0)
                sql += " and vouchOrder = " + t.vouchOrder.ToString();
            r = Context.Sql(sqlBase + sql).QueryMany<VouchType>();
            return r;
        }
    }

}
