using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DataDB.ModelBase
{
    //客商员工基类模型
    
    /// <summary>
    /// 联系人
    /// </summary>
    [Serializable]
    public partial class Contact// : IContact
    {
        /// <summary>
        /// 编码
        /// </summary>
        [Display(Name = "编码")]
        public string code { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string name { get; set; }
        /// <summary>
        /// 所属单位编码
        /// </summary>
        [Display(Name="所属单位编码")]
        public string DWCode { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        [Display(Name = "固定电话")]
        public string phone { get; set; }
        /// <summary>
        /// 办公电话
        /// </summary>
        [Display(Name = "办公电话")]
        public string officePhone { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        [Display(Name = "移动电话")]
        public string mobile { get; set; }
        /// <summary>
        /// 即时通讯
        /// </summary>
        [Display(Name = "即时通讯")]        
        public IList<IIM> IMs { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        [UIHint("EMailAddress")]
        [Display(Name = "电子邮箱")]
        public string EMail { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name="备注")]
        public string Memo { get; set; }
    }
    /// <summary>
    /// 即时通讯
    /// </summary>
    [Serializable]
    public partial class IM : IIM
    {
        /// <summary>
        /// 即时通讯软件名称
        /// </summary>
        [Display(Name = "软件名称")]
        public string name { get; set; }
        /// <summary>
        /// 即时通讯软件帐号
        /// </summary>
        [Display(Name = "IM号码")]
        public string code { get; set; }
    }
    /// <summary>
    /// 员工
    /// </summary>
    [Serializable]
    public partial class Employee : IEmployee
    {
        /// <summary>
        /// 编码
        /// </summary>
        [Display(Name = "编码")]
        public string EmpId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string name { get; set; }
    }
    /// <summary>
    /// 客户
    /// </summary>
    [Serializable]
    public partial class Customer : ICustomer, IOperatorBase
    {
        /// <summary>
        /// 编码
        /// </summary>
        [Display(Name = "客户编码")]
        public string cCusCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "客户名称")]
        public string cCusName { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        [Display(Name = "客户简称")]
        public string cCusAbbName { get; set; }
        /// <summary>
        /// 上级单位
        /// </summary>
        [Display(Name = "上级单位")]
        public ICustomer topCus { get; set; }
        /// <summary>
        /// 上级单位编码
        /// </summary>
        [Display(Name = "上级单位编码")]
        public string topCusCode { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        [Display(Name = "地区")]
        public string District { get; set; }
        /// <summary>
        /// 单位地址
        /// </summary>
        [UIHint("MultilineText")]
        [Display(Name = "公司地址")]
        public string cAddress { get; set; }
        /// <summary>
        /// 送货地址
        /// </summary>
        [UIHint("MultilineText")]
        [Display(Name = "送货地址")]
        public string cShipAddress { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        [Display(Name = "固定电话")]
        public string cPhone { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        [Display(Name = "移动电话")]
        public string cMobile { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        [UIHint("EMailAddress")]
        [Display(Name = "电子邮箱")]
        public string EMail { get; set; }
        /// <summary>
        /// 即时通讯
        /// </summary>
        [Display(Name = "即时通讯")]
        public IList<IIM> IMs { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [Display(Name = "联系人列表")]
        public IList<IContact> Contacts { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [Display(Name = "联系人姓名")]
        public string ContactName { get; set; }
        /// <summary>
        /// 客户经理
        /// </summary>
        [Display(Name = "客户经理")]
        public IList<IEmployee> Employees { get; set; }
        /// <summary>
        /// 客户经理姓名
        /// </summary>
        [Display(Name = "客户经理姓名")]
        public string employeeName { get; set; }
        /// <summary>
        /// 开发日期
        /// </summary>
        [Display(Name = "开发日期")]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        [Display(Name = "分类")]
        public string cusClass { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Memo { get; set; }

        public string CreateMan
        { get; set; }

        public string ModifyMan
        { get; set; }

        public DateTime ModifyDate
        { get; set; }
    }
    /// <summary>
    /// 供货商
    /// </summary>
    [Serializable]
    public partial class Vendor
    {
        /// <summary>
        /// 编码
        /// </summary>
        [Display(Name = "卖方编码")]
        public string cVenCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "卖方名称")]
        public string cVenName { get; set; }
        /// <summary>
        /// 上级单位
        /// </summary>
        [Display(Name = "上级单位")]
        public Customer topVen { get; set; }
        /// <summary>
        /// 上级单位编码
        /// </summary>
        [Display(Name = "上级单位编码")]
        public string topVenCode { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        [Display(Name = "地区")]
        public string District { get; set; }
        /// <summary>
        /// 公司地址
        /// </summary>
        [UIHint("MultilineText")]
        [Display(Name = "公司地址")]
        public string cAddress { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        [Display(Name = "固定电话")]
        public string cPhone { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        [Display(Name = "移动电话")]
        public string cMobile { get; set; }
        /// <summary>
        /// 即时通讯
        /// </summary>
        [Display(Name = "即时通讯")]
        public IList<IM> IMs { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [Display(Name = "联系人列表")]
        public IList<Contact> Contacts { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [Display(Name = "联系人姓名")]
        public string ContactName { get; set; }
        /// <summary>
        /// 客户经理
        /// </summary>
        [Display(Name = "客户经理")]
        public IList<Employee> Employees { get; set; }
        /// <summary>
        /// 客户经理姓名
        /// </summary>
        [Display(Name = "客户经理姓名")]
        public string employeeName { get; set; }
        /// <summary>
        /// 开发日期
        /// </summary>
        [Display(Name = "开发日期")]
        public DateTime devDate { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        [Display(Name = "分类")]
        public string venClass { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        [UIHint("EMailAddress")]
        [Display(Name = "电子邮箱")]
        public string EMail { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Memo { get; set; }
    }
}
