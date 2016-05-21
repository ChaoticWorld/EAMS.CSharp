using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;
using FluentData;
using DBAccessBase;
using DataAccess;

namespace strategyLib
{

    public class DAL : abstractDataAccess
    {
        public DAL()
        {
            Context = new DbContext().ConnectionStringName("eamsAppDataConn", new SqlServerProvider());
        }
    }
    public class mainDAL :  DAL, IdbCRUD<Main>
    {
        public mainDAL()
        { TableName = "strategyMain"; }
        public long Create(Main m)
        {
            long id;
            try
            {
                m.cSourceType = string.IsNullOrEmpty(m.cSourceType) ? "期初" : m.cSourceType;
                m.dEffDate = (m.dEffDate >= SqlDateTime.MaxValue.Value || m.dEffDate < SqlDateTime.MinValue.Value) ?
                  SqlDateTime.MinValue.Value : m.dEffDate;

                m.dExpDate = (m.dExpDate >= SqlDateTime.MaxValue.Value
                    || m.dExpDate < SqlDateTime.MinValue.Value) ?
                    SqlDateTime.MaxValue.Value : m.dExpDate;

                id = Context.Insert<Main>(TableName, m)
                    .Column("cDCName",m.cDCName)
                    .Column("cDWCode", m.cDWCode)
                    .Column("cDWName", m.cDWName)
                    .Column("cDWContact", m.cDWContact)
                    .Column("cLevel", m.cLevel)
                    .Column("cMemo", m.cMemo)
                    .Column("cSourceCode", m.cSourceCode)
                    .Column("cSourceType", m.cSourceType)
                    .Column("cVouchCode", m.cVouchCode)
                    .Column("cVouchType", m.cVouchType)
                    .Column("dEffDate", m.dEffDate)
                    .Column("dExpDate", m.dExpDate)
                    .Column("cModifier",m.cModifier)
                    .ExecuteReturnLastId<long>();
            }
            catch (Exception e)
            {
                id = -1;
                //throw e;
            }
            return id;
        }
        public int Update(Main m)
        {
            m.cSourceType = string.IsNullOrEmpty(m.cSourceType) ? "期初" : m.cSourceType;
            m.dEffDate = (m.dEffDate >= SqlDateTime.MaxValue.Value || m.dEffDate < SqlDateTime.MinValue.Value) ?
               SqlDateTime.MinValue.Value : m.dEffDate;
            m.dExpDate = (m.dExpDate >= SqlDateTime.MaxValue.Value || m.dExpDate < SqlDateTime.MinValue.Value) ?
               SqlDateTime.MaxValue.Value : m.dExpDate;
            int rowsAffected = Context.Update<Main>(TableName, m)
                    .Column("cDCName", m.cDCName)
                    .Column("cDWCode", m.cDWCode)
                    .Column("cDWName", m.cDWName)
                    .Column("cDWContact", m.cDWContact)
                    .Column("cLevel", m.cLevel)
                    .Column("cMemo", m.cMemo)
                    .Column("cSourceCode", m.cSourceCode)
                    .Column("cSourceType", m.cSourceType)
                    .Column("cVouchCode", m.cVouchCode)
                    .Column("cVouchType", m.cVouchType)
                    .Column("dEffDate", m.dEffDate)
                    .Column("dExpDate", m.dExpDate)
                    .Column("cModifier", m.cModifier)
                    .Where(x => x.ID)
                    .Execute();
            return rowsAffected;
        }
        public bool Exists(Main m)
        {
            bool r = false;
            var single = Context.Sql(@"select id from strategyMain where ID = '" + m.ID + "'")
                .QuerySingle<long>();
            r = single > 0;
            return r;
        }
        public Main Retrieve(int id)
        {
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
        public int Delete(long id)
        {
            int rowsAffected = Context.Delete(TableName).Where("ID", id)
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
                    if (t.dEffDate != null && t.dEffDate > SqlDateTime.MinValue.Value)
                        cmd.Append(" and dEffDate <= '" + t.dEffDate.ToString("yyyy-MM-dd") + "'");
                    if (t.dExpDate != null && t.dExpDate < SqlDateTime.MaxValue.Value)
                        cmd.Append(" and dExpDate <= '" + t.dExpDate.ToString("yyyy-MM-dd") + "'");
                    if (!string.IsNullOrEmpty(t.cVouchType))
                        cmd.Append(" and cVouchType like '%" + t.cVouchType + "%'");
                    if (!string.IsNullOrEmpty(t.cVouchCode))
                        cmd.Append(" and cVouchCode like '%" + t.cVouchCode + "%'");
                    if (!string.IsNullOrEmpty(t.cSourceType))
                        cmd.Append(" and cSourceType like '%" + t.cSourceType + "%'");
                    if (!string.IsNullOrEmpty(t.cSourceCode))
                        cmd.Append(" and cSourceCode like '%" + t.cSourceCode + "%'");
                    if (!string.IsNullOrEmpty(t.cLevel))
                        cmd.Append(" and cLevel like '%" + t.cLevel + "%'");
                    if (!string.IsNullOrEmpty(t.cDWName))
                        cmd.Append(" and cDWName like '%" + t.cDWName + "%'");
                    if (!string.IsNullOrEmpty(t.cDWCode))
                        cmd.Append(" and cDWCode like '%" + t.cDWCode + "%'");
                    if (!string.IsNullOrEmpty(t.cDWContact))
                        cmd.Append(" and cDWContact like '%" + t.cDWContact + "%'");
                    if (!string.IsNullOrEmpty(t.cDCName))
                        cmd.Append(" and cDCName like '%" + t.cDCName + "%'");
                    if (!string.IsNullOrEmpty(t.cModifier))
                        cmd.Append(" and cModifier like '%" + t.cModifier + "%'");
                    mains = Context.Sql(cmd.ToString()).QueryMany<Main>();
                }
                else mains.Add(Retrieve((int)t.ID));
            }
            else mains = Context.Sql(cmd.ToString()).QueryMany<Main>();
            return mains;
        }
    }
    public class detailDAL : DAL, IdbCRUD<Detail>
    {
        public detailDAL() { TableName = "strategyDetail"; }
        public long Create(Detail d)
        {
            long id;
            id = Context.Insert<Detail>(TableName, d)
                .Column("cinvACode",d.cinvACode)
                .Column("cinvAName", d.cinvAName)
                .Column("cinvAStd", d.cinvAStd)
                .Column("invAPrice", d.invAPrice)
                .Column("invAQuantity", d.invAQuantity)
                .Column("invARate", d.invARate)
                .Column("cinvBCode", d.cinvBCode)
                .Column("cinvBName", d.cinvBName)
                .Column("cinvBStd", d.cinvBStd)
                .Column("invBPrice", d.invBPrice)
                .Column("invBQuantity", d.invBQuantity)
                .Column("invBRate", d.invBRate)
                .Column("ID",d.ID)
                .ExecuteReturnLastId<long>();
            return id;
        }
        public int Update(Detail d)
        {
            int rowsAffected = Context.Update<Detail>(TableName, d)
                .Column("cinvACode", d.cinvACode)
                .Column("cinvAName", d.cinvAName)
                .Column("cinvAStd", d.cinvAStd)
                .Column("invAPrice", d.invAPrice)
                .Column("invAQuantity", d.invAQuantity)
                .Column("invARate", d.invARate)
                .Column("cinvBCode", d.cinvBCode)
                .Column("cinvBName", d.cinvBName)
                .Column("cinvBStd", d.cinvBStd)
                .Column("invBPrice", d.invBPrice)
                .Column("invBQuantity", d.invBQuantity)
                .Column("invBRate", d.invBRate)
                .Column("ID", d.ID)
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
        public int Delete(long id)
        {
            int rowsAffected = Context.Delete(TableName).Where("autoid", id)
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

    public class strategyDAL : DAL
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

        //public string isValid(DataModel.IVouch vouch)
        //{
        //    string r = string.Empty;
        //    DataModel.VouchMain vouchMain = vouch.Main;
        //    List<DataModel.VouchDetail> vouchDetails = vouch.Details.ToList();

        //    string cmdBase = "select * from vw_strategy";
        //    StringBuilder cmdwhere = new StringBuilder();
        //    foreach (DataModel.VouchDetail vd in vouchDetails)
        //    {
        //        cmdwhere = new StringBuilder();
        //        cmdwhere.Append(" where 1 = 1 and cinvAName like '%" + vd.inventory.InvName + "%'");
        //        cmdwhere.Append(" and ( dExpDate is null or dEffDate is null or ");
        //        cmdwhere.Append(" ( dExpdate >= '" + vouchMain.vouchDate + "' and dEffDate <= '" + vouchMain.vouchDate + "') )");
        //        cmdwhere.Append(" and (cDWName is null or cDWName like '%" + vouchMain.corporatio.Name + "%')");
        //        cmdwhere.Append(" and (cDCName is null or cDCName like '%" + vouchMain.corporatio.district.dcName + "%')");
        //        cmdwhere.Append(" and (cDWContact is null or cDWContact like '%" + vouchMain.vouchContact + "%')");
        //        cmdwhere.Append(" ORDER BY cLevel desc ");
        //        string tsql = cmdBase + cmdwhere.ToString();
        //        List<vwStrategy> sqlResult1 = Context.Sql(cmdBase + cmdwhere.ToString()).QueryMany<vwStrategy>();
        //        var sqlResult2 = sqlResult1.Where(w => w.cinvAStd.Contains(vd.inventory.InvStd));
        //        foreach (vwStrategy vs in sqlResult2)
        //            if (vd.iPrice < (double)vs.invAPrice)
        //            {
        //                r = "存货:" + vd.inventory.InvName + "-" + vd.inventory.InvStd + "\n低于策略:"
        //                  + vs.cSourceType + "-" + vs.cSourceCode + ":" + vs.invAPrice.ToString();
        //                return r;
        //            }
        //    }
        //    return r;
        //}

        public IstrategyMain getStrategyMain(int ID)
        {
            return mDal.Retrieve(ID);
        }
        public string isValid(int ID)
        {
            string r = string.Empty;
            IstrategyMain main = getStrategyMain(ID);
            DataAccess.IERP erp = ERPFactory.ERPFactory.Create(erpName: "U8");
            erp.stor = new DB();
            erp.stor.MultiTableQuery = ERPFactory.ERPFactory.createMultiTableQuery();
            List<string> mainCodes = new List<string>();
            if (!string.IsNullOrEmpty(main.cVouchCode))
            {
                r += validErpVouch(main.cVouchType, main.cVouchCode);
            }
            else
            {
                mainCodes = erp.stor.MultiTableQuery.getDispatchMainCodesWithScope(main.dEffDate, main.dExpDate, main.cDCName, main.cDWCode);                
                foreach (string code in mainCodes)
                    r += validErpVouch("Dispatch", code);
                
                mainCodes = erp.stor.MultiTableQuery.getSODetailMainCodesWithScope(main.dEffDate, main.dExpDate, main.cDCName, main.cDWCode);
                foreach (string code in mainCodes)
                    r += validErpVouch("SaleOrder", code);

            }
            return r;
        }
        private string validErpVouch(string vouchType, string vouchCode)
        {
            DataAccess.IERP erp = ERPFactory.ERPFactory.Create(erpName: "U8");
            erp.stor = new DB();
            erp.stor.Dispatch = ERPFactory.ERPFactory.createDispatch();
            erp.stor.SaleOrder = ERPFactory.ERPFactory.createSaleOrder();
            DataModel.Vouch vouch;
            string spStr = string.Empty;
            string valid = string.Empty;
            switch (vouchType)
            {
                case "Dispatch":
                    vouch = erp.stor.Dispatch.getSingle(vouchCode);
                    spStr = "saleFHD_validStrategy";
                    break;
                case "SaleOrder":
                    vouch = erp.stor.SaleOrder.getSingle(vouchCode);
                    spStr = "saleOrder_validStrategy";
                    break;
                default:
                    vouch = null;
                    break;
            }
            if (vouch != null)
            {
                valid = string.Empty;
                foreach (DataModel.VouchDetail d in vouch.Details)
                {
                    var sp =
                        erp.stor.Context.StoredProcedure(spStr)
                           .Parameter("id", vouch.Main.Mid)
                           .Parameter("invcode", d.inventory.InvCode)
                           .Parameter("iquantity", d.quantity.iQuantity)
                           .Parameter("iTaxUnitPrice", d.iPrice)
                           .ParameterOut("valid", DataTypes.String, 512);
                    try
                    {
                        sp.Execute();
                        valid += sp.ParameterValue<string>("valid") + ";\n";
                    }
                    catch (Exception e)
                    {
                        var em = e.Message;
                    }
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
            cmd.Append("select * from AppData.dbo.vw_strategy where 1 = 1 ");

            if (t != null)
            {
                if (t.dEffDate != null && t.dEffDate > SqlDateTime.MinValue.Value)
                    cmd.Append(" and dEffDate <= '" + t.dEffDate.ToString("yyyy-MM-dd") + "'");
                if (t.dExpDate != null && t.dExpDate < SqlDateTime.MaxValue.Value)
                    cmd.Append(" and dExpDate <= '" + t.dExpDate.ToString("yyyy-MM-dd") + "'");
                if (!string.IsNullOrEmpty(t.cVouchType))
                    cmd.Append(" and cVouchType like '%" + t.cVouchType + "%'");
                if (!string.IsNullOrEmpty(t.cVouchCode))
                    cmd.Append(" and cVouchCode like '%" + t.cVouchCode + "%'");
                if (!string.IsNullOrEmpty(t.cSourceType))
                    cmd.Append(" and cSourceType like '%" + t.cSourceType + "%'");
                if (!string.IsNullOrEmpty(t.cSourceCode))
                    cmd.Append(" and cSourceCode like '%" + t.cSourceCode + "%'");
                if (!string.IsNullOrEmpty(t.cLevel))
                    cmd.Append(" and cLevel like '%" + t.cLevel + "%'");
                if (!string.IsNullOrEmpty(t.cDWName))
                    cmd.Append(" and cDWName like '%" + t.cDWName + "%'");
                if (!string.IsNullOrEmpty(t.cDWCode))
                    cmd.Append(" and cDWCode like '%" + t.cDWCode + "%'");
                if (!string.IsNullOrEmpty(t.cDWContact))
                    cmd.Append(" and cDWContact like '%" + t.cDWContact + "%'");
                if (!string.IsNullOrEmpty(t.cDCName))
                    cmd.Append(" and cDCName like '%" + t.cDCName + "%'");

                if (t.ID > 0)
                    cmd.Append(" and ID = '" + t.ID + "'");
                if (!string.IsNullOrEmpty(t.cinvACode))
                    cmd.Append(" and cinvACode like '%" + t.cinvACode + "%'");
                if (!string.IsNullOrEmpty(t.cinvAName))
                    cmd.Append(" and cinvAName like '%" + t.cinvAName + "%'");
                if (!string.IsNullOrEmpty(t.cinvAStd))
                    cmd.Append(" and cinvAStd like '%" + t.cinvAStd + "%'");
                if (!string.IsNullOrEmpty(t.cinvBCode))
                    cmd.Append(" and cinvBCode like '%" + t.cinvBCode + "%'");
                if (!string.IsNullOrEmpty(t.cinvBName))
                    cmd.Append(" and cinvBName like '%" + t.cinvBName + "%'");
                if (!string.IsNullOrEmpty(t.cinvBStd))
                    cmd.Append(" and cinvBStd like '%" + t.cinvBStd + "%'");

            }

            r = Context.Sql(cmd.ToString()).QueryMany<vwStrategy>();

            return r;
        }

    }
}
