using HB.Admin.Configuration;
using HB.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HB.Admin.Services
{
    public class WorkContextService : IWorkContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISysAdminService _adminService;
        private readonly ICacheManagerService _cache;
        private SysAdmin _cachedAdmin;

        public WorkContextService(IHttpContextAccessor httpContextAccessor,
            ISysAdminService adminService,
            ICacheManagerService cache)
        {
            _httpContextAccessor = httpContextAccessor;
            _adminService = adminService;
            _cache = cache;
        }
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public SysAdmin Admin
        {
            get
            {
                if (_cachedAdmin != null)
                {
                    return _cachedAdmin;
                }


                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null && httpContext.User.Identity.IsAuthenticated && !string.IsNullOrWhiteSpace(httpContext.User.Identity.Name))
                {
                    Claim adminClaim = httpContext.User.FindFirst(
                             claim => claim.Type == ClaimTypes.Name
                          && claim.Issuer == HBAuthenticationDefaults.ClaimsIssuer);

                    if (adminClaim != null)
                    {
                        string key = string.Format(HBCachingDefaults.AdminUserNameCacheKey, adminClaim.Value);
                        _cachedAdmin = _cache.Get<SysAdmin>(key, () =>
                        {
                            return _adminService.GetAdminAllInfoByUserName(adminClaim.Value);
                        });
                    }
                    return _cachedAdmin;
                }
                return null;

            }
            set
            {
                if (value != null)
                {
                    //值不为空的时候，缓存
                    string key = string.Format(HBCachingDefaults.AdminUserNameCacheKey, value.UserName);
                    _cache.Set<SysAdmin>(key, value);
                }
                _cachedAdmin = value;
            }
        }

        public HttpContext HttpContext
        {
            get
            {
                return _httpContextAccessor.HttpContext;
            }
        }
    }
}
