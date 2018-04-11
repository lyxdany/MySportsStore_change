using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySportsStore.Model;
using MySportsStore.IBLL;

namespace MySportsStore.BLL
{
    public class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
       public UserInfoService() : base() { }

        public override bool SetCurrentRepository()
        {
            //若是其他XXXservice对应的就是DbSessionContext.xxxRepository
            this.CurrentRepository = DbSessionContext.UserInfoRepository;
            this.AddDisposableObject(this.CurrentRepository);
            return true;
        }
    }
}
