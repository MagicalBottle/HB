using HB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Api.Services
{
    public interface ISysApiUserService :IBaseService<SysApiUser>
    {
        /// <summary>
        /// 根据Appid获取接口用户
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        SysApiUser GetUserByAppId(int appId);
    }
}
