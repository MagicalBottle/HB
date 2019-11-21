using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB.Admin.Attributes;
using HB.Admin.Configuration;
using HB.Admin.Models;
using HB.Admin.Services;
using HB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using AutoMapper;

namespace HB.Admin.Controllers
{

    public class AccountController : AdminBaseController
    {
        private readonly ISysAdminService _adminService;
        private readonly IMenuService _menuService;
        private readonly IWorkContextService _context;
        private readonly IMapper _mapper;
        public AccountController(ISysAdminService adminService,
            IWorkContextService context,
           IMapper mapper,
           IMenuService menuService)
        {
            _adminService = adminService;
            _context = context;
            _mapper = mapper;
            _menuService = menuService;
        }

        [AdminAuthorize(Policy = HBPermissionKeys.AdminView)]
        public IActionResult Index()
        {
            return View();
        }

        [AdminAuthorize(Policy = HBPermissionKeys.AdminView)]
        public IActionResult List(AdminQueryParamInput param)
        {
            PagedList<SysAdmin> admins = null;
            var result = new PagedListReponseOutPut<SysAdmin>();
            try
            {
                admins = _adminService.GetAdmins(param);
                result.Rows = admins;
            }
            catch (Exception ex)
            {
                result.Status = ReutnStatus.Error;
                result.Message = "Error";
            }
            return new JsonResult(JsonConvert.SerializeObject(result, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffff" }));
        }


        [AdminAuthorize(Policy = HBPermissionKeys.AdminAdd)]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [AdminAuthorize(Policy = HBPermissionKeys.AdminAdd)]
        [HttpPost]
        public IActionResult Add(AdminInput param)
        {
            var response = new ReponseOutPut();
            response.Code = "menu_add_success";
            response.Message = "新增账号成功";
            if (!ModelState.IsValid)
            {
                response.Status = ReutnStatus.Error;
                response.Code = "param_vaild_error";

                var errorProperty = ModelState.Values.First(m => m.ValidationState == ModelValidationState.Invalid);
                response.Message = errorProperty.Errors.First().ErrorMessage;//验证不通过的 //全局配置一个验证不通过就不在验证了，只存在一个错误信息

                return new JsonResult(JsonConvert.SerializeObject(response));
            }

            // 检查用户名是否重复
            var isExistUserName = _adminService.ExistAdminUserName(param.UserName);
            if (isExistUserName)
            {
                response.Status = ReutnStatus.Error;
                response.Code = "username_is_exist";
                response.Message = "用户名已经存在";
                return new JsonResult(JsonConvert.SerializeObject(response));
            }

            SysAdmin admin = _mapper.Map<AdminInput, SysAdmin>(param);

            admin.CreateBy = _context.Admin.Id;
            admin.CreatebyName = _context.Admin.UserName;
            admin.CreateDate = DateTime.Now;
            admin.LastUpdateBy = admin.CreateBy;
            admin.LastUpdateByName = admin.CreatebyName;
            admin.LastUpdateDate = admin.CreateDate;



            var result = _adminService.AddAdmin(admin, param.RoleIds);
            if (result < 0)
            {
                response.Status = ReutnStatus.Error;
                response.Code = "menu_add_error";
                response.Message = "新增账号失败";
            }

            return new JsonResult(JsonConvert.SerializeObject(response));
        }

        [AdminAuthorize(Policy = HBPermissionKeys.AdminEdit)]
        public IActionResult Edit(int id)
        {
            var admin = _adminService.GetAdminWithRoles(id);
            return View(admin);
        }

        [AdminAuthorize(Policy = HBPermissionKeys.AdminEdit)]
        [HttpPost]
        public IActionResult Edit(AdminInput param)
        {
            var response = new ReponseOutPut();
            response.Code = "account_edit_success";
            response.Message = "新增账号成功";
            if (!ModelState.IsValid)
            {
                response.Status = ReutnStatus.Error;
                response.Code = "param_vaild_error";

                var errorProperty = ModelState.Values.First(m => m.ValidationState == ModelValidationState.Invalid);
                response.Message = errorProperty.Errors.First().ErrorMessage;//验证不通过的 //全局配置一个验证不通过就不在验证了，只存在一个错误信息

                return new JsonResult(JsonConvert.SerializeObject(response));
            }

            // 检查用户名是否重复
            var isExistUserName = _adminService.ExistAdminUserName(param.UserName, param.Id);
            if (isExistUserName)
            {
                response.Status = ReutnStatus.Error;
                response.Code = "username_is_exist";
                response.Message = "用户名已经存在";
                return new JsonResult(JsonConvert.SerializeObject(response));
            }

            SysAdmin admin = _mapper.Map<AdminInput, SysAdmin>(param);

            admin.LastUpdateBy = _context.Admin.Id;
            admin.LastUpdateByName = _context.Admin.UserName;
            admin.LastUpdateDate = DateTime.Now;

            var result = _adminService.UpdateAdmin(admin, param.RoleIds);
            if (result < 0)
            {
                response.Status = ReutnStatus.Error;
                response.Code = "account_edit_error";
                response.Message = "更新账号失败";
            }

            return new JsonResult(JsonConvert.SerializeObject(response));
        }

        [AdminAuthorize(Policy = HBPermissionKeys.AdminView)]
        public IActionResult GetAllAdmins()
        {
            List<SysAdmin> admins = null;
            var result = new ListReponseOutPut<SelectOutPut>();
            try
            {
                admins = _adminService.GetAll().ToList();

                List<SelectOutPut> rows = _mapper.Map<List<SysAdmin>, List<SelectOutPut>>(admins);
                result.Rows = rows;
            }
            catch (Exception ex)
            {
                result.Status = ReutnStatus.Error;
                result.Code = "get_data_error";
                result.Message = "Error";
            }
            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        [AdminAuthorize(Policy = HBPermissionKeys.AdminDelete)]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var response = new ReponseOutPut();
            var result = _adminService.DeleteAdmin(id);
            if (result<0)
            {
                response.Status = ReutnStatus.Error;
                response.Code = "error";
                response.Message = "删除账号失败";
            }

            return new JsonResult(JsonConvert.SerializeObject(response));
        }

        [AdminAuthorize(Policy = HBPermissionKeys.AdminPermission)]
        public IActionResult Permission(int id)
        {
            List<SysMenu> model = _menuService.GetMenusByAdminId(id);
            return View(model);
        }

    }
}