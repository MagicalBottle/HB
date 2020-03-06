using HB.Api.Configuration;
using HB.Models;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HB.Api.Services
{

    public class WorkContextService : IWorkContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly ISysAdminService _adminService;
        private readonly ICacheManagerService _cache;
        private MebUser _cachedUser;
        private readonly HBConfiguration _hBConfiguration;

        public WorkContextService(IHttpContextAccessor httpContextAccessor,
            ICacheManagerService cache,
            HBConfiguration hBConfiguration)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _hBConfiguration = hBConfiguration;
        }
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public MebUser User
        {
            get
            {
                if (_cachedUser != null)
                {
                    return _cachedUser;
                }


                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null && httpContext.User.Identity.IsAuthenticated )
                {
                    //var claim = httpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name); 
                    var claim = httpContext.User.Claims.FirstOrDefault(c => c.Type == "AccessToken"); 

                    if (claim != null)
                    {
                        string key = string.Format(_hBConfiguration.MebUserCacheKey, claim.Value);
                        _cachedUser = _cache.Get<MebUser>(key);
                    }
                    return _cachedUser;
                }
                return null;

            }
            set
            {
                if (value != null)
                {
                    //值不为空的时候，缓存
                    string key = string.Format(_hBConfiguration.MebUserCacheKey, value.AccessToken);
                    _cache.Set<MebUser>(key, value);
                }
                _cachedUser = value;
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
