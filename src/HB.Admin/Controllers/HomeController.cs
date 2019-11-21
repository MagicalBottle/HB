using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HB.Admin.Attributes;
using HB.Admin.Models;
using Newtonsoft.Json;
using HB.Models;
using HB.Admin.Configuration;

namespace HB.Admin.Controllers
{
    public class HomeController :Controller//: AdminBaseController
    {
        private readonly ISysAdminService _adminService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IWorkContextService _workContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(ISysAdminService adminService,
            IAuthenticationService authenticationService,
            IWorkContextService workContext,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _adminService = adminService;
            _authenticationService = authenticationService;
            _workContext = workContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        public IActionResult Index()
        {
            //只需要登录都能调整到首页，首页里面异步查询数据再限制权限
            var admin = _workContext.Admin;
            return View(admin);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (_httpContextAccessor.HttpContext.User.Claims.Count() > 0)
            {
                //有cookie 删除
                _authenticationService.SignOut();
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] AdminLoginInput adminLoginModel)
        {
            SysAdmin admin = _adminService.GetAdminAllInfoByUserName(adminLoginModel.UserName);
            AdminLoginSuccessOutput loginSuccessModel = new AdminLoginSuccessOutput();
            loginSuccessModel.LoginStatus = LoginStatus.Error;
            if (admin != null && string.Equals( admin.Password, adminLoginModel.Password,StringComparison.InvariantCultureIgnoreCase))
            {
                var r = HttpContext.Request;
                _authenticationService.SignIn(admin, adminLoginModel.IsPersistent);
                loginSuccessModel.LoginStatus = LoginStatus.Success;
                loginSuccessModel.ReturnUrl = adminLoginModel.ReturnUrl;
            }
            string responseData = JsonConvert.SerializeObject(loginSuccessModel);
            return new JsonResult(responseData);
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            _authenticationService.SignOut();
            return RedirectToAction("Login", "Home");
        }

        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return View();
        }
    }

}