using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBAccessBase;

namespace strategyLib
{
    public partial class InvClsStdConvertRate : IInvStdConvert
    {
        public int autoid { get; set; }
        public int invClsID { get; set; }
        public string invClsName { get; set; }
        public string invStd { get; set; }
        public decimal priceRate { get; set; }
        /// <summary>
        /// 从参数中克隆数据到自身,除ID外
        /// </summary>
        /// <param name="item">被克隆的数据实例项</param>        
        public void Clone(InvClsStdConvertRate item)
        {
            this.invClsID = item.invClsID;
            this.invClsName = item.invClsName;
            this.invStd = item.invStd;
            this.priceRate = item.priceRate;
        }

    }
    public interface IInvStdConvert
    {
        void Clone(InvClsStdConvertRate s);
        //autoid,invClsID,invClsName,invStd,priceRate
        int autoid { get; set; }
        int invClsID { get; set; }
        string invClsName { get; set; }
        string invStd { get; set; }
        decimal priceRate { get; set; }

    }

    public partial class InvStdConvertDAL : DAL,IdbCRUD<InvClsStdConvertRate>
    {
        public long Create(InvClsStdConvertRate t)
        {
            long id = 0;
            id = Context.Insert("InvClsStdConvertRate", t)
                .Column("invClsID", t.invClsID)
                .Column("invClsName",t.invClsName)
                .Column("invStd",t.invStd)
                .Column("priceRate",t.priceRate)
                .ExecuteReturnLastId<int>();
            //.ExecuteReturnLastId<InvClsStdConvertRate>();
            return id;
        }
        public InvClsStdConvertRate Retrieve(int id) 
        {
            StringBuilder cmd = new StringBuilder();
            cmd.Append(@"select * from InvClsStdConvertRate where autoid = '" + id + "'");
            var single = Context.Sql(cmd.ToString())
                .QuerySingle<InvClsStdConvertRate>();
            return single;
        }
        public InvClsStdConvertRate Retrieve(string code) {
            StringBuilder cmd = new StringBuilder();
            int invclsID = -1;
            cmd.Append(@"select * from InvClsStdConvertRate where ");
            if (int.TryParse(code, out invclsID))
                cmd.Append(" invClsID = '" + invclsID.ToString() + "'");
            else
                cmd.Append(" invClsName = '" + code + "'" );

            var single = Context.Sql(cmd.ToString())
                .QuerySingle<InvClsStdConvertRate>();
            return single;
        }
        public int Update(InvClsStdConvertRate t)
        {
            int rowsAffected = Context.Update("InvClsStdConvertRate", t)
                                        .AutoMap(x => x.autoid)
                                        .Where(x => x.autoid)
                                        .Execute();
            return rowsAffected;
        }
        public int Delete(long id)
        {
            int rowsAffected = Context.Delete("InvClsStdConvertRate").Where("autoid", id)
                .Execute();
            return rowsAffected;
        }
        public List<InvClsStdConvertRate> getList(InvClsStdConvertRate t) {

            StringBuilder cmd = new StringBuilder();
            cmd.Append("select * from InvClsStdConvertRate where 1 = 1 ");
            List<InvClsStdConvertRate> rates = new List<InvClsStdConvertRate>();
            if (t != null)
            {
                if (t.autoid < 0)
                {
                    if (t.invClsID > 0)
                        cmd.Append("and invClsID = " + t.invClsID);
                    if (!string.IsNullOrEmpty(t.invClsName))
                        cmd.Append("and invClsName like '%" + t.invClsName + "%'");
                    if (!string.IsNullOrEmpty(t.invStd))
                        cmd.Append("and invStd like '%" + t.invStd + "%'");

                    rates = Context.Sql(cmd.ToString()).QueryMany<InvClsStdConvertRate>();
                }
                else rates.Add(Retrieve((int)t.autoid));
            }
            else rates = Context.Sql(cmd.ToString()).QueryMany<InvClsStdConvertRate>();
            return rates;
        }
    }
}
