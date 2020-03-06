using HB.Api.Filters;
using HB.Api.Models;
using HB.Api.Services;
using HB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Context;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HB.Api.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly  ILogger<AccountController> _logger;
        private readonly IWorkContextService _workContext;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMebUserService _mebUserService;
        public AccountController(IAuthenticationService authenticationService,
            IMebUserService mebUserService,
             IWorkContextService workContext,
             ILogger<AccountController> logger)
        {
            _authenticationService = authenticationService;
            _mebUserService = mebUserService;
            _workContext = workContext;
            _logger = logger;
        }

        [AllowAnonymous]
        public ActionResult<string> Login( [FromBody] UserLoginInput user)
        {
            using (LogContext.PushProperty("LogType", "Order"))
            {
                _logger.LogInformation(user.UserName);
            }
            using (LogContext.PushProperty("LogType", "Api"))
            {
                _logger.LogInformation(user.Password);
            }
            #region 返回值
            Dictionary<string, string> message = new Dictionary<string, string>();
            message.Add("Success", "成功");
            message.Add("Error_Server", "服务器异常，请稍后重试");
            message.Add("Error_Param", "参数校验失败：");
            message.Add("Error_User", "账号或密码错误");
            #endregion
            try
            {
                #region 校验参数
                if (!ModelState.IsValid)
                {

                    var errorProperty = ModelState.Values.First(m => m.ValidationState == ModelValidationState.Invalid);
                    string errorMessage = errorProperty.Errors.First().ErrorMessage;//验证不通过的 //全局配置一个验证不通过就不在验证了，只存在一个错误信息

                    return ApiResult.Error_Param(message: errorMessage);
                }

                #endregion

                #region 执行逻辑

                var model = _mebUserService.GetUserByUserName(user.UserName);
                if (model == null || !string.Equals(model.Password, user.Password, StringComparison.InvariantCultureIgnoreCase))
                {

                    return ApiResult.Error(code: "Error_User", message: message["Error_User"]);
                }
                else
                {
                    string token = _authenticationService.SignIn(model);
                    return ApiResult.Success(new { AccessToken = token,model.UserName,model.Id });
                }
                #endregion
            }
            catch (Exception ex)
            {
                return ApiResult.Error_Server();
            }
        }

        
        public ActionResult<string> GetUser([FromHeader] string AccessToken)
        {
            #region 返回值
            Dictionary<string, string> message = new Dictionary<string, string>();
            message.Add("Success", "成功");
            message.Add("Error_Server", "服务器异常，请稍后重试");
            message.Add("Error_Param", "参数校验失败：");
            message.Add("Error_User", "账号或密码错误");
            #endregion
            try
            {
                #region 校验参数
                if (string.IsNullOrEmpty(AccessToken))
                {
                    return ApiResult.Error_Param(message: message["Error_Param"]);
                }

                #endregion

                #region 执行逻辑

                if (_workContext == null || _workContext.User==null)
                {
                    return ApiResult.Error(code: "Error_User", message: message["Error_User"]);
                }
                else
                {
                    return ApiResult.Success(new { AccessToken = AccessToken, _workContext.User.UserName, _workContext.User.Id });
                }
                #endregion
            }
            catch (Exception ex)
            {
                return ApiResult.Error_Server();
            }
        }

        public ActionResult<string> Logout([FromBody] string AccessToken)
        {
            _authenticationService.SignOut();
            return ApiResult.Success();
        }

        public ActionResult<string> Index()
        {
            return ApiResult.Success();
        }
    }
}