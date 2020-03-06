using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HB.Api.Configuration;
using HB.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using IdentityModel;
using HB.Api.Models;

namespace HB.Api.Services
{
    public class JwtAuthenticationService : IAuthenticationService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheManagerService _cache;
        private readonly IWorkContextService _workContext;
        private readonly HBConfiguration _hBConfiguration;
        private readonly IMebUserService _mebUserService;


        public JwtAuthenticationService(IHttpContextAccessor httpContextAccessor,
            ICacheManagerService cache,
            IWorkContextService workContext,
             HBConfiguration hBConfiguration,
             IMebUserService mebUserService)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _workContext = workContext;
            _hBConfiguration = hBConfiguration;
            _mebUserService = mebUserService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录的账号</param>
        /// <param name="captcha">验证码</param>
        public string SignIn(MebUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_hBConfiguration.Key);
            var authTime = DateTime.Now;
            var expiresAt = authTime.AddMinutes(_hBConfiguration.ExpiresTime);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Issuer, _hBConfiguration.Issuer),
                    new Claim(JwtClaimTypes.Audience,_hBConfiguration.Audience),
                    new Claim(JwtClaimTypes.Name,user.UserName)
                }),
                Expires = expiresAt,
                //Updated: For HmacSha256Signature, Secret key length should not be less than 128 bits; in other words, it should have at least 16 characters.
                //HmacSha256Signature加密方法秘钥至少128位即16个字符
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            user.AccessToken = tokenString;
            _workContext.User = user;
            return tokenString;
        }

        /// <summary>
        /// 登出
        /// </summary>
        public  void SignOut()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //移除缓存
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.User.Identity.IsAuthenticated )
            {
                var claim = httpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name);

                if (claim != null)
                {
                    string key = string.Format(_hBConfiguration.MebUserCacheKey, claim.Value);
                    _cache.Remove(key);
                }
            }
        }

        /// <summary>
        /// 获取身份验证的账号
        /// </summary>
        /// <returns></returns>
        public MebUser GetAuthenticatedUser()
        {
            return _workContext.User;
        }



    }
}
