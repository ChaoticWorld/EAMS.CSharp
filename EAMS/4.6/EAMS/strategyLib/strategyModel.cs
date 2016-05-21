using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;
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
            dExpDate = SqlDateTime.MaxValue.Value;
            dEffDate = SqlDateTime.MinValue.Value;
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
        public string cModifier { get; set; }
        public string cSourceCode { get; set; }
        public string cSourceType { get; set; }
        public string cMemo { get; set; }
        public string cLevel { get; set; }
        public List<Detail> Details { get { return getDetails(); } }
        private List<Detail> getDetails()
        {
            List<Detail> r = new List<Detail>();
            detailDAL dDal = new detailDAL();
            r = dDal.getList(new Detail() { ID = ID });
            return r;
        }
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
            m.cModifier = this.cModifier;
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
        public vwStrategy()
        {
            dExpDate = SqlDateTime.MaxValue.Value;
            dEffDate = SqlDateTime.MinValue.Value;
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
}
