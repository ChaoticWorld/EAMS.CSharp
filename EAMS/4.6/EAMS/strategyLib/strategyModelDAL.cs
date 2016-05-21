using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentData;
using DataDB;

namespace strategyLib
{
    public interface IstrategyMain
    {
        Int64 ID { get; set; }
        string cDCName { get; set; }
        string cDWCode { get; set; }
        string cDWName { get; set; }
        string cDWContact { get; set; }
        DateTime dEffDate { get; set; }
        DateTime dExpDate { get; set; }
        string cVouchCode { get; set; }
        string cVouchType { get; set; }
        //string cBizType { get; set; }
        string cSourceCode { get; set; }
        string cSourceType { get; set; }
        string cMemo { get; set; }
        string cLevel { get; set; }
        Main Clone();
    }
	public interface IstrategyDetail {
        string cinvACode { get; set; }
        string cinvAName { get; set; }
        string cinvAStd { get; set; }
        decimal invAQuantity { get; set; }
        decimal invAPrice { get; set; }
        decimal invARate { get; set; }
        string cinvBCode { get; set; }
        string cinvBName { get; set; }
        string cinvBStd { get; set; }
        decimal invBQuantity { get; set; }
        decimal invBPrice { get; set; }
        decimal invBRate { get; set; }
        Int64 autoid { get; set; }
        long ID { get; set; }
	}
    public class Main : IstrategyMain
    {
        public Main()
        {
            dExpDate = DateTime.MaxValue;
            dEffDate = DateTime.MinValue;
        }
        public Int64 ID { get; set; }
        public string cDCName { get; set; }
        public string cDWCode { get; set; }
        public string cDWName { get; set; }
        public string cDWContact { get; set; }
        public DateTime dEffDate { get; set; }
        public DateTime dExpDate { get; set; }
        public string cVouchCode { get; set; }
        public string cVouchType { get; set; }
        //public string cBizType { get; set; }
        public string cSourceCode { get; set; }
        public string cSourceType { get; set; }
        public string cMemo { get; set; }
        public string cLevel { get; set; }
        public Main Clone() {
            var m = new Main();
            //m.cBizType = this.cBizType;
            m.cDCName = this.cDCName;
            m.cDWCode = this.cDWCode;
            m.cDWContact = this.cDWContact;
            m.cDWName = this.cDWName;
            m.cLevel = this.cLevel;
            m.cMemo = this.cMemo;
            m.cSourceCode = this.cSourceCode;
            m.cSourceType = this.cSourceType;
            m.cVouchCode = this.cVouchCode;
            m.cVouchType = this.cVouchType;
            m.dEffDate = this.dEffDate;
            m.dExpDate = this.dExpDate;
            return m;
        }
    }
    public class Detail : IstrategyDetail
    {
        public string cinvACode { get; set; }
        public string cinvAName { get; set; }
        public string cinvAStd { get; set; }
        public decimal invAQuantity { get; set; }
        public decimal invAPrice { get; set; }
        public decimal invARate { get; set; }
        public string cinvBCode { get; set; }
        public string cinvBName { get; set; }
        public string cinvBStd { get; set; }
        public decimal invBQuantity { get; set; }
        public decimal invBPrice { get; set; }
        public decimal invBRate { get; set; }
        public Int64 autoid{get;set;}
        public long ID { get; set; }
    }

    public interface Istrategy{
        Main main { get; set; }
        List<Detail> details { get; set; }
    }
    public class cStrategy : Istrategy
    {
        public Main main { get; set; }
        public List<Detail> details { get; set; }
    }
    public class vwStrategy
    {
        public vwStrategy() {
            dExpDate = DateTime.MaxValue;
            dEffDate = DateTime.MinValue;
        }
        public Int64 ID { get; set; }
        public string cDCName { get; set; }
        public string cDWCode { get; set; }
        public string cDWName { get; set; }
        public string cDWContact { get; set; }
        public DateTime dEffDate { get; set; }
        public DateTime dExpDate { get; set; }
        public string cVouchCode { get; set; }
        public string cVouchType { get; set; }
        public string cBizType { get; set; }
        public string cSourceCode { get; set; }
        public string cSourceType { get; set; }
        public string cMemo { get; set; }
        public string cLevel { get; set; }
        public string cinvACode { get; set; }
        public string cinvAName { get; set; }
        public string cinvAStd { get; set; }
        public decimal invAQuantity { get; set; }
        public decimal invAPrice { get; set; }
        public decimal invARate { get; set; }
        public string cinvBCode { get; set; }
        public string cinvBName { get; set; }
        public string cinvBStd { get; set; }
        public decimal invBQuantity { get; set; }
        public decimal invBPrice { get; set; }
        public decimal invBRate { get; set; }
        public Int64 autoid { get; set; }
        public Int64 MID { get; set; }
    }
    public class DAL {
         protected static IDbContext Context =
            new DbContext().ConnectionStringName("StrategyDB", new SqlServerProvider());
    }
    public class mainDAL:DAL,DBAccessBase.IdbCRUD<Main> {
        public long Create(Main m)
        {
            long id;
            try
            {
                m.cSourceType =  string.IsNullOrEmpty(m.cSourceType) ? "期初" : m.cSourceType;
                m.dEffDate = (m.dEffDate >= (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue 
                    ||m.dEffDate < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) ?
                    (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue : m.dEffDate;

                m.dExpDate = (m.dExpDate >= (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue 
                    || m.dExpDate < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue )?
                    (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue : m.dExpDate;

                id = Context.Insert<Main>("strategyMain", m).AutoMap(x=>x.ID).ExecuteReturnLastId<long>();
            }
            catch (Exception e) {
                id = -1;
                //throw e;
            }
            return id;
        }
        public int Update(Main m) {
            m.cSourceType = string.IsNullOrEmpty(m.cSourceType) ? "期初" : m.cSourceType;
            m.dEffDate = (m.dEffDate >= (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue
                || m.dEffDate < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) ?
                (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue : m.dEffDate;

            m.dExpDate = (m.dExpDate >= (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue
                || m.dExpDate < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) ?
                (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue : m.dExpDate;
            int rowsAffected = Context.Update<Main>("strategyMain", m)
                .AutoMap(x => x.ID)
                .Where(x => x.ID)
                .Execute();
            return rowsAffected;
        }
        public bool Exists(Main m)
        {
            bool r = false;
            var single = Context.Sql(@"select id from strategyMain where ID = '"+m.ID+"'")
                .QuerySingle<long>();
            r = single > 0;
            return r;
        }
        public Main Retrieve(int id) {
            StringBuilder cmd = new StringBuilder();
            cmd.Append(@"select * from strategyMain where ID = '" + id + "'");
            var single = Context.Sql(cmd.ToString())
                .QuerySingle<Main>();
            return single;
        }
        public Main Retrieve(string code)
        {
            return null;
        }
        public int Delete(int id)
        {
            int rowsAffected = Context.Delete("strategyMain").Where("ID", id)
                .Execute();
            return rowsAffected;
        }
        public List<Main> getList(Main t)
        {
            StringBuilder cmd = new StringBuilder();
            cmd.Append("select * from strategyMain where 1 = 1 ");
            List<Main> mains = new List<Main>();
            if (t != null)
            {
                if (t.ID < 0)
                {
                    if (t.dEffDate != null && t.dEffDate > DateTime.MinValue)
                        cmd.Append("and dEffDate <= '" + t.dEffDate.ToString("yyyy-MM-dd") + "'");
                    if (t.dExpDate != null && t.dExpDate < DateTime.MaxValue)
                        cmd.Append("and dExpDate <= '" + t.dExpDate.ToString("yyyy-MM-dd") + "'");
                    if (!string.IsNullOrEmpty(t.cVouchType))
                        cmd.Append("and cVouchType like '%" + t.cVouchType + "%'");
                    if (!string.IsNullOrEmpty(t.cVouchCode))
                        cmd.Append("and cVouchCode like '%" + t.cVouchCode + "%'");
                    if (!string.IsNullOrEmpty(t.cSourceType))
                        cmd.Append("and cSourceType like '%" + t.cSourceType + "%'");
                    if (!string.IsNullOrEmpty(t.cSourceCode))
                        cmd.Append("and cSourceCode like '%" + t.cSourceCode + "%'");
                    if (!string.IsNullOrEmpty(t.cLevel))
                        cmd.Append("and cLevel like '%" + t.cLevel + "%'");
                    if (!string.IsNullOrEmpty(t.cDWName))
                        cmd.Append("and cDWName like '%" + t.cDWName + "%'");
                    if (!string.IsNullOrEmpty(t.cDWCode))
                        cmd.Append("and cDWCode like '%" + t.cDWCode + "%'");
                    if (!string.IsNullOrEmpty(t.cDWContact))
                        cmd.Append("and cDWContact like '%" + t.cDWContact + "%'");
                    if (!string.IsNullOrEmpty(t.cDCName))
                        cmd.Append("and cDCName like '%" + t.cDCName + "%'");
                    mains = Context.Sql(cmd.ToString()).QueryMany<Main>();
                }
                else mains.Add(Retrieve((int)t.ID));
            }
            else mains = Context.Sql(cmd.ToString()).QueryMany<Main>();
            return mains;
        }
    }
    public class detailDAL : DAL, DBAccessBase.IdbCRUD<Detail>
    {
        public long Create(Detail d)
        {
            long id;
            id = Context.Insert<Detail>("strategyDetail", d).AutoMap(x=>x.autoid).ExecuteReturnLastId<long>();
            return id;
        }
        public int Update(Detail d)
        {
            int rowsAffected = Context.Update<Detail>("strategyDetail", d)
                .AutoMap(x => x.autoid)
                .Where(x => x.autoid)
                .Execute();
            return rowsAffected;
        }
        public bool Exists(Detail d)
        {
            bool r = false;
            var single = Context.Sql(@"select id from strategyDetail where autoid = '" + d.autoid + "'")
                .QuerySingle<long>();
            r = single > 0;
            return r;
        }
        public Detail Retrieve(int id)
        {
            StringBuilder cmd = new StringBuilder();
            cmd.Append(@"select * from strategyDetail where autoid = '" + id + "'");
            var single = Context.Sql(cmd.ToString())
                .QuerySingle<Detail>();
            return single;
        }
        public Detail Retrieve(string code)
        {
            return null;
        }
        public int Delete(int id)
        {
            int rowsAffected = Context.Delete("strategyDetail").Where("autoid", id)
                .Execute();
            return rowsAffected;
        }
        public List<Detail> getList(Detail t)
        {
            StringBuilder cmd = new StringBuilder();
            cmd.Append("select * from strategyDetail where 1 = 1 ");
            List<Detail> details = new List<Detail>();
            if (t != null)
            {
                if (t.autoid <= 0)
                {
                    if (t.ID > 0)
                        cmd.Append("and ID = '" + t.ID + "'");
                    if (!string.IsNullOrEmpty(t.cinvACode))
                        cmd.Append("and cinvACode like '%" + t.cinvACode + "%'");
                    if (!string.IsNullOrEmpty(t.cinvAName))
                        cmd.Append("and cinvAName like '%" + t.cinvAName + "%'");
                    if (!string.IsNullOrEmpty(t.cinvAStd))
                        cmd.Append("and cinvAStd like '%" + t.cinvAStd + "%'");
                    if (!string.IsNullOrEmpty(t.cinvBCode))
                        cmd.Append("and cinvBCode like '%" + t.cinvBCode + "%'");
                    if (!string.IsNullOrEmpty(t.cinvBName))
                        cmd.Append("and cinvBName like '%" + t.cinvBName + "%'");
                    if (!string.IsNullOrEmpty(t.cinvBStd))
                        cmd.Append("and cinvBStd like '%" + t.cinvBStd + "%'");
                    details = Context.Sql(cmd.ToString()).QueryMany<Detail>();
                }
                else details.Add(Retrieve((int)t.autoid));
            }
            else
                details = Context.Sql(cmd.ToString()).QueryMany<Detail>();
            return details;
        }
    }

    public class strategyDAL:DAL
    {
        static mainDAL mDal = new mainDAL();
        static detailDAL dDal = new detailDAL();
        public void Save(cStrategy s)
        {
            long mid = -1;
            if (s.main.ID <= 0)
            {
                mid = mDal.Create(s.main);
                foreach (Detail d in s.details)
                    dDal.Create(d);
            }
            else
            {
                mDal.Update(s.main);
                foreach (Detail d in s.details)
                {
                    if (d.autoid > 0)
                    {
                        if (dDal.Exists(d))
                            dDal.Update(d);
                        else dDal.Create(d);
                    }
                    else dDal.Create(d);
                }
                var sdids = s.details.Select(ds => ds.autoid);
                var oldsdids = dDal.getList(new Detail() { ID = s.main.ID }).Select(dss => dss.autoid);
                var exp = oldsdids.Except(sdids);
                foreach (var dd in exp) dDal.Delete((int)dd);
            }
        }

        public string isValid(DataDB.ModelBase.IVouch vouch)
        {
            string r = string.Empty ;
            DataDB.ModelBase.Main vouchMain = vouch.Main;
            List<DataDB.ModelBase.Detail> vouchDetails = vouch.Details;

            string cmdBase = "select * from vw_strategy";
            StringBuilder cmdwhere = new StringBuilder();
            foreach (DataDB.ModelBase.Detail vd in vouchDetails)
            {
                cmdwhere = new StringBuilder();
                cmdwhere.Append(" where 1 = 1 and cinvAName like '%" + vd.inventory.cInvName + "%'");
                cmdwhere.Append(" and ( dExpDate is null or dEffDate is null or ");
                cmdwhere.Append(" ( dExpdate >= '" + vouchMain.dDate + "' and dEffDate <= '" + vouchMain.dDate + "') )");
                cmdwhere.Append(" and (cDWName is null or cDWName like '%"+vouchMain.customer.cCusAbbName+"%')");
                cmdwhere.Append(" and (cDCName is null or cDCName like '%"+vouchMain.customer.District+"%')");
                cmdwhere.Append(" and (cDWContact is null or cDWContact like '%"+vouchMain.customer.ContactName+"%')");
                cmdwhere.Append(" ORDER BY cLevel desc ");
                string tsql = cmdBase + cmdwhere.ToString();
                List<vwStrategy> sqlResult1 = Context.Sql(cmdBase+cmdwhere.ToString()).QueryMany<vwStrategy>();
                var sqlResult2 = sqlResult1.Where(w=>w.cinvAStd.Contains(vd.inventory.cInvStd));
                foreach (vwStrategy vs in sqlResult2)
                    if ( vd.iPrice < (double)vs.invAPrice )
                    {
                        r = "存货:" + vd.inventory.cInvName + "-" + vd.inventory.cInvStd + "\n低于策略:"
                          + vs.cSourceType + "-" + vs.cSourceCode + ":" + vs.invAPrice.ToString();
                        return r; }
            }
            return r;
        }

        public IstrategyMain getStrategyMain(int ID) {
            return mDal.Retrieve(ID);
        }
        public string isValid(int ID) {
            string r = string.Empty;
            IstrategyMain main = getStrategyMain(ID); 
            DataDB.u8.dbU8 erp = new DataDB.u8.dbU8();
            List<string> mainCodes = new List<string>();
            if (!string.IsNullOrEmpty(main.cVouchCode))
            {
                r += validErpVouch(main.cVouchType, main.cVouchCode);
            }
            else {
                mainCodes = erp.filterDispatchMainCode(main.dEffDate, main.dExpDate, main.cDCName, main.cDWCode);
                foreach (string code in mainCodes) 
                    r += validErpVouch("Dispatch", code);

                mainCodes = erp.filterSaleOrderMainCode(main.dEffDate, main.dExpDate, main.cDCName, main.cDWCode);
                foreach (string code in mainCodes)
                    r += validErpVouch("SaleOrder", code);
                
            }
            return r;
        }
        private string validErpVouch(string vouchType, string vouchCode)
        {
            DataDB.u8.dbU8 erp = new DataDB.u8.dbU8();
            DataDB.ModelBase.IVouch Ivouch;
            string spStr = string.Empty;
            string valid = string.Empty;
            switch (vouchType)
            {
                case "Dispatch":
                    Ivouch = erp.getDispatch(vouchCode);
                    spStr = "saleFHD_validStrategy";
                    break;
                case "SaleOrder":
                    Ivouch = erp.getSaleOrder(vouchCode);
                    spStr = "saleOrder_validStrategy";
                    break;
                default:
                    Ivouch = null;
                    break;
            }
            if (Ivouch != null)
            {
                valid = string.Empty;
                foreach (DataDB.ModelBase.Detail d in Ivouch.Details)
                {
                    var sp =
                        erp.Context.StoredProcedure(spStr)
                           .Parameter("id", Ivouch.Main.id)
                           .Parameter("invcode", d.inventory.cInvCode)
                           .Parameter("iquantity", d.quantity.iQuantity)
                           .Parameter("iTaxUnitPrice", d.iPrice)
                           .ParameterOut("valid", DataTypes.String, 512);
                    try
                    {
                        sp.Execute();
                        valid += sp.ParameterValue<string>("valid") + ";\n";
                    }
                    catch (Exception e) { 
                        var em = e.Message; }
                }
            }
            return valid;
        }
        public List<cStrategy> getStrategys(Main m, Detail d)
        {
            List<cStrategy> r = new List<cStrategy>();
            cStrategy cs = new cStrategy();
            List<long> MIDs = new List<long>();
            List<long> DIDs = new List<long>();
            if (m != null)
                MIDs = mDal.getList(m).Select(s => s.ID).ToList();
            else if (d != null)
            {
                DIDs = dDal.getList(d).Select(s => s.ID).ToList();
                MIDs = MIDs.Union(DIDs).ToList();
            }
            else MIDs = mDal.getList(null).Select(s => s.ID).ToList();
            foreach (long mid in MIDs)
                if (!r.Exists(e => e.main.ID == mid))
                {
                    cs = new cStrategy();
                    cs.main = mDal.Retrieve((int)mid);
                    cs.details = dDal.getList(new Detail() { ID = mid });
                    r.Add(cs);
                }

            return r;
        }

        public List<vwStrategy> getVWStrategys(vwStrategy t)
        {
            List<vwStrategy> r = new List<vwStrategy>();
            vwStrategy s = new vwStrategy();

            StringBuilder cmd = new StringBuilder();
            cmd.Append("select * from AppData.dbo.vw_strategy");

            if (t != null)
            {
                if (t.dEffDate != null && t.dEffDate > DateTime.MinValue)
                    cmd.Append("and dEffDate <= '" + t.dEffDate.ToString("yyyy-MM-dd") + "'");
                if (t.dExpDate != null && t.dExpDate < DateTime.MaxValue)
                    cmd.Append("and dExpDate <= '" + t.dExpDate.ToString("yyyy-MM-dd") + "'");
                if (!string.IsNullOrEmpty(t.cVouchType))
                    cmd.Append("and cVouchType like '%" + t.cVouchType + "%'");
                if (!string.IsNullOrEmpty(t.cVouchCode))
                    cmd.Append("and cVouchCode like '%" + t.cVouchCode + "%'");
                if (!string.IsNullOrEmpty(t.cSourceType))
                    cmd.Append("and cSourceType like '%" + t.cSourceType + "%'");
                if (!string.IsNullOrEmpty(t.cSourceCode))
                    cmd.Append("and cSourceCode like '%" + t.cSourceCode + "%'");
                if (!string.IsNullOrEmpty(t.cLevel))
                    cmd.Append("and cLevel like '%" + t.cLevel + "%'");
                if (!string.IsNullOrEmpty(t.cDWName))
                    cmd.Append("and cDWName like '%" + t.cDWName + "%'");
                if (!string.IsNullOrEmpty(t.cDWCode))
                    cmd.Append("and cDWCode like '%" + t.cDWCode + "%'");
                if (!string.IsNullOrEmpty(t.cDWContact))
                    cmd.Append("and cDWContact like '%" + t.cDWContact + "%'");
                if (!string.IsNullOrEmpty(t.cDCName))
                    cmd.Append("and cDCName like '%" + t.cDCName + "%'");

                if ( t.ID > 0)
                    cmd.Append("and ID = '" + t.ID + "'");
                if (!string.IsNullOrEmpty(t.cinvACode))
                    cmd.Append("and cinvACode like '%" + t.cinvACode + "%'");
                if (!string.IsNullOrEmpty(t.cinvAName))
                    cmd.Append("and cinvAName like '%" + t.cinvAName + "%'");
                if (!string.IsNullOrEmpty(t.cinvAStd))
                    cmd.Append("and cinvAStd like '%" + t.cinvAStd + "%'");
                if (!string.IsNullOrEmpty(t.cinvBCode))
                    cmd.Append("and cinvBCode like '%" + t.cinvBCode + "%'");
                if (!string.IsNullOrEmpty(t.cinvBName))
                    cmd.Append("and cinvBName like '%" + t.cinvBName + "%'");
                if (!string.IsNullOrEmpty(t.cinvBStd))
                    cmd.Append("and cinvBStd like '%" + t.cinvBStd + "%'");

            }
            
            r = Context.Sql(cmd.ToString()).QueryMany<vwStrategy>();

            return r;
        }

    }
}
