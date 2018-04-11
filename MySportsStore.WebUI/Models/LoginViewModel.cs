using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;//这是限定属性类型，约束属性的输入
using MySportsStore.IBLL;


namespace MySportsStore.WebUI.Models
{
    public class LoginViewModel
    {
        

        [Display(Name = "用户名")]
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "密码不能为空")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^\w+$", ErrorMessage = "密码格式有误,只能是字母数字或者下划线")]
        public string Password { get; set; }


        [Display(Name = "记住登陆?")]
        public bool RememberMe { get; set; }

        
    }
}