using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;//约束


namespace MySportsStore.WebUI.Models
{
    public class RegisterViewModel
    {

        [Required]
        [Display(Name = "姓名")]
        public string UserName { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "邮箱")]
        [RegularExpression(@"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", ErrorMessage = "请输入正确的Email格式\n示例：abc@123.com")]
        public string UserEmail { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码必须为{2}到{1}长度")]
        [RegularExpression(@"^\w+$", ErrorMessage = "密码格式有误,只能是字母数字或者下划线")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string UserPwd { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("UserPwd", ErrorMessage = "密码与确认密码不一致.")]
        public string ConfirmPassword { get; set; }

        //[Display(Name = "记住登陆?")]
        //public bool RememberMe { get; set; }

        
    }
}