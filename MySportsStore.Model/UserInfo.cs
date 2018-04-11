
using System.ComponentModel.DataAnnotations;//这是限定属性类型，约束属性的输入
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySportsStore.Model
{
    public class UserInfo
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage="用户名不能为空")]
        [Display(Name="用户名")]
        [StringLength(20,MinimumLength=2,ErrorMessage="用户名必须为{2}到{1}个字符")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "用户类型不能为空")]
        public string UserType { get; set; }

        [Required(ErrorMessage="密码不能为空")]
        [Display(Name="密码")]
        public string UserPwd { get; set; }

        public string Salt { get; set; }

        [Display(Name="邮箱")]
        [Required(ErrorMessage="邮箱必填")]
        [RegularExpression(@"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$",ErrorMessage="请输入正确的Email格式\n示例：abc@123.com")]
        public string UserEmail { get; set; }


        public int UserRank { get; set; }

        public DateTime RegisterTime { get; set; }



    }
}
