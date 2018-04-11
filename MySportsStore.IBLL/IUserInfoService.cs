using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySportsStore.Model;

namespace MySportsStore.IBLL
{
    /// <summary>
    /// 同理，添加新的类<==>表的时候，可以简单实现扩展
    /// </summary>
    public interface IUserInfoService : IBaseService<UserInfo>
    {

    }
}
