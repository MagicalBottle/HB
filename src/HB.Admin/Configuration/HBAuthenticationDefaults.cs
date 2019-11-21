using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Admin.Configuration
{

    public static class HBAuthenticationDefaults
    {
        /// <summary>
        /// 后台管理员验证
        /// </summary>
        public static string AdminAuthenticationScheme => "AdminAuthentication";

        /// <summary>
        /// 前台用户验证
        /// </summary>
        public static string CustomerAuthenticationScheme => "CustomerAuthentication";

        /// <summary>
        /// 证书颁发者
        /// </summary>
        public static string ClaimsIssuer => "HB";

        /// <summary>
        /// 后台登录页面
        /// </summary>
        public static PathString LoginPath => new PathString("/Home/Login");

        /// <summary>
        ///后台没有权限跳转的页面
        /// </summary>
        public static PathString AccessDeniedPath => new PathString("/Home/Forbidden");

        /// <summary>
        /// 前台登录页面
        /// </summary>
        public static PathString SigninPath => new PathString("/Signin");

    }
}
