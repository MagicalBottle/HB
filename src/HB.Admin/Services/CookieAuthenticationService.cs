using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HB.Admin.Configuration;
using Microsoft.AspNetCore.Authentication;
using HB.Models;

namespace HB.Admin.Services
{

    public class CookieAuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISysAdminService _adminService;
        private readonly ICacheManagerService _cache;
        private readonly IWorkContextService _workContext;

        public CookieAuthenticationService(IHttpContextAccessor httpContextAccessor,
            ISysAdminService adminService,
            ICacheManagerService cache,
            IWorkContextService workContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _adminService = adminService;
            _cache = cache;
            _workContext = workContext;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="admin">登录的账号</param>
        /// <param name="isPersistent">登录信息持久化到客户端，true 是，false 否</param>
        public async void SignIn(SysAdmin admin, bool isPersistent)
        {

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, admin.UserName, ClaimValueTypes.String, HBAuthenticationDefaults.ClaimsIssuer));
            ClaimsIdentity identity = new ClaimsIdentity(claims, HBAuthenticationDefaults.AdminAuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            AuthenticationProperties properties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc =new DateTimeOffset(DateTime.Now.AddMinutes(HBCachingDefaults.CacheTime)) //最大值 。默认值14天
            };
            await _httpContextAccessor.HttpContext.SignInAsync(HBAuthenticationDefaults.AdminAuthenticationScheme, principal, properties);

            _workContext.Admin = admin;
        }

        /// <summary>
        /// 登出
        /// </summary>
        public async void SignOut()
        {
            //移除缓存
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.User.Identity.IsAuthenticated && !string.IsNullOrWhiteSpace(httpContext.User.Identity.Name))
            {
                Claim adminClaim = httpContext.User.FindFirst(
                         claim => claim.Type == ClaimTypes.Name
                      && claim.Issuer == HBAuthenticationDefaults.ClaimsIssuer);

                if (adminClaim != null)
                {
                    string key = string.Format(HBCachingDefaults.AdminUserNameCacheKey, adminClaim.Value);
                    _cache.Remove(key);
                }

            }
            await httpContext.SignOutAsync(HBAuthenticationDefaults.AdminAuthenticationScheme);
        }

        /// <summary>
        /// 获取身份验证的账号
        /// </summary>
        /// <returns></returns>
        public SysAdmin GetAuthenticatedAdmin()
        {
            return _workContext.Admin;
        }

    }
}
