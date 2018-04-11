using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySportsStore.Model;

namespace MySportsStore.WebUI.Models
{
    public class UserInfoViewModel
    {
        public IEnumerable<UserInfo> UserInfo { get; set; }
    }
}