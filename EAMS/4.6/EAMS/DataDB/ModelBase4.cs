using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DataDB.ModelBase
{
    #region 款项单据基类模型
    /// <summary>
    /// 主表
    /// </summary>
    [Serializable]
    public partial class AccountMain:Main
    {
        /// <summary>
        /// 单据金额
        /// </summary>
        [Display(Name="单据金额")]
        public double Money { get; set; }
        /// <summary>
        /// 借贷方向
        /// </summary>
        [UIHint("Boolean")]
        [Display(Name = "借贷")]
        public bool isDC { get; set; }
    }
    /// <summary>
    /// 子表
    /// </summary>
    [Serializable]
    public partial class AccountDetail
    {
        /// <summary>
        /// 业务单据对应收款金额
        /// </summary>
        [Display(Name = "业务单据对应收款金额")]
        public double Money { get; set; }
        /// <summary>
        /// 业务单据编码
        /// </summary>
        [Display(Name = "业务单据编码")]
        public string cBizVouchId { get; set; }
        /// <summary>
        /// 业务单据金额
        /// </summary>
        [Display(Name = "业务单据金额")]
        public double iBizMoney { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Memo { get; set; }
    }
    #endregion 单据基类模型

    #region 应收付款
    /// <summary>
    /// 应收付款
    /// </summary>
    [Serializable]
    public partial class Account
    {
        /// <summary>
        /// 主表
        /// </summary>
        public AccountMain Main { get; set; }
        /// <summary>
        /// 子表
        /// </summary>
        public IList<AccountDetail> Details { get; set; }
    }
    #endregion 应收付款
}