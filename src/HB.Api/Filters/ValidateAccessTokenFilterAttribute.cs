using HB.Api.Configuration;
using HB.Api.Models;
using HB.Api.Services;
using HB.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HB.Api.Filters
{
    public class ValidateAccessTokenFilterAttribute : TypeFilterAttribute
    {
        public ValidateAccessTokenFilterAttribute() : base(typeof(ValidateAccessTokenFilterImpl))
        {
        }

        private class ValidateAccessTokenFilterImpl : IActionFilter
        {

            private const string ACCESSTOKEN = "AccessToken";
            private readonly HBConfiguration _config;
            private readonly ICacheManagerService _cache;

            public ValidateAccessTokenFilterImpl( HBConfiguration config
                , ICacheManagerService cache)
            {
                _config = config;
                _cache = cache;
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
               
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                //全局的filter 根据自定义的Attribute判定 control 或者action 是否执行 ValidateAccessToken

                #region 
                // https://www.cnblogs.com/hulizhong/p/10779687.html

                //var isDefined = false;
                //var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                //if (controllerActionDescriptor != null)
                //{
                //    isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                //      .Any(a => a.GetType().Equals(typeof(AllowAnonymousAttribute)));
                //}
                //if (isDefined) return;

                #endregion

                if (context.Filters.Any(f => f.GetType().Equals(typeof(AllowAnonymousFilter))))
                {
                    return;
                }

                #region  验证AccessToken
                var param = context.HttpContext.Request.Form;

                if (!param.ContainsKey(ACCESSTOKEN) || string.IsNullOrEmpty(param[ACCESSTOKEN]))
                {
                    context.Result = ApiResult.ErrorAccessToken(message: "参数不正确");
                    return;
                }

                //根据登录的时候生成token(jwt方式），这里校验
                //https://www.cnblogs.com/lechengbo/p/9860711.html

                #region 自定义验证
                var tokenHandler = new JwtSecurityTokenHandler();
                string strToken = param[ACCESSTOKEN];
                if (tokenHandler.CanReadToken(strToken))
                {
                    //var token = tokenHandler.ReadToken(strToken);
                    var token = new JwtSecurityToken() as SecurityToken;
                    var validator = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.Key)),//秘钥
                        ValidateIssuer = true,
                        ValidIssuer = _config.Issuer,
                        ValidateAudience = true,
                        ValidAudience = _config.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(5),
                        RequireExpirationTime=true
                    };

                    try
                    {
                       var claimsPrincipal = tokenHandler.ValidateToken(strToken, validator, out token);

                        var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name);
                        if(claim==null)
                        {
                            context.Result = ApiResult.ErrorAccessToken(JwtClaimTypes.Name+"不存在");
                            return;
                        }
                        var userName = claim.Value;
                        context.HttpContext.User = claimsPrincipal;

                        //查询缓存是否存在
                        string key = string.Format(_config.MebUserCacheKey, userName);
                        var user = _cache.Get<MebUser>(key);
                        if (user == null)
                        {
                            context.Result = ApiResult.ErrorAccessToken(",已失效，请重新登录");
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        context.Result = ApiResult.ErrorAccessToken();
                        return;
                    }
                }
                else
                {
                    context.Result = ApiResult.ErrorAccessToken(",格式不正确");
                    return;
                }

                #endregion

               


                #endregion
            }
        }
    }
}
