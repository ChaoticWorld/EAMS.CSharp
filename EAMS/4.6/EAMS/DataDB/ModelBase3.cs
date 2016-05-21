using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DataDB.ModelBase
{
    #region 接口

    public partial interface IVouch
    {
        
        /// <summary>
        /// 主表
        /// </summary>
        Main Main { get; set; }
        /// <summary>
        /// 从表
        /// </summary>
        List<Detail> Details { get; set; }
    }

    /// <summary>
    /// 主表
    /// </summary>
    public partial interface IMain
    {
        /// <summary>
        /// 单据编码
        /// </summary>
        [Display(Name = "单据编码")]
         string Code { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日期")]
         DateTime dDate { get; set; }
        /// <summary>
        /// 对方编码
        /// </summary>
        [Display(Name = "对方编码")]
         string DWCode { get; set; }

        /// <summary>
        /// 单据发货地址
        /// </summary>
        [Display(Name = "地址")]
         string cShipAddress { get; set; }
        /// <summary>
        /// 送货人
        /// </summary>
        [Display(Name = "收送货人")]
         string cSender { get; set; }

        /// <summary>
        /// 送货人
        /// </summary>
        [Display(Name = "单据金额")]
         decimal Je { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
         string Memo { get; set; }

        [Display(Name = "制单人")]
         string Maker { get; set; }
        [Display(Name = "审核人")]
        string Verifier { get; set; }
        /// <summary>
        /// 内部编码
        /// </summary>
         string id { get; set; }
        /// <summary>
        /// 客户信息
        /// </summary>
         Customer customer { get; set; }

         object getMainField(string field, string code);
         void updateMainField(string field, string value, string code);
    }
    /// <summary>
    /// 子表
    /// </summary>
    public partial interface IDetail
    {    /// <summary>
        /// 内部编码
        /// </summary>
        string autoid { get; set; }
    
        /// <summary>
        /// 存货
        /// </summary>
         Inventory inventory { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
         Quantity quantity { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [Display(Name = "单价")]
         double iPrice { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "金额")]
         double iSum { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
         string Memo { get; set; }
        void getDetailField(string field, string code);
    }

    #endregion
    #region 进销单据基类模型
    /// <summary>
    /// 主表
    /// </summary>
    [Serializable]
    public partial class Main : IMain
    {
        /// <summary>
        /// 客户信息
        /// </summary>
        public Customer customer { get; set; }

        /// <summary>
        /// 单据编码
        /// </summary>
        [Display(Name = "单据编码")]
        public string Code { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日期")]
        public DateTime dDate { get; set; }
        /// <summary>
        /// 对方编码
        /// </summary>
        [Display(Name = "对方编码")]
        public string DWCode { get; set; }

        /// <summary>
        /// 单据发货地址
        /// </summary>
        [Display(Name = "地址")]
        public string cShipAddress { get; set; }
        /// <summary>
        /// 送货人
        /// </summary>
        [Display(Name = "收送货人")]
        public string cSender { get; set; }

        /// <summary>
        /// 送货人
        /// </summary>
        [Display(Name = "单据金额")]
        public decimal Je { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Memo { get; set; }

        [Display(Name = "制单人")]
        public string Maker { get; set; }
        [Display(Name = "审核人")]
        public string Verifier { get; set; }
        public string id { get; set; }
        public virtual object getMainField(string field,string code) { return new object();}

        public virtual void updateMainField(string field, string value, string code){;}
    }
    /// <summary>
    /// 子表
    /// </summary>
    [Serializable]
    public partial class Detail :IDetail
    {    /// <summary>
        /// 内部编码
        /// </summary>
        public string autoid { get; set; }
    
        /// <summary>
        /// 存货
        /// </summary>
        public Inventory inventory { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Quantity quantity { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [Display(Name = "单价")]
        public double iPrice { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "金额")]
        public double iSum { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Memo { get; set; }

        public virtual void getDetailField(string field, string code) { }
    }
    #endregion 单据基类模型

    #region 发货单

    /// <summary>
    /// 发货单
    /// </summary>
    [Serializable]
    public partial class Dispatch :IVouch
    {
        /// <summary>
        /// 主表
        /// </summary>
        public Main Main { get; set; }
        /// <summary>
        /// 从表
        /// </summary>
        public List<Detail> Details { get; set; }
    }
    /// <summary>
    /// 发货单主表
    /// </summary>
    [Serializable]
    public partial class DispatchMain : Main
    {
        public override object getMainField(string field, string code)
        {
            u8.dbU8 dbu8 = new u8.dbU8();
            return dbu8.getDispatchMainField(field, code);
        }
        public override void updateMainField(string field, string value, string code)
        {
            u8.dbU8 dbu8 = new u8.dbU8();
            dbu8.updateDispatchField(field, value, code);
        }
    }
    /// <summary>
    /// 发货单子表
    /// </summary>
    [Serializable]
    public partial class DispatchDetail : Detail
    {  }
    #endregion 发货单


    #region 入库单
    /// <summary>
    /// 入库单
    /// </summary>
    [Serializable]
    public partial class InWarehouse
    {
        /// <summary>
        /// 主表
        /// </summary>
        public InWarehouseMain Main { get; set; }
        /// <summary>
        /// 从表
        /// </summary>
        public IList<InWarehouseDetail> Details { get; set; }
    }
    /// <summary>
    /// 入库单主表
    /// </summary>
    [Serializable]
    public partial class InWarehouseMain : Main
    {
        /// <summary>
        /// 供应商信息
        /// </summary>
        public Vendor vendor { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        [Display(Name = "收货人")]
        public string cReceiver { get; set; }
    }
    /// <summary>
    /// 入库单子表
    /// </summary>
    [Serializable]
    public partial class InWarehouseDetail : Detail
    { }
    #endregion 入库单

    #region 销售订单
    /// <summary>
    /// 销售订单
    /// </summary>
    [Serializable]
    public partial class SaleOrder : IVouch
    {
        /// <summary>
        /// 主表
        /// </summary>
        public Main Main { get; set; }
        /// <summary>
        /// 明细表
        /// </summary>
        public List<Detail> Details { get; set; }
    }
    /// <summary>
    /// 销售订单主表
    /// </summary>
    [Serializable]
    public partial class SaleOrderMain:Main
    { 
        public override object getMainField(string field, string code)
        {
            DataDB.u8.dbU8 u8 = new u8.dbU8();
            return u8.getSaleOrderMainField(field, code);
        }
        public override void updateMainField(string field, string value, string code)
        {
            u8.dbU8 dbu8 = new u8.dbU8();
            dbu8.updateSaleOrderField(field, value, code);
        }
    }
    /// <summary>
    /// 销售订单明细表
    /// </summary>
    [Serializable]
    public partial class SaleOrderDetail:Detail
    {    }
#endregion

#region 单据类型
    interface IVouchType
    {
        int id { get; set; }
        string vouchName { get; set; }
        string Key { get; set; }
        string vouchClass {get;set;}
        int vouchOrder { get; set; }
        string vouchType { get; set; }

        VouchType Clone();
    }
    public class VouchType : DAL, IVouchType ,DBAccessBase.IdbCRUD<VouchType>
    {
        string sqlBase = "select id,vouchClass,vouchType,vouchName,vouchOrder,[KEY] from vouchType where 1 = 1 ";
        public int id { get; set; }
        public string vouchName { get; set; }
        public string Key { get; set; }
        public string vouchClass { get; set; }
        public int vouchOrder { get; set; }
        public string vouchType { get; set; }

        public VouchType Clone() {
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
        public static List<VouchType> Init() {
            List<VouchType> list = new List<VouchType>();
            string sql = "select id,vouchClass,vouchType,vouchName,vouchOrder,[KEY] "
                + " from vouchType where 1 = 1 ";
            list = Context.Sql(sql).QueryMany<VouchType>();
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
        public VouchType Retrieve(int id) {
            VouchType r = new VouchType();
            string sql = sqlBase + " and id = "+id.ToString();
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
        public int Update(VouchType t) {
            int r = -1;
            r = Context.Update<VouchType>("VouchType", t)
                        .AutoMap(a => a.id)
                        .Where(w => w.id)
                        .Execute();
            return r;
        }
        public int Delete(long id)
        {
            int r = -1;
            r = Context.Delete("VouchType")
                        .Where("id",id)
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
                sql += " and [Key] = '"+t.Key+"'";
            if (!string.IsNullOrEmpty(t.vouchClass))
                sql += " and vouchClass = '"+t.vouchClass+"'";
            if (!string.IsNullOrEmpty(t.vouchName))
                sql += " and vouchName = '"+t.vouchName+"'";
            if (!string.IsNullOrEmpty(t.vouchType))
                sql += " and vouchType = '"+t.vouchType+"'";
            if (t.vouchOrder > 0)
                sql += " and vouchOrder = " + t.vouchOrder.ToString();
            r = Context.Sql(sqlBase + sql).QueryMany<VouchType>();
            return r; 
        } 
    }
#endregion
}
