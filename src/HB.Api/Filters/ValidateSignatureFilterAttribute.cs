using HB.Api.Configuration;
using HB.Api.Models;
using HB.Api.Services;
using HB.Models;
using HB.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Api.Filters
{
    public class ValidateSignatureFilterAttribute : TypeFilterAttribute
    {
        public ValidateSignatureFilterAttribute() : base(typeof(ValidateParamsFilterImpl))
        {
        }

        private class ValidateParamsFilterImpl : IActionFilter
        {
            //private readonly ILogger _logger;
            private readonly ISysApiUserService _sysApiUserService;
            private readonly HBConfiguration _config;
            private readonly ICacheManagerService _cache;

            private const string SIGN_KEY = "Sign";
            private const string APPID = "Appid";
            private const string NONCE = "Nonce";

            public ValidateParamsFilterImpl(ISysApiUserService sysApiUserService
                , HBConfiguration config
                , ICacheManagerService cache)
            {
                //_logger = logger;
                _sysApiUserService = sysApiUserService;
                _config = config;
                _cache = cache;
            }
            public void OnActionExecuted(ActionExecutedContext context)
            {
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (!string.Equals("post", context.HttpContext.Request.Method, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Result = ApiResult.Error(message: "请使用POST请求");
                    return;
                }

                try
                {
                    #region 验参

                    var param = context.HttpContext.Request.Form;

                    if (!param.ContainsKey(SIGN_KEY) || string.IsNullOrEmpty(param[SIGN_KEY]))
                    {
                        context.Result = ApiResult.Error_Signature(message: SIGN_KEY + "参数不正确");
                        return;
                    }

                    int appid = 0;
                    if (!param.ContainsKey(APPID) || string.IsNullOrEmpty(param[APPID]) || !int.TryParse(param[APPID], out appid))
                    {
                        context.Result = ApiResult.Error_Signature(message: APPID + "参数不正确");
                        return;
                    }
                    long nonce = 0; 
                    if (!param.ContainsKey(NONCE) || string.IsNullOrEmpty(param[NONCE]) || !long.TryParse(param[NONCE],out nonce))
                    {
                        context.Result = ApiResult.Error_Signature(message: NONCE + "参数不正确");
                        return;
                    }

                    if (DateTimeOffset.FromUnixTimeSeconds(nonce).ToLocalTime().AddSeconds(_config.NonceExpiresTime) < DateTimeOffset.Now)
                    {
                        context.Result = ApiResult.Error_Signature(message: "请求超时");
                        return;
                    }


                    #endregion

                    #region user

                    string apiUserCacheKey =string.Format( _config.ApiUserCacheKey,appid);

                    var user = _cache.Get<SysApiUser>(apiUserCacheKey,()=>{
                        return _sysApiUserService.GetUserByAppId(appid);
                    });

                    if (user == null)
                    {
                        context.Result = ApiResult.Error_Signature(message: "Appid用户不存在");
                        return;
                    }

                    #endregion

                    #region Sign
                    var signParams = from p in param.OrderBy(x => x.Key, StringComparer.Ordinal)
                                     where !string.IsNullOrEmpty(p.Value) && p.Key != SIGN_KEY
                                     //orderby p.Key ascending
                                     select p.Key + "=" + p.Value;
                    string mySign = Security.HMACSHA256(String.Join("&", signParams), user.UserKey);
                    string postSign = param[SIGN_KEY];
                    if (!String.Equals(mySign, postSign, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //_logger.LogDebug("Sign Body: {0}, Sign: " + String.Join("&", signParams), mySign);

                        context.Result = ApiResult.Error_Signature(message: "Sign不匹配");
                        return;
                    }
                    #endregion
                }
                catch (Exception e)
                {
                    //_logger.LogError("参数错误， Exception: {0}, PostValue: {1}", e, param.ToString());

                    context.Result = ApiResult.Error_Signature();
                    return;
                }

            }
        }
    }

}
