using System.ComponentModel.DataAnnotations;
using UserInfo;
using UserBLL;

namespace MvcApp.Models
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
        public bool logoned { get; set; } = false;
        public LogonModel() {
            //logoned = false;
        }

        public static UserModel getAuthentication(LogonModel logu)
        {
            UserModel logonuser = null;
            userBLL userBll = new userBLL();
            logonuser = userBll.Authentication(logu.ID, logu.PW);

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