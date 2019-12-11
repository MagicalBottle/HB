using HB.Api.Models;
using HB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Api.Services
{

    public interface IAuthenticationService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录的账号</param>
        string SignIn(MebUser user);

        /// <summary>
        /// 登出
        /// </summary>
        void SignOut();

        /// <summary>
        /// 获取身份验证的账号
        /// </summary>
        /// <returns></returns>
        MebUser GetAuthenticatedUser();
    }
}
