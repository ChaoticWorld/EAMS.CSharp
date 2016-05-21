using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DataDB.ModelBase
{
    //存货基类模型
    
    /// <summary>
    /// 存货分类
    /// </summary>
    [Serializable]
    public partial class InventoryClass
    {
        /// <summary>
        /// 编码
        /// </summary>
        [Display(Name = "编码")]
        public string code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "姓名")]
        public string name { get; set; }
        /// <summary>
        /// 上级分类编码
        /// </summary>
        [Display(Name = "上级分类编码")]
        public string upClass { get; set; }
        /// <summary>
        /// 子分类列表
        /// </summary>
        [Display(Name = "子分类列表")]
        public List<InventoryClass> subClass { get; set; }
        /// <summary>
        /// 子分类编码列表
        /// </summary>
        [Display(Name = "子分类编码列表")]
        public List<string> subClsCodes { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name="备注")]
        public string Memo { get; set; }
    }
    /// <summary>
    /// 存货
    /// </summary>
    [Serializable]
    public partial class Inventory
    {
        /// <summary>
        /// 编码
        /// </summary>
        [Display(Name = "存货编码")]
        public string cInvCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "存货名称")]
        public string cInvName { get; set; }
        /// <summary>
        /// 存货分类
        /// </summary>
        [Display(Name = "存货分类")]
        public string cInvClass { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [Display(Name="规格型号")]
        public string cInvStd { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [Display(Name = "单位")]
        public string cUnit { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Memo { get; set; }

        public InventoryClass invClass { get; set; }
    }
    /// <summary>
    /// 数量
    /// </summary>
    public partial class Quantity 
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Display(Name = "仓库名称")]
        public string cWhName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [UIHint("Decimal")]
        [Display(Name = "数量")]
        public double iQuantity { get; set; }
        /// <summary>
        /// 件数
        /// </summary>
        [UIHint("Decimal")]
        [Display(Name = "件数")]
        public double iNum { get; set; }
    }

    /// <summary>
    /// 现存数量
    /// </summary>
    public partial class CurrentStockQ {
        /// <summary>
        /// 存货
        /// </summary>
        public Inventory inventory { get; set; }
        /// <summary>
        /// 仓库数量列表
        /// </summary>
        public IList<Quantity> Quantitys { get; set; }
    }
}
