using HB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Api.Services
{
   public interface IMebUserService : IBaseService<MebUser>
    {

        /// <summary>
        /// 根据用户名账号信息
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <returns></returns>
        MebUser GetUserByUserName(string userName);
    }
}
