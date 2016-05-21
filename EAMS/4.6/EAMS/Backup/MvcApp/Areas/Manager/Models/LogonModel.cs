using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SystemDB;
using WebCommon;

namespace MvcApp.Areas.Manager.Models
{
    public class LogonModel
    {
        [Required]
        public string ID { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PW { get; set; }
        [Display(Name = "记住账号")]
        public bool RememberMe { get; set; }
        public LogonModel() { }

        public static User getUser(LogonModel logu)
        {
            User logonuser = new SystemDB.User();
            logonuser.cUserPassWord = logu.PW;
            if (RegExp.IsEmail(logu.ID)) //EMail登录
                logonuser.cUserEMail = logu.ID;
            else if (RegExp.IsMobile(logu.ID))//Mobile登录
                logonuser.cUserMobile = logu.ID;
            else logonuser.cUserCode = logu.ID;

            return logonuser;
        }
    }
    public class RegisterModel
    {
        [Required]
        [Display(Name="登录ID")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "显示姓名")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string pw { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "电子邮箱")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "移动电话")]
        public string Mobile { get; set; }
    }
    
    public class ReSecurity
    {
        [Required]
        public int id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "旧密码")]
        public string oldpw { get; set; }
     
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string pw { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        public string ConfirmPassword { get; set; }
    }
}